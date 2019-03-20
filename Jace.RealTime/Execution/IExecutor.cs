using System;
using System.Collections.Generic;
using Jace.RealTime.Compilation;
using Jace.RealTime.Operations;

namespace Jace.RealTime.Execution
{
    public interface IExecutor
    {
        float Execute(Operation operation, IFunctionRegistry functionRegistry);
        float Execute(Operation operation, IFunctionRegistry functionRegistry, IDictionary<string, float> variables);

        Func<IDictionary<string, float>, float> BuildFormula(Operation operation, IFunctionRegistry functionRegistry);
    }
}
