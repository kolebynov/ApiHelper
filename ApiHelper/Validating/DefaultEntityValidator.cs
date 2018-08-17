using System.Threading.Tasks;

namespace RestApi.Validating
{
    public class DefaultEntityValidator<TEntity> : IEntityValidator<TEntity>
    {
        public virtual Task<ValidationResult> ValidateAsync(TEntity entity)
        {
            return Task.FromResult(ValidationResult.SuccessResult);
        }
    }
}
