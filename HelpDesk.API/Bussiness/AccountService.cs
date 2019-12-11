using HelpDesk.API.DataAccess;
using HelpDesk.API.GenericHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.API.Bussiness
{
    public class AccountService : IAccountService
    {
        private readonly IAccountModel model;
        public AccountService(AccountModel _model)
        {
            model = _model;
        }

        #region Account CURD Operation Interface implementation
        public IEnumerable<AccountDTO> GetAccountList(AccountDTO obj)
        {
            var data = model.GetAccountList(obj);
            var list = CustomDataReaderToGenericExtension.GetDataObjects<AccountDTO>(data);
            data.Close();
            return list;
        }
        public AccountDTO InsertUpdateAccount(AccountDTO obj)
        {
            try
            {
                var data = model.InsertUpdateAccount(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public AccountDTO DeleteAccount(AccountDTO obj)
        {
            try
            {
                var data = model.DeleteAccount(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.message = data["message"].ToString();
                    }
                }
                else
                    obj.message = "0";
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        public AccountDTO GetAccountDetailsById(AccountDTO obj)
        {
            try
            {
                var data = model.GetAccountDetailsById(obj);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        obj.AccountName = data["AccountName"].ToString();
                        obj.AccountCode = data["AccountCode"].ToString();
                        obj.isActive = bool.Parse(data["isActive"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                obj.message = ex.ToString();
                throw;
            }
            return obj;
        }
        #endregion
    }

    public interface IAccountService
    {
        #region Account CURD Operation Interface (declartion)
        IEnumerable<AccountDTO> GetAccountList(AccountDTO obj);
        AccountDTO InsertUpdateAccount(AccountDTO obj);
        AccountDTO DeleteAccount(AccountDTO obj);
        AccountDTO GetAccountDetailsById(AccountDTO obj);
        #endregion
    }
}