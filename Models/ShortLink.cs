using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCTools.Shortener.Models
{
    [Table("Short_Link")]
    public class ShortLink
    {
        /// <summary>
        /// The random unique token to be used 
        /// </summary>
        [Key]
        public string Token { get; set; }
        /// <summary>
        /// the url was shorted
        /// </summary>
        public string RelatedUrl { get; set; }
    }
}