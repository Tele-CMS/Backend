using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IAppointmentPaymentRepository : IRepositoryBase<AppointmentPayments>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appointmentPayment"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        AppointmentPayments SaveUpdatePayment(AppointmentPayments appointmentPayment, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        AppointmentPayments GetAppointmentPaymentsById(int id, TokenModel tokenModel);
        /// <summary>
        /// /
        /// </summary>
        /// <param name="paymentToken"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        AppointmentPayments GetAppointmentPaymentsByPaymentToken(string paymentToken, TokenModel tokenModel);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        IQueryable<T> GetTotalAppointmentRevenue<T>(TokenModel token, int staffId = 0) where T : class, new();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="AppointmentPaymentListingModel"></typeparam>
        /// <param name="paymentFilterModel"></param>
        /// <returns></returns>
        IQueryable<AppointmentPaymentListingModel> GetAppointmentPayments<AppointmentPaymentListingModel>(PaymentFilterModel paymentFilterModel, TokenModel tokenModel) where AppointmentPaymentListingModel : class, new();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="AppointmentRefundListingModel"></typeparam>
        /// <param name="refundFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<AppointmentRefundListingModel> GetAppointmentRefunds<AppointmentRefundListingModel>(RefundFilterModel refundFilterModel, TokenModel tokenModel) where AppointmentRefundListingModel : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="AppointmentPaymentListingModel"></typeparam>
        /// <param name="paymentFilterModel"></param>
        /// <returns></returns>
        IQueryable<AppointmentPaymentListingModel> GetClientAppointmentPayments<AppointmentPaymentListingModel>(PaymentFilterModel paymentFilterModel, TokenModel tokenModel) where AppointmentPaymentListingModel : class, new();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="AppointmentRefundListingModel"></typeparam>
        /// <param name="refundFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        IQueryable<AppointmentRefundListingModel> GetClientAppointmentRefunds<AppointmentRefundListingModel>(RefundFilterModel refundFilterModel, TokenModel tokenModel) where AppointmentRefundListingModel : class, new();

        IQueryable<AppointmentPaymentListingModel> GetAppointmentPaymentsForReport<AppointmentPaymentListingModel>(PaymentFilterModel paymentFilterModel, TokenModel tokenModel) where AppointmentPaymentListingModel : class, new();
        /// <summary>
        ///GetClientNetAppointmentPayment
        /// </summary>
        /// <typeparam name="ClientPaymentgModel"></typeparam>
        /// <param name="paymentFilterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        ClientPaymentgModel GetClientNetAppointmentPayment<ClientPaymentgModel>(int clientId, TokenModel tokenModel) where ClientPaymentgModel : class, new();
        /// <summary>
        /// GetPastUpcomingAppointment
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="staffIds"></param>
        /// <param name="patientIds"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        ClientAppointmentPastUpcomingModel GetPastUpcomingAppointment<ClientAppointmentPastUpcomingModel>(string locationIds, string staffIds, string patientIds, TokenModel tokenModel) where ClientAppointmentPastUpcomingModel : class, new();
    }
}
