using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ReviewRatings
{
    public class ReviewRatingProviderModel
    {
        public int? Id { get; set; }
        public int? Rating { get; set; }
        public string Review { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string PatientName { get; set; }
        public int? TotalRecords { get; set; }
    }
    public class ReviewRatingAverage
    {
        public int Average { get; set; }
        public int Total { get; set; }

    }
}
