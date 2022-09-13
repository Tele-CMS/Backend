using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ReviewSystem
{
    public class SystemReviewDto
    {
        public int system_id { get; set; }
        public int saved_category_id { get; set; }
        public string system_name { get; set; }
        public string comments { get; set; }
        public List<ReviewHistory> history { get; set; }
        public List<ReviewHistory> selectedhistoptions { get; set; }

    }
    public class ReviewHistory
    {
        public int history_id { get; set; }
        public string history_name { get; set; }
        public int system_id { get; set; }
        public bool active { get; set; }

    }
}
