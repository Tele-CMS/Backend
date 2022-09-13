using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Interfaces
{
    public interface IRepository //: IRepository<MasterDataModel>
    {
        MasterDataModel GetMasterDataByName(List<string> masterDataNames, TokenModel token,string globalCodeId="");
        List<MasterState> GetStateByCountryID(int countryID);
        IQueryable<T> GetAutoComplateSearchingValues<T>(string tableName, string columnName, string searchText, TokenModel token) where T : class, new();
    }
}
