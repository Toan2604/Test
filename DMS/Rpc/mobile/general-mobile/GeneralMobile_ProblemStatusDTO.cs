using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_ProblemStatusDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public GeneralMobile_ProblemStatusDTO() { }
        public GeneralMobile_ProblemStatusDTO(ProblemStatus ProblemStatus)
        {
            this.Id = ProblemStatus.Id;
            this.Code = ProblemStatus.Code;
            this.Name = ProblemStatus.Name;
        }
    }

    public class GeneralMobile_ProblemStatusFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public ProblemOrder OrderBy { get; set; }
    }
}
