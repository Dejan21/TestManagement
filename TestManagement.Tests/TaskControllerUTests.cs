using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestManagement.Controllers;
using TestManagement.Dtos;
using TestManagement.Models;
using TestManagement.Repositories;

namespace TestManagement.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskRepository> _repoMock;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _repoMock = new Mock<ITaskRepository>();
            _controller = new TaskController(_repoMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithTasks()
        {
            
            var tasks = new List<TaskModel>
            {
                new TaskModel
                {
                    Id = 1,
                    Title = "Test",
                    Description = "Test description",
                    IsCompleted = false,
                    Priority = 1
                }
            };

            _repoMock
                .Setup(r => r.GetAll("all"))
                .ReturnsAsync(tasks);

           
            var result = await _controller.GetAll("all");

            
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<TaskModel>>(ok.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            
            _repoMock
                .Setup(r => r.GetById(999))
                .ReturnsAsync((TaskModel?)null);

            
            var result = await _controller.GetById(999);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenTaskExists()
        {
           
            var task = new TaskModel
            {
                Id = 1,
                Title = "Test",
                IsCompleted = false,
                Priority = 1
            };

            _repoMock
                .Setup(r => r.GetById(1))
                .ReturnsAsync(task);

           
            var result = await _controller.GetById(1);

            
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<TaskModel>(ok.Value);
            Assert.Equal(1, value.Id);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAt_WhenValid()
        {
            
            var dto = new TaskDto
            {
                Title = "New task",
                Description = "Some desc",
                Priority = 1,
                DueDate = null
            };

           
            _repoMock
                .Setup(r => r.Create(
                    It.IsAny<string>(),
                    It.IsAny<string?>(),
                    dto.Priority,
                    dto.DueDate))
                .ReturnsAsync(1);

            _repoMock
                .Setup(r => r.GetById(1))
                .ReturnsAsync(new TaskModel
                {
                    Id = 1,
                    Title = dto.Title,
                    Description = dto.Description,
                    Priority = dto.Priority,
                    DueDate = dto.DueDate,
                    IsCompleted = false
                });


            var result = await _controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(TaskController.GetById), created.ActionName);
            var value = Assert.IsType<TaskModel>(created.Value);
            Assert.Equal("New task", value.Title);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenRepositoryReturnsTrue()
        {
            
            var dto = new UpdateTaskDto
            {
                Title = "Updated title",
                Description = "Updated desc",
                Priority = 2,
                DueDate = null,
                IsCompleted = true
            };

            _repoMock
                .Setup(r => r.Update(
                    1,
                    dto.Title,
                    dto.Description,
                    dto.Priority,
                    dto.DueDate,
                    dto.IsCompleted))
                .ReturnsAsync(true);

           
            var result = await _controller.Update(1, dto);

       
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenRepositoryReturnsFalse()
        {

            var dto = new UpdateTaskDto
            {
                Title = "Updated title",
                Description = "Updated desc",
                Priority = 2,
                DueDate = null,
                IsCompleted = true
            };

            _repoMock
                .Setup(r => r.Update(
                    1,
                    dto.Title,
                    dto.Description,
                    dto.Priority,
                    dto.DueDate,
                    dto.IsCompleted))
                .ReturnsAsync(false);

       
            var result = await _controller.Update(1, dto);


            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SetComplete_ReturnsNoContent_WhenRepositoryReturnsTrue()
        {
            
            _repoMock
                .Setup(r => r.SetComplete(1, true))
                .ReturnsAsync(true);

           
            var result = await _controller.SetComplete(1, true);

           
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task SetComplete_ReturnsNotFound_WhenRepositoryReturnsFalse()
        {
            
            _repoMock
                .Setup(r => r.SetComplete(1, true))
                .ReturnsAsync(false);

            
            var result = await _controller.SetComplete(1, true);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenRepositoryReturnsTrue()
        {
            
            _repoMock
                .Setup(r => r.Delete(1))
                .ReturnsAsync(true);

           
            var result = await _controller.Delete(1);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenRepositoryReturnsFalse()
        {
            
            _repoMock
                .Setup(r => r.Delete(1))
                .ReturnsAsync(false);

        
            var result = await _controller.Delete(1);

            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
