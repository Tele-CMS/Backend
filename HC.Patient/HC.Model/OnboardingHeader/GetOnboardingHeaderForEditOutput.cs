using System;
using System.ComponentModel.DataAnnotations;

namespace HC.Patient.Model.OnboardingHeader
{
    public class GetOnboardingHeaderForEditOutput
    {
        public CreateOrEditOnboardingHeaderDto OnboardingHeader { get; set; }

        public string UserName { get; set; }

    }
}