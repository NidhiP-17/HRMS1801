using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HolidayModel
    {
        public int holidayId { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTime date { get; set; }
        public string? holidaydate { get; set; }
        public DateOnly? datelist { get; set; }
        [DisplayName("Day")]
        public string day { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Type")]
        public string type  { get; set; }
        [DisplayName("Hours")]
        public decimal? hours  { get; set; }
        public decimal? hourslist  { get; set; }
    }
}
