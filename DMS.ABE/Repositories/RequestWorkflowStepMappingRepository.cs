using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Repositories
{
    public interface IRequestWorkflowStepMappingRepository
    {
        Task<long> Count(RequestWorkflowStepMappingFilter filter);
        Task<List<RequestWorkflowStepMapping>> List(Guid RequestId);
        Task<bool> BulkMerge(Guid RequestId, List<RequestWorkflowStepMapping> RequestWorkflowStepMappings);
    }
    public class RequestWorkflowStepMappingRepository : IRequestWorkflowStepMappingRepository
    {
        private DataContext DataContext;
        public RequestWorkflowStepMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<long> Count(RequestWorkflowStepMappingFilter filter)
        {
            IQueryable<RequestWorkflowStepMappingDAO> query = DataContext.RequestWorkflowStepMapping.AsNoTracking();
            if (filter.RequestId != null)
                query = query.Where(q => q.RequestId, filter.RequestId);
            if (filter.WorkflowStepId != null)
                query = query.Where(q => q.WorkflowStepId, filter.WorkflowStepId);
            return await query.CountAsync();
        }

        public async Task<List<RequestWorkflowStepMapping>> List(Guid RequestId)
        {
            List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = await DataContext.RequestWorkflowStepMapping.AsNoTracking()
                .Where(r => r.RequestId == RequestId)
                .Select(r => new RequestWorkflowStepMapping
                {
                    AppUserId = r.AppUserId,
                    CreatedAt = r.CreatedAt,
                    RequestId = r.RequestId,
                    UpdatedAt = r.UpdatedAt,
                    WorkflowStateId = r.WorkflowStateId,
                    WorkflowStepId = r.WorkflowStepId,
                    AppUser = new AppUser
                    {
                        Id = r.AppUser.Id,
                        Username = r.AppUser.Username,
                        DisplayName = r.AppUser.DisplayName,
                        RowId = r.AppUser.RowId
                    },
                    WorkflowStep = new WorkflowStep
                    {
                        Id = r.WorkflowStep.Id,
                        Code = r.WorkflowStep.Code,
                        Name = r.WorkflowStep.Name,
                        RoleId = r.WorkflowStep.RoleId,
                        Role = new Role
                        {
                            Name = r.WorkflowStep.Role.Name,
                        },
                    },
                    WorkflowState = new WorkflowState
                    {
                        Id = r.WorkflowState.Id,
                        Code = r.WorkflowState.Code,
                        Name = r.WorkflowState.Name,
                    }
                }).ToListAsync();
            return RequestWorkflowStepMappings;
        }

        public async Task<bool> BulkMerge(Guid RequestId, List<RequestWorkflowStepMapping> RequestWorkflowStepMappings)
        {
            List<RequestWorkflowStepMappingDAO> RequestWorkflowStepMappingDAOs = DataContext.RequestWorkflowStepMapping
                .Where(r => r.RequestId == RequestId).ToList();
            RequestWorkflowStepMappingDAOs.ForEach(r => r.DeletedAt = StaticParams.DateTimeNow);

            foreach (RequestWorkflowStepMapping RequestWorkflowStepMapping in RequestWorkflowStepMappings)
            {
                RequestWorkflowStepMappingDAO RequestWorkflowStepMappingDAO = RequestWorkflowStepMappingDAOs
                    .Where(r => r.WorkflowStepId == RequestWorkflowStepMapping.WorkflowStepId).FirstOrDefault();
                if (RequestWorkflowStepMappingDAO == null)
                {
                    RequestWorkflowStepMappingDAO = new RequestWorkflowStepMappingDAO();
                    RequestWorkflowStepMappingDAO.RequestId = RequestId;
                    RequestWorkflowStepMappingDAO.WorkflowStepId = RequestWorkflowStepMapping.WorkflowStepId;
                    RequestWorkflowStepMappingDAO.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
                    RequestWorkflowStepMappingDAO.AppUserId = RequestWorkflowStepMapping.AppUserId;
                    RequestWorkflowStepMappingDAO.CreatedAt = StaticParams.DateTimeNow;
                    RequestWorkflowStepMappingDAO.UpdatedAt = StaticParams.DateTimeNow;
                    RequestWorkflowStepMappingDAO.DeletedAt = null;
                    RequestWorkflowStepMappingDAOs.Add(RequestWorkflowStepMappingDAO);
                }
                else
                {
                    RequestWorkflowStepMappingDAO.DeletedAt = null;
                    if (RequestWorkflowStepMappingDAO.WorkflowStateId != RequestWorkflowStepMapping.WorkflowStateId)
                    {
                        RequestWorkflowStepMappingDAO.WorkflowStateId = RequestWorkflowStepMapping.WorkflowStateId;
                        RequestWorkflowStepMappingDAO.AppUserId = RequestWorkflowStepMapping.AppUserId;
                        RequestWorkflowStepMappingDAO.UpdatedAt = StaticParams.DateTimeNow;
                    }
                }
            }
            await DataContext.BulkMergeAsync(RequestWorkflowStepMappingDAOs);
            await DataContext.RequestWorkflowStepMapping.Where(x => x.DeletedAt.HasValue).DeleteFromQueryAsync();

            return true;
        }
    }
}
