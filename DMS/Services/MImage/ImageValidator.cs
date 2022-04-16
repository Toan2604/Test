using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MImage
{
    public interface IImageValidator : IServiceScoped
    {
        Task<bool> Create(Image Image);
        Task<bool> Update(Image Image);
        Task<bool> Delete(Image Image);
        Task<bool> BulkDelete(List<Image> Images);
        Task<bool> Import(List<Image> Images);
    }

    public class ImageValidator : IImageValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ImageValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Image Image)
        {
            ImageFilter ImageFilter = new ImageFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Image.Id },
                Selects = ImageSelect.Id
            };

            int count = await UOW.ImageRepository.Count(ImageFilter);
            if (count == 0)
                Image.AddError(nameof(ImageValidator), nameof(Image.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(Image Image)
        {
            return Image.IsValidated;
        }

        public async Task<bool> Update(Image Image)
        {
            if (await ValidateId(Image))
            {
            }
            return Image.IsValidated;
        }

        public async Task<bool> Delete(Image Image)
        {
            if (await ValidateId(Image))
            {
            }
            return Image.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Image> Images)
        {
            return true;
        }

        public async Task<bool> Import(List<Image> Images)
        {
            return true;
        }
    }
}
