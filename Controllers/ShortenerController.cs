using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JCTools.Shortener.Models;
using JCTools.Shortener.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JCTools.Shortener.Controllers
{
    [Authorize(Settings.Options.PolicyName)]
    public class ShortenerController<TDatabaseContext> : Controller
        where TDatabaseContext : DbContext, IDatabaseContext
    {
        /// <summary>
        /// The logger instance to be use for log the application messages
        /// </summary>
        private readonly ILogger<ShortenerController<TDatabaseContext>> _logger;
        /// <summary>
        /// the database context instance to be use
        /// </summary>
        private readonly TDatabaseContext _context;

        public ShortenerController(
            ILogger<ShortenerController<TDatabaseContext>> logger,
            TDatabaseContext context
        )
        {
            _logger = logger;
            this._context = context;
        }

        /// <summary>
        /// Valid and redirect from a shorted link to the related url
        /// </summary>
        /// <param name="token">The token to be use for the redirection</param>
        /// <returns>The task to be execute</returns>
        [HttpGet]
        [Route("/{token}")]
        public async Task<IActionResult> RedirectTo(string token)
        {
            var link = await _context.ShortLinks.FirstOrDefaultAsync(sl => sl.Token == token);
            if (link == null)
                return NotFound();

            return Redirect(link.RelatedUrl);
        }
    }
}
