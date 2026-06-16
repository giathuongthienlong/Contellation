using System.Reflection;

namespace Contellation.Custom.Controls.Datas.DataGrids
{
    public static class Extensions
    {
        public static bool IsSystemType(this Type type) => type.Assembly == typeof(object).Assembly;

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null) { throw new ArgumentException("Value cannot be null.", nameof(obj)); }
            if (propertyName == null) { throw new ArgumentException("Value cannot be null.", nameof(propertyName)); }

            foreach (var prop in propertyName.Split('.').Select(s => obj?.GetType().GetProperty(s))) { obj = prop?.GetValue(obj, null); }

            return obj;
        }

        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            foreach (var prop in propertyName.Split('.').Select(s => obj?.GetType().GetProperty(s))) { obj = prop?.GetValue(obj, null); }

            return (obj != null) ? (T)obj : default;
        }

        public static PropertyInfo GetPropertyInfo(this Type srcType, string propertyName)
        {
            if (srcType == null) { throw new ArgumentException("Value cannot be null.", nameof(srcType)); }
            if (propertyName == null) { throw new ArgumentException("Value cannot be null.", nameof(propertyName)); }

            PropertyInfo infos = null;

            if (!propertyName.Contains('.')) { return srcType.GetProperty(propertyName); }

            foreach (var info in propertyName.Split('.').Select(s => srcType?.GetProperty(s, BindingFlags.Public | BindingFlags.Instance)))
            {
                srcType = info?.PropertyType;
                if (srcType == null) break;
                infos = info;
            }

            return infos;
        }
    }
}
