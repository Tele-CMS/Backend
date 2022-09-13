using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HC.Patient.Data.Configuration.Organization
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notifications>
    {
        public void Configure(EntityTypeBuilder<Notifications> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
        }
    }
    public class NotificationTypeMappingConfiguration : IEntityTypeConfiguration<NotificationTypeMapping>
    {
        public void Configure(EntityTypeBuilder<NotificationTypeMapping> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
        }
    }
    public class MasterNotificationTypeConfiguration : IEntityTypeConfiguration<MasterNotificationType>
    {
        public void Configure(EntityTypeBuilder<MasterNotificationType> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
        }
    }
    public class MasterNotificationActionTypeConfiguration : IEntityTypeConfiguration<MasterNotificationActionType>
    {
        public void Configure(EntityTypeBuilder<MasterNotificationActionType> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
        }
    }
}
