using Etcd.Services;
using Etcd.Services.Abstract;
using Microsoft.Extensions.Configuration;
using System;

namespace Etcd
{
	/// <summary>
	///     Extension methods for adding <see cref="EtcdConfigurationProvider"/>.
	/// </summary>
	public static class EtcdConfigurationBuilderExtensions
	{
		/// <summary>
		///     Adds the etcd configuration provider to <paramref name="builder"/>.
		/// </summary>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
		/// <param name="connectionString">The etcd connection string.</param>
		/// <param name="port">The etcd port.</param>
		/// <param name="configureOptions">Configure the options.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
		public static IConfigurationBuilder AddEtcd(
			this IConfigurationBuilder builder,
			string connectionString,
			int port = 2379,
			Action<EtcdOptions>? configureOptions = null)
			=> builder.AddEtcd(connectionString, port, clientOptions => { }, configureOptions);

		/// <summary>
		///     Adds the etcd configuration provider to <paramref name="builder"/>.
		/// </summary>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
		/// <param name="connectionString">The etcd connection string.</param>
		/// <param name="port">The etcd port.</param>
		/// <param name="configureServiceOptions">Configure the service options.</param>
		/// <param name="configureOptions">Configure the options.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
		public static IConfigurationBuilder AddEtcd(
			this IConfigurationBuilder builder,
			string connectionString,
			int port = 2379,
			Action<EtcdServiceOptions>? configureServiceOptions = null,
			Action<EtcdOptions>? configureOptions = null)
		{
			if (connectionString == null)
			{
				throw new ArgumentNullException(nameof(connectionString));
			}

			var etcdServiceOptions = new EtcdServiceOptions(connectionString, port);
			configureServiceOptions?.Invoke(etcdServiceOptions);

			return builder.AddEtcd(new EtcdService(etcdServiceOptions), configureOptions);
		}

		/// <summary>
		///     Adds the etcd configuration provider to <paramref name="builder"/>.
		/// </summary>
		/// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
		/// <param name="etcdService">The implementation of etcd service.</param>
		/// <param name="configureOptions">Configure the options.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
		public static IConfigurationBuilder AddEtcd(
			this IConfigurationBuilder builder,
			IEtcdService etcdService,
			Action<EtcdOptions>? configureOptions = null)
		{
			if (etcdService == null)
			{
				throw new ArgumentNullException(nameof(etcdService));
			}

			var options = new EtcdOptions();
			configureOptions?.Invoke(options);

			return builder.Add(new EtcdConfigurationSource(etcdService, options));
		}
	}
}
