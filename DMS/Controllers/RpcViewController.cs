using DMS.Rpc.dashboards.director;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Controllers
{
    public class ViewRoot
    {
        protected const string Module = "dms/view";
        protected const string Rpc = "rpc/";
        protected const string Rest = "rest/";
    }
    public class RpcViewController : Controller
    {
        protected string JsonSerialize(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return json;
        }
    }
}
