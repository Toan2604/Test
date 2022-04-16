using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MStoreChecking
{
    public interface IStoreCheckingValidator : IServiceScoped
    {
        Task<bool> CheckIn(StoreChecking StoreChecking);
        Task<bool> Update(StoreChecking StoreChecking);
        Task<bool> CheckOut(StoreChecking StoreChecking);
        Task<bool> Delete(StoreChecking StoreChecking);
    }

    public class StoreCheckingValidator : IStoreCheckingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            StoreNotExisted,
            DistanceOutOfRange,
            HasCheckOut,
            HasCheckin,
            LocationEmpty,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreCheckingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreChecking StoreChecking)
        {
            StoreCheckingFilter StoreCheckingFilter = new StoreCheckingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreChecking.Id },
                Selects = StoreCheckingSelect.Id
            };

            int count = await UOW.StoreCheckingRepository.Count(StoreCheckingFilter);
            if (count == 0)
                StoreChecking.AddError(nameof(StoreCheckingValidator), nameof(StoreChecking.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateGPS(StoreChecking StoreChecking)
        {
            if (StoreChecking.Latitude.HasValue == false || StoreChecking.Longitude.HasValue == false)
            {
                StoreChecking.AddError(nameof(StoreCheckingValidator), nameof(StoreChecking.Latitude), ErrorCode.LocationEmpty);
            }
            return StoreChecking.IsValidated;
        }

        private async Task<bool> ValidateDistance(StoreChecking StoreChecking)
        {
            var Store = await UOW.StoreRepository.Get(StoreChecking.StoreId);
            if (Store == null)
            {
                StoreChecking.AddError(nameof(StoreCheckingValidator), nameof(StoreChecking.Store), ErrorCode.StoreNotExisted);
            }
            else
            {
                var Distance = Math.Sqrt(Math.Pow(Convert.ToDouble(Store.Longitude - StoreChecking.Longitude), 2)
                           + Math.Pow(Convert.ToDouble(Store.Latitude - StoreChecking.Latitude), 2));

                if (Distance > 100)
                {
                    StoreChecking.AddError(nameof(StoreCheckingValidator), nameof(StoreChecking.Store), ErrorCode.DistanceOutOfRange);
                }
            }

            return StoreChecking.IsValidated;
        }

        public async Task<bool> CheckIn(StoreChecking StoreChecking)
        {
            await ValidateGPS(StoreChecking);
            await ValidateDistance(StoreChecking);
            return StoreChecking.IsValidated;
        }

        public async Task<bool> Update(StoreChecking StoreChecking)
        {
            if (await ValidateId(StoreChecking))
            {
                var oldData = await UOW.StoreCheckingRepository.Get(StoreChecking.Id);
                if (oldData.CheckOutAt.HasValue)
                    StoreChecking.AddError(nameof(StoreCheckingValidator), nameof(StoreChecking.CheckOutAt), ErrorCode.HasCheckOut);
            }
            return StoreChecking.IsValidated;
        }

        public async Task<bool> CheckOut(StoreChecking StoreChecking)
        {
            if (await ValidateId(StoreChecking))
            {
                await ValidateGPS(StoreChecking);
            }
            return StoreChecking.IsValidated;
        }


        public async Task<bool> Delete(StoreChecking StoreChecking)
        {
            if (await ValidateId(StoreChecking))
            {
            }
            return StoreChecking.IsValidated;
        }
    }
}
