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
    public interface IWorkflowStepRepository
    {
        Task<int> Count(WorkflowStepFilter WorkflowStepFilter);
        Task<int> CountAll(WorkflowStepFilter WorkflowStepFilter);
        Task<List<WorkflowStep>> List(WorkflowStepFilter WorkflowStepFilter);
        Task<WorkflowStep> Get(long Id);
        Task<bool> Create(WorkflowStep WorkflowStep);
        Task<bool> Update(WorkflowStep WorkflowStep);
        Task<bool> Delete(WorkflowStep WorkflowStep);
        Task<bool> BulkMerge(List<WorkflowStep> WorkflowSteps);
        Task<bool> BulkDelete(List<WorkflowStep> WorkflowSteps);
    }
    public class WorkflowStepRepository : IWorkflowStepRepository
    {
        private DataContext DataContext;
        public WorkflowStepRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<WorkflowStepDAO> DynamicFilter(IQueryable<WorkflowStepDAO> query, WorkflowStepFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.WorkflowDefinitionId, filter.WorkflowDefinitionId);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.RoleId, filter.RoleId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            return query;
        }

        private IQueryable<WorkflowStepDAO> OrFilter(IQueryable<WorkflowStepDAO> query, WorkflowStepFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WorkflowStepDAO> initQuery = query.Where(q => false);
            foreach (WorkflowStepFilter WorkflowStepFilter in filter.OrFilter)
            {
                IQueryable<WorkflowStepDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WorkflowStepFilter.Id);
                queryable = queryable.Where(q => q.WorkflowDefinitionId, WorkflowStepFilter.WorkflowDefinitionId);
                queryable = queryable.Where(q => q.Code, WorkflowStepFilter.Code);
                queryable = queryable.Where(q => q.Name, WorkflowStepFilter.Name);
                queryable = queryable.Where(q => q.RoleId, WorkflowStepFilter.RoleId);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<WorkflowStepDAO> DynamicOrder(IQueryable<WorkflowStepDAO> query, WorkflowStepFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowStepOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WorkflowStepOrder.WorkflowDefinition:
                            query = query.OrderBy(q => q.WorkflowDefinition.Name);
                            break;
                        case WorkflowStepOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case WorkflowStepOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case WorkflowStepOrder.Role:
                            query = query.OrderBy(q => q.Role.Name);
                            break;
                        case WorkflowStepOrder.CreatedAt:
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                        case WorkflowStepOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowStepOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WorkflowStepOrder.WorkflowDefinition:
                            query = query.OrderByDescending(q => q.WorkflowDefinition.Name);
                            break;
                        case WorkflowStepOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case WorkflowStepOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case WorkflowStepOrder.Role:
                            query = query.OrderByDescending(q => q.Role.Name);
                            break;
                        case WorkflowStepOrder.CreatedAt:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                        case WorkflowStepOrder.UpdatedAt:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<WorkflowStep>> DynamicSelect(IQueryable<WorkflowStepDAO> query, WorkflowStepFilter filter)
        {
            List<WorkflowStep> WorkflowSteps = await query.Select(q => new WorkflowStep()
            {
                Id = filter.Selects.Contains(WorkflowStepSelect.Id) ? q.Id : default(long),
                WorkflowDefinitionId = filter.Selects.Contains(WorkflowStepSelect.WorkflowDefinition) ? q.WorkflowDefinitionId : default(long),
                Code = filter.Selects.Contains(WorkflowStepSelect.Code) ? q.Code : default(string),
                Name = filter.Selects.Contains(WorkflowStepSelect.Name) ? q.Name : default(string),
                RoleId = filter.Selects.Contains(WorkflowStepSelect.Role) ? q.RoleId : default(long),
                StatusId = filter.Selects.Contains(WorkflowStepSelect.Status) ? q.StatusId : default(long),
                CreatedAt = filter.Selects.Contains(WorkflowStepSelect.CreatedAt) ? q.CreatedAt : default(DateTime),
                UpdatedAt = filter.Selects.Contains(WorkflowStepSelect.UpdatedAt) ? q.UpdatedAt : default(DateTime),
                Role = filter.Selects.Contains(WorkflowStepSelect.Role) && q.Role != null ? new Role
                {
                    Id = q.Role.Id,
                    Code = q.Role.Code,
                    Name = q.Role.Name,
                    StatusId = q.Role.StatusId,
                } : null,
                Status = filter.Selects.Contains(WorkflowStepSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                WorkflowDefinition = filter.Selects.Contains(WorkflowStepSelect.WorkflowDefinition) && q.WorkflowDefinition != null ? new WorkflowDefinition
                {
                    Id = q.WorkflowDefinition.Id,
                    Name = q.WorkflowDefinition.Name,
                    WorkflowTypeId = q.WorkflowDefinition.WorkflowTypeId,
                    StartDate = q.WorkflowDefinition.StartDate,
                    EndDate = q.WorkflowDefinition.EndDate,
                    StatusId = q.WorkflowDefinition.StatusId,
                } : null,
                Used = q.WorkflowDefinition.Used,
            }).ToListWithNoLockAsync();
            return WorkflowSteps;
        }

        public async Task<int> Count(WorkflowStepFilter filter)
        {
            IQueryable<WorkflowStepDAO> WorkflowSteps = DataContext.WorkflowStep.AsNoTracking();
            WorkflowSteps = DynamicFilter(WorkflowSteps, filter);
            WorkflowSteps = OrFilter(WorkflowSteps, filter);
            return await WorkflowSteps.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(WorkflowStepFilter filter)
        {
            IQueryable<WorkflowStepDAO> WorkflowSteps = DataContext.WorkflowStep.AsNoTracking();
            WorkflowSteps = DynamicFilter(WorkflowSteps, filter);
            return await WorkflowSteps.CountWithNoLockAsync();
        }

        public async Task<List<WorkflowStep>> List(WorkflowStepFilter filter)
        {
            if (filter == null) return new List<WorkflowStep>();
            IQueryable<WorkflowStepDAO> WorkflowStepDAOs = DataContext.WorkflowStep.AsNoTracking();
            WorkflowStepDAOs = DynamicFilter(WorkflowStepDAOs, filter);
            WorkflowStepDAOs = OrFilter(WorkflowStepDAOs, filter);
            WorkflowStepDAOs = DynamicOrder(WorkflowStepDAOs, filter);
            List<WorkflowStep> WorkflowSteps = await DynamicSelect(WorkflowStepDAOs, filter);
            return WorkflowSteps;
        }

        public async Task<WorkflowStep> Get(long Id)
        {
            WorkflowStep WorkflowStep = await DataContext.WorkflowStep.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new WorkflowStep()
            {
                Id = x.Id,
                WorkflowDefinitionId = x.WorkflowDefinitionId,
                Code = x.Code,
                Name = x.Name,
                RoleId = x.RoleId,
                StatusId = x.StatusId,
                SubjectMailForReject = x.SubjectMailForReject,
                BodyMailForReject = x.BodyMailForReject,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Status = x.Status == null ? null : new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
                Role = x.Role == null ? null : new Role
                {
                    Id = x.Role.Id,
                    Code = x.Role.Code,
                    Name = x.Role.Name,
                    StatusId = x.Role.StatusId,
                },
                WorkflowDefinition = x.WorkflowDefinition == null ? null : new WorkflowDefinition
                {
                    Id = x.WorkflowDefinition.Id,
                    Code = x.WorkflowDefinition.Code,
                    Name = x.WorkflowDefinition.Name,
                    WorkflowTypeId = x.WorkflowDefinition.WorkflowTypeId,
                    StartDate = x.WorkflowDefinition.StartDate,
                    EndDate = x.WorkflowDefinition.EndDate,
                    StatusId = x.WorkflowDefinition.StatusId,
                    UpdatedAt = x.WorkflowDefinition.UpdatedAt,
                },
                Used = x.WorkflowDefinition.Used,
            }).FirstOrDefaultWithNoLockAsync();

            if (WorkflowStep == null)
                return null;

            return WorkflowStep;
        }
        public async Task<bool> Create(WorkflowStep WorkflowStep)
        {
            WorkflowDefinitionDAO WorkflowDefinitionDAO = await DataContext.WorkflowDefinition
                .Where(x => x.Id == WorkflowStep.WorkflowDefinitionId)
                .FirstOrDefaultWithNoLockAsync();
            if (WorkflowDefinitionDAO == null)
                return false;
            WorkflowDefinitionDAO.ModifierId = WorkflowStep.ModifierId;
            await DataContext.SaveChangesAsync();
            if (WorkflowDefinitionDAO.Used == false)
            {
                WorkflowStepDAO WorkflowStepDAO = new WorkflowStepDAO();
                WorkflowStepDAO.Id = WorkflowStep.Id;
                WorkflowStepDAO.WorkflowDefinitionId = WorkflowStep.WorkflowDefinitionId;
                WorkflowStepDAO.Code = WorkflowStep.Code;
                WorkflowStepDAO.Name = WorkflowStep.Name;
                WorkflowStepDAO.RoleId = WorkflowStep.RoleId;
                WorkflowStepDAO.SubjectMailForReject = WorkflowStep.SubjectMailForReject;
                WorkflowStepDAO.BodyMailForReject = WorkflowStep.BodyMailForReject;
                WorkflowStepDAO.CreatedAt = StaticParams.DateTimeNow;
                WorkflowStepDAO.UpdatedAt = StaticParams.DateTimeNow;
                WorkflowStepDAO.StatusId = WorkflowStep.StatusId;
                DataContext.WorkflowStep.Add(WorkflowStepDAO);
                await DataContext.SaveChangesAsync();
                WorkflowStep.Id = WorkflowStepDAO.Id;
            }
            return true;
        }

        public async Task<bool> Update(WorkflowStep WorkflowStep)
        {
            WorkflowStepDAO WorkflowStepDAO = await DataContext.WorkflowStep
                .Where(x => x.Id == WorkflowStep.Id)
                .FirstOrDefaultWithNoLockAsync();
            if (WorkflowStepDAO == null)
                return false;
            WorkflowDefinitionDAO WorkflowDefinitionDAO = await DataContext.WorkflowDefinition
                .Where(x => x.Id == WorkflowStepDAO.WorkflowDefinitionId)
                .FirstOrDefaultWithNoLockAsync();
            if (WorkflowDefinitionDAO == null)
                return false;
            WorkflowDefinitionDAO.ModifierId = WorkflowStep.ModifierId;
            await DataContext.SaveChangesAsync();

            WorkflowStepDAO.Code = WorkflowStep.Code;
            WorkflowStepDAO.Name = WorkflowStep.Name;
            WorkflowStepDAO.RoleId = WorkflowStep.RoleId;
            WorkflowStepDAO.StatusId = WorkflowStep.StatusId;
            WorkflowStepDAO.SubjectMailForReject = WorkflowStep.SubjectMailForReject;
            WorkflowStepDAO.BodyMailForReject = WorkflowStep.BodyMailForReject;
            WorkflowStepDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(WorkflowStep WorkflowStep)
        {
            WorkflowDefinitionDAO WorkflowDefinitionDAO = await DataContext.WorkflowDefinition
                .Where(x => x.Id == WorkflowStep.WorkflowDefinitionId)
                .FirstOrDefaultWithNoLockAsync();
            if (WorkflowDefinitionDAO == null)
                return false;
            if (WorkflowDefinitionDAO.Used == false)
            {
                WorkflowDefinitionDAO.ModifierId = WorkflowStep.ModifierId;
                await DataContext.SaveChangesAsync();

                await DataContext.WorkflowDirection.Where(x => x.FromStepId == WorkflowStep.Id).DeleteFromQueryAsync();
                await DataContext.WorkflowDirection.Where(x => x.ToStepId == WorkflowStep.Id).DeleteFromQueryAsync();
                await DataContext.WorkflowStep.Where(x => x.Id == WorkflowStep.Id).UpdateFromQueryAsync(x => new WorkflowStepDAO { DeletedAt = StaticParams.DateTimeNow });
                return true;
            }
            return false;
        }

        public async Task<bool> BulkMerge(List<WorkflowStep> WorkflowSteps)
        {
            List<WorkflowStepDAO> WorkflowStepDAOs = new List<WorkflowStepDAO>();
            foreach (WorkflowStep WorkflowStep in WorkflowSteps)
            {
                WorkflowStepDAO WorkflowStepDAO = new WorkflowStepDAO();
                WorkflowStepDAO.Id = WorkflowStep.Id;
                WorkflowStepDAO.WorkflowDefinitionId = WorkflowStep.WorkflowDefinitionId;
                WorkflowStepDAO.Code = WorkflowStep.Code;
                WorkflowStepDAO.Name = WorkflowStep.Name;
                WorkflowStepDAO.RoleId = WorkflowStep.RoleId;
                WorkflowStepDAO.StatusId = WorkflowStep.StatusId;
                WorkflowStepDAO.SubjectMailForReject = WorkflowStep.SubjectMailForReject;
                WorkflowStepDAO.BodyMailForReject = WorkflowStep.BodyMailForReject;
                WorkflowStepDAO.CreatedAt = StaticParams.DateTimeNow;
                WorkflowStepDAO.UpdatedAt = StaticParams.DateTimeNow;
                WorkflowStepDAOs.Add(WorkflowStepDAO);
            }
            await DataContext.BulkMergeAsync(WorkflowStepDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<WorkflowStep> WorkflowSteps)
        {
            List<long> Ids = WorkflowSteps.Select(x => x.Id).ToList();
            await DataContext.WorkflowDirection.Where(x => Ids.Contains(x.FromStepId)).DeleteFromQueryAsync();
            await DataContext.WorkflowDirection.Where(x => Ids.Contains(x.ToStepId)).DeleteFromQueryAsync();
            await DataContext.WorkflowStep
                .Where(x => Ids.Contains(x.Id)).UpdateFromQueryAsync(x => new WorkflowStepDAO { DeletedAt = StaticParams.DateTimeNow });
            return true;
        }

    }
}
