using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.IServices
{
    public interface ITagService
    {
        Task<Tag> GetByIdAsync(int id);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(int id);
        Task<List<Tag>> GetAllAsync();
        Task<List<Tag>> FindAsync(Expression<Func<Tag, bool>> filter);
    }
}
