using Elasticsearch.Net;
using LogQueryServer.Models;
using LogQueryServer.Protocols.Request;
using LogQueryServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogQueryServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;

        private readonly ElasticClient _elasticClient;

        public QueryController(ILogger<QueryController> logger,
            QueryService queryService)
        {
            _logger = logger;
            _elasticClient = queryService.Client;
        }

        [HttpDelete("{index}")]
        public async Task Delete(string index)
        {
            await _elasticClient.Indices.DeleteAsync(index);
        }

        [HttpPost("{index}")]
        public async Task<string> Post(string index, [FromBody] GenericData value)
        {
            var jobject = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            //dynamic ex = new ExpandoObject();
            //ex.Id = logData.Id;
            //ex.Date = logData.Date;

            //var dict = ex as IDictionary<string, dynamic>;
            //foreach (var kvp in logData.Content)
            //{
            //    dict[kvp.Key] = kvp.Value;
            //}

            //var json = JsonConvert.SerializeObject(ex);
            //var request = new IndexRequest<object>(ex, index, logData.Id);
            //var response = await _elasticClient.IndexAsync<object>(request); //or specify index via settings.DefaultIndex("mytweetindex");

            var response = await _elasticClient.LowLevel.IndexAsync<StringResponse>(index, value.Id.ToString(), PostData.String(jobject));
            return response.Body;
        }

        [HttpGet("{index}/{id}")]
        public async Task<GenericData> Get(string index, int id)
        {
            var response = await _elasticClient.GetAsync<GenericData>(id, idx => idx.Index(index)); // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            return response.Source; // the original document
        }

        [HttpPost("{index}/query")]
        public async Task<IEnumerable<GenericData>> Query(string index, [FromQuery]Pageable pageable, [FromBody]Query query)
        {
            var searchResponse = await _elasticClient.SearchAsync<GenericData>(sd => sd
                .Index(index)
                .From(pageable.From)
                .Size(pageable.Size)
                .Query(q => query.Queries.Select(rq => q.Match(m => m.Field(rq.Key).Query(rq.Value))).Aggregate((c1, c2) => c1 || c2)));

            return searchResponse.Documents;
        }


        [HttpGet("{index}")]
        public async Task<IEnumerable<GenericData>> Get(string index, [FromQuery]Pageable pageable)
        {
            var searchResponse = await _elasticClient.SearchAsync<GenericData>(sd => sd
                .Index(index)
                .From(pageable.From)
                .Size(pageable.Size));

            return searchResponse.Documents;
        }
    }
}
