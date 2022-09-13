using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Repositories.SecurityQuestion
{
    public class SecurityQuestionsRepository : RepositoryBase<SecurityQuestions>, ISecurityQuestionsRepository
    {
        private HCOrganizationContext _context;
        public SecurityQuestionsRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public bool CheckUserQuestionAnswer(int QuestionID, int UserID, int OrganizationID, string Answer)
        {
            return _context.UserSecurityQuestionAnswer.Any(x => x.QuestionID == QuestionID
                                            && x.UserID == UserID
                                            && x.OrganizationID == OrganizationID
                                            && x.Answer.Equals(Answer));
        }

        public List<SecurityQuestions> GetSecurityQuestionByOrganization(int OrganizationID)
        {
            return _context.SecurityQuestions.Where(a => a.OrganizationID == OrganizationID
               && a.IsActive == true
               && a.IsDeleted == false
            ).ToList();
        }
    }
}
