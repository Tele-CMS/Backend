using HC.Patient.Model.ReviewRatings;
using HC.Patient.Model.Users;
using HC.Patient.Service.IServices.ReviewRatings;
using HC.Patient.Service.IServices.User;
using HC.Patient.Service.Token.Interfaces;
using HC.Patient.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HC.Patient.Web.Controllers
{

    [Produces("application/json")]
    [Route("ReviewRatings")]
    [ActionFilter]
    public class ReviewRatingsController : BaseController
    {
        private readonly IReviewRatingService _reviewRatingService;
        private readonly ITokenService _tokenService;

        #region Construtor of the class
        public ReviewRatingsController(IReviewRatingService reviewRatingService, ITokenService tokenService)

        {
            _reviewRatingService = reviewRatingService;
            _tokenService = tokenService;
        }
        #endregion

        #region Class Methods     


        /// <summary>
        /// SaveUpdateReviewRating
        /// </summary>
        /// <param name="reviewRatingsModel"></param>
        /// <returns></returns>
        [HttpPost("SaveUpdateReviewRating")]
        public JsonResult SaveUpdateReviewRating([FromBody]ReviewRatingsModel reviewRatingsModel)
        {
            return Json(_reviewRatingService.ExecuteFunctions(() => _reviewRatingService.SaveUpdateReviewRating(reviewRatingsModel, GetToken(HttpContext))));
        }

        /// <summary>
        /// Description  : Get ReviewRating By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetReviewRatingById")]
        public JsonResult GetReviewRatingById(int id)
        {
            return Json(_reviewRatingService.ExecuteFunctions(() => _reviewRatingService.GetReviewRatingById(id, GetToken(HttpContext))));
        }


        [HttpGet]
        [Route("ReviewRatingByStaffId")]
        public JsonResult ReviewRatingByStaffId(string id,int pageNumber,int pageSize)
        {
           
            return Json(_reviewRatingService.ExecuteFunctions(() => _reviewRatingService.GetReviewRatingByStaffId( id, pageNumber, pageSize)));
        }
        [HttpGet]
        [Route("ReviewRatingAverage")]
        public JsonResult ReviewRatingAverageByStaffId (string id, int pageNumber, int pageSize)
        {

            return Json(_reviewRatingService.ExecuteFunctions(() => _reviewRatingService.ReviewRatingAverageByStaffId(id)));
        }

        #endregion
    }
}