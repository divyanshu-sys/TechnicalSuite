using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class NextUserSettingFluent
    {
        public NextUserSettingFluent(EntityTypeBuilder<NextUserSetting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(250);

            builder.Property(x => x.NextUserNumber).IsRequired(true);

            // Navigation references start
            // Navigation references end
        }
    }
}
