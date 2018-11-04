using RestApi.Common;
using RestApi.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace RestApi.Infrastructure
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        private const string PropertyPathSeparator = ".";

        private static readonly ConcurrentDictionary<ExpressionCacheKey, Expression> ExpressionsCache = new ConcurrentDictionary<ExpressionCacheKey, Expression>();

        private static readonly Dictionary<ComparisonType, Func<Expression, Expression, BinaryExpression>> ComparisonTypeMap =
            new Dictionary<ComparisonType, Func<Expression, Expression, BinaryExpression>>()
            {
                { ComparisonType.Equal, Expression.Equal },
                { ComparisonType.NotEqual, Expression.NotEqual },
                { ComparisonType.Less, Expression.LessThan },
                { ComparisonType.LessOrEqual, Expression.LessThanOrEqual },
                { ComparisonType.Greater, Expression.GreaterThan },
                { ComparisonType.GreaterOrEqual, Expression.GreaterThanOrEqual }
            };

        public LambdaExpression GetPropertyExpression(Type actualType, string propertyName,
            Type exptectedParamType = null, Type exptectedReturnType = null, bool ignoreCase = true)
        {
            actualType.CheckArgumentNull(nameof(actualType));

            if (exptectedParamType == null)
            {
                exptectedParamType = actualType;
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

                finalExpression =
                    GetPropertyAccessExpression(finalExpression, propertyName, exptectedReturnType, ignoreCase);

                return finalExpression
                    .Lambda(new[] {parameter});
            });
        }

        public LambdaExpression GetFilterExpression(Type filterType, Filter filter)
        {
            string[] properties = filter.PropertyPath.Split(PropertyPathSeparator);
            ParameterExpression parameter = Expression.Parameter(filterType);
            Expression finalExpression = parameter;

            foreach (string propertyName in properties)
            {
                finalExpression = GetPropertyAccessExpression(finalExpression, propertyName);
            }

            TypeConverter converter = TypeDescriptor.GetConverter(finalExpression.Type);
            Type comparerType = typeof(Comparer<>).MakeGenericType(finalExpression.Type);
            finalExpression = Expression.Call(
                Expression.Property(null, comparerType, "Default"),
                "Compare",
                null,
                finalExpression,
                Expression.Constant(converter.ConvertFromString(filter.Value)));
            
            finalExpression = ComparisonTypeMap[filter.ComparisonType](finalExpression,
                Expression.Constant(0));

            return finalExpression.Lambda(new[] {parameter});
        }

        private Expression GetPropertyAccessExpression(Expression srcExpression, string propertyName,
            Type exptectedReturnType = null, bool ignoreCase = true)
        {
            propertyName.CheckArgumentNullOrEmpty(nameof(propertyName));

            Type actualType = srcExpression.Type;

            PropertyInfo property = actualType.GetPublicProperty(propertyName, ignoreCase);
            if (exptectedReturnType == null)
            {
                exptectedReturnType = property.PropertyType;
            }

            Expression finalExpression = srcExpression.Property(property);

            if (property.PropertyType != exptectedReturnType)
            {
                finalExpression = finalExpression.Convert(exptectedReturnType);
            }

            return finalExpression;
        }

        private struct ExpressionCacheKey
        {
            private readonly int _methodIndex;
            private readonly int _value1;
            private readonly int _value2;

            public ExpressionCacheKey(int methodIndex, int value1, int value2)
            {
                _methodIndex = methodIndex;
                _value1 = value1;
                _value2 = value2;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is ExpressionCacheKey))
                {
                    return false;
                }

                var key = (ExpressionCacheKey)obj;
                return _methodIndex == key._methodIndex &&
                       _value1 == key._value1 &&
                       _value2 == key._value2;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(_methodIndex, _value1, _value2);
            }
        }
    }
}