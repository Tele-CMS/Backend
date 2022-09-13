using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Patient.Data.Configuration.Organization
{
    class InvitationRejectLogConfig : IEntityTypeConfiguration<InvitationRejectLog>
    {
        public void Configure(EntityTypeBuilder<InvitationRejectLog> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.InvitationId).IsRequired(true);
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
            builder.Property(x => x.RejectRemarks).IsRequired(true);
        }
    }
}
