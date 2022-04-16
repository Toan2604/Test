using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IMenuRepository
    {
        Task<int> Count(MenuFilter MenuFilter);
        Task<int> CountAll(MenuFilter MenuFilter);
        Task<List<Menu>> List(MenuFilter MenuFilter);
        Task<Menu> Get(long Id);
    }
    public class MenuRepository : IMenuRepository
    {
        private DataContext DataContext;
        public MenuRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<MenuDAO> DynamicFilter(IQueryable<MenuDAO> query, MenuFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Path, filter.Path);
            return query;
        }

        private IQueryable<MenuDAO> OrFilter(IQueryable<MenuDAO> query, MenuFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<MenuDAO> initQuery = query.Where(q => false);
            foreach (MenuFilter MenuFilter in filter.OrFilter)
            {
                IQueryable<MenuDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, MenuFilter.Id);
                queryable = queryable.Where(q => q.Code, MenuFilter.Code);
                queryable = queryable.Where(q => q.Name, MenuFilter.Name);
                queryable = queryable.Where(q => q.Path, MenuFilter.Path);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<MenuDAO> DynamicOrder(IQueryable<MenuDAO> query, MenuFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case MenuOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case MenuOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case MenuOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case MenuOrder.Path:
                            query = query.OrderBy(q => q.Path);
                            break;
                        case MenuOrder.IsDeleted:
                            query = query.OrderBy(q => q.IsDeleted);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case MenuOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case MenuOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case MenuOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case MenuOrder.Path:
                            query = query.OrderByDescending(q => q.Path);
                            break;
                        case MenuOrder.IsDeleted:
                            query = query.OrderByDescending(q => q.IsDeleted);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Menu>> DynamicSelect(IQueryable<MenuDAO> query, MenuFilter filter)
        {
            List<Menu> Menus = await query.Select(q => new Menu()
            {
                Id = filter.Selects.Contains(MenuSelect.Id) ? q.Id : default(long),
                Code = filter.Selects.Contains(MenuSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(MenuSelect.Name) ? q.Name : default(string),
                Path = filter.Selects.Contains(MenuSelect.Path) ? q.Path : default(string),
                IsDeleted = filter.Selects.Contains(MenuSelect.IsDeleted) ? q.IsDeleted : default(bool),
            }).ToListWithNoLockAsync();
            return Menus;
        }

        public async Task<int> Count(MenuFilter filter)
        {
            IQueryable<MenuDAO> Menus = DataContext.Menu;
            Menus = DynamicFilter(Menus, filter);
            Menus = OrFilter(Menus, filter);
            return await Menus.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(MenuFilter filter)
        {
            IQueryable<MenuDAO> Menus = DataContext.Menu;
            Menus = DynamicFilter(Menus, filter);
            return await Menus.CountWithNoLockAsync();
        }

        public async Task<List<Menu>> List(MenuFilter filter)
        {
            if (filter == null) return new List<Menu>();
            IQueryable<MenuDAO> MenuDAOs = DataContext.Menu.AsNoTracking();
            MenuDAOs = DynamicFilter(MenuDAOs, filter);
            MenuDAOs = OrFilter(MenuDAOs, filter);
            MenuDAOs = DynamicOrder(MenuDAOs, filter);
            List<Menu> Menus = await DynamicSelect(MenuDAOs, filter);
            return Menus;
        }

        public async Task<Menu> Get(long Id)
        {
            Menu Menu = await DataContext.Menu.AsNoTracking()
                .Where(x => x.Id == Id).Select(x => new Menu()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Path = x.Path,
                    IsDeleted = x.IsDeleted,
                }).FirstOrDefaultWithNoLockAsync();

            if (Menu == null)
                return null;
            Menu.Fields = await DataContext.Field
                .Where(x => x.MenuId == Menu.Id)
                .Select(x => new Field
                {
                    Id = x.Id,
                    Name = x.Name,
                    MenuId = x.MenuId,
                    IsDeleted = x.IsDeleted,
                }).ToListWithNoLockAsync();
            Menu.Actions = await DataContext.Action
                .Where(x => x.MenuId == Menu.Id)
                .Select(x => new Entities.Action
                {
                    Id = x.Id,
                    Name = x.Name,
                    MenuId = x.MenuId,
                    IsDeleted = x.IsDeleted,
                }).ToListWithNoLockAsync();
            return Menu;
        }

    }
}
