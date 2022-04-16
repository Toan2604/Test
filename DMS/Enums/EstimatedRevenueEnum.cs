using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Enums
{
    public class EstimatedRevenueEnum
    {
        public static GenericEnum LESS_20M = new GenericEnum { Id = 1, Code = "LESS_20M", Name = "< 20 triệu" };
        public static GenericEnum BETWEEN_20M_50M = new GenericEnum { Id = 2, Code = "BETWEEN_20M_50M", Name = "20 triệu -> 50 triệu" };
        public static GenericEnum BETWEEN_50M_100M = new GenericEnum { Id = 3, Code = "BETWEEN_50M_100M", Name = "50 triệu -> 100 triệu" };
        public static GenericEnum BETWEEN_100M_200M = new GenericEnum { Id = 4, Code = "BETWEEN_100M_200M", Name = "100 triệu -> 200 triệu" };
        public static GenericEnum BETWEEN_200M_300M = new GenericEnum { Id = 5, Code = "BETWEEN_200M_300M", Name = "200 triệu -> 300 triệu" };
        public static GenericEnum BETWEEN_300M_500M = new GenericEnum { Id = 6, Code = "BETWEEN_300M_500M", Name = "300 triệu -> 500 triệu" };
        public static GenericEnum GREATER_500M = new GenericEnum { Id = 7, Code = "GREATER_500M", Name = "> 500 triệu" };

        public static List<GenericEnum> EstimatedRevenueEnumList = new List<GenericEnum>
        {
            LESS_20M, BETWEEN_20M_50M, BETWEEN_50M_100M, BETWEEN_100M_200M, BETWEEN_200M_300M, BETWEEN_300M_500M, GREATER_500M
        };
    }

}
