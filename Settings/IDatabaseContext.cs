using JCTools.Shortener.Models;
using Microsoft.EntityFrameworkCore;

namespace JCTools.Shortener.Settings
{
    /// <summary>
    /// Allows acccess to the database of the third-part application
    /// </summary>
    public interface IDatabaseContext
    {
        /// <summary>
        /// The collection of the generated short links 
        /// </summary>
        DbSet<ShortLink> ShortLinks { get; set; }

    }
}