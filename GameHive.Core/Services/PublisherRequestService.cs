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

        public Task AddAsync(PublisherRequest pr)
        {
            return _repository.AddAsync(pr);
        }

        public Task DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<List<PublisherRequest>> FindAsync(Expression<Func<PublisherRequest, bool>> filter)
        {
            return _repository.FindAsync(filter);
        }

        public Task<List<PublisherRequest>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<PublisherRequest> GetAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task UpdateAsync(PublisherRequest pr)
        {
            return _repository.UpdateAsync(pr);
        }
    }
}
