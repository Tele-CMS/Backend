
using System;

namespace HC.Patient.Model.OnboardingHeader
{
    public class GetAllOnboardingHeadersInput 
    {
        public string Filter { get; set; }

        public string HeaderFilter { get; set; }

        public string HeaderDescriptionFilter { get; set; }

        public string HeaderImageFilter { get; set; }

        public string HeaderVideoFilter { get; set; }

        public int? ActiveStatusFilter { get; set; }

        public int? MaxOrganizationIdFilter { get; set; }
        public int? MinOrganizationIdFilter { get; set; }

        public int? MaxTenantIdFilter { get; set; }
        public int? MinTenantIdFilter { get; set; }

        public string CategoryFilter { get; set; }

        public int? MaxTotalStepsFilter { get; set; }
        public int? MinTotalStepsFilter { get; set; }

        public int? MaxIsImageFilter { get; set; }
        public int? MinIsImageFilter { get; set; }

        public int? MaxDurationFilter { get; set; }
        public int? MinDurationFilter { get; set; }

        public string UserNameFilter { get; set; }
        public string Sorting { get; set; }
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }

    }
}