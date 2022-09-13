using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Repositories;

namespace HC.Patient.Repositories.Repositories.SecurityQuestion
{
    public class UserSecurityQuestionAnswerRepository : RepositoryBase<UserSecurityQuestionAnswer>, IUserSecurityQuestionAnswerRepository
    {
        private HCOrganizationContext _context;
        public UserSecurityQuestionAnswerRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }
    }
}
