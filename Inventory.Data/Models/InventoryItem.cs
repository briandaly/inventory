using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory.Data.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}