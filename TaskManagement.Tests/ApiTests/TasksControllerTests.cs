using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Enums;
public class TasksControllerTests
{
    [Fact]
    public async Task GetTasts_whenInvoked_shouldReturnTasksLists()
    {
        var mockedTaskService = new Mock<ITaskService>();

        var expectedResult = new TaskDto[]
        {
            new TaskDto {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "More infor on task 1",
                Status = "TODO",
                Priority = 0,
                AssignedTo = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                CreatedAt = DateTime.Now
            },
            new TaskDto {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                Description = "More infor on task 2",
                Status = "TODO",
                Priority = 0,
                AssignedTo = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                CreatedAt = DateTime.Now
            }
        };

        mockedTaskService.Setup(s => s.GetTasksAsync(null, null)).ReturnsAsync(expectedResult);

        var tasksController = new TasksController(mockedTaskService.Object);

        var res = await tasksController.GetTasks(UserTaskStatus.TODO, Guid.NewGuid());

        var okResult = Assert.IsType<OkObjectResult>(res);
        var returnedTasks = Assert.IsType<TaskDto[]>(okResult.Value);
    }
}