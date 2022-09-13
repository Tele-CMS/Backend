using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.Patient;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.EDI
{
    public interface IEDI270Repository : IRepositoryBase<EligibilityEnquiry270Master>
    {
        IQueryable<T> SaveEligibilityEnquiryRequestData<T>(int patientId, int patientInsuranceId, string serviceTypeCodeIds, string serviceCodeIds, TokenModel token) where T : class, new();
        EDI270FileModel GetEDI270RequestInfo(int patientId, int patientInsuranceId,int eligibilityEnquiry270MasterId,TokenModel token);
        IQueryable<T> UpdateEligibilityRequest<T>(int eligibilityInquiry270MasterId,string ediText,string action, TokenModel token) where T : class, new();
        IQueryable<T> GetEligibilityEnquiryServiceCodes<T>(TokenModel token) where T : class, new();

    }
}
