using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.SecurityQuestion;
using HC.Repositories;

namespace HC.Patient.Repositories.Repositories.SecurityQuestion
{
    public class MasterSecurityQuestionsRepository : MasterRepositoryBase<MasterSecurityQuestions>, IMasterSecurityQuestionsRepository
    {
        private HCMasterContext _masterContext;
        public MasterSecurityQuestionsRepository(HCMasterContext masterContext) : base(masterContext)
        {
            this._masterContext = masterContext;
        }
    }
}
