using HC.Model;
using HC.Patient.Model.Patient;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace HC.Patient.Repositories.IRepositories.Authorization
{
    public interface IAuthorizationRepository : IRepositoryBase<Entity.Authorization>
    {
        GetAuthorizationByIdModel GetAutorizationById(SearchFilterModel searchFilterModel, TokenModel tokenModel);
    }
}
