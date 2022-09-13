using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientReminderRepository: IRepositoryBase<PatientReminder>
    {
        void SavePatientReminderAndPatientIdMapping(List<PatientReminderAndPatientIdMapping> patientReminderAndPatientIdMapping, bool isUpdate);
        void SavePatientReminderAndMessageTypeMapping(List<PatientReminderAndMessageTypeMapping> patientReminderAndMessageTypeMapping, bool isUpdate);
    }
}
