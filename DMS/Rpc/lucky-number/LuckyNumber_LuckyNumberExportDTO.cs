using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public class LuckyNumber_LuckyNumberExportDTO : DataDTO
    {
        public long STT { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string RewardStatus { get; set; }
        public string Date { get; set; }
    }

    public class LuckyNumber_LuckyNumberStoreExportDTO : DataDTO
    {
        public long STT { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public decimal Revenue { get; set; }
        public string AppUserName { get; set; }
        public string LuckyNumberCode { get; set; }
        public string LuckyNumberName { get; set; }
        public string LuckyNumberValue { get; set; }
        public string LuckyNumberUsedAt { get; set; }

        public LuckyNumber_LuckyNumberStoreExportDTO()
        {
        }

    }
}
