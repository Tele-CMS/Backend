using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.ReviewSystem
{
    public class ROSModel
    {
      public int patient_id { get; set; }
      public int encounter_id { get; set; } 
      public int user_id { get; set; }
       public List<SystemReviewModel> system_review { get; set; }
        
    }

    public class SystemReviewModel
    {
        public int system_id { get; set; }
        public int saved_category_id { get; set; }
        public string system_name { get; set; }
        public string comments { get; set; }
        public List<ReviewHistoryModel> history { get; set; }
        public List<ReviewHistoryModel> selectedhistoptions { get; set; }

    }
    public class ReviewHistoryModel
    {
        public int history_id { get; set; }
        public string history_name { get; set; }
        public int system_id { get; set; }
        public bool active { get; set; }

    }

    //public class SystemReviewModel
    //{
    //    public string comments { get; set; }
    //    public int saved_category_id { get; set; }
    //    public int system_id { get; set; }
    //    public int system_name { get; set; }

    //    history
    //        selectedhistoptions
    //    //system_review: this.reviewSystemForm.value.systems,
    //}



}
