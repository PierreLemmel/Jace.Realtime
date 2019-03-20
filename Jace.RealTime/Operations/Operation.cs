﻿namespace Jace.RealTime.Operations
{
    public abstract class Operation
    {
        public Operation(bool dependsOnVariables)
        {
            this.DependsOnVariables = dependsOnVariables;
        }

        public bool DependsOnVariables { get; private set; }
    }
}