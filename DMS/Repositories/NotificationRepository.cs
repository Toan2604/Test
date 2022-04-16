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
    public interface INotificationRepository
    {
        Task<int> Count(NotificationFilter NotificationFilter);
        Task<int> CountAll(NotificationFilter NotificationFilter);
        Task<List<Notification>> List(NotificationFilter NotificationFilter);
        Task<Notification> Get(long Id);
        Task<bool> Create(Notification Notification);
        Task<bool> Update(Notification Notification);
        Task<bool> Delete(Notification Notification);
        Task<bool> BulkMerge(List<Notification> Notifications);
        Task<bool> BulkDelete(List<Notification> Notifications);
    }
    public class NotificationRepository : INotificationRepository
    {
        private DataContext DataContext;
        public NotificationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<NotificationDAO> DynamicFilter(IQueryable<NotificationDAO> query, NotificationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Title, filter.Title);
            query = query.Where(q => q.Content, filter.Content);
            query = query.Where(q => q.OrganizationId, filter.OrganizationId);
            query = query.Where(q => q.NotificationStatusId, filter.NotificationStatusId);
            query = OrFilter(query, filter);
            return query;
        }

        private IQueryable<NotificationDAO> OrFilter(IQueryable<NotificationDAO> query, NotificationFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<NotificationDAO> initQuery = query.Where(q => false);
            foreach (NotificationFilter NotificationFilter in filter.OrFilter)
            {
                IQueryable<NotificationDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, NotificationFilter.Id);
                queryable = queryable.Where(q => q.Title, NotificationFilter.Title);
                queryable = queryable.Where(q => q.Content, NotificationFilter.Content);
                queryable = queryable.Where(q => q.OrganizationId, NotificationFilter.OrganizationId);
                queryable = queryable.Where(q => q.NotificationStatusId, NotificationFilter.NotificationStatusId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<NotificationDAO> DynamicOrder(IQueryable<NotificationDAO> query, NotificationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case NotificationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case NotificationOrder.Title:
                            query = query.OrderBy(q => q.Title);
                            break;
                        case NotificationOrder.Content:
                            query = query.OrderBy(q => q.Content);
                            break;
                        case NotificationOrder.Organization:
                            query = query.OrderBy(q => q.OrganizationId);
                            break;
                        case NotificationOrder.NotificationStatus:
                            query = query.OrderBy(q => q.NotificationStatusId);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case NotificationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case NotificationOrder.Title:
                            query = query.OrderByDescending(q => q.Title);
                            break;
                        case NotificationOrder.Content:
                            query = query.OrderByDescending(q => q.Content);
                            break;
                        case NotificationOrder.Organization:
                            query = query.OrderByDescending(q => q.OrganizationId);
                            break;
                        case NotificationOrder.NotificationStatus:
                            query = query.OrderByDescending(q => q.NotificationStatusId);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Notification>> DynamicSelect(IQueryable<NotificationDAO> query, NotificationFilter filter)
        {
            List<Notification> Notifications = await query.Select(q => new Notification()
            {
                Id = filter.Selects.Contains(NotificationSelect.Id) ? q.Id : default(long),
                Title = filter.Selects.Contains(NotificationSelect.Title) ? q.Title : default(string),
                Content = filter.Selects.Contains(NotificationSelect.Content) ? q.Content : default(string),
                OrganizationId = filter.Selects.Contains(NotificationSelect.Organization) ? q.OrganizationId : default(long?),
                NotificationStatusId = filter.Selects.Contains(NotificationSelect.NotificationStatus) ? q.NotificationStatusId : default(long),
                Organization = filter.Selects.Contains(NotificationSelect.Organization) && q.Organization != null ? new Organization
                {
                    Id = q.Organization.Id,
                    Code = q.Organization.Code,
                    Name = q.Organization.Name,
                    Address = q.Organization.Address,
                    Email = q.Organization.Email,
                    Phone = q.Organization.Phone,
                } : null,
                NotificationStatus = filter.Selects.Contains(NotificationSelect.NotificationStatus) && q.NotificationStatus != null ? new NotificationStatus
                {
                    Id = q.NotificationStatus.Id,
                    Code = q.NotificationStatus.Code,
                    Name = q.NotificationStatus.Name,
                } : null,
            }).ToListWithNoLockAsync();
            return Notifications;
        }

        public async Task<int> Count(NotificationFilter filter)
        {
            IQueryable<NotificationDAO> Notifications = DataContext.Notification.AsNoTracking();
            Notifications = DynamicFilter(Notifications, filter);
            Notifications = OrFilter(Notifications, filter);
            return await Notifications.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(NotificationFilter filter)
        {
            IQueryable<NotificationDAO> Notifications = DataContext.Notification.AsNoTracking();
            Notifications = DynamicFilter(Notifications, filter);
            return await Notifications.CountWithNoLockAsync();
        }
        public async Task<List<Notification>> List(NotificationFilter filter)
        {
            if (filter == null) return new List<Notification>();
            IQueryable<NotificationDAO> NotificationDAOs = DataContext.Notification.AsNoTracking();
            NotificationDAOs = DynamicFilter(NotificationDAOs, filter);
            NotificationDAOs = OrFilter(NotificationDAOs, filter);
            NotificationDAOs = DynamicOrder(NotificationDAOs, filter);
            List<Notification> Notifications = await DynamicSelect(NotificationDAOs, filter);
            return Notifications;
        }

        public async Task<Notification> Get(long Id)
        {
            Notification Notification = await DataContext.Notification.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new Notification()
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                OrganizationId = x.OrganizationId,
                NotificationStatusId = x.NotificationStatusId,
                Organization = new Organization
                {
                    Id = x.Organization.Id,
                    Code = x.Organization.Code,
                    Name = x.Organization.Name,
                    Address = x.Organization.Address,
                    Email = x.Organization.Email,
                    Phone = x.Organization.Phone,
                },
                NotificationStatus = new NotificationStatus
                {
                    Id = x.NotificationStatus.Id,
                    Code = x.NotificationStatus.Code,
                    Name = x.NotificationStatus.Name,
                }
            }).FirstOrDefaultWithNoLockAsync();

            if (Notification == null)
                return null;
            return Notification;
        }
        public async Task<bool> Create(Notification Notification)
        {
            NotificationDAO NotificationDAO = new NotificationDAO();
            NotificationDAO.Id = Notification.Id;
            NotificationDAO.Title = Notification.Title;
            NotificationDAO.Content = Notification.Content;
            NotificationDAO.OrganizationId = Notification.OrganizationId;
            NotificationDAO.NotificationStatusId = Notification.NotificationStatusId;
            DataContext.Notification.Add(NotificationDAO);
            await DataContext.SaveChangesAsync();
            Notification.Id = NotificationDAO.Id;
            await SaveReference(Notification);
            return true;
        }

        public async Task<bool> Update(Notification Notification)
        {
            NotificationDAO NotificationDAO = DataContext.Notification.Where(x => x.Id == Notification.Id).FirstOrDefault();
            if (NotificationDAO == null)
                return false;
            NotificationDAO.Id = Notification.Id;
            NotificationDAO.Title = Notification.Title;
            NotificationDAO.Content = Notification.Content;
            NotificationDAO.OrganizationId = Notification.OrganizationId;
            NotificationDAO.NotificationStatusId = Notification.NotificationStatusId;
            await DataContext.SaveChangesAsync();
            await SaveReference(Notification);
            return true;
        }

        public async Task<bool> Delete(Notification Notification)
        {
            await DataContext.Notification.Where(x => x.Id == Notification.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<Notification> Notifications)
        {
            List<NotificationDAO> NotificationDAOs = new List<NotificationDAO>();
            foreach (Notification Notification in Notifications)
            {
                NotificationDAO NotificationDAO = new NotificationDAO();
                NotificationDAO.Id = Notification.Id;
                NotificationDAO.Title = Notification.Title;
                NotificationDAO.Content = Notification.Content;
                NotificationDAO.OrganizationId = Notification.OrganizationId;
                NotificationDAO.NotificationStatusId = Notification.NotificationStatusId;
                NotificationDAOs.Add(NotificationDAO);
            }
            await DataContext.BulkMergeAsync(NotificationDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<Notification> Notifications)
        {
            List<long> Ids = Notifications.Select(x => x.Id).ToList();
            await DataContext.Notification
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(Notification Notification)
        {
        }

    }
}
