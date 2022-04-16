using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;
using DMS.Services.MDirectSalesOrder;
using DMS.Services.MIndirectSalesOrder;

namespace DMS.Services.MWorkflow
{
    public interface IWorkflowService : IServiceScoped
    {
        Task<RequestState> GetRequestState(Guid RequestId);
        RequestState GetRequestState(long RequestStateId);
        WorkflowState GetWorkflowState(long WorkflowStateId);
        Task<List<RequestWorkflowStepMapping>> ListRequestWorkflowStepMapping(Guid RequestId);
        Task<GenericEnum> Send(Guid RequestId, long WorkflowTypeId, long OrganiaztionId, Dictionary<string, string> Parameters);
        Task<GenericEnum> Approve(Guid RequestId, long WorkflowTypeId, Dictionary<string, string> Parameters);
        Task<GenericEnum> Reject(Guid RequestId, long WorkflowTypeId, Dictionary<string, string> Parameters);
    }
    public class WorkflowService : IWorkflowService
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private IDirectSalesOrderTemplate DirectSalesOrderTemplate;
        private IIndirectSalesOrderTemplate IndirectSalesOrderTemplate;
        private IRabbitManager RabbitManager;
        public WorkflowService(IUOW UOW,
            ICurrentContext CurrentContext,
            IDirectSalesOrderTemplate DirectSalesOrderTemplate,
            IIndirectSalesOrderTemplate IndirectSalesOrderTemplate,
            IRabbitManager RabbitManager
            )
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.DirectSalesOrderTemplate = DirectSalesOrderTemplate;
            this.IndirectSalesOrderTemplate = IndirectSalesOrderTemplate;
            this.RabbitManager = RabbitManager;
        }

        public async Task<RequestState> GetRequestState(Guid RequestId)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            if (RequestWorkflowDefinitionMapping == null)
                return null;
            return RequestWorkflowDefinitionMapping.RequestState;
        }

        public async Task<List<RequestWorkflowStepMapping>> ListRequestWorkflowStepMapping(Guid RequestId)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            List<RequestWorkflowHistory> RequestWorkflowHistories = await UOW.RequestWorkflowHistoryRepository.List(RequestId);
            List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = RequestWorkflowHistories.Select(x => new RequestWorkflowStepMapping
            {
                AppUserId = x.AppUserId,
                CreatedAt = x.CreatedAt,
                RequestId = x.RequestId,
                UpdatedAt = x.UpdatedAt,
                WorkflowStateId = x.WorkflowStateId,
                WorkflowStepId = x.WorkflowStepId,
                AppUser = x.AppUser == null ? null : new AppUser
                {
                    Id = x.AppUser.Id,
                    Username = x.AppUser.Username,
                    DisplayName = x.AppUser.DisplayName,
                },
                WorkflowState = x.WorkflowState == null ? null : new WorkflowState
                {
                    Id = x.WorkflowState.Id,
                    Code = x.WorkflowState.Code,
                    Name = x.WorkflowState.Name,
                },
                WorkflowStep = x.WorkflowStep == null ? null : new WorkflowStep
                {
                    Id = x.WorkflowStep.Id,
                    Code = x.WorkflowStep.Code,
                    Name = x.WorkflowStep.Name,
                    RoleId = x.WorkflowStep.RoleId,
                    Role = x.WorkflowStep.Role == null ? null : new Role
                    {
                        Id = x.WorkflowStep.Role.Id,
                        Code = x.WorkflowStep.Role.Code,
                        Name = x.WorkflowStep.Role.Name,
                    }
                },
            }).ToList();

            var RequestWorkflowStepApproved = RequestWorkflowStepMappings.Where(rq => rq.WorkflowState.Id == 3).OrderByDescending(x => x.CreatedAt).ToList(); // approved
            var RequestWorkflowStepReject = RequestWorkflowStepMappings.Where(rq => rq.WorkflowState.Id == 4).OrderByDescending(x => x.CreatedAt).ToList(); // rejected
            var RequestWorkflowStepPending = RequestWorkflowStepMappings.Where(rq => rq.WorkflowState.Id == 2).OrderByDescending(x => x.CreatedAt).ToList(); // pending

            if (RequestWorkflowStepApproved.Count > 0)
            {
                if (RequestWorkflowStepReject.Count() > 0)
                    RequestWorkflowStepMappings = RequestWorkflowStepApproved.Concat(RequestWorkflowStepReject).ToList();

                if (RequestWorkflowStepReject.Count() == 0)
                    RequestWorkflowStepMappings = RequestWorkflowStepApproved.Concat(RequestWorkflowStepPending).ToList();
            }

            if (RequestWorkflowStepApproved.Count == 0) RequestWorkflowStepMappings = RequestWorkflowStepPending; // if approved list is empty, return all pending


            foreach (RequestWorkflowStepMapping RequestWorkflowStepMapping in RequestWorkflowStepMappings)
            {
                if (RequestWorkflowStepMapping.WorkflowStateId == WorkflowStateEnum.PENDING.Id)
                {
                    List<AppUser> NextApprovers = await UOW.AppUserRepository.List(new AppUserFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName,
                        RoleId = new IdFilter { Equal = RequestWorkflowStepMapping.WorkflowStep.RoleId },
                        StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                    });
                    RequestWorkflowStepMapping.NextApprovers = NextApprovers;
                }
            }
            return RequestWorkflowStepMappings;
        }

        /// <summary>
        /// Trả về trạng thái chung của request
        /// </summary>
        /// <param name="RequestId"></param>
        /// <returns>Request State Enum</returns>
        private async Task<GenericEnum> IsInitializedStep(Guid RequestId)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            if (RequestWorkflowDefinitionMapping == null)
                return RequestStateEnum.APPROVED;
            else
            {
                if (RequestWorkflowDefinitionMapping.RequestStateId == RequestStateEnum.REJECTED.Id)
                    return RequestStateEnum.REJECTED;
                return RequestStateEnum.PENDING;
            }
        }
        private async Task<bool> IsStartedStep(Guid RequestId)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            if (RequestWorkflowDefinitionMapping == null)
                return false;
            if (RequestWorkflowDefinitionMapping.RequestStateId == RequestStateEnum.PENDING.Id)
                return true;
            if (RequestWorkflowDefinitionMapping.RequestStateId == RequestStateEnum.NEW.Id || RequestWorkflowDefinitionMapping.RequestStateId == RequestStateEnum.REJECTED.Id)
                return false;
            return false;
        }

        /// <summary>
        /// Trả về trạng thái sau khi khởi tạo
        /// </summary>
        /// <param name="RequestId">Guid đại diện cho 1 request</param>
        /// <param name="WorkflowTypeId"></param>
        /// <param name="OrganizationId"></param>
        /// <param name="Parameters"></param>
        /// <returns>Request State Enum</returns>
        public async Task<GenericEnum> Initialize(Guid RequestId, long WorkflowTypeId, long OrganizationId, Dictionary<string, string> Parameters)
        {
            // Tìm kiếm workflow definition đang active mà phù hợp với request hiện tại
            // Mỗi thời điểm chỉ có 1 workflow definition đang active cho 1 đối tượng
            Organization Organization = await UOW.OrganizationRepository.Get(OrganizationId);

            List<WorkflowDefinition> WorkflowDefinitions = await UOW.WorkflowDefinitionRepository.List(new WorkflowDefinitionFilter
            {
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                StartDate = new DateFilter { LessEqual = StaticParams.DateTimeNow },
                WorkflowTypeId = new IdFilter { Equal = WorkflowTypeId },
                Selects = WorkflowDefinitionSelect.Id | WorkflowDefinitionSelect.Organization,
                Skip = 0,
                Take = int.MaxValue,
            });

            WorkflowDefinition WorkflowDefinition = null;
            WorkflowDefinitions = WorkflowDefinitions
                .Where(x => Organization.Path.StartsWith(x.Organization.Path))
                .OrderByDescending(x => x.Organization.Path).ToList();
            foreach (var wd in WorkflowDefinitions)
            {
                if (Organization.Path.StartsWith(wd.Organization.Path))
                {
                    WorkflowDefinition = wd;
                    break;
                }
            }

            if (WorkflowDefinition == null)
                return RequestStateEnum.APPROVED;
            NotifyUsed(WorkflowDefinition);
            WorkflowDefinition = await UOW.WorkflowDefinitionRepository.Get(WorkflowDefinition.Id);

            // Kiểm tra trong workflow definition chọn được có workflow step nào thoả mãn việc step NGUỒN là 1 trong các role của user đang đăng nhập không
            // Nếu có step NGUỒN thì workflow có thể được khởi tạo
            if (WorkflowDefinition.WorkflowSteps != null)
            {
                bool ShouldInit = false;
                foreach (WorkflowStep WorkflowStep in WorkflowDefinition.WorkflowSteps)
                {
                    if (WorkflowDefinition.WorkflowDirections.Any(d => d.FromStepId == WorkflowStep.Id) &&
                        !WorkflowDefinition.WorkflowDirections.Any(d => d.ToStepId == WorkflowStep.Id))
                    {
                        if (CurrentContext.RoleIds.Contains(WorkflowStep.RoleId))
                            ShouldInit = true;
                    }
                }
                if (ShouldInit == false)
                    return RequestStateEnum.APPROVED;
            }

            // Xoá tất cả thông tin cũ của workflow
            await UOW.RequestWorkflowDefinitionMappingRepository.Delete(RequestId);
            // khởi tạo workflow
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            if (RequestWorkflowDefinitionMapping == null)
            {
                RequestWorkflowDefinitionMapping = new RequestWorkflowDefinitionMapping
                {
                    RequestId = RequestId,
                    WorkflowDefinitionId = WorkflowDefinition.Id,
                    RequestStateId = RequestStateEnum.NEW.Id,
                    CreatorId = CurrentContext.UserId,
                    Counter = 1,
                };
            }
            else
            {
                RequestWorkflowDefinitionMapping.RequestStateId = RequestStateEnum.NEW.Id;
                RequestWorkflowDefinitionMapping.Counter += 1;
            }
            await UOW.RequestWorkflowDefinitionMappingRepository.Update(RequestWorkflowDefinitionMapping);

            // khởi tạo trạng thái cho tất cả các nút với trạng thái NEW
            List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = new List<RequestWorkflowStepMapping>();
            foreach (WorkflowStep WorkflowStep in WorkflowDefinition.WorkflowSteps)
            {
                RequestWorkflowStepMapping RequestWorkflowStepMapping = new RequestWorkflowStepMapping();
                RequestWorkflowStepMappings.Add(RequestWorkflowStepMapping);
                RequestWorkflowStepMapping.RequestId = RequestId;
                RequestWorkflowStepMapping.WorkflowStepId = WorkflowStep.Id;
                RequestWorkflowStepMapping.WorkflowStateId = WorkflowStateEnum.NEW.Id;
                RequestWorkflowStepMapping.CreatedAt = StaticParams.DateTimeNow;
                RequestWorkflowStepMapping.UpdatedAt = StaticParams.DateTimeNow;
                RequestWorkflowStepMapping.AppUserId = null;
            }
            await UOW.RequestWorkflowStepMappingRepository.BulkMerge(RequestId, RequestWorkflowStepMappings);

            // Update các parameter chính từ request vào workflow để chạy điều kiện theo giá trị của request
            await UpdateParameters(RequestId, WorkflowDefinition, Parameters);
            return RequestStateEnum.PENDING;
        }

        private async Task<bool> StartStep(Guid RequestId, Dictionary<string, string> Parameters)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            WorkflowDefinition WorkflowDefinition = await UOW.WorkflowDefinitionRepository.Get(RequestWorkflowDefinitionMapping.WorkflowDefinitionId);

            if (WorkflowDefinition == null || RequestWorkflowDefinitionMapping.RequestStateId == RequestStateEnum.PENDING.Id)
            {
                return false;
            }
            else
            {
                // khởi tạo workflow
                RequestWorkflowDefinitionMapping = new RequestWorkflowDefinitionMapping
                {
                    RequestId = RequestId,
                    WorkflowDefinitionId = WorkflowDefinition.Id,
                    RequestStateId = RequestStateEnum.PENDING.Id,
                    CreatorId = CurrentContext.UserId,
                };
                await UOW.RequestWorkflowDefinitionMappingRepository.Update(RequestWorkflowDefinitionMapping);
                List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = await UOW.RequestWorkflowStepMappingRepository.List(RequestId);
                // tìm điểm bắt đầu của workflow
                // nút không có ai trỏ đến là nút bắt đầu
                // trong trường hợp có nhiều nút bắt đầu thì xét có nút nào thuộc các role hiện tại không
                foreach (WorkflowStep WorkflowStep in WorkflowDefinition.WorkflowSteps)
                {
                    if (CurrentContext.RoleIds.Contains(WorkflowStep.RoleId) &&
                        !WorkflowDefinition.WorkflowDirections.Any(d => d.ToStepId == WorkflowStep.Id))
                    {
                        RequestWorkflowStepMapping RequestWorkflowStepMapping = RequestWorkflowStepMappings.Where(r => r.WorkflowStepId == WorkflowStep.Id).FirstOrDefault();
                        RequestWorkflowStepMapping.WorkflowStateId = WorkflowStateEnum.PENDING.Id;
                    }
                }
                await UOW.RequestWorkflowStepMappingRepository.BulkMerge(RequestId, RequestWorkflowStepMappings);

                await UpdateParameters(RequestId, WorkflowDefinition, Parameters);
            }
            return true;
        }

        private async Task<bool> ApproveStep(Guid RequestId, Dictionary<string, string> Parameters)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            WorkflowDefinition WorkflowDefinition = await UOW.WorkflowDefinitionRepository.Get(RequestWorkflowDefinitionMapping.WorkflowDefinitionId);
            {
                List<WorkflowDirection> WorkflowDirections = new List<WorkflowDirection>();
                foreach (WorkflowDirection WorkflowDirection in WorkflowDefinition.WorkflowDirections)
                {
                    bool result = await ApplyCondition(RequestId, WorkflowDirection);
                    if (result)
                    {
                        WorkflowDirections.Add(WorkflowDirection);
                    }
                }
                WorkflowDefinition.WorkflowDirections = WorkflowDirections;
            }
            // tìm điểm bắt đầu
            // tìm điểm nhảy tiếp theo
            // chuyển trạng thái điểm nhảy
            // gửi mail cho các điểm nhảy có trạng thái thay đổi.
            List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = await UOW.RequestWorkflowStepMappingRepository.List(RequestId);
            if (WorkflowDefinition != null && RequestWorkflowStepMappings.Count > 0)
            {
                // Tìm điểm đích
                List<WorkflowStep> ToSteps = new List<WorkflowStep>();
                foreach (WorkflowStep WorkflowStep in WorkflowDefinition.WorkflowSteps)
                {
                    if (CurrentContext.RoleIds.Contains(WorkflowStep.RoleId))
                    {
                        var StartRequestStep = RequestWorkflowStepMappings
                            .Where(x => x.WorkflowStateId == WorkflowStateEnum.PENDING.Id)
                            .Where(x => x.WorkflowStepId == WorkflowStep.Id).FirstOrDefault();
                        if (StartRequestStep != null)
                        {
                            StartRequestStep.WorkflowStateId = WorkflowStateEnum.APPROVED.Id;
                            StartRequestStep.UpdatedAt = StaticParams.DateTimeNow;
                            StartRequestStep.AppUserId = CurrentContext.UserId;
                            foreach (WorkflowDirection WorkflowDirection in WorkflowDefinition.WorkflowDirections)
                            {
                                if (WorkflowDirection.FromStepId == WorkflowStep.Id)
                                {
                                    bool result = await ApplyCondition(RequestId, WorkflowDirection);
                                    if (result)
                                    {
                                        ToSteps.Add(WorkflowDirection.ToStep);
                                    }
                                }
                            }
                        }
                    }
                }

                ToSteps = ToSteps.Distinct().ToList();
                List<Mail> Mails = new List<Mail>();
                var NextStepIds = new List<long>();
                var PreviousStepIds = new List<long>();
                // tìm điểm bắt đầu cho những điểm đích tìm được
                foreach (WorkflowStep WorkflowStep in ToSteps)
                {
                    // Xác định các direction đang trỏ đến đích
                    var ActiveDirections = WorkflowDefinition.WorkflowDirections.Where(d => d.ToStepId == WorkflowStep.Id).ToList();
                    var ApprovedFromRequestSteps = new List<RequestWorkflowStepMapping>();
                    foreach (var Direction in ActiveDirections)
                    {
                        // Với các direction 
                        bool result = await ApplyCondition(RequestId, Direction);
                        if (result)
                        {
                            var FromRequestStep = RequestWorkflowStepMappings
                            .Where(x =>
                                x.WorkflowStepId == Direction.FromStepId)
                            .FirstOrDefault();
                            ApprovedFromRequestSteps.Add(FromRequestStep);
                            PreviousStepIds.Add(Direction.FromStepId);
                        }
                    }

                    // Nếu tất cả các điểm bắt đầu của điểm đích đang xét đều là APPROVED thì điểm đích sẽ là PENDING
                    if (ApprovedFromRequestSteps.All(x => x.WorkflowStateId == WorkflowStateEnum.APPROVED.Id))
                    {
                        var RequestWorkflowStepMapping = RequestWorkflowStepMappings.Where(x => x.WorkflowStepId == WorkflowStep.Id).FirstOrDefault();
                        RequestWorkflowStepMapping.WorkflowStateId = WorkflowStateEnum.PENDING.Id;
                        RequestWorkflowStepMapping.UpdatedAt = null;
                        RequestWorkflowStepMapping.AppUserId = null;
                        NextStepIds.Add(WorkflowStep.Id);
                    }
                }
                await UOW.RequestWorkflowStepMappingRepository.BulkMerge(RequestId, RequestWorkflowStepMappings);

                // Gửi mail cho những direction mà điểm nguồn và đích được tìm ra bên trên
                List<WorkflowDirection> WorkflowDirections = WorkflowDefinition.WorkflowDirections
                    .Where(x => PreviousStepIds.Contains(x.FromStepId) && NextStepIds.Contains(x.ToStepId)).ToList();
                foreach (var WorkflowDirection in WorkflowDirections)
                {
                    //gửi mail
                    AppUserFilter AppUserFilter = new AppUserFilter
                    {
                        RoleId = new IdFilter { Equal = WorkflowDirection.ToStep.RoleId },
                        Skip = 0,
                        Take = int.MaxValue,
                        Selects = AppUserSelect.Email | AppUserSelect.RowId
                    };
                    List<AppUser> appUsers = await UOW.AppUserRepository.List(AppUserFilter);
                    List<string> recipients = appUsers.Select(au => au.Email).Distinct().ToList();
                    //TODO gửi thông báo cho to step
                    if (Parameters.TryGetValue("Type", out string value1) && value1 == nameof(DirectSalesOrder))
                    {
                        List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                        var Sender = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                        DirectSalesOrder DirectSalesOrder = new DirectSalesOrder();
                        if (Parameters.TryGetValue(nameof(DirectSalesOrder.Code), out string Code))
                        {
                            DirectSalesOrder.Code = Code;
                            DirectSalesOrder.Id = Int64.Parse(Code);
                            foreach (var Recipient in appUsers)
                            {
                                GlobalUserNotification GlobalUserNotification = 
                                    DirectSalesOrderTemplate.CreateAppUserNotification(Sender.RowId, Recipient.RowId, DirectSalesOrder, Sender, NotificationType.SEND);
                                GlobalUserNotifications.Add(GlobalUserNotification);
                            }
                            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                        }
                    }
                    else if (Parameters.TryGetValue("Type", out string value2) && value2 == nameof(IndirectSalesOrder))
                    {
                        List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                        var Sender = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                        IndirectSalesOrder IndirectSalesOrder = new IndirectSalesOrder();
                        if (Parameters.TryGetValue(nameof(IndirectSalesOrder.Code), out string Code))
                        {
                            IndirectSalesOrder.Code = Code;
                            IndirectSalesOrder.Id = Int64.Parse(Code);
                            foreach (var Recipient in appUsers)
                            {
                                GlobalUserNotification GlobalUserNotification = 
                                    IndirectSalesOrderTemplate.CreateAppUserNotification(Sender.RowId, Recipient.RowId, IndirectSalesOrder, Sender, NotificationType.SEND);
                                GlobalUserNotifications.Add(GlobalUserNotification);
                            }
                            RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                        }
                    }
                    Mail MailForCreator = new Mail
                    {
                        Recipients = recipients,
                        Subject = CreateMailContent(WorkflowDirection.SubjectMailForCreator, Parameters),
                        Body = CreateMailContent(WorkflowDirection.BodyMailForCreator, Parameters),
                        RowId = Guid.NewGuid(),
                    };


                    Mail MailForCurrentStep = new Mail
                    {
                        Recipients = recipients,
                        Subject = CreateMailContent(WorkflowDirection.SubjectMailForCurrentStep, Parameters),
                        Body = CreateMailContent(WorkflowDirection.BodyMailForCurrentStep, Parameters),
                        RowId = Guid.NewGuid(),
                    };

                    Mail MailForNextStep = new Mail
                    {
                        Recipients = recipients,
                        Subject = CreateMailContent(WorkflowDirection.SubjectMailForNextStep, Parameters),
                        Body = CreateMailContent(WorkflowDirection.BodyMailForNextStep, Parameters),
                        RowId = Guid.NewGuid(),
                    };
                    Mails.Add(MailForCreator);
                    Mails.Add(MailForCurrentStep);
                    Mails.Add(MailForNextStep);
                }
                Mails = Mails.Distinct().ToList();
                RabbitManager.PublishList(Mails, RoutingKeyEnum.MailSend.Code);
                return true;
            }

            return false;
        }

        private async Task<bool> RejectStep(Guid RequestId, Dictionary<string, string> Parameters)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            WorkflowDefinition WorkflowDefinition = await UOW.WorkflowDefinitionRepository.Get(RequestWorkflowDefinitionMapping.WorkflowDefinitionId);
            List<RequestWorkflowStepMapping> RequestWorkflowStepMappings = await UOW.RequestWorkflowStepMappingRepository.List(RequestId);
            if (WorkflowDefinition != null && RequestWorkflowStepMappings.Count > 0)
            {
                List<WorkflowStep> ToSteps = new List<WorkflowStep>();
                List<Mail> Mails = new List<Mail>();
                foreach (WorkflowStep WorkflowStep in WorkflowDefinition.WorkflowSteps)
                {
                    if (CurrentContext.RoleIds.Contains(WorkflowStep.RoleId))
                    {
                        var StartRequestStep = RequestWorkflowStepMappings
                            .Where(x => x.WorkflowStateId == WorkflowStateEnum.PENDING.Id)
                            .Where(x => x.WorkflowStepId == WorkflowStep.Id).FirstOrDefault();
                        if (StartRequestStep == null)
                            continue;
                        StartRequestStep.WorkflowStateId = WorkflowStateEnum.REJECTED.Id;
                        StartRequestStep.UpdatedAt = StaticParams.DateTimeNow;
                        StartRequestStep.AppUserId = CurrentContext.UserId;
                        await UOW.RequestWorkflowStepMappingRepository.BulkMerge(RequestId, RequestWorkflowStepMappings);
                        AppUserFilter AppUserFilter = new AppUserFilter
                        {
                            Id = new IdFilter { Equal = RequestWorkflowDefinitionMapping.CreatorId },
                            Skip = 0,
                            Take = int.MaxValue,
                            Selects = AppUserSelect.Email
                        };
                        List<AppUser> appUsers = await UOW.AppUserRepository.List(AppUserFilter);
                        List<string> recipients = appUsers.Select(au => au.Email).Distinct().ToList();
                        Mail MailForReject = new Mail
                        {
                            Recipients = recipients,
                            Subject = CreateMailContent(WorkflowStep.SubjectMailForReject, Parameters),
                            Body = CreateMailContent(WorkflowStep.BodyMailForReject, Parameters),
                            RowId = Guid.NewGuid(),
                        };
                        Mails.Add(MailForReject);
                        return true;
                    }
                }
                RabbitManager.PublishList(Mails, RoutingKeyEnum.MailSend.Code);
            }
            return false;
        }

        private async Task<GenericEnum> EndStep(Guid RequestId, Dictionary<string, string> Parameters)
        {
            RequestWorkflowDefinitionMapping RequestWorkflowDefinitionMapping = await UOW.RequestWorkflowDefinitionMappingRepository.Get(RequestId);
            if (RequestWorkflowDefinitionMapping == null)
                return null;
            List<RequestWorkflowStepMapping> RequestWorkflowStepMapping = await UOW.RequestWorkflowStepMappingRepository.List(RequestId);

            GenericEnum RequestState = RequestStateEnum.PENDING;
            if (RequestWorkflowStepMapping != null)
            {
                if (RequestWorkflowStepMapping.Any(x => x.WorkflowStateId == WorkflowStateEnum.REJECTED.Id))
                {
                    RequestState = RequestStateEnum.REJECTED;
                }
                else if (RequestWorkflowStepMapping
                    .Where(x => x.WorkflowStateId != WorkflowStateEnum.NEW.Id)
                    .All(x => x.WorkflowStateId == WorkflowStateEnum.APPROVED.Id))
                {
                    RequestState = RequestStateEnum.APPROVED;
                }
            }

            RequestWorkflowDefinitionMapping = new RequestWorkflowDefinitionMapping
            {
                RequestId = RequestId,
                WorkflowDefinitionId = RequestWorkflowDefinitionMapping.WorkflowDefinitionId,
                RequestStateId = RequestState.Id,
            };
            await UOW.RequestWorkflowDefinitionMappingRepository.Update(RequestWorkflowDefinitionMapping);

            return RequestState;
        }

        private string CreateMailContent(string templateContent, Dictionary<string, string> Parameters)
        {
            if (string.IsNullOrWhiteSpace(templateContent))
                return "";
            var template = Handlebars.Compile(templateContent);
            var result = template(Parameters);
            return result;
        }

        private async Task UpdateParameters(Guid RequestId, WorkflowDefinition WorkflowDefinition, Dictionary<string, string> Parameters)
        {
            // khởi tạo các parameter cho workflow
            List<RequestWorkflowParameterMapping> RequestWorkflowParameterMappings = new List<RequestWorkflowParameterMapping>();
            List<WorkflowParameter> WorkflowParameters = await UOW.WorkflowParameterRepository.List(new WorkflowParameterFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                WorkflowTypeId = new IdFilter { Equal = WorkflowDefinition.WorkflowTypeId },
                Selects = WorkflowParameterSelect.ALL,
            });
            foreach (WorkflowParameter WorkflowParameter in WorkflowParameters)
            {
                RequestWorkflowParameterMapping RequestWorkflowParameterMapping = new RequestWorkflowParameterMapping();
                RequestWorkflowParameterMappings.Add(RequestWorkflowParameterMapping);
                RequestWorkflowParameterMapping.RequestId = RequestId;
                RequestWorkflowParameterMapping.WorkflowParameterId = WorkflowParameter.Id;
                RequestWorkflowParameterMapping.Value = null;
                foreach (var pair in Parameters)
                {
                    if (WorkflowParameter.Code == pair.Key)
                    {
                        RequestWorkflowParameterMapping.Value = pair.Value;
                    }
                }
            }
            await UOW.RequestWorkflowParameterMappingRepository.BulkMerge(RequestId, RequestWorkflowParameterMappings);
        }
        private async Task<bool> ApplyCondition(Guid RequestId, WorkflowDirection WorkflowDirection)
        {
            List<RequestWorkflowParameterMapping> RequestWorkflowParameterMappings = await UOW.RequestWorkflowParameterMappingRepository.List(RequestId);
            bool result = true;
            foreach (RequestWorkflowParameterMapping RequestWorkflowParameterMapping in RequestWorkflowParameterMappings)
            {
                List<WorkflowDirectionCondition> WorkflowDirectionConditions = WorkflowDirection.WorkflowDirectionConditions
                    .Where(x => x.WorkflowParameterId == RequestWorkflowParameterMapping.WorkflowParameterId).ToList();
                if (WorkflowDirectionConditions.Count == 0)
                    continue;

                if (RequestWorkflowParameterMapping.WorkflowParameterTypeId == WorkflowParameterTypeEnum.ID.Id && RequestWorkflowParameterMapping.IdValue.HasValue)
                {
                    List<long> In = new List<long>();
                    List<long> NotIn = new List<long>();
                    List<WorkflowDirectionCondition> EqualConditions = WorkflowDirectionConditions.Where(x => x.WorkflowOperatorId == WorkflowOperatorEnum.ID_EQ.Id).ToList();
                    if (EqualConditions.Count > 0)
                    {
                        if (EqualConditions.Count == 1)
                        {
                            long value = EqualConditions.Select(x => long.Parse(x.Value)).FirstOrDefault();
                            In.Add(value);
                        }
                        else
                        {
                            result = result && false;
                        }
                    }
                    else
                    {
                        List<WorkflowDirectionCondition> InConditions = WorkflowDirectionConditions.Where(x => x.WorkflowOperatorId == WorkflowOperatorEnum.ID_IN.Id).ToList();
                        List<long> value = InConditions.Select(x => long.Parse(x.Value)).ToList();
                        In.AddRange(value);
                    }

                    List<WorkflowDirectionCondition> NotEqualConditions = WorkflowDirectionConditions.Where(x => x.WorkflowOperatorId == WorkflowOperatorEnum.ID_NE.Id).ToList();
                    if (NotEqualConditions.Count > 0)
                    {
                        if (NotEqualConditions.Count == 1)
                        {
                            long value = NotEqualConditions.Select(x => long.Parse(x.Value)).FirstOrDefault();
                            NotIn.Add(value);
                        }
                        else
                        {
                            result = result && false;
                        }
                    }
                    else
                    {
                        List<WorkflowDirectionCondition> NotInConditions = WorkflowDirectionConditions.Where(x => x.WorkflowOperatorId == WorkflowOperatorEnum.ID_NI.Id).ToList();
                        List<long> value = NotInConditions.Select(x => long.Parse(x.Value)).ToList();
                        NotIn.AddRange(value);
                    }

                    if (In.Count > 0 && !In.Any(x => x == RequestWorkflowParameterMapping.IdValue.Value))
                        result = result && false;

                    if (NotIn.Count > 0 && NotIn.Any(x => x == RequestWorkflowParameterMapping.IdValue.Value))
                        result = result && false;
                }

                if (RequestWorkflowParameterMapping.WorkflowParameterTypeId == WorkflowParameterTypeEnum.LONG.Id && RequestWorkflowParameterMapping.LongValue.HasValue)
                {
                    foreach (WorkflowDirectionCondition WorkflowDirectionCondition in WorkflowDirectionConditions)
                    {
                        if (WorkflowDirectionCondition.Value != null)
                        {
                            long value = long.Parse(WorkflowDirectionCondition.Value);
                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.LONG_EQ.Id &&
                                !(RequestWorkflowParameterMapping.LongValue.Value == value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.LONG_GE.Id &&
                                !(RequestWorkflowParameterMapping.LongValue.Value >= value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.LONG_GT.Id &&
                               !(RequestWorkflowParameterMapping.LongValue.Value > value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.LONG_LE.Id &&
                               !(RequestWorkflowParameterMapping.LongValue.Value <= value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.LONG_LT.Id &&
                               !(RequestWorkflowParameterMapping.LongValue.Value < value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.LONG_NE.Id &&
                               !(RequestWorkflowParameterMapping.LongValue.Value != value))
                                result = result && false;
                        }
                    }
                }

                if (RequestWorkflowParameterMapping.WorkflowParameterTypeId == WorkflowParameterTypeEnum.DECIMAL.Id && RequestWorkflowParameterMapping.DecimalValue.HasValue)
                {
                    foreach (WorkflowDirectionCondition WorkflowDirectionCondition in WorkflowDirectionConditions)
                    {
                        if (WorkflowDirectionCondition.Value != null)
                        {
                            decimal value = decimal.Parse(WorkflowDirectionCondition.Value);

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DECIMAL_EQ.Id &&
                                !(RequestWorkflowParameterMapping.DecimalValue.Value == value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DECIMAL_GE.Id &&
                                !(RequestWorkflowParameterMapping.DecimalValue.Value >= value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DECIMAL_GT.Id &&
                               !(RequestWorkflowParameterMapping.DecimalValue.Value > value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DECIMAL_LE.Id &&
                               !(RequestWorkflowParameterMapping.DecimalValue.Value <= value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DECIMAL_LT.Id &&
                               !(RequestWorkflowParameterMapping.DecimalValue.Value < value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DECIMAL_NE.Id &&
                               !(RequestWorkflowParameterMapping.DecimalValue.Value != value))
                                result = result && false;
                        }
                    }
                }

                if (RequestWorkflowParameterMapping.WorkflowParameterTypeId == WorkflowParameterTypeEnum.DATE.Id && RequestWorkflowParameterMapping.DateValue.HasValue)
                {
                    foreach (WorkflowDirectionCondition WorkflowDirectionCondition in WorkflowDirectionConditions)
                    {
                        if (WorkflowDirectionCondition.Value != null)
                        {
                            DateTime value = DateTime.Parse(WorkflowDirectionCondition.Value);
                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DATE_EQ.Id &&
                                !(RequestWorkflowParameterMapping.DateValue.Value == value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DATE_GE.Id &&
                                !(RequestWorkflowParameterMapping.DateValue.Value >= value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DATE_GT.Id &&
                               !(RequestWorkflowParameterMapping.DateValue.Value > value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DATE_LE.Id &&
                               !(RequestWorkflowParameterMapping.DateValue.Value <= value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DATE_LT.Id &&
                               !(RequestWorkflowParameterMapping.DateValue.Value < value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.DATE_NE.Id &&
                               !(RequestWorkflowParameterMapping.DateValue.Value != value))
                                result = result && false;
                        }
                    }
                }

                if (RequestWorkflowParameterMapping.WorkflowParameterTypeId == WorkflowParameterTypeEnum.STRING.Id && RequestWorkflowParameterMapping.StringValue != null)
                {
                    foreach (WorkflowDirectionCondition WorkflowDirectionCondition in WorkflowDirectionConditions)
                    {
                        if (WorkflowDirectionCondition.Value != null)
                        {
                            string value = WorkflowDirectionCondition.Value;
                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.STRING_CT.Id &&
                                !RequestWorkflowParameterMapping.StringValue.Contains(value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.STRING_EQ.Id &&
                                !RequestWorkflowParameterMapping.StringValue.Equals(value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.STRING_EW.Id &&
                               !RequestWorkflowParameterMapping.StringValue.EndsWith(value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.STRING_NC.Id &&
                               RequestWorkflowParameterMapping.StringValue.Contains(value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.STRING_NE.Id &&
                               RequestWorkflowParameterMapping.StringValue.Equals(value))
                                result = result && false;

                            if (WorkflowDirectionCondition.WorkflowOperatorId == WorkflowOperatorEnum.STRING_NEW.Id &&
                               RequestWorkflowParameterMapping.StringValue.EndsWith(value))
                                result = result && false;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="WorkflowTypeId"></param>
        /// <param name="OrganiaztionId"></param>
        /// <param name="Parameters"></param>
        /// <returns>Request State Enum</returns>
        public async Task<GenericEnum> Send(Guid RequestId, long WorkflowTypeId, long OrganiaztionId, Dictionary<string, string> Parameters)
        {
            GenericEnum RequestState = await Initialize(RequestId, WorkflowTypeId, OrganiaztionId, Parameters);
            if (RequestState == RequestStateEnum.APPROVED)
                return RequestStateEnum.APPROVED;
            bool IsStarted = await StartStep(RequestId, Parameters);
            if (IsStarted)
            {
                RequestState = await Approve(RequestId, WorkflowTypeId, Parameters);
                return RequestState;
            }
            return RequestStateEnum.APPROVED;
        }

        /// <summary>
        /// Thực hiện phê duyệt bản ghi
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="WorkflowTypeId"></param>
        /// <param name="Parameters"></param>
        /// <returns>Request State Enum</returns>
        public async Task<GenericEnum> Approve(Guid RequestId, long WorkflowTypeId, Dictionary<string, string> Parameters)
        {
            await ApproveStep(RequestId, Parameters);
            GenericEnum RequestState = await EndStep(RequestId, Parameters);
            return RequestState;
        }

        public async Task<GenericEnum> Reject(Guid RequestId, long WorkflowTypeId, Dictionary<string, string> Parameters)
        {
            await RejectStep(RequestId, Parameters);
            GenericEnum RequestState = await EndStep(RequestId, Parameters);
            return RequestState;
        }

        /// <summary>
        /// Khi workflow được sử dụng bởi 1 trong các loại request thì nó được đánh dấu là đã sử dụng
        /// </summary>
        /// <param name="WorkflowDefinition"></param>
        private void NotifyUsed(WorkflowDefinition WorkflowDefinition)
        {
            {
                RabbitManager.PublishSingle(WorkflowDefinition, RoutingKeyEnum.WorkflowDefinitionUsed.Code);
            }
        }

        public RequestState GetRequestState(long RequestStateId)
        {
            RequestState RequestState = RequestStateEnum.RequestStateEnumList
                .Where(x => x.Id == RequestStateId)
                .Select(x => new RequestState
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).FirstOrDefault();
            return RequestState;
        }

        public WorkflowState GetWorkflowState(long WorkflowStateId)
        {
            WorkflowState WorkflowState = WorkflowStateEnum.WorkflowStateEnumList
                .Where(x => x.Id == WorkflowStateId)
                .Select(x => new WorkflowState
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).FirstOrDefault();
            return WorkflowState;
        }
    }
}
