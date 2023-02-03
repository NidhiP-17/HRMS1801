using DataAccess;
using Microsoft.VisualBasic;
using Models;
using Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constants = Models.Constants;

namespace Repositories
{
    public class EmployeeRepository : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        private EmployeeDataAccess instance;
        #region "=================== Constructor =============================="
        public EmployeeRepository()
        {
            this.instance = new EmployeeDataAccess();
        }

        ~EmployeeRepository()
        {
            this.Dispose(false);
        }
        #endregion

        //public ResponseSingleModel<UserProfileModel> getUserProfile(string emailaddress)
        //{
        //    var result = new ResponseSingleModel<UserProfileModel>();
        //    var dt = instance.GetProfile(emailaddress);
        //    var lst = DataAccessUtility.ConvertToList<UserProfileModel>(dt);
        //    UserProfileModel user = lst.Count > 0 ? lst[0] : null;
        //    result.Response = user;
        //    result.Status = user != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
        //    result.Message = "Ok";
        //    return result;
        //}

        //public ResponseSingleModel<UserProfileModel> Login(string emailAddress, string password)
        //{
        //    var result = new ResponseSingleModel<UserProfileModel>();
        //    var message = string.Empty;
        //    var dt = instance.UserLogIn(emailAddress, password, out message);
        //    var lst = DataAccessUtility.ConvertToList<UserProfileModel>(dt);
        //    UserProfileModel user = lst != null && lst.Count > 0 ? lst[0] : null;
        //    result.Response = user;
        //    if (user == null)
        //    {
        //        result.Status = Constants.WebApiStatusFail;
        //        result.Message = "Email Address is not registered, please contact addministrator";
        //    }
        //    else
        //    {
        //        if (password == user.Password)
        //        {
        //            result.Status = Constants.WebApiStatusOk;
        //            result.Message = "Success";
        //        }
        //        else
        //        {
        //            result.Response = null;
        //            result.Status = Constants.WebApiStatusFail;
        //            result.Message = "Email Address or Password is invalid";
        //        }

        //    }

        //    return result;
        //}


        //public ResponseSingleModel<UserProfileModel> ForgotPassword(string emailAddress, string key)
        //{
        //    var result = new ResponseSingleModel<UserProfileModel>();
        //    var message = string.Empty;
        //    var dt = instance.forgotPassword(emailAddress, key);
        //    var lst = DataAccessUtility.ConvertToList<UserProfileModel>(dt);
        //    UserProfileModel user = lst != null && lst.Count > 0 ? lst[0] : null;
        //    result.Response = user;
        //    if (user == null)
        //    {
        //        result.Status = Constants.WebApiStatusFail;
        //        result.Message = "Email Address is not registered, please contact addministrator";
        //    }
        //    else
        //    {
        //        result.Status = Constants.WebApiStatusOk;
        //        result.Message = "Success";
        //    }

        //    return result;
        //}

        public ResponseSingleModel<bool> ChangePassword(long userid, string oldpassword, string newPassword, out string message)
        {
            var epass = EncodePasswordToBase64(newPassword);
            var result = new ResponseSingleModel<bool>();
            message = string.Empty;
            result.Response = instance.ResetPassword(userid, oldpassword, epass, out message);
            result.Status = message == "Password changed." ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }

        public ResponseSingleModel<bool> ResetPassword(int userid, string password, out string message)
        {
            var epass = EncodePasswordToBase64(password);
            var result = new ResponseSingleModel<bool>();
            message = string.Empty;
            result.Response = instance.ResetPassword(userid, epass, out message);
            result.Status = message == "Password changed." ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        
        //res.UpdateUserIDDocument(UserID, DocumentType, profileImagePath, out Message);

        //public ResponseSingleModel<UserProfileModel> getUserProfile(long userId)
        //{
        //    var result = new ResponseSingleModel<UserProfileModel>();
        //    var dt = instance.GetProfile(userId);
        //    var lst = DataAccessUtility.ConvertToList<UserProfileModel>(dt);
        //    UserProfileModel user = lst.Count > 0 ? lst[0] : null;
        //    result.Response = user;
        //    result.Status = user != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
        //    result.Message = "Ok";
        //    return result;
        //}


        //public ResponseCollectionModel<UserProfileModel> GetUserListList(long userid, out string message)
        //{
        //    var result = new ResponseCollectionModel<UserProfileModel>();
        //    message = string.Empty;
        //    var dt = instance.GetUserList(userid, out message);
        //    var lst = DataAccessUtility.ConvertToList<UserProfileModel>(dt);
        //    result.Response = lst;
        //    result.Status = Constants.WebApiStatusOk;
        //    result.Message = message;
        //    return result;
        //}
        public ResponseCollectionModel<EmployeeModel> GetList(long userId, out string message)
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.GetemployeeList(userId, out message);
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<EmployeeModel> ListEmployees(long userId, out string message)
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.ListEmployees(userId, out message);
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<EmployeeModel> GetStatus()
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.GetStatus();
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<EmployeeModel> ListStatus()
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.ListStatus();
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<EmployeeModel> GetUserTypes()
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.GetUserTypes();
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<EmployeeModel> ListUserTypes()
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.ListUserTypes();
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }

