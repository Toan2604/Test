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
    public interface IFileRepository
    {
        Task<int> Count(FileFilter FileFilter);
        Task<int> CountAll(FileFilter FileFilter);
        Task<List<File>> List(FileFilter FileFilter);
        Task<File> Get(long Id);
        Task<bool> Create(File File);
        Task<bool> Delete(File File);
    }
    public class FileRepository : IFileRepository
    {
        private DataContext DataContext;
        public FileRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<FileDAO> DynamicFilter(IQueryable<FileDAO> query, FileFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Path, filter.Path);

            return query;
        }

        private IQueryable<FileDAO> OrFilter(IQueryable<FileDAO> query, FileFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<FileDAO> initQuery = query.Where(q => false);
            foreach (FileFilter FileFilter in filter.OrFilter)
            {
                IQueryable<FileDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, FileFilter.Id);
                queryable = queryable.Where(q => q.Name, FileFilter.Name);
                queryable = queryable.Where(q => q.Path, FileFilter.Path);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<FileDAO> DynamicOrder(IQueryable<FileDAO> query, FileFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case FileOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case FileOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case FileOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case FileOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case FileOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case FileOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<File>> DynamicSelect(IQueryable<FileDAO> query, FileFilter filter)
        {
            List<File> Files = await query.Select(q => new File()
            {
                Id = filter.Selects.Contains(FileSelect.Id) ? q.Id : default(long),
                Name = filter.Selects.Contains(FileSelect.Name) ? q.Name : default(string),
                Path = filter.Selects.Contains(FileSelect.Path) ? q.Path : default(string),
            }).ToListWithNoLockAsync();
            return Files;
        }

        public async Task<int> Count(FileFilter filter)
        {
            IQueryable<FileDAO> Files = DataContext.File;
            Files = DynamicFilter(Files, filter);
            Files = OrFilter(Files, filter);
            return await Files.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(FileFilter filter)
        {
            IQueryable<FileDAO> Files = DataContext.File;
            Files = DynamicFilter(Files, filter);
            return await Files.CountWithNoLockAsync();
        }

        public async Task<List<File>> List(FileFilter filter)
        {
            if (filter == null) return new List<File>();
            IQueryable<FileDAO> FileDAOs = DataContext.File.AsNoTracking();
            FileDAOs = DynamicFilter(FileDAOs, filter);
            FileDAOs = OrFilter(FileDAOs, filter);
            FileDAOs = DynamicOrder(FileDAOs, filter);
            List<File> Files = await DynamicSelect(FileDAOs, filter);
            return Files;
        }

        public async Task<File> Get(long Id)
        {
            File File = await DataContext.File.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new File()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Path = x.Path,
                }).FirstOrDefaultWithNoLockAsync();

            if (File == null)
                return null;

            return File;
        }
        public async Task<bool> Create(File File)
        {
            FileDAO FileDAO = new FileDAO();
            FileDAO.Id = File.Id;
            FileDAO.Name = File.Name;
            FileDAO.Path = File.Path;
            FileDAO.RowId = File.RowId;
            FileDAO.CreatedAt = StaticParams.DateTimeNow;
            FileDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.BulkMergeAsync<FileDAO>(new List<FileDAO> { FileDAO });
            return true;
        }

        public async Task<bool> Delete(File File)
        {
            await DataContext.File.Where(x => x.Id == File.Id).UpdateFromQueryAsync(x => new FileDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

    }
}
