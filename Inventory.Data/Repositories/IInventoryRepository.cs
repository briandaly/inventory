using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Data.Models;

namespace Inventory.Data.Repositories
{
    public interface IInventoryRepository
    {
        InventoryItem Get(int id);
        IEnumerable<InventoryItem> GetAll();
        void Add(InventoryItem inventoryItem);
        void Update(InventoryItem inventoryItem);
        void Delete(int inventoryItemId);
        void SaveChanges();
    }
}
