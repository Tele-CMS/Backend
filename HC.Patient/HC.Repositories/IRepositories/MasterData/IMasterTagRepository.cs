using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.MasterData
{
    public interface IMasterTagRepository : IRepositoryBase<MasterTags>
    {
        IQueryable<T> GetMasterTagList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
