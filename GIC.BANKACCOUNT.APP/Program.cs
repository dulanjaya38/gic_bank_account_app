// See https://aka.ms/new-console-template for more information

using Autofac;
using Autofac.Extensions.DependencyInjection;
using GIC.BANKACCOUNT.DATA.Entities;
using GIC.BANKACCOUNT.DATA.Repositories.Interfaces;
using GIC.BANKACCOUNT.SERVICES.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace GIC.BANKACCOUNT.APP
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            IConfiguration configuration = configBuilder.Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).Enrich.FromLogContext().CreateLogger();


            var builder = Host.CreateDefaultBuilder()
                              .ConfigureServices(
                                    services =>
                                    {
                                        services.AddDbContext<AppDbContext>(options =>
                                        {
                                            options.UseSqlServer(configuration.GetConnectionString("AppDbConnectionString"));
                                        })
                                        .AddTransient<App>();
                                    })
                             .ConfigureLogging(logging =>
                             {
                                 logging.ClearProviders().AddSerilog();
                             })
                             .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                                 .ConfigureContainer<ContainerBuilder>(builder =>
                                 {
                                     builder.RegisterAssemblyTypes(typeof(IBaseRepository).Assembly)
                                            .Where(x => x.IsClass && !x.IsAbstract && typeof(IBaseRepository).IsAssignableFrom(x))
                                            .AsImplementedInterfaces()
                                            .InstancePerLifetimeScope();

                                     builder.RegisterAssemblyTypes(typeof(IBaseService).Assembly)
                                            .Where(x => x.IsClass && !x.IsAbstract && typeof(IBaseService).IsAssignableFrom(x))
                                            .AsImplementedInterfaces()
                                            .InstancePerLifetimeScope();
                                 })
                             .UseConsoleLifetime()
                             .Build();

            await ExicuteApp(builder, true);
        }

        private static async Task ExicuteApp(IHost builder, bool isFirstLogin)
        {
            bool returnToMain = true;

            while (returnToMain)
            {
                using var serviceScope = builder.Services.CreateScope();

                var services = serviceScope.ServiceProvider;
                var myService = services.GetRequiredService<App>();
                returnToMain = await myService.Run(isFirstLogin);
                isFirstLogin = false;
            }

            Console.ReadKey();
        }
    }
}