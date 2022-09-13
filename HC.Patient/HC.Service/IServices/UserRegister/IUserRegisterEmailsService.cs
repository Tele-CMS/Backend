using HC.Model;
using HC.Patient.Model;
using Microsoft.AspNetCore.Http;

namespace HC.Patient.Service.IServices
{
    public interface IUserRegisterEmailsService
    {
        string SendAcceptedEmailToRegisterUser(RegisterUserModel registerUser, TokenModel tokenModel, HttpRequest Request, bool isPatient);
    }
}
