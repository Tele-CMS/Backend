using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.MasterData.Interfaces
{
    public interface IMasterDataService : IBaseService
    {
        MasterDataModel GetMasterDataByName(List<string> masterDataNames, TokenModel token, string globalCodeId="");
        List<MasterState> GetStateByCountryID(int countryID);
        JsonModel GetAutoComplateSearchingValues(string tableName, string columnName, string searchText, TokenModel token);
    }
}
