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

        ///// <summary>
        /////     Parameterless constructor created for nunit test
        ///// </summary>
        //public ItemContext()
        //{

        //}

        /// <summary>
        ///     Items DB set
        /// </summary>
        public virtual DbSet<Item> Items { get; set; } //Setting as virtual so that mock can override

    }
}
