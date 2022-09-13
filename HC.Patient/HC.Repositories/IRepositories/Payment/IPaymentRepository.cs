using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.Payment;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Payment
{
    public interface IPaymentRepository : IRepositoryBase<ClaimServiceLinePaymentDetails>
    {
        IQueryable<T> ApplyPayment<T>(PaymentApplyModel paymentCheckDetailModel, TokenModel token) where T : class, new();
        IQueryable<T> EOBPayment<T>(PaymentApplyModel paymentCheckDetailModel, TokenModel token) where T : class, new();
        ClaimsFullDetailModel GetAllClaimsWithServiceLinesForPayer(string payerIds, string patientIds, string tags, string fromDate, string toDate,int locationId,string claimBalanceStatus, TokenModel token);
        IQueryable<T> GetServiceLinePaymentDetailsForPatientLedger<T>(string serviceLineIds, TokenModel token) where T : class, new();
        IQueryable<T> GetServiceLinePaymentDetailsForLedger<T>(string serviceLineIds, TokenModel token) where T : class, new();
    }
}
