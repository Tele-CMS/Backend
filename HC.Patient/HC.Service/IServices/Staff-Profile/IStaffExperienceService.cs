using HC.Common.Model.Staff;
using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.Services
{
    public interface IStaffExperienceService : IBaseService
    {
        /// <summary>
        /// To get staff experience with staff id
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        JsonModel getStaffExperiences(TokenModel tokenModel, string staffId);

        /// <summary>
        /// To save and update staff experience
        /// </summary>
        /// <param name="staffExperienceModels"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveUpdateExperience(StaffExperienceRequestModel staffExperienceModels, TokenModel tokenModel);
    }
}
