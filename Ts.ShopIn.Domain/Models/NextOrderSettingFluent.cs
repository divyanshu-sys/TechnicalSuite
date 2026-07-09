using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Ts.ShopIn.Domain.Models
{
    public class NextOrderSettingFluent
    {
        public NextOrderSettingFluent(EntityTypeBuilder<NextOrderSetting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("varchar").HasMaxLength(250);

            builder.Property(x => x.NextOrderNumber).IsRequired(true);

            // Navigation references start
            // Navigation references end
        }
    }
}
