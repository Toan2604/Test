using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.ABE.Common;
using DMS.ABE.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using DMS.ABE.Service;
using DMS.ABE.Services.MGlobalUser;
using DMS.ABE.Service.MComment;
using DMS.ABE.Services.MFile;
using DMS.ABE.Enums;

namespace DMS.ABE.Rpc.discussion
{
    public class DiscussionRoute : Root
    {
        public const string Base = Rpc + Module + "/discussion";
        public const string Count = Base + "/count";
        public const string List = Base + "/list";
        public const string Create = Base + "/create";
        public const string Update = Base + "/update";
        public const string Delete = Base + "/delete";
        public const string SaveFile = Base + "/save-file";
        public const string MultiSaveFile = Base + "/multi-save-file";
        public const string SingleListGlobalUser = Base + "/single-list-global-user";
        public const string ListMentioned = Base + "/list-mentioned";
    }

    public class DiscussionController : RpcSimpleController
    {
        private ICurrentContext CurrentContext;
        private ICommentService CommentService;
        private IGlobalUserService GlobalUserService;
        private IFileService FileService;
        private DataContext DataContext;
        public DiscussionController(
            ICurrentContext CurrentContext,
            ICommentService CommentService,
            IGlobalUserService GlobalUserService,
            IFileService FileService,
            DataContext DataContext)
        {
            this.CurrentContext = CurrentContext;
            this.CommentService = CommentService;
            this.GlobalUserService = GlobalUserService;
            this.FileService = FileService;
            this.DataContext = DataContext;
        }

        [Route(DiscussionRoute.Count), HttpPost]
        public async Task<int> Count([FromBody] Discussion_CommentFilterDTO Discussion_CommentFilterDTO)
        {
            Guid DiscussionId = Discussion_CommentFilterDTO.DiscussionId?.Equal ?? Guid.Empty;

            return await CommentService.Count(DiscussionId);
        }


        [Route(DiscussionRoute.List), HttpPost]
        public async Task<List<Discussion_CommentDTO>> ListPost([FromBody] Discussion_CommentFilterDTO Discussion_CommentFilterDTO)
        {
            Guid DiscussionId = Discussion_CommentFilterDTO.DiscussionId?.Equal ?? Guid.Empty;
            List<Comment> Comments = await CommentService.List(DiscussionId, Discussion_CommentFilterDTO.OrderType);
            List<Discussion_CommentDTO> Discussion_CommentDTOs = Comments.Select(p => new Discussion_CommentDTO(p)).ToList();
            return Discussion_CommentDTOs;
        }


        [Route(DiscussionRoute.Create), HttpPost]
        public async Task<Discussion_CommentDTO> Create([FromBody] Discussion_CommentDTO Discussion_CommentDTO)
        {
            Discussion_CommentDTO.TrimString();
            Comment Comment = new Comment
            {
                Id = Discussion_CommentDTO.Id,
                DiscussionId = Discussion_CommentDTO.DiscussionId,
                Content = Discussion_CommentDTO.Content,
                Url = Discussion_CommentDTO.Url,
            };
            Comment.CommentAttachments = Discussion_CommentDTO.CommentAttachments?
            .Select(x => new CommentAttachment
            {
                Id = x.Id,
                AttachmentTypeId = x.AttachmentTypeId,
                Url = x.Url,
                Thumbnail = x.Thumbnail,
                Size = x.Size,
                Name = x.Name,
                Checksum = x.Checksum,
                Type = x.Type,
                AttachmentType = x.AttachmentType == null ? null : new AttachmentType
                {
                    Id = x.AttachmentType.Id,
                    Code = x.AttachmentType.Code,
                    Name = x.AttachmentType.Name,
                },
            }).ToList();

            Comment = await CommentService.Create(Comment);
            Discussion_CommentDTO = new Discussion_CommentDTO(Comment);
            return Discussion_CommentDTO;
        }

        [Route(DiscussionRoute.Update), HttpPost]
        public async Task<Discussion_CommentDTO> Update([FromBody] Discussion_CommentDTO Discussion_CommentDTO)
        {
            Discussion_CommentDTO.TrimString();
            Comment Comment = new Comment
            {
                Id = Discussion_CommentDTO.Id,
                DiscussionId = Discussion_CommentDTO.DiscussionId,
                Content = Discussion_CommentDTO.Content,
                Url = Discussion_CommentDTO.Url,
            };
            Comment.CommentAttachments = Discussion_CommentDTO.CommentAttachments?
                .Select(x => new CommentAttachment
                {
                    Id = x.Id,
                    AttachmentTypeId = x.AttachmentTypeId,
                    Url = x.Url,
                    Thumbnail = x.Thumbnail,
                    Size = x.Size,
                    Name = x.Name,
                    Checksum = x.Checksum,
                    Type = x.Type,
                    AttachmentType = x.AttachmentType == null ? null : new AttachmentType
                    {
                        Id = x.AttachmentType.Id,
                        Code = x.AttachmentType.Code,
                        Name = x.AttachmentType.Name,
                    },
                }).ToList();

            Comment = await CommentService.Update(Comment);
            Discussion_CommentDTO = new Discussion_CommentDTO(Comment);
            return Discussion_CommentDTO;
        }

