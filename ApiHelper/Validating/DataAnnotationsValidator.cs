using System.Threading.Tasks;

namespace RestApi.Validating
{
    public class DataAnnotationsValidator<TEntity> : IEntityValidator<TEntity>
    {
        public Task<ValidationResult> ValidateAsync(TEntity entity)
        {
            return Task.FromResult(ValidationResult.SuccessResult);
        }
    }
}
