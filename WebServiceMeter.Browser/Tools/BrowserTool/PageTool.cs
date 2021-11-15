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

using Microsoft.Playwright;
using WebServiceMeter.Reports;
using WebServiceMeter.Support;

namespace WebServiceMeter.Tools;

public class PageTool : Tool
{
    public readonly IPage Page;

    public readonly IBrowserContext BrowserContext;

    public readonly string UserName;

    public string? Url { get; private set; }

    public PageTool(IBrowserContext browserContext, IPage page, string userName, Watcher watcher)
        : base(watcher)
    {
        this.BrowserContext = browserContext;
        this.Page = page;

        this.UserName = userName;
    }

    public async Task OpenPage(string url, string label = "goto")
    {
        var start = ScenarioTimer.Time.Elapsed.Ticks;
        await this.Page.GotoAsync(url);
        await this.WaitAsync();
        var end = ScenarioTimer.Time.Elapsed.Ticks;

        this.Url = url;

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                "UserActionLog.json",
                $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}",
                typeof(ChromiumLogMessage));
        }
    }

    public async Task ReloadAsync(string label = "reload")
    {
        var start = ScenarioTimer.Time.Elapsed.Ticks;
        await this.Page.ReloadAsync();
        await this.WaitAsync();
        var end = ScenarioTimer.Time.Elapsed.Ticks;

        this.Url = this.Page.Url;

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                "UserActionLog.json",
                $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}",
                typeof(ChromiumLogMessage));
        }
    }

    public async Task Click(string selector, string label = "click")
    {
        var start = ScenarioTimer.Time.Elapsed.Ticks;
        await this.Page.ClickAsync(selector);
        await this.WaitAsync();
        var end = ScenarioTimer.Time.Elapsed.Ticks;

        this.Url = this.Page.Url;

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                "UserActionLog.json",
                $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}",
                typeof(ChromiumLogMessage));
        }
    }

    public async Task Type(string selector, string text, string label = "type")
    {
        var start = ScenarioTimer.Time.Elapsed.Ticks;
        await this.Page.TypeAsync(selector, text);
        await this.WaitAsync();
        var end = ScenarioTimer.Time.Elapsed.Ticks;

        this.Url = this.Page.Url;

        if (this.Watcher is not null)
        {
            this.Watcher.SendMessage(
                "UserActionLog.json",
                $"{this.UserName}\t{this.Url}\t{label}\t{start}\t{end}",
                typeof(ChromiumLogMessage));
        }
    }

    public async Task WaitAsync()
    {
        await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task CloseAsync()
    {
        await this.Page.CloseAsync();
        await this.BrowserContext.CloseAsync();
    }
}
