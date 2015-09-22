using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory;
using Inventory.Controllers;
using Inventory.Data.Repositories;
using Inventory.Data.Models;
using System.Web.Http.Results;
using System.Web.Http.Hosting;
using System.Web.Http;
using Inventory.Tests.Repositories;
using Inventory.Controllers.API;

namespace Inventory.Tests.Controllers
{
    [TestClass]
    public class InventoryControllerTest
    {
        InventoryItem item1 = null;
        InventoryItem item2 = null;
        InventoryItem item3 = null;
        InventoryItem item4 = null;
        List<InventoryItem> items = null;

        InventoryController inventoryController = null;
        DummyInventoryRepository dummyInventoryRepository = null;

        public InventoryControllerTest()
        {
            item1 = new InventoryItem() { Id = 1, ExpirationDate = DateTime.Now.AddDays(1), Label = "item1" };
            item2 = new InventoryItem() { Id = 2, ExpirationDate = DateTime.Now.AddDays(2), Label = "item2" };
            item3 = new InventoryItem() { Id = 3, ExpirationDate = DateTime.Now.AddDays(3), Label = "item3" };
            item4 = new InventoryItem() { Id = 4, ExpirationDate = DateTime.Now.AddDays(4), Label = "item4" };

            items = new List<InventoryItem>() { item1, item2, item3 };

            dummyInventoryRepository = new DummyInventoryRepository(items);
            inventoryController = new InventoryController(dummyInventoryRepository);
        }

        [TestMethod]
        public void GetAllItems_ShouldReturnAllItems()
        {
            var model = inventoryController.Get().ToList();

            Assert.AreEqual(items.Count, model.Count);
        }

        [TestMethod]
        public void GetAllItems_ShouldReturnCorrectItems()
        {
            var model = inventoryController.Get().ToList();

            CollectionAssert.Contains(model, item1);
            CollectionAssert.Contains(model, item2);
            CollectionAssert.Contains(model, item3);

            CollectionAssert.DoesNotContain(model, item4);
        }

        [TestMethod]
        public void GetItem_ShouldReturnCorrectItem()
        {
            var result = inventoryController.Get(1) as OkNegotiatedContentResult<InventoryItem>;
            Assert.IsNotNull(result);
            Assert.AreEqual(item1, result.Content);
        }

        [TestMethod]
        public void GetItem_ShouldNotFindItem()
        {
            var result = inventoryController.Get(999);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void CreateItem_ShouldCreateAndBePresent()
        {
            InventoryItem item5 = new InventoryItem() { Id = 5, ExpirationDate = DateTime.Now.AddDays(5), Label = "item5" };

            inventoryController.Request = new HttpRequestMessage();
            inventoryController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            inventoryController.Post(item5);

            // get the list of books
            List<InventoryItem> items = inventoryController.Get().ToList();

            CollectionAssert.Contains(items, item5);
        }

        [TestMethod]
        public void UpdateItem_ShouldUpdate()
        {
            var result = inventoryController.Get(1) as OkNegotiatedContentResult<InventoryItem>;
            InventoryItem item = result.Content;
            Assert.AreEqual(item.Label, "item1");

            item.Label = "item1_1";
            inventoryController.Put(1, item);

            result = inventoryController.Get(1) as OkNegotiatedContentResult<InventoryItem>;
            item = result.Content;
            Assert.AreEqual(item.Label, "item1_1");
        }

        [TestMethod]
        public void DeleteItem_ShouldDelete()
        {
            var result = inventoryController.Get(1) as OkNegotiatedContentResult<InventoryItem>;
            Assert.IsNotNull(result);

            inventoryController.Delete(1);

            result = inventoryController.Get(1) as OkNegotiatedContentResult<InventoryItem>;
            Assert.IsNull(result);
        }
    }
}
