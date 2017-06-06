using System.Collections.Generic;

namespace CourseManager.Core.EntitiesFromCustom
{
    public class ResultData
    {
        public ResultData()
        {
            isSuccess = true;
        }

        public bool isSuccess { get; set; }

        public string errorMsg { get; set; }

        public dynamic otherData { get; set; }

        public Dictionary<string, object> returnData {get;set;}
    }
}
