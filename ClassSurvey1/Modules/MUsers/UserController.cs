﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules
{
    [Route("api/Users")]
    public class UserController : CommonController
    {
        public IUserService UserService;
        
        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
           
        }

        [Route("Count"), HttpGet]
        public long Count(SearchUserEntity SearchUserEntity)
        {
            return UserService.Count(SearchUserEntity);
        }

        [Route(""), HttpGet]
        public List<UserEntity> Get(SearchUserEntity SearchUserEntity)
        {
            return UserService.Get(SearchUserEntity);
        }
        [Route("{UserId}"), HttpGet]
        public UserEntity Get(Guid UserId)
        {
            return UserService.Get(UserId);
        }
        [Route(""), HttpPost]
        public UserEntity Create([FromBody]UserEntity UserEntity)
        {
            return UserService.Create(UserEntity);
        }
        //[Route("{UserId}"), HttpPut]
        //public UserEntity Update(Guid UserId, [FromBody]UserEntity UserEntity)
        //{
        //    return UserService.Update(UserId, UserEntity);
        //}
        [Route("{UserId}/ChangePassword"), HttpPut]
        public bool ChangePassword(Guid UserId, [FromBody]PasswordChangeEntity PasswordEntity)
        {
            return UserService.ChangePassword(UserId, PasswordEntity);
        }
        [Route("{UserId}"), HttpDelete]
        public bool Delete(Guid UserId)
        {
            return UserService.Delete(UserId);
        }
        [Route("Login"), HttpPost]
        public string Login([FromBody] UserEntity UserEntity)
        {
            string token = UserService.Login(UserEntity);
            var CookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30), Path = "/" };
            Response.Cookies.Append("JWT", token, CookieOptions);
            return token;
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> InsertUsers([FromForm]UploadClass data)
        {
            IEnumerable<IFormFile> files = data.myFiles;
            foreach (var file in files)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    byte[] bytes = ms.ToArray();
                    UserService.ConvertToIEnumrable<UserEntity>(bytes);
                }
            }
            return Ok();
        }
    }
}