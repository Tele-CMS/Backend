using HC.Model;
using HC.Patient.Model;

namespace HC.Patient.Service.IServices
{
    public interface IEmailWriteService
    {
        /// <summary>
        /// To Save Email or maintain email log within site
        /// </summary>
        /// <param name="emailLog">EmailModel M0del</param>
        /// <param name="tokenModel">TokenModel value</param>
        /// <returns></returns>
        JsonModel SaveEmailLog(EmailModel emailModel, TokenModel tokenModel);
    }
}
