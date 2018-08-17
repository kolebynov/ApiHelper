using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApi.Validating
{
    public class GroupedEntityValidators<TEntity> : IEntityValidator<TEntity>
    {
        private readonly IEnumerable<IEntityValidator<TEntity>> _validators;

        public GroupedEntityValidators(IEnumerable<IEntityValidator<TEntity>> entityValidators)
        {
            _validators = entityValidators ?? throw new ArgumentNullException(nameof(entityValidators));
        }

        public async Task<ValidationResult> ValidateAsync(TEntity entity)
        {
            foreach (var validator in _validators)
            {
                ValidationResult validationResult = await validator.ValidateAsync(entity);
                if (!validationResult.Success)
                {
                    return validationResult;
                }
            }

            return ValidationResult.SuccessResult;
        }
    }
}
