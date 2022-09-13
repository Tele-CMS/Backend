using HC.Model;
using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
    public interface IStaffExperienceRepository
    {
        /// <summary>
        /// To Get Staff Experiences based on staffid
        /// </summary>
        /// <param name="staffId">Staff id</param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        List<StaffExperience> GetStaffExperience(int staffId, TokenModel tokenModel);

        /// <summary>
        /// To Save and update staff experience
        /// </summary>
        /// <param name="staffExperience"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        StaffExperience SaveUpdateStaffExperience(StaffExperience staffExperience, TokenModel tokenModel);

        /// <summary>
        /// To Get Staff Experience by staffId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        StaffExperience GetStaffExperienceById(int id, TokenModel tokenModel);
    }
}
