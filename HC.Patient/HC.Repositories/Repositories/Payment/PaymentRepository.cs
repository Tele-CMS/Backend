using HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.Payment;
using HC.Patient.Repositories.IRepositories.Payment;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payment
{
    public class PaymentRepository : RepositoryBase<ClaimServiceLinePaymentDetails>, IPaymentRepository
    {
        private HCOrganizationContext _context;
        
        public PaymentRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;        
        }

        public ClaimsFullDetailModel GetAllClaimsWithServiceLinesForPayer(string payerIds,string patientIds,string tags,string fromDate, string toDate,int locationId,string claimBalanceStatus, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId",token.OrganizationID),
                                          new SqlParameter("@FromDate",fromDate),
                                          new SqlParameter("@ToDate",toDate),
                                          new SqlParameter("@PayerIds",payerIds),
                                          new SqlParameter("@PatientIds",patientIds),
                                          new SqlParameter("@Tags",tags),
                                          new SqlParameter("@LocationId",locationId),
                                          new SqlParameter("@ClaimBalanceStatus",claimBalanceStatus),                                          
            };
            return _context.ExecStoredProcedureForAllClaimDetailsWithServiceLine(SQLObjects.CLM_GetAllClaimsWithServiceLinesForPayer.ToString(), parameters.Length, parameters);
        }

        public IQueryable<T> ApplyPayment<T>(PaymentApplyModel paymentCheckDetailModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PayerId",paymentCheckDetailModel.PayerId),
                                          new SqlParameter("@PaymentTypeId",paymentCheckDetailModel.PaymentTypeId),
                                          new SqlParameter("@DescriptionTypeId",paymentCheckDetailModel.DescriptionTypeId),
                                          new SqlParameter("@PatientId",paymentCheckDetailModel.PatientId),
                                          new SqlParameter("@GuarantorId",paymentCheckDetailModel.GuarantorId),
                                          new SqlParameter("@CustomReferenceNumber",paymentCheckDetailModel.CustomReferenceNumber),
                                          new SqlParameter("@Amount",paymentCheckDetailModel.Amount),
                                          new SqlParameter("@PaymentDate",paymentCheckDetailModel.PaymentDate),
                                          new SqlParameter("@ClaimServiceLine",paymentCheckDetailModel.ClaimServiceLineXml.ToString()),
                                          new SqlParameter("@Claims",paymentCheckDetailModel.ClaimsXml.ToString()),
                                          new SqlParameter("@ClaimServiceLineAdjustment",paymentCheckDetailModel.ClaimServiceLineAdjustment.ToString()),
                                          new SqlParameter("@UserId",token.UserID),
                                          new SqlParameter("@OrganizationId",token.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_ApplyPayments.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> EOBPayment<T>(PaymentApplyModel paymentCheckDetailModel, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PayerId",paymentCheckDetailModel.PayerId),
                                          new SqlParameter("@PaymentTypeId",paymentCheckDetailModel.PaymentTypeId),
                                          new SqlParameter("@DescriptionTypeId",paymentCheckDetailModel.DescriptionTypeId),
                                          new SqlParameter("@CustomReferenceNumber",paymentCheckDetailModel.CustomReferenceNumber),
                                          new SqlParameter("@Amount",paymentCheckDetailModel.Amount),
                                          new SqlParameter("@PaymentDate",paymentCheckDetailModel.PaymentDate),
                                          new SqlParameter("@ClaimServiceLine",paymentCheckDetailModel.ClaimServiceLineXml.ToString()),
                                          new SqlParameter("@Claims",paymentCheckDetailModel.ClaimsXml.ToString()),
                                          new SqlParameter("@ClaimServiceLineAdjustment",paymentCheckDetailModel.ClaimServiceLineAdjustment.ToString()),
                                          new SqlParameter("@UserId",token.UserID),
                                          new SqlParameter("@OrganizationId",token.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_ApplyEOBPayments.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetServiceLinePaymentDetailsForPatientLedger<T>(string serviceLineIds, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ServiceLineIds",serviceLineIds)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetServiceLinePaymentDetailsForPatientLedger.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetServiceLinePaymentDetailsForLedger<T>(string serviceLineIds, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@ServiceLineIds",serviceLineIds)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.CLM_GetServiceLinePaymentDetailsForLedger.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
