using Lms.Core.Entities;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Repositories;

namespace Lms.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly LmsApiContext db;

        public GameRepository(LmsApiContext db)
        {
            this.db = db;

        }
        public Task<IEnumerable<Game>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Game>> GetAsync()
        {
            return await db.Game.ToListAsync();
        }
        public async Task<Game> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();

        }


        public void Add(Game Game)
        {
            throw new NotImplementedException();
        }

        public void Update(Game Game)
        {
            throw new NotImplementedException();
        }

        public void Remove(Game Game)
        {
            throw new NotImplementedException();
        }
    }
}
