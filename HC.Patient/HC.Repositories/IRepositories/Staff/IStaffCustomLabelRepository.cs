using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Staff
{
    public interface IStaffCustomLabelRepository : IRepositoryBase<StaffCustomLabels>
    {
        Dictionary<string, object> GetStaffCustomLabel(int staffId, TokenModel token);
    }
}
