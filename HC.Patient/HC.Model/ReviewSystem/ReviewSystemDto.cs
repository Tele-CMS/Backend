using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ReviewSystem
{
    public class ReviewSystemDto
    {
        public List<ReviewSystemCategoryDto> system { get; set; }
        public List<ReviewSystemHistoryDto> history { get; set; }
    }

    public class ReviewSystemCategoryDto
    {
        public int system_id { get; set; }
        public string system_name { get; set; }
        public int saved_category_id { get; set; }
        public string comments { get; set; }
        public int systemanonysm_id { get; set; }
    }

    public class ReviewSystemHistoryDto
    {
        public int history_id { get; set; }
        public int system_id { get; set; }
        public string history_name { get; set; }
        public bool active { get; set; } = true;
        public bool ideal_choice { get; set; }

    }

    public class MasterSystemDto
    {
        public int systemanonysm_id { get; set; }
        public int system_id { get; set; }
        public string system_name { get; set; }

    }

    public class FinalReviewSystemDto
    {
        public ReviewSystemDto reviewSystemDto { get; set; }
        public ReviewSystemDto savedReviewSystemDto { get; set; }
    }

    public class FullReviewSystemDto
    {
        public ReviewSystemDto reviewSystemDto { get; set;}
        public ReviewSystemDto savedReviewSystemDto { get; set;}

        //public List<ReviewSystemHistoryDto> history { get; set; }
        //public List<ReviewSystemHistoryDto> history2 { get; set; }
        //public List<MasterSystemDto> master { get; set; }
        //public List<ReviewSystemCategoryDto> system { get; set; }

    }
}
