using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories
{
    public class PatientReminderRepository : RepositoryBase<PatientReminder>, IPatientReminderRepository
    {
        private HCOrganizationContext _context;
        public PatientReminderRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public void SavePatientReminderAndPatientIdMapping(List<PatientReminderAndPatientIdMapping> patientReminderAndPatientIdMapping, bool isUpdate)
        {
            if (isUpdate == false)

                _context.PatientReminderAndPatientIdMapping.AddRange(patientReminderAndPatientIdMapping);
            else
                _context.PatientReminderAndPatientIdMapping.UpdateRange(patientReminderAndPatientIdMapping);

            _context.SaveChanges();
        }

        public void SavePatientReminderAndMessageTypeMapping(List<PatientReminderAndMessageTypeMapping> patientReminderAndMessageTypeMapping, bool isUpdate)
        {
            if (isUpdate == false)

                _context.PatientReminderAndMessageTypeMapping.AddRange(patientReminderAndMessageTypeMapping);
            else
                _context.PatientReminderAndMessageTypeMapping.UpdateRange(patientReminderAndMessageTypeMapping);

            _context.SaveChanges();
        }
    }
}
