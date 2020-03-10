using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JCTools.Shortener.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JCTools.Shortener.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JCTools.Shortener
{
    public static class StartupExtensors
    {
        /// <summary>
        /// The options to be use for the generation short link process
        /// </summary>
        internal static Options Options;
        public static IServiceCollection AddLinksShortener<TDatabaseContext>(
            this IServiceCollection services,
            Action<Options> optionsActions
        )
            where TDatabaseContext : DbContext, IDatabaseContext
        {
            Options = new Options();
            optionsActions?.Invoke(Options);

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<ILinkGenerator, LinkGenerator<TDatabaseContext>>();

            services.AddAuthorization(o =>
            {
                o.AddPolicy(Options.PolicyName, Options.ConfigurePolicy);
            });

            return services;
        }
    }
}
