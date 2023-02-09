using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lms.Core.Repositories
{
    public interface IGameRepository
    {

        Task<IEnumerable<Game>> GetAllAsync(string title);  
        Task<Game?> GetAsync(string title, int id);
        Task AddAsync(Game game);
    

        //Task<bool> AnyAsync(int id); 
        //void Remove(Game Game);
        //Task AddAsync(Game game);
        //void Add(Game Game);
        //void Update(Game Game);
    }
}
