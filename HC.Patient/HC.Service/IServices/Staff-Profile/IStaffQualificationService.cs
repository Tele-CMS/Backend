using HC.Common.Model.Staff;
using HC.Model;

namespace HC.Patient.Service.Services
{
    public interface IStaffQualificationService
    {
        /// <summary>
        /// To Get staff qualifications based on staffid
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        JsonModel getStaffQualifications(TokenModel tokenModel, string staffId);

        /// <summary>
        /// To Save and update staff qualifications
        /// </summary>
        /// <param name="staffExperienceModels"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveUpdateQualifications(StaffQualificationRequestModel staffQualificationRequestModel, TokenModel tokenModel);
    }
}
