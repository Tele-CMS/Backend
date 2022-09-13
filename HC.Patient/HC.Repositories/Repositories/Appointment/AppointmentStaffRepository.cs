using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Appointment
{
    public class AppointmentStaffRepository : RepositoryBase<AppointmentStaff>, IAppointmentStaffRepository
    {
        private HCOrganizationContext _context;
        public AppointmentStaffRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
    }
}
