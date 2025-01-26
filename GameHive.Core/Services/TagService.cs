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
    internal class TagService : ITagService


        private readonly IRepository<Tag> _repo;
        public void Add(Tag tag)
        {
            
        }

        public void Delete(Tag tag)
        {
            throw new NotImplementedException();
        }

        public List<Tag> Find(Expression<Func<Game, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Tag> GetAll()
        {
            throw new NotImplementedException();
        }

        public Game GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
