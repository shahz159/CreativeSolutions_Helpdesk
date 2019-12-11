using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HelpDesk.API.DbHelpers
{
    public sealed class SqlParameterBuilder
    {
        //   var param = GenericSqlParameterBuilder.CreateParameters<ClassName>(ClassObject);
        public static SqlParameter[] CreateParameters<T>(T myInput) where T : class
        {
            var parameter = new SqlParameter[typeof(T).GetProperties().Count()];
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            int i = 0;
            foreach (PropertyInfo property in properties)
            {

                parameter[i] = new SqlParameter("@" + property.Name, property.GetValue(myInput));
                i++;
            }
            return parameter;
        }

        public static SqlParameter[] CreateParameters<T>(T myInput, List<string> ignoreNames) where T : class
        {
            var parameter = new SqlParameter[typeof(T).GetProperties().Count() - ignoreNames.Count];
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                parameter[i] = new SqlParameter("@" + property.Name, property.GetValue(myInput));
                i++;
            }
            return parameter;
        }
    }
}