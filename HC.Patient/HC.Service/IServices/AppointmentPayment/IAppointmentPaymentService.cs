using HC.Model;
using HC.Patient.Model;
using HC.Service.Interfaces;
using System.IO;

namespace HC.Patient.Service.IServices
{
    public interface IAppointmentPaymentService : IBaseService
    {
        /// <summary>
        /// To Save Appointment Payments
        /// </summary>
        /// <param name="appointmentPaymentModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveUpdateAppointmentPayment(AppointmentPaymentModel appointmentPaymentModel, TokenModel tokenModel);
        /// <summary>
        /// To Get Appointment Payments
        /// </summary>
        /// <param name="paymentFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetAppointmentPaymentList(PaymentFilterModel paymentFilterModel, TokenModel tokenModel);
        /// <summary>
        /// To get all refunds with respect to appointment cancelled
        /// </summary>
        /// <param name="refundFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetAppointmentRefundList(RefundFilterModel refundFilterModel, TokenModel tokenModel);

        /// <summary>
        /// To Get Appointment Payments
        /// </summary>
        /// <param name="paymentFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetClientAppointmentPaymentList(PaymentFilterModel paymentFilterModel, TokenModel tokenModel);
        /// <summary>
        /// To get all refunds with respect to appointment cancelled
        /// </summary>
        /// <param name="refundFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetClientAppointmentRefundList(RefundFilterModel refundFilterModel, TokenModel tokenModel);
        MemoryStream GetPaymentPdf(PaymentFilterModel paymentFilterModel, TokenModel tokenModel);
        /// <summary>
        /// GetClientNetAppointmentPayment
        /// </summary>
        /// <param name="paymentFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetClientNetAppointmentPayment(int clientId, TokenModel tokenModel);
        JsonModel GetPastUpcomingAppointment(string locationIds, string staffIds, string patientIds, TokenModel tokenModel);


    }
}
