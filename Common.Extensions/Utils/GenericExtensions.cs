using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;


namespace Common.Extensions.Utils
{
    public static class GenericExtensions
    {
        public static int GetValue(this Enum value)
        {
            var type = value.GetType();
            return (int)Enum.Parse(type, Enum.GetName(type, value));
        }

        public static List<string> GetPropertiesNames(this Type type)
        {
            var result = new List<string>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                result.Add(property.Name);
            }
            return result;
        }

        public static string Serialize(this object AnObject)
        {
            XmlSerializer Xml_Serializer = new XmlSerializer(AnObject.GetType());
            StringWriter Writer = new StringWriter();

            Xml_Serializer.Serialize(Writer, AnObject);
            return Writer.ToString();
        }

        public static Object DeSerialize(this string XmlOfAnObject, Type ObjectType)
        {
            StringReader StrReader = new StringReader(XmlOfAnObject);
            XmlSerializer Xml_Serializer = new XmlSerializer(ObjectType);
            XmlTextReader XmlReader = new XmlTextReader(StrReader);
            try
            {
                Object AnObject = Xml_Serializer.Deserialize(XmlReader);
                return AnObject;
            }
            finally
            {
                XmlReader.Close();
                StrReader.Close();
            }
        }

        public static string FormatDate(this DateTime? value)
        {
            if (value.HasValue)
            {
                return value.GetValueOrDefault().ToString("MM/dd/yyyy"); // hh:mm:ss");
            }
            return "";
        }

        public static DateTime? ToDatetime(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            DateTime result = new DateTime();

            if (!DateTime.TryParseExact(value, "MM/dd/yyyy hh:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out result))
                return null;

            return result;
        }

        public static byte[] toByteArray(this string value)
        {
            return System.Text.Encoding.UTF8.GetBytes(value);
        }

        public static string toString(this byte[] value)
        {
            return System.Text.Encoding.UTF8.GetString(value);
        }
        public static PropertyInfo[] getPropertiesObjectContract<T>(T obj)
        {
            var tip = obj.GetType();
            var prop = tip.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return prop;
        }

        public static PropertyInfo[] getPropertiesObject<T>(T obj)
        {
            Type objType = typeof(T);
            return objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }


        public static List<TFrom> MapToList<TFrom>(this object objTo, Dictionary<string, string> changeNameProperty = null, string[] ignoreProperty = null, bool allowNullableMap = false) where TFrom : new()
        {
            var lista = objTo as IEnumerable<object>;
            var result = new List<TFrom>();

            foreach (var l in lista)
            {
                result.Add(l.MapTo<TFrom>(changeNameProperty, ignoreProperty, allowNullableMap));
            }
            return result;
        }

        public static TTarget MapTo<TFrom, TTarget>(this TFrom objFrom, Action<TFrom, TTarget> customAction) where TFrom : new() where TTarget : new()
        {
            var resObj = objFrom.MapTo<TTarget>();
            customAction(objFrom, resObj);
            return resObj;
        }

        public static TFrom MapTo<TFrom>(this object objTo, Dictionary<string, string> changeNameProperty = null, string[] ignoreProperty = null, bool allowNullableMap = false) where TFrom : new()
        {
            if (objTo == null)
                return new TFrom();

            var objFrom = new TFrom();
            var propsTTo = getPropertiesObjectContract(objTo);
            var propTFrom = getPropertiesObject(objFrom);

            if (ignoreProperty != null)
                ignoreProperty = ignoreProperty.ToList().ConvertAll(x => x.ToUpper()).ToArray();

            foreach (PropertyInfo prop in propsTTo)
            {
                if (ignoreProperty != null && ignoreProperty.Contains(prop.Name.ToUpper()))
                    continue;

                var propTtoName = prop.Name;
                if (changeNameProperty != null && changeNameProperty.ContainsKey(prop.Name))
                    propTtoName = changeNameProperty[prop.Name];

                var pFind = propTFrom.ToList().Find(p => p.Name.ToUpper() == propTtoName.ToUpper());

                if (pFind != null && pFind.CanWrite)
                {
                    bool nullableConv = false;
                    if (pFind.PropertyType != prop.PropertyType && !(allowNullableMap && pFind.PropertyType.IsGenericType && pFind.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(pFind.PropertyType) == prop.PropertyType))
                    {
                        Type nullableBaseType;
                        if (allowNullableMap && (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && pFind.PropertyType == (nullableBaseType = Nullable.GetUnderlyingType(prop.PropertyType))))
                        {
                            nullableConv = true;
                            object value = prop.GetValue(objTo, null) ?? Activator.CreateInstance(nullableBaseType);

                            pFind.SetValue(objFrom, value);
                        }
                        else
                            throw new Exception($"The property are not the same. [PROPERTY SET: {pFind.Name} - Type {pFind.PropertyType}] PROPERTY SOURCE: {prop.Name} - Type {prop.PropertyType}]");
                    }
                    if (!nullableConv)
                    {
                        object value = prop.GetValue(objTo, null);
                        pFind.SetValue(objFrom, value);
                    }
                }
            }

            return objFrom;
        }

        public static T Convert<T>(this object value, T defualtValue = default(T))
        {
            try
            {
                if (typeof(T).IsEnum)
                    return value != null ? (T)Enum.Parse(typeof(T), value.ToString(), true) : defualtValue;

                return value != null ? ((T)System.Convert.ChangeType(value, typeof(T))) : defualtValue;
            }
            catch (Exception ex)
            {
                return defualtValue;
            }
        }

    }
}
