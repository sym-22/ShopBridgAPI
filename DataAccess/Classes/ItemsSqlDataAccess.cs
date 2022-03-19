using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Classes
{
    public class ItemsSqlDataAccess : IItemDataAccess
    {
        private IDbContextFactory<ItemContext> _dbContextFactory;

        public ItemsSqlDataAccess(IDbContextFactory<ItemContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task<Item> AddItem(Item item)
        {
            using (ItemContext context = _dbContextFactory.CreateDbContext())
            {
                await context.Items.AddAsync(item);
                await context .SaveChangesAsync();
                return item;
            }
        }

        public async Task DeleteItem(int itemId)
        {
            using (ItemContext context = _dbContextFactory.CreateDbContext())
            {
                context.Items.Remove(context.Items.Find(itemId));
                await context.SaveChangesAsync();
            }
        }

        public async Task<Item> EditItem(Item item)
        {
            using (ItemContext context = _dbContextFactory.CreateDbContext())
            {
                context.Items.Update(item);
                await context.SaveChangesAsync();
                return item;
            }
        }

        public async Task<List<Item>> GetAllItems()
        {
            using (ItemContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Items.ToListAsync();
            }
        }

        public async Task<bool> DoesItemExist(int itemId)
        {
            using (ItemContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.Items.AnyAsync(i => i.ItemId.Equals(itemId));
            }
        }
    }
}
