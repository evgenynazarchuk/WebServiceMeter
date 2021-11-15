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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketWebApplication.IntegrationTest.Support.Tool;

namespace WebSocketWebApplication.IntegrationTest;

[TestClass]
public class UnitTest2
{
    [TestMethod]
    public async Task TestMethod1()
    {
        // Arrange
        var wsUri = new UriBuilder()
        {
            Host = "localhost",
            Scheme = "ws",
            Path = "ws",
            Port = 5000
        }.Uri;
        var client = new WebSocketClientTool(wsUri);

        // Act
        await client.ConnectAsync();
        var message = await client.ReceiveMessageAsync();
        await client.DisconnectAsync();

        // Assert
        Console.WriteLine(message);
    }

    [TestMethod]
    public async Task TestMethod2()
    {
        // Arrange
        var wsUri = new UriBuilder()
        {
            Host = "localhost",
            Scheme = "ws",
            Path = "ws",
            Port = 5000
        }.Uri;
        var client1 = new WebSocketClientTool(wsUri);
        var client2 = new WebSocketClientTool(wsUri);

        // Act
        await client1.ConnectAsync();
        await client2.ConnectAsync();

        var message1 = await client1.ReceiveMessageAsync();
        var message2 = await client2.ReceiveMessageAsync();

        await client1.DisconnectAsync();
        await client2.DisconnectAsync();

        // Assert
        Console.WriteLine(message1);
        Console.WriteLine(message2);
    }

    [TestMethod]
    public async Task TestMethod3()
    {
        // Arrange
        var wsUri = new UriBuilder()
        {
            Host = "localhost",
            Scheme = "ws",
            Path = "ws",
            Port = 5000
        }.Uri;
        var client1 = new WebSocketClientTool(wsUri);
        var client2 = new WebSocketClientTool(wsUri);

        // Act
        await client1.ConnectAsync();
        await client2.ConnectAsync();

        var message1_1 = await client1.ReceiveMessageAsync();
        var message1_2 = await client1.ReceiveMessageAsync();

        var message2 = await client2.ReceiveMessageAsync();

        await client1.DisconnectAsync();
        await client2.DisconnectAsync();

        // Assert
        Console.WriteLine(message1_1);
        Console.WriteLine(message1_2);
        Console.WriteLine(message2);
    }

    [TestMethod]
    public async Task TestMethod4()
    {
        // Arrange
        var wsUri = new UriBuilder()
        {
            Host = "localhost",
            Scheme = "ws",
            Path = "ws",
            Port = 5000
        }.Uri;
        var client1 = new WebSocketClientTool(wsUri);
        var client2 = new WebSocketClientTool(wsUri);

        // Act
        await client1.ConnectAsync();
        await client2.ConnectAsync();

        var message1_1 = await client1.ReceiveMessageAsync();
        var message1_2 = await client1.ReceiveMessageAsync();

        await client1.DisconnectAsync();

        var message2 = await client2.ReceiveMessageAsync();

        await client2.DisconnectAsync();

        // Assert
        Console.WriteLine(message1_1);
        Console.WriteLine(message1_2);
        Console.WriteLine(message2);
    }

    [TestMethod]
    public void TestMethod5()
    {
        // Arrange
        var wsUri = new UriBuilder()
        {
            Host = "localhost",
            Scheme = "ws",
            Path = "ws",
            Port = 5000
        }.Uri;
        var tasks = new List<Task>();

        for (int i = 0; i < 1000; i++)
        {
            var task = Task.Run(async () =>
            {
                DateTime startConnect;
                DateTime endConnect;
                DateTime startReceive;
                DateTime endReceive;

                var client = new WebSocketClientTool(wsUri);

                startConnect = DateTime.UtcNow;
                await client.ConnectAsync();
                endConnect = DateTime.UtcNow;

                startReceive = DateTime.UtcNow;
                var message = await client.ReceiveMessageAsync();
                endReceive = DateTime.UtcNow;

                Console.WriteLine($"{endConnect - startConnect} {endReceive - startReceive}");

                await client.DisconnectAsync();
            });

            tasks.Add(task);
        }

        // Assert
        Task.WaitAll(tasks.ToArray());
    }
}