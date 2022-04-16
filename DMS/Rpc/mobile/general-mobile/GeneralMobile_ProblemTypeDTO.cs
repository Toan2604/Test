using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_ProblemTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public GeneralMobile_ProblemTypeDTO() { }
        public GeneralMobile_ProblemTypeDTO(ProblemType ProblemType)
        {
            this.Id = ProblemType.Id;
            this.Code = ProblemType.Code;
            this.Name = ProblemType.Name;
        }
    }
}
