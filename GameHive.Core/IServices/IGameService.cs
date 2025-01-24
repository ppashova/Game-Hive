using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameHive.Models;

namespace GameHive.Core.IServices
{
    public interface IGameService
    {
        Game GetById(int id);
        void Add(Game game);
        void Update(Game game);
        public void Delete(int id);
        List<Game> GetAll();
        List<Game> Find(Expression<Func<Game, bool>> filter);

    }
}
