using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Patient.Data.Configuration.Organization
{
    public class ModifiersConfig : IEntityTypeConfiguration<MasterServiceCodeModifiers>
    {
        public void Configure(EntityTypeBuilder<MasterServiceCodeModifiers> builder)
        {   
            builder.Property(x => x.ServiceCodeID).IsRequired(true);


            builder.HasOne(x => x.MasterServiceCode).WithMany(x => x.MasterServiceCodeModifiers).HasForeignKey(x => x.ServiceCodeID);

            builder.Property(x => x.IsDeleted).IsRequired(true).HasColumnType("BIT").HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasColumnType("BIT").HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
