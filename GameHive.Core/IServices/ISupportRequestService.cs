using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.IServices
{
    public interface ISupportRequestService
    {
        Task<IEnumerable<SupportRequest>> GetAllRequestsAsync();
        Task<SupportRequest> GetRequestById(string id);
        Task CreateRequestAsync(SupportRequest request);
    }
}
