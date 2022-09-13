using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IProviderAppointmentRepository
    {
        /// <summary>
        /// To search provider and make appointment based on search criteria
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="locationModel"></param>
        /// <param name="appointmentSearchModel"></param>
        /// <returns></returns>
        List<AppointmentModel> GetProviderListToMakeAppointment(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        //List<AppointmentModel> GetProviderListToMakeAppointmentForRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel, string v1, string v2);
        List<AppointmentModel> GetProviderListToMakeAppointmentLessRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        List<AppointmentModel> GetProviderListToMakeAppointmentMoreRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        List<AppointmentModel> GetProviderListToMakeAppointmentForReviewrating(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel, string rating);
        List<AppointmentModel> GetProviderListToMakeAppointmentForRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel, string payrate,string minrate);
        List<AppointmentModel> SortedProviderAvailableList(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        List<AppointmentModel> GetSpecialitySearchTextProviderListToMakeAppointment(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        List<AppointmentModel> GetSpecialitySearchTextProviderListToMakeAppointmentKeySearch(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        List<AppointmentModel> GetUrgentCareProviderListToMakeAppointment(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        List<AppointmentModel> GetUrgentCareProviderListToMakeAppointmentForRate(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel, string payrate, string minrate);
        List<AppointmentModel> GetSpecialitySearchTextUrgentCareProviderListToMakeAppointmentKeySearch(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
        List<AppointmentModel> SortedUrgentCareProviderAvailableList(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModel appointmentSearchModel);
    }
}
