using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LaunchDateModel
    {
        [Required(ErrorMessage = "Launch Date is required")]
        [DisplayName("Launch Date")]
        public string? launchdate { get; set; }
    }
}
