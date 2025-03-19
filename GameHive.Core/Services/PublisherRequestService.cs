using GameHive.Core.IServices;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.Services
{
    public class PublisherRequestService : IPublisherRequestService
    {
        private readonly IRepository<PublisherRequest> _repository;

        public PublisherRequestService(IRepository<PublisherRequest> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(PublisherRequest pr)
        {
            await _repository.AddAsync(pr);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<PublisherRequest>> FindAsync(Expression<Func<PublisherRequest, bool>> filter)
        {
            return await _repository.FindAsync(filter);
        }

        public async Task<List<PublisherRequest>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PublisherRequest> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<PublisherRequest?> GetRequestByUserIdAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task UpdateAsync(PublisherRequest pr)
        {
            await _repository.UpdateAsync(pr);
        }
    }
}
