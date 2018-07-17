using System;
using System.Linq.Expressions;
using System.Reflection;
using RestApi.Infrastructure;

namespace RestApi.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, object>> GetPropertyExpression<T>(this IExpressionBuilder expressionBuilder, string propertyName, bool ignoreCase = true)
        {
            expressionBuilder.CheckArgumentNull(nameof(expressionBuilder));

            return (Expression<Func<T, object>>) expressionBuilder.GetPropertyExpression(typeof(T),
                propertyName, typeof(T), typeof(object), ignoreCase);
        }

        public static UnaryExpression Convert(this Expression expression, Type type)
        {
            expression.CheckArgumentNull(nameof(expression));

            return Expression.Convert(expression, type);
        }

        public static MemberExpression Property(this Expression expression, PropertyInfo property)
        {
            expression.CheckArgumentNull(nameof(expression));

            return Expression.Property(expression, property);
        }

        public static LambdaExpression Lambda(this Expression expression, ParameterExpression[] parameters)
        {
            expression.CheckArgumentNull(nameof(expression));

            return Expression.Lambda(expression, parameters);
        }
    }
}