        public ResponseCollectionModel<EmployeeModel> ListDepartments()
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.ListDepartments();
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }

        public ResponseCollectionModel<EmployeeModel> ListDesignations(int departmentId)
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.ListDesignations(departmentId);
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<EmployeeModel> GetEmployeeTypes()
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.GetEmployeeTypes();
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseCollectionModel<EmployeeModel> ListEmployeeTypes()
        {
            var result = new ResponseCollectionModel<EmployeeModel>();
            var dt = instance.ListEmployeeTypes();
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            result.Response = lst;
            result.Status = Constants.WebApiStatusOk;
            result.Message = "";
            return result;
        }
        public ResponseSingleModel<bool> CreateEmployee(EmployeeModel userProfile, long userId)
        {
            var epass = EncodePasswordToBase64(userProfile.password);
            var result = new ResponseSingleModel<bool>();
            var message = string.Empty;
            userProfile.password = epass;
            result.Response = instance.CreateEmployee(userProfile, userId, out message);
            result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<bool> UpdateEmployee(EmployeeModel userProfile, long userId)
        {
            userProfile.password = EncodePasswordToBase64(userProfile.password);
            var result = new ResponseSingleModel<bool>();
            var message = string.Empty;
            result.Response = instance.UpdateEmployee(userProfile, userId, out message);
            result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<bool> DeleteEmployee(long userId)
        {
            var result = new ResponseSingleModel<bool>();
            var message = string.Empty;
            result.Response = instance.DeleteEmployee(userId, out message);
            result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = message;
            return result;
        }
        public ResponseSingleModel<EmployeeModel> GetDetail(long id)
        {
            var result = new ResponseSingleModel<EmployeeModel>();
            var dt = instance.GetEmployeeDetailById(id);
            //foreach (DataRow row in dt.Rows)
            //{
            //    row["Password"] = DecodeFrom64(row["Password"].ToString());
            //}
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            EmployeeModel detail = lst.Count > 0 ? lst[0] : null;
            result.Response = detail;
            result.Status = detail != null ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
            result.Message = "Ok";
            return result;
        }

        //public ResponseSingleModel<bool> UpdatePassword(ChangePasswordModel passwordModel, long userId)
        //{
        //    var result = new ResponseSingleModel<bool>();
        //    var message = string.Empty;
        //    result.Response = instance.UpdatePassword(passwordModel, userId, out message);
        //    result.Status = result.Response ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
        //    result.Message = message;
        //    return result;
        //}

        //public ResponseSingleModel<bool> ResetPassword(long userid, string oldpassword, string newPassword, out string message)
        //{
        //    var result = new ResponseSingleModel<bool>();
        //    message = string.Empty;
        //    result.Response = instance.ResetPassword(userid, oldpassword, newPassword, out message);
        //    result.Status = message == "Password changed." ? Constants.WebApiStatusOk : Constants.WebApiStatusFail;
        //    result.Message = message;
        //    return result;
        //}
        public ResponseSingleModel<EmployeeModel> LoginUsingId(string? loginId, string? password)
        {
            var epass = EncodePasswordToBase64(password);
            var result = new ResponseSingleModel<EmployeeModel>();
            var message = string.Empty;
            var dt = instance.UserLogInWithId(loginId, epass, out message);
            var lst = DataAccessUtility.ConvertToList<EmployeeModel>(dt);
            EmployeeModel user = lst != null && lst.Count > 0 ? lst[0] : null;
            result.Response = user;
            if (user == null)
            {
                result.Status = Models.Constants.WebApiStatusFail;
                result.Message = "Invalid credentials.";
            }
            else
            {
                if (epass == user.password)
                {
                    result.Status = Constants.WebApiStatusOk;
                    result.Message = "Success";
                }
                else
                {
                    result.Response = null;
                    result.Status = Constants.WebApiStatusFail;
                    result.Message = "Email or Password is invalid";
                }

            }

            return result;
        }
        //this function Convert to Encord your Password
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //this function Convert to Decord your Password
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        #region ========================= Dispose Method ==============
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            if (disposing)
            {
                if (this.instance != null)
                {
                    this.instance.Dispose();
                    this.instance = null;
                }

                ////Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }
        #endregion
    }
}
