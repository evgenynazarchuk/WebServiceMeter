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

using Microsoft.AspNetCore.Mvc;

namespace ServiceMeter.Runner;

[Controller]
[Route("[controller]/[action]")]
public class TestRunnerController : ControllerBase
{
    private readonly TestRunnerService _testRunner;

    public TestRunnerController(TestRunnerService testRunner)
    {
        this._testRunner = testRunner;
    }

    [HttpGet]
    public IActionResult GetTestsDetails()
    {
        var testsDetails = this._testRunner.GetTestsDetails();

        return Ok(testsDetails);
    }

    [HttpPost]
    public async Task<IActionResult> StartTest([FromBody] StartTestMethodDto startTestDto)
    {
        try
        {
            await this._testRunner.StartTestAsync(startTestDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult GetStatus()
    {
        var status = this._testRunner.GetStatus();

        if (status is not null)
        {
            return BadRequest(status);
        }
        else
        {
            return Ok("Test Runner is available");
        }
    }
}
