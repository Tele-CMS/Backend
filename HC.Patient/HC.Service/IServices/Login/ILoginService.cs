using HC.Common.Options;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.Common;
using HC.Patient.Model.SecurityQuestion;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Login
{
    public interface ILoginService : IBaseService
    {
        JsonModel MobileLogin(ApplicationUser applicationUser, JwtIssuerOptions _jwtOptions, TokenModel token);
        JsonModel MobileLogout(TokenModel token);
        JsonModel Login(ApplicationUser applicationUser, JwtIssuerOptions _jwtOptions, TokenModel token);
        JsonModel PatientLogin(ApplicationUser applicationUser, JwtIssuerOptions _jwtOptions, TokenModel token);
        JsonModel SaveUserSecurityQuestion(SecurityQuestionListModel securityQuestionListModel, JwtIssuerOptions _jwtOptions, TokenModel token);
        JsonModel CheckQuestionAnswer(SecurityQuestionModel securityQuestionModel, JwtIssuerOptions _jwtOptions, TokenModel token);
        PasswordCheckModel CheckPasswordExpiredMessage(Entity.User user, TokenModel token);
    }
}
