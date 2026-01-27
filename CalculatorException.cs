// tiny exception used for calculator operations
using System;

namespace BasicCalculatorPart1
{
    public sealed class CalculatorException : Exception
    {
        // pass message to system.exception
        public CalculatorException(string message) : base(message) { }
    }
}
