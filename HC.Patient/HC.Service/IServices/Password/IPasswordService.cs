using HC.Model;
using HC.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace HC.Patient.Service.IServices.Password
{
    public interface IPasswordService : IBaseService
    {
        JsonModel ForgotPassword(JObject userInfo, TokenModel token, HttpRequest Request);
        JsonModel ResetPasswordForUser(JObject userInfo, TokenModel token);
    }
}
