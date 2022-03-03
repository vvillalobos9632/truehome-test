using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Framework.Core.Extensions
{
    public static class ExtensionMethods
    {
        public static PropertyInfo[] getPropertiesObject<T>(T obj)
        {
            Type objType = typeof(T);
            return objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static PropertyInfo[] getPropertiesObjectContract<T>(T obj)
        {
            var tip = obj.GetType();
            var prop = tip.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return prop;
        }

        public static List<T> GetDataListFromDataReader<T>(SqlDataReader reader) where T : class, new()
        {
            var result = new List<T>();

            if (reader == null)
                return result;

            HashSet<string> tableColumnNames = null;
            while (reader.Read())
            {
                if (tableColumnNames == null)
                    tableColumnNames = CollectColumnNames(reader);

                var modelType = new T();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    object retrievedObject = null;
                    var propertyName = propertyInfo.Name.ToUpper();
                    if (tableColumnNames.Contains(propertyName) && (retrievedObject = reader[propertyName]) != null && !reader[propertyName].Equals(System.DBNull.Value))
                        propertyInfo.SetValue(modelType, retrievedObject, null);
                }

                result.Add(modelType);
            }
            reader.NextResult();
            return result;
        }

        private static HashSet<string> CollectColumnNames(SqlDataReader reader)
        {
            var collectedColumnInfo = new HashSet<string>();

            for (int i = 0; i < reader.FieldCount; i++)
                collectedColumnInfo.Add(reader.GetName(i).ToUpper());

            return collectedColumnInfo;
        }
    }

}
