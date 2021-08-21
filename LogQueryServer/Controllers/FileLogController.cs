using Elasticsearch.Net;
using LogQueryServer.Models;
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
    public class FileLogController : ControllerBase
    {
        private readonly ILogger<FileLogController> _logger;

        private readonly ElasticClient _elasticClient;

        public FileLogController(ILogger<FileLogController> logger, QueryService queryService)
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
        public async Task<string> Post(string index, [FromBody] FileLogData fileLogData)
        {
            var response = await _elasticClient.IndexAsync(fileLogData, idx => idx.Index(index)); //or specify index via settings.DefaultIndex("mytweetindex");
            return response.ToString();
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
