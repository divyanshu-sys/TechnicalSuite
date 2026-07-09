using Microsoft.EntityFrameworkCore;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedData(this ModelBuilder builder)
        {
            builder.Entity<NextUserSetting>().HasData(new NextUserSetting
            {
                Id = "d95ecbe5-b708-4252-86be-2631fc6634b2",
                NextUserNumber = 1
            });

            builder.Entity<NextOrderSetting>().HasData(new NextOrderSetting
            {
                Id = "d95ecbe5-b708-4252-86be-2631fc6634b3",
                NextOrderNumber = 1
            });
        }
    }
}
