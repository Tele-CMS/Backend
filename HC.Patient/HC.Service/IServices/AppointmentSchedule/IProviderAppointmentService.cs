using HC.Model;
using HC.Patient.Model;

namespace HC.Patient.Service.IServices
{
    public interface IProviderAppointmentService
    {
        /// <summary>
        /// To get provider list and make appointments
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="appointmentSearchModel"></param>
        /// <returns></returns>
        JsonModel GetProviderListToMakeAppointment(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel);


        /// <summary>
        /// To get the provider profile with staff id
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="id">encrypted value of staff id</param>
        /// <returns></returns>
        JsonModel GetProvider(TokenModel tokenModel, string id);

        JsonModel SortedProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel);
        JsonModel SearchTextProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel);
        JsonModel GetUrgentCareProviderListToMakeAppointment(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel);
        JsonModel SearchTextUrgentCareProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel);
        JsonModel SortedUrgentCareProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel);
    }
}
