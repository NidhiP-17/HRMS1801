using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Numerics;
using ExpressiveAnnotations.Attributes;

namespace Models
{
    public class EmployeeModel
    {
        public int employeeId { get; set; }
        [UserTypeValidation(ErrorMessage = "Select usertype")]
        [Required(ErrorMessage = "UserType is required")]
        [DisplayName("User Type")]
        public int userTypeId { get; set; }
        [Required(ErrorMessage = "Department is required")]
        [DisplayName("Department")]
        public int departmentId { get; set; }
        [DisplayName("Department")]
        public string? department { get; set; }
        [Required(ErrorMessage = "Designation is required")]
        [DisplayName("Designation")]
        public int designationId { get; set; }
        [DisplayName("Designation")]
        public string? designation { get; set; }
        [DisplayName("User Type")]
        public string? userType { get; set; }
        [DisplayName("Employee Code")]
        public string? employeeCode { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [DisplayName("First Name")]
        public string? firstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [DisplayName("Last Name")]
        public string? lastName { get; set; }

        //[Required(ErrorMessage = "Birthdate is required")]
        [DisplayName("Birth Date")]
        public string? DOB { get; set; }
        public string? DOBVal { get; set; }
        //[Required(ErrorMessage = "Gender is required")]
        [DisplayName("Gender")]
        public string? Gender { get; set; }
        public string? GenderValue { get; set; }

        [Required(ErrorMessage = "Joining Date is required")]
        [DisplayName("Joining Date")]
        public string? joiningDate { get; set; }
        public string? joiningDateVal { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [DisplayName("Phone Number")]
        [MaxLength(10)]
        public string? phoneNumber { get; set; }
        public string? phoneNumberVal { get; set; }
        [MaxLength(10)]
        public string? phoneNumber_1 { get; set; }
        public string? phoneNumber_1Val { get; set; }
        [Required(ErrorMessage = "Emergency Phone Number is required")]
        [DisplayName("Emergency Phone Number")]
        [MaxLength(10)]
        public string? emergencyphoneNumber { get; set; }
        [DisplayName("Emergency Number_1")]
        [MaxLength(10)]
        public string? emergencyphoneNumber_1 { get; set; }
        //[Required(ErrorMessage = "Address Line_1 is required")]
        [DisplayName("Address Line_1")]
        [MaxLength(200)]
        public string? addressLine_1 { get; set; }
        //[Required(ErrorMessage = "Address Line_2 is required")]
        [DisplayName("Address Line_2")]
        [MaxLength(200)]
        public string? addressLine_2 { get; set; }
        //[Required(ErrorMessage = "City is required")]
        [DisplayName("City")]
        public string? city { get; set; }
        //[Required(ErrorMessage = "State is required")]
        [DisplayName("State")]
        [MaxLength(50)]
        public string? state { get; set; }
        //[Required(ErrorMessage = "Country is required")]
        [DisplayName("Country")]
        [MaxLength(50)]
        public string? country { get; set; }
        //[Required(ErrorMessage = "Position is required")]
        //[EmployeeTypeValidation(ErrorMessage = "Select position")]
        [DisplayName("Technology")]
        public int employeeTypeId { get; set; }
        [DisplayName("Technology")]
        public string? employeeType { get; set; }

        //[Required(ErrorMessage = "AadharNumber is required")]
        [DisplayName("Aadhar Number")]
        public string? aadharNumber { get; set; }
        //[Required(ErrorMessage = "PanNumber is required")]
        [DisplayName("Pan Number")]
        public string? panNumber { get; set; }
        //[Required(ErrorMessage = "PostalCode is required")]
        [DisplayName("Postal Code")]
        [MaxLength(7)]
        public string? postalCode { get; set; }
        public int postalCodeVal { get; set; }
        //[Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email.")]
        [MaxLength(50)]
        public string? email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        [MaxLength(8)]
        public string? password { get; set; }
        //[Required(ErrorMessage = "EmployeeImage is required")]
        [DisplayName("Employee Image")]
        public IFormFile? employeeImage { get; set; }
        public string? employeeImageName { get; set; }
        [DisplayName("Aadhar Image")]
        public IFormFile? aadharImage { get; set; }
        public string? aadharImageName { get; set; }
        [DisplayName("Pan Image")]
        public IFormFile? panImage { get; set; }
        public string? panImageName { get; set; }
        public string? status { get; set; }
        [DisplayName("Status")]
        public string? statusId { get; set; }
        [DisplayName("Is Deleted")]
        public bool isdeleted { get; set; }
        [DisplayName("Employeement Date")]
        public DateOnly? employeementDate { get; set; }
        [DisplayName("Probation Period")]
        [RequiredIf("userTypeId == 4", ErrorMessage = "Probation period is required")]
        public Decimal? probationPeriod { get; set; }
        [RequiredIf("status == 'Resign'", ErrorMessage = "Resign date is required")]
        [DisplayName("Resign Date")]
        public string? resignDate { get; set; }
        [RequiredIf("status == 'Resign'", ErrorMessage = "Reason is required")]
        [DisplayName("Reason")]
        public string? resignreason { get; set; }

        [RequiredIf("status == 'Suspend'", ErrorMessage = "Reason is required")]
        [DisplayName("Reason")]
        public string? suspendreason { get; set; }
        [DisplayName("Resign Approval Date")]
        public string? resignApprovalDate { get; set; }
        [DisplayName("Leaving Date")]
        public string? leavingDate { get; set; }
        [DisplayName("Actual Relieving Date")]
        public string? actualRelievingDate { get; set; }
        [RequiredIf("status == 'Suspend'", ErrorMessage = "Suspended date is required")]
        [DisplayName("Suspended Date")]
        public string? suspendedDate { get; set; }
    }
}
