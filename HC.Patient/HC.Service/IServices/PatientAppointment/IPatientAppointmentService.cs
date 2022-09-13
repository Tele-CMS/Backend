using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Model.Reports;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.PatientAppointment
{
    public interface IPatientAppointmentService :IBaseService
    {
        List<PatientAppointmentsModel> UpdatePatientAppointment(PatientAppointmentFilter patientAppointmentFilter);
        //List<StaffAvailabilityModel> GetStaffAvailability(string StaffID, DateTime FromDate, DateTime ToDate,TokenModel token);
        JsonModel  GetPatientAppointmentList(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate,string patientTags,string staffTags, TokenModel token);
        JsonModel SaveAppointment(PatientAppointmentModel patientAppointmentModel,List<PatientAppointmentModel> patientAppointmentList, bool isAdmin, TokenModel tokenModel);

        JsonModel DeleteAppointment(int appointmentId, int? parentAppointmentId,bool deleteSeries, bool isAdmin, TokenModel token);

        JsonModel GetAppointmentDetails(int appointmentId,TokenModel token, bool isGetPatientDetails = false);
        JsonModel GetStaffAndPatientByLocation(string locationIds, string permissionKey, string isActiveCheckRequired, TokenModel token);
        JsonModel GetStaffByLocation(string locationIds, string isActiveCheckRequired, TokenModel token);
        List<AvailabilityMessageModel> CheckIsValidAppointment(string staffIds, DateTime startDate, DateTime endDate,Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId,Nullable<int> patientId, Nullable<int> appointmentTypeID, TokenModel token);
        List<AvailabilityMessageModel> CheckIsValidAppointmentWithLocation(string staffIds, DateTime startDate, DateTime endDate, Nullable<DateTime> currentDate, Nullable<int> patientAppointmentId, Nullable<int> patientId, Nullable<int> appointmentTypeID, decimal currentOffset, TokenModel token);
        JsonModel GetDataForSchedulerByPatient(Nullable<int> patientId,int locationId, DateTime startDate, DateTime endDate,Nullable<int> patientInsuranceId,TokenModel token);
        JsonModel UpdateServiceCodeBlockedUnit(int authProcedureCPTLinkId, string action,TokenModel token);
        JsonModel CancelAppointments(int[] appointmentIds, int CancelTypeId, string reson, TokenModel token);
        JsonModel ActivateAppointments(int appointmentId, bool isAdmin, TokenModel token);
        JsonModel UpdateAppointmentStatus(AppointmentStatusModel appointmentStatusModel, TokenModel token);
        JsonModel SaveAppointmentFromPatientPortal(PatientAppointmentModel patientAppointmentModel,TokenModel token);
        JsonModel DeleteAppointment(int appointmentId,TokenModel token);

        JsonModel RescheduleAppointment(RescheduleModel rescheduleModel, TokenModel token);

        LocationModel GetLocationOffsets(int? locationId);
        /// <summary>
        /// To Book new appointment and send email to both client as well as provider on sucessfully booking
        /// </summary>
        /// <param name="patientAppointmentModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        JsonModel BookNewAppointementFromPatientPortal(PatientAppointmentModel patientAppointmentModel, TokenModel token);

        /// <summary>
        /// To get appointment list for dashboard count
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="staffIds"></param>
        /// <param name="patientIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="patientTags"></param>
        /// <param name="staffTags"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        JsonModel GetPatientAppointmentListForDashboard(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate, string patientTags, string staffTags, TokenModel token);
        JsonModel GetPendingAppointmentList(PatientAppointmentFilterModel appointmentFilterModel, TokenModel token);
        JsonModel GetPastAndUpcomingAppointmentsList(int patientId, DateTime dateTime, TokenModel token);
        /// <summary>
        /// To save new appointment into invited staff scheduler
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="appointmentId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        JsonModel SaveAppointmentWhenStaffInvitedForGroupSession(int staffId, int appointmentId, TokenModel token);

        JsonModel UpdateAppointmentFromPatientPortal(PatientAppointmentModel patientAppointmentModel, TokenModel token);
        JsonModel BookNewFreeAppointementFromPatientPortal(PatientAppointmentModel patientAppointmentModel, TokenModel token);
        JsonModel GetLastNewAppintment( int providerId, TokenModel token);
        JsonModel GetPreviousAppintment(int providerId,int clientId, TokenModel token);

        JsonModel BookNewAppointmentFromPaymentPage(PatientAppointmentModel patientAppointmentModel, TokenModel token);
        JsonModel CheckAppointmentTimeExpiry(int appointmentId, TokenModel token);
        JsonModel BookUrgentCareAppointment(PatientAppointmentModel patientAppointmentModel, TokenModel token);
        JsonModel UrgentCareRefundAppointmentFee(int appointmentId, TokenModel token);
        int GetStaffUserId(int appointmentId);
        JsonModel GetLastUrgentCareCallStatus(int providerId, TokenModel token);
        int GetPatientUserId(int appointmentId);
        JsonModel GetLastUrgentCareCallStatusForPatientPortal(int patientuserid, TokenModel token);
        JsonModel GetLastPatientFollowupDetails(int patientuserid,TokenModel token);
        JsonModel GetFilteredAppointmentList(string locationIds, string staffIds, string patientIds, DateTime? fromDate, DateTime? toDate, string patientTags, string staffTags, string appttype, string apptmode, TokenModel token);
        JsonModel GetUrgentCareAppointmentList(PatientAppointmentFilterModel appointmentFilterModel, TokenModel token);
        JsonModel GetFilteredAppointmentsForReporting(AppointmentReportingFilterModel appointmentReportingFilter, TokenModel token);
    }
}
