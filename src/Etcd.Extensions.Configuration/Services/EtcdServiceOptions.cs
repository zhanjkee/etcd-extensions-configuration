using System;

namespace Etcd.Services
{
	/// <summary>
	///		The etcd service options.
	/// </summary>
	public class EtcdServiceOptions
	{
		/// <summary>
		///		Constructor.
		/// </summary>
		/// <param name="connectionString">The etcd connection string.</param>
		/// <param name="port">The etcd port.</param>
		public EtcdServiceOptions(string connectionString, int port = 2379)
		{
			ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
			Port = port;
		}

		/// <summary>
		///		Gets the etcd <see cref="ConnectionString"/>.
		/// </summary>
		public string ConnectionString { get; }

		/// <summary>
		///		Gets the etcd <see cref="Port"/>.
		/// </summary>
		public int Port { get; }

		/// <summary>
		///		Gets or sets the <see cref="CaCert"/>.
		/// </summary>
		public string CaCert { get; set; } = string.Empty;

		/// <summary>		
		///		Gets or sets the <see cref="ClientCert"/>.
		/// </summary>
		public string ClientCert { get; set; } = string.Empty;

		/// <summary>
		///		Gets or sets the <see cref="ClientKey"/>.
		/// </summary>
		public string ClientKey { get; set; } = string.Empty;

		/// <summary>
		///		Gets or sets the <see cref="PublicRootCa"/>.
		/// </summary>
		public bool PublicRootCa { get; set; } = false;

		/// <summary>
		///		Gets or sets the <see cref="UserName"/>.
		/// </summary>
		public string? UserName { get; set; }

		/// <summary>
		///		Gets or sets the <see cref="Password"/>.
		/// </summary>
		public string? Password { get; set; }

		internal bool HasCredentials()
		{
			return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
		}
	}
}