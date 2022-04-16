using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class FileDAO
    {
        public FileDAO()
        {
            SurveyQuestionFileMappings = new HashSet<SurveyQuestionFileMappingDAO>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string MimeType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual ICollection<SurveyQuestionFileMappingDAO> SurveyQuestionFileMappings { get; set; }
    }
}
