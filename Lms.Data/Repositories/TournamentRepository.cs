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
    public class TournamentRepository
    {
        private readonly LmsApiContext db;

        public TournamentRepository(LmsApiContext db)
        {
            this.db = db;

        }
       // public Task<IEnumerable<Tournament>> GetAllAsync( return);

        public async Task<IEnumerable<Tournament>> GetAsync(bool includeGames = false)
        {
            return includeGames ? await db.Tournament.Include(g => g.Games)
                                                       .ToListAsync() :
                                     await db.Tournament.ToListAsync();
        }
        //public Task<Tournament> GetAsync(int id) { return }
        //public Task<bool> AnyAsync(int id){        return }
        public void Add(Tournament tournament){ }
        public void Update(Tournament tournament){ }
        public void Remove(Tournament tournament){ }
    }
}
