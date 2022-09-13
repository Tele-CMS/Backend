using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Payer;
using HC.Patient.Model.Users;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.Interfaces
{
    public interface IUserCommonRepository//: IRepository<MasterDataModel>
    {
        User GetUserByUserName(string userName);
        AuthenticationToken AuthenticationToken( AuthenticationToken authenticationToken);
        int SetPasswordForUser(UserPassword userPassword);
        bool CheckIfRecordExists(RecordExistFilter recordExistFilter);
        List<PayerInfoDropDownModel> GetPayerByPatientID(int patientID, string Key);
        JsonModel UpdateAccessFailedCount(int userID, TokenModel tokenModel);
        void ResetUserAccess(int userID, TokenModel tokenModel);
        bool UpdateStaffActiveStatus(int staffId, bool isActive);
        bool UserAlreadyLoginFromSameMachine(MachineLoginLog machineLoginUser);
        bool SaveMachineLoginUser(MachineLoginLog machineLoginUser);
        bool IsUserMachineLogin(MachineLoginLog machineLoginUser);
        IQueryable<T> CheckRecordDepedencies<T>(int Id,string tableName,bool isTableListRequired, TokenModel token) where T : class, new();
        bool CheckIfRecordExistsMasterDB(RecordExistFilter recordExistFilter);
        void AddIPAddress(IpAddressLog ipAddressLog);
        /// <summary>
        /// To Get Authentication Token by Token Id
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        AuthenticationToken GetAuthenticationToken(string tokenId, TokenModel tokenModel);
    }
}
