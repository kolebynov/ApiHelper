using RestApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace RestApi.Infrastructure
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        private static readonly Dictionary<ExpressionCacheKey, Expression> ExpressionsCache = new Dictionary<ExpressionCacheKey, Expression>();

        public Expression<Func<object, object>> GetPropertyExpression(Type type, string propertyName, bool ignoreCase = true)
        {
            type.CheckArgumentNull(nameof(type));
            propertyName.CheckArgumentNullOrEmpty(nameof(propertyName));

            ExpressionCacheKey cacheKey = new ExpressionCacheKey(0, type.GetHashCode(), propertyName.ToLowerInvariant().GetHashCode());

            if (!ExpressionsCache.TryGetValue(cacheKey, out Expression result))
            {
                PropertyInfo property = type.GetPublicProperty(propertyName, ignoreCase);
                ParameterExpression parameter = Expression.Parameter(typeof(object));
                result = Expression.Lambda<Func<object, object>>(Expression.Property(Expression.Convert(parameter, type), property), parameter);
                ExpressionsCache[cacheKey] = result;
            }

            return (Expression<Func<object, object>>) result;
        }

        private struct ExpressionCacheKey
        {
            public readonly int MethodIndex;
            public readonly int Value1;
            public readonly int Value2;

            public ExpressionCacheKey(int methodIndex, int value1, int value2)
            {
                MethodIndex = methodIndex;
                Value1 = value1;
                Value2 = value2;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is ExpressionCacheKey))
                {
                    return false;
                }

                var key = (ExpressionCacheKey)obj;
                return MethodIndex == key.MethodIndex &&
                       Value1 == key.Value1 &&
                       Value2 == key.Value2;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(MethodIndex, Value1, Value2);
            }
        }
    }
}