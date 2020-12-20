using System;
using Microsoft.AspNetCore.Authorization;

namespace JCTools.Shortener.Settings
{
    /// <summary>
    /// Defines all the package settings that are used to suit the application where it will be used
    /// </summary>
    public class Options
    {
        /// <summary>
        /// The police name used for add security to the controller access
        /// </summary>
        public const string PolicyName = "JCTools.Shortener.Policy";

        /// <summary>
        /// String with the collection of allowed characters 
        /// to be use for the generation fo the short links.
        /// The default characters are "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
        /// Using the full English alphabet plus all numerals from 0-9 that gives us 62 available characters, meaning we have: 
        /// (62^2) + (62^3) + (62^4) + (62^5) + (62^6) possible unique tokens which equals: `57 billion 731 million 386 thousand 924´.
        /// </summary>
        public string ValidCharacters { get; set; } = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// The minimum length (default = 2) of the generated random unique token.
        /// </summary>
        private int _minLength = 2;
        /// <summary>
        /// The minimum length (default = 2) of the generated random unique token.
        /// </summary>
        public int MinLength
        {
            get => _minLength;
            set
            {
                if (value >= MaxLength)
                    throw new ArgumentException($"The {nameof(MinLength)} must be less that the {nameof(MaxLength)}. {value} < {MaxLength}?");
                else
                    _minLength = value;
            }
        }
        /// <summary>
        /// The maximum length (default = 6) of the generated random unique token.
        /// </summary>
        private int _maxLength = 6;
        /// <summary>
        /// The maximum length (default = 6) of the generated random unique token.
        /// </summary>
        public int MaxLength
        {
            get => _maxLength;
            set
            {
                if (value <= MinLength)
                    throw new ArgumentException($"The {nameof(MaxLength)} must be greater that the {nameof(MinLength)}. {value} > {MinLength}?");
                else
                    _maxLength = value;
            }
        }
        /// <summary>
        /// Use this property for manage the authorization policy according to you needs.
        /// By default, it's configured with the anonymous access, and the redirection url can implement the adequate authorization.
        /// </summary>
        public Action<AuthorizationPolicyBuilder> ConfigurePolicy { get; set; } = p => p.RequireAssertion(a => true);
    }
}