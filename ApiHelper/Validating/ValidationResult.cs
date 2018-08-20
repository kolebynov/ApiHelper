using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestApi.Validating
{
    public class ValidationResult
    {
        public static readonly ValidationResult SuccessResult = new ValidationResult();

        public bool Success => !Errors.Any();

        //TODO: Move initialization to constructor if needed
        public string ErrorMessage => Errors.Aggregate(new StringBuilder(), (sb, error) => sb.AppendLine(error.Message))
            .ToString();
        public IEnumerable<ValidationError> Errors { get; }

        public ValidationResult(IEnumerable<ValidationError> errors = null)
        {
            Errors = new List<ValidationError>(errors ?? new ValidationError[0]);
        }
    }
}
