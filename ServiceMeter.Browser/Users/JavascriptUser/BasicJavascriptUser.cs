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

namespace ServiceMeter.HttpTools.Tools.HttpTool.Users;

public abstract partial class BasicJavascriptUser : BasicUser, IDisposable
{
    public BasicJavascriptUser(string userName)
        : base(userName)
    {
        this.playwright = Microsoft.Playwright.Playwright.CreateAsync().GetAwaiter().GetResult();
        this.browser = playwright.Firefox.LaunchAsync(new()
        {
            FirefoxUserPrefs = new Dictionary<string, object>()
                {
                    { "network.http.max-connections", 20000 }
                },
            Headless = true
        }).GetAwaiter().GetResult();

        this.browserContext = browser.NewContextAsync().GetAwaiter().GetResult();
        this.page = this.browserContext.NewPageAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        this.page.CloseAsync().GetAwaiter().GetResult();
        this.browser.CloseAsync().GetAwaiter().GetResult();
    }

    protected readonly IPlaywright playwright;

    protected readonly IBrowser browser;

    protected readonly IBrowserContext browserContext;

    protected readonly IPage page;
}
