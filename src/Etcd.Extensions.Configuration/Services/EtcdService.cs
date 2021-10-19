using dotnet_etcd;
using Etcd.Services.Abstract;
using Etcdserverpb;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Etcd.Services
{
	internal class EtcdService : IEtcdService
	{
		private readonly EtcdClient _client;
		private readonly EtcdServiceOptions _options;

		public EtcdService(EtcdServiceOptions options)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_client = new EtcdClient(
				_options.ConnectionString,
				_options.Port,
				_options.CaCert,
				_options.ClientCert,
				_options.ClientKey,
				_options.PublicRootCa);
		}

		private Metadata? Metadata { get; set; }

		public IReadOnlyDictionary<string, string> GetKeyValues()
		{
			if (!TryLogin())
			{
				var kvs = _client.GetRange("")?.Kvs;
				return kvs == null
					? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
					: kvs.ToDictionary(x => x.Key.ToStringUtf8(), x => x.Value.ToStringUtf8());
			}

			var user = _client.UserGet(new AuthUserGetRequest
			{
				Name = _options.UserName
			}, Metadata);

			if (user?.Roles == null || !user.Roles.Any()) throw new InvalidOperationException("User hasn't roles");

			var permissions = user.Roles
				.Select(x => _client.RoleGet(new AuthRoleGetRequest
				{
					Role = x
				}, Metadata))
				.SelectMany(x => x?.Perm)
				.Where(x => x != null)
				.ToList();

			var keyValues = permissions
				.Select(permission => _client.GetRange(permission.Key.ToStringUtf8(), Metadata)?.Kvs)
				.Where(keyValues => keyValues != null)
				.SelectMany(x => x)
				.ToList();

			return keyValues.ToDictionary(x => x.Key.ToStringUtf8(), x => x.Value.ToStringUtf8());
		}

		private bool TryLogin()
		{
			if (!_options.HasCredentials()) return false;

			var authenticateResponse = _client.Authenticate(new AuthenticateRequest
			{
				Name = _options.UserName,
				Password = _options.Password
			});

			Metadata = new Metadata
			{
				new Metadata.Entry("token", authenticateResponse.Token)
			};

			return true;
		}

		public void Dispose()
		{
			_client.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}