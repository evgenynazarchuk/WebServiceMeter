﻿/*
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

using ServiceMeter.LogsServices;
using System.Text;
using System.Text.Json;
using ServiceMeter.HttpTools.GenerateReports.Models;

namespace ServiceMeter.HttpTools.GenerateReports.Services;

public class HttpRequestHtmlReport : HtmlGenerator<HttpLogMessage>
{
    public HttpRequestHtmlReport(
        string sourceJsonPath,
        string resultHtmlPath)
        : base(sourceJsonPath, resultHtmlPath) { }

    protected override string GenerateHtml(List<HttpLogMessage> logs)
    {
	    var startedRequest = logs
            .GroupBy(x => new
            {
                x.UserName,
                x.RequestLabel,
                x.StatusCode,
                x.RequestMethod,
                x.RequestUri,
                StartRequestTime = (long)(x.StartSendRequestTime / 10000000)
            })
            .Select(x => new StartRequestLog(
                userName: x.Key.UserName,
                requestLabel: x.Key.RequestLabel,
                statusCode: x.Key.StatusCode,
                requestMethod: x.Key.RequestMethod,
                requestUri: x.Key.RequestUri,
                startRequestTime: x.Key.StartRequestTime,
                countStartedRequest: x.LongCount()))
            .ToList();

        var completedRequest = logs
            .GroupBy(x => new
            {
                x.UserName,
                x.RequestLabel,
                x.StatusCode,
                x.RequestMethod,
                x.RequestUri,
                EndResponseTime = (long)(x.EndResponseTime / 10000000)
            })
            .Select(x => new EndRequestLog(
                x.Key.UserName,
                x.Key.RequestLabel,
                x.Key.StatusCode,
                x.Key.RequestMethod,
                x.Key.RequestUri,
                x.Key.EndResponseTime,
                x.LongCount(),
                responseTime: x.Average(y => y.EndResponseTime - y.StartSendRequestTime), // response time
                sentTime: x.Average(y => y.StartWaitResponseTime - y.StartSendRequestTime), // sent time
                waitTime: x.Average(y => y.StartResponseTime - y.StartWaitResponseTime), // wait time
                receiveTime: x.Average(y => y.EndResponseTime - y.StartResponseTime), // receive time
                x.Sum(y => y.SendBytes),
                x.Sum(y => y.ReceiveBytes)))
            .ToList();

        // traffic
        var totalTraffic = logs
            .GroupBy(x => new { EndResponseTime = x.EndResponseTime / 10000000 })
            .Select(x => new TotalDataLog(
                x.Key.EndResponseTime,
                x.Sum(y => y.SendBytes),
                x.Sum(y => y.ReceiveBytes)))
            .ToList();

        var userTraffic = logs
            .GroupBy(x => new
            {
                x.UserName,
                EndResponseTime = x.EndResponseTime / 10000000
            }
            ).Select(x => new UserDataLog(
                x.Key.UserName,
                x.Key.EndResponseTime,
                x.Sum(y => y.SendBytes),
                x.Sum(y => y.ReceiveBytes)))
            .ToList();

        var startedRequestTimeJsonString = new StringBuilder();
        var completedRequestTimeJsonString = new StringBuilder();
        var totalTrafficJsonStringLog = new StringBuilder();
        var userTrafficJsonStringLog = new StringBuilder();

        foreach (var item in completedRequest)
        {
            completedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
        }

        foreach (var item in startedRequest)
        {
            startedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
        }

        foreach (var item in totalTraffic)
        {
            totalTrafficJsonStringLog.Append(JsonSerializer.Serialize(item) + ",\n");
        }

        foreach (var item in userTraffic)
        {
            userTrafficJsonStringLog.Append(JsonSerializer.Serialize(item) + ",\n");
        }

        //
        string sourceData = @$"
<script>
const startedRequestRawLog = [{startedRequestTimeJsonString}]
const completedRequestRawLog = [{completedRequestTimeJsonString}]
const totalTrafficLog = [{totalTrafficJsonStringLog}]
const userTrafficLog = [{userTrafficJsonStringLog}]
</script>
";

        var plotlyJsLineDraw = @"
<script>
function PlotlyJsLineDraw(chartName, yaxisLabel, plotlyIdent, plotlyData, rawData=true) {
    let chartPlotData = []
	if(rawData)
	{
		for(let key in plotlyData) {
		chartPlotData.push({
			x: plotlyData[key].map(item => item.x),
			y: plotlyData[key].map(item => item.y),
			type: 'scatter',
			name: key,
			})
		}
	}
	else {
		chartPlotData = plotlyData
	}
	
	let chartLayout ={
		showlegend: true,
		legend: {
			bgcolor: '#1A1A1A',
			font: {
				color: '#7C7C7C',
				family: 'Open Sans',
				size: 14
			},
			orientation: 'h',
			y: -0.4
		},
		title: {
			text: chartName,
			font: {
				color: '#828282',
				family: 'Open Sans',
				size: 21
			},
		},
		xaxis: {
			title: {
				text: '',
			},
			gridcolor: '#3C3C3C',
			gridwidth: 1,
			tickfont : {
				size : 11,
				color : '#7C7C7C'
			}
		},
		
		yaxis: {
			title: {
				text: yaxisLabel,
				font: {
					color: '#7C7C7C',
					family: 'Open Sans',
					size: 14
				},
			},
			gridcolor: '#3C3C3C',
			gridwidth: 1,
		},
		plot_bgcolor:'#1A1A1A',
		paper_bgcolor:'#1A1A1A',
	}
	
	
	Plotly.newPlot(plotlyIdent, chartPlotData, chartLayout);
}
</script>
";

        var charts = @"
<script>
/*
**
*/
let startedRequestData = { };
for (let item of startedRequestRawLog)
{
    if (startedRequestData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        startedRequestData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.StartRequestTime);
	let timeString = date.toISOString().substr(11, 8);
    startedRequestData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.CountStartedRequest })
}
PlotlyJsLineDraw('Started Requests', 'Count', 'StartedRequestsChart', startedRequestData)
/*
**
*/
let completedRequestsData = { };
for (let item of completedRequestRawLog)
{
    if (completedRequestsData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        completedRequestsData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    completedRequestsData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.CompletedRequests })
}
PlotlyJsLineDraw('Completed Requests', 'Count', 'CompletedRequestsChart', completedRequestsData)
/*
**
*/
let responseTimeData = {}
for(let item of completedRequestRawLog) {
	if (responseTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        responseTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    responseTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ResponseTime / 10000 })
}
PlotlyJsLineDraw('Response Time', 'Milliseconds', 'ResponseTimeChart', responseTimeData)
/*
**
*/
let sentTimeData = { };
for (let item of completedRequestRawLog)
{
    if (sentTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        sentTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    sentTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.SentTime / 10000 })
}
PlotlyJsLineDraw('Data Timed Sending', 'Milliseconds', 'SentTimeChart', sentTimeData)
/*
**
*/
let waitTimeData = { };
for (let item of completedRequestRawLog)
{
    if (waitTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        waitTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    waitTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.WaitTime / 10000 })
}
PlotlyJsLineDraw('Data Wait Times', 'Milliseconds', 'WaitTimeChart', waitTimeData)
/*
**
*/
let receivedTimeData = { };
for (let item of completedRequestRawLog)
{
    if (receivedTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        receivedTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    receivedTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ReceivedTime / 10000 })
}
PlotlyJsLineDraw('Data Timed Receiving', 'Milliseconds', 'ReceivedTimeChart', receivedTimeData)
/*
**
*/
let sentBytesChartDataset = []
sentBytesChartDataset.push({
	x: totalTrafficLog.map(item => {
		let date = new Date(0);
		date.setSeconds(item.Time);
		return date.toISOString().substr(11, 8);
	}),
	y: totalTrafficLog.map(item => item.SentBytes),
	type: 'scatter'
})
PlotlyJsLineDraw('Total Sent Bytes', 'Bytes', 'SentBytesChart', sentBytesChartDataset, false)
/*
**
*/
let receivedBytesChartDataset = []
receivedBytesChartDataset.push({
	x: totalTrafficLog.map(item => {
		let date = new Date(0);
		date.setSeconds(item.Time);
		return date.toISOString().substr(11, 8);
	}),
	y: totalTrafficLog.map(item => item.ReceivedBytes),
	type: 'scatter'
})
PlotlyJsLineDraw('Total Received Bytes', 'Bytes', 'ReceivedBytesChart', receivedBytesChartDataset, false)
/*
**
*/
let userTrafficSentChartDataset = {}
for (let item of userTrafficLog)
{
    if (userTrafficSentChartDataset[item.UserName] == undefined)
    {
        userTrafficSentChartDataset[item.UserName] = []
    }
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);
    userTrafficSentChartDataset[item.UserName].push({ x: timeString, y: item.SentBytes })
}
PlotlyJsLineDraw('User Sent Bytes Chart', 'Bytes', 'UserSentBytesChart', userTrafficSentChartDataset)
/*
**
*/
let userTrafficReceivedChartDataset = {}
for (let item of userTrafficLog)
{
    if (userTrafficReceivedChartDataset[item.UserName] == undefined)
    {
        userTrafficReceivedChartDataset[item.UserName] = []
    }
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);
    userTrafficReceivedChartDataset[item.UserName].push({ x: timeString, y: item.ReceivedBytes })
}
PlotlyJsLineDraw('User Received Bytes Chart', 'Bytes', 'UserReceivedBytesChart', userTrafficReceivedChartDataset)
/*
**
*/
let requestTrafficSentChartDataset = { }
for (let item of completedRequestRawLog)
{
    if (requestTrafficSentChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        requestTrafficSentChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    requestTrafficSentChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.SentBytes })
}
PlotlyJsLineDraw('Request Sent Bytes Chart', 'Bytes', 'RequestSentBytesChart', requestTrafficSentChartDataset)
/*
**
*/
let requestTrafficReceivedChartDataset = { }
for (let item of completedRequestRawLog)
{
    if (requestTrafficReceivedChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        requestTrafficReceivedChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    requestTrafficReceivedChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.RequestUri + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ReceivedBytes })
}
PlotlyJsLineDraw('Request Received Bytes Chart', 'Bytes', 'RequestReceivedBytesChart', requestTrafficReceivedChartDataset)
</script>
";

        var bodyStyle = @"
