using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Etcd.Sample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(config =>
				{
					config.AddEtcd("https://test-intelsoft.com/infrastructure/etcd", 2381,
						configureServiceOptions: serviceOptions =>
						{
							serviceOptions.UserName = "UserName";
							serviceOptions.Password = "Password";
						},
						configureOptions: options =>
						{
							options.Prefixes = new[] { "AppName" };
							options.RemovePrefixes = true;
						});
				})
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
	}
}