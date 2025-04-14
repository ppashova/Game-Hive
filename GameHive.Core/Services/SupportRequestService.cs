using GameHive.Core.IServices;
using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.Services
{
    public class SupportRequestService : ISupportRequestService
    {
        private readonly ISupportRequestRepository _repo;
        public SupportRequestService(ISupportRequestRepository repo)
        {
            _repo = repo;
        }

        public async Task CreateRequestAsync(SupportRequest request)
        {
            await _repo.AddAsync(request);
        }

        public async Task<List<SupportRequest>> GetAllRequestsAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<SupportRequest> GetRequestById(string id)
        {
            return await _repo.GetByIdAsync(id);
        }
    }
}
