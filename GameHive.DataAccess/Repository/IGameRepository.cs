﻿using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Repository
{
    public interface IGameRepository
    {
        Task AddAsync(Game game);
        Task DeleteAsync(int id);
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(int id);
        Task UpdateAsync(Game game);
    }
}
