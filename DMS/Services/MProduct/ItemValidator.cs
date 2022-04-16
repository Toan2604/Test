using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MProduct
{
    public interface IItemValidator : IServiceScoped
    {
        Task<bool> Create(Item Item);
        Task<bool> Update(Item Item);
        Task<bool> Delete(Item Item);
        Task<bool> BulkDelete(List<Item> Items);
        Task<bool> Import(List<Item> Items);
    }

    public class ItemValidator : IItemValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            ProductNotExisted,
            NameEmpty,
            NameOverLength,
            CodeEmpty,
            CodeOverLength,
            ScanCodeEmpty,
            ScanCodeOverLength,
            ItemHasOrder
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ItemValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Item Item)
        {
            ItemFilter ItemFilter = new ItemFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Item.Id },
                Selects = ItemSelect.Id
            };

            int count = await UOW.ItemRepository.Count(ItemFilter);
            if (count == 0)
                Item.AddError(nameof(ItemValidator), nameof(Item.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }
        public async Task<bool> ValidateName(Item Item)
        {
            if (string.IsNullOrWhiteSpace(Item.Name))
            {
                Item.AddError(nameof(ItemValidator), nameof(Item.Name), ErrorCode.NameEmpty);
            }
            else if (Item.Name.Length > 3000)
            {
                Item.AddError(nameof(ItemValidator), nameof(Item.Name), ErrorCode.NameOverLength);
            }
            return true;
        }
        public async Task<bool> ValidateCode(Item Item)
        {
            if (string.IsNullOrWhiteSpace(Item.Code))
            {
                Item.AddError(nameof(ItemValidator), nameof(Item.Code), ErrorCode.CodeEmpty);
            }
            else if (Item.Code.Length > 4000)
            {
                Item.AddError(nameof(ItemValidator), nameof(Item.Code), ErrorCode.CodeOverLength);
            }
            return true;
        }
        public async Task<bool> ValidateScanCode(Item Item)
        {
            if (string.IsNullOrWhiteSpace(Item.ScanCode))
            {
                Item.AddError(nameof(ItemValidator), nameof(Item.ScanCode), ErrorCode.ScanCodeEmpty);
            }
            else if (Item.ScanCode.Length > 4000)
            {
                Item.AddError(nameof(ItemValidator), nameof(Item.ScanCode), ErrorCode.ScanCodeOverLength);
            }
            return true;
        }

        public async Task<bool> Create(Item Item)
        {
            return Item.IsValidated;
        }

        public async Task<bool> Update(Item Item)
        {
            if (await ValidateId(Item))
            {
            }
            return Item.IsValidated;
        }

        public async Task<bool> Delete(Item Item)
        {
            if (await ValidateId(Item))
            {
            }
            return Item.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Item> Items)
        {
            return true;
        }

        public async Task<bool> Import(List<Item> Items)
        {
            foreach (var Item in Items)
            {
                await ValidateName(Item);
                await ValidateCode(Item);
                await ValidateScanCode(Item);
                if (!Item.IsValidated) return false;
                if (Item.ProductId == 0)
                {
                    Item.AddError(nameof(ItemValidator), nameof(Item.ProductId), ErrorCode.ProductNotExisted);
                    return false;
                }
            }
            return true;
        }
    }
}
