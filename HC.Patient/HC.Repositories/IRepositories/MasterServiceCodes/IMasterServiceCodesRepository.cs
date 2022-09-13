using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterServiceCodes;
using HC.Repositories.Interfaces;
using System;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.MasterServiceCodes
{
    public interface IMasterServiceCodesRepository : IRepositoryBase<Entity.MasterServiceCode>
    {
        MasterServiceCode AddMasterServiceCode(MasterServiceCode masterServiceCode);
        MasterServiceCode GetServiceCodeByID(int ServiceCodeID, TokenModel token);
        MasterServiceCode UpdateServiceCode(MasterServiceCode serviceCode, DateTime CurrentDate);
        IQueryable<T> GetMasterServiceCodes<T>(string searchText, TokenModel token, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "") where T : class, new();
    }
}
