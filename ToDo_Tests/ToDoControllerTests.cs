using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_Tests
{
    public class ToDoControllerTests
    {
        [Fact]
        public async Task Test_GetAllToDos_ReturnsOkWithAllToDos()
        {
            // Arrange
            var factory = new WebApplicationFactory<ToDo_API.Program>();
            var client = factory.CreateClient();
            //Act
            var response = await client.GetAsync("/api/todo");
            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        }
    }
}
