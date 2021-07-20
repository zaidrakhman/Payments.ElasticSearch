using Payments.ElasticSearch.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payments.ElasticSearch.Services
{
    public interface IPaymentService
    {
        Task<Payment> GetPaymentById(int id);
        Task SaveSingleAsync(Payment payment);
        Task DeleteAsync(Payment payment);
        Task<IEnumerable<Payment>> GetPayments(int count, int skip = 0);
        Task SaveManyAsync(Payment[] payments);
    }
}
