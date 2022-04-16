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
    public interface IWorkflowDirectionRepository
    {
        Task<int> Count(WorkflowDirectionFilter WorkflowDirectionFilter);
        Task<int> CountAll(WorkflowDirectionFilter WorkflowDirectionFilter);
        Task<List<WorkflowDirection>> List(WorkflowDirectionFilter WorkflowDirectionFilter);
        Task<WorkflowDirection> Get(long Id);
        Task<bool> Create(WorkflowDirection WorkflowDirection);
        Task<bool> Update(WorkflowDirection WorkflowDirection);
        Task<bool> Delete(WorkflowDirection WorkflowDirection);
        Task<bool> BulkMerge(List<WorkflowDirection> WorkflowDirections);
        Task<bool> BulkDelete(List<WorkflowDirection> WorkflowDirections);
    }
    public class WorkflowDirectionRepository : IWorkflowDirectionRepository
    {
        private DataContext DataContext;
        public WorkflowDirectionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<WorkflowDirectionDAO> DynamicFilter(IQueryable<WorkflowDirectionDAO> query, WorkflowDirectionFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.WorkflowDefinitionId, filter.WorkflowDefinitionId);
            query = query.Where(q => q.FromStepId, filter.FromStepId);
            query = query.Where(q => q.ToStepId, filter.ToStepId);
            query = query.Where(q => q.StatusId, filter.StatusId);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            return query;
        }

        private IQueryable<WorkflowDirectionDAO> OrFilter(IQueryable<WorkflowDirectionDAO> query, WorkflowDirectionFilter filter)
        {
            if (filter.OrFilter == null || filter.OrFilter.Count == 0)
                return query;
            IQueryable<WorkflowDirectionDAO> initQuery = query.Where(q => false);
            foreach (WorkflowDirectionFilter WorkflowDirectionFilter in filter.OrFilter)
            {
                IQueryable<WorkflowDirectionDAO> queryable = query;
                queryable = queryable.Where(q => q.Id, WorkflowDirectionFilter.Id);
                queryable = queryable.Where(q => q.WorkflowDefinitionId, WorkflowDirectionFilter.WorkflowDefinitionId);
                queryable = queryable.Where(q => q.FromStepId, WorkflowDirectionFilter.FromStepId);
                queryable = queryable.Where(q => q.ToStepId, WorkflowDirectionFilter.ToStepId);
                queryable = queryable.Where(q => q.StatusId, WorkflowDirectionFilter.StatusId);
                queryable = queryable.Where(q => q.UpdatedAt, WorkflowDirectionFilter.UpdatedAt);
                initQuery = initQuery.Union(queryable);
            }
            return initQuery;
        }

        private IQueryable<WorkflowDirectionDAO> DynamicOrder(IQueryable<WorkflowDirectionDAO> query, WorkflowDirectionFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowDirectionOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case WorkflowDirectionOrder.WorkflowDefinition:
                            query = query.OrderBy(q => q.WorkflowDefinition.Name);
                            break;
                        case WorkflowDirectionOrder.FromStep:
                            query = query.OrderBy(q => q.FromStep.Name);
                            break;
                        case WorkflowDirectionOrder.ToStep:
                            query = query.OrderBy(q => q.ToStep.Name);
                            break;
                        case WorkflowDirectionOrder.Status:
                            query = query.OrderBy(q => q.StatusId);
                            break;
                        case WorkflowDirectionOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case WorkflowDirectionOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case WorkflowDirectionOrder.WorkflowDefinition:
                            query = query.OrderByDescending(q => q.WorkflowDefinition.Name);
                            break;
                        case WorkflowDirectionOrder.FromStep:
                            query = query.OrderByDescending(q => q.FromStep.Name);
                            break;
                        case WorkflowDirectionOrder.ToStep:
                            query = query.OrderByDescending(q => q.ToStep.Name);
                            break;
                        case WorkflowDirectionOrder.Status:
                            query = query.OrderByDescending(q => q.StatusId);
                            break;
                        case WorkflowDirectionOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<WorkflowDirection>> DynamicSelect(IQueryable<WorkflowDirectionDAO> query, WorkflowDirectionFilter filter)
        {
            List<WorkflowDirection> WorkflowDirections = await query.Select(q => new WorkflowDirection()
            {
                Id = filter.Selects.Contains(WorkflowDirectionSelect.Id) ? q.Id : default(long),
                WorkflowDefinitionId = filter.Selects.Contains(WorkflowDirectionSelect.WorkflowDefinition) ? q.WorkflowDefinitionId : default(long),
                FromStepId = filter.Selects.Contains(WorkflowDirectionSelect.FromStep) ? q.FromStepId : default(long),
                ToStepId = filter.Selects.Contains(WorkflowDirectionSelect.ToStep) ? q.ToStepId : default(long),
                StatusId = filter.Selects.Contains(WorkflowDirectionSelect.Status) ? q.StatusId : default(long),
                UpdatedAt = filter.Selects.Contains(WorkflowDirectionSelect.UpdatedAt) ? q.UpdatedAt : default(DateTime),
                FromStep = filter.Selects.Contains(WorkflowDirectionSelect.FromStep) && q.FromStep != null ? new WorkflowStep
                {
                    Id = q.FromStep.Id,
                    WorkflowDefinitionId = q.FromStep.WorkflowDefinitionId,
                    Code = q.FromStep.Code,
                    Name = q.FromStep.Name,
                    RoleId = q.FromStep.RoleId,
                    SubjectMailForReject = q.FromStep.SubjectMailForReject,
                    BodyMailForReject = q.FromStep.BodyMailForReject,
                } : null,
                ToStep = filter.Selects.Contains(WorkflowDirectionSelect.ToStep) && q.ToStep != null ? new WorkflowStep
                {
                    Id = q.ToStep.Id,
                    WorkflowDefinitionId = q.ToStep.WorkflowDefinitionId,
                    Code = q.ToStep.Code,
                    Name = q.ToStep.Name,
                    RoleId = q.ToStep.RoleId,
                    SubjectMailForReject = q.ToStep.SubjectMailForReject,
                    BodyMailForReject = q.ToStep.BodyMailForReject,
                } : null,
                Status = filter.Selects.Contains(WorkflowDirectionSelect.Status) && q.Status != null ? new Status
                {
                    Id = q.Status.Id,
                    Code = q.Status.Code,
                    Name = q.Status.Name,
                } : null,
                WorkflowDefinition = filter.Selects.Contains(WorkflowDirectionSelect.WorkflowDefinition) && q.WorkflowDefinition != null ? new WorkflowDefinition
                {
                    Id = q.WorkflowDefinition.Id,
                    Code = q.WorkflowDefinition.Code,
                    Name = q.WorkflowDefinition.Name,
                    WorkflowTypeId = q.WorkflowDefinition.WorkflowTypeId,
                    StartDate = q.WorkflowDefinition.StartDate,
                    EndDate = q.WorkflowDefinition.EndDate,
                    StatusId = q.WorkflowDefinition.StatusId,
                } : null,
                Used = q.WorkflowDefinition.Used,
            }).ToListWithNoLockAsync();
            return WorkflowDirections;
        }

        public async Task<int> Count(WorkflowDirectionFilter filter)
        {
            IQueryable<WorkflowDirectionDAO> WorkflowDirections = DataContext.WorkflowDirection.AsNoTracking();
            WorkflowDirections = DynamicFilter(WorkflowDirections, filter);
            WorkflowDirections = OrFilter(WorkflowDirections, filter);
            return await WorkflowDirections.CountWithNoLockAsync();
        }
        public async Task<int> CountAll(WorkflowDirectionFilter filter)
        {
            IQueryable<WorkflowDirectionDAO> WorkflowDirections = DataContext.WorkflowDirection.AsNoTracking();
            WorkflowDirections = DynamicFilter(WorkflowDirections, filter);
            return await WorkflowDirections.CountWithNoLockAsync();
        }

        public async Task<List<WorkflowDirection>> List(WorkflowDirectionFilter filter)
        {
            if (filter == null) return new List<WorkflowDirection>();
            IQueryable<WorkflowDirectionDAO> WorkflowDirectionDAOs = DataContext.WorkflowDirection.AsNoTracking();
            WorkflowDirectionDAOs = DynamicFilter(WorkflowDirectionDAOs, filter);
            WorkflowDirectionDAOs = OrFilter(WorkflowDirectionDAOs, filter);
            WorkflowDirectionDAOs = DynamicOrder(WorkflowDirectionDAOs, filter);
            List<WorkflowDirection> WorkflowDirections = await DynamicSelect(WorkflowDirectionDAOs, filter);
            return WorkflowDirections;
        }

        public async Task<WorkflowDirection> Get(long Id)
        {
            WorkflowDirection WorkflowDirection = await DataContext.WorkflowDirection.AsNoTracking()
            .Where(x => x.Id == Id).Select(x => new WorkflowDirection()
            {
                Id = x.Id,
                WorkflowDefinitionId = x.WorkflowDefinitionId,
                FromStepId = x.FromStepId,
                ToStepId = x.ToStepId,
                SubjectMailForCreator = x.SubjectMailForCreator,
                SubjectMailForCurrentStep = x.SubjectMailForCurrentStep,
                SubjectMailForNextStep = x.SubjectMailForNextStep,
                BodyMailForCreator = x.BodyMailForCreator,
                BodyMailForCurrentStep = x.BodyMailForCurrentStep,
                BodyMailForNextStep = x.BodyMailForNextStep,
                StatusId = x.StatusId,
                UpdatedAt = x.UpdatedAt,
                FromStep = x.FromStep == null ? null : new WorkflowStep
                {
                    Id = x.FromStep.Id,
                    WorkflowDefinitionId = x.FromStep.WorkflowDefinitionId,
                    Code = x.FromStep.Code,
                    Name = x.FromStep.Name,
                    RoleId = x.FromStep.RoleId,
                    SubjectMailForReject = x.FromStep.SubjectMailForReject,
                    BodyMailForReject = x.FromStep.BodyMailForReject,
                    Role = new Role
                    {
                        Id = x.FromStep.Role.Id,
                        Code = x.FromStep.Role.Code,
                        Name = x.FromStep.Role.Name,
                    },
                },
                ToStep = x.ToStep == null ? null : new WorkflowStep
                {
                    Id = x.ToStep.Id,
                    WorkflowDefinitionId = x.ToStep.WorkflowDefinitionId,
                    Code = x.ToStep.Code,
                    Name = x.ToStep.Name,
                    RoleId = x.ToStep.RoleId,
                    SubjectMailForReject = x.ToStep.SubjectMailForReject,
                    BodyMailForReject = x.ToStep.BodyMailForReject,
                    Role = new Role
                    {
                        Id = x.ToStep.Role.Id,
                        Code = x.ToStep.Role.Code,
                        Name = x.ToStep.Role.Name,
                    },
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
                Status = new Status
                {
                    Id = x.Status.Id,
                    Code = x.Status.Code,
                    Name = x.Status.Name,
                },
                Used = x.WorkflowDefinition.Used,
            }).FirstOrDefaultWithNoLockAsync();

            if (WorkflowDirection == null)
                return null;
            WorkflowDirection.WorkflowDirectionConditions = await DataContext.WorkflowDirectionCondition
                .AsNoTracking()
                .Where(x => x.WorkflowDirectionId == WorkflowDirection.Id)
                .Select(x => new WorkflowDirectionCondition
                {
                    Id = x.Id,
                    Value = x.Value,
                    WorkflowDirectionId = x.WorkflowDirectionId,
                    WorkflowOperatorId = x.WorkflowOperatorId,
                    WorkflowParameterId = x.WorkflowParameterId,
                    Used = x.WorkflowDirection.WorkflowDefinition.Used,
                    WorkflowOperator = new WorkflowOperator
                    {
                        Id = x.WorkflowOperator.Id,
                        Code = x.WorkflowOperator.Code,
                        Name = x.WorkflowOperator.Name,
                        WorkflowParameterTypeId = x.WorkflowOperator.WorkflowParameterTypeId,
                    },
                    WorkflowParameter = new WorkflowParameter
                    {
                        Id = x.WorkflowParameter.Id,
                        Code = x.WorkflowParameter.Code,
                        Name = x.WorkflowParameter.Name,
                        WorkflowParameterTypeId = x.WorkflowParameter.WorkflowParameterTypeId,
                        WorkflowTypeId = x.WorkflowParameter.WorkflowTypeId,
                    },
                }).ToListWithNoLockAsync();

            WorkflowDirection.WorkflowParameters = await DataContext.WorkflowParameter
                .Where(x => x.WorkflowTypeId == WorkflowDirection.WorkflowDefinition.WorkflowTypeId)
                .Select(x => new WorkflowParameter
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    WorkflowTypeId = x.WorkflowTypeId,
                    WorkflowParameterTypeId = x.WorkflowParameterTypeId,
                }).ToListWithNoLockAsync();
            return WorkflowDirection;
        }
        public async Task<bool> Create(WorkflowDirection WorkflowDirection)
        {
            WorkflowDefinitionDAO WorkflowDefinitionDAO = await DataContext.WorkflowDefinition
               .Where(x => x.Id == WorkflowDirection.WorkflowDefinitionId)
               .FirstOrDefaultWithNoLockAsync();
            if (WorkflowDefinitionDAO == null)
                return false;
            WorkflowDefinitionDAO.ModifierId = WorkflowDirection.ModifierId;

            if (WorkflowDefinitionDAO.Used == false)
            {
                WorkflowDirectionDAO WorkflowDirectionDAO = new WorkflowDirectionDAO();
                WorkflowDirectionDAO.Id = WorkflowDirection.Id;
                WorkflowDirectionDAO.WorkflowDefinitionId = WorkflowDirection.WorkflowDefinitionId;
                WorkflowDirectionDAO.FromStepId = WorkflowDirection.FromStepId;
                WorkflowDirectionDAO.ToStepId = WorkflowDirection.ToStepId;
                WorkflowDirectionDAO.StatusId = WorkflowDirection.StatusId;
                WorkflowDirectionDAO.SubjectMailForCreator = WorkflowDirection.SubjectMailForCreator;
                WorkflowDirectionDAO.SubjectMailForCurrentStep = WorkflowDirection.SubjectMailForCurrentStep;
                WorkflowDirectionDAO.SubjectMailForNextStep = WorkflowDirection.SubjectMailForNextStep;
                WorkflowDirectionDAO.BodyMailForCreator = WorkflowDirection.BodyMailForCreator;
                WorkflowDirectionDAO.BodyMailForCurrentStep = WorkflowDirection.BodyMailForCurrentStep;
                WorkflowDirectionDAO.BodyMailForNextStep = WorkflowDirection.BodyMailForNextStep;
                WorkflowDirectionDAO.UpdatedAt = StaticParams.DateTimeNow;
                DataContext.WorkflowDirection.Add(WorkflowDirectionDAO);
                await DataContext.SaveChangesAsync();
                WorkflowDirection.Id = WorkflowDirectionDAO.Id;
                await SaveReference(WorkflowDirection);
            }
            return true;
        }

        public async Task<bool> Update(WorkflowDirection WorkflowDirection)
        {
            WorkflowDirectionDAO WorkflowDirectionDAO = await DataContext.WorkflowDirection
                .Where(x => x.Id == WorkflowDirection.Id)
                .FirstOrDefaultWithNoLockAsync();
            if (WorkflowDirectionDAO == null)
                return false;
            WorkflowDefinitionDAO WorkflowDefinitionDAO = await DataContext.WorkflowDefinition
                .Where(x => x.Id == WorkflowDirectionDAO.WorkflowDefinitionId)
                .FirstOrDefaultWithNoLockAsync();

            WorkflowDefinitionDAO.ModifierId = WorkflowDirection.ModifierId;

            WorkflowDirectionDAO.FromStepId = WorkflowDirection.FromStepId;
            WorkflowDirectionDAO.ToStepId = WorkflowDirection.ToStepId;
            WorkflowDirectionDAO.StatusId = WorkflowDirection.StatusId;
            WorkflowDirectionDAO.SubjectMailForCreator = WorkflowDirection.SubjectMailForCreator;
            WorkflowDirectionDAO.SubjectMailForCurrentStep = WorkflowDirection.SubjectMailForCurrentStep;
            WorkflowDirectionDAO.SubjectMailForNextStep = WorkflowDirection.SubjectMailForNextStep;
            WorkflowDirectionDAO.BodyMailForCreator = WorkflowDirection.BodyMailForCreator;
            WorkflowDirectionDAO.BodyMailForCurrentStep = WorkflowDirection.BodyMailForCurrentStep;
            WorkflowDirectionDAO.BodyMailForNextStep = WorkflowDirection.BodyMailForNextStep;
            WorkflowDirectionDAO.UpdatedAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            await SaveReference(WorkflowDirection);
            return true;
        }

        public async Task<bool> Delete(WorkflowDirection WorkflowDirection)
        {
            WorkflowDirectionDAO WorkflowDirectionDAO = await DataContext.WorkflowDirection
               .Where(x => x.Id == WorkflowDirection.Id)
               .FirstOrDefaultWithNoLockAsync();

            await DataContext.WorkflowDirectionCondition.Where(x => x.WorkflowDirectionId == WorkflowDirection.Id).DeleteFromQueryAsync();
            await DataContext.WorkflowDirection.Where(x => x.Id == WorkflowDirection.Id).DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> BulkMerge(List<WorkflowDirection> WorkflowDirections)
        {
            List<WorkflowDirectionDAO> WorkflowDirectionDAOs = new List<WorkflowDirectionDAO>();
            foreach (WorkflowDirection WorkflowDirection in WorkflowDirections)
            {
                WorkflowDirection.RowId = Guid.NewGuid();
                WorkflowDirectionDAO WorkflowDirectionDAO = new WorkflowDirectionDAO();
                WorkflowDirectionDAO.Id = WorkflowDirection.Id;
                WorkflowDirectionDAO.WorkflowDefinitionId = WorkflowDirection.WorkflowDefinitionId;
                WorkflowDirectionDAO.FromStepId = WorkflowDirection.FromStepId;
                WorkflowDirectionDAO.ToStepId = WorkflowDirection.ToStepId;
                WorkflowDirectionDAO.StatusId = WorkflowDirection.StatusId;
                WorkflowDirectionDAO.SubjectMailForCreator = WorkflowDirection.SubjectMailForCreator;
                WorkflowDirectionDAO.SubjectMailForCurrentStep = WorkflowDirection.SubjectMailForCurrentStep;
                WorkflowDirectionDAO.SubjectMailForNextStep = WorkflowDirection.SubjectMailForNextStep;
                WorkflowDirectionDAO.BodyMailForCreator = WorkflowDirection.BodyMailForCreator;
                WorkflowDirectionDAO.BodyMailForCurrentStep = WorkflowDirection.BodyMailForCurrentStep;
                WorkflowDirectionDAO.BodyMailForNextStep = WorkflowDirection.BodyMailForNextStep;
                WorkflowDirectionDAO.UpdatedAt = StaticParams.DateTimeNow;
                WorkflowDirectionDAO.RowId = WorkflowDirection.RowId;
                WorkflowDirectionDAOs.Add(WorkflowDirectionDAO);
            }
            await DataContext.BulkMergeAsync(WorkflowDirectionDAOs);
            List<WorkflowDirectionConditionDAO> WorkflowDirectionConditionDAOs = new List<WorkflowDirectionConditionDAO>();
            foreach (WorkflowDirection WorkflowDirection in WorkflowDirections)
            {
                long Id = WorkflowDirectionDAOs.Where(x => x.RowId == WorkflowDirection.RowId).Select(x => x.Id).FirstOrDefault();
                foreach (WorkflowDirectionCondition WorkflowDirectionCondition in WorkflowDirection.WorkflowDirectionConditions)
                {
                    WorkflowDirectionConditionDAO WorkflowDirectionConditionDAO = new WorkflowDirectionConditionDAO
                    {
                        WorkflowDirectionId = Id,
                        Value = WorkflowDirectionCondition.Value,
                        WorkflowOperatorId = WorkflowDirectionCondition.WorkflowOperatorId,
                        WorkflowParameterId = WorkflowDirectionCondition.WorkflowParameterId,
                    };
                    WorkflowDirectionConditionDAOs.Add(WorkflowDirectionConditionDAO);
                }
            }
            await DataContext.WorkflowDirectionCondition.BulkMergeAsync(WorkflowDirectionConditionDAOs);
            return true;
        }

        public async Task<bool> BulkDelete(List<WorkflowDirection> WorkflowDirections)
        {
            List<long> Ids = WorkflowDirections.Select(x => x.Id).ToList();
            await DataContext.WorkflowDirectionCondition
                .Where(x => Ids.Contains(x.WorkflowDirectionId)).DeleteFromQueryAsync();
            await DataContext.WorkflowDirection
                .Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(WorkflowDirection WorkflowDirection)
        {
            await DataContext.WorkflowDirectionCondition.Where(x => x.WorkflowDirectionId == WorkflowDirection.Id).DeleteFromQueryAsync();
            if (WorkflowDirection.WorkflowDirectionConditions != null)
            {
                List<WorkflowDirectionConditionDAO> WorkflowDirectionConditionDAOs = new List<WorkflowDirectionConditionDAO>();
                foreach (WorkflowDirectionCondition WorkflowDirectionCondition in WorkflowDirection.WorkflowDirectionConditions)
                {
                    WorkflowDirectionConditionDAO WorkflowDirectionConditionDAO = new WorkflowDirectionConditionDAO
                    {
                        Value = WorkflowDirectionCondition.Value,
                        WorkflowDirectionId = WorkflowDirection.Id,
                        WorkflowOperatorId = WorkflowDirectionCondition.WorkflowOperatorId,
                        WorkflowParameterId = WorkflowDirectionCondition.WorkflowParameterId,
                    };
                    WorkflowDirectionConditionDAOs.Add(WorkflowDirectionConditionDAO);
                }
                await DataContext.WorkflowDirectionCondition.BulkMergeAsync(WorkflowDirectionConditionDAOs);
            }
        }

    }
}
