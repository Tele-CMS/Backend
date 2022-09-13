using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Data.Configuration.Organization
{
    public class MessageRecepientConfig : IEntityTypeConfiguration<MessageRecepient>
    {
        public void Configure(EntityTypeBuilder<MessageRecepient> builder)
        {
            builder.HasKey(c => c.MessageRecepientID);
            builder.Property(x => x.MessageRecepientID).HasColumnName("Id");
            builder.Property(x => x.MessageDate).IsRequired(true);
            builder.Property(x => x.Unread).IsRequired(true).HasDefaultValue(true);
            builder.Property(x => x.MessageId).IsRequired(true);
            builder.Property(x => x.ToUserID).IsRequired(true);
            builder.Property(x => x.IsFavourite).IsRequired(true).HasDefaultValue(false);

            builder.HasOne(x => x.Message).WithMany(x => x.MessageRecepients).HasForeignKey(x => x.MessageId);
            builder.HasOne(x => x.User).WithMany(x => x.MessageRecepients).HasForeignKey(x => x.ToUserID);

            builder.Ignore(x => x.IsActive);
            builder.Property(x => x.IsDeleted).IsRequired(true).HasColumnType("BIT").HasDefaultValue(false);
            builder.Property(x => x.CreatedBy).IsRequired(true);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");

        }
    }
}
