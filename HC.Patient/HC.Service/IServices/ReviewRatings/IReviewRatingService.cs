using HC.Model;
using HC.Patient.Model.ReviewRatings;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.ReviewRatings
{
    public interface IReviewRatingService: IBaseService
    {
        JsonModel SaveUpdateReviewRating(ReviewRatingsModel  reviewRatingsModel, TokenModel tokenModel);
        JsonModel GetReviewRatingById (int id, TokenModel tokenModel);
        JsonModel GetReviewRatingByStaffId(string id,int pageNumber,int pageSize);
        JsonModel ReviewRatingAverageByStaffId(string id);

    }
}
