using Elasticsearch.Net;
using Server.Models;
using Server.Protocols.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EzAspDotNet.Protocols.Page;
using EzAspDotNet.Services;

namespace Server.Controllers
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
        public async Task<string> Post(string index, [FromBody] GenericRequestData value)
        {
            var requestData = new GenericData { Date = value.Date, Content = value.Content };
            var countResponse = await _elasticClient.CountAsync<GenericData>(sd => sd
                .Index(index));

            requestData.Id = (countResponse.Count + 1).ToString();

            var jobject = JsonSerializer.Serialize(requestData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            var response = await _elasticClient.LowLevel.IndexAsync<StringResponse>(index, requestData.Id, PostData.String(jobject));
            return response.Body;
        }

        [HttpGet("{index}/{id}")]
        public async Task<GenericData> Get(string index, long id)
        {
            var response = await _elasticClient.GetAsync<GenericData>(id, idx => idx.Index(index)); // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            return response.Source; // the original document
        }

        [HttpPost("{index}/query")]
        public async Task<IEnumerable<GenericData>> Query(string index, [FromQuery] Pageable pageable, [FromBody] Query query)
        {
            var searchResponse = await _elasticClient.SearchAsync<GenericData>(sd => sd
                .Index(index)
                .From(pageable.Offset)
                .Size(pageable.Limit)
                .Query(q => query.Queries.Select(rq => q.Wildcard(c => c.Field(rq.Key).Value(rq.Value)))
                .Aggregate((c1, c2) => c1 || c2)));
            return searchResponse.Documents;
        }


        [HttpGet("{index}")]
        public async Task<IEnumerable<GenericData>> Get(string index, [FromQuery] Pageable pageable)
        {
            var searchResponse = await _elasticClient.SearchAsync<GenericData>(sd => sd
                .Index(index)
                .From(pageable.Offset)
                .Size(pageable.Limit));

            return searchResponse.Documents;
        }
    }
}
