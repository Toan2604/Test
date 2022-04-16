using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MBanner
{
    public interface IBannerValidator : IServiceScoped
    {
        Task<bool> Create(Banner Banner);
        Task<bool> Update(Banner Banner);
        Task<bool> Delete(Banner Banner);
        Task<bool> BulkDelete(List<Banner> Banners);
        Task<bool> Import(List<Banner> Banners);
    }

    public class BannerValidator : IBannerValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            TitleEmpty,
            ImageEmpty,
            TitleOverLength,
            ContentOverLength,
            PriorityInvalid,
            StatusNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public BannerValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Banner Banner)
        {
            BannerFilter BannerFilter = new BannerFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Banner.Id },
                Selects = BannerSelect.Id
            };

            int count = await UOW.BannerRepository.Count(BannerFilter);
            if (count == 0)
                Banner.AddError(nameof(BannerValidator), nameof(Banner.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateTitle(Banner Banner)
        {
            if (string.IsNullOrWhiteSpace(Banner.Title))
                Banner.AddError(nameof(BannerValidator), nameof(Banner.Title), ErrorCode.TitleEmpty);
            else
            {
                if (Banner.Title.Length > 255)
                    Banner.AddError(nameof(BannerValidator), nameof(Banner.Title), ErrorCode.TitleOverLength);
            }
            return Banner.IsValidated;
        }

        private async Task<bool> ValidateImage(Banner Banner)
        {
            if (Banner.ImageId.HasValue == false || Banner.ImageId.Value == 0)
            {
                Banner.AddError(nameof(BannerValidator), nameof(Banner.Image), ErrorCode.ImageEmpty);
            }
            return Banner.IsValidated;
        }

        private async Task<bool> ValidateContent(Banner Banner)
        {
            if (!string.IsNullOrWhiteSpace(Banner.Content))
            {
                if (Banner.Title.Length > 50000)
                    Banner.AddError(nameof(BannerValidator), nameof(Banner.Content), ErrorCode.ContentOverLength);
            }
            return Banner.IsValidated;
        }

        private async Task<bool> ValidatePriority(Banner Banner)
        {
            if (Banner.Priority.HasValue && Banner.Priority <= 0)
                Banner.AddError(nameof(BannerValidator), nameof(Banner.Priority), ErrorCode.PriorityInvalid);
            return Banner.IsValidated;
        }

        public async Task<bool> ValidateStatus(Banner Banner)
        {
            if (StatusEnum.ACTIVE.Id != Banner.StatusId && StatusEnum.INACTIVE.Id != Banner.StatusId)
                Banner.AddError(nameof(BannerValidator), nameof(Banner.Status), ErrorCode.StatusNotExisted);
            return Banner.IsValidated;
        }

        public async Task<bool> Create(Banner Banner)
        {
            await ValidateTitle(Banner);
            await ValidateImage(Banner);
            await ValidateContent(Banner);
            await ValidatePriority(Banner);
            await ValidateStatus(Banner);
            return Banner.IsValidated;
        }

        public async Task<bool> Update(Banner Banner)
        {
            if (await ValidateId(Banner))
            {
                await ValidateTitle(Banner);
                await ValidateImage(Banner);
                await ValidateContent(Banner);
                await ValidatePriority(Banner);
                await ValidateStatus(Banner);
            }
            return Banner.IsValidated;
        }

        public async Task<bool> Delete(Banner Banner)
        {
            if (await ValidateId(Banner))
            {
            }
            return Banner.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Banner> Banners)
        {
            foreach (Banner Banner in Banners)
            {
                await Delete(Banner);
            }
            return Banners.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<Banner> Banners)
        {
            return true;
        }
    }
}
