using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.MasterData.Interfaces;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.MasterData
{
    public class MasterDataService : BaseService, IMasterDataService
    {
        #region Global Variables
        private readonly IRepository _masterDataRepository;
        #endregion
        public MasterDataService(IRepository masterDataRepository)
        {
            this._masterDataRepository = masterDataRepository;
        }
        public MasterDataModel GetMasterDataByName(List<string> masterDataNames, TokenModel token,string globalCodeId="")
        {
            return _masterDataRepository.GetMasterDataByName(masterDataNames, token,globalCodeId);
        }

        public List<MasterState> GetStateByCountryID(int countryID)
        {
            return _masterDataRepository.GetStateByCountryID(countryID);
        }

        public JsonModel GetAutoComplateSearchingValues(string tableName,string columnName,string searchText,  TokenModel token)
        {
            try
            {
                List<AutoCompleteSearchModel> values = _masterDataRepository.GetAutoComplateSearchingValues<AutoCompleteSearchModel>(tableName, columnName, searchText, token).ToList();
                return new JsonModel()
                {
                    data = values,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError=e.Message
                };
            }
        }
    }
}
