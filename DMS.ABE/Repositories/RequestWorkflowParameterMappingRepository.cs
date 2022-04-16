using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IRequestWorkflowParameterMappingRepository
    {
        Task<List<RequestWorkflowParameterMapping>> List(Guid RequestId);
        Task<bool> BulkMerge(Guid RequestId, List<RequestWorkflowParameterMapping> RequestWorkflowParameterMappings);
    }
    public class RequestWorkflowParameterMappingRepository : IRequestWorkflowParameterMappingRepository
    {
        private DataContext DataContext;
        public RequestWorkflowParameterMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<List<RequestWorkflowParameterMapping>> List(Guid RequestId)
        {
            List<RequestWorkflowParameterMapping> RequestWorkflowParameterMappings = await DataContext.RequestWorkflowParameterMapping
                .Where(r => r.RequestId == RequestId).AsNoTracking()
                .Select(r => new RequestWorkflowParameterMapping
                {
                    RequestId = r.RequestId,
                    Value = r.Value,
                    WorkflowParameterId = r.WorkflowParameterId,
                    WorkflowParameterTypeId = r.WorkflowParameter.WorkflowParameterTypeId,
                }).ToListAsync();
            return RequestWorkflowParameterMappings;
        }
        public async Task<bool> BulkMerge(Guid RequestId, List<RequestWorkflowParameterMapping> RequestWorkflowParameterMappings)
        {
            await DataContext.RequestWorkflowParameterMapping
                .Where(r => r.RequestId == RequestId)
                .DeleteFromQueryAsync();
            List<RequestWorkflowParameterMappingDAO> RequestWorkflowParameterMappingDAOs = RequestWorkflowParameterMappings
                .Select(x => new RequestWorkflowParameterMappingDAO
                {
                    RequestId = RequestId,
                    WorkflowParameterId = x.WorkflowParameterId,
                    Value = x.Value,
                }).ToList();
            await DataContext.RequestWorkflowParameterMapping.BulkInsertAsync(RequestWorkflowParameterMappingDAOs);
            return true;
        }
    }
}
