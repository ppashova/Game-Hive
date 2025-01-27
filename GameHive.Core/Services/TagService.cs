using GameHive.Core.IServices;
using GameHive.DataAccess.Repository;
using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _repo;

        public TagService(IRepository<Tag> repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(Tag tag) 
        {
            await _repo.AddAsync(tag); 
        }

        public async Task DeleteAsync(int id) 
        {
            await _repo.DeleteAsync(id); 
        }

        public async Task<List<Tag>> FindAsync(Expression<Func<Tag, bool>> filter) 
        {
            return await _repo.FindAsync(filter); 
        }

        public async Task<List<Tag>> GetAllAsync() 
        {
            return await _repo.GetAllAsync(); 
        }

        public async Task<Tag> GetByIdAsync(int id) 
        {
            return await _repo.GetAsync(id); 
        }

        public async Task UpdateAsync(Tag tag) 
        {
            await _repo.UpdateAsync(tag); 
        }
    }
}
