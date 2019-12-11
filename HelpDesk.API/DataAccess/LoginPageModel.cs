using HelpDesk.API.DatabaseConnector;
using HelpDesk.API.DbHelpers;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DataAccess
{
    public class LoginPageModel : ILoginPageModel
    {
        public string getLogin(LoginPageDTO obj)
        {
            try
            {
                var parameter = new[]
                {
                    new SqlParameter("@Email",obj.Email),
                    new SqlParameter("@Password",obj.Password)
                };
                return DbConnector.ExecuteDataSet("UspgetLogin", parameter);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "LoginPageModel -> getLogin");
                DataModelExceptionUtility.NotifySystemOperators(ex);
                return null;
            }
        }
    }
    public interface ILoginPageModel
    {
        string getLogin(LoginPageDTO obj);

    }
}