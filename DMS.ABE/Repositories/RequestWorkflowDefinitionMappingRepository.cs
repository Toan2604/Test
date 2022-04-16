using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface IRequestWorkflowDefinitionMappingRepository
    {
        Task<long> Count(RequestWorkflowDefinitionMappingFilter filter);
        Task<List<RequestWorkflowDefinitionMapping>> List(RequestWorkflowDefinitionMappingFilter filter);
        Task<RequestWorkflowDefinitionMapping> Get(Guid RequestId);
        Task<RequestWorkflowDefinitionMapping> Update(RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping);
        Task<bool> Delete(Guid RequestId);
    }
    public class RequestWorkflowDefinitionMappingRepository : IRequestWorkflowDefinitionMappingRepository
    {
        private DataContext DataContext;
        public RequestWorkflowDefinitionMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<long> Count(RequestWorkflowDefinitionMappingFilter filter)
        {
            IQueryable<RequestWorkflowDefinitionMappingDAO> query = DataContext.RequestWorkflowDefinitionMapping.AsNoTracking();
            if (filter.RequestId != null)
                query = query.Where(q => q.RequestId, filter.RequestId);
            if (filter.WorkflowDefinitionId != null)
                query = query.Where(q => q.WorkflowDefinitionId, filter.WorkflowDefinitionId);
            return await query.CountAsync();
        }
        public async Task<RequestWorkflowDefinitionMapping> Get(Guid RequestId)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await DataContext.RequestWorkflowDefinitionMapping.AsNoTracking()
                .Where(q => q.RequestId == RequestId)
                .Select(q => new RequestWorkflowDefinitionMapping
                {
                    RequestId = q.RequestId,
                    RequestStateId = q.RequestStateId,
                    WorkflowDefinitionId = q.WorkflowDefinitionId,
                    Counter = q.Counter,
                    CreatorId = q.CreatorId,
                    RequestState = new RequestState
                    {
                        Id = q.RequestState.Id,
                        Code = q.RequestState.Code,
                        Name = q.RequestState.Name,
                    },
                }).FirstOrDefaultAsync();
            return RequestWorkflowDefinitionMapping;
        }

        public async Task<RequestWorkflowDefinitionMapping> Update(RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping)
        {
            RequestWorkflowDefinitionMappingDAO RequestWorkflowDefinitionMappingDAO = await DataContext.RequestWorkflowDefinitionMapping
                .Where(q => q.RequestId == RequestWorkflowDefinitionMapping.RequestId).FirstOrDefaultAsync();
            if (RequestWorkflowDefinitionMappingDAO == null)
            {
                RequestWorkflowDefinitionMappingDAO = new RequestWorkflowDefinitionMappingDAO
                {
                    RequestId = RequestWorkflowDefinitionMapping.RequestId,
                    RequestStateId = RequestWorkflowDefinitionMapping.RequestStateId,
                    WorkflowDefinitionId = RequestWorkflowDefinitionMapping.WorkflowDefinitionId,
                    CreatorId = RequestWorkflowDefinitionMapping.CreatorId,
                    Counter = RequestWorkflowDefinitionMapping.Counter,
                };
                DataContext.RequestWorkflowDefinitionMapping.Add(RequestWorkflowDefinitionMappingDAO);
            }
            else
            {
                RequestWorkflowDefinitionMappingDAO.RequestStateId = RequestWorkflowDefinitionMapping.RequestStateId;
                RequestWorkflowDefinitionMappingDAO.WorkflowDefinitionId = RequestWorkflowDefinitionMapping.WorkflowDefinitionId;
                RequestWorkflowDefinitionMappingDAO.Counter = RequestWorkflowDefinitionMapping.Counter;
            }
            RequestWorkflowDefinitionMappingDAO.Counter = RequestWorkflowDefinitionMapping.Counter;
            await DataContext.SaveChangesAsync();
            RequestWorkflowDefinitionMapping.RequestId = RequestWorkflowDefinitionMappingDAO.RequestId;

            RequestWorkflowDefinitionMappingDAO = await DataContext.RequestWorkflowDefinitionMapping
               .Where(x => x.RequestId == RequestWorkflowDefinitionMapping.RequestId).FirstOrDefaultAsync();
            if (RequestWorkflowDefinitionMappingDAO != null)
            {
                long counter = RequestWorkflowDefinitionMappingDAO.Counter;
                List<RequestWorkflowStepMappingDAO> RequestWorkflowStepMappingDAOs = DataContext.RequestWorkflowStepMapping
                  .Where(r => r.RequestId == RequestWorkflowDefinitionMapping.RequestId).ToList();
                await DataContext.RequestWorkflowHistory.Where(x => x.Counter == counter && x.RequestId == RequestWorkflowDefinitionMapping.RequestId)
                            .DeleteFromQueryAsync();
                List<RequestWorkflowHistoryDAO> RequestWorkflowHistoryDAOs = RequestWorkflowStepMappingDAOs
                    .Select(x => new RequestWorkflowHistoryDAO
                    {
                        RequestId = x.RequestId,
                        AppUserId = x.AppUserId,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Counter = counter,
                        WorkflowStateId = x.WorkflowStateId,
                        WorkflowStepId = x.WorkflowStepId,
                    }).ToList();
                await DataContext.RequestWorkflowHistory.BulkMergeAsync(RequestWorkflowHistoryDAOs);
            }
            return RequestWorkflowDefinitionMapping;
        }

        public async Task<List<RequestWorkflowDefinitionMapping>> List(RequestWorkflowDefinitionMappingFilter filter)
        {
            IQueryable<RequestWorkflowDefinitionMappingDAO> query = DataContext.RequestWorkflowDefinitionMapping.AsNoTracking();
            if (filter.RequestId != null)
                query = query.Where(q => q.RequestId, filter.RequestId);
            if (filter.WorkflowDefinitionId != null)
                query = query.Where(q => q.WorkflowDefinitionId, filter.WorkflowDefinitionId);
            query = query.OrderBy(q => q.RequestId).Skip(0).Take(int.MaxValue);
            var result = await query.Select(q => new RequestWorkflowDefinitionMapping
            {
                RequestId = q.RequestId,
                RequestStateId = q.RequestStateId,
                WorkflowDefinitionId = q.WorkflowDefinitionId,
                Counter = q.Counter,
                CreatorId = q.CreatorId,
            }).ToListAsync();
            return result;
        }

        public async Task<bool> Delete(Guid RequestId)
        {
            await DataContext.RequestWorkflowStepMapping.Where(r => r.RequestId == RequestId).DeleteFromQueryAsync();
            await DataContext.RequestWorkflowParameterMapping.Where(r => r.RequestId == RequestId).DeleteFromQueryAsync();
            await DataContext.RequestWorkflowDefinitionMapping.Where(r => r.RequestId == RequestId).DeleteFromQueryAsync();
            return true;
        }
    }
}
