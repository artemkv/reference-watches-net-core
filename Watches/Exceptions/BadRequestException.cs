using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Watches.Exceptions
{
    /// <summary>
    /// Is thrown when the request could not be completed because the information submitted by the client was incorrect.
    /// The client SHOULD NOT repeat the request without modifications.
    /// </summary>
	[Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException() : base()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BadRequestException(string message, string key) : base(message)
        {
            Key = key;
        }

        public BadRequestException(string message, string key, Exception innerException)
            : base(message, innerException)
        {
            Key = key;
        }

        public string Key { get; set; }
    }
}
