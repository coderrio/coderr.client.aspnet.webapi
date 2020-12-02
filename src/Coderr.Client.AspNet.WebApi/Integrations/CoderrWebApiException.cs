using System;

namespace Coderr.Client.AspNet.WebApi.Integrations
{
    /// <summary>
    ///     Internal exception to be able to report errors.
    /// </summary>
    internal class CoderrWebApiException : Exception
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="CoderrWebApiException" /> class.
        /// </summary>
        /// <param name="message">Error message</param>
        public CoderrWebApiException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Generate an exception (try/catches internally)
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns>exception</returns>
        public static CoderrWebApiException Generate(string message)
        {
            try
            {
                throw new CoderrWebApiException(message);
            }
            catch (CoderrWebApiException e)
            {
                return e;
            }
        }
    }
}