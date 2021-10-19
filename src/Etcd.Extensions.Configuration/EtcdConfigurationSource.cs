using Etcd.Services.Abstract;
using Microsoft.Extensions.Configuration;
using System;

namespace Etcd
{
	/// <summary>
	///     Represents an etcd as an <see cref="IConfigurationSource"/>.
	/// </summary>
	public class EtcdConfigurationSource : IConfigurationSource
	{
		private readonly IEtcdService _etcdService;
		private readonly EtcdOptions _options;

		/// <summary>
		///     Constructor.
		/// </summary>
		/// <param name="etcdService">The etcd service.</param>
		/// <param name="options">The options.</param>
		/// <exception cref="ArgumentNullException">Arguments cannot be null.</exception>
		public EtcdConfigurationSource(IEtcdService etcdService, EtcdOptions options)
		{
			_etcdService = etcdService ?? throw new ArgumentNullException(nameof(etcdService));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		/// <inheritdoc />
		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new EtcdConfigurationProvider(_etcdService, _options);
		}
	}
}
