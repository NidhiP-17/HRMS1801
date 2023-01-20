using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressiveAnnotations.Attributes;

namespace Models
{
    public partial class TimesheetModel
    {
        public int timesheetId { get; set; }
        public int? reasonId { get; set; }
        [Required(ErrorMessage = "Employee is required")]
        [DisplayName("Employee")]
        public int? employeeId { get; set; }
        [DisplayName("Project")]
        [RequiredIf("employeeId != 0", ErrorMessage = "Project is required")]
        public int? projectId { get; set; }
        [DisplayName("Project")]
        public string? projectName { get; set; }
        [DisplayName("Employee")]
        public string? employeeName { get; set; }
        [DisplayName("Task")]
        [RequiredIf("projectId != 0", ErrorMessage = "Task is required")]
        public int? taskId { get; set; }
        [DisplayName("Task")]
        public string? taskName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Required(ErrorMessage = "Date is required")]
        [DisplayName("Date")]
        public string? date { get; set; }
        [DisplayName("Select Any")]
        public int? trackerFlag { get; set; }
        public string? trackerFlagVal { get; set; }
        [DisplayName("Type")]
        public string? type { get; set; }
        [RequiredIf("projectId != 0", ErrorMessage = "Hours is required")]
        [Range(0, 16, ErrorMessage = "Enter number between 0 to 16")]
        [DisplayName("Hours")]
        public decimal? hours { get; set; }
        [DisplayName("Reason Code")]
        public string? reasonCode { get; set; }
        [DisplayName("Notes")]
        [RequiredIf("projectId != 0", ErrorMessage = "Notes is required")]
        public string? notes { get; set; }
        [DisplayName("Notes")]
        [RequiredIf("reasonCode != '0'", ErrorMessage = "Leave Note is required")]
        public string? leavenotes { get; set; }
        [DisplayName("Hours")]
        [Range(0, 8.76, ErrorMessage = "Enter number between 0 to 8.76")]
        [RequiredIf("reasonCode != '0'", ErrorMessage = "Leave Hours is required")]
        public decimal? leavehours { get; set; }
        public bool isDeleted { get; set; }
        [DisplayName("Created Date")]
        public string? createdDate { get; set; }
        public string? holidaydate { get; set; }
        public string? isWorkingDay { get; set; }
        public string? daterange { get; set; }

        public IEnumerable<Models.TimesheetModel>? TimeSheetList { get; set; }

    }
}
