using System;

namespace ServiceClient
{
    /// <summary>
    /// an exception thrown by the service client
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceClient.ServiceException"/> class.
        /// </summary>
        public ServiceException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceClient.ServiceException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public ServiceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceClient.ServiceException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="inner">Inner.</param>
        public ServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
