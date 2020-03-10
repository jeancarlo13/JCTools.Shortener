using System.Threading.Tasks;
using JCTools.Shortener.Models;

namespace JCTools.Shortener.Services
{
    public interface ILinkGenerator
    {

        /// <summary>
        /// Generate and save a short link with a random token
        /// </summary>
        /// <param name="relatedTo">the related url to be short</param>
        /// <returns>The task to be execute</returns>
        Task<ShortLink> GenerateAndSaveAsync(string relatedTo);

        /// <summary>
        /// Generate a short url with an random unique token
        /// </summary>
        /// <param name="relatedTo">the related url to be short</param>
        /// <returns>The generate shorted link</returns>
        Task<ShortLink> GenerateAsync(string relatedTo);
        /// <summary>
        /// Generate a random unique string token, with a configured length.
        /// </summary>
        /// <returns>The generate token</returns>
        Task<string> GenerateTokenAsync();
        /// <summary>
        /// Allows get the absolute Url for the specified short link
        /// </summary>
        /// <param name="link">The short link to be use for generate the absolute Url</param>
        /// <returns>The generated absolute url</returns>
        string GetAbsoluteUrl(ShortLink link);
        /// <summary>
        /// Allows get the absolute Url for the specified toke string
        /// </summary>
        /// <param name="token">The toke to be use for generate the absolute Url</param>
        /// <returns>The generated absolute url</returns>
        string GetAbsoluteUrl(string token);
    }
}