namespace Lms.Data.Repositories
{
    using global::Lms.Core.Repositories;
    using global::Lms.Data.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Lms.Core.Repositories
    {
        public class UnitOfWork : IUnitOfWork
        {

            private readonly LmsApiContext db;
            public UnitOfWork(LmsApiContext db)
            {
                this.db = db;
            }
            public ITournamentRepository TournamentRepository
            {
                get;
            }
            public IGameRepository GameRepository
            {
                get;
            }
            public async Task CompleteAsync()
            {
                await db.SaveChangesAsync();
            }
        }
    }

}
