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

using Grpc.Net.Client;
using GrpcWebApplication.IntegrationTest.Support.Tool;
using GrpcWebApplication.Services;
using System;
using System.Net.Http;

namespace GrpcWebApplication.IntegrationTest.Support;

public class TestEnvironment : IDisposable
{
    public readonly TestApplication App;

    public readonly HttpClient HttpClient;

    public readonly GrpcChannel GrpcChannel;

    public readonly UserMessagerService.UserMessagerServiceClient UserMessagerClient;

    public readonly GrpcClientTool GrpcClient;

    public readonly DataContext Repository;

    public TestEnvironment()
    {
        this.App = new();

        this.HttpClient = this.App.CreateDefaultClient();
        if (this.HttpClient.BaseAddress is null)
        {
            throw new ApplicationException("address is not set");
        }

        this.GrpcChannel = GrpcChannel.ForAddress(this.HttpClient.BaseAddress, new GrpcChannelOptions { HttpClient = this.HttpClient });
        this.UserMessagerClient = new UserMessagerService.UserMessagerServiceClient(this.GrpcChannel);
        this.GrpcClient = new GrpcClientTool(this.HttpClient, typeof(UserMessagerService.UserMessagerServiceClient));

        this.Repository = new DataContext();
        this.Repository.Database.EnsureCreated();
    }

    public void Dispose()
    {
        this.GrpcChannel.Dispose();
        this.Repository.Database.EnsureDeleted();
    }
}
