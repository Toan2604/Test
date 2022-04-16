using DMS.Common;
using DMS.Services.MRole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Rpc
{
    public class PermissionController : SimpleController
    {
        private IPermissionService PermissionService;
        private ICurrentContext CurrentContext;
        public PermissionController(IPermissionService PermissionService, ICurrentContext CurrentContext)
        {
            this.PermissionService = PermissionService;
            this.CurrentContext = CurrentContext;
        }

        [HttpPost, Route("rpc/dms/permission/list-path")]
        public async Task<List<string>> ListPath()
        {
            List<string> paths = await PermissionService.ListPath(CurrentContext.UserId);
            return paths;
        }

    }
}
