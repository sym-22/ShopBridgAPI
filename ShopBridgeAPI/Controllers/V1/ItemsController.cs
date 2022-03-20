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
        public async Task<IActionResult> GetAllItemsAsync(int pageNumber = 0)
        {
            try
            {
                if (pageNumber < 0)
                    return BadRequest(Constants.INVALID_PAGE_NUMBER_MESSAGE);
                List<Item> itemList = await _itemDataAccess.GetAllItemsAsync(pageNumber);
                return Ok(itemList);
            }
            catch (Exception)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.INTERNAL_SERVER_ERROR_MESSAGE);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddItemAsync(Item item)
        {
            try
            {
                Item createdItem = await _itemDataAccess.AddItemAsync(item);
                return Created(string.Empty, createdItem);
            }
            catch (Exception)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.INTERNAL_SERVER_ERROR_MESSAGE); 
            }
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditItemAsync(Item item)
        {
            try
            {
                if (!await _itemDataAccess.DoesItemExistAsync(item.ItemId))
                    return NotFound(string.Format(Constants.ITEM_NOT_EXIST_MESSAGE, item.ItemId));

                Item editedItem = await _itemDataAccess.EditItemAsync(item);
                return Ok(editedItem);
            }
            catch (Exception)
            { 
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.INTERNAL_SERVER_ERROR_MESSAGE); 
            }
        }

        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> DeleteItemAsync([Required] int itemId)
        {
            try
            {
                if (!await _itemDataAccess.DoesItemExistAsync(itemId))
                    return NotFound(string.Format(Constants.ITEM_NOT_EXIST_MESSAGE, itemId));

                await _itemDataAccess.DeleteItemAsync(itemId);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.INTERNAL_SERVER_ERROR_MESSAGE);
            }
        }
    }
}
