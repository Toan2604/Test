using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSupplier
{
    public interface ISupplierValidator : IServiceScoped
    {
        Task<bool> Create(Supplier Supplier);
        Task<bool> Update(Supplier Supplier);
        Task<bool> Delete(Supplier Supplier);
        Task<bool> BulkDelete(List<Supplier> Suppliers);
        Task<bool> BulkMerge(List<Supplier> Suppliers);
    }

    public class SupplierValidator : ISupplierValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeEmpty,
            CodeHasSpecialCharacter,
            CodeExisted,
            NameEmpty,
            NameExisted,
            StatusNotExisted,
            ProvinceNotExisted,
            DistrictNotExisted,
            WardNotExisted,
            PersonInChargeNotExisted,
            SupplierInUsed,
            NameOverLength
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public SupplierValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        private async Task<bool> ValidateId(Supplier Supplier)
        {
            SupplierFilter SupplierFilter = new SupplierFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Supplier.Id },
                Selects = SupplierSelect.Id
            };

            int count = await UOW.SupplierRepository.Count(SupplierFilter);
            if (count == 0)
                Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateCode(Supplier Supplier)
        {
            if (string.IsNullOrWhiteSpace(Supplier.Code))
            {
                Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Code), ErrorCode.CodeEmpty);
            }
            else
            {
                var Code = Supplier.Code;
                if (Supplier.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Supplier.Code))
                {
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Code), ErrorCode.CodeHasSpecialCharacter);
                }

                SupplierFilter SupplierFilter = new SupplierFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = Supplier.Id },
                    Code = new StringFilter { Equal = Supplier.Code },
                    Selects = SupplierSelect.Code
                };

                int count = await UOW.SupplierRepository.Count(SupplierFilter);
                if (count != 0)
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Code), ErrorCode.CodeExisted);
            }
            return Supplier.IsValidated;
        }

        private async Task<bool> ValidateName(Supplier Supplier)
        {
            if (string.IsNullOrWhiteSpace(Supplier.Name))
            {
                Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Name), ErrorCode.NameEmpty);
            }
            else
            {
                if (Supplier.Name.Length > 255)
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Name), ErrorCode.NameOverLength);
                SupplierFilter SupplierFilter = new SupplierFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = Supplier.Id },
                    Name = new StringFilter { Equal = Supplier.Name },
                    Selects = SupplierSelect.Name
                };

                int count = await UOW.SupplierRepository.Count(SupplierFilter);
                if (count != 0)
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Name), ErrorCode.NameExisted);
            }

            return Supplier.IsValidated;
        }

        private async Task<bool> ValidateProvinceId(Supplier Supplier)
        {
            if (Supplier.ProvinceId != 0)
            {
                ProvinceFilter ProvinceFilter = new ProvinceFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Supplier.ProvinceId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = ProvinceSelect.Id
                };

                int count = await UOW.ProvinceRepository.Count(ProvinceFilter);
                if (count == 0)
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.ProvinceId), ErrorCode.ProvinceNotExisted);
            }
            return Supplier.IsValidated;
        }
        private async Task<bool> ValidateDistrictId(Supplier Supplier)
        {
            if (Supplier.DistrictId != 0)
            {
                DistrictFilter DistrictFilter = new DistrictFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Supplier.DistrictId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = DistrictSelect.Id
                };

                int count = await UOW.DistrictRepository.Count(DistrictFilter);
                if (count == 0)
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.DistrictId), ErrorCode.DistrictNotExisted);
            }
            return Supplier.IsValidated;
        }
        private async Task<bool> ValidateWardId(Supplier Supplier)
        {
            if (Supplier.WardId != 0)
            {
                WardFilter WardFilter = new WardFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Supplier.WardId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = WardSelect.Id
                };

                int count = await UOW.WardRepository.Count(WardFilter);
                if (count == 0)
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.WardId), ErrorCode.WardNotExisted);
            }
            return Supplier.IsValidated;
        }

        private async Task<bool> ValidatePersonInCharge(Supplier Supplier)
        {
            if (Supplier.PersonInChargeId != 0)
            {
                AppUserFilter AppUserFilter = new AppUserFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = Supplier.PersonInChargeId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = AppUserSelect.Id
                };
                int count = await UOW.AppUserRepository.Count(AppUserFilter);
                if (count == 0)
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.PersonInChargeId), ErrorCode.PersonInChargeNotExisted);
            }
            return Supplier.IsValidated;
        }

        private async Task<bool> ValidateStatusId(Supplier Supplier)
        {
            if (StatusEnum.ACTIVE.Id != Supplier.StatusId && StatusEnum.INACTIVE.Id != Supplier.StatusId)
                Supplier.AddError(nameof(SupplierValidator), nameof(Store.Status), ErrorCode.StatusNotExisted);
            return Supplier.IsValidated;
        }

        private async Task<bool> ValidateSuppilerInUsed(Supplier Supplier)
        {
            ProductFilter ProductFilter = new ProductFilter
            {
                SupplierId = new IdFilter { Equal = Supplier.Id },
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
            };
            int count = await UOW.ProductRepository.Count(ProductFilter);
            if (count > 0)
                Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Id), ErrorCode.SupplierInUsed);

            return Supplier.IsValidated;
        }

        public async Task<bool> Create(Supplier Supplier)
        {
            await ValidateCode(Supplier);
            await ValidateName(Supplier);
            await ValidateProvinceId(Supplier);
            await ValidateDistrictId(Supplier);
            await ValidatePersonInCharge(Supplier);
            await ValidateStatusId(Supplier);
            await ValidateWardId(Supplier);
            return Supplier.IsValidated;
        }

        public async Task<bool> Update(Supplier Supplier)
        {
            if (await ValidateId(Supplier))
            {
                await ValidateCode(Supplier);
                await ValidateName(Supplier);
                await ValidateProvinceId(Supplier);
                await ValidateDistrictId(Supplier);
                await ValidatePersonInCharge(Supplier);
                await ValidateStatusId(Supplier);
                await ValidateWardId(Supplier);
            }
            return Supplier.IsValidated;
        }

        public async Task<bool> Delete(Supplier Supplier)
        {
            if (await ValidateId(Supplier))
            {
                await ValidateSuppilerInUsed(Supplier);
            }
            return Supplier.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Supplier> Suppliers)
        {
            SupplierFilter SupplierFilter = new SupplierFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Id = new IdFilter { In = Suppliers.Select(a => a.Id).ToList() },
                Selects = SupplierSelect.Id
            };

            var listInDB = await UOW.SupplierRepository.List(SupplierFilter);
            var listExcept = Suppliers.Except(listInDB);
            if (listExcept == null || listExcept.Count() == 0) return true;
            foreach (var Supplier in listExcept)
            {
                Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Id), ErrorCode.IdNotExisted);
            }
            return false;
        }

        public async Task<bool> BulkMerge(List<Supplier> Suppliers)
        {
            var listCodeInDB = (await UOW.SupplierRepository.List(new SupplierFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = SupplierSelect.Code
            })).Select(e => e.Code);
            var listNameInDB = (await UOW.SupplierRepository.List(new SupplierFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = SupplierSelect.Name
            })).Select(e => e.Name);

            foreach (var Supplier in Suppliers)
            {
                if (listCodeInDB.Contains(Supplier.Code))
                {
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Code), ErrorCode.CodeExisted);
                    return false;
                }

                if (listNameInDB.Contains(Supplier.Name))
                {
                    Supplier.AddError(nameof(SupplierValidator), nameof(Supplier.Name), ErrorCode.NameExisted);
                    return false;
                }
                await ValidatePersonInCharge(Supplier);
                await ValidateProvinceId(Supplier);
                await ValidateDistrictId(Supplier);
                await ValidateWardId(Supplier);
                await ValidateStatusId(Supplier);
            }
            return Suppliers.Any(s => !s.IsValidated) ? false : true;
        }
    }
}