        [Route(DiscussionRoute.Delete), HttpPost]
        public async Task<bool> Delete([FromBody] Discussion_CommentDTO Discussion_CommentDTO)
        {
            Discussion_CommentDTO.TrimString();
            Comment Comment = new Comment
            {
                Id = Discussion_CommentDTO.Id,
                DiscussionId = Discussion_CommentDTO.DiscussionId,
                Content = Discussion_CommentDTO.Content,
            };

            return await CommentService.Delete(Comment);
        }

        [Route(DiscussionRoute.SaveFile), HttpPost]
        public async Task<ActionResult<Discussion_FileDTO>> SaveFile(IFormFile file)
        {
            FileInfo fileInfo = new FileInfo(file.FileName);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var filename = file.FileName.ToLower().ChangeToEnglishChar().Replace(" ", "");
            Entities.File File = new Entities.File
            {
                Path = $"/discussion/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}/{filename}",
                Name = file.FileName,
                Content = memoryStream.ToArray()
            };
            File = await FileService.Create(File, File.Path);
            return new Discussion_FileDTO(File);
        }

        [Route(DiscussionRoute.MultiSaveFile), HttpPost]
        public async Task<ActionResult<List<Discussion_FileDTO>>> MultiSaveFile(List<IFormFile> files)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            List<Discussion_FileDTO> Discussion_FileDTOs = new List<Discussion_FileDTO>();
            foreach (IFormFile file in files)
            {
                MemoryStream memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                var filename = file.FileName.ToLower().ChangeToEnglishChar().Replace(" ","");
                Entities.File File = new Entities.File
                {
                    Path = $"/discussion/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}/{filename}",
                    Name = file.FileName,
                    Content = memoryStream.ToArray()
                };
                File = await FileService.Create(File, File.Path);
                if (File == null)
                    return BadRequest();
                Discussion_FileDTO Discussion_FileDTO = new Discussion_FileDTO(File);
                Discussion_FileDTOs.Add(Discussion_FileDTO);
            }
            return Ok(Discussion_FileDTOs);
        }

        [Route(DiscussionRoute.SingleListGlobalUser), HttpPost]
        public async Task<List<Discussion_GlobalUserDTO>> SingleListGlobalUser([FromBody] Discussion_GlobalUserFilterDTO Discussion_GlobalUserFilterDTO)
        {
            GlobalUserFilter GlobalUserFilter = new GlobalUserFilter
            {
                Id = Discussion_GlobalUserFilterDTO.Id,
                DisplayName = Discussion_GlobalUserFilterDTO.DisplayName,
                Username = Discussion_GlobalUserFilterDTO.Username,
                Skip = 0,
                Take= 10,
                OrderBy = GlobalUserOrder.Username,
                OrderType = OrderType.ASC,
                Selects = GlobalUserSelect.ALL,
                GlobalUserTypeId = new IdFilter { Equal =  GlobalUserTypeEnum.APPUSER.Id }
            };
            List<GlobalUser> GlobalUsers = await GlobalUserService.List(GlobalUserFilter);
            List<Discussion_GlobalUserDTO> Discussion_GlobalUserDTOs = GlobalUsers.Select(a => new Discussion_GlobalUserDTO(a)).ToList();

            return Discussion_GlobalUserDTOs;
        }

        [Route(DiscussionRoute.ListMentioned), HttpPost]
        public async Task<List<Discussion_MentionedDTO>> ListMentioned([FromBody] Discussion_MentionedFilterDTO Discussion_MentionedFilterDTO)
        {
            var query1 = from ac in DataContext.GlobalUserCommentMapping
                         join c in DataContext.Comment on ac.CommentId equals c.Id
                         join au in DataContext.GlobalUser on c.CreatorId equals au.Id
                         where (Discussion_MentionedFilterDTO.GlobalUserId.HasValue == false || ac.GlobalUserId == Discussion_MentionedFilterDTO.GlobalUserId.Value)
                         select new Discussion_MentionedDTO
                         {
                             GlobalUserName = c.Creator.DisplayName,
                             Avatar = c.Creator.Avatar,
                             CreatedAt = c.CreatedAt,
                             Url = c.Url,
                             Content = $"{c.Creator.DisplayName} đã nhắc đến bạn trong một bình luận"
                         };


            var result = query1.Skip(0).Take(5).AsEnumerable().ToList();
            return result;
        }
    }
}