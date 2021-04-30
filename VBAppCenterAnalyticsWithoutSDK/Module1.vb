Imports System.Net.Http
Imports System.Text
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks

Module Module1

    Sub Main()
        Dim analyticsClient As AnalyticsClient = New AnalyticsClient()

        Dim t As Task(Of HttpResponseMessage) = Task.Run(Function() analyticsClient.PostEvent("VBNet-Event"))
        t.Wait()
        Console.WriteLine(String.Format("Results: {0}", t.Result))

        Console.ReadLine()
    End Sub

End Module

Public Class AnalyticsClient
    Private Shared httpClient As HttpClient

    Private Shared HttpClientHandler As HttpClientHandler

    Private Shared apiUri As String

    Private Shared AppSecret As String

    Private Shared InstallId As String

    Private Shared sid As String

    Private Shared model As String

    Shared Sub New()
        AnalyticsClient.apiUri = "https://in.appcenter.ms/logs?api-version=1.0.0"
        AnalyticsClient.AppSecret = Environment.GetEnvironmentVariable("AppCenterPowerShell")
        AnalyticsClient.InstallId = Guid.NewGuid().ToString()
        AnalyticsClient.model = "DotNet Direct API"
    End Sub

    Public Sub New()
        MyBase.New()
        AnalyticsClient.sid = Guid.NewGuid().ToString()
    End Sub

    Private Shared Function GetHttpClientHandler() As System.Net.Http.HttpClientHandler
        If (AnalyticsClient.HttpClientHandler Is Nothing) Then
            Dim httpClientHandler As System.Net.Http.HttpClientHandler = New System.Net.Http.HttpClientHandler()
            httpClientHandler.MaxConnectionsPerServer = 20
            AnalyticsClient.HttpClientHandler = httpClientHandler
        End If
        Return AnalyticsClient.HttpClientHandler
    End Function

    Public Async Function PostEvent(Optional ByVal EventName As String = "CSharp-Event") As Task(Of System.Net.Http.HttpResponseMessage)
        Dim str As String = Guid.NewGuid().ToString()
        Dim str1 As String = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")
        Dim keyValuePair() As KeyValuePair(Of String, String) = {New KeyValuePair(Of String, String)("App-Secret", AnalyticsClient.AppSecret), New KeyValuePair(Of String, String)("Install-ID", AnalyticsClient.InstallId)}
        Dim formUrlEncodedContent As System.Net.Http.FormUrlEncodedContent = New System.Net.Http.FormUrlEncodedContent(keyValuePair)
        Dim str2 As String = "{""logs"":[{""id"":""$id"",""name"":""$EventName"",""timestamp"":""$timestamp"",""sid"":""$sid "",""device"":{""sdkName"":""appcenter.winforms"",""sdkVersion"":""4.2.0"",""model"":""$model"",""oemName"":""HP"",""osName"":""WINDOWS"",""osVersion"":""10.0.19042"",""osBuild"":""10.0.19042.928"",""locale"":""en-US"",""timeZoneOffset"":-360,""screenSize"":""3440x1440"",""appVersion"":""1.0.0.0"",""appBuild"":""1.0.0.0"",""appNamespace"":""AppCenter_WinForm""},""type"":""event""}]}"

        str2 = str2.Replace("$id", str)
        str2 = str2.Replace("$sid", AnalyticsClient.sid)
        str2 = str2.Replace("$model", AnalyticsClient.model)
        str2 = str2.Replace("$EventName", EventName)
        str2 = str2.Replace("$timestamp", str1)

        Console.WriteLine(str2)

        Dim stringContent As System.Net.Http.StringContent = New System.Net.Http.StringContent(str2, Encoding.UTF8)
        AnalyticsClient.httpClient = New HttpClient(AnalyticsClient.GetHttpClientHandler(), False)
        AnalyticsClient.httpClient.DefaultRequestHeaders().Add("App-Secret", AnalyticsClient.AppSecret)
        AnalyticsClient.httpClient.DefaultRequestHeaders().Add("Install-ID", AnalyticsClient.InstallId)
        Dim configuredTaskAwaitable As ConfiguredTaskAwaitable(Of System.Net.Http.HttpResponseMessage) = AnalyticsClient.httpClient.PostAsync(AnalyticsClient.apiUri, stringContent).ConfigureAwait(False)
        Dim httpResponseMessage As System.Net.Http.HttpResponseMessage = Await configuredTaskAwaitable
        Dim httpResponseMessage1 As System.Net.Http.HttpResponseMessage = httpResponseMessage
        httpResponseMessage = Nothing
        Dim httpResponseMessage2 As System.Net.Http.HttpResponseMessage = httpResponseMessage1
        str = Nothing
        httpResponseMessage1 = Nothing
        str1 = Nothing
        formUrlEncodedContent = Nothing
        str2 = Nothing
        stringContent = Nothing
        Return httpResponseMessage2
    End Function
End Class
