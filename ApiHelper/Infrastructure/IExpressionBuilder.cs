using System;
using System.Linq.Expressions;

namespace RestApi.Infrastructure
{
    public interface IExpressionBuilder
    {
        LambdaExpression GetPropertyExpression(Type actualType, string propertyName,
            Type exptectedParamType = null, Type exptectedReturnType = null, bool ignoreCase = true);
    }
}
