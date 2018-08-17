using System;

namespace RestApi.Validating
{
    public class EntityValidationException : Exception
    {
        public ValidationResult ValidationResult { get; }

        public EntityValidationException(string message) : base(message)
        { }

        public EntityValidationException(string message, Exception innerException) : base(message, innerException)
        { }

        public EntityValidationException(ValidationResult validationResult) : base(validationResult.ErrorMessage)
        {
            ValidationResult = validationResult;
        }
    }
}
