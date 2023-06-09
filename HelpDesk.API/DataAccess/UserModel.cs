﻿using HelpDesk.API.DatabaseConnector;
using HelpDesk.API.DbHelpers;
using HelpDesk.API.DTO_s;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HelpDesk.API.DataAccess
{
    public class UserModel : IUserModel
    {
        public SqlDataReader CheckEmailExists(string email)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Email",email),
                };
                return DbConnector.ExecuteReader("UspCheckEmail", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> CheckEmailExists");
                return null;
            }
        }
        public SqlDataReader changepasswordrequest(string email, string token)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Email",email),
                new SqlParameter("@token",token)
                };
                return DbConnector.ExecuteReader("uspChangePasswordRequest", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> CheckEmailExists");
                return null;
            }
        }
        public SqlDataReader emailnotificationmodel()
        {
            try
            {
                //var para = new[];
                return DbConnector.ExecuteReader("uspGetEmailsforNotification", null);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> emailnotificationmodel");
                return null;
            }
        }
        public SqlDataReader updateemailnotificationmodel(long EmailId)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@EmailId",EmailId)
                };
                return DbConnector.ExecuteReader("uspUpdateEmailNotification", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> updateemailnotificationmodel");
                return null;
            }
        }

        public SqlDataReader verifypasswordrequest(string email, string token)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@email",email),
                new SqlParameter("@token",token)
                };
                return DbConnector.ExecuteReader("uspverifyrequest", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> CheckEmailExists");
                return null;
            }
        }

        public SqlDataReader CheckEmpIdExists(string empid)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@EmpId",empid),
                };
                return DbConnector.ExecuteReader("UspCheckEmployeeId", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> CheckEmpIdExists");
                return null;
            }
        }
        public SqlDataReader updateuserstatus(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@isApproved",obj.isApproved),
                new SqlParameter("@isCancelled",obj.isCancelled)
                };
                return DbConnector.ExecuteReader("uspUpdateUser", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> CheckEmpIdExists");
                return null;
            }
        }
        public SqlDataReader addproduct(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@ProductId",obj.ProductId),
                new SqlParameter("@UserId",obj.UserId)
                };
                return DbConnector.ExecuteReader("uspAddProduct", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> addproduct");
                return null;
            }
        }
        public SqlDataReader addAccount(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@AccountId",obj.AccountId),
                new SqlParameter("@UserId",obj.UserId)
                };
                return DbConnector.ExecuteReader("uspAddAccount", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> addproduct");
                return null;
            }
        }
        public SqlDataReader removeaccountorproduct(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@Type",obj.Type),
                new SqlParameter("@Id",obj.MUPId)
                };
                return DbConnector.ExecuteReader("uspRemoveAssignAccountorProduct", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> removeaccountorproduct");
                return null;
            }
        }

        public SqlDataReader GetCompanyAccounts(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId),
                };
                return DbConnector.ExecuteReader("uspGetCompanyAccounts", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetCompanyAccounts");
                return null;
            }
        }
        public SqlDataReader GetCompanyManagerAccounts(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@RoleId",obj.RoleId)
                };
                return DbConnector.ExecuteReader("uspGetCompanyManagerExistsAccounts", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetCompanyManagerAccounts");
                return null;
            }
        }

        public SqlDataReader GetCompanyProducts(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId),
                };
                return DbConnector.ExecuteReader("uspGetProductsByCompanyId", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetCompanyProducts");
                return null;
            }
        }
        public SqlDataReader GetCompanyProductsRoleWise(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@RoleId",obj.RoleId),
                new SqlParameter("@AccountId",obj.AccountId)
                };
                return DbConnector.ExecuteReader("uspGetProductsByCompanyIdRoleWise", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetCompanyProductsRoleWise");
                return null;
            }
        }

        public string GetUserDetailsById(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.UserId),
                };
                return DbConnector.ExecuteDataSet("uspGetUserDetailsById", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetUserDetailsById");
                return null;
            }
        }

        public string GetUserList(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspUsersList", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetUserList");
                return null;
            }
        }
        public string GetSystemUserListforApproval(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspGetSystemUsersSignup", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetUserList");
                return null;
            }
        }
        public string GetSystemUserDetailsById(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.UserId)
                };
                return DbConnector.ExecuteDataSet("uspGetSystemUserProducts", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetSystemUserDetailsById");
                return null;
            }
        }


        public SqlDataReader NewUserSignUp(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@RoleId",obj.RoleId),
                 new SqlParameter("@FullName",obj.FullName),
                 new SqlParameter("@Mobile",obj.Mobile),
                new SqlParameter("@Email",obj.Email),
                new SqlParameter("@xml",obj.Productsxml)
                };
                return DbConnector.ExecuteReader("uspInsertUsersSignup", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetUserList");
                return null;
            }
        }
        public SqlDataReader NewUser(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@RoleId",obj.RoleId),
                new SqlParameter("@EmpId",obj.EmpId),
                 new SqlParameter("@FullName",obj.FullName),
                new SqlParameter("@Gender",obj.Gender),
                 new SqlParameter("@Mobile",obj.Mobile),
                new SqlParameter("@Email",obj.Email),
                new SqlParameter("@Password",obj.Password),
                new SqlParameter("@isApproved",obj.isApproved),
                 new SqlParameter("@isActive",obj.isActive),
                new SqlParameter("@OrganizationId",obj.OrganizationId),
                 new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@CreatedBy",obj.UserId),
                new SqlParameter("@accountxml",obj.Accountsxml),
                new SqlParameter("@productxml",obj.Productsxml),
                new SqlParameter("@SignUp",obj.SignUp)
                };
                return DbConnector.ExecuteReader("uspInsertUsers", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> GetUserList");
                return null;
            }
        }
        public SqlDataReader UpdateUser(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@FullName",obj.FullName),
                 new SqlParameter("@Gender",obj.Gender),
                new SqlParameter("@Mobile",obj.Mobile),
                new SqlParameter("@RoleId",obj.RoleId)
                };
                return DbConnector.ExecuteReader("uspUpdatebasicUserInfo", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> UpdateUser");
                return null;
            }
        }
        public SqlDataReader UpdateSignUpUser(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@isApproved",obj.isApproved),
                new SqlParameter("@isCancelled",obj.isCancelled),
                 new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@CreatedBy",obj.CreatedBy),
                new SqlParameter("@isActive",obj.isActive)
                };
                return DbConnector.ExecuteReader("uspUpdateSignUpUser", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> UpdateUser");
                return null;
            }
        }
        public SqlDataReader UpdateUserStatusActive(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                 new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@CreatedBy",obj.CreatedBy)
                };
                return DbConnector.ExecuteReader("uspUpdateUserStatus", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> UpdateuserStatus");
                return null;
            }
        }


        public SqlDataReader UpdateUserpassword(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@UserId",obj.UserId),
                new SqlParameter("@Password",obj.Password)
                };
                return DbConnector.ExecuteReader("uspChangePassword", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> UpdateUserpassword");
                return null;
            }
        }
        public SqlDataReader UpdateUserpasswordwithemail(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                new SqlParameter("@email",obj.Email),
                new SqlParameter("@Password",obj.Password)
                };
                return DbConnector.ExecuteReader("uspChangePasswordWithEmail", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> UpdateUserpassword");
                return null;
            }
        }

        public string RoleCompanyDropDowns(UsersDTO obj)
        {
            try
            {
                var para = new[] {
                    new SqlParameter("@CompanyId",obj.CompanyId),
                new SqlParameter("@OrganizationId",obj.OrganizationId)
                };
                return DbConnector.ExecuteDataSet("uspGetDropdownForUserCreation", para);
            }
            catch (Exception ex)
            {
                DataModelExceptionUtility.LogException(ex, "UserModel -> RoleCompanyDropDowns");
                return null;
            }
        }
    }

    public interface IUserModel
    {
        SqlDataReader NewUser(UsersDTO obj);
        SqlDataReader NewUserSignUp(UsersDTO obj);
        SqlDataReader UpdateUser(UsersDTO obj);
        SqlDataReader UpdateSignUpUser(UsersDTO obj);
        SqlDataReader UpdateUserStatusActive(UsersDTO obj);
        SqlDataReader UpdateUserpassword(UsersDTO obj);
        SqlDataReader UpdateUserpasswordwithemail(UsersDTO obj);
        string RoleCompanyDropDowns(UsersDTO obj);
        string GetUserDetailsById(UsersDTO obj);
        string GetUserList(UsersDTO obj);
        string GetSystemUserListforApproval(UsersDTO obj);
        string GetSystemUserDetailsById(UsersDTO obj);
        SqlDataReader CheckEmailExists(string email);
        SqlDataReader changepasswordrequest(string email, string token);
        SqlDataReader emailnotificationmodel();
        SqlDataReader updateemailnotificationmodel(long EmailId);
        SqlDataReader verifypasswordrequest(string email, string token);
        SqlDataReader CheckEmpIdExists(string empid);
        SqlDataReader updateuserstatus(UsersDTO obj);
        SqlDataReader addproduct(UsersDTO obj);
        SqlDataReader addAccount(UsersDTO obj);
        SqlDataReader removeaccountorproduct(UsersDTO obj);
        SqlDataReader GetCompanyAccounts(UsersDTO obj);
        SqlDataReader GetCompanyManagerAccounts(UsersDTO obj);
        SqlDataReader GetCompanyProducts(UsersDTO obj);
        SqlDataReader GetCompanyProductsRoleWise(UsersDTO obj);

    }
}