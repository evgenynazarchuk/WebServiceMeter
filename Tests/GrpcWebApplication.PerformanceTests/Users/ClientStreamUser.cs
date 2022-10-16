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
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceMeter.HttpService.Tools.HttpTool;

namespace GrpcWebApplication.PerformanceTests.Users;

public class ClientStreamUser : GrpcUser
{
    public ClientStreamUser(string address, string? userName = null)
        : base(address, typeof(UserMessagerService.UserMessagerServiceClient), userName)
    {
        UseGrpcClient(typeof(UserMessagerService.UserMessagerServiceClient));
    }

    protected override async Task PerformanceAsync()
    {
        var messages = new List<MessageCreateDto>
            {
                new MessageCreateDto { Text = "test 1" },
                new MessageCreateDto { Text = "test 2" },
                new MessageCreateDto { Text = "test 3" },
                new MessageCreateDto { Text = "test 4" },
                new MessageCreateDto { Text = "test 5" },
                new MessageCreateDto { Text = "test 6" },
                new MessageCreateDto { Text = "test 7" },
                new MessageCreateDto { Text = "test 8" },
                new MessageCreateDto { Text = "test 9" },
            };

        await ClientStream<Empty, MessageCreateDto>("SendMessages", messages);
    }
}
