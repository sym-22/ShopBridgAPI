using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    /// <summary>
    ///     Interface having methods for data repo CRUD operations
    /// </summary>
    public interface IItemDataAccess
    {
        /// <summary>
        ///     Lists all items present in Database
        /// </summary>
        /// <returns>Lit of item objects</returns>
        public Task<List<Item>> GetAllItemsAsync();

        /// <summary>
        ///     Adds given item to database
        /// </summary>
        /// <param name="item">Item object to be added</param>
        /// <returns>Added item</returns>
        public Task<Item> AddItemAsync(Item item);

        /// <summary>
        ///     modify an item
        /// </summary>
        /// <param name="item">item object to be modified</param>
        /// <returns>modified item object</returns>
        public Task<Item> EditItemAsync(Item item);

        /// <summary>
        ///     Delete an item with given id
        /// </summary>
        /// <param name="itemId">id of item to be deleted</param>
        /// <returns></returns>
        public Task DeleteItemAsync(int itemId);

        /// <summary>
        ///     Checks if item with given id exists in database
        /// </summary>
        /// <param name="itemId">id of item</param>
        /// <returns>true if item with given id exists else false</returns>
        public Task<bool> DoesItemExistAsync(int employeeId);
    }
}
