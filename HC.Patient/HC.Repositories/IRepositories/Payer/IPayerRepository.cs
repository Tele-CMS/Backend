using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Payer
{
    public interface IPayerRepository : IRepositoryBase<InsuranceCompanies>
    {
        IQueryable<T> GetPayerList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
