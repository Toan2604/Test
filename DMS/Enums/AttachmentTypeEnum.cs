using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Enums
{
    public static class AttachmentTypeEnum
    {
        public static GenericEnum IMAGE = new GenericEnum { Id = 1, Code = "Image", Name = "Image" };
        public static GenericEnum STICKER = new GenericEnum { Id = 2, Code = "Sticker", Name = "Sticker" };
        public static GenericEnum FILE = new GenericEnum { Id = 3, Code = "File", Name = "File" };
        public static GenericEnum URL = new GenericEnum { Id = 4, Code = "URL", Name = "Url" };
        public static List<GenericEnum> AttachmentTypeEnumList = new List<GenericEnum>
        {
            IMAGE,STICKER,FILE, URL
        };
    }
}
