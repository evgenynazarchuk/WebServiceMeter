using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using FluentAssertions;
using WebServiceMeter;

namespace TestService.Tests;

[TestClass]
public class ContentTests
{
    [TestMethod]
    public async Task GetDefaultStringContentTest()
    {
        // Arrange
        var app = new WebApplicationFactory<Startup>();
        var client = app.CreateClient();
        var httpTool = new HttpTool(client);

        // Act
        var httpResult = await httpTool.GetAsync(path: "Test/GetDefaultString");

        // Assert
        httpResult.StatusCode.Should().Be(200);
        httpResult.ContentAsUTF8.Should().Be("Hello world");
    }
}
