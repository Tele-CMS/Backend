using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Patient.Data.Configuration.Organization
{
    public class PayerModifiersConfig : IEntityTypeConfiguration<PayerServiceCodeModifiers>
    {
        public void Configure(EntityTypeBuilder<PayerServiceCodeModifiers> builder)
        {   
            builder.Property(x => x.PayerServiceCodeId).IsRequired(true);


            builder.HasOne(x => x.PayerServiceCodes).WithMany(x => x.PayerServiceCodeModifiers).HasForeignKey(x => x.PayerServiceCodeId);

            builder.Property(x => x.IsDeleted).IsRequired(true).HasColumnType("BIT").HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasColumnType("BIT").HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
