using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TechnologyModel
    {
        public int technologyId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        public string? Name { get; set; } 
    }
}
