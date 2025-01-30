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

        public async Task<List<Tag>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags ?? new List<Tag>();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _tagRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Tag tag)
        {
            await _tagRepository.UpdateAsync(tag);
        }
        public async Task<List<Tag>> GetTagsByGameIdAsync(int id)
        {
            return await _gameTagRepository.GetTagsByGameIdAsync(id);
        }
    }
}
