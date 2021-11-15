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
using WebServiceMeter.Tools;

namespace WebServiceMeter;

public class ChromiumTool : Tool, IAsyncDisposable
{
    public readonly IPlaywright Playwright;

    public readonly IBrowser Browser;

    public readonly string UserName;

    public ChromiumTool(string userName, Watcher watcher)
        : base(watcher)
    {
        this.Playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
        this.Browser = Playwright.Chromium.LaunchAsync(new()
        {
            Headless = true
        }).GetAwaiter().GetResult();
        this.UserName = userName;
    }

    public ChromiumTool(IPlaywright playwright, IBrowser browser, string userName, Watcher watcher)
        : base(watcher)
    {
        this.Playwright = playwright;
        this.Browser = browser;
        this.UserName = userName;
    }

    public async Task<PageTool> GetPageTool()
    {
        IBrowserContext browserContext = await Browser.NewContextAsync();
        IPage page = await browserContext.NewPageAsync();

        return new PageTool(browserContext, page, this.UserName, this.Watcher);
    }

    public static Task<IBrowserContext> GetNewContextAsync(IBrowser browser)
    {
        return browser.NewContextAsync();
    }

    public static Task<IPage> GetNewPageAsync(IBrowserContext browserContext)
    {
        return browserContext.NewPageAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await this.Browser.CloseAsync();
    }
}
