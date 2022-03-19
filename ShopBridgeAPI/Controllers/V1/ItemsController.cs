using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeAPI.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Controllers.V1
{
    [Route("v1.0/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IItemDataAccess _itemDataAccess;
        public ItemsController(IItemDataAccess itemDataAccess)
        {
            _itemDataAccess = itemDataAccess;            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            List<Item> itemList = await _itemDataAccess.GetAllItems();
            return StatusCode(Convert.ToInt32(HttpStatusCode.OK), itemList);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddItem(Item item)
        {
            Item createdItem = await _itemDataAccess.AddItem(item);
            return Created(string.Empty, createdItem);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditItem(Item item)
        {
            if (!await _itemDataAccess.DoesItemExist(item.ItemId))
                return NotFound(string.Format(Constants.ITEM_NOT_EXIST_MESSAGE, item.ItemId));

            Item editedItem = await _itemDataAccess.EditItem(item);
            return Ok(editedItem);
        }

        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> DeleteItem([Required] int itemId)
        {
            if (!await _itemDataAccess.DoesItemExist(itemId))
                return NotFound(string.Format(Constants.ITEM_NOT_EXIST_MESSAGE, itemId));

            await _itemDataAccess.DeleteItem(itemId);
            return NoContent();
        }
    }
}
