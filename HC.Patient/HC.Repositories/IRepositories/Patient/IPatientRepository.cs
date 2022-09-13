using CreateClinicalReport.Model;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Repositories.IRepositories.Patient
{
    public interface IPatientRepository : IRepositoryBase<Patients>
    {
        IQueryable<T> GetPatientsByTags<T>(string tags, string startWith, int? locationID, bool? isActive) where T : class, new();
        IQueryable<T> GetPatientDiagnosisCodes<T>(int patientId, FilterModel filterModel) where T : class, new();
        PatientInfoDetails GetPatientsDetails(int PatientID, TokenModel token);
        IQueryable<T> GetPatientGuarantor<T>(int patientId, TokenModel token) where T : class, new();
        IQueryable<T> GetActivitiesForPatientPayer<T>(Nullable<int> patientId, string preference, DateTime startDate, DateTime endDate, Nullable<int> patientInsuranceId, TokenModel token) where T : class, new();
        Dictionary<string, object> GetPatientAuthorizationData(int patientId, int appointmentTypeId, DateTime startDate, string payerPreference);
        IQueryable<T> GetPatientAddressList<T>(Nullable<int> patientId, int locationId) where T : class, new();

        IQueryable<T> GetPatientPayerServiceCodes<T>(int patientId, string payerPreference, DateTime date, int payerId, int patientInsuranceId) where T : class, new();

        Dictionary<string, object> GetPatientPayerServiceCodesAndModifiers(int patientId, string payerPreference, DateTime? date, int payerId, int patientInsuranceId);

        IQueryable<T> GetAuthDataForPatientAppointment<T>(int patientId, int appointmentTypeId, DateTime startDate, DateTime endDate, string payerPreference, Nullable<int> patientAppointmentId, bool IsAdmin, Nullable<int> patientInsuranceId, Nullable<int> authorizationId) where T : class, new();

        IQueryable<T> CheckServiceCodesAuthorizationForPatient<T>(int patientId, string payerPreference, string serviceCodes, DateTime date) where T : class, new();
        bool CheckAuthorizationSetting();
        Patients AddPatient(Patients patients);
        IQueryable<T> GetPatients<T>(ListingFiltterModel patientFiltterModel, TokenModel token) where T : class, new();

        MemoryStream GetPatientCCDA(int PatientID, TokenModel token);
        //int ImportPatientCCDA(string base64File, int organizationID, int userID);
        int ImportPatientCCDA(PatientClinicalInformation patientClinicalInformation, int organizationID, int userID);
        IQueryable<T> GetAuthorizationsForPatientPayer<T>(int patientId, int patientInsuranceId, DateTime startDate, TokenModel token) where T : class, new();
        IQueryable<T> GetEncryptedPHIData<T>(string firstName, string middleName, string lastName, string dob, string emailAddress, string ssn, string mrn, string aptnumber, string address1, string address2, string city, string zipCode, string phonenumber, string healthPlanBeneficiaryNumber) where T : class, new();
        IQueryable<T> GetDecryptedPHIData<T>(byte[] firstName, byte[] middleName, byte[] lastName, byte[] dob, byte[] emailAddress, byte[] ssn, byte[] mrn, byte[] aptnumber, byte[] address1, byte[] address2, byte[] city, byte[] zipCode, byte[] phonenumber, byte[] healthPlanBeneficiaryNumber) where T : class, new();
        IQueryable<T> CheckExistingPatient<T>(string email, string mrn, string userName, int patientId, TokenModel token) where T : class, new();
        IQueryable<T> EncryptMultipleValues<T>(string data) where T : class, new();
        PatientHeaderModel GetPatientHeaderInfo(int PatientID, TokenModel token);
        int GetPatientByUserId(int UserId);
        T GetPatientDetailsForStripe<T>(int patientId) where T : class, new();
        bool UpdateCustomerId(int patientId, string customerId);
        PatientInfo GetPatientsDetailedInfo(int PatientID, TokenModel token);
        IQueryable<T> GetStaffPatients<T>(ListingFiltterModel patientFiltterModel, TokenModel token) where T : class, new();
        PatientInfoDetails GetPatientsDashboardDetails(int PatientID, TokenModel token);
        IQueryable<T> GetRedAlertsInfo<T>(AlertsRedIndicatorFilterModel filterModel, TokenModel token) where T : class, new();
        IQueryable<T> UpdatePatientBBData<T>(string dOB, string genderName, string identifier, int patientId, string firstName, string lastName, string middleName, string address, string city, string zip, string stateName, string phoneNumber, string phoneNumberType, TokenModel token) where T : class, new();
    }
}