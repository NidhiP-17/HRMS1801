using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProjectModel
    {
        public int projectId { get; set; }
        [DisplayName("Project Code")]
        public string? projectCode { get; set; }
        [Required(ErrorMessage = "ProjectName is required")]
        [DisplayName("Project Name")]
        public string? projectName { get; set; }
        public string? employeeName { get; set; }
        [Required(ErrorMessage = "Technology is required")]
        [DisplayName("Technology")]
        public string? technology { get; set; }
        public string[] technologies { get; set; }
        public string? technologiesname { get; set; }
        [Required(ErrorMessage = "ClientName is required")]
        [DisplayName("Client Name")]
        public string? clientName { get; set; }
        [DefaultValue("Pending")]
        [DisplayName("Status")]
        public string? status { get; set; }
        [Required(ErrorMessage = "StartDate is required")]
        [DisplayName("Start Date")]
        public string? startDate { get; set; }
        public bool isDeleted { get; set; }
    }
}
