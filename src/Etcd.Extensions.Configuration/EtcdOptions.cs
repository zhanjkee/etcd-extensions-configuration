using System;
using System.Collections.Generic;
using System.Linq;

namespace Etcd
{
	/// <summary>
	///     The etcd options.
	/// </summary>
	public class EtcdOptions
	{
		/// <summary>
		///     The exception handler delegate.
		/// </summary>
		public delegate void ExceptionHandler(Exception ex);

		/// <summary>
		///     The etcd prefixes.
		/// </summary>
		public IReadOnlyCollection<string> Prefixes { get; set; } = Array.Empty<string>();

		/// <summary>
		///     The etcd prefix delimiter.
		/// </summary>
		/// <remarks>Default delimiter is :</remarks>
		public string PrefixDelimiter { get; set; } = ":";

		/// <summary>
		///     The flag indicates that it removes all prefixes when loading keys from etcd.
		/// </summary>
		public bool RemovePrefixes { get; set; } = true;

		/// <summary>
		///     The excpetion handler event.
		/// </summary>
		public event ExceptionHandler? OnException;

		internal IReadOnlyCollection<string> GetPrefixesWithDelimiter()
		{
			if (!Prefixes.Any())
			{
				return Array.Empty<string>();
			}

			return Prefixes.Select(prefix => string.Concat(prefix, PrefixDelimiter)).ToList();
		}

		internal void InvokeOnException(Exception ex)
		{
			OnException?.Invoke(ex);
		}
	}
}