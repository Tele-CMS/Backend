using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Patient.Data.Configuration.Organization
{
    class StaffExperienceConfiguration : IEntityTypeConfiguration<StaffExperience>
    {
        public void Configure(EntityTypeBuilder<StaffExperience> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.StaffId).IsRequired(true);
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
            builder.Property(x => x.OrganizationName).IsRequired(true);
        }
    }
    class StaffAwardConfiguration : IEntityTypeConfiguration<StaffAward>
    {
        public void Configure(EntityTypeBuilder<StaffAward> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.StaffId).IsRequired(true);
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
            builder.Property(x => x.AwardType).IsRequired(true);
        }

    }
    class StaffQualificationConfiguration : IEntityTypeConfiguration<StaffQualification>
    {
        public void Configure(EntityTypeBuilder<StaffQualification> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.StaffId).IsRequired(true);
            builder.Property(x => x.IsDeleted).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.IsActive).IsRequired(true).HasDefaultValue(true);
            builder.Property(x => x.Course).IsRequired(true);
            builder.Property(x => x.University).IsRequired(true);
        }
    }
}
