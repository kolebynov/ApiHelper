using System;
using System.Linq.Expressions;

namespace RestApi.Infrastructure
{
    public interface IExpressionBuilder
    {
        Expression<Func<object, object>> GetPropertyExpression(Type type, string propertyName, bool ignoreCase = true);
    }
}
