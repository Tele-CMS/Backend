using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface IStaffProfileService : IBaseService
    {
        /// <summary>
        /// To Get Staff Location With Availabilities with respect to location and staff id
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel GetStaffLocationAndAvailability(string staffId, TokenModel tokenModel);
    }
}
