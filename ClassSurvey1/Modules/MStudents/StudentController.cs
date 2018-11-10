using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClassSurvey1.Entities;
using ClassSurvey1.Modules.MClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassSurvey1.Modules.MStudents
{
    [Route("api/Students")]
    public class StudentController : CommonController
    {
        private IStudentService StudentService;

        public StudentController(IStudentService StudentService)
        {
            this.StudentService = StudentService;
        }
        
        
        [HttpGet("Count")]
        public int Count([FromBody]StudentSearchEntity StudentSearchEntity)
        {
            return StudentService.Count(UserEntity, StudentSearchEntity);
        }
        [HttpGet("List")]
        public List<StudentEntity> List([FromBody]StudentSearchEntity StudentSearchEntity)
        {
            return StudentService.List(UserEntity, StudentSearchEntity);
        }
        [HttpPut("{StudentId}")]
        public StudentEntity Update([FromBody] StudentEntity StudentEntity, [FromRoute]Guid StudentId)
        {
            return StudentService.Update(UserEntity, StudentId, StudentEntity);
        }
        [HttpGet("{StudentId}")]
        public StudentEntity Get([FromRoute]Guid StudentId)
        {
            return StudentService.Get(UserEntity, StudentId);
        }
        [HttpGet("Classes/{StudentId}")]
        public List<ClassEntity> GetClasses([FromRoute]Guid StudentId)
        {
            return StudentService.GetClasses(StudentId);
        }
        [HttpDelete("{StudentId}")]
        public bool Delete([FromRoute]Guid StudentId)
        {
            return StudentService.Delete(UserEntity, StudentId);
        }

        [HttpPost]
        public StudentEntity Create([FromBody]StudentExcelModel StudentExcelModel)
        {
            return StudentService.Create(StudentExcelModel);
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
                    StudentService.CreateFromExcel(bytes);
                }
            }
            return Ok();
        }
    }
}