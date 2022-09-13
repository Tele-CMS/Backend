using HC.Patient.Model.OnboardingDetails;
using HC.Patient.Model.OnboardingHeader;
using HC.Patient.Service.IServices.OnboardingHeaders;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("OnboardingHeader")]
    [ActionFilter]
    public class OnboardingHeaderController : BaseController
    {

        private readonly IOnboardingHeadersService _onboardingHeadersService;
        //   private readonly IBulkMessageService _bulkMessageService;
        public OnboardingHeaderController(IOnboardingHeadersService onboardingHeadersService)
        {
            _onboardingHeadersService = onboardingHeadersService;
        }

        [HttpGet]
        [Route("GetOnboardingHeaderForView")]
        public async Task<OnboardingHeaderDto> GetOnboardingHeaderForView(int id)
        {
            return  await _onboardingHeadersService.ExecuteFunctions<Task<OnboardingHeaderDto>>(() => _onboardingHeadersService.GetOnboardingHeaderForView(id, GetToken(HttpContext)));
        }

        [HttpGet]
        [Route("GetOnboardingHeaderForEdit")]
        public async Task<GetOnboardingHeaderForEditOutput> GetOnboardingHeaderForEdit(EntityDto input)
        {
            return await _onboardingHeadersService.ExecuteFunctions<Task<GetOnboardingHeaderForEditOutput>>(() => _onboardingHeadersService.GetOnboardingHeaderForEdit(input, GetToken(HttpContext)));
        }

        [HttpPost]
        [Route("CreateOrEdit")]
        public int CreateOrEdit([FromBody] CreateOrEditOnboardingHeaderDto input)
        {
            return _onboardingHeadersService.ExecuteFunctions<int>(() => _onboardingHeadersService.CreateOrEdit(input, GetToken(HttpContext)));
        }

        [HttpPatch]
        [Route("Delete")]
        public async Task Delete(EntityDto input)
        {
            await _onboardingHeadersService.ExecuteFunctions(() => _onboardingHeadersService.Delete(input, GetToken(HttpContext)));
        }

        [HttpGet]
        [Route("GetAllWithoutPagination")]
        public async Task<List<OnboardingHeaderDto>> GetAllWithoutPagination()
        {
            return await _onboardingHeadersService.ExecuteFunctions<Task<List<OnboardingHeaderDto>>>(() => _onboardingHeadersService.GetAllWithoutPagination(GetToken(HttpContext)));
        }
        [HttpGet]
        [Route("GetAllByCategory")]
        public async Task<List<OnboardingHeaderDto>> GetAllByCategory(string category)
        {
            return await _onboardingHeadersService.ExecuteFunctions<Task<List<OnboardingHeaderDto>>>(() => _onboardingHeadersService.GetAllByCategory(category,GetToken(HttpContext)));
        }
    }
}
