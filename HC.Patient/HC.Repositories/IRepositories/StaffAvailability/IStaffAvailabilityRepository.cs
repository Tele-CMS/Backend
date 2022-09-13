using HC.Model;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.StaffAvailability
{
    public interface IStaffAvailabilityRepository : IRepositoryBase<Entity.StaffAvailability>
    {
        IQueryable<T> GetAllDatesForLeaveDateRange<T>(string staffIds,DateTime? startDate,DateTime? endDate) where T : class, new();
    }
}
