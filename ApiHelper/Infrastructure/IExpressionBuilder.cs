using System;
using System.Linq.Expressions;
using RestApi.Common;

namespace RestApi.Infrastructure
{
    public interface IExpressionBuilder
    {
        LambdaExpression GetPropertyExpression(Type actualType, string propertyName,
            Type exptectedParamType = null, Type exptectedReturnType = null, bool ignoreCase = true);

        LambdaExpression GetFilterExpression(Type filterType, Filter filter);
    }
}
