using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;

namespace Lms.Data.Data
{
    public class LmsApiContext : DbContext
    {
        public LmsApiContext(DbContextOptions<LmsApiContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournament => Set<Tournament>();

        public DbSet<Game> Game => Set<Game>();
    }
}
