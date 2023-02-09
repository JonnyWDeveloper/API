using Lms.Core.Entities;
using Lms.Data.Data;
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
        public async Task<IEnumerable<Tournament>> GetAllAsync(bool includegames)
        {
            //true with Include - not working...
            return includegames ? await db.Tournament.Include(t => t.Games).ToListAsync() :
                                  await db.Tournament.ToListAsync();
        }
        //GetAllAsync is used instead of this method    
        public async Task<IEnumerable<Tournament>> GetAsync(bool includegames = false)
        {

            throw new NotImplementedException();
        }

        public async Task<Tournament> GetAsync(int id)
        {

            var tournament = await db.Tournament.FindAsync(id);

            return tournament;

        }

        public async Task<Tournament?> GetAsync(string title, bool includegames = false)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace", nameof(title));
            }

            var query = db.Tournament
                    .AsQueryable();

            if (includegames)
            {
                query = query.Include(t => t.Games);
            }

            return await query.FirstOrDefaultAsync(t => t.Title == title);

        }


        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(Tournament tournament)
        {
            if (tournament is null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            await db.AddAsync(tournament);
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
