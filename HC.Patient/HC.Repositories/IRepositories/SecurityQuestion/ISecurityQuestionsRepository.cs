using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Repositories.IRepositories.SecurityQuestion
{
    public interface ISecurityQuestionsRepository : IRepositoryBase<SecurityQuestions>
    {
        List<SecurityQuestions> GetSecurityQuestionByOrganization(int OrganizationID);
        bool CheckUserQuestionAnswer(int QuestionID, int UserID, int OrganizationID, string Answer);
    }
}
