using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IAlbumRepository
    {
        Task<int> Count(AlbumFilter AlbumFilter);
        Task<int> CountAll(AlbumFilter AlbumFilter);
        Task<List<Album>> List(AlbumFilter AlbumFilter);
        Task<List<Album>> List(List<long> Ids);
        Task<Album> Get(long Id);
        Task<bool> Create(Album Album);
        Task<bool> Update(Album Album);
        Task<bool> Delete(Album Album);
        Task<bool> BulkMerge(List<Album> Albums);
        Task<bool> BulkDelete(List<Album> Albums);
        Task<bool> BulkUsed(List<Album> Albums);
        Task<bool> Used(List<long> Ids);
    }
    public class AlbumRepository : IAlbumRepository
    {
        private DataContext DataContext;
        public AlbumRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<AlbumDAO> DynamicFilter(IQueryable<AlbumDAO> query, AlbumFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.StatusId, filter.StatusId);
            if (filter.StoreId != null)
            {
                if (filter.StoreId.Equal.HasValue)
                {
                    var AlbumIds = DataContext.AlbumImageMapping.Where(x => x.StoreId == filter.StoreId.Equal.Value).Select(x => x.AlbumId).Distinct().ToList();
                    IdFilter IdFilter = new IdFilter { In = AlbumIds };
                    query = query.Where(q => q.Id, IdFilter);
                }

            }
            return query;
        }

        private IQueryable<AlbumDAO> OrFilter(IQueryable<AlbumDAO> query, AlbumFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<AlbumDAO> initQuery = query.Where(q => false);
            foreach (AlbumFilter AlbumFilter in filter.OrFilter)
            {
                IQueryable<AlbumDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, AlbumFilter.Id);
                queryable = queryable.Where(q => q.Name, AlbumFilter.Name);
                queryable = queryable.Where(q => q.StatusId, AlbumFilter.StatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<AlbumDAO> DynamicOrder(IQueryable<AlbumDAO> query, AlbumFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case AlbumOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case AlbumOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case AlbumOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case AlbumOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case AlbumOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case AlbumOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Album>> DynamicSelect(IQueryable<AlbumDAO> query, AlbumFilter filter)
        {
            List<Album> Albums = await query.Select(q => new Album()
            {
                Id = q.Id,
                Name = filter.Selects.Contains(AlbumSelect.Name) ? q.Name : default(string),
                StatusId = filter.Selects.Contains(AlbumSelect.Status) ? q.StatusId : default(long),
                Status = filter.Selects.Contains(AlbumSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                Used = q.Used,
            }).ToListWithNoLockAsync();
            if (filter.Selects.Contains(AlbumSelect.Mapping))
            {
                List<long> Ids = Albums.Select(a => a.Id).ToList();
                IdFilter IdFilter = new IdFilter { In = Ids };
                IQueryable<AlbumImageMappingDAO> AlbumImageMappingQuery = DataContext.AlbumImageMapping
                    .AsNoTracking()
                    .Include(x => x.Image);

                if (filter.ShootingAt != null)
                    AlbumImageMappingQuery = AlbumImageMappingQuery.Where(x => x.ShootingAt, filter.ShootingAt);
                List<AlbumImageMapping> AlbumImageMappings = await AlbumImageMappingQuery
                    .Where(x => x.AlbumId, IdFilter)
                    .Select(x => new AlbumImageMapping
                    {
                        AlbumId = x.AlbumId,
                        ImageId = x.ImageId,
                        OrganizationId = x.OrganizationId,
                        StoreId = x.StoreId,
                        ShootingAt = x.ShootingAt,
                        SaleEmployeeId = x.SaleEmployeeId,
                        Distance = x.Distance,
                        Image = x.Image == null ? null : new Image
                        {
                            Id = x.Image.Id,
                            Name = x.Image.Name,
                            Url = x.Image.Url,
                            ThumbnailUrl = x.Image.ThumbnailUrl,
                        }
                    }).ToListWithNoLockAsync();

                foreach (Album Album in Albums)
                {
                    Album.AlbumImageMappings = AlbumImageMappings.Where(x => x.AlbumId == Album.Id).ToList();
                }
            }
            return Albums;
        }

        public async Task<int> Count(AlbumFilter filter)
        {
            IQueryable<AlbumDAO> Albums = DataContext.Album.AsNoTracking();
            Albums = DynamicFilter(Albums, filter);
            Albums = OrFilter(Albums, filter);
            return await Albums.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(AlbumFilter filter)
        {
            IQueryable<AlbumDAO> Albums = DataContext.Album.AsNoTracking();
            Albums = DynamicFilter(Albums, filter);
            return await Albums.CountWithNoLockAsync();
        }

        public async Task<List<Album>> List(AlbumFilter filter)
        {
            if (filter == null) return new List<Album>();
            IQueryable<AlbumDAO> AlbumDAOs = DataContext.Album.AsNoTracking();
            AlbumDAOs = DynamicFilter(AlbumDAOs, filter);
            AlbumDAOs = OrFilter(AlbumDAOs, filter);
            AlbumDAOs = DynamicOrder(AlbumDAOs, filter);
            List<Album> Albums = await DynamicSelect(AlbumDAOs, filter);
            return Albums;
        }

        public async Task<List<Album>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<Album> Albums = await DataContext.Album.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new Album()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Name = x.Name,
                StatusId = x.StatusId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).ToListWithNoLockAsync();

            var AlbumImageMappings = await DataContext.AlbumImageMapping.AsNoTracking()
                .Where(x => Ids.Contains(x.AlbumId))
                .Select(x => new AlbumImageMapping
                {
                    AlbumId = x.AlbumId,
                    ImageId = x.ImageId,
                    OrganizationId = x.OrganizationId,
                    ShootingAt = x.ShootingAt,
                    Distance = x.Distance,
                    SaleEmployeeId = x.SaleEmployeeId,
                    StoreId = x.StoreId,
                    Image = x.Image == null ? null : new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    }
                }).ToListWithNoLockAsync();
            foreach (Album Album in Albums)
            {
                Album.AlbumImageMappings = AlbumImageMappings.Where(x => x.AlbumId == Album.Id).ToList();
            }
            return Albums;
        }

        public async Task<Album> Get(long Id)
        {
            Album Album = await DataContext.Album.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new Album()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                Name = x.Name,
                StatusId = x.StatusId,
                Used = x.Used,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (Album == null)
                return null;
            Album.AlbumImageMappings = await DataContext.AlbumImageMapping.Where(x => x.AlbumId == Id)
                .Select(x => new AlbumImageMapping
                {
                    AlbumId = x.AlbumId,
                    ImageId = x.ImageId,
                    OrganizationId = x.OrganizationId,
                    ShootingAt = x.ShootingAt,
                    Distance = x.Distance,
                    SaleEmployeeId = x.SaleEmployeeId,
                    StoreId = x.StoreId,
                    Image = x.Image == null ? null : new Image
                    {
                        Id = x.Image.Id,
                        Name = x.Image.Name,
                        Url = x.Image.Url,
                        ThumbnailUrl = x.Image.ThumbnailUrl,
                    }
                }).ToListWithNoLockAsync();
            return Album;
        }
        public async Task<bool> Create(Album Album)
        {
            AlbumDAO AlbumDAO = new AlbumDAO();
            AlbumDAO.Id = Album.Id;
            AlbumDAO.Name = Album.Name;
            AlbumDAO.StatusId = Album.StatusId;
            AlbumDAO.CreatedAt = StaticParams.DateTimeNow;
            AlbumDAO.UpdatedAt = StaticParams.DateTimeNow;
            AlbumDAO.Used = false;
            DataContext.Album.Add(AlbumDAO);
            await DataContext.SaveChangesAsync();
            Album.Id = AlbumDAO.Id;
            return true;
        }

        public async Task<bool> Update(Album Album)
        {
            AlbumDAO AlbumDAO = DataContext.Album.Where(x => x.Id == Album.Id).FirstOrDefault();
            if (AlbumDAO == null)
                return false;
            AlbumDAO.Id = Album.Id;
            AlbumDAO.Name = Album.Name;
            AlbumDAO.StatusId = Album.StatusId;
            AlbumDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(Album);
            return true;
        }

        public async Task<bool> Delete(Album Album)
        {
            await DataContext.StoreCheckingImageMapping.Where(x => x.AlbumId == Album.Id).DeleteFromQueryAsync();
            await DataContext.Album.Where(x => x.Id == Album.Id).UpdateFromQueryAsync(x => new AlbumDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        public async Task<bool> BulkMerge(List<Album> Albums)
        {
            List<AlbumDAO> AlbumDAOs = new List<AlbumDAO>();
            foreach (Album Album in Albums)
            {
                AlbumDAO AlbumDAO = new AlbumDAO();
                AlbumDAO.Id = Album.Id;
                AlbumDAO.Name = Album.Name;
                AlbumDAO.StatusId = Album.StatusId;
                AlbumDAO.CreatedAt = StaticParams.DateTimeNow;
                AlbumDAO.UpdatedAt = StaticParams.DateTimeNow;
                AlbumDAOs.Add(AlbumDAO);
            }
            await DataContext.BulkMergeAsync(AlbumDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<Album> Albums)
        {
            List<long> Ids = Albums.Select(x => x.Id).ToList();
            await DataContext.Album
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new AlbumDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

        private async Task SaveReference(Album Album)
        {
            List<AlbumImageMappingDAO> AlbumImageMappingDAOs = await DataContext.AlbumImageMapping.Where(x => x.AlbumId == Album.Id).ToListWithNoLockAsync();
            AlbumImageMappingDAOs.ForEach(x => x.DeletedAt = StaticParams.DateTimeNow);
            if (Album.AlbumImageMappings != null)
            {
                foreach (var AlbumImageMapping in Album.AlbumImageMappings)
                {
                    AlbumImageMappingDAO AlbumImageMappingDAO = AlbumImageMappingDAOs.Where(x => x.ImageId == AlbumImageMapping.ImageId).FirstOrDefault();
                    if (AlbumImageMappingDAO == null)
                    {
                        AlbumImageMappingDAO = new AlbumImageMappingDAO();
                        AlbumImageMappingDAO.AlbumId = Album.Id;
                        AlbumImageMappingDAO.ImageId = AlbumImageMapping.ImageId;
                        AlbumImageMappingDAO.OrganizationId = AlbumImageMapping.OrganizationId;
                        AlbumImageMappingDAO.StoreId = AlbumImageMapping.StoreId;
                        AlbumImageMappingDAO.Distance = AlbumImageMapping.Distance;
                        AlbumImageMappingDAO.SaleEmployeeId = AlbumImageMapping.SaleEmployeeId;
                        AlbumImageMappingDAO.ShootingAt = StaticParams.DateTimeNow;
                        AlbumImageMappingDAO.DeletedAt = null;
                        AlbumImageMappingDAOs.Add(AlbumImageMappingDAO);
                    }
                    else
                    {
                        AlbumImageMappingDAO.AlbumId = Album.Id;
                        AlbumImageMappingDAO.ShootingAt = AlbumImageMapping.ShootingAt;
                        AlbumImageMappingDAO.Distance = AlbumImageMapping.Distance;
                        AlbumImageMappingDAO.DeletedAt = null;
                    }
                }
            }
            await DataContext.AlbumImageMapping.BulkMergeAsync(AlbumImageMappingDAOs);
        }

        public async Task<bool> BulkUsed(List<Album> Albums)
        {
            List<long> Ids = Albums.Select(x => x.Id).ToList();
            await DataContext.Album
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(a => new AlbumDAO { Used = true });
            return true;
        }
        public async Task<bool> Used(List<long> Ids)
        {
            await DataContext.Album
                .Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new AlbumDAO { Used = true });
            return true;
        }
    }
}
