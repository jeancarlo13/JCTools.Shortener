using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JCTools.Shortener.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JCTools.Shortener.Controllers
{
    [Authorize(Settings.Options.PolicyName)]
    public class ShortenerController : Controller
    {
        /// <summary>
        /// The name of the route template to be use by the <see cref="RedirectTo"> action
        /// </summary>
        internal const string redirectToRouteName = "ShortenerController_RedirectTo";

        /// <summary>
        /// The logger instance to be use for log the application messages
        /// </summary>
        private readonly ILogger<ShortenerController> _logger;
        /// <summary>
        /// The database context instance to be use
        /// </summary>
        private readonly IDatabaseContext _context;
        /// <summary>
        /// Initialize the controller instances
        /// </summary>
        /// <param name="loggerFactory">The logger factory instance to be used for log creation</param>
        /// <param name="context">The database context instance to be use/param>
        public ShortenerController(
            ILoggerFactory loggerFactory,
            IDatabaseContext context
        )
        {
            _logger = loggerFactory.CreateLogger<ShortenerController>();
            _context = context;
            if (context.GetType().IsAssignableFrom(typeof(DbContext)))
                throw new ArgumentException($"The argument {nameof(context)} not implement {typeof(DbContext).FullName}.", nameof(context));
        }

        /// <summary>
        /// Valid and redirect from a shorted link to the related url
        /// </summary>
        /// <param name="token">The token to be use for the redirection</param>
        /// <returns>The task to be execute</returns>
        [HttpGet]
        [Route("/{token}", Order = int.MaxValue, Name = redirectToRouteName)]
        public async Task<IActionResult> RedirectTo(string token)
        {
            var link = await _context.ShortLinks.FirstOrDefaultAsync(sl => sl.Token == token);
            if (link == null)
            {
                _logger.LogWarning($"Attemp of access with an invalid token: {token}");
                return NotFound();
            }
            return Redirect(link.RelatedUrl);
        }
    }
}
