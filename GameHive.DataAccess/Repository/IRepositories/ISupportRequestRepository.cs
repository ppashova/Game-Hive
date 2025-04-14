using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository.IRepositories
{
    public interface ISupportRequestRepository
    {
        Task<List<SupportRequest>> GetAllAsync();
        Task<SupportRequest> GetByIdAsync(string id);
        Task AddAsync(SupportRequest request);
        Task DeleteAsync(SupportRequest request);
    }

}
