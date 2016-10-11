using System;

namespace NeuralMotion.Evolution.GeneticSharp.Validations
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("Config did not produc viable setup!")
        {
        }
    }
}