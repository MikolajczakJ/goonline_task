using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_API;
using ToDo_API.Entities;

namespace ToDo_Tests
{
    public class ToDoControllerTests:IClassFixture<WebApplicationFactory<ToDo_API.Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private static readonly InMemoryDatabaseRoot _dbRoot = new();
        public ToDoControllerTests(WebApplicationFactory<ToDo_API.Program> factory )
        {

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(service => service.ServiceType == typeof(IDbContextOptionsConfiguration<ToDoDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ToDoDbContext>(options =>
                        options.UseInMemoryDatabase("TestsToDoDB", _dbRoot));
                });
            });
            _client = _factory.CreateClient();

        }
        [Fact]
        public async Task Test_GetAllToDos_ReturnsOkWithAllToDos()
        {
            //Act
            var response = await _client.GetAsync("/api/todo");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task Test_GetOneToDo_ReturnsOkWithSingleToDo()
        {
            // Arrange
            int toDoId;
            var toDo = new ToDo
            {
                Title = "Test",
                Description = "Opis testowy",
                PercentageDone = 0,
                CreatedAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(1),
                IsDone = false
            };
            toDoId = SeedToDo(toDo);
            //Act 
            var response = await _client.GetAsync($"/api/todo/{toDoId}");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task Test_GetOneToDo_ReturnsNotFound()
        {
            //Act 
            var response = await _client.GetAsync("/api/todo/-1");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        }
        [Fact]
        public async Task Test_DeleteToDo_ReturnsNoContent()
        {
            //Arrange
            int toDoId;
            var toDo = new ToDo
            {
                Title = "Test",
                Description = "Opis testowy",
                PercentageDone = 0,
                CreatedAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(1),
                IsDone = false
            };
            toDoId = SeedToDo(toDo);
            //Act
            var response = await _client.DeleteAsync($"/api/todo/{toDoId}");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task Test_DeleteToDo_ReturnsNotFound()
        {
            //Act
            var response = await _client.DeleteAsync($"/api/todo/-1");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Test_AddNewToDo_ReturnsCreated()
        {
            //Arrange
            var newToDo = new
            {
                Title = "New Task",
                Description = "This is a new task",
                Expiration = DateTime.UtcNow.AddDays(2),
                PercentageDone = 0
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newToDo), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/todo", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task Test_AddNewToDo_ReturnsBadRequest_ForInvalidData()
        {
            //Arrange
            var newToDo = new
            {
                Title = "", // Invalid: Title is required
                Description = "This is a new task",
                Expiration = DateTime.UtcNow.AddDays(2),
                PercentageDone = 0
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(newToDo), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/todo", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Test_UpdateToDo_ReturnsNoContent()
        {
            //Arrange
            int toDoId;
            var toDo = new ToDo
            {
                Title = "Test",
                Description = "Opis testowy",
                PercentageDone = 0,
                CreatedAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(1),
                IsDone = false
            };
            toDoId = SeedToDo(toDo);
            var updatedToDo = new
            {
                Id = toDoId,
                Title = "Updated Task",
                Description = "This task has been updated",
                Expiration = DateTime.UtcNow.AddDays(3),
                PercentageDone = 50,
                IsDone = false
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedToDo), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync($"/api/todo/{toDoId}", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task Test_UpdateToDo_ReturnsNotFound_ForNonExistentToDo()
        {
            //Arrange
            var updatedToDo = new
            {
                Id = -1, // Non-existent ID
                Title = "Updated Task",
                Description = "This task has been updated",
                Expiration = DateTime.UtcNow.AddDays(3),
                PercentageDone = 50,
                IsDone = false
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedToDo), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync($"/api/todo/{updatedToDo.Id}", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Test_UpdateToDo_ReturnsBadRequest_ForInvalidData()
        {
            //Arrange
            int toDoId;
            var toDo = new ToDo
            {
                Title = "Test",
                Description = "Test description",
                PercentageDone = 0,
                CreatedAt = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddDays(1),
                IsDone = false
            };
            toDoId = SeedToDo(toDo);
            var updatedToDo = new
            {
                Id = toDoId,
                Title = "", // Invalid: Title is required
                Description = "This task has been updated",
                Expiration = DateTime.UtcNow.AddDays(3),
                PercentageDone = 50,
                IsDone = false
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedToDo), Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PutAsync($"/api/todo/{toDoId}", content);
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        private int SeedToDo(ToDo toDo)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

            dbContext.ToDos.Add(toDo);
            dbContext.SaveChanges();

            return toDo.Id;
        }

    }
}
