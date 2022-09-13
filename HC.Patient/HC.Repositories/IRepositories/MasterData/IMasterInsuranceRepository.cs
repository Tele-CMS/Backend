using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.MasterData
{
    public interface IMasterInsuranceRepository: IRepositoryBase<InsuranceCompanies>
    {
        IQueryable<T> GetMasterInsuranceList<T>(string name, int pageNumber, int pageSize,int organizationId) where T : class, new();
    }
}
