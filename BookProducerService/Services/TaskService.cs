using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using AutoMapper;
using BookProducer.Core.Entities;
using BookProducerService.Models.DTOs;
using BookProducerService.Repositories.Interfaces;
using BookProducerService.Services.Interfaces;

namespace BookProducerService.Services
{
    public class TaskService : ITaskService
    {
        private readonly IGenericRepository<TaskHistory> _repository;
        private readonly IMapper _mapper;
        public TaskService(IGenericRepository<TaskHistory> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TaskDto> GetTask(Guid id)
        {
            try
            {
                var task = await _repository.Get(id);
                return _mapper.Map<TaskDto>(task);
            }
            catch (Exception)
            {

                return null;
            }

        }

        public async Task<IEnumerable<TaskDto>> GetTasks(string strFind)
        {
            try
            {
                IEnumerable<TaskHistory> tasks = await _repository.GetAll();
                var tasksDto = _mapper.Map<ICollection<TaskDto>>(tasks);
                if (strFind != null && strFind != "")
                    return tasksDto.Where(item => item.Finish.Contains(strFind) || item.Requested.Contains(strFind) || item.Id.ToString().Equals(strFind)).ToList();
                else
                    return tasksDto;
            }
            catch// (Exception ex)
            {

                return null;
            }

        }
        public async Task<bool> InsertTask(TaskHistory taskHistory)
        {
            bool success;
            try
            {
                success = await _repository.Insert(taskHistory);
            }
            catch (Exception )
            {
                success = false;
            }
            return await Task.FromResult(success);
        }
        public Task<bool> UpdateTask(TaskHistory taskHistory)
        {
            bool success = false;
            try
            {
                success = _repository.Update(taskHistory);
            }
            catch (Exception)
            {
            }
            return Task.FromResult(success);
        }
        public async Task<bool> DeleteTask(Guid id)
        {
            bool res = false;
            try
            {
                TaskHistory task = await _repository.Get(id);

                if (task != null)
                {
                    res = _repository.Delete(task);
                }
            }
            catch (Exception)
            {

                // throw;
            }

            return res;
        }
    }
}
