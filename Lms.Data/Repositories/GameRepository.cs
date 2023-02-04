using Lms.Core.Entities;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lms.Core.Repositories
{
    public class GameRepository
    {
        private readonly LmsApiContext db;

        public GameRepository(LmsApiContext db)
        {
            this.db = db;

        }
        // public Task<IEnumerable<Game>> GetAllAsync( return);

        public async Task<IEnumerable<Game>> GetAsync()
        {
            return await db.Game.ToListAsync();
        }
        //public Task<Game> GetAsync(int id) { return }
        //public Task<bool> AnyAsync(int id){ return }
        public void Add(Game Game)
        {
        }
        public void Update(Game Game)
        {
        }
        public void Remove(Game Game)
        {
        }
    }
}
