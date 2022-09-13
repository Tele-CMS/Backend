using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.ReviewRatings;
using HC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.ReviewRating
{
   public interface IReviewRatingRepository: IRepositoryBase<ReviewRatings>
    {
        //int SaveReviewRating(ReviewRatings reviewRatings, TokenModel tokenModel);
        // int UpdateReviewRating(ReviewRatings reviewRatings, TokenModel tokenModel);

        List<ReviewRatingProviderModel> GetReviewRatingListByStaffId(int id,int pageNumber, int pageSize) ;
        ReviewRatingAverage GetReviewRatingAverageByStaffId (int id);

    }
}
