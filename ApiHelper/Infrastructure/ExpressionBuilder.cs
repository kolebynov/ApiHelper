using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RestApi.Extensions;

namespace RestApi.Infrastructure
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        private static readonly Dictionary<ExpressionCacheKey, Expression> ExpressionsCache = new Dictionary<ExpressionCacheKey, Expression>();

        public Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName, bool ignoreCase = true)
        {
            propertyName.CheckArgumentNullOrEmpty(nameof(propertyName));

            Type type = typeof(T);
            ExpressionCacheKey cacheKey = new ExpressionCacheKey(0, type.GetHashCode(), propertyName.ToLowerInvariant().GetHashCode());

            if (!ExpressionsCache.TryGetValue(cacheKey, out Expression result))
            {
                PropertyInfo property = GetPublicProperty(type, propertyName, ignoreCase);
                ParameterExpression parameter = Expression.Parameter(type);
                result = Expression.Lambda<Func<T, object>>(Expression.Property(parameter, property), parameter);
                ExpressionsCache[cacheKey] = result;
            }

            return (Expression<Func<T, object>>) result;
        }

        private PropertyInfo GetPublicProperty(Type type, string propertyName, bool ignoreCase)
        {
            PropertyInfo result = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(property => property.Name.Equals(propertyName,
                    ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture));

            if (result == null)
            {
                throw new ArgumentException($"Property \"{propertyName}\" not found in type \"{type.Name}\". Ignore case: {ignoreCase}", nameof(propertyName));
            }

            return result;
        }

        private struct ExpressionCacheKey
        {
            public readonly int MethodIndex;
            public readonly int HashCode1;
            public readonly int HashCode2;

            public ExpressionCacheKey(int methodIndex, int hashCode1, int hashCode2)
            {
                MethodIndex = methodIndex;
                HashCode1 = hashCode1;
                HashCode2 = hashCode2;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is ExpressionCacheKey))
                {
                    return false;
                }

                var key = (ExpressionCacheKey)obj;
                return MethodIndex == key.MethodIndex &&
                       HashCode1 == key.HashCode1 &&
                       HashCode2 == key.HashCode2;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(MethodIndex, HashCode1, HashCode2);
            }
        }
    }
}