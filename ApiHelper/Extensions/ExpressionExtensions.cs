using System;
using System.Linq.Expressions;
using RestApi.Infrastructure;

namespace RestApi.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, object>> GetPropertyExpression<T>(this IExpressionBuilder expressionBuilder, string propertyName, bool ignoreCase = true)
        {
            expressionBuilder.CheckArgumentNull(nameof(expressionBuilder));

            return expressionBuilder.GetPropertyExpression(typeof(T),
                propertyName,
                ignoreCase) as Expression<Func<T, object>>;
        }
    }
}
