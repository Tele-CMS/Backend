using HC.Model;
using HC.Patient.Model.Eligibility;
using HC.Patient.Model.Patient;
using HC.Patient.Model.SymptomChecker;
using HC.Service.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientService : IBaseService
    {
        List<PatientModel> GetPatientsByTags(string tags, string startWith, int? locationID, bool? isActive);
        JsonModel GetPatientByTags(ListingFiltterModel patientFiltterModel,TokenModel token);
        JsonModel GetPatientsDetails(int PatientID,TokenModel token);
        JsonModel GetPatientById(int patientId, TokenModel token);
        JsonModel UpdatePatientPortalVisibility(int patientID, int userID, bool isPortalActive, string url, TokenModel token);
        JsonModel UpdatePatientActiveStatus(int patientID, bool isActive, TokenModel token);
        JsonModel GetActivitiesForPatientPayer(int patientId, DateTime startDate, DateTime endDate, TokenModel token);
        JsonModel GetPatientAuthorizationData(Nullable<int> appointmentId,int patientId,int appointmentTypeId,DateTime startDate,DateTime endDate, bool isAdmin,Nullable<int> patientInsuranceId,Nullable<int> authorizationId, TokenModel token);
        AuthorizationValidityModel CheckAuthorizationForServiceCodes(List<string> serviceCodesList, int patientId, int appointentTypeId, DateTime startDate, string payerPreference);
        JsonModel GetPatientPayerServiceCodes(int patientId,string payerPreference,DateTime date,int payerId, int patientInsuranceId, TokenModel token);
        JsonModel GetPatientPayerServiceCodesAndModifiers(int patientId, string payerPreference, DateTime date, int payerId, int patientInsuranceId, TokenModel token);
        JsonModel GetPatientsDetailsForMobile(int PatientID, TokenModel token);
        JsonModel GetAllAuthorizationsForPatient(int patientId, int pageNumber,int pageSize,string authType, TokenModel token);
        MemoryStream GetPatientCCDA(int patientID, TokenModel token);
        JsonModel GetPatientGuarantor(int patientId, TokenModel token);
        JsonModel ImportPatientCCDA(JObject file, TokenModel token);
        JsonModel CreateUpdatePatient(PatientDemographicsModel patients, TokenModel token);
        JsonModel CreateUpdateClient(PatientDemographicsModel patients, TokenModel token);
        JsonModel GetPatients(ListingFiltterModel patientFiltterModel,TokenModel token);
        JsonModel GetAuthorizationsForPatientPayer(int patientId, int patientInsuranceId, DateTime startDate, TokenModel token);
        JsonModel GetPatientHeaderInfo(int PatientID, TokenModel token);
        JsonModel GetPatientsDashboardDetails(int PatientID, TokenModel token);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>


        JsonModel GetPatientIdByUserId(int userId, TokenModel token);
        JsonModel GetPatientByIdForPushNotificatons(int? patientId, TokenModel token);

        JsonModel GetPatientSymptoms(string search, TokenModel token);
        JsonModel GetSymptomQuestions(List<evidence> symptom, int age, string gender, TokenModel token);

        JsonModel GetObservations(List<evidence> symptom, int age, string gender, TokenModel token);

        JsonModel GetSummary(string type, TokenModel token);

        JsonModel GetCovidQuestions(CovidQuestion quest, TokenModel token);
        JsonModel GetCovidSummary(List<CovidEvidence> symptom, int age,string gender, TokenModel token);
        JsonModel GetPatientsDetailedInfo( int patientId, TokenModel token);

        JsonModel GetRedAlertsInfo(AlertsRedIndicatorFilterModel filterModel, TokenModel tokenModel);
        bool IsValidUserForDataAccess(TokenModel tokenModel, int patientId);
        JsonModel GetPatientDetailsBB(PatientBBAuthorizationModel bBAuthorizationModel, TokenModel token);
        JsonModel CheckBlueButtonStatus(int clientId, TokenModel token);
    }
}
