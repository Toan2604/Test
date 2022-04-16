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
    public interface IRequestWorkflowHistoryRepository
    {
        Task<List<RequestWorkflowHistory>> List(Guid RequestId);
    }
    public class RequestWorkflowHistoryRepository : IRequestWorkflowHistoryRepository
    {
        private DataContext DataContext;
        public RequestWorkflowHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<List<RequestWorkflowHistory>> List(Guid RequestId)
        {
            List<RequestWorkflowHistory> RequestWorkflowHistories = await DataContext.RequestWorkflowHistory.AsNoTracking()
                .Where(r => r.RequestId == RequestId)
                .OrderBy(x => x.CreatedAt).ThenBy(x => x.UpdatedAt)
                .Select(r => new RequestWorkflowHistory
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
                    },
                    WorkflowStep = new WorkflowStep
                    {
                        Id = r.WorkflowStep.Id,
                        Code = r.WorkflowStep.Code,
                        Name = r.WorkflowStep.Name,
                        RoleId = r.WorkflowStep.RoleId,
                        Role = new Role
                        {
                            Id = r.WorkflowStep.Role.Id,
                            Code = r.WorkflowStep.Role.Code,
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
            return RequestWorkflowHistories;
        }
    }
}
