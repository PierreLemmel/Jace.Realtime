using System;

namespace Jace.RealTime.Parsing
{
    public class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }
    }
}