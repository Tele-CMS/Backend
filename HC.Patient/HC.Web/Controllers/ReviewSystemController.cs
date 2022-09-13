using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Model;
using HC.Patient.Model.ReviewSystem;
using HC.Patient.Service.IServices.ReviewSystem;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("ReviewSystem")]
    [ActionFilter]
    public class ReviewSystemController : BaseController
    {
        #region Construtor of the class
        private readonly IReviewSystemService _reviewSystemService;


        public ReviewSystemController(IReviewSystemService reviewSystemService)
        {
            _reviewSystemService = reviewSystemService;
        }
            #endregion


            [HttpPost("GetReviewSystem")]
        public JsonResult GetReviewSystem([FromBody] GetROSModel model)
        {
            try
            {
                // return await Mediator.Send(query);
                return Json(_reviewSystemService.ExecuteFunctions(() => _reviewSystemService.GetReviewSystem(model)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost("SaveReviewSystem")]
        public IActionResult SaveReviewSystem([FromBody] ROSModel model)
        {
            return Json(_reviewSystemService.ExecuteFunctions<JsonModel>(() => _reviewSystemService.SaveReviewSystem(model)));
        }

    }
}
