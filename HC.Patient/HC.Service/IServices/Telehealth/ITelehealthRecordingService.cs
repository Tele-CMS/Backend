using HC.Model;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices
{
    public interface ITelehealthRecordingService : IBaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivedId"></param>
        /// <param name="sessionId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        JsonModel SaveVideoArchived(string archivedId, int sessionId, TokenModel tokenModel);
    }
}
