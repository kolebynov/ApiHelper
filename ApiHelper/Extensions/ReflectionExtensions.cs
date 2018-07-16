using System;
using System.Linq;
using System.Reflection;

namespace RestApi.Extensions
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPublicProperty(this Type type, string propertyName, bool ignoreCase)
        {
            type.CheckArgumentNull(nameof(type));
            propertyName.CheckArgumentNullOrEmpty(nameof(propertyName));

            PropertyInfo result = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(property => property.Name.Equals(propertyName,
                    ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture));

            if (result == null)
            {
                throw new ArgumentException($"Property \"{propertyName}\" not found in type \"{type.FullName}\". Ignore case: {ignoreCase}", nameof(propertyName));
            }

            return result;
        }
    }
}
