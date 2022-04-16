using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.conversation_message
{
    public class ConversationMessage_ConversationAttachmentTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ConversationMessage_ConversationAttachmentTypeDTO() { }
        public ConversationMessage_ConversationAttachmentTypeDTO(AttachmentType ConversationAttachmentType)
        {
            this.Id = ConversationAttachmentType.Id;
            this.Code = ConversationAttachmentType.Code;
            this.Name = ConversationAttachmentType.Name;
            this.Informations = ConversationAttachmentType.Informations;
            this.Warnings = ConversationAttachmentType.Warnings;
            this.Errors = ConversationAttachmentType.Errors;
        }
    }

    public class ConversationMessage_ConversationAttachmentTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public AttachmentTypeOrder OrderBy { get; set; }
    }
}