using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReportFilter
    {
        [DisplayName("Working Days")]
        public string? Day { get; set; }
        public int MonthId { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date")]
        public DateTime? ReportDate { get; set; }

    }
}
