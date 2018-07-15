using System;
using System.Linq.Expressions;

namespace RestApi.Infrastructure
{
    public interface IExpressionBuilder
    {
        Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName, bool ignoreCase = true);
    }
}