<style>
body {
    background-color: #1A1A1A;
}
</style>
";

        //
        string htmlReport = $@"
<html>
<head>
<script src='https://cdn.plot.ly/plotly-2.3.0.min.js'></script>
{bodyStyle}
</head>
<body>
<div id='StartedRequestsChart' style='width:99%;height:400px;'></div>
<div id='CompletedRequestsChart' style='width:99%;height:400px;'></div>
<div id='ResponseTimeChart' style='width:99%;height:400px;'></div>
<div id='SentTimeChart' style='width:99%;height:400px;'></div>
<div id='WaitTimeChart' style='width:99%;height:400px;'></div>
<div id='ReceivedTimeChart' style='width:99%;height:400px;'></div>
<div id='SentBytesChart' style='width:99%;height:400px;'></div>
<div id='ReceivedBytesChart' style='width:99%;height:400px;'></div>
<div id='UserSentBytesChart' style='width:99%;height:400px;'></div>
<div id='UserReceivedBytesChart' style='width:99%;height:400px;'></div>
<div id='RequestSentBytesChart' style='width:99%;height:400px;'></div>
<div id='RequestReceivedBytesChart' style='width:99%;height:400px;'></div>
{sourceData}
{plotlyJsLineDraw}
{charts}
</body>
</html>
";

        return htmlReport;
    }
}