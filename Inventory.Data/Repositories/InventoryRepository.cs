using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Inventory.Data.Models;

namespace Inventory.Data.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<InventoryItem> _db { get; set; }

        public InventoryRepository()
        {
            if (HttpRuntime.Cache["_db"] != null)
            {
                _db = (List<InventoryItem>)HttpRuntime.Cache["_db"];
            }
            else
            {
                initDb();
            }
        }

        public InventoryItem Get(int id)
        {
            return _db.FirstOrDefault(d => d.Id == id);
        }

        public IEnumerable<InventoryItem> GetAll()
        {
            return _db.ToList().OrderByDescending(i => i.Id);
        }

        public void Add(InventoryItem inventoryItem)
        {
            _db.Add(inventoryItem);
        }

        public void Update(InventoryItem inventoryItem)
        {
            foreach (InventoryItem item in _db)
            {
                if (item.Id == inventoryItem.Id)
                {
                    _db.Remove(item);
                    _db.Add(inventoryItem);
                    break;
                }
            }
        }

        public void Delete(int inventoryItemId)
        {
            _db.Remove(Get(inventoryItemId));
        }

        public void SaveChanges()
        {
            HttpRuntime.Cache["_db"] = _db;
        }

        private void initDb()
        {
            _db = new List<InventoryItem>();

            DateTime dt = DateTime.Today;

            Add(new InventoryItem() { Id = 1, Label = "Item 1", ExpirationDate = new DateTime(dt.Year, dt.Month, dt.Day).AddDays(1) });
            Add(new InventoryItem() { Id = 2, Label = "Item 2", ExpirationDate = new DateTime(dt.Year, dt.Month, dt.Day).AddDays(2) });
            Add(new InventoryItem() { Id = 3, Label = "Item 3", ExpirationDate = new DateTime(dt.Year, dt.Month, dt.Day).AddDays(3) });
        }
    }
}
