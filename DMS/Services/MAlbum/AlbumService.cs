using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MAlbum
{
    public interface IAlbumService : IServiceScoped
    {
        Task<int> Count(AlbumFilter AlbumFilter);
        Task<List<Album>> List(AlbumFilter AlbumFilter);
        Task<Album> Get(long Id);
        Task<Album> Create(Album Album);
        Task<Album> Update(Album Album);
        Task<Album> UpdateMobile(Album Album);
        Task<Album> Delete(Album Album);
        Task<List<Album>> BulkDelete(List<Album> Albums);
        Task<List<Album>> Import(List<Album> Albums);
        AlbumFilter ToFilter(AlbumFilter AlbumFilter);
    }

    public class AlbumService : BaseService, IAlbumService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IAlbumValidator AlbumValidator;
        private IRabbitManager RabbitManager;

        public AlbumService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IAlbumValidator AlbumValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.AlbumValidator = AlbumValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(AlbumFilter AlbumFilter)
        {
            try
            {
                int result = await UOW.AlbumRepository.Count(AlbumFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return 0;
        }

        public async Task<List<Album>> List(AlbumFilter AlbumFilter)
        {
            try
            {
                List<Album> Albums = await UOW.AlbumRepository.List(AlbumFilter);
                return Albums;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return null;
        }
        public async Task<Album> Get(long Id)
        {
            Album Album = await UOW.AlbumRepository.Get(Id);
            if (Album == null)
                return null;
            return Album;
        }

        public async Task<Album> Create(Album Album)
        {
            if (!await AlbumValidator.Create(Album))
                return Album;

            try
            {
                await UOW.AlbumRepository.Create(Album);
                Album = await UOW.AlbumRepository.Get(Album.Id);
                await SyncAsync(new List<Album> { Album });
                Logging.CreateAuditLog(Album, new { }, nameof(AlbumService));
                return Album;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return null;
        }

        public async Task<Album> Update(Album Album)
        {
            if (!await AlbumValidator.Update(Album))
                return Album;
            try
            {
                var oldData = await UOW.AlbumRepository.Get(Album.Id);
                Album.AlbumImageMappings = oldData.AlbumImageMappings;
                await UOW.AlbumRepository.Update(Album);
                Album = await UOW.AlbumRepository.Get(Album.Id);
                await SyncAsync(new List<Album> { Album });
                Logging.CreateAuditLog(Album, oldData, nameof(AlbumService));
                return Album;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return null;
        }

        public async Task<Album> UpdateMobile(Album Album)
        {
            if (!await AlbumValidator.UpdateMobile(Album))
                return Album;
            try
            {
                var appUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var oldData = await UOW.AlbumRepository.Get(Album.Id);
                Album.Name = oldData.Name;
                Album.StatusId = oldData.StatusId;
                foreach (var AlbumImageMapping in Album.AlbumImageMappings)
                {
                    AlbumImageMapping.OrganizationId = appUser.OrganizationId;
                }
                Album.AlbumImageMappings.AddRange(oldData.AlbumImageMappings);
                await UOW.AlbumRepository.Update(Album);
                await SyncAsync(new List<Album> { Album });
                var newData = await UOW.AlbumRepository.Get(Album.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(AlbumService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return null;
        }

        public async Task<Album> Delete(Album Album)
        {
            if (!await AlbumValidator.Delete(Album))
                return Album;

            try
            {
                await UOW.AlbumRepository.Delete(Album);
                Logging.CreateAuditLog(new { }, Album, nameof(AlbumService));
                await SyncAsync(new List<Album> { Album });
                return Album;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return null;
        }

        public async Task<List<Album>> BulkDelete(List<Album> Albums)
        {
            if (!await AlbumValidator.BulkDelete(Albums))
                return Albums;

            try
            {
                await UOW.AlbumRepository.BulkDelete(Albums);
                await SyncAsync(Albums);
                Logging.CreateAuditLog(new { }, Albums, nameof(AlbumService));
                return Albums;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return null;
        }

        public async Task<List<Album>> Import(List<Album> Albums)
        {
            if (!await AlbumValidator.Import(Albums))
                return Albums;
            try
            {
                await UOW.AlbumRepository.BulkMerge(Albums);
                await SyncAsync(Albums);
                Logging.CreateAuditLog(Albums, new { }, nameof(AlbumService));
                return Albums;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(AlbumService));
            }
            return null;
        }

        public AlbumFilter ToFilter(AlbumFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<AlbumFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                AlbumFilter subFilter = new AlbumFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterPermissionDefinition.IdFilter;
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterPermissionDefinition.StringFilter;
                }
            }
            return filter;
        }

        private void NotifyUsed(Album Album)
        {
        }
        private async Task SyncAsync(List<Album> Albums)
        {
            if (Albums.Any(x => x.AlbumImageMappings != null))
            {
                List<Store> Stores = Albums.SelectMany(x => x.AlbumImageMappings)
                    .Select(x => new Store { Id = x.StoreId }).Distinct().ToList();
                RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
            }

            var AlbumIds = Albums.Select(x => x.Id).ToList();
            var count = AlbumIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = AlbumIds.Skip(skip).Take(take).ToList();
                var albums = await UOW.AlbumRepository.List(Batch);
                RabbitManager.PublishList(albums, RoutingKeyEnum.AlbumSync.Code);
            }
        }
    }
}
