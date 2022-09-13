using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Patient.Data.Configuration.Organization
{
    public class ChatConnectedUserConfig : IEntityTypeConfiguration<ChatConnectedUser>
    {
        public void Configure(EntityTypeBuilder<ChatConnectedUser> builder)
        {   
            builder.Property(x => x.UserId).IsRequired(true);
            builder.HasOne(x=>x.User).WithOne(x => x.ChatConnectedUser).HasForeignKey<ChatConnectedUser>(x => x.UserId);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
