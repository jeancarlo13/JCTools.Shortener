using System;
using JCTools.Shortener.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JCTools.Shortener.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JCTools.Shortener
{
    /// <summary>
    /// Add the available extensors methods for the configuration and initialization of the package 
    /// </summary>
    public static class StartupExtensors
    {
        /// <summary>
        /// The options to be use for the generation short link process
        /// </summary>
        internal static Options Options;

        /// <summary>
        /// Allows registry the required services for the correctly functioning of the package  
        /// </summary>
        /// <typeparam name="TDatabaseContext">The type of the database context to be used for stored the generated links</typeparam>
        /// <param name="services">The application services collection to be used for the services register</param>
        /// <param name="optionsFactory">Action to be invoke for get the configured options of the package</param>
        /// <returns>The modified app services collection</returns>
        public static IServiceCollection AddLinksShortener<TDatabaseContext>(
            this IServiceCollection services,
            Action<Options> optionsFactory = null
        )
            where TDatabaseContext : DbContext, IDatabaseContext
        {
            Options = new Options();
            optionsFactory?.Invoke(Options);

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<ILinkGenerator, LinkGenerator<TDatabaseContext>>();
            services.AddTransient<IDatabaseContext, TDatabaseContext>();

            services.AddAuthorization(o =>
            {
                o.AddPolicy(Options.PolicyName, Options.ConfigurePolicy);
            });

            return services;
        }
    }
}
