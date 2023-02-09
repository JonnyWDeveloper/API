using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data
{
    public class SeedData
    {
        private static LmsApiContext db;

        public static async Task InitAsync(LmsApiContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            db = context;


            if (await db.Game.AnyAsync())
                return;

            var faker = new Faker("sv");
            var tournaments = new List<Tournament>();
            int i;

            for (i = 0; i < 5; i++)
            {
                tournaments.Add(new Tournament
                {
                    Title = faker.Random.Word(),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-20, 20)),
                    Games = new Game[]
                    {
                        new Game
                        {

                            Title = faker.Commerce.ProductName(),
                            Time = DateTime.Now.AddDays(faker.Random.Int(-20, 20)),
                            //TournamentId = i,
                        },
                        new Game
                        {
                            Title = faker.Commerce.ProductName(),
                            Time = DateTime.Now.AddDays(faker.Random.Int(-20, 20)),
                            //TournamentId = i,
                        }
                    }
                });
            }

            db.AddRange(tournaments);
            await db.SaveChangesAsync();
        }

    }
}

