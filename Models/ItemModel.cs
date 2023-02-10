using Microsoft.AspNetCore.Http;
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
    public class ItemModel
    {
        public string mstrSource = "";
        private string mstrModule = "Model=>Item";

        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [DisplayName("Name")]
        public string? ItemName { get; set; }

        [DisplayName("Description")]
        public string? ItemDesc { get; set; }
        [DisplayName("Category")]
        public int ItemCatID { get; set; }

        [DisplayName("Category")]
        public string? ItemCatName { get; set; }
        public int Quantity { get; set; }
        public List<ItemCategoryModel>? ItemCategory { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date")]
        public DateTime? ReportDate { get; set; }

        [Key]
        public int ImageId { get; set; }
        public string? Title { get; set; }

        [DisplayName("Image Name")]
        public string? ImageName { get; set; }

        [DisplayName("Upload Image")]
        public IFormFile ImageFile { get; set; }
      
    }
}
