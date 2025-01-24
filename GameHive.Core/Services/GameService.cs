using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameHive.Core.IServices;
using GameHive.DataAccess.Repository;
using GameHive.Models;

namespace GameHive.Core.Services
{
    internal class GameService : IGameService
    {
        private readonly IRepository<Game> _repo;
        public void Add(Game game)
        {
            this._repo.Add(game);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public List<Game> Find(Expression<Func<Game, bool>> filter)
        {
            return _repo.Find(filter);
        }

        public List<Game> GetAll()
        {
            return _repo.GetAll();
        }

        public Game GetById(int id)
        {
            return _repo.Get(id);
        }

        public void Update(Game game)
        {
            _repo.Update(game);
        }
    }
}
