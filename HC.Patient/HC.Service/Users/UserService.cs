using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Payer;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Service.Users.Interfaces;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Users
{
    public  class UserService : BaseService, IUserService
    {
        #region Global Variables
        private readonly IUserCommonRepository _userCommonRepository;
        private JsonModel response;
        #endregion
        public UserService(IUserCommonRepository userCommonRepository)
        {
            this._userCommonRepository = userCommonRepository;
        }

        public User GetUserByUserName(string userName)
        {
            return _userCommonRepository.GetUserByUserName(userName);
        }

        public AuthenticationToken AuthenticationToken(AuthenticationToken authenticationToken)
        {
            return _userCommonRepository.AuthenticationToken(authenticationToken);
        }

        public int SetPasswordForUser(UserPassword userPassword)
        {
            return _userCommonRepository.SetPasswordForUser(userPassword);
        }

        public bool CheckIfRecordExists(RecordExistFilter recordExistFilter)
        {
            return _userCommonRepository.CheckIfRecordExists(recordExistFilter);
        }

        public bool CheckIfRecordExistsMasterDB(RecordExistFilter recordExistFilter)
        {
            return _userCommonRepository.CheckIfRecordExistsMasterDB(recordExistFilter);
        }

        public List<PayerInfoDropDownModel> GetPayerByPatientID(int patientID, string Key)
        {
            return _userCommonRepository.GetPayerByPatientID(patientID, Key);
        }

        public JsonModel UpdateAccessFailedCount(int userID, TokenModel tokenModel)
        {
            return _userCommonRepository.UpdateAccessFailedCount(userID, tokenModel);
        }
        public void ResetUserAccess(int userID, TokenModel tokenModel)
        {
            _userCommonRepository.ResetUserAccess(userID, tokenModel);
        }

        public JsonModel UpdateStaffActiveStatus(int userID, bool isActive)
        {
            try
            {
                var userDetails = _userCommonRepository.UpdateStaffActiveStatus(userID, isActive);
                if (isActive)
                {
                    return response = new JsonModel()
                    {
                        data = userDetails,
                        Message = StatusMessage.UserActivation,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
                else
                {
                    return response = new JsonModel()
                    {
                        data = userDetails,
                        Message = StatusMessage.UserDeactivation,
                        StatusCode = (int)HttpStatusCodes.OK
                    };
                }
            }
            catch (Exception e)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError=e.Message
                };
            }
        }
        public bool SaveMachineLoginUser(MachineLoginLog machineLoginLog)
        {
            //bool Result = false;
            //if (!_userCommonRepository.IsUserMachineLogin(machineLoginLog))
            //{
            //    Result = _userCommonRepository.SaveMachineLoginUser(machineLoginLog);
            //}
            //return Result;
            return _userCommonRepository.SaveMachineLoginUser(machineLoginLog);
        }
        public bool UserAlreadyLoginFromSameMachine(MachineLoginLog machineLoginLog)
        {
            return _userCommonRepository.UserAlreadyLoginFromSameMachine(machineLoginLog);
        }
        public void AddIpAddress()
        {
            IpAddressLog ipAddressLog = new IpAddressLog()
            {
                LocalIP = CommonMethods.GetLocalIPAddress(),
                PublicIP = CommonMethods.GetPublicIpAddress(),
                HostMachine = Dns.GetHostEntry(Dns.GetHostName()).HostName,
                UserHost= System.Security.Principal.WindowsIdentity.GetCurrent().Name

        };
            _userCommonRepository.AddIPAddress(ipAddressLog);
        }
    }
}
