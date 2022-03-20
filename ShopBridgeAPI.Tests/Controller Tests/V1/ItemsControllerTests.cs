using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShopBridgeAPI.Configuration;
using ShopBridgeAPI.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Tests
{
    [TestFixture]
    public class ItemsControllerTests
    {
        private Mock<IItemDataAccess> _itemDataAccess;

        [SetUp]
        public void Setup()
        {
            _itemDataAccess = new Mock<IItemDataAccess>();
        }

        [Test]
        public void GetAllItemsTest_Ok()
        {
            List<Item> itemList = GetDefaultItems();

            _itemDataAccess.Setup(x => x.GetAllItemsAsync()).Returns(Task.FromResult(itemList));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.GetAllItemsAsync().Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.OK), result.StatusCode);
        }

        [Test]
        public void GetAllItemsTest_InternalServerError()
        {
            List<Item> itemList = GetDefaultItems();

            _itemDataAccess.Setup(x => x.GetAllItemsAsync()).ThrowsAsync(new Exception(message: "error"));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.GetAllItemsAsync().Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.InternalServerError), result.StatusCode);
            Assert.AreEqual(Constants.INTERNAL_SERVER_ERROR_MESSAGE, result.Value);
        }

        [Test]
        public void AddItemTest_Created()
        {
            List<Item> addedList = new();
            Item itemToadd = new Item() { ItemId = 33, ItemName = "added", ItemPrice = 44 };
            _itemDataAccess.Setup(x => x.AddItemAsync(It.IsAny<Item>()))
                                       .Callback((Item addedItem) => addedList.Add(addedItem));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.AddItemAsync(itemToadd).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.Created), result.StatusCode);
            Assert.AreEqual(addedList.Count, 1);
            Assert.AreEqual(addedList[0].ItemName, "added");
        }

        [Test]
        public void AddItemTest_InternalServerError()
        {

            _itemDataAccess.Setup(x => x.AddItemAsync(It.IsAny<Item>())).ThrowsAsync(new Exception(message: Constants.INTERNAL_SERVER_ERROR_MESSAGE));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.AddItemAsync(It.IsAny<Item>()).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.InternalServerError), result.StatusCode);
            Assert.AreEqual(Constants.INTERNAL_SERVER_ERROR_MESSAGE, result.Value);
        }

        [Test]
        public void EditItemTest_Ok()
        {
            List<Item> itemList = GetDefaultItems();
            Item modifiedItem = null;
            Item existingItem = new Item { ItemId = 1, ItemName = "modified", ItemDescription = "desc 1", ItemPrice = 55 };

            _itemDataAccess.Setup(x => x.DoesItemExistAsync(existingItem.ItemId))
                                        .Returns(Task.FromResult(itemList.FindIndex(x => x.ItemId == existingItem.ItemId) > -1));
            _itemDataAccess.Setup(x => x.EditItemAsync(It.IsAny<Item>()))
                                .Callback((Item item) => modifiedItem = item);

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.EditItemAsync(existingItem).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.OK), result.StatusCode);
            Assert.IsNotNull(modifiedItem);
            Assert.AreEqual(existingItem.ItemName, modifiedItem.ItemName);
        }

        [Test]
        public void EditItemTest_NotFound()
        {
            List<Item> itemList = GetDefaultItems();
            Item modifiedItem = null;
            Item existingItem = new Item { ItemId = 122, ItemName = "modified", ItemDescription = "desc 1", ItemPrice = 55 };

            _itemDataAccess.Setup(x => x.DoesItemExistAsync(existingItem.ItemId))
                                        .Returns(Task.FromResult(itemList.FindIndex(x => x.ItemId == existingItem.ItemId) > -1));
            _itemDataAccess.Setup(x => x.EditItemAsync(It.IsAny<Item>()))
                                .Callback((Item item) => modifiedItem = item);

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.EditItemAsync(existingItem).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.NotFound), result.StatusCode);
            Assert.AreEqual(string.Format(Constants.ITEM_NOT_EXIST_MESSAGE, existingItem.ItemId), result.Value);

        }

        [Test]
        public void EditItemTest_InternalServerError()
        {

            _itemDataAccess.Setup(x => x.EditItemAsync(It.IsAny<Item>()))
                            .ThrowsAsync(new Exception(message: Constants.INTERNAL_SERVER_ERROR_MESSAGE));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.EditItemAsync(It.IsAny<Item>()).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.InternalServerError), result.StatusCode);
            Assert.AreEqual(Constants.INTERNAL_SERVER_ERROR_MESSAGE, result.Value);
        }

        [Test]
        public void DeleteItemTest_NoContent()
        {
            List<Item> itemList = GetDefaultItems();
            Item existingItem = new Item { ItemId = 1, ItemName = "test item1", ItemDescription = "desc 1", ItemPrice = 55 };

            _itemDataAccess.Setup(x => x.DoesItemExistAsync(existingItem.ItemId))
                                        .Returns(Task.FromResult(itemList.FindIndex(x => x.ItemId == existingItem.ItemId) > -1));

            _itemDataAccess.Setup(x => x.DeleteItemAsync(It.IsAny<int>()))
                                .Callback((int itemId) => itemList.RemoveAll(x => x.ItemId == existingItem.ItemId));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.DeleteItemAsync(existingItem.ItemId).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(2, itemList.Count);
        }

        [Test]
        public void DeleteItemTest_NotFound()
        {
            List<Item> itemList = GetDefaultItems();
            Item existingItem = new Item { ItemId = 122, ItemName = "modified", ItemDescription = "desc 1", ItemPrice = 55 };

            _itemDataAccess.Setup(x => x.DoesItemExistAsync(existingItem.ItemId))
                                        .Returns(Task.FromResult(itemList.FindIndex(x => x.ItemId == existingItem.ItemId) > -1));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.DeleteItemAsync(existingItem.ItemId).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.NotFound), result.StatusCode);
            Assert.AreEqual(string.Format(Constants.ITEM_NOT_EXIST_MESSAGE, existingItem.ItemId), result.Value);

        }

        [Test]
        public void DeleteItemTest_InternalServerError()
        {
            _itemDataAccess.Setup(x => x.DoesItemExistAsync(It.IsAny<int>())).Returns(Task.FromResult(true));
            _itemDataAccess.Setup(x => x.DeleteItemAsync(It.IsAny<int>()))
                            .ThrowsAsync(new Exception(message: Constants.INTERNAL_SERVER_ERROR_MESSAGE));

            ItemsController itemsController = new ItemsController(_itemDataAccess.Object);
            IActionResult actionResult = itemsController.DeleteItemAsync(It.IsAny<int>()).Result;
            var result = actionResult as ObjectResult;

            Assert.AreEqual(Convert.ToInt32(HttpStatusCode.InternalServerError), result.StatusCode);
            Assert.AreEqual(Constants.INTERNAL_SERVER_ERROR_MESSAGE, result.Value);
        }

        private List<Item> GetDefaultItems()
        {
            Item item1 = new Item { ItemId = 1, ItemName = "test item1", ItemDescription = "desc 1", ItemPrice = 55 };
            Item item2 = new Item { ItemId = 2, ItemName = "test item2", ItemDescription = "desc 2", ItemPrice = 48 };
            Item item3 = new Item { ItemId = 3, ItemName = "test item3", ItemDescription = "desc 3", ItemPrice = 47 };

            var itemList = new List<Item> { item1, item2, item3 };

            return itemList;

        }
    }
}