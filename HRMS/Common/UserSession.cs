using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace WebSite.Common
{
    [Serializable]
    public class UserSession
    {
        public long UserId { get; set; }

        public string LoginUserName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MobileNo { get; set; }

        public DateTime LastLogin { get; set; }
        public string JoiningDate { get; set; }

        public string Token { get; set; }

        public string AccessLevel { get; set; }

        public int userTypeId { get; set; }
        public int departmentId { get; set; }
        public int designationId { get; set; }
        public string designation { get; set; }


        public void SetValue(EmployeeModel user)
        {
            this.UserId = Convert.ToInt64(user.employeeId);
            this.LoginUserName = string.Concat(user.firstName, " ", user.lastName);
            this.EmailAddress = user.email;
            this.FirstName = user.firstName;
            this.LastName = user.lastName ;
            this.MobileNo =user.phoneNumber;
            this.userTypeId = user.userTypeId;
            this.designationId = user.designationId;
            this.designation = user.designation;
            this.JoiningDate = user.joiningDate;
        }
    }
}
