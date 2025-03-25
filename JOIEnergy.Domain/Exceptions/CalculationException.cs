using System;

namespace JOIEnergy.Domain.Exceptions
{
    public class CalculationException : Exception
    {
        public CalculationException(string message) : base(message) { }

        public CalculationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
