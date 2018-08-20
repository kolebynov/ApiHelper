using System;

namespace RestApi.Validating
{
    public class ValidationException : Exception
    {
        public ValidationResult ValidationResult { get; }

        public ValidationException(string message) : base(message)
        { }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        { }

        public ValidationException(ValidationResult validationResult) : base(validationResult.ErrorMessage)
        {
            ValidationResult = validationResult;
        }
    }
}
