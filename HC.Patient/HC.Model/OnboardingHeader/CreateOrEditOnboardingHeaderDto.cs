using System;
using System.ComponentModel.DataAnnotations;

namespace HC.Patient.Model.OnboardingHeader
{
    public class CreateOrEditOnboardingHeaderDto 
    {
        public int? Id { get; set; }
        public string Header { get; set; }

        public string HeaderDescription { get; set; }

        public string HeaderImage { get; set; }

        public string HeaderVideo { get; set; }

        public bool ActiveStatus { get; set; }

        public int? OrganizationId { get; set; }

        public int? TenantId { get; set; }

        public string Category { get; set; }

        public int? TotalSteps { get; set; }

        public int? IsImage { get; set; }

        public int? Duration { get; set; }

        public long? UserId { get; set; }

    }
}