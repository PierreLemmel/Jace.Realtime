using System;
using System.Collections.Generic;

namespace Jace.RealTime.Compilation
{
    public interface IFunctionRegistry : IEnumerable<FunctionInfo>
    {
        FunctionInfo GetFunctionInfo(string functionName);
        bool IsFunctionName(string functionName);
        void RegisterFunction(string functionName, Delegate function);
    }
}
