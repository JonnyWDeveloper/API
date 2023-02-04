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
    public class TournamentRepository : ITournamentRepository
    {
        private readonly LmsApiContext db;

        public TournamentRepository(LmsApiContext db)
        {
            this.db = db;

        }
        public Task<IEnumerable<Tournament>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tournament>> GetAsync(bool includeGames = false)
        {
            return includeGames ? await db.Tournament.Include(g => g.Games)
                                                       .ToListAsync() :
                                     await db.Tournament.ToListAsync();
        }
        public Task<Tournament> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }
        public void Add(Tournament tournament)
        {
            throw new NotImplementedException();
        }
        public void Update(Tournament tournament)
        {
            throw new NotImplementedException();
        }
        public void Remove(Tournament tournament)
        {
            throw new NotImplementedException();
        }
    }
}
