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

using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcWebApplication.IntegrationTest.Support.Tool;

public sealed class GrpcClientTool : IGrpcClientTool, IDisposable
{
    private readonly GrpcChannel _grpcChannel;

    private readonly object _grpcClient;

    public GrpcClientTool(string address, Type serviceClientType)
    {
        this._grpcChannel = GrpcChannel.ForAddress(address);

        var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

        this._grpcClient = ctor is not null
            ? ctor.Invoke(new[] { this._grpcChannel })
            : throw new ApplicationException("gRpc client is not create");

        if (this._grpcClient is null)
        {
            throw new ApplicationException("gRpc client is not create");
        }
    }

    public GrpcClientTool(HttpClient httpClient, Type serviceClientType)
    {
        this._grpcChannel = httpClient.BaseAddress is not null
            ? GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions { HttpClient = httpClient })
            : throw new ApplicationException("Base address is not set");

        var ctor = serviceClientType.GetConstructor(new[] { typeof(GrpcChannel) });

        this._grpcClient = ctor is not null
            ? ctor.Invoke(new[] { this._grpcChannel })
            : throw new ApplicationException("gRpc client is not create");
    }

    public async ValueTask<TResponse> UnaryCallAsync<TResponse, TRequest>(string methodCall, TRequest requestBody)
        where TRequest : class, new()
        where TResponse : class, new()
    {
        var method = this._grpcClient
            .GetType()
            .GetMethods()
            .Where(x => x.Name == methodCall)
            .Single(x => x.GetParameters().Length == 4);

        var result = (AsyncUnaryCall<TResponse>?)method.Invoke(this._grpcClient, new object[] { requestBody, default!, default!, default! });

        if (result is not null)
        {
            return await result;
        }
        else
        {
            return await result!;
        }
    }

    public async ValueTask<TResponse> ClientStreamAsync<TResponse, TRequest>(string methodCall, ICollection<TRequest> requestBodyList)
        where TRequest : class, new()
        where TResponse : class, new()
    {
        var method = this._grpcClient
            .GetType()
            .GetMethods()
            .Where(x => x.Name == methodCall)
            .Single(x => x.GetParameters().Length == 3);

        using var grpcConnect = (AsyncClientStreamingCall<TRequest, TResponse>?)method.Invoke(this._grpcClient, new object[] { default!, default!, default! });

        if (grpcConnect is null)
        {
            throw new ApplicationException($"grpc connnect is null");
        }

        // TODO logging
        foreach (var requestBody in requestBodyList)
        {
            await grpcConnect.RequestStream.WriteAsync(requestBody);
        }

        await grpcConnect.RequestStream.CompleteAsync();

        return grpcConnect.ResponseAsync.Result;
    }

    public async ValueTask<IReadOnlyCollection<TResponse>> ServerStreamAsync<TResponse, TRequest>(string methodCall, TRequest requestBody)
        where TRequest : class, new()
        where TResponse : class, new()
    {
        var method = this._grpcClient
            .GetType()
            .GetMethods()
            .Where(x => x.Name == methodCall)
            .Single(x => x.GetParameters().Length == 4);

        using var grpcConnect = (AsyncServerStreamingCall<TResponse>?)method.Invoke(this._grpcClient, new object[] { requestBody, default!, default!, default! });
        if (grpcConnect is null)
        {
            throw new ApplicationException($"grpc connect error");
        }

        var messages = new List<TResponse>();

        while (await grpcConnect.ResponseStream.MoveNext())
        {
            messages.Add(grpcConnect.ResponseStream.Current);
        }

        return messages;
    }

    public async ValueTask<IReadOnlyCollection<TResponse>> BidirectionalStreamAsync<TResponse, TRequest>(string methodCall, ICollection<TRequest> requestBodyList)
        where TRequest : class, new()
        where TResponse : class, new()
    {
        var method = this._grpcClient
            .GetType()
            .GetMethods()
            .Where(x => x.Name == methodCall)
            .Single(x => x.GetParameters().Length == 3);

        using var grpcConnect = (AsyncDuplexStreamingCall<TRequest, TResponse>?)method.Invoke(this._grpcClient, new object[] { default!, default!, default! });
        if (grpcConnect is null)
        {
            throw new ApplicationException("grpc connect error");
        }

        var responseMessages = new List<TResponse>();

        var readMessageTask = Task.Run(async () =>
        {
            await foreach (var responseMessage in grpcConnect.ResponseStream.ReadAllAsync())
            {
                responseMessages.Add(responseMessage);
            }
        });

        foreach (var requestBody in requestBodyList)
        {
            await grpcConnect.RequestStream.WriteAsync(requestBody);
        }

        await grpcConnect.RequestStream.CompleteAsync();
        await readMessageTask;

        return responseMessages;
    }

    public void Dispose()
    {
        this._grpcChannel.Dispose();
    }
}
