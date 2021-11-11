using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Models
{
   
    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }

        [JsonProperty("Microsoft.Hosting.Lifetime")]
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class ApiKeyConfig
    {
        public string Secret { get; set; }
       
    }

    public class Mongo
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Secret { get; set; }
    }

    public class CommonEndpoint
    {
        public string BaseUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Apikey { get; set; }
    }

    public class AppSettings
    {
        public Logging Logging { get; set; }
        public ApiKeyConfig ApiKeyConfig { get; set; }
        public Mongo mongo { get; set; }
        public CommonEndpoint CommonEndpoint { get; set; }
    }
}
