using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_AppUserDTO : DataDTO
    {

        public long Id { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}