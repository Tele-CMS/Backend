using System;

namespace HC.Patient.Model.OnboardingDetails
{
    public class OnboardingDetailDto 
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int? OrganizationId { get; set; }

        public int? TenantId { get; set; }

        public int? Order { get; set; }

        public int? OnboardingHeaderId { get; set; }

    }
}