using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TaskModel
    {
        public int taskId { get; set; }
        [Required(ErrorMessage = "ProjectName is required")]
        [DisplayName("Project Name")]
        public int projectId { get; set; }
        [DisplayName("Project Name")]
        public string? projectName { get; set; }
        [Required(ErrorMessage = "EmployeeName is required")]
        [DisplayName("Employee Name")]
        public int employeeId { get; set; }
        [DisplayName("Employee Name")]
        public string? employeeName { get; set; }
        [Required(ErrorMessage = "TaskName is required")]
        [DisplayName("Task Name")]
        public string? taskName { get; set; }
        [DefaultValue("Pending")]
        [DisplayName("Status")]
        public string? status { get; set; }
        //public int statusId { get; set; }
        [DisplayName("Assigned Date")]
        public DateTime createdDate { get; set; }
        public bool isDeleted { get; set; }
    }


}
