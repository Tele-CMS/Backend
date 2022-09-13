using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Payer;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories
{
    public class UserCommonRepository : IUserCommonRepository
    {
        private readonly HCOrganizationContext _context;
        private readonly HCMasterContext _masterContext;
        public UserCommonRepository(HCOrganizationContext context, HCMasterContext masterContext)
        {
            this._context = context;
            this._masterContext = masterContext;
        }

        /// <summary>
        /// get user by user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUserByUserName(string userName)
        {
            try
            {
                return _context.User.Where(m => m.UserName.ToUpper() == userName.ToUpper()).Include(x => x.UserRoles).FirstOrDefault();
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// save token into database
        /// <CreateBy>Prince Kumar </CreateBy>        
        /// </summary>
        /// <param name="authenticationToken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public AuthenticationToken AuthenticationToken(AuthenticationToken authenticationToken)
        {
            try
            {
                AuthenticationToken tokenInfo = _context.AuthenticationToken.Where(a => a.UserID == authenticationToken.UserID).FirstOrDefault();
                if (tokenInfo != null)
                {
                    tokenInfo.ResetPasswordToken = authenticationToken.ResetPasswordToken;
                    tokenInfo.Token = authenticationToken.Token;
                    tokenInfo.IsDeleted = false;
                    tokenInfo.IsActive = true;
                    tokenInfo.OrganizationID = authenticationToken.OrganizationID;
                    _context.SaveChanges();
                    return tokenInfo;
                }
                else
                {
                    _context.AuthenticationToken.Add(authenticationToken);
                    _context.SaveChanges();
                    return authenticationToken;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public AuthenticationToken GetAuthenticationToken(string tokenId, TokenModel tokenModel)
        {
            return _context.AuthenticationToken.Where(a => a.ResetPasswordToken == tokenId && a.OrganizationID == tokenModel.OrganizationID && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();

        }

        /// <summary>
        /// save user IP and MAC address along with userid
        /// </summary>
        /// <param name="machineLoginUser"></param>
        /// <returns></returns>
        public bool SaveMachineLoginUser(MachineLoginLog machineLoginUser)
        {
            try
            {

                //if user login from same machine same ip
                List<MachineLoginLog> _machineLoginLog = _context.MachineLoginLog.Where(a => a.IpAddress == machineLoginUser.IpAddress && a.MacAddress == machineLoginUser.MacAddress && a.UserId == machineLoginUser.UserId).ToList();
                if (_machineLoginLog != null && _machineLoginLog.Count() > 0)
                {
                    return true;
                }
                else//if user login from diff machine
                {
                    _context.MachineLoginLog.Add(machineLoginUser);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///// <summary>
        ///// save user IP and MAC address along with userid
        ///// </summary>
        ///// <param name="machineLoginUser"></param>
        ///// <returns></returns>
        //public bool SaveMachineLoginUser(MachineLoginLog machineLoginUser)
        //{
        //    try
        //    {
        //        _context.MachineLoginLog.Add(machineLoginUser);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Checks if the user has logged in from the machine
        /// </summary>
        /// <param name="machineLoginUser"></param>
        /// <returns></returns>
        public bool IsUserMachineLogin(MachineLoginLog machineLoginUser)
        {
            try
            {
                //if user login from same machine same ip
                return _context.MachineLoginLog.Any(x => x.IpAddress == machineLoginUser.IpAddress && x.MacAddress == machineLoginUser.MacAddress && x.UserId == machineLoginUser.UserId);
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// check user already login from same user or not
        /// </summary>
        /// <param name="machineLoginUser"></param>
        /// <returns></returns>
        public bool UserAlreadyLoginFromSameMachine(MachineLoginLog machineLoginUser)
        {
            try
            {
                List<MachineLoginLog> _machineLoginLog = _context.MachineLoginLog.Where(a => a.IpAddress == machineLoginUser.IpAddress && a.MacAddress == machineLoginUser.MacAddress && a.UserId == machineLoginUser.UserId).ToList();
                if (_machineLoginLog != null && _machineLoginLog.Count() > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// set password of the user
        /// </summary>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public int SetPasswordForUser(UserPassword userPassword)
        {
            try
            {
                var userToken = _context.AuthenticationToken.Where(p => p.ResetPasswordToken == userPassword.Token).FirstOrDefault();
                var user = _context.User.Where(p => p.Id == userToken.UserID).FirstOrDefault();
                user.Password = CommonMethods.Encrypt(userPassword.Password);
                _context.User.Update(user);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }


        /// <summary>
        /// check if record exists in database for a particular table
        /// </summary>
        /// <param name="recordExistFilter"></param>
        /// <returns></returns>
        public bool CheckIfRecordExists(RecordExistFilter recordExistFilter)
        {
            try
            {
                var recordsCount = _context.ExecStoredProcedureListWithOutput<NoOfRecordsModel>("USR_CheckIfRecordExists",
            typeof(NoOfRecordsModel).GetProperties().Count(),

            new SqlParameter()
            {
                ParameterName = "@TableName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.TableName

            },
            new SqlParameter()
            {
                ParameterName = "@Value",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.Value

            },
            new SqlParameter()
            {
                ParameterName = "@ColmnName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.ColmnName

            },
            new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.ID

            },
            new SqlParameter()
            {
                ParameterName = "@OrganizationID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.OrganizationID

            }).FirstOrDefault();
                if (recordsCount.NoOfRecords > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// check if record exists in database for a particular table
        /// </summary>
        /// <param name="recordExistFilter"></param>
        /// <returns></returns>
        public bool CheckIfRecordExistsMasterDB(RecordExistFilter recordExistFilter)
        {
            try
            {
                var recordsCount = _masterContext.ExecStoredProcedureListWithOutput<NoOfRecordsModel>("MST_CheckIfRecordExists",
            typeof(NoOfRecordsModel).GetProperties().Count(),

            new SqlParameter()
            {
                ParameterName = "@TableName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.TableName

            },
            new SqlParameter()
            {
                ParameterName = "@Value",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.Value

            },
            new SqlParameter()
            {
                ParameterName = "@ColmnName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.ColmnName

            },
            new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = recordExistFilter.ID

            }).FirstOrDefault();
                //,
                //new SqlParameter()
                //{
                //    ParameterName = "@OrganizationID",
                //    SqlDbType = SqlDbType.Int,
                //    Direction = ParameterDirection.Input,
                //    Value = recordExistFilter.OrganizationID

                //})



                if (recordsCount.NoOfRecords > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        public List<PayerInfoDropDownModel> GetPayerByPatientID(int patientID, string key)
        {
            try
            {
                return _context.ExecStoredProcedureListWithOutput<PayerInfoDropDownModel>("PAY_GetPayerByPatientID",
               typeof(PayerInfoDropDownModel).GetProperties().Count(),
               new SqlParameter()
               {
                   ParameterName = "@PatientID",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = patientID

               },
               new SqlParameter()
               {
                   ParameterName = "@Key",
                   SqlDbType = SqlDbType.VarChar,
                   Direction = ParameterDirection.Input,
                   Value = key

               }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        ///  To update failed access count
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public JsonModel UpdateAccessFailedCount(int userID, TokenModel tokenModel)
        {
            try
            {
                string Message = string.Empty;
                var user = _context.User.Where(p => p.Id == userID).FirstOrDefault();
                if (user.UserRoles.RoleName == OrganizationRoles.Admin.ToString())
                {
                    Message = StatusMessage.InvalidUserOrPassword;//If Admin login with wrong credentials
                }
                else if (user.AccessFailedCount >= 2) // if user attemped 3 time with wrong credentials
                {
                    user.IsBlock = true;
                    user.BlockDateTime = DateTime.UtcNow;
                    user.AccessFailedCount = user.AccessFailedCount + 1;
                    Message = UserAccountNotification.AccountDeactiveOrExpirePass;//block
                }
                else // if wrong attemped increase the failed count
                {
                    if (user.BlockDateTime == null)
                    {
                        Message = UserAccountNotification.AccountDeactive;
                        user.AccessFailedCount = user.AccessFailedCount + 1;
                        Message = UserAccountNotification.InvalidPassword;//Invaild Password
                    }
                    else
                    {
                        user.AccessFailedCount = user.AccessFailedCount + 1;
                        Message = UserAccountNotification.InvalidPassword;//Invaild Password
                    }
                }
                //save
                _context.User.Update(user);
                _context.SaveChanges();
                //return
                return new JsonModel()
                {
                    data = new object(),
                    Message = Message,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                };
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                };
            }
        }

        /// <summary>
        /// To reset user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tokenModel"></param>
        public void ResetUserAccess(int userID, TokenModel tokenModel)
        {
            try
            {
                var user = _context.User.Where(p => p.Id == userID).FirstOrDefault();
                user.AccessFailedCount = 0;
                user.BlockDateTime = null;
                user.IsBlock = false;
                var patient = _context.Patients.Where(p => p.UserID == userID).FirstOrDefault();
                if (patient != null)
                {
                    _context.Entry(patient).State = EntityState.Detached;

                }
                _context.User.Update(user);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateStaffActiveStatus(int staffId, bool isActive)
        {
            try
            {
                var userInfo = _context.Staffs.Where(j => j.Id == staffId && j.IsDeleted == false).FirstOrDefault();
                if (userInfo != null)
                {
                    userInfo.IsActive = isActive;
                    _context.Staffs.Update(userInfo);
                    var result = _context.SaveChanges();
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {

                        return false;
                    }
                }
                else
                { return false; }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public IQueryable<T> CheckRecordDepedencies<T>(int Id, string tableName, bool isTableListRequired, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Id",Id),
                    new SqlParameter("@TableName",tableName),
                    new SqlParameter("@IsTableListRequired",isTableListRequired)
                };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_CheckRecordDepedencies.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public void AddIPAddress(IpAddressLog ipAddressLog)
        {
            try
            {
                var ipAddress = _masterContext.IpAddressLog.Where(j => j.UserHost == ipAddressLog.UserHost).FirstOrDefault();
                if (ipAddress == null)
                {
                    ipAddress = new IpAddressLog
                    {
                        UserHost = ipAddressLog.UserHost,
                        HostMachine = ipAddressLog.HostMachine,
                        LocalIP = ipAddressLog.LocalIP,
                        PublicIP = ipAddressLog.PublicIP,
                        Count = 0,
                        CreatedDate = DateTime.UtcNow
                    };
                    _masterContext.Add(ipAddress);
                    _masterContext.SaveChanges();
                }
                else
                {
                    ipAddress.Count += 1;
                    ipAddress.UpdatedDate = DateTime.UtcNow;
                    _masterContext.Update(ipAddress);
                    _masterContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
