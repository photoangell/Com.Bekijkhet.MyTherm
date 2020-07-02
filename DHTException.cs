using System;

namespace Com.Bekijkhet.MyTherm
{
    public class DHTException : Exception {
        public DHTException(string message, Exception innerException) 
            : base(message, innerException) {}
    }
}
