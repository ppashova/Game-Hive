using GameHive.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.IServices
{
    public interface IGameRequestService
    {
        Task AddGameRequestAsync(GameRequest game, IFormFile imageFile, List<int> tagIds,List<IFormFile> Images, IFormFile gameHeader, string publisherId);
        Task<List<GameRequest>> GetPendingRequestsAsync();
        Task<bool> ApproveGameRequestAsync(int requestId);
        Task RejectGameRequestAsync(int requestId);
        Task<GameRequest> GetGameRequestByIdAsync(int id);
        Task<List<GameRequest>> GetPublisherRequestsById(string PublisherId);
        Task AddExistingImageToRequestAsync(int requestId, string imageUrl);
    }

}
