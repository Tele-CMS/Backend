using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientReminderService : IBaseService
    {
        //JsonModel SaveReminder(PatientReminderModel patientReminderModel, TokenModel tokenModel);

        //JsonModel SaveReminder(TaskReminderModel tasksReminderModel, TokenModel tokenModel);

        JsonModel SaveReminder(AlertReminderModel tasksReminderModel, TokenModel tokenModel);
    }
}
