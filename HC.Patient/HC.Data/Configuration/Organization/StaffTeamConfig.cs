using HC.Patient.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Data.Configuration.Organization
{
    public class StaffTeamConfig : IEntityTypeConfiguration<StaffTeam>
    {
        public void Configure(EntityTypeBuilder<StaffTeam> builder)
        {
            builder.HasOne(x => x.Staffs).WithMany(x => x.StaffTeam).HasForeignKey(x => x.StaffId);
        }
    }
}