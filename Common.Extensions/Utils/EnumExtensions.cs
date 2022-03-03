using Common.Types.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Extensions.Utils
{
    public static class EnumExtensions
    {
        static Dictionary<Tuple<Type, string>, string> enumStringValueCache = new Dictionary<Tuple<Type, string>, string>();
        static Dictionary<Enum, string> EnumStringValueCache = new Dictionary<Enum, string>();

        public static T ToEnumOrNull<T>(this string stringValue)
        {
            if (stringValue == null)
                return default(T);

            return ToEnum<T>(stringValue);
        }

        public static T ToEnum<T>(this string stringValue)
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException($"{enumType.Name} is not a valid enum");

            var tupleKey = new Tuple<Type, string>(enumType, stringValue);
            string element;
            if (!enumStringValueCache.TryGetValue(tupleKey, out element))
            {
                var enumElement = Enum.GetValues(typeof(T)).Cast<Enum>().FirstOrDefault(x => x.GetStringValue() == stringValue);
                if (enumElement == null)
                    throw new ArgumentException($"Invalid string value: {stringValue} for enum: {typeof(T).FullName}");

                element = enumElement.ToString();
                enumStringValueCache.Add(tupleKey, element);
            }

            return (T)Enum.Parse(typeof(T), element);
        }

        public static string GetStringValue(this Enum value)
        {
            if (EnumStringValueCache.ContainsKey(value))
                return EnumStringValueCache[value];

            var enumType = value.GetType();
            var fieldInfo = enumType.GetField(value.ToString());
            var attribs = fieldInfo.GetCustomAttributes<StringValueAttribute>(false);

            string stringValue;
            if (attribs.Any())
                stringValue = attribs.First().Description;
            else
                stringValue = value.ToString();

            try
            {
                EnumStringValueCache.Add(value, stringValue);
            }
            catch (Exception)
            {

            }

            return stringValue;
        }

        public static string GetDisplayAttribute(this Enum value)
        {
            return value.GetAttribute<DisplayAttribute>().Name;
        }

        private static string GetDescriprionAttribute(this Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>().Description;
        }

        private static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static string GetDescription(this Enum value)
        {
            var fullDescription = string.Empty;
            var displayAttribute = value.GetDescriprionAttribute();
            var stringValueAttribute = value.GetStringValue();

            if (!string.IsNullOrWhiteSpace(displayAttribute) && !string.IsNullOrWhiteSpace(stringValueAttribute))
                fullDescription = $"{displayAttribute}";
            else
                fullDescription = string.Empty;

            return fullDescription;
        }

        public static string GetFullDisplayName(this Enum value)
        {
            var fullDescription = string.Empty;
            var displayAttribute = value.GetDisplayAttribute();
            var stringValueAttribute = value.GetStringValue();

            if (!string.IsNullOrWhiteSpace(displayAttribute) && !string.IsNullOrWhiteSpace(stringValueAttribute))
                fullDescription = $"{stringValueAttribute} - {displayAttribute}";
            else
                fullDescription = string.Empty;

            return fullDescription;
        }

        public static List<T> GetFullEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            return new List<T>(array);
        }

        public static List<string> GetListStringValuesForEnum<T>()
        {
            return GetListStringValuesForEnum<T>(GetFullEnumList<T>());
        }

        private static string GetInternalStringValue<T>(T value)
        {
            FieldInfo field = typeof(T).GetField(value.ToString());
            return field.GetCustomAttributes(typeof(StringValueAttribute), false)
                        .Cast<StringValueAttribute>()
                        .Select(x => x.Description)
                        .FirstOrDefault();
        }

        public static List<string> GetListStringValuesForEnum<T>(List<T> allEnums)
        {
            var stringValues = new List<string>();

            allEnums.ToList().ForEach(x =>
            {
                stringValues.Add(GetInternalStringValue(x));
            });

            return stringValues;
        }
    }
}
