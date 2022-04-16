using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.discussion
{
    public class Discussion_AttachmentTypeDTO : DataDTO
    {
        
        public long Id { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public Discussion_AttachmentTypeDTO() {}
        public Discussion_AttachmentTypeDTO(AttachmentType AttachmentType)
        {
            
            this.Id = AttachmentType.Id;
            
            this.Code = AttachmentType.Code;
            
            this.Name = AttachmentType.Name;
            
            this.Informations = AttachmentType.Informations;
            this.Warnings = AttachmentType.Warnings;
            this.Errors = AttachmentType.Errors;
        }
    }

    public class Discussion_AttachmentTypeFilterDTO : FilterDTO
    {
        
        public IdFilter Id { get; set; }
        
        public StringFilter Code { get; set; }
        
        public StringFilter Name { get; set; }
        
        public AttachmentTypeOrder OrderBy { get; set; }
    }
}