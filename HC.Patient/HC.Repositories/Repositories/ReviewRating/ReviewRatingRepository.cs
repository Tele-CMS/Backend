using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.ReviewRatings;
using HC.Patient.Repositories.IRepositories.ReviewRating;
using HC.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HC.Patient.Repositories.Repositories.ReviewRating
{
    public class ReviewRatingRepository : RepositoryBase<ReviewRatings>, IReviewRatingRepository
    {
        private HCOrganizationContext _context;
        public ReviewRatingRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public List<ReviewRatingProviderModel> GetReviewRatingListByStaffId(int id, int pageNumber, int pageSize)
        {
            SqlParameter[] parameters = {
                                          new SqlParameter("@StaffId", id),
                                          new SqlParameter("@PageNumber", pageNumber),
                                          new SqlParameter("@PageSize", pageSize),
            };
            return _context.ExecStoredProcedureListWithOutput<ReviewRatingProviderModel>("RR_Get_AllReviewRatingList", parameters.Length, parameters).ToList();
        }

        public ReviewRatingAverage GetReviewRatingAverageByStaffId(int id)
        {
            int count = 0, average = 0;
            var data = from RR in _context.ReviewRatings
                       join AS in _context.AppointmentStaff
                       on RR.PatientAppointmentId equals AS.PatientAppointmentID
                       join ST in _context.Staffs
                       on AS.StaffID equals ST.Id
                       where AS.StaffID == id && RR.IsDeleted == false && RR.IsActive == true
                       select new ReviewRatingsModel
                       {
                           Id = RR.Id,
                           Rating = RR.Rating,

                       };
              count = data.Count();
           
            if (count > 0)
            {
                var sum = data.Select(x => x.Rating).Sum();
                if (sum > 0)
                {
                    average = sum / count;

                }
            }
            ReviewRatingAverage reviewRatingAverage = new ReviewRatingAverage()
            {
                Average = average,
                Total = count
            };

            return reviewRatingAverage;
        }
    }
}
