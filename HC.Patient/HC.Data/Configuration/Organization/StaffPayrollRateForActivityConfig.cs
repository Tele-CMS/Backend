using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Data.Configuration.Organization
{
    public class StaffPayrollRateForActivityConfig : IEntityTypeConfiguration<StaffPayrollRateForActivity>
    {
        public void Configure(EntityTypeBuilder<StaffPayrollRateForActivity> builder)
        {
            builder.Property(x => x.StaffId).IsRequired(true);
            builder.Property(x => x.AppointmentTypeId).IsRequired(true);
            builder.Property(x => x.PayRate).IsRequired(true);

            builder.HasOne(x => x.Staffs).WithMany(x => x.StaffPayrollRateForActivity).HasForeignKey(x => x.StaffId);
            builder.HasOne(x => x.AppointmentType).WithMany(x => x.StaffPayrollRateForActivity).HasForeignKey(x => x.AppointmentTypeId);

            builder.Property(x => x.IsDeleted).IsRequired(true).HasColumnType("BIT").HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasColumnType("BIT").HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
