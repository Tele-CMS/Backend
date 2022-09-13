using System;
using System.ComponentModel.DataAnnotations;

namespace HC.Patient.Model.OnboardingDetails
{
    public class GetOnboardingDetailForEditOutput
    {
        public CreateOrEditOnboardingDetailDto OnboardingDetail { get; set; }

        public string OnboardingHeaderHeader { get; set; }

    }
}