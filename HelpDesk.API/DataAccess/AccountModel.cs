using HelpDesk.API.DatabaseConnector;
using HelpDesk.API.DbHelpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DataAccess
{

    public class AccountModel: IAccountModel
    {
        public SqlDataReader GetAccountList(AccountDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@CompanyId", obj.CompanyId)
            };
                return DbConnector.ExecuteReader("UspGetAccountList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AccountModel -> GetAccountList");
                return null;
            }
        }
        public SqlDataReader InsertUpdateAccount(AccountDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@AccountId",obj.AccountId),
                    new SqlParameter("@AccountCode",obj.AccountCode),
                    new SqlParameter("@AccountName",obj.AccountName),
                    new SqlParameter("@isActive",obj.isActive),
                    new SqlParameter("@FlagId",obj.FlagId),
                    new SqlParameter("@CreatedBy",obj.CreatedBy),
                    new SqlParameter("@CompanyId",obj.CompanyId)

                };
                return DbConnector.ExecuteReader("UspInsertUpdateAccount", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AccountModel -> InsertUpdateAccount");
                return null;
            }

        }
        public SqlDataReader DeleteAccount(AccountDTO obj)
        {
            try
            {
                var para = new[]
                {
                    new SqlParameter("@AccountId",obj.AccountId)
                };
                return DbConnector.ExecuteReader("UspUpdateAccountStatus", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AccountModel -> DeleteAccount");
                return null;
            }
        }

        public SqlDataReader GetAccountDetailsById(AccountDTO obj) {
            try
            {
                var para = new[] {
                    new SqlParameter("@AccountId",obj.AccountId)
                };
                return DbConnector.ExecuteReader("uspGetAccountDetailsById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "AccountModel -> GetAccountDetailsById");
                return null;
            }
        }
    }
    public interface IAccountModel
    {
        SqlDataReader GetAccountList(AccountDTO obj);
        SqlDataReader InsertUpdateAccount(AccountDTO obj);
        SqlDataReader DeleteAccount(AccountDTO obj);
        SqlDataReader GetAccountDetailsById(AccountDTO obj);
    }
}