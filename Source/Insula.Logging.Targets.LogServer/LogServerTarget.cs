using Insula.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Insula.Logging.Targets.LogServer
{
    public class LogServerTarget : ILogTarget
    {
        public LogServerTarget(string url, string applicationKey)
        {
            if (url.IsNullOrWhiteSpace()
                || !Uri.IsWellFormedUriString(url, UriKind.Absolute)
                || !Uri.TryCreate(url, UriKind.Absolute, out _uri))
            {
                throw new ArgumentException("Provide valid URL.");
            }

            if (applicationKey.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Provide valid Application Key.");
            }

            _applicationKey = applicationKey;
        }

        private Uri _uri;
        private string _applicationKey;

        public void Submit(LogEvent logEvent)
        {
            var url = new Uri(_uri, "LogEvent/" + _applicationKey).ToString();
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(logEvent), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.BaseAddress = _uri;
            client.PostAsync(url, content)
                .ContinueWith(response => response.Result.EnsureSuccessStatusCode())
                .ContinueWith(error =>
                {
                    if (error.Exception != null)
                    {
                        System.Diagnostics.Debug.WriteLine(error.Exception.Message);
                        System.Diagnostics.Debug.WriteLine(error.Exception.StackTrace);
                        throw new Exception("Submit failed.", error.Exception);
                    }
                })
                .Wait();
        }
    }
}
