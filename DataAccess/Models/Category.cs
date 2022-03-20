using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Category
    {
        /// <summary>
        ///     category id - Primary key, auto generated
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        ///     Name of category - can be max 20 characters.
        /// </summary>
        [MaxLength(20, ErrorMessage = "category name cannot exceed 20 characters")]
        public string CategoryName { get; set; }
    }
}
