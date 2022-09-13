using HC.Common.Model.Staff;
using HC.Model;

namespace HC.Patient.Service.Services
{
    public interface IStaffAwardService
    {
        /// <summary>
        /// To get staff awrds by staff id
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        JsonModel getStaffAwards(TokenModel tokenModel, string staffId);

        /// <summary>
        /// To save and update staff awards
        /// </summary>
        /// <param name="staffAwardRequestModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveUpdateAwards(StaffAwardRequestModel staffAwardRequestModel, TokenModel tokenModel);
    }
}
