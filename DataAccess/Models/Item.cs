using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [MaxLength(20, ErrorMessage = "Item name can not exceed 20 characters")]
        [Required(ErrorMessage = "Item name is required")]
        public string ItemName { get; set; }

        [MaxLength(100, ErrorMessage = "Item description cannot exceed 100 characters")]
        public string ItemDescription { get; set; }

        [Required(ErrorMessage = "Item price is required")]
        public decimal ItemPrice { get; set; }
    }
}
