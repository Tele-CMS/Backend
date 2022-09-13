using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Service.Services.MasterData
{
    public class MasterInsuranceService: BaseService, IMasterInsuranceService
    {
        private IMasterInsuranceRepository _masterInsuranceRepository;
        public MasterInsuranceService(IMasterInsuranceRepository masterInsuranceRepository)
        {
            _masterInsuranceRepository = masterInsuranceRepository;
        }
        public List<MasterInsuranceModel> GetMasterInsuranceList(string name, int pageNumber, int pageSize, int organizationId)
        {
            return _masterInsuranceRepository.GetMasterInsuranceList<MasterInsuranceModel>(name, pageNumber, pageSize, organizationId).ToList();
        }
    }
}
