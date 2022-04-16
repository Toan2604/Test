using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MColor
{
    public interface IColorValidator : IServiceScoped
    {
        Task<bool> Create(Color Color);
        Task<bool> Update(Color Color);
        Task<bool> Delete(Color Color);
        Task<bool> BulkDelete(List<Color> Colors);
        Task<bool> Import(List<Color> Colors);
    }

    public class ColorValidator : IColorValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public ColorValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Color Color)
        {
            ColorFilter ColorFilter = new ColorFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Color.Id },
                Selects = ColorSelect.Id
            };

            int count = await UOW.ColorRepository.Count(ColorFilter);
            if (count == 0)
                Color.AddError(nameof(ColorValidator), nameof(Color.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(Color Color)
        {
            return Color.IsValidated;
        }

        public async Task<bool> Update(Color Color)
        {
            if (await ValidateId(Color))
            {
            }
            return Color.IsValidated;
        }

        public async Task<bool> Delete(Color Color)
        {
            if (await ValidateId(Color))
            {
            }
            return Color.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Color> Colors)
        {
            foreach (Color Color in Colors)
            {
                await Delete(Color);
            }
            return Colors.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<Color> Colors)
        {
            return true;
        }
    }
}
