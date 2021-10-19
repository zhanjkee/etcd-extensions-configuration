using System;
using System.Collections.Generic;

namespace Etcd.Services.Abstract
{
	/// <summary>
	///		The etcd service.
	/// </summary>
	public interface IEtcdService : IDisposable
	{
		/// <summary>
		///		Get all keys from etcd.
		/// </summary>
		/// <returns>The key value dictionary.</returns>
		IReadOnlyDictionary<string, string> GetKeyValues();
	}
}
