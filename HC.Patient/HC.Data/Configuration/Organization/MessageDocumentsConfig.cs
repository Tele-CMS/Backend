using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Data.Configuration.Organization
{
    public class MessageDocumentsConfig : IEntityTypeConfiguration<MessageDocuments>
    {
        public void Configure(EntityTypeBuilder<MessageDocuments> builder)
        {
            builder.HasKey(c => c.MessageDocumentID);
            builder.Property(x => x.MessageDocumentID).HasColumnName("Id");
            builder.Property(x => x.MessageID).IsRequired(true);
            builder.Property(x => x.Name).IsRequired(true).HasColumnType("VARCHAR(200)"); ;
            builder.HasOne(x => x.Message).WithMany(x => x.MessageDocuments).HasForeignKey(x => x.MessageID);
        }
    }
}
