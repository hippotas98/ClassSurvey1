using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClassSurvey1.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules.MClasses
{
    [Route("api/Classes")]
    [ApiController]
    public class ClassController : CommonController
    {
        private IClassService ClassService;
        public ClassController(IClassService classService)
        {
            this.ClassService = classService;
        }
        [HttpGet("Count")]
        public int Count([FromBody]ClassSearchEntity classSearchEntity)
        {
            return ClassService.Count(UserEntity, classSearchEntity);
        }
        [HttpGet("List")]
        public List<ClassEntity> List([FromBody]ClassSearchEntity classSearchEntity)
        {
            return ClassService.List(UserEntity, classSearchEntity);
        }
        [HttpPut("{ClassId}")]
        public ClassEntity Update([FromBody] ClassEntity classEntity, [FromRoute]Guid ClassId)
        {
            return ClassService.Update(UserEntity, ClassId, classEntity);
        }
        [HttpGet("{ClassId}")]
        public ClassEntity Get([FromRoute]Guid ClassId)
        {
            return ClassService.Get(UserEntity, ClassId);
        }
        [HttpDelete("{ClassId}")]
        public bool Delete([FromRoute]Guid ClassId)
        {
            return ClassService.Delete(UserEntity, ClassId);
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> Create([FromForm]UploadClass data)
        {
            IEnumerable<IFormFile> files = data.myFiles;
            foreach (var file in files)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    byte[] bytes = ms.ToArray();
                    ClassService.Create(bytes);
                }
            }
            return Ok();
        }
    }
}