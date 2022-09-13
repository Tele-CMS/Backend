using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.AuditLog;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/MasterModifier")]
    [ActionFilter]
    public class MasterModifierController : BaseController
    {
        private readonly IMasterModifierService _modifierService;
        
        public MasterModifierController(IMasterModifierService modifierService)
        {
            _modifierService = modifierService;
        }

        /// <summary>
        /// <Description> Get Master Modifiers </Description>
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("GetMasterModifiersList")]
        public JsonResult GetMasterModifiersList(int pageNumber = 1, int pageSize = 10, string modifier = "")
        {
            return Json(_modifierService.ExecuteFunctions(() => _modifierService.GetMasterModifiersList(pageNumber, pageSize, modifier, GetToken(HttpContext))));
        }
        /// <summary>
        /// <Description> Create Master Modifiers </Description>
        /// </summary>       
        /// <returns></returns>
        [HttpPost]
        [Route("CreateMasterModifiers")]
        public JsonResult CreateMasterModifiers([FromBody]MasterModifierModel masterModifierModel)
        {
            return Json(_modifierService.ExecuteFunctions(() => _modifierService.CreateMasterModifiers(masterModifierModel, GetToken(HttpContext))));
        }
        /// <summary>
        /// <Description> Get Master Modifiers Detail by Id </Description>
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        [Route("GetMasterModifierDetail")]
        public JsonResult GetMasterModifierDetail(int modifierId)
        {
            return Json(_modifierService.ExecuteFunctions(() => _modifierService.GetMasterModifierDetail(modifierId)));
        }
        /// <summary>
        /// <Description> Delete Master Modifiers </Description>
        /// </summary>       
        /// <returns></returns>
        [HttpPatch]
        [Route("DeleteMasterModifier")]
        public JsonResult DeleteMasterModifier(int modifierId)
        {
            return Json(_modifierService.ExecuteFunctions(() => _modifierService.DeleteMasterModifier(modifierId, GetToken(HttpContext))));

        }
    }
}