using System;

namespace TestTask.Core.Network
{
    public sealed class WebRequestException : Exception
    {
        public long ResponseCode { get; }

        public WebRequestException(string message, long responseCode)
            : base(message)
        {
            ResponseCode = responseCode;
        }
    }
}
