using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class EmployeeDataAccess : IDisposable
    {
        /// <summary>
        /// Flag: Has Dispose already been called
        /// </summary>
        bool disposed;
        #region "=================== Constructor =============================="
        public EmployeeDataAccess()
        {
        }

        ~EmployeeDataAccess()
        {
            this.Dispose(false);
        }
        #endregion


        public DataTable GetProfile(string emailAddress)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetUserDetailById", true))
            {
                connector.AddInParameterWithValue("@emailaddress", emailAddress);
                dt = connector.GetDataTable();

            }

            return dt;
        }

        public DataTable GetEmployeeDetailById(long userId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetEmployeeById", true))
            {
                connector.AddInParameterWithValue("@employeeId", userId);
                dt = connector.GetDataTable();

            }

            return dt;
        }


        public DataTable UserLogIn(string emailAddress, string password, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetUserDetail", true))
            {
                connector.AddInParameterWithValue("@emailaddress", emailAddress);

                dt = connector.GetDataTable();
                if (dt.Rows.Count <= 0)
                    message = "Email address is not registered, please contact addministrator";
                else
                    message = "";
            }

            return dt;
        }

        public DataTable forgotPassword(string emailAddress, string key)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_forgotPassword", true))
            {
                connector.AddInParameterWithValue("@emailaddress", emailAddress);
                connector.AddInParameterWithValue("@key", key);

                dt = connector.GetDataTable();
            }

            return dt;
        }

        public bool ResetPassword(int userid, string password, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_ResetPassword", true))
            {
                connector.AddInParameterWithValue("@userId", userid);
                connector.AddInParameterWithValue("@password", password);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message == "Password changed." ? true : false;
        }

        public DataTable GetemployeeList(long userid, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetEmployee", true))
            {
                connector.AddInParameterWithValue("@userId", userid);
                dt = connector.GetDataTable();
                message = "ok";
            }

            return dt;
        }

        public DataTable ListEmployees(long userid, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListEmployees", true))
            {
                connector.AddInParameterWithValue("@userId", userid);
                dt = connector.GetDataTable();
                message = "ok";
            }

            return dt;
        }

        public DataTable GetStatus()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetStatus", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable ListStatus()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListStatus", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable GetUserTypes()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetUserTypes", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable ListUserTypes()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListUserTypes", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
        }
        public DataTable ListDepartments()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListDepartments", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable ListDesignations(int departmentId)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListDesignations", true))
            {
                connector.AddInParameterWithValue("@departmentId", departmentId);
                dt = connector.GetDataTable();
            }

            return dt;
        }


        public DataTable GetEmployeeTypes()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetEmployeeTypes", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
        }

        public DataTable ListEmployeeTypes()
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_ListEmployeeTypes", true))
            {
                //connector.AddInParameterWithValue("@userid", userid);
                dt = connector.GetDataTable();
            }

            return dt;
        }
        public bool CreateEmployee(EmployeeModel employee, long userId, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_CreateEmployee", true))
            {
                connector.AddInParameterWithValue("@userTypeId", employee.userTypeId);
                connector.AddInParameterWithValue("@departmentId", employee.departmentId);
                connector.AddInParameterWithValue("@designationId", employee.designationId);
                connector.AddInParameterWithValue("@firstName", employee.firstName);
                connector.AddInParameterWithValue("@lastName", employee.lastName);
                if(employee.DOB == null)
                connector.AddInParameterWithValue("@dob", DBNull.Value);
                else
                connector.AddInParameterWithValue("@dob", employee.DOB);
                if (employee.Gender == null)
                    connector.AddInParameterWithValue("@gender", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@gender", employee.Gender);
                    connector.AddInParameterWithValue("@joiningDate", employee.joiningDate);
                connector.AddInParameterWithValue("@phoneNo", Convert.ToInt64(employee.phoneNumber));
                if (employee.phoneNumber_1 == null)
                    connector.AddInParameterWithValue("@phoneNo_1", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@phoneNo_1", Convert.ToInt64(employee.phoneNumber_1));
                connector.AddInParameterWithValue("@emergencyPhoneNo", Convert.ToInt64(employee.emergencyphoneNumber));
                if (employee.emergencyphoneNumber_1 == null)
                    connector.AddInParameterWithValue("@emergencyPhoneNo_1", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@emergencyPhoneNo_1", Convert.ToInt64(employee.emergencyphoneNumber_1));
                if (employee.addressLine_1 == null)
                    connector.AddInParameterWithValue("@addressLine_1", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@addressLine_1", employee.addressLine_1);
                if (employee.addressLine_2 == null)
                    connector.AddInParameterWithValue("@addressLine_2", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@addressLine_2", employee.addressLine_2);
                if (employee.city == null)
                    connector.AddInParameterWithValue("@city", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@city", employee.city);
                if (employee.state == null)
                    connector.AddInParameterWithValue("@state", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@state", employee.state);
                if (employee.country == null)
                    connector.AddInParameterWithValue("@country", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@country", employee.country);
                connector.AddInParameterWithValue("@employeeTypeId", employee.employeeTypeId);
                if (employee.aadharNumber == null)
                    connector.AddInParameterWithValue("@aadharNumber", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@aadharNumber", employee.aadharNumber);
                if (employee.panNumber == null)
                    connector.AddInParameterWithValue("@panNumber", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@panNumber", employee.panNumber);
                if (employee.postalCode == null)
                    connector.AddInParameterWithValue("@postalCode", Convert.ToInt32(employee.postalCode));
                else
                    connector.AddInParameterWithValue("@postalCode", Convert.ToInt32(employee.postalCode));
                connector.AddInParameterWithValue("@email", employee.email);
                connector.AddInParameterWithValue("@password", employee.password);
                if (employee.employeeImageName == null)
                    connector.AddInParameterWithValue("@employeeImage", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@employeeImage", employee.employeeImageName);
                if (employee.aadharImageName == null)
                    connector.AddInParameterWithValue("@aadharImage", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@aadharImage", employee.aadharImageName);
                if (employee.panImageName == null)
                    connector.AddInParameterWithValue("@panImage", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@panImage", employee.panImageName);
                connector.AddInParameterWithValue("@status","Active");
                if (employee.userTypeId == 4 || employee.employeementDate == null)
                    connector.AddInParameterWithValue("@employeementDate", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@employeementDate", employee.employeementDate);
                connector.AddInParameterWithValue("@isDeleted", employee.isdeleted);
                connector.AddInParameterWithValue("@createdDate", System.DateTime.Now);
                connector.AddInParameterWithValue("@createdBy", userId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message == "Success" ? true : false;
        }


        public bool UpdateEmployee(EmployeeModel employee, long userId, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_UpdateEmployee", true))
            {
                connector.AddInParameterWithValue("@employeeId", employee.employeeId);
                connector.AddInParameterWithValue("@userTypeId", employee.userTypeId);
                connector.AddInParameterWithValue("@departmentId", employee.departmentId);
                connector.AddInParameterWithValue("@designationId", employee.designationId);
                connector.AddInParameterWithValue("@firstName", employee.firstName);
                connector.AddInParameterWithValue("@lastName", employee.lastName);
                if (employee.DOB == null)
                    connector.AddInParameterWithValue("@dob", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@dob", employee.DOB);
                if (employee.Gender == null)
                    connector.AddInParameterWithValue("@gender", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@gender", employee.Gender);
                connector.AddInParameterWithValue("@joiningDate", employee.joiningDate);
                connector.AddInParameterWithValue("@phoneNo", Convert.ToInt64(employee.phoneNumber));
                if (employee.phoneNumber_1 == null)
                    connector.AddInParameterWithValue("@phoneNo_1", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@phoneNo_1", Convert.ToInt64(employee.phoneNumber_1));
                connector.AddInParameterWithValue("@emergencyPhoneNo", Convert.ToInt64(employee.emergencyphoneNumber));
                if (employee.emergencyphoneNumber_1 == null)
                    connector.AddInParameterWithValue("@emergencyPhoneNo_1", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@emergencyPhoneNo_1", Convert.ToInt64(employee.emergencyphoneNumber_1));
                if (employee.addressLine_1 == null)
                    connector.AddInParameterWithValue("@addressLine_1", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@addressLine_1", employee.addressLine_1);
                if (employee.addressLine_2 == null)
                    connector.AddInParameterWithValue("@addressLine_2", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@addressLine_2", employee.addressLine_2);
                if (employee.city == null)
                    connector.AddInParameterWithValue("@city", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@city", employee.city);
                if (employee.state == null)
                    connector.AddInParameterWithValue("@state", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@state", employee.state);
                if (employee.country == null)
                    connector.AddInParameterWithValue("@country", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@country", employee.country);
                connector.AddInParameterWithValue("@employeeTypeId", employee.employeeTypeId);
                if (employee.aadharNumber == null)
                    connector.AddInParameterWithValue("@aadharNumber", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@aadharNumber", employee.aadharNumber);
                if (employee.panNumber == null)
                    connector.AddInParameterWithValue("@panNumber", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@panNumber", employee.panNumber);
                if (employee.postalCode == null)
                    connector.AddInParameterWithValue("@postalCode", Convert.ToInt32(employee.postalCode));
                else
                    connector.AddInParameterWithValue("@postalCode", Convert.ToInt32(employee.postalCode));
                connector.AddInParameterWithValue("@email", employee.email);
                connector.AddInParameterWithValue("@password", employee.password);
                if (employee.employeeImageName == null)
                    connector.AddInParameterWithValue("@employeeImage", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@employeeImage", employee.employeeImageName);
                if (employee.aadharImageName == null)
                    connector.AddInParameterWithValue("@aadharImage", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@aadharImage", employee.aadharImageName);
                if (employee.panImageName == null)
                    connector.AddInParameterWithValue("@panImage", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@panImage", employee.panImageName);
                connector.AddInParameterWithValue("@status", employee.status);
                if (employee.userTypeId == 4 || employee.employeementDate == null)
                    connector.AddInParameterWithValue("@employeementDate", DBNull.Value);
                else
                    connector.AddInParameterWithValue("@employeementDate", employee.employeementDate);
                connector.AddInParameterWithValue("@isDeleted", employee.isdeleted);
                connector.AddInParameterWithValue("@updatedDate", System.DateTime.Now);
                connector.AddInParameterWithValue("@updatedBy", userId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message == "Success" ? true : false;
        }

        public bool DeleteEmployee(long userId, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_DeleteUser", true))
            {
                connector.AddInParameterWithValue("@employeeId", userId);
                connector.AddInParameterWithValue("@deletedDate", System.DateTime.Now);
                connector.AddInParameterWithValue("@deletedBy", userId);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return true;
        }

        //public bool UpdatePassword(ChangePasswordModel passwordModel, long userId, out string message)
        //{
        //    int rowEffected = 0;
        //    using (DBConnector connector = new DBConnector("sp_UpdateUserPassword", true))
        //    {
        //        connector.AddInParameterWithValue("@userId", passwordModel.userid);
        //        connector.AddInParameterWithValue("@password", passwordModel.NewPassword);
        //        connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
        //        rowEffected = connector.ExceuteNonQuery();
        //        message = connector.GetParamaeterValue("@Message").ToString();
        //    }

        //    return message == "Success" ? true : false;
        //}

        public bool ResetPassword(long userid, string oldpassword, string newPassword, out string message)
        {
            int rowEffected = 0;
            using (DBConnector connector = new DBConnector("sp_ChangePassword", true))
            {
                connector.AddInParameterWithValue("@userId", userid);
                connector.AddInParameterWithValue("@oldpassword", oldpassword);
                connector.AddInParameterWithValue("@newpassword", newPassword);
                connector.AddOutParameterWithType("@Message", SqlDbType.VarChar);
                rowEffected = connector.ExceuteNonQuery();
                message = connector.GetParamaeterValue("@Message").ToString();
            }

            return message == "Password changed." ? true : false;
        }

        public DataTable UserLogInWithId(string loginId, string password, out string message)
        {
            DataTable dt = null;
            using (DBConnector connector = new DBConnector("sp_GetUserDetailByLoginId", true))
            {
                connector.AddInParameterWithValue("@email", loginId);
                connector.AddInParameterWithValue("@password", password);

                dt = connector.GetDataTable();
                if (dt.Rows.Count <= 0)
                    message = "Email or Password is invalid";
                else
                    message = "";
            }

            return dt;
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

                ////TODO: Clean all memeber and release resource.
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion
    }
}
