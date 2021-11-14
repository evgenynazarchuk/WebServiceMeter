/*
 * MIT License
 *
 * Copyright (c) Evgeny Nazarchuk.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using FluentAssertions;
using WebServiceMeter;

namespace TestService.Tests;

[TestClass]
public class FileTests
{
    private string Path => Directory.GetCurrentDirectory() + "/Tests/HttpToolTests/";

    [TestMethod]
    public async Task UploadFileTest()
    {
        // Arrange
        var app = new WebApplicationFactory<Startup>();
        var client = app.CreateClient();
        var httpTool = new HttpTool(client);
        var file = File.ReadAllBytes(this.Path + "/FileTests1.txt");

        // Act
        var httpResult = await httpTool.UploadFileAsync(path: "File/UploadFile", fileName: "FileTests1.txt", file: file);

        // Assert
        httpResult.Should().Be(200);
    }

    [TestMethod]
    public async Task UploadFilesTest()
    {
        // Arrange
        var app = new WebApplicationFactory<Startup>();
        var client = app.CreateClient();
        var httpTool = new HttpTool(client);
        var file1 = File.ReadAllBytes(this.Path + "/FileTests1.txt");
        var file2 = File.ReadAllBytes(this.Path + "/FileTests2.txt");
        var uploadFiles = new List<(string, byte[])> { ("FileTest1.txt", file1), ("FileTest2.txt", file2) };

        // Act
        var httpResult = await httpTool.UploadFilesAsync(path: "File/UploadFiles", files: uploadFiles);
        // Assert
        httpResult.Should().Be(200);
    }

    [TestMethod]
    public async Task DownloadFile()
    {
        // Arrange
        var app = new WebApplicationFactory<Startup>();
        var client = app.CreateClient();
        var httpTool = new HttpTool(client);
        var file = File.ReadAllBytes(this.Path + "/FileTests1.txt");
        await httpTool.UploadFileAsync(path: "File/UploadFile", fileName: "FileTests1.txt", file: file);

        // Act
        var (fileName, fileBytes) = await httpTool.DownloadFileAsync(path: "File/DownloadFile/1");

        // Assert
        fileName.Should().Be("FileTests1.txt");
        fileBytes.Should().HaveCount(217);
    }
}
