using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientAlertService : IBaseService
    {
        JsonModel GetPatientAlerts(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel);
        bool SavePatientAlerts(string alertDetails, int patientId, int masterAlertTypeId, int? documentId, TokenModel tokenModel);
        //SignalRNotificationForBulkMessageModel SendBulkMessagePatientAlerts(PatientAlertFilterModel patientFilterModel, TokenModel tokenModel);
        // SignalRNotificationForBulkEmailModel SendBulkEmailPatientAlerts(PatientAlertFilterModel filterModel, TokenModel tokenModel);
    }
}
