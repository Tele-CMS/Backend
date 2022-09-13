using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.User;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.User
{
    public class UserRoleService : BaseService, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        public UserRoleService(IUserRoleRepository userRoleRepository, IUserCommonRepository userCommonRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetRoles(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<UserRoleModel> userRoleModels = _userRoleRepository.GetRoles<UserRoleModel>(searchFilterModel, tokenModel).ToList();
            if (userRoleModels != null && userRoleModels.Count > 0)
            {
                response = new JsonModel(userRoleModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(userRoleModels, searchFilterModel);
            }
            return response;
        }
        public JsonModel SaveRole(UserRoleModel userRoleModel, TokenModel tokenModel, Boolean isActive = true)
        {
            UserRoles userRoles = null;
            if (userRoleModel.Id == 0)
            {
                userRoles = _userRoleRepository.Get(l => l.RoleName == userRoleModel.RoleName && l.IsDeleted == false && l.OrganizationID == tokenModel.OrganizationID);
                if (userRoles != null)//duplicate check on new insertion
                {
                    response = new JsonModel(userRoles, StatusMessage.UserRoleAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //insert new
                {
                    userRoles = new UserRoles();
                    AutoMapper.Mapper.Map(userRoleModel, userRoles);
                    userRoles.OrganizationID = tokenModel.OrganizationID;
                    userRoles.IsActive = isActive;
                    //userRoles.CreatedBy = tokenModel.UserID;
                    //userRoles.CreatedDate = DateTime.UtcNow;
                    userRoles.IsDeleted = false;
                    _userRoleRepository.Create(userRoles);
                    _userRoleRepository.SaveChanges();
                    response = new JsonModel(userRoles, StatusMessage.RoleSave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                userRoles = _userRoleRepository.Get(l => l.RoleName == userRoleModel.RoleName && l.Id != userRoleModel.Id && l.IsDeleted == false && l.OrganizationID == tokenModel.OrganizationID);
                if (userRoles != null) //duplicate check
                {
                    response = new JsonModel(userRoles, StatusMessage.UserRoleAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //update existing
                {
                    userRoles = _userRoleRepository.Get(a => a.Id == userRoleModel.Id && a.IsDeleted == false);
                    if (userRoles != null)
                    {
                        AutoMapper.Mapper.Map(userRoleModel, userRoles);
                        //userRoles.UpdatedBy = tokenModel.UserID;
                        //userRoles.UpdatedDate = DateTime.UtcNow;
                        _userRoleRepository.Update(userRoles);
                        _userRoleRepository.SaveChanges();
                        response = new JsonModel(userRoles, StatusMessage.RoleUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }
        public JsonModel GetRoleById(int id, TokenModel tokenModel)
        {
            UserRoles userRoles = _userRoleRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (userRoles != null)
            {
                UserRoleModel userRoleModel = new UserRoleModel();
                AutoMapper.Mapper.Map(userRoles, userRoleModel);
                response = new JsonModel(userRoleModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel DeleteRole(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.UserRoles, false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                UserRoles userRoles = _userRoleRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
                if (userRoles != null)
                {
                    userRoles.IsDeleted = true;
                    userRoles.DeletedBy = tokenModel.UserID;
                    userRoles.DeletedDate = DateTime.UtcNow;
                    _userRoleRepository.Update(userRoles);
                    _userRoleRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.RoleDeleted, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }

        public JsonModel GetLoggedinUserInfo(TokenModel tokenModel)
        {
            var roleId = tokenModel.RoleID;
            UserRoles userRoles = _userRoleRepository.Get(a => a.Id == roleId && a.IsDeleted == false);
            if (userRoles != null)
            {
                UserRoleModel userRoleModel = new UserRoleModel();
                AutoMapper.Mapper.Map(userRoles, userRoleModel);

                LoggedInUserModel user = new LoggedInUserModel()
                {
                    RoleId = tokenModel.RoleID,
                    StaffId = tokenModel.StaffID,
                    UserType = userRoleModel.UserType
                };
                response = new JsonModel(user, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}
