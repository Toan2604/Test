using TrueSight.Common;
using DMS.ABE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Rpc.conversation_message
{
    public class ConversationMessage_PublicFileDTO : DataDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsFile { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public Guid RowId { get; set; }
        public ConversationMessage_PublicFileDTO() { }
        public ConversationMessage_PublicFileDTO(PublicFile PublicFile)
        {
            this.Id = PublicFile.Id;
            this.Name = PublicFile.Name;
            this.IsFile = PublicFile.IsFile;
            this.Path = PublicFile.Path;
            this.Level = PublicFile.Level;
            this.RowId = PublicFile.RowId;
        }
    }
}
