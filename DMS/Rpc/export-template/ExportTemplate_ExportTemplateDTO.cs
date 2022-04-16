using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.export_template
{
    public class ExportTemplate_ExportTemplateDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public ExportTemplate_ExportTemplateDTO() { }
        public ExportTemplate_ExportTemplateDTO(ExportTemplate ExportTemplate)
        {
            this.Id = ExportTemplate.Id;
            this.Code = ExportTemplate.Code;
            this.Name = ExportTemplate.Name;
            this.Content = ExportTemplate.Content;
            this.FileName = ExportTemplate.FileName;
            this.Extension = ExportTemplate.Extension;
            this.Errors = ExportTemplate.Errors;
        }
    }

    public class ExportTemplate_ExportTemplateFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter FileName { get; set; }
        public StringFilter Extension { get; set; }
        public ExportTemplateOrder OrderBy { get; set; }
    }
}
