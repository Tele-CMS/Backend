using HC.Common;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.Appointment
{
    public class AppointmentAuthorizationRepository : RepositoryBase<AppointmentAuthorization>, IAppointmentAuthorizationRepository
    {
        private HCOrganizationContext _context;
        
        public AppointmentAuthorizationRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;        
        }
    }
}
