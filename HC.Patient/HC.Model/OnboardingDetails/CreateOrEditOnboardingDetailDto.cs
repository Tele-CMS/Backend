using System;
using System.ComponentModel.DataAnnotations;

namespace HC.Patient.Model.OnboardingDetails
{
    public class CreateOrEditOnboardingDetailDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int? OrganizationId { get; set; }

        public int? TenantId { get; set; }

        public int? Order { get; set; }

        public int? OnboardingHeaderId { get; set; }

    }
}