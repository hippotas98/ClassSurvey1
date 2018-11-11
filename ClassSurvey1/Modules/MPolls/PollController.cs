using ClassSurvey1.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules.MPolls
{
    [Route("api/Polls")]
    public class PollController : CommonController
    {
        private IPollService pollService;
        public PollController(IPollService pollService)
        {
            this.pollService = pollService;
        }
        [HttpPost]
        public IActionResult Create([FromBody]PollEntity pollEntity)
        {
            pollService.CreateOrUpdatePoll(UserEntity, pollEntity);
            return Ok();
        }
        [HttpPut]
        public IActionResult Update([FromBody]PollEntity pollEntity)
        {
            pollService.CreateOrUpdatePoll(UserEntity, pollEntity);
            return Ok();
        }
    }
}
