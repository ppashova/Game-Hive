using GameHive.Models;
using GameHive.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository.IRepositories
{
    public interface IGameRequestRepository
    {
        Task AddRequestAsync(GameRequest request);
        Task<GameRequest> GetByIdAsync(int requestId);
        Task<List<GameRequest>> GetPendingRequestsAsync();
        Task UpdateRequestAsync(GameRequest request);
        Task DeleteRequestAsync(int requestId);
        Task AddGameRequestImagesAsync(RequestImage requestImage);
        Task AddGameRequestTagsAsync(RequestTag requestTag);
        Task AddRequestImagesAsync(int reqId, string imgurl);
        Task<List<GameRequest>> GetPublisherRequestsById(string PublisherId);
    }
}
