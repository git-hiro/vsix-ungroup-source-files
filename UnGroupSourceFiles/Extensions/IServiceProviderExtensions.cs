using System;

namespace Toybox.UnGroupSourceFiles.Extensions
{
	/// <summary>
	/// サービスプロバイダ拡張
	/// </summary>
	internal static class IServiceProviderExtensions
	{
		/// <summary>
		/// サービスの取得
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public static T GetService<T>(this IServiceProvider serviceProvider)
		{
			return (T)serviceProvider.GetService(typeof(T));
		}

		/// <summary>
		/// サービスの取得
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public static T2 GetService<T1, T2>(this IServiceProvider serviceProvider)
		{
			return (T2)serviceProvider.GetService(typeof(T1));
		}
	}
}