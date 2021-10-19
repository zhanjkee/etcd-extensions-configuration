using Etcd.Services.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Etcd
{
	/// <summary>
	///		An etcd based <see cref="ConfigurationProvider"/>.
	/// </summary>
	public class EtcdConfigurationProvider : ConfigurationProvider, IDisposable
	{
		private readonly IEtcdService _etcdService;
		private readonly EtcdOptions _options;

		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="etcdService">The etcd service.</param>
		/// <param name="options">The etcd options.</param>
		/// <exception cref="ArgumentNullException">Arguments cannot be null.</exception>
		public EtcdConfigurationProvider(IEtcdService etcdService, EtcdOptions options)
		{
			_etcdService = etcdService ?? throw new ArgumentNullException(nameof(etcdService));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_etcdService.Dispose();
			GC.SuppressFinalize(this);
		}

		/// <inheritdoc />
		public override void Load()
		{
			IReadOnlyDictionary<string, string>? data;

			try
			{
				data = _etcdService.GetKeyValues();
			}
			catch (Exception e)
			{
				_options.InvokeOnException(e);
				return;
			}

			var prefixes = _options.GetPrefixesWithDelimiter();
			foreach (var item in data)
			{
				if (prefixes.Any())
				{
					if (_options.RemovePrefixes)
					{
						var key = prefixes.Aggregate(item.Key, (current, prefix) =>
							current.StartsWith(prefix)
								? current.Replace(prefix, string.Empty)
								: current);

						Data.Add(key, item.Value);
						continue;
					}
				}

				Data.Add(item.Key, item.Value);
			}
		}
	}
}