using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookProducerService.Models.DTOs;
using BookProducerService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookProducerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskHistoriesController : ControllerBase
    {

        private readonly ITaskService _taskService;
        public TaskHistoriesController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        // GET: api/TaskHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTaskHis(string strFind)
        {
            try
            {
                IEnumerable<TaskDto> taskHistories = await _taskService.GetTasks(strFind); ;
                return Ok(taskHistories);
            }
            catch (Exception ex)
            {

                return NotFound(ex.InnerException);
            }

        }
    }
}
