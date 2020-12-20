using System;
using System.Linq;
using System.Threading.Tasks;
using JCTools.Shortener.Models;
using JCTools.Shortener.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;


namespace JCTools.Shortener.Services
{
    internal class LinkGenerator<TDatabaseContext> : ILinkGenerator
        where TDatabaseContext : DbContext, IDatabaseContext
    {
        /// <summary>
        /// the database context to be use
        /// </summary>
        private readonly TDatabaseContext _context;
        private readonly IUrlHelper _urlHelper;
        public LinkGenerator(
            TDatabaseContext context,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
        )
        {
            this._context = context;
            this._urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        /// <summary>
        /// Generate a random unique string token, with a configured length.
        /// </summary>
        /// <returns>The generate token</returns>
        public async Task<string> GenerateTokenAsync()
        {
            var random = new Random();
            var token = string.Empty;
            var isValid = false;

            while (!isValid)
            {
                var chars = Enumerable.Range(0, StartupExtensors.Options.ValidCharacters.Length - 1)
                  .OrderBy(o => random.Next())
                  .Select(i => StartupExtensors.Options.ValidCharacters[i])
                  .ToList();

                token = string.Join("", chars);
                var length = random.Next(StartupExtensors.Options.MinLength, StartupExtensors.Options.MaxLength);
                var start = random.Next(0, token.Length - length - 1);

                token = token.Substring(start, length);

                isValid = !await _context.ShortLinks.AnyAsync(sl => sl.Token == token);
            }

            return token;
        }

        /// <summary>
        /// Generate a short url with an random unique token
        /// </summary>
        /// <param name="relatedTo">the related url to be short</param>
        /// <returns>The generate shorted link</returns>
        public async Task<ShortLink> GenerateAsync(string relatedTo)
        {
            var tokenEntry = new ShortLink()
            {
                Token = await GenerateTokenAsync(),
                RelatedUrl = relatedTo
            };

            return tokenEntry;
        }

        /// <summary>
        /// Generate and save a short link with a random token
        /// </summary>
        /// <param name="relatedTo">the related url to be short</param>
        /// <returns>The task to be execute</returns>
        public async Task<ShortLink> GenerateAndSaveAsync(string relatedTo)
        {
            var link = await GenerateAsync(relatedTo);
            await _context.AddAsync(link);
            await _context.SaveChangesAsync();
            return link;
        }
        /// <summary>
        /// Allows get the absolute Url for the specified short link
        /// </summary>
        /// <param name="link">The short link to be use for generate the absolute Url</param>
        /// <returns>The generated absolute url</returns>
        public string GetAbsoluteUrl(ShortLink link) => GetAbsoluteUrl(link.Token);

        /// <summary>
        /// Allows get the absolute Url for the specified toke string
        /// </summary>
        /// <param name="token">The toke to be use for generate the absolute Url</param>
        /// <returns>The generated absolute url</returns>
        public string GetAbsoluteUrl(string token)
        {
            return _urlHelper.RouteUrl(
                Controllers.ShortenerController.redirectToRouteName,
                new { token = token },
                this._urlHelper.ActionContext.HttpContext.Request.Scheme
            );
        }
    }
}