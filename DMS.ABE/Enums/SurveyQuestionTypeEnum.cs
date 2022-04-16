using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class SurveyQuestionTypeEnum
    {
        public static GenericEnum QUESTION_SINGLE_CHOICE = new GenericEnum { Id = 1, Code = "QUESTION_SINGLE_CHOICE", Name = "Câu hỏi chọn một" };
        public static GenericEnum QUESTION_MULTIPLE_CHOICE = new GenericEnum { Id = 2, Code = "QUESTION_MULTIPLE_CHOICE", Name = "Câu hỏi chọn nhiều" };
        public static GenericEnum TABLE_SINGLE_CHOICE = new GenericEnum { Id = 3, Code = "TABLE_SINGLE_CHOICE", Name = "Bảng chọn một" };
        public static GenericEnum TABLE_MULTIPLE_CHOICE = new GenericEnum { Id = 4, Code = "TABLE_MULTIPLE_CHOICE", Name = "Bảng chọn nhiều" };
        public static GenericEnum QUESTION_TEXT = new GenericEnum { Id = 5, Code = "QUESTION_TEXT", Name = "Câu hỏi nội dung" };
        public static List<GenericEnum> SurveyQuestionTypeEnumList = new List<GenericEnum>()
        {
            QUESTION_SINGLE_CHOICE, QUESTION_MULTIPLE_CHOICE, TABLE_SINGLE_CHOICE, TABLE_MULTIPLE_CHOICE, QUESTION_TEXT
        };
    }
}
