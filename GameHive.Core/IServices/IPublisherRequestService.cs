using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.IServices
{
    public interface IPublisherRequestService
    {
        Task AddAsync(PublisherRequest pr);
        Task UpdateAsync(PublisherRequest pr);
        Task DeleteAsync(int id);
        Task<PublisherRequest> GetAsync(int id);
        Task<List<PublisherRequest>> GetAllAsync();
        Task<List<PublisherRequest>> FindAsync(Expression<Func<PublisherRequest, bool>> filter);
    }
}
