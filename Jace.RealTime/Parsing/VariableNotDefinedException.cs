using System;

namespace Jace.RealTime.Parsing
{
    public class VariableNotDefinedException : Exception
    {
        public VariableNotDefinedException(string message)
            : base(message)
        {
        }
    }
}