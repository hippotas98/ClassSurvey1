using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClassSurvey1.Entities;
using ClassSurvey1.Modules.MClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules.MAdmins
{
    [Route("api/Admins")]
    public class AdminController : CommonController
    {
        private IAdminService AdminService;

        public AdminController(IAdminService AdminService)
        {
            this.AdminService = AdminService;
        }
        
        
        [HttpGet("Count")]
        public int Count([FromBody]AdminSearchEntity AdminSearchEntity)
        {
            return AdminService.Count(UserEntity, AdminSearchEntity);
        }
        [HttpGet("List")]
        public List<AdminEntity> List([FromBody]AdminSearchEntity AdminSearchEntity)
        {
            return AdminService.List(UserEntity, AdminSearchEntity);
        }
        [HttpPut("{AdminId}")]
        public AdminEntity Update([FromBody] AdminEntity AdminEntity, [FromRoute]Guid AdminId)
        {
            return AdminService.Update(UserEntity, AdminId, AdminEntity);
        }
        [HttpGet("{AdminId}")]
        public AdminEntity Get([FromRoute]Guid AdminId)
        {
            return AdminService.Get(UserEntity, AdminId);
        }
        [HttpDelete("{AdminId}")]
        public bool Delete([FromRoute]Guid AdminId)
        {
            return AdminService.Delete(UserEntity, AdminId);
        }

        [HttpPost]
        public AdminEntity Create([FromBody]AdminDto AdminDto)
        {
            return AdminService.Create(UserEntity, AdminDto);
        }
        
    }
}