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
        Task<IEnumerable<Tournament>> GetAllAsync(bool includegames = false);
        Task<Tournament> GetAsync(string title, bool includegames = false);

        Task AddAsync(Tournament tournament);
    

        //Task<bool> AnyAsync(int id);
        //void Add(Tournament tournament);
        //void Update(Tournament tournament);
        //void Remove(Tournament tournament);
    }
}
