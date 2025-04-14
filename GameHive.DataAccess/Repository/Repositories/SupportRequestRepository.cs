using GameHive.DataAccess.Repository.IRepositories;
using GameHive.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository.Repositories
{
    public class SupportRequestRepository : ISupportRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public SupportRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupportRequest>> GetAllAsync()
        {
            return await _context.SupportRequests.Include(r => r.User).ToListAsync();
        }

        public async Task<SupportRequest> GetByIdAsync(string id)
        {
            return await _context.SupportRequests.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RequestId.ToString() == id);
        }

        public async Task AddAsync(SupportRequest request)
        {
            await _context.SupportRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SupportRequest request)
        {
            _context.SupportRequests.Remove(request);
            await _context.SaveChangesAsync();
        }

    }

}
