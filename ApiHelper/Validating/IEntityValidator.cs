using System.Threading.Tasks;

namespace RestApi.Validating
{
    public interface IEntityValidator<in TEntity>
    {
        Task<ValidationResult> ValidateAsync(TEntity entity);
    }
}
