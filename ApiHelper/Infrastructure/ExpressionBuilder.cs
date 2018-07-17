using RestApi.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace RestApi.Infrastructure
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        private static readonly ConcurrentDictionary<ExpressionCacheKey, Expression> ExpressionsCache = new ConcurrentDictionary<ExpressionCacheKey, Expression>();

        public LambdaExpression GetPropertyExpression(Type actualType, string propertyName,
            Type exptectedParamType = null, Type exptectedReturnType = null, bool ignoreCase = true)
        {
            actualType.CheckArgumentNull(nameof(actualType));
            propertyName.CheckArgumentNullOrEmpty(nameof(propertyName));

            if (exptectedParamType == null)
            {
                exptectedParamType = actualType;
            }

            PropertyInfo property = actualType.GetPublicProperty(propertyName, ignoreCase);
            if (exptectedReturnType == null)
            {
                exptectedReturnType = property.PropertyType;
            }

            ExpressionCacheKey cacheKey = new ExpressionCacheKey(0,
                HashCode.Combine(actualType, exptectedParamType, exptectedReturnType),
                propertyName.ToLowerInvariant().GetHashCode());

            return (LambdaExpression) ExpressionsCache.GetOrAdd(cacheKey, _ =>
            {
                ParameterExpression parameter = Expression.Parameter(exptectedParamType);
                Expression finalExpression = parameter;

                if (actualType != exptectedParamType)
                {
                    finalExpression = finalExpression.Convert(actualType);
                }

                finalExpression = finalExpression.Property(property);

                if (property.PropertyType != exptectedReturnType)
                {
                    finalExpression = finalExpression.Convert(exptectedReturnType);
                }

                return finalExpression
                    .Lambda(new[] {parameter});
            });
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