using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.EDI
{
    public class EDI270Repository : RepositoryBase<EligibilityEnquiry270Master>, IEDI270Repository
    {
        private HCOrganizationContext _context;
        public EDI270Repository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<T> SaveEligibilityEnquiryRequestData<T>(int patientId, int patientInsuranceId, string serviceTypeCodeIds, string serviceCodeIds, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@PatientInsuranceId", patientInsuranceId),
                                          new SqlParameter("@ServiceTypeCodeIds", serviceTypeCodeIds),
                                          new SqlParameter("@ServiceCodeIds", serviceCodeIds),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@OrganizationId", token.OrganizationID)

            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_SaveEligibilityEnquiryRequestData, parameters.Length, parameters).AsQueryable();
        }

        public EDI270FileModel GetEDI270RequestInfo(int patientId, int patientInsuranceId, int eligibilityEnquiry270MasterId, TokenModel token)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientId),
                                          new SqlParameter("@PatientInsuranceId", patientInsuranceId),
                                          new SqlParameter("@EligibilityEnquiry270MasterId", eligibilityEnquiry270MasterId),
                                          new SqlParameter("@OrganizationId", token.OrganizationID),
                                          new SqlParameter("@LocationId", token.LocationID)

            };
            return _context.ExecStoredProcedureForEDI270Info(SQLObjects.PAT_GetEDI270RequestInfo, parameters.Length, parameters);
        }

        public IQueryable<T> UpdateEligibilityRequest<T>(int eligibilityInquiry270MasterId, string ediText, string action, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@EligibilityEnquiry270MasterId", eligibilityInquiry270MasterId),
                                          new SqlParameter("@UserId", token.UserID),
                                          new SqlParameter("@EDIText", ediText),
                                          new SqlParameter("@Action", action)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_UpdateEligibilityRequest, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetEligibilityEnquiryServiceCodes<T>(TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", token.OrganizationID)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetEligibilityEnquiryServiceCodes.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
