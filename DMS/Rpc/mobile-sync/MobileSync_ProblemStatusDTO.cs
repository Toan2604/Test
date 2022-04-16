using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_ProblemStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public MobileSync_ProblemStatusDTO() { }
        public MobileSync_ProblemStatusDTO(ProblemStatus ProblemStatus)
        {
            this.Id = ProblemStatus.Id;
            this.Code = ProblemStatus.Code;
            this.Name = ProblemStatus.Name;
        }
    }
}