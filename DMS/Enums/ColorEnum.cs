using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class ColorEnum
    {
        public static GenericEnum MIDNIGHT_BLUE = new GenericEnum { Id = 1, Code = "#223263", Name = "MIDNIGHT_BLUE" };
        public static GenericEnum ROSE_RED = new GenericEnum { Id = 2, Code = "#BC2C3D", Name = "ROSE_RED" };
        public static GenericEnum LIGHT_PURPLE = new GenericEnum { Id = 3, Code = "#5C78FF", Name = "LIGHT_PURPLE" };
        public static GenericEnum LIGHT_ORANGE = new GenericEnum { Id = 4, Code = "#FFB531", Name = "LIGHT_ORANGE" };
        public static GenericEnum MID_GREEN = new GenericEnum { Id = 5, Code = "#23AF63", Name = "MID_GREEN" };
        public static GenericEnum LOW_COBAN = new GenericEnum { Id = 6, Code = "#53D1B6", Name = "LOW_COBAN" };
        public static GenericEnum DARK_COBAN = new GenericEnum { Id = 7, Code = "#23B0B0", Name = "DARK_COBAN" };
        public static GenericEnum LOW_BROWN = new GenericEnum { Id = 8, Code = "#A0616A", Name = "LOW_BROWN" };
        public static GenericEnum ORANGE = new GenericEnum { Id = 9, Code = "#FF3D00", Name = "ORANGE" };
        public static GenericEnum PINK_PASTEL = new GenericEnum { Id = 10, Code = "#FF8D8D", Name = "PINK_PASTEL" };
        public static GenericEnum BABY_RED = new GenericEnum { Id = 11, Code = "#FF414D", Name = "BABY_RED" };
        public static GenericEnum NEON = new GenericEnum { Id = 12, Code = "#D2E603", Name = "NEON" };
        public static GenericEnum BABY_GREEN = new GenericEnum { Id = 13, Code = "#81B214", Name = "BABY_GREEN" };
        public static GenericEnum LOTUS_PINK = new GenericEnum { Id = 14, Code = "#FA26A0", Name = "LOTUS_PINK" };
        public static GenericEnum DUCK_GREEN = new GenericEnum { Id = 15, Code = "#17706E", Name = "DUCK_GREEN" };
        public static GenericEnum COBAN_BLUES = new GenericEnum { Id = 16, Code = "#40BFFF", Name = "COBAN_BLUES" };
        public static List<GenericEnum> ColorEnumList = new List<GenericEnum>
        {
            MIDNIGHT_BLUE, ROSE_RED, LIGHT_PURPLE, LIGHT_ORANGE,
            MID_GREEN,LOW_COBAN, DARK_COBAN, LOW_BROWN,
            ORANGE,PINK_PASTEL, BABY_RED, NEON,
            BABY_GREEN, LOTUS_PINK, DUCK_GREEN, COBAN_BLUES,
        };
    }
}
