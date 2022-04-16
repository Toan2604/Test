using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS;
using DMS.Common;
using DMS.Enums;
using DMS.Entities;
using DMS.Repositories;

namespace DMS.Services.MInventory
{
    public interface IInventoryValidator : IServiceScoped
    {
        Task<bool> Create(Inventory Inventory);
        Task<bool> Update(Inventory Inventory);
    }

    public class InventoryValidator : IInventoryValidator
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private InventoryMessage InventoryMessage;

        public InventoryValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.InventoryMessage = new InventoryMessage();
        }

        public async Task Get(Inventory Inventory)
        {
        }

        public async Task<bool> Create(Inventory Inventory)
        {
            await ValidateSaleStock(Inventory);
            await ValidateAccountingStock(Inventory);
            await ValidateItem(Inventory);
            await ValidateWarehouse(Inventory);
            return Inventory.IsValidated;
        }

        public async Task<bool> Update(Inventory Inventory)
        {
            if (await ValidateId(Inventory))
            {
                await ValidateSaleStock(Inventory);
                await ValidateAccountingStock(Inventory);
                await ValidateItem(Inventory);
                await ValidateWarehouse(Inventory);
            }
            return Inventory.IsValidated;
        }

        public async Task<bool> Delete(Inventory Inventory)
        {
            if (await ValidateId(Inventory))
            {
            }
            return Inventory.IsValidated;
        }
        
        public async Task<bool> BulkDelete(List<Inventory> Inventories)
        {
            foreach (Inventory Inventory in Inventories)
            {
                await Delete(Inventory);
            }
            return Inventories.All(x => x.IsValidated);
        }
        
        public async Task<bool> Import(List<Inventory> Inventories)
        {
            return true;
        }
        
        private async Task<bool> ValidateId(Inventory Inventory)
        {
            InventoryFilter InventoryFilter = new InventoryFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Inventory.Id },
                Selects = InventorySelect.Id
            };

            int count = await UOW.InventoryRepository.CountAll(InventoryFilter);
            if (count == 0)
                Inventory.AddError(nameof(InventoryValidator), nameof(Inventory.Id), InventoryMessage.Error.IdNotExisted, InventoryMessage);
            return Inventory.IsValidated;
        }

        private async Task<bool> ValidateSaleStock(Inventory Inventory)
        {   
            return true;
        }
        private async Task<bool> ValidateAccountingStock(Inventory Inventory)
        {   
            return true;
        }
        private async Task<bool> ValidateItem(Inventory Inventory)
        {       
            if(Inventory.ItemId == 0)
            {
                Inventory.AddError(nameof(InventoryValidator), nameof(Inventory.Item), InventoryMessage.Error.ItemEmpty, InventoryMessage);
            }
            else
            {
                int count = await UOW.ItemRepository.CountAll(new ItemFilter
                {
                    Id = new IdFilter{ Equal =  Inventory.ItemId },
                });
                if(count == 0)
                {
                    Inventory.AddError(nameof(InventoryValidator), nameof(Inventory.Item), InventoryMessage.Error.ItemNotExisted, InventoryMessage);
                }
            }
            return true;
        }
        private async Task<bool> ValidateWarehouse(Inventory Inventory)
        {       
            if(Inventory.WarehouseId == 0)
            {
                Inventory.AddError(nameof(InventoryValidator), nameof(Inventory.Warehouse), InventoryMessage.Error.WarehouseEmpty, InventoryMessage);
            }
            else
            {
                int count = await UOW.WarehouseRepository.CountAll(new WarehouseFilter
                {
                    Id = new IdFilter{ Equal =  Inventory.WarehouseId },
                });
                if(count == 0)
                {
                    Inventory.AddError(nameof(InventoryValidator), nameof(Inventory.Warehouse), InventoryMessage.Error.WarehouseNotExisted, InventoryMessage);
                }
            }
            return true;
        }
    }
}
