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

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWebApplication.Models;
using GrpcWebApplication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GrpcWebApplication;

public class UserMessagerHandler : UserMessagerService.UserMessagerServiceBase
{
    public UserMessagerHandler(
        ILogger<UserMessagerHandler> logger,
        DataContext dataContext)
    {
        this._logger = logger;
        this._dataContext = dataContext;
    }

    public override async Task<MessageIdentityDto> SendMessage(
        MessageCreateDto content,
        ServerCallContext context)
    {
        await this._dataContext.Set<Message>().AddAsync(new Message { Text = content.Text });
        await this._dataContext.SaveChangesAsync();

        return await Task.FromResult(new MessageIdentityDto { Id = 1 });
    }

    public override async Task<Empty> SendMessages(
        IAsyncStreamReader<MessageCreateDto> requestStream,
        ServerCallContext context)
    {
        // read stream
        while (await requestStream.MoveNext())
        {
            await this._dataContext.Set<Message>().AddAsync(new Message
            {
                Text = requestStream.Current.Text
            });
        }

        // save
        await this._dataContext.SaveChangesAsync();

        return new Empty();
    }

    public override async Task<MessageSimpleDto> GetMessage(
        MessageIdentityDto request,
        ServerCallContext context)
    {
        var message = await this._dataContext.Set<Message>().FindAsync(request.Id);
        var simpleDto = new MessageSimpleDto
        {
            Id = message.Id,
            Text = message.Text
        };

        return simpleDto;
    }

    public override async Task GetMessages(
        Empty request,
        IServerStreamWriter<MessageSimpleDto> responseStream,
        ServerCallContext context)
    {
        var messages = await this._dataContext.Set<Message>().ToListAsync();

        foreach (var message in messages)
        {
            await responseStream.WriteAsync(new MessageSimpleDto
            {
                Id = message.Id,
                Text = message.Text
            });
        }
    }

    public override async Task Messages(
        IAsyncStreamReader<MessageCreateDto> requestStream,
        IServerStreamWriter<MessageSimpleDto> responseStream,
        ServerCallContext context)
    {
        while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
        {
            await this._dataContext.Set<Message>().AddAsync(new Message
            {
                Text = requestStream.Current.Text
            });
        }
        await this._dataContext.SaveChangesAsync();

        if (!context.CancellationToken.IsCancellationRequested)
        {
            var messagesTask = await this._dataContext.Set<Message>().ToListAsync();

            foreach (var message in messagesTask)
            {
                await responseStream.WriteAsync(new MessageSimpleDto
                {
                    Id = message.Id,
                    Text = message.Text
                });
            }
        }
    }

    private readonly ILogger<UserMessagerHandler> _logger;

    private readonly DataContext _dataContext;
}
