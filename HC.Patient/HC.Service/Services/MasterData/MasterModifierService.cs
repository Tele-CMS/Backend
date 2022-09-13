using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.MasterData
{
    public class MasterModifierService : BaseService, IMasterModifierService
    {
        private readonly HCOrganizationContext _context;
        private readonly IMasterModifierRepository _modifierRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        private JsonModel response;
        public MasterModifierService(IMasterModifierRepository modifierRepository, HCOrganizationContext context, IUserCommonRepository userCommonRepository)
        {
            _modifierRepository = modifierRepository;
            _userCommonRepository = userCommonRepository;
            _context = context;
        }
        public JsonModel GetMasterModifiersList(int pageNumber, int pageSize,string modifier,TokenModel token)
        {
            var query = _modifierRepository.GetAll().Select(x => new MasterModifierModel()
            {
                Id = x.Id,
                Modifier = x.Modifier,
                Description = x.Description,
            }).Where(x => (modifier=="" || modifier.Contains(x.Modifier)));

            int count = query.Count();

            List<MasterModifierModel> modifierList = query.Take(pageSize).Skip((pageNumber-1)*pageSize).ToList();
            return new JsonModel
            {
                data = modifierList,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCode.OK,
                meta = new Meta()
                {
                    TotalRecords = count,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    DefaultPageSize = pageSize,
                    TotalPages = Math.Ceiling(Convert.ToDecimal(count / pageSize))
                },
            };

        }
        public JsonModel CreateMasterModifiers(MasterModifierModel masterModifierModel, TokenModel token)
        {
            MasterModifiers requestObj = null;
            try
            {
                if (!ReferenceEquals(masterModifierModel, null) && masterModifierModel.Id == 0)
                {
                    requestObj = new MasterModifiers();
                    AutoMapper.Mapper.Map(masterModifierModel, requestObj);
                    requestObj.CreatedBy = token.UserID;
                    requestObj.OrganizationID = token.OrganizationID;
                    requestObj.CreatedDate = DateTime.UtcNow;
                    requestObj.IsActive = true;
                    requestObj.IsDeleted = false;
                    _modifierRepository.Create(requestObj);
                    _modifierRepository.SaveChanges();
                    response = new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.ModifierAdded,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                else
                {
                    requestObj = _modifierRepository.Get(x => x.Id == masterModifierModel.Id && x.IsActive == true && x.IsDeleted == false);
                    if (requestObj != null)
                    {
                        requestObj.Description = masterModifierModel.Description;
                        requestObj.UpdatedBy = token.UserID;
                        requestObj.UpdatedDate = DateTime.UtcNow;
                        _modifierRepository.Update(requestObj);
                        _modifierRepository.SaveChanges();
                    }
                    response = new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.ModifierUpdated,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                return response;
            }
            catch
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel GetMasterModifierDetail(int modifierId)
        {
            try
            {
                MasterModifierModel masterModifierModel = new MasterModifierModel();
                if (modifierId > 0)
                {
                    masterModifierModel = _modifierRepository.GetAll(x => x.Id == modifierId && x.IsActive == true && x.IsDeleted == false).Select(y => new MasterModifierModel()
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Modifier = y.Modifier,
                    }).FirstOrDefault();
                }
                return response = new JsonModel()
                {
                    data = masterModifierModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel DeleteMasterModifier(int modifierId, TokenModel token)
        {
            try
            {
                RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(modifierId, DatabaseTables.MasterModifiers, false, token).FirstOrDefault();
                if (recordDependenciesModel == null || (recordDependenciesModel != null && recordDependenciesModel.TotalCount == 0))
                {
                    MasterModifiers masterModifiers = _modifierRepository.Get(x => x.Id == modifierId && x.IsActive == true && x.IsDeleted == false);
                    if (!ReferenceEquals(masterModifiers, null))
                    {
                        masterModifiers.IsDeleted = true;
                        masterModifiers.DeletedBy = token.UserID;
                        masterModifiers.DeletedDate = DateTime.UtcNow;
                        _modifierRepository.Update(masterModifiers);
                        response = new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.ModifierDelete,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    else
                    {
                        response = new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.ModifierNotExists,
                            StatusCode = (int)HttpStatusCodes.OK
                        };
                    }
                    return response;
                }
                else
                {
                    return response = new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.AlreadyExists,
                        StatusCode = (int)HttpStatusCodes.Unauthorized
                    };
                }
            }
            catch
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
    }
}
