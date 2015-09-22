using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Inventory.Data.Models;
using Inventory.Data.Repositories;

namespace Inventory.Tests.Repositories
{
    public class DummyInventoryRepository : IInventoryRepository
    {
        private List<InventoryItem> _db { get; set; }

        public DummyInventoryRepository(List<InventoryItem> items)
        {
            _db = items;
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
            // Not required for tests
        }
    }
}
