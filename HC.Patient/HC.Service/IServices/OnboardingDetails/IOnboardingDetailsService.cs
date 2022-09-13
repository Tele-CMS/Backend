using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.OnboardingDetails;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.Patient.Service.IServices.OnboardingDetails
{
    public interface IOnboardingDetailsService : IBaseService
    {
        Task<GetOnboardingDetailForViewDto> GetOnboardingDetailForView(int id);
        Task<GetOnboardingDetailForEditOutput> GetOnboardingDetailForEdit(EntityDto input);
        Task CreateOrEdit(CreateOrEditOnboardingDetailDto input, TokenModel token);
        Task Delete(EntityDto input, TokenModel token);
        List<OnboardingDetail> GetAllByHeaderId(int id);
    }
}
