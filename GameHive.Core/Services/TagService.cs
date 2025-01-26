using GameHive.Core.IServices;
using GameHive.DataAccess.Repository;
using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _repo;
        public void Add(Tag tag)
        {
            this._repo.Add(tag);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public List<Tag> Find(Expression<Func<Tag, bool>> filter)
        {
            return _repo.Find(filter);
        }

        public List<Tag> GetAll()
        {
            return _repo.GetAll();
        }

        public Tag GetById(int id)
        {
            return _repo.Get(id);
        }

        public void Update(Tag tag)
        {
            _repo.Update(tag);
        }
    }
}
