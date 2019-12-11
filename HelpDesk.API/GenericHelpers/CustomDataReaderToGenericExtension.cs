using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HelpDesk.API.GenericHelpers
{
    public static class CustomDataReaderToGenericExtension
    {
        public static IList<T> GetDataObjects<T>(SqlDataReader reader) where T : class, new()
        {
            var list = new List<T>();

            if (reader == null)
                return list;

            HashSet<string> tableColumnNames = null;
            while (reader.Read())
            {
                tableColumnNames = tableColumnNames ?? CollectColumnNames(reader);
                var entity = new T();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {

                    object retrievedObject = null;
                    if (tableColumnNames.Contains(propertyInfo.Name) && (retrievedObject = reader[propertyInfo.Name]) != null)
                    {
                        if (retrievedObject == DBNull.Value)
                            retrievedObject = null;
                        //p.SetValue(r, value, null);
                        propertyInfo.SetValue(entity, retrievedObject, null);
                    }
                }
                list.Add(entity);
            }
            //reader.Close();
            return list;
        }

        static HashSet<string> CollectColumnNames(SqlDataReader reader)
        {
            var collectedColumnInfo = new HashSet<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                collectedColumnInfo.Add(reader.GetName(i));
            }
            return collectedColumnInfo;
        }

        public static IList<T> GetDataObjects<T>(SqlDataReader reader, Action<T> updateEntity) where T : class, new()
        {
            var list = new List<T>();

            if (reader == null)
                return list;

            while (reader.Read())
            {
                var entity = new T();
                updateEntity(entity);
                list.Add(entity);
            }
            return list;
        }
    }
}