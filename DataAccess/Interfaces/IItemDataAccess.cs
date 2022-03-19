using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IItemDataAccess
    {
        public Task<List<Item>> GetAllItems();
        public Task<Item> AddItem(Item item);
        public Task<Item> EditItem(Item item);
        public Task DeleteItem(int itemId);
        public Task<bool> DoesItemExist(int employeeId);
    }
}
