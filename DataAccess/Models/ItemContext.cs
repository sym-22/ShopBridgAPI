using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    ///     Item context class
    /// </summary>
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {

        }

        /// <summary>
        ///     Items DB set
        /// </summary>
        public virtual DbSet<Item> Items { get; set; } 

        /// <summary>
        ///     Categories Db set
        /// </summary>
        public virtual DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(i => i.AvailableQuantity)
                .HasDefaultValue(1);
        }

    }
}
