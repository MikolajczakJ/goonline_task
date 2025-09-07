using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        private readonly HttpClient _client;

        public ToDoControllerTests(WebApplicationFactory<ToDo_API.Program> factory )
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(IDbContextOptionsConfiguration<ToDoDbContext>));
                        services.Remove(dbContextOptions!);

                        services.AddDbContext<ToDoDbContext>(options => options.UseInMemoryDatabase("ToDoDB"));
                    });
                })
                .CreateClient();

        }
        [Fact]
        public async Task Test_GetAllToDos_ReturnsOkWithAllToDos()
        {
            //Act
            var response = await _client.GetAsync("/api/todo");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        }
        //[Fact]
        //public async Task Test_GetOneToDo_ReturnsOkWithSingleToDo()
        //{
        //    //Act 
        //    var response = await _client.GetAsync("/api/todo/1");
        //    //Assert
        //    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        //}
        [Fact]
        public async Task Test_GetOneToDo_ReturnsNotFound()
        {
            //Act 
            var response = await _client.GetAsync("/api/todo/-1");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        }

    }
}
