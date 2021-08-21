using Microsoft.Extensions.Configuration;

namespace LogQueryServer.Settings
{
    public static class Extend
    {
        public static ElasticSearch ElasticSearch(this IConfiguration configuration)
        {
            return configuration.GetSection("ElasticSearch").Get<ElasticSearch>();
        }
    }
}
