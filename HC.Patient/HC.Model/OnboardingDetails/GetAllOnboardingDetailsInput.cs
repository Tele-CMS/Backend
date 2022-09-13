
using System;

namespace HC.Patient.Model.OnboardingDetails
{
    public class GetAllOnboardingDetailsInput 
    {
        public string Filter { get; set; }
        public string Sorting { get; set; }
        public  int MaxResultCount { get; set; }
            public  int SkipCount { get; set; }
        public string TitleFilter { get; set; }

        public string ShortDescriptionFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? MaxOrganizationIdFilter { get; set; }
        public int? MinOrganizationIdFilter { get; set; }

        public int? MaxTenantIdFilter { get; set; }
        public int? MinTenantIdFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

        public string OnboardingHeaderHeaderFilter { get; set; }

    }
}