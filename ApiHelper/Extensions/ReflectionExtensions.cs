using System;
using System.ComponentModel.DataAnnotations;
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

            PropertyInfo result = type.GetPublicProperties()
                .FirstOrDefault(property => property.Name.Equals(propertyName,
                    ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture));

            if (result == null)
            {
                throw new ArgumentException($"Property \"{propertyName}\" not found in type \"{type.FullName}\". Ignore case: {ignoreCase}", nameof(propertyName));
            }

            return result;
        }

        public static PropertyInfo[] GetPublicProperties(this Type type) =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        public static string GetDisplayName(this Type type) =>
            type.GetCustomAttribute<DisplayAttribute>()?.Name ?? type.Name;
    }
}
