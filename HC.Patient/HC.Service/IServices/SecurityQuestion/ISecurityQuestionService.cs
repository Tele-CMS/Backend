using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.SecurityQuestion;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.SecurityQuestion
{
    public interface ISecurityQuestionService :IBaseService
    {
        List<UserSecurityQuestionAnswer> GetUserSecurityQuestionsById(int UserId, TokenModel token);
        List<UserSecurityQuestionAnswer> GetUserSecurityQuestionsById(int UserId, int OrganizationID);
        bool CheckUserQuestionAnswer(SecurityQuestionModel securityQuestionModel, TokenModel token);
        List<SecurityQuestions> GetSecurityQuestion(TokenModel token);
        bool SaveUserSecurityQuestion(SecurityQuestionListModel securityQuestionListModel, TokenModel token);
        bool CheckUserQuestionAnswer(int QuestionID, int UserID, int OrganizationID, string Answer);
    }
}
