using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Services.MAppUser;
using DMS.Services.MNotification;
using DMS.Services.MNotificationStatus;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.notification
{
    public class NotificationController : RpcController
    {
        private INotificationStatusService NotificationStatusService;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private IStatusService StatusService;
        private INotificationService NotificationService;
        private ICurrentContext CurrentContext;
        public NotificationController(
            INotificationStatusService NotificationStatusService,
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            IStatusService StatusService,
            INotificationService NotificationService,
            ICurrentContext CurrentContext
        )
        {
            this.NotificationStatusService = NotificationStatusService;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.StatusService = StatusService;
            this.NotificationService = NotificationService;
            this.CurrentContext = CurrentContext;
        }

        [Route(NotificationRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Notification_NotificationFilterDTO Notification_NotificationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            NotificationFilter NotificationFilter = ConvertFilterDTOToFilterEntity(Notification_NotificationFilterDTO);
            NotificationFilter = NotificationService.ToFilter(NotificationFilter);
            int count = await NotificationService.Count(NotificationFilter);
            return count;
        }

        [Route(NotificationRoute.List), HttpPost]
        public async Task<ActionResult<List<Notification_NotificationDTO>>> List([FromBody] Notification_NotificationFilterDTO Notification_NotificationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            NotificationFilter NotificationFilter = ConvertFilterDTOToFilterEntity(Notification_NotificationFilterDTO);
            NotificationFilter = NotificationService.ToFilter(NotificationFilter);
            List<Notification> Notifications = await NotificationService.List(NotificationFilter);
            List<Notification_NotificationDTO> Notification_NotificationDTOs = Notifications
                .Select(c => new Notification_NotificationDTO(c)).ToList();
            return Notification_NotificationDTOs;
        }

        [Route(NotificationRoute.Get), HttpPost]
        public async Task<ActionResult<Notification_NotificationDTO>> Get([FromBody] Notification_NotificationDTO Notification_NotificationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Notification_NotificationDTO.Id))
                return Forbid();

            Notification Notification = await NotificationService.Get(Notification_NotificationDTO.Id);
            return new Notification_NotificationDTO(Notification);
        }

        [Route(NotificationRoute.Create), HttpPost]
        public async Task<ActionResult<Notification_NotificationDTO>> Create([FromBody] Notification_NotificationDTO Notification_NotificationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Notification_NotificationDTO.Id))
                return Forbid();

            Notification Notification = ConvertDTOToEntity(Notification_NotificationDTO);
            Notification = await NotificationService.Create(Notification);
            Notification_NotificationDTO = new Notification_NotificationDTO(Notification);
            if (Notification.IsValidated)
                return Notification_NotificationDTO;
            else
                return BadRequest(Notification_NotificationDTO);
        }

        [Route(NotificationRoute.Update), HttpPost]
        public async Task<ActionResult<Notification_NotificationDTO>> Update([FromBody] Notification_NotificationDTO Notification_NotificationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Notification_NotificationDTO.Id))
                return Forbid();

            Notification Notification = ConvertDTOToEntity(Notification_NotificationDTO);
            Notification = await NotificationService.Update(Notification);
            Notification_NotificationDTO = new Notification_NotificationDTO(Notification);
            if (Notification.IsValidated)
                return Notification_NotificationDTO;
            else
                return BadRequest(Notification_NotificationDTO);
        }

        [Route(NotificationRoute.Send), HttpPost]
        public async Task<ActionResult<Notification_NotificationDTO>> Send([FromBody] Notification_NotificationDTO Notification_NotificationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Notification_NotificationDTO.Id))
                return Forbid();

            Notification Notification = ConvertDTOToEntity(Notification_NotificationDTO);
            var oldData = await NotificationService.Get(Notification.Id);
            if (oldData == null)
            {
                Notification = await NotificationService.Create(Notification);
            }
            Notification = await NotificationService.Send(Notification);
            Notification_NotificationDTO = new Notification_NotificationDTO(Notification);
            if (Notification.IsValidated)
                return Notification_NotificationDTO;
            else
                return BadRequest(Notification_NotificationDTO);
        }

        [Route(NotificationRoute.Delete), HttpPost]
        public async Task<ActionResult<Notification_NotificationDTO>> Delete([FromBody] Notification_NotificationDTO Notification_NotificationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Notification_NotificationDTO.Id))
                return Forbid();

            Notification Notification = ConvertDTOToEntity(Notification_NotificationDTO);
            Notification = await NotificationService.Delete(Notification);
            Notification_NotificationDTO = new Notification_NotificationDTO(Notification);
            if (Notification.IsValidated)
                return Notification_NotificationDTO;
            else
                return BadRequest(Notification_NotificationDTO);
        }

        [Route(NotificationRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            NotificationFilter NotificationFilter = new NotificationFilter();
            NotificationFilter = NotificationService.ToFilter(NotificationFilter);
            NotificationFilter.Id = new IdFilter { In = Ids };
            NotificationFilter.Selects = NotificationSelect.Id;
            NotificationFilter.Skip = 0;
            NotificationFilter.Take = int.MaxValue;

            List<Notification> Notifications = await NotificationService.List(NotificationFilter);
            Notifications = await NotificationService.BulkDelete(Notifications);
            if (Notifications.Any(x => !x.IsValidated))
                return BadRequest(Notifications.Where(x => !x.IsValidated));
            return true;
        }

        private async Task<bool> HasPermission(long Id)
        {
            NotificationFilter NotificationFilter = new NotificationFilter();
            NotificationFilter = NotificationService.ToFilter(NotificationFilter);
            if (Id == 0)
            {

            }
            else
            {
                NotificationFilter.Id = new IdFilter { Equal = Id };
                int count = await NotificationService.Count(NotificationFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Notification ConvertDTOToEntity(Notification_NotificationDTO Notification_NotificationDTO)
        {
            Notification_NotificationDTO.TrimString();
            Notification Notification = new Notification();
            Notification.Id = Notification_NotificationDTO.Id;
            Notification.Title = Notification_NotificationDTO.Title;
            Notification.Content = Notification_NotificationDTO.Content;
            Notification.OrganizationId = Notification_NotificationDTO.OrganizationId;
            Notification.NotificationStatusId = Notification_NotificationDTO.NotificationStatusId;
            Notification.NotificationStatus = Notification_NotificationDTO.NotificationStatus == null ? null : new NotificationStatus
            {
                Id = Notification_NotificationDTO.NotificationStatus.Id,
                Code = Notification_NotificationDTO.NotificationStatus.Code,
                Name = Notification_NotificationDTO.NotificationStatus.Name,
            };
            Notification.Organization = Notification_NotificationDTO.Organization == null ? null : new Organization
            {
                Id = Notification_NotificationDTO.Organization.Id,
                Code = Notification_NotificationDTO.Organization.Code,
                Name = Notification_NotificationDTO.Organization.Name,
                ParentId = Notification_NotificationDTO.Organization.ParentId,
                Path = Notification_NotificationDTO.Organization.Path,
                Level = Notification_NotificationDTO.Organization.Level,
                StatusId = Notification_NotificationDTO.Organization.StatusId,
                Phone = Notification_NotificationDTO.Organization.Phone,
                Email = Notification_NotificationDTO.Organization.Email,
                Address = Notification_NotificationDTO.Organization.Address,
            };
            Notification.BaseLanguage = CurrentContext.Language;
            return Notification;
        }

        private NotificationFilter ConvertFilterDTOToFilterEntity(Notification_NotificationFilterDTO Notification_NotificationFilterDTO)
        {
            NotificationFilter NotificationFilter = new NotificationFilter();
            NotificationFilter.Selects = NotificationSelect.ALL;
            NotificationFilter.Skip = Notification_NotificationFilterDTO.Skip;
            NotificationFilter.Take = Notification_NotificationFilterDTO.Take;
            NotificationFilter.OrderBy = Notification_NotificationFilterDTO.OrderBy;
            NotificationFilter.OrderType = Notification_NotificationFilterDTO.OrderType;

            NotificationFilter.Id = Notification_NotificationFilterDTO.Id;
            NotificationFilter.Title = Notification_NotificationFilterDTO.Title;
            NotificationFilter.Content = Notification_NotificationFilterDTO.Content;
            NotificationFilter.OrganizationId = Notification_NotificationFilterDTO.OrganizationId;
            NotificationFilter.NotificationStatusId = Notification_NotificationFilterDTO.NotificationStatusId;
            return NotificationFilter;
        }

        [Route(NotificationRoute.FilterListAppUser), HttpPost]
        public async Task<List<Notification_AppUserDTO>> FilterListAppUser([FromBody] Notification_AppUserFilterDTO Notification_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = Notification_AppUserFilterDTO.Id;
            AppUserFilter.Username = Notification_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Notification_AppUserFilterDTO.DisplayName;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Notification_AppUserDTO> StoreCheckerMonitor_AppUserDTOs = AppUsers
                .Select(x => new Notification_AppUserDTO(x)).ToList();
            return StoreCheckerMonitor_AppUserDTOs;
        }

        [Route(NotificationRoute.FilterListNotificationStatus), HttpPost]
        public async Task<List<Notification_NotificationStatusDTO>> FilterListNotificationStatus([FromBody] Notification_NotificationStatusFilterDTO Notification_NotificationStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            NotificationStatusFilter NotificationStatusFilter = new NotificationStatusFilter();
            NotificationStatusFilter.Skip = 0;
            NotificationStatusFilter.Take = 20;
            NotificationStatusFilter.OrderBy = NotificationStatusOrder.Id;
            NotificationStatusFilter.OrderType = OrderType.ASC;
            NotificationStatusFilter.Selects = NotificationStatusSelect.ALL;
            NotificationStatusFilter.Id = Notification_NotificationStatusFilterDTO.Id;
            NotificationStatusFilter.Code = Notification_NotificationStatusFilterDTO.Code;
            NotificationStatusFilter.Name = Notification_NotificationStatusFilterDTO.Name;

            List<NotificationStatus> NotificationStatuses = await NotificationStatusService.List(NotificationStatusFilter);
            List<Notification_NotificationStatusDTO> Notification_NotificationStatusDTOs = NotificationStatuses
                .Select(x => new Notification_NotificationStatusDTO(x)).ToList();
            return Notification_NotificationStatusDTOs;
        }

        [Route(NotificationRoute.FilterListOrganization), HttpPost]
        public async Task<List<Notification_OrganizationDTO>> FilterListOrganization([FromBody] Notification_OrganizationFilterDTO Notification_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Notification_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Notification_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Notification_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Notification_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Notification_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Notification_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = Notification_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = Notification_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = Notification_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = Notification_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Notification_OrganizationDTO> Notification_OrganizationDTOs = Organizations
                .Select(x => new Notification_OrganizationDTO(x)).ToList();
            return Notification_OrganizationDTOs;
        }

        [Route(NotificationRoute.SingleListAppUser), HttpPost]
        public async Task<List<Notification_AppUserDTO>> SingleListAppUser([FromBody] Notification_AppUserFilterDTO Notification_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Id = Notification_AppUserFilterDTO.Id;
            AppUserFilter.Username = Notification_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Notification_AppUserFilterDTO.DisplayName;
            AppUserFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Notification_AppUserDTO> StoreCheckerMonitor_AppUserDTOs = AppUsers
                .Select(x => new Notification_AppUserDTO(x)).ToList();
            return StoreCheckerMonitor_AppUserDTOs;
        }

        [Route(NotificationRoute.SingleListNotificationStatus), HttpPost]
        public async Task<List<Notification_NotificationStatusDTO>> SingleListNotificationStatus([FromBody] Notification_NotificationStatusFilterDTO Notification_NotificationStatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            NotificationStatusFilter NotificationStatusFilter = new NotificationStatusFilter();
            NotificationStatusFilter.Skip = 0;
            NotificationStatusFilter.Take = 20;
            NotificationStatusFilter.OrderBy = NotificationStatusOrder.Id;
            NotificationStatusFilter.OrderType = OrderType.ASC;
            NotificationStatusFilter.Selects = NotificationStatusSelect.ALL;
            NotificationStatusFilter.Id = Notification_NotificationStatusFilterDTO.Id;
            NotificationStatusFilter.Code = Notification_NotificationStatusFilterDTO.Code;
            NotificationStatusFilter.Name = Notification_NotificationStatusFilterDTO.Name;

            List<NotificationStatus> NotificationStatuses = await NotificationStatusService.List(NotificationStatusFilter);
            List<Notification_NotificationStatusDTO> Notification_NotificationStatusDTOs = NotificationStatuses
                .Select(x => new Notification_NotificationStatusDTO(x)).ToList();
            return Notification_NotificationStatusDTOs;
        }

        [Route(NotificationRoute.SingleListOrganization), HttpPost]
        public async Task<List<Notification_OrganizationDTO>> SingleListOrganization([FromBody] Notification_OrganizationFilterDTO Notification_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Notification_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Notification_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Notification_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Notification_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Notification_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Notification_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = Notification_OrganizationFilterDTO.Phone;
            OrganizationFilter.Email = Notification_OrganizationFilterDTO.Email;
            OrganizationFilter.Address = Notification_OrganizationFilterDTO.Address;
            OrganizationFilter.IsDisplay = true;
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Notification_OrganizationDTO> Notification_OrganizationDTOs = Organizations
                .Select(x => new Notification_OrganizationDTO(x)).ToList();
            return Notification_OrganizationDTOs;
        }

    }
}

