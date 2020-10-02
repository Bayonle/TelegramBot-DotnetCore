using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zedvance.telegrambot.api
{
    class ConsoleDIFactory {
        private static ServiceCollection services;

        public static IServiceProvider Services { get; private set; }
        public static IConfiguration Configuration { get; set; }
        public static void ConfigureServices (IConfiguration Configuration) {
            //

            IServiceCollection services = new ServiceCollection ();

            // build configuration
            services.AddOptions ();
            services.AddLogging ();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // services.AddTransient<App> ();

            Services = services.BuildServiceProvider ();

        }

    }

}