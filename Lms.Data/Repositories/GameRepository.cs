using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly LmsApiContext db;

        public GameRepository(LmsApiContext db)
        {
            this.db = db;

        }
        //Get ALL Games by Title
        public async Task<IEnumerable<Game>> GetAllAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            }

            return await db.Game.Where(g => g.Tournament.Title == title).ToListAsync();
        }


        //Get a SINGLE Game by Tournament.Title and Game.Id
        public async Task<Game?> GetAsync(string title, int id)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            }

            return await db.Game.Where(g => g.Tournament.Title == title)
                                  .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Game game)
        {
            if (game is null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            await db.Game.AddAsync(game);
        }

        //public async Task<bool> AnyAsync(int id)
        //{
        //    return await db.Game.AnyAsync(g => g.Id == id);

        //}

        //public void Add(Game Game)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update(Game Game)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Remove(Game Game)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
