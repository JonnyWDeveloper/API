using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<Tournament>> GetAllAsync(bool includeGames = false);
        Task<Tournament> GetAsync(int id);
        Task<Tournament> GetAsync(string title, bool includeGames = false);
        
        Task<bool> AnyAsync(int id);
        void Add(Tournament tournament);
        void Update(Tournament tournament);
        void Remove(Tournament tournament);
    }
}
