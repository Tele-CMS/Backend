using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Staff
{
    public class UserDriveTimeRepository : RepositoryBase<UserDriveTime>, IUserDriveTimeRepository
    {
        private HCOrganizationContext _context;
        public UserDriveTimeRepository(HCOrganizationContext context):base(context)
        {
            _context = context;
        }
    }
}
