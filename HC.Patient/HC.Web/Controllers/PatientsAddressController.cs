using HC.Model;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("PatientsAddress")]
    [ActionFilter]
    public class PatientsAddressController : BaseController
    {
        private readonly IPatientPhoneAddressService _patientPhoneAddressService;        

        #region Construtor of the class
        public PatientsAddressController(IPatientPhoneAddressService patientPhoneAddressService)
        {
            _patientPhoneAddressService = patientPhoneAddressService;            
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Description  :  this method is used to get patient's phone number and address by patientid   
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("GetPatientPhoneAddress")]
        public JsonResult GetPatientPhoneAddress(int patientId)
        {   
            return Json(_patientPhoneAddressService.ExecuteFunctions<JsonModel>(()=>_patientPhoneAddressService.GetPatientPhoneAddress(patientId, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : this method is used to save patient's phone number and address by patientid    
        /// </summary>
        /// <param name="phoneAddressModel"></param>
        /// <returns></returns>
        [HttpPost("SavePhoneAddress")]
        public JsonResult SavePhoneAddress([FromBody]PhoneAddressModel phoneAddressModel)
        {   
            return Json(_patientPhoneAddressService.ExecuteFunctions<JsonModel>(()=>_patientPhoneAddressService.SavePhoneAddress(phoneAddressModel.PatientId,phoneAddressModel, GetToken(HttpContext))));
        }
        #endregion

        #region Helping Methods
        /////////////////////////
        //helping methods
        #endregion
    }
}