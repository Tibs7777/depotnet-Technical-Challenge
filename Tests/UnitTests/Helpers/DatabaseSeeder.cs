using Entities.Models;
using Repository;

namespace Tests.UnitTests.Helpers
{
    internal class DatabaseSeeder
    {
        public static void SeedDatabase(RepositoryContext context)
        {
            context.Database.EnsureCreated();

            var orders = new List<Order>()
            {
                new Order() { OrderId = 1, },
                new Order() { OrderId = 2, }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
