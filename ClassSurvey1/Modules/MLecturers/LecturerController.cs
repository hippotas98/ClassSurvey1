using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClassSurvey1.Entities;
using ClassSurvey1.Modules.MClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules.MLecturers
{
    [Route("api/Lecturers")]
    public class LecturerController : CommonController
    {
        private ILecturerService LecturerService;

        public LecturerController(ILecturerService lecturerService)
        {
            this.LecturerService = lecturerService;
        }
        
        
        [HttpGet("Count")]
        public int Count([FromBody]LecturerSearchEntity LecturerSearchEntity)
        {
            return LecturerService.Count(UserEntity, LecturerSearchEntity);
        }
        [HttpGet("List")]
        public List<LecturerEntity> List([FromBody]LecturerSearchEntity LecturerSearchEntity)
        {
            return LecturerService.List(UserEntity, LecturerSearchEntity);
        }
        [HttpPut("{LecturerId}")]
        public LecturerEntity Update([FromBody] LecturerEntity LecturerEntity, [FromRoute]Guid LecturerId)
        {
            return LecturerService.Update(UserEntity, LecturerId, LecturerEntity);
        }
        [HttpGet("{LecturerId}")]
        public LecturerEntity Get([FromRoute]Guid LecturerId)
        {
            return LecturerService.Get(UserEntity, LecturerId);
        }
        [HttpDelete("{LecturerId}")]
        public bool Delete([FromRoute]Guid LecturerId)
        {
            return LecturerService.Delete(UserEntity, LecturerId);
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
                    LecturerService.Create(bytes);
                }
            }
            return Ok();
        }
    }
}