using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MAlbum
{
    public interface IAlbumValidator : IServiceScoped
    {
        Task<bool> Create(Album Album);
        Task<bool> Update(Album Album);
        Task<bool> Delete(Album Album);
        Task<bool> BulkDelete(List<Album> Albums);
        Task<bool> Import(List<Album> Albums);
        Task<bool> UpdateMobile(Album Album);
    }

    public class AlbumValidator : IAlbumValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            NameEmpty,
            NameOverLength,
            StatusNotExisted,
            AlbumInUsed
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public AlbumValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Album Album)
        {
            AlbumFilter AlbumFilter = new AlbumFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Album.Id },
                Selects = AlbumSelect.Id
            };

            int count = await UOW.AlbumRepository.Count(AlbumFilter);
            if (count == 0)
                Album.AddError(nameof(AlbumValidator), nameof(Album.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateName(Album Album)
        {
            if (string.IsNullOrWhiteSpace(Album.Name))
                Album.AddError(nameof(AlbumValidator), nameof(Album.Name), ErrorCode.NameEmpty);
            else
            {
                if (Album.Name.Length > 255)
                    Album.AddError(nameof(AlbumValidator), nameof(Album.Name), ErrorCode.NameOverLength);
            }
            return Album.IsValidated;
        }

        public async Task<bool> ValidateStatus(Album Album)
        {
            if (StatusEnum.ACTIVE.Id != Album.StatusId && StatusEnum.INACTIVE.Id != Album.StatusId)
                Album.AddError(nameof(AlbumValidator), nameof(Album.Status), ErrorCode.StatusNotExisted);
            return Album.IsValidated;
        }

        public async Task<bool> Create(Album Album)
        {
            await ValidateName(Album);
            await ValidateStatus(Album);
            return Album.IsValidated;
        }

        public async Task<bool> Update(Album Album)
        {
            if (await ValidateId(Album))
            {
                await ValidateName(Album);
                await ValidateStatus(Album);
            }
            return Album.IsValidated;
        }
        public async Task<bool> UpdateMobile(Album Album)
        {
            if (await ValidateId(Album))
            {
                await ValidateStatus(Album);
            }
            return Album.IsValidated;
        }

        public async Task<bool> Delete(Album Album)
        {
            if (await ValidateId(Album))
            {
                Album = await UOW.AlbumRepository.Get(Album.Id);
                if (Album.Used)
                    Album.AddError(nameof(AlbumValidator), nameof(Album.Id), ErrorCode.AlbumInUsed);
            }
            return Album.IsValidated;
        }

        public async Task<bool> BulkDelete(List<Album> Albums)
        {
            foreach (Album Album in Albums)
            {
                await Delete(Album);
            }
            return Albums.All(st => st.IsValidated);
        }

        public async Task<bool> Import(List<Album> Albums)
        {
            return true;
        }
    }
}
