using System;

namespace Yo
{
    public class YoException : ApplicationException
    {
        public YoException(string message) : base(message) { }
    }
}
