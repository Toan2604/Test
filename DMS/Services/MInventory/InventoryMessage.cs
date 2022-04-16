using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.MInventory
{
    public class InventoryMessage
    {
        public enum Information
        {

        }

        public enum Warning
        {

        }

        public enum Error
        {
            IdNotExisted,
            ItemEmpty,
            ItemNotExisted,
            WarehouseEmpty,
            WarehouseNotExisted,
        }
    }
}
