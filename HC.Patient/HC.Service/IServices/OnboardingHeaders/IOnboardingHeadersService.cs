using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.OnboardingDetails;
using HC.Patient.Model.OnboardingHeader;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.Patient.Service.IServices.OnboardingHeaders
{
    public  interface IOnboardingHeadersService : IBaseService
    {
        Task<OnboardingHeaderDto> GetOnboardingHeaderForView(int id, TokenModel token);
        Task<GetOnboardingHeaderForEditOutput> GetOnboardingHeaderForEdit(EntityDto input, TokenModel token);
        int CreateOrEdit(CreateOrEditOnboardingHeaderDto input, TokenModel token);
        Task Delete(EntityDto input, TokenModel token);
        Task<List<OnboardingHeaderDto>> GetAllWithoutPagination(TokenModel token);
        Task<List<OnboardingHeaderDto>> GetAllByCategory(string category, TokenModel token);
    }
}
