using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using GameHive.Models.enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository.Repositories
{
    public class GameRequestRepository : IGameRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRequestAsync(GameRequest request)
        {
            _context.GameRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task AddRequestImagesAsync(int reqId, string imgurl)
        {
            await _context.AddAsync(new RequestImage { RequestId = reqId, ImageUrl = imgurl });
            await _context.SaveChangesAsync();
        }

        public async Task<GameRequest> GetByIdAsync(int requestId)
        {
            return await _context.GameRequests
                .Where(gr => gr.Status == RequestEnums.Pending)
                .Include(gr => gr.Tags) // Include the navigation property
                    .ThenInclude(gt => gt.Tag) // Include the related Tag entity
                .Include(gr => gr.Images) // Just include the entire Images collection
                .FirstOrDefaultAsync(gr => gr.Id == requestId);
        }

        public async Task<List<GameRequest>> GetPendingRequestsAsync()
        {
            return await _context.GameRequests
                .Where(gr => gr.Status == RequestEnums.Pending)
                .Include(gr => gr.Tags) // Include the navigation property
                    .ThenInclude(gt => gt.Tag) // Include the related Tag entity
                .Include(gr => gr.Images) // Just include the entire Images collection
                .ToListAsync();
        }

        public async Task UpdateRequestAsync(GameRequest request)
        {
            _context.GameRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRequestAsync(int requestId)
        {
            var request = await _context.GameRequests.FindAsync(requestId);
            if (request != null)
            {
                _context.GameRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddGameRequestImagesAsync(RequestImage requestImage)
        {
            await _context.RequestImages.AddAsync(requestImage);
            await _context.SaveChangesAsync();
        }

        public async Task AddGameRequestTagsAsync(RequestTag requestTag)
        {
            await _context.AddAsync(requestTag);
            await _context.SaveChangesAsync();
        }
        public async Task<List<RequestImage>> GetRequestImagesByIdAsync(int id)
        {
            return await _context.RequestImages.Where(rq => rq.RequestId == id).ToListAsync();
        }

        public async Task<List<GameRequest>> GetPublisherRequestsById(string PublisherId)
        {
            return await _context.GameRequests.Where(rq => rq.PublisherId == PublisherId).ToListAsync();
        }
    }

}
