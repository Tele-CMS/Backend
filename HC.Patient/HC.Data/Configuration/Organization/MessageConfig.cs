using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Data.Configuration.Organization
{
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(c => c.MessageID);
            builder.Property(x => x.MessageID).HasColumnName("Id");
            builder.Property(x => x.MessageDate).IsRequired(true);
            builder.Property(x => x.Subject).IsRequired(true).HasColumnType("NVARCHAR(1500)");
            builder.Property(x => x.Text).IsRequired(true);
            builder.Property(x => x.FromUserID).IsRequired(true);
            builder.Property(x => x.IsFavourite).IsRequired(true).HasDefaultValue(false);
            builder.Property(x => x.OrganizationId).IsRequired(true);

            builder.HasOne(x => x.User).WithMany(x => x.Messages).HasForeignKey(x => x.FromUserID);
            builder.HasOne(x => x.Organization).WithMany(x => x.Messages).HasForeignKey(x => x.OrganizationId);

            builder.Ignore(x => x.IsActive);
            builder.Property(x => x.IsDeleted).IsRequired(true).HasColumnType("BIT").HasDefaultValue(false);
            builder.Property(x => x.CreatedBy).IsRequired(true);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");

        }
    }
}