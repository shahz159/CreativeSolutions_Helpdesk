using HelpDesk.API.DataAccess;
using HelpDesk.API.DbHelpers;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class LoginPageService : ILoginPageService
    {
        private readonly ILoginPageModel model;
        public LoginPageService(LoginPageModel _model)
        {
            model = _model;
        }
        public LoginPageDTO getLogin(LoginPageDTO obj)
        {
            try
            {
                bool validateSucceeded = true;
                if (validateSucceeded)
                {
                    obj.datasetxml = model.getLogin(obj);
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception Ex)
            {
                DataModelExceptionUtility.LogException(Ex, "AdminService -> getLogin");
                DataModelExceptionUtility.NotifySystemOperators(Ex);
                throw;
            }
        }
    }
    public interface ILoginPageService
    {
        LoginPageDTO getLogin(LoginPageDTO obj);
    }
}