using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Common.Model.Staff
{
    public class StaffExperienceRequestModel
    {
        public string staffId;
        public List<StaffExperienceModel> staffExperiences;
    }

    public class StaffQualificationRequestModel
    {
        public string staffId;
        public List<StaffQualificationModel> staffQualification;
    }

    public class StaffAwardRequestModel
    {
        public string staffId;
        public List<StaffAwardModel> staffAward;
    }
}
