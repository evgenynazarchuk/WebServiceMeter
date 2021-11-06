using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using FluentAssertions;
using WebServiceMeter;
using TestService.Models;

namespace TestService.Tests
{
    [TestClass]
    public class JsonContentTests
    {
        [TestMethod]
        public async Task GetDefaultObjectTest()
        {
            // Arrange
            var app = new WebApplicationFactory<Startup>();
            var client = app.CreateClient();
            var httpTool = new HttpTool(client);

            // Act
            var person = await httpTool.GetAsJsonAsync<Person>(path: "Test/GetDefaultObject");

            // Assert
            person.Id.Should().Be(-1);
            person.Name.Should().Be("TestName");
        }
    }
}
