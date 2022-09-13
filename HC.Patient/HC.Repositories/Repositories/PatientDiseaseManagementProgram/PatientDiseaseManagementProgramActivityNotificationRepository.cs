using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.Repositories.PatientDiseaseManagementProgram
{
    public class PatientDiseaseManagementProgramActivityNotificationRepository : RepositoryBase<DiseaseManagementPlanPatientActivityNotifications>, IPatientDiseaseManagementProgramActivityNotificationRepository
    {
        private HCOrganizationContext _context;
        public PatientDiseaseManagementProgramActivityNotificationRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
