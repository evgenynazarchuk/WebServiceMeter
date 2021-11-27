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

using System.Reflection;
using System.Text;

namespace ServiceMeter.Runner;

public class ConsoleRunner
{
    private readonly Assembly _assembly;

    private readonly Dictionary<int, (Type, MethodInfo, object[]?)> _testsCollection = new();

    public ConsoleRunner(Assembly assembly)
    {
        this._assembly = assembly;

        var assemblyTypes = assembly.GetTypes()
            .Where(x => x.GetCustomAttributes()
            .Any(x => x is PerformanceTestClassAttribute))
            .ToList();

        int testNumber = 1;

        foreach (var assemblyType in assemblyTypes)
        {
            foreach (var methodInfo in assemblyType.GetMethods())
            {
                foreach (var attribute in methodInfo
                    .GetCustomAttributes()
                    .Where(x => x is PerformanceTestAttribute)
                    .Select(x => x as PerformanceTestAttribute)
                    )
                {
                    this._testsCollection.Add(testNumber++, (assemblyType, methodInfo, attribute?.Parameters));
                }
            }
        }
    }

    public async Task StartAsync()
    {
        this.DisplayTests();

        Console.Write($"Enter test number: ");

        if (!Int32.TryParse(Console.ReadLine(), out int selectedTestNumber))
        {
            throw new ApplicationException("Test number is incorrect");
        }

        var testClassType = this._testsCollection[selectedTestNumber].Item1;
        var testMethodInfo = this._testsCollection[selectedTestNumber].Item2;
        var parametersValues = this._testsCollection[selectedTestNumber].Item3;

        await StartTestAsync(testClassType, testMethodInfo, parametersValues);
    }

    public void DisplayTests()
    {
        foreach (var test in this._testsCollection)
        {
            var methodParameters = test.Value.Item2.GetParameters();
            var methodParametersName = methodParameters.Select(x => x.Name).ToList();
            var valuesParameters = new StringBuilder();

            for (int i = 0; i < test.Value.Item3?.Length; i++)
            {
                valuesParameters.Append($"{methodParametersName[i]}: {test.Value.Item3[i]}, ");
            }

            if (valuesParameters.Length > 0)
            {
                valuesParameters.Remove(valuesParameters.Length - 2, 2);
            }

            if (valuesParameters.Length > 0)
            {
                Console.WriteLine($"#{test.Key}: {test.Value.Item1.Name}.{test.Value.Item2.Name}: {valuesParameters}");
            }
            else
            {
                Console.WriteLine($"#{test.Key}: {test.Value.Item1.Name}.{test.Value.Item2.Name}");
            }
        }
    }

    private async Task StartTestAsync(Type testClassType, MethodInfo testMethodInfo, object[]? parametersValues)
    {
        var testClass = testClassType.GetConstructors().First().Invoke(null);

        if (testClass is null)
        {
            throw new ApplicationException("Error create test class");
        }

        Task? testTask;

        if (parametersValues is not null)
        {
            testTask = (Task?)testMethodInfo.Invoke(testClass, parametersValues);
        }
        else
        {
            testTask = (Task?)testMethodInfo.Invoke(testClass, null);
        }

        if (testTask is not null)
        {
            await testTask;
        }
    }
}
