using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    /// <summary>
    ///     Items model class with all properties, used to generate DB table 
    /// </summary>
    public class Item
    {
        /// <summary>
        ///     Id if item, primary key, auto generated.
        /// </summary>
        [Key]
        public int ItemId { get; set; }

        /// <summary>
        ///     Name of item - required - can be max 20 characters
        /// </summary>
        [MaxLength(20, ErrorMessage = "Item name can not exceed 20 characters")]
        [Required(ErrorMessage = "Item name is required")]
        public string ItemName { get; set; }

        /// <summary>
        ///     Description of item - can be max 100 characters
        /// </summary>
        [MaxLength(100, ErrorMessage = "Item description cannot exceed 100 characters")]
        public string ItemDescription { get; set; }

        /// <summary>
        ///     Item price - required
        /// </summary>
        [Required(ErrorMessage = "Item price is required")]
        public decimal ItemPrice { get; set; }
    }
}
