using Microsoft.Extensions.Logging;
using Nest;
using Payments.ElasticSearch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.ElasticSearch.Services
{
    public class ElasticSearchPaymentService : IPaymentService
    {
        //TODO
        private List<Payment> _cache = new List<Payment>();


        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticSearchPaymentService> _logger;

        public ElasticSearchPaymentService(ILogger<ElasticSearchPaymentService> logger, IElasticClient elasticClient)
        {
            _logger = logger;
            _elasticClient = elasticClient;
        }

        public async Task DeleteAsync(Payment payment)
        {
            await _elasticClient.DeleteAsync<Payment>(payment);
        }

        public Task<Payment> GetPaymentById(int id)
        {
            var payment = _cache
                .Where(p => p.ReleaseDate <= DateTime.UtcNow)
                .FirstOrDefault(p => p.Id == id);

            return Task.FromResult(payment);
        }

        public async Task<IEnumerable<Payment>> GetPayments(int count, int skip = 0)
        {
            var payments = _cache
                  .Where(p => p.ReleaseDate <= DateTime.UtcNow)
                  .Skip(skip)
                  .Take(count);

            if (!payments.Any())
            {
                var response = await _elasticClient.SearchAsync<Payment>(s=>s.Skip(skip).Size(count));

                if (!response.IsValid)
                {
                    _logger.LogError("Failed to search documents");
                    return (IEnumerable<Payment>)Task.FromResult(new Payment[] { });
                }

                return response.Documents;
            }

            return payments;
        }

        public Task SaveSingleAsync(Payment payment)
        {
            if (_cache.Any(p => p.Id == payment.Id))
            {
                return _elasticClient.UpdateAsync<Payment>(payment, u => u.Doc(payment));
            }
            else
            {
                _cache.Add(payment);
                return _elasticClient.IndexDocumentAsync<Payment>(payment);
            }
        }

        public async Task SaveManyAsync(Payment[] payments)
        {
            _cache.AddRange(payments);

            var result = await _elasticClient.IndexManyAsync(payments);
            if (result.Errors)
            {
                foreach (var itemWithError in result.ItemsWithErrors)
                {
                    _logger.LogError("Failed to index document {0}: {1}",
                        itemWithError.Id, itemWithError.Error);
                }
            }
        }
    }
}
