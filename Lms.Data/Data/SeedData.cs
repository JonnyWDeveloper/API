using Bogus;
using Lms.Core.Entities;
using Lms.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            int i = 0;

            for (i = 0; i < 50; i++)
            {
                tournaments.Add(new Tournament
                {
                    Title = faker.Name + " " + faker.Random.Word(),
                    StartDate = DateTime.UtcNow.AddDays(faker.Random.Int(-20, 20)),
                    Games = new Game[]
                    {
                        new Game
                        {

                            Title = faker.Commerce.ProductName(),
                            Time = DateTime.UtcNow.AddDays(faker.Random.Int(-20, 20)),
                            //TournamentId = i,
                        },
                        new Game
                        {
                            Title = faker.Commerce.ProductName(),
                            Time = DateTime.UtcNow.AddDays(faker.Random.Int(-20, 20)),
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

