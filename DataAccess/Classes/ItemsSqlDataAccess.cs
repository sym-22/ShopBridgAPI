using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Classes
{
    /// <summary>
    ///     Concrete class containing data repo methods for CRUD operations.
    /// </summary>
    public class ItemsSqlDataAccess : IItemDataAccess
    {
        private readonly IDbContextFactory<ItemContext> _dbContextFactory;
        private readonly IConfiguration _configuration;

        public ItemsSqlDataAccess(IDbContextFactory<ItemContext> dbContextFactory, IConfiguration configuration)
        {
            _dbContextFactory = dbContextFactory;
            _configuration = configuration;
        }

        /// <summary>
        ///     Adds given item to database
        /// </summary>
        /// <param name="item">Item object to be added</param>
        /// <returns>Added item</returns>
        public async Task<Item> AddItemAsync(Item item)
        {
            try
            {
                using (ItemContext context = _dbContextFactory.CreateDbContext())
                {
                    Item existingItem =  context.Items.FirstOrDefault(i => i.ItemName == item.ItemName);

                    //If item with given name already exists then increase quantity by 1
                    if (existingItem is not null)
                    {
                        existingItem.AvailableQuantity += 1;
                        item = existingItem;
                    }
                    else
                    {
                        await context.Items.AddAsync(item);
                    }

                    await context.SaveChangesAsync();
                    return item;
                }
            }
            catch (Exception)
            { 
                throw;
            }
        }

        /// <summary>
        ///     Delete an item with given id
        /// </summary>
        /// <param name="itemId">id of item to be deleted</param>
        /// <returns></returns>
        public async Task DeleteItemAsync(int itemId)
        {
            try
            {
                using (ItemContext context = _dbContextFactory.CreateDbContext())
                {
                    Item existingItem =  context.Items.Find(itemId);

                    //If item exists with quantity > 1 then decrease quantity 
                    if (existingItem is not null && existingItem.AvailableQuantity > 1)
                    {
                        existingItem.AvailableQuantity -= 1;
                    }
                    
                    else
                    {
                        context.Items.Remove(existingItem);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            { 
                throw;
            }
        }

        /// <summary>
        ///     modify an item
        /// </summary>
        /// <param name="item">item object to be modified</param>
        /// <returns>modified item object</returns>
        public async Task<Item> EditItemAsync(Item item)
        {
            try
            {
                using (ItemContext context = _dbContextFactory.CreateDbContext())
                {
                    context.Items.Update(item);
                    await context.SaveChangesAsync();
                    return item;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Lists all items present in Database
        /// </summary>
        /// <returns>Lit of item objects</returns>
        public async Task<List<Item>> GetAllItemsAsync(int pageNumber)
        {
            try
            {
                int itemsPerPage = _configuration.GetValue<int>("ItemsPerPage");
                int skip = pageNumber == 0 ? 0 : (pageNumber - 1) * itemsPerPage;
                using (ItemContext context = _dbContextFactory.CreateDbContext())
                {
                    return await context.Items.OrderBy(i => i.ItemName)
                                              .Skip(skip).Take(itemsPerPage)
                                              .ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Checks if item with given id exists in database
        /// </summary>
        /// <param name="itemId">id of item</param>
        /// <returns>true if item with given id exists else false</returns>
        public async Task<bool> DoesItemExistAsync(int itemId)
        {
            try
            {
                using (ItemContext context = _dbContextFactory.CreateDbContext())
                {
                    return await context.Items.AnyAsync(i => i.ItemId.Equals(itemId));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
