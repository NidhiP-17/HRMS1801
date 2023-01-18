using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ItemCategoryModel
    {
        private string mstrModule = "Model=>ItemCategory";
        [Key]
        public int ItemCatID { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [DisplayName("Category Name")]
        public string? ItemCatName { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }

    }
}
