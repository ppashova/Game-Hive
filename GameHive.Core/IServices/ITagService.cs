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
        Tag GetById(int id);
        void Add(Tag tag);
        void Update(Tag tag);
        public void Delete(int id);
        List<Tag> GetAll();
        List<Tag> Find(Expression<Func<Tag, bool>> filter);
    }
}
