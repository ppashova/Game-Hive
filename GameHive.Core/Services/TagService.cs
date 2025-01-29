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
        private readonly ITagRepository _tagRepository;
        private readonly IGameTagRepository _gameTagRepository;
        public TagService(ITagRepository tagRepository, IGameTagRepository gameTagRepository)
        {
            _tagRepository = tagRepository;
            _gameTagRepository = gameTagRepository;
        }
        public async Task AddAsync(Tag tag)
        {
            await _tagRepository.AddAsync(tag);
        }

        public async Task DeleteAsync(int id)
        {
            await _gameTagRepository.DeleteByTagIdAsync(id);
            await _tagRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public Task<Tag> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
}
