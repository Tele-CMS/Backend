using HC.Model;
using HC.Patient.Model.Eligibility;
using HC.Patient.Model.PatientEncounters;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HC.Patient.Service.IServices.PatientEncounters
{
    public interface IPatientEncounterService : IBaseService
    {
        JsonModel GetPatientEncounterDetails(int appointmentId,int encounterId,bool isAdmin,TokenModel token);
        JsonModel GetPatientNonBillableEncounterDetails(int appointmentId,int encounterId,bool isAdmin,TokenModel token);
        JsonModel SavePatientEncounter(PatientEncounterModel patientEncounter,bool isAdmin, TokenModel token);
        JsonModel SavePatientNonBillableEncounter(PatientEncounterModel patientEncounter, bool isAdmin, TokenModel token);
        JsonModel TrackEncounterAddUpdateClicks(EncounterClickLogsModel encounterClickLogsModel, TokenModel tokenModel);
        List<PatientEncounterModel> GetPatientEncounter(int? patientID, string appointmentType = "", string staffName = "", string status = "", string fromDate = "", string toDate = "", int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "", TokenModel token= null);

        List<PatientEncounterModel> GetAllPatientEncounter(EncounterFilterModel filtermodel, TokenModel token);
        JsonModel SavePatientSignForPatientEncounter(int patientEncounterId, PatientEncounterModel patientEncounterModel);
        MemoryStream DownloadEncounter(int encounterId, TokenModel token);
        JsonModel SaveEncounterSignature(EncounterSignatureModel encounterSignatureModel);
        JsonModel SavePatientEncounterTemplateData(PatientEncounterTemplateModel patientEncounterTemplateModel, TokenModel token);
        JsonModel GetPatientEncounterTemplateData(int patientEncounterId, int masterTemplateId, TokenModel tokenModel);
        JsonModel DeleteEncounter(int encounterId,TokenModel token);
        MemoryStream PrintEncounterSummaryDetails(int encounterId,string checkListIds,string portalkey, TokenModel token); 
        // JsonModel EmailEncounterSummary(int encounterId, string checkListIds, TokenModel token);
        JsonModel GetEncounterSummaryDetailsForPDF(int encounterId, TokenModel tokenModel);
        JsonModel DiscardEncounterChanges(int encounterId, TokenModel token);
        //SignalRNotificationForBulkMessageModel SendBulkMessageForEncounters(EncounterFilterModel filterModel, TokenModel tokenModel);
    //    SignalRNotificationForBulkEmailModel SendBulkEmailForEncounters(EncounterFilterModel filterModel, TokenModel tokenModel);
      //  MemoryStream ExportEncounters(EncounterFilterModel filterModel, TokenModel tokenModel);
     //   MemoryStream PrintEncountersPDF(EncounterFilterModel filterModel, TokenModel tokenModel);
        JsonModel SavePatientEncounterNotes(PatientEncounterNotesModel patientEncounterNotes, TokenModel token);
        JsonModel GetPatientDiagnosisCodes(int patientId, FilterModel filterModel, TokenModel tokenModel);
        JsonModel SavePatientEncounterSOAP(PatientEncounterModel requestObj, bool isAdmin, TokenModel token);
        JsonModel CheckPatientEligibility(PatientEligibilityRequestModel eligibilityRequestModel, TokenModel tokenModel);
        JsonModel SendBBIntructionsMail(BlueButtonModel blueButtonModel, TokenModel token);

    }
}
