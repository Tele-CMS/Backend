using HC.Patient.Entity;
using HC.Patient.Model.OnboardingDetails;
using HC.Patient.Service.IServices.OnboardingDetails;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("OnboardingDetail")]
    [ActionFilter]
    public class OnboardingDetailController : BaseController
    {
        private readonly IOnboardingDetailsService _onboardingDetailsService;
        //   private readonly IBulkMessageService _bulkMessageService;
        public OnboardingDetailController(IOnboardingDetailsService onboardingDetailsService)
        {
            _onboardingDetailsService = onboardingDetailsService;
        }

        [HttpGet]
        [Route("GetOnboardingDetailForView")]
        public async Task<GetOnboardingDetailForViewDto> GetOnboardingDetailForView(int id)
        {
            return await _onboardingDetailsService.ExecuteFunctions<Task<GetOnboardingDetailForViewDto>>(() => _onboardingDetailsService.GetOnboardingDetailForView(id));
        }

        [HttpGet]
        [Route("GetOnboardingDetailForEdit")]
        public async Task<GetOnboardingDetailForEditOutput> GetOnboardingDetailForEdit(EntityDto input)
        {
            return await _onboardingDetailsService.ExecuteFunctions<Task<GetOnboardingDetailForEditOutput>>(() => _onboardingDetailsService.GetOnboardingDetailForEdit(input));
        }

        [HttpPost]
        [Route("CreateOrEdit")]
        public async Task CreateOrEdit([FromBody] CreateOrEditOnboardingDetailDto input)
        {
             await _onboardingDetailsService.ExecuteFunctions(() => _onboardingDetailsService.CreateOrEdit(input,GetToken(HttpContext)));
        }
        [HttpPatch]
        [Route("Delete")]
        public async Task Delete(EntityDto input)
        {
            await _onboardingDetailsService.ExecuteFunctions(() => _onboardingDetailsService.Delete(input, GetToken(HttpContext)));
        }

        [HttpGet]
        [Route("GetAllByHeaderId")]
        public List<OnboardingDetail> GetAllByHeaderId(int id)
        {
            return  _onboardingDetailsService.ExecuteFunctions<List<OnboardingDetail>>(() => _onboardingDetailsService.GetAllByHeaderId(id));
        }
    }
}
