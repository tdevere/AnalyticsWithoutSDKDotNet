using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppCenterAnalyticsWithoutSDK
{
    class Program
    {
        static void Main(string[] args)
        {
            AnalyticsClient analyticsClient = new AnalyticsClient();
            Task.Run(() => analyticsClient.PostEvent().ContinueWith((previousTask) => 
            {
                Console.WriteLine($"Results: {previousTask.Result}");
            }));

            Console.ReadLine();
        }
    }

    public class AnalyticsClient
    {
        static System.Net.Http.HttpClient httpClient;
        static System.Net.Http.HttpClientHandler HttpClientHandler;
        static string apiUri = @"https://in.appcenter.ms/logs?api-version=1.0.0";
        static string AppSecret = System.Environment.GetEnvironmentVariable("AppCenterPowerShell");
        static string InstallId = Guid.NewGuid().ToString();
        static string sid;
        static string model = "DotNet Direct API";
        public AnalyticsClient()
        {
            sid = Guid.NewGuid().ToString();
        }

        private static System.Net.Http.HttpClientHandler GetHttpClientHandler()
        {
            if (HttpClientHandler == null)
            {
                HttpClientHandler = new System.Net.Http.HttpClientHandler() { MaxConnectionsPerServer = 20 };
            }

            return HttpClientHandler;
        }

        public async Task<HttpResponseMessage> PostEvent(string EventName = "CSharp-Event")
        {
            string msgId = Guid.NewGuid().ToString();
            HttpResponseMessage message;
            string SessiongDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var stringContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("App-Secret", AppSecret),
                new KeyValuePair<string, string>("Install-ID", InstallId),
            });

            string json = "{\"logs\":[{\"id\":\"$id\",\"name\":\"$EventName\",\"timestamp\":\"$timestamp\",\"sid\":\"$sid \",\"device\":{\"sdkName\":\"appcenter.winforms\",\"sdkVersion\":\"4.2.0\",\"model\":\"$model\",\"oemName\":\"HP\",\"osName\":\"WINDOWS\",\"osVersion\":\"10.0.19042\",\"osBuild\":\"10.0.19042.928\",\"locale\":\"en-US\",\"timeZoneOffset\":-360,\"screenSize\":\"3440x1440\",\"appVersion\":\"1.0.0.0\",\"appBuild\":\"1.0.0.0\",\"appNamespace\":\"AppCenter_WinForm\"},\"type\":\"event\"}]}";

            json = json.Replace("$id", msgId);
            json = json.Replace("$sid", sid);
            json = json.Replace("$model", model);
            json = json.Replace("$EventName", EventName);
            json = json.Replace("$timestamp", SessiongDateTime);


            Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8);

            httpClient = new HttpClient(GetHttpClientHandler(), false);
            httpClient.DefaultRequestHeaders.Add("App-Secret", AppSecret);
            httpClient.DefaultRequestHeaders.Add("Install-ID", InstallId);


            message = await httpClient.PostAsync(apiUri, content).ConfigureAwait(false);

            return message;
        }
       

    }
}
