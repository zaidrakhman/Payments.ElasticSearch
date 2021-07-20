using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using Payments.ElasticSearch.Entities;
using Payments.ElasticSearch.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.ElasticSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IElasticClient elasticClient, IPaymentService paymentService, ILogger<SearchController> logger)
        {
            _elasticClient = elasticClient;
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find(string query, int page = 1, int pageSize = 5) 
        {
            var response = await _elasticClient.SearchAsync<Payment>(
             s => s.Query(q => q.QueryString(d => d.Query('*' + query + '*')))
                 .From((page - 1) * pageSize)
                 .Size(pageSize));

            if (!response.IsValid)
            {
                _logger.LogError("Failed to search documents");
                return Ok(new Payment[] { });
            }

            return Ok(response.Documents);
        }

        [HttpGet("reindex")]
        public async Task<IActionResult> ReIndex() 
        {
            await _elasticClient.DeleteByQueryAsync<Payment>(q => q.MatchAll());

            var payments = (await _paymentService.GetPayments(int.MaxValue)).ToArray();

            foreach (var item in payments)
            {
                await _elasticClient.IndexDocumentAsync(item);
            }

            return Ok($"{payments.Length} product(s) reindexed");
        }
    }
}
