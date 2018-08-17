namespace RestApi.Validating
{
    public class ValidationResult
    {
        public static readonly ValidationResult SuccessResult = new ValidationResult(true, null);

        public ValidationResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; }
        public string ErrorMessage { get; }
    }
}
