﻿using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_TaxTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
