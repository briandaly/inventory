using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Inventory.Data.Models;
using Inventory.Data.Repositories;

namespace Inventory
{
    public class InventoryCheck
    {
        public static void StartCheckingForExpiredItems()
        {
            var thread = new Thread(new ThreadStart(StartJob));
            thread.IsBackground = true;
            thread.Name = "InventoryCheck.ExpiredItems";
            thread.Start();
        }

        private static void StartJob()
        {
            var logChecker = new InventoryChecker();
            var timer = new System.Timers.Timer();
            timer.Interval = 30000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(logChecker.CheckForExpiredItems);
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        private class InventoryChecker
        {
            internal void CheckForExpiredItems(object sender, System.Timers.ElapsedEventArgs e)
            {
                InventoryRepository inventoryRepository = new InventoryRepository();

                var items = inventoryRepository.GetAll();
                foreach (var item in items)
                {
                    if (item.ExpirationDate <= DateTime.Now)
                    {
                        NotificationHub hub = new NotificationHub();
                        hub.Send("BGJob", "Expired Item: " + item.Label + ". Expired on: " + item.ExpirationDate.ToString());
                    }
                }
            }
        }
    }
}