using HC.Model;
using HC.Patient.Model.Payment;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Payment
{
    public interface IPaymentService : IBaseService
    {
        JsonModel GetAllClaimsWithServiceLinesForPayer(string payerIds, string patientIds, string tags, string fromDate, string toDate,int locationId, string claimBalanceStatus, TokenModel token);
        JsonModel ApplyPayment(PaymentApplyModel paymentCheckDetailModel, TokenModel token);
        JsonModel EOBPayement(PaymentApplyModel paymentCheckDetailModel, TokenModel token);
        JsonModel SaveServiceLinePayment(PaymentModel payment, TokenModel token);
        JsonModel DeleteServiceLinePayment(int paymentDetailId, TokenModel token);
        JsonModel GetPaymentDetailsById(int paymentDetailId, TokenModel token);
        JsonModel CreateCard(string cardmodel, TokenModel token);
        JsonModel ListAllCards(TokenModel token);
        JsonModel DeleteCard(string cardId, TokenModel token);
        JsonModel SetDefaultCard(string cardId, TokenModel token);
        JsonModel CheckDefaultCard(TokenModel token);
        ExpiryCardNotification CardDetailsForNotification(TokenModel token);
        JsonModel UpdateProvidersFeesAndRefundsSettings(ManageFeesRefundsModel model, TokenModel token);
        JsonModel GetProvidersFeesAndRefundsSettings(List<int> providerIds, TokenModel token);
        JsonModel SaveUpdateProviderFeesforMobile(ProviderFeesModel providerfeemodel, TokenModel token);
    }
}
