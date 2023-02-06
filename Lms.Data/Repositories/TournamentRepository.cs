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
        public async Task<IEnumerable<Tournament>> GetAllAsync(bool includeGames)
        {
            //true with Include - not working...
            return includeGames ? await db.Tournament.Include(t => t.Games).ToListAsync() :
                                  await db.Tournament.ToListAsync();
        }
        //GetAllAsync is used instead of this method    
        public async Task<IEnumerable<Tournament>> GetAsync(bool includeGames = false)
        {

            throw new NotImplementedException();
        }

        public async Task<Tournament?> GetAsync(string title, bool includeGames = false)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace", nameof(title));
            }

            var query = db.Tournament
                    .AsQueryable();

            if (includeGames)
            {
                query = query.Include(t => t.Games);
            }

            return await query.FirstOrDefaultAsync(t => t.Title == title);

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
