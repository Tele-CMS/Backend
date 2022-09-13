using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.MasterServiceCodes;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.MasterServiceCodes;
using HC.Patient.Service.IServices.MasterServiceCodes;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.MasterServiceCodes
{
    public class MasterServiceCodesService : BaseService, IMasterServiceCodesService
    {
        private readonly IMasterServiceCodesRepository _masterServiceCodesRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        public MasterServiceCodesService(IMasterServiceCodesRepository masterServiceCodesRepository, IUserCommonRepository userCommonRepository)
        {
            _masterServiceCodesRepository = masterServiceCodesRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel AddUpdateMasterServiceCode(MasterServiceCodesModel masterServiceCodes, TokenModel token)
        {
            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };

            try
            {
                MasterServiceCode alreadyExist = _masterServiceCodesRepository.Get(l => l.ServiceCode == masterServiceCodes.ServiceCode && l.Id != masterServiceCodes.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == token.OrganizationID);
                if (alreadyExist != null)// if service code already exist
                {
                    Result.data = new object();
                    Result.Message = StatusMessage.ServiceCodeAlreadyExist;
                    Result.StatusCode = (int)HttpStatusCodes.UnprocessedEntity;
                }
                else
                {
                    DateTime CurrentDate = DateTime.UtcNow;
                    MasterServiceCode serviceCode = null;
                    MasterServiceCodeModifiers codeModifiers = null;
                    if (masterServiceCodes.Id == 0)
                    {
                        serviceCode = new MasterServiceCode();
                        AutoMapper.Mapper.Map(masterServiceCodes, serviceCode);
                        serviceCode.OrganizationID = token.OrganizationID;
                        serviceCode.MasterServiceCodeModifiers = new List<MasterServiceCodeModifiers>();
                        serviceCode.CreatedBy = token.UserID;
                        serviceCode.CreatedDate = CurrentDate;
                        serviceCode.IsDeleted = false;
                        if (masterServiceCodes.ModifierModel != null && masterServiceCodes.ModifierModel.Count > 0)
                        {
                            foreach (var item in masterServiceCodes.ModifierModel.Where(x => x.ModifierID == 0 && !string.IsNullOrWhiteSpace(x.Modifier)))
                            {
                                codeModifiers = new MasterServiceCodeModifiers();
                                AutoMapper.Mapper.Map(item, codeModifiers);
                                codeModifiers.CreatedBy = token.UserID;
                                codeModifiers.CreatedDate = CurrentDate;
                                codeModifiers.IsActive = true;
                                codeModifiers.IsDeleted = false;
                                serviceCode.MasterServiceCodeModifiers.Add(codeModifiers);
                            }
                        }
                        _masterServiceCodesRepository.AddMasterServiceCode(serviceCode);
                        if (serviceCode.Id != 0)
                        {
                            Result.data = serviceCode;
                            Result.Message = StatusMessage.MasterServiceCodeAdded;
                            Result.StatusCode = (int)HttpStatusCodes.OK;
                        }
                    }
                    else
                    {
                        serviceCode = _masterServiceCodesRepository.GetServiceCodeByID(masterServiceCodes.Id, token);
                        AutoMapper.Mapper.Map(masterServiceCodes, serviceCode);
                        if (masterServiceCodes.ModifierModel != null && masterServiceCodes.ModifierModel.Count > 0)
                        {

                            foreach (var item in masterServiceCodes.ModifierModel)
                            {

                                if (serviceCode.MasterServiceCodeModifiers.Any(x => x.IsDeleted == false && x.Id == item.ModifierID && x.CreatedDate != CurrentDate))
                                {
                                    var Type = serviceCode.MasterServiceCodeModifiers.Where(x => x.IsDeleted == false && x.Id == item.ModifierID && x.CreatedDate != CurrentDate).Single();
                                    AutoMapper.Mapper.Map(item, Type);
                                    Type.UpdatedBy = token.UserID;
                                    Type.UpdatedDate = CurrentDate;
                                }
                                else
                                {
                                    codeModifiers = new MasterServiceCodeModifiers();
                                    AutoMapper.Mapper.Map(item, codeModifiers);
                                    codeModifiers.CreatedBy = token.UserID;
                                    codeModifiers.CreatedDate = CurrentDate;
                                    codeModifiers.IsDeleted = false;
                                    serviceCode.MasterServiceCodeModifiers.Add(codeModifiers);
                                }
                            }
                            serviceCode.MasterServiceCodeModifiers
                                    .Where(x => x.CreatedDate != CurrentDate && x.UpdatedDate != CurrentDate)
                                    .ToList().ForEach(x =>
                                    {
                                        x.DeletedBy = token.UserID;
                                        x.DeletedDate = CurrentDate;
                                        x.IsDeleted = true;
                                    });

                        }

                        serviceCode.UpdatedBy = token.UserID;
                        serviceCode.UpdatedDate = CurrentDate;
                        _masterServiceCodesRepository.UpdateServiceCode(serviceCode, CurrentDate);
                        //response
                        Result.data = true;
                        Result.Message = StatusMessage.MasterServiceCodeUpdated;
                        Result.StatusCode = (int)HttpStatusCodes.OK;
                    }
                }
            }
            finally
            {

            }
            return Result;
        }

        public JsonModel GetMasterServiceCodes(string searchText, TokenModel token, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            try
            {
                JsonModel Result = new JsonModel()
                {
                    data = false,
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };

                List<MasterServiceCodesModel> masterServiceCodeListModel = _masterServiceCodesRepository.GetMasterServiceCodes<MasterServiceCodesModel>(searchText, token, pageNumber, pageSize, sortColumn, sortOrder).ToList();
                if (masterServiceCodeListModel != null && masterServiceCodeListModel.Count > 0)
                {
                    return new JsonModel()
                    {
                        data = masterServiceCodeListModel,
                        Message = StatusMessage.FetchMessage,
                        meta = new Meta()
                        {
                            TotalRecords = masterServiceCodeListModel[0].TotalRecords,
                            CurrentPage = pageNumber,
                            PageSize = pageSize,
                            DefaultPageSize = pageSize,
                            TotalPages = Math.Ceiling(Convert.ToDecimal(masterServiceCodeListModel[0].TotalRecords / pageSize))
                        },
                        StatusCode = (int)HttpStatusCodes.OK//(Unprocessable Entity)
                    };
                }
                else { Result.data = new object(); Result.Message = StatusMessage.NotFound; Result.StatusCode = (int)HttpStatusCodes.NotFound; return Result; }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonModel DeleteServiceCode(int serviceCodeId, TokenModel token)
        {
            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };
            try
            {
                RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(serviceCodeId, DatabaseTables.MasterServiceCode, false, token).FirstOrDefault();
                if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0 && recordDependenciesModel.TableName.ToLower() != DatabaseTables.MasterServiceCodeModifiers.ToLower())
                {

                    Result.Message = StatusMessage.AlreadyExists;
                    Result.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Already Exists)
                    Result.data = new object();
                }
                else
                {
                    DateTime CurrentDate = DateTime.UtcNow;
                    MasterServiceCode masterServiceCode = _masterServiceCodesRepository.GetServiceCodeByID(serviceCodeId, token);
                    if (masterServiceCode != null)
                    {
                        masterServiceCode.IsDeleted = true;
                        masterServiceCode.DeletedBy = token.UserID;
                        masterServiceCode.DeletedDate = CurrentDate;
                        foreach (var item in masterServiceCode.MasterServiceCodeModifiers.Where(x => x.IsDeleted == false))
                        {
                            item.DeletedBy = token.UserID;
                            item.DeletedDate = CurrentDate;
                            item.IsDeleted = true;
                        }

                        _masterServiceCodesRepository.UpdateServiceCode(masterServiceCode, CurrentDate);

                        Result.data = new object();
                        Result.Message = StatusMessage.MasterServiceCodeDeleted;
                        Result.StatusCode = (int)HttpStatusCodes.OK;
                    }
                    else
                    {
                        Result.data = new object();
                        Result.Message = StatusMessage.NotFound;
                        Result.StatusCode = (int)HttpStatusCodes.NotFound;
                    }
                }
            }
            finally
            {

            }
            return Result;
        }

        public JsonModel GetMasterServiceCodeById(int serviceCodeId, TokenModel token)
        {
            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };

            try
            {
                MasterServiceCode masterServiceCode = _masterServiceCodesRepository.GetServiceCodeByID(serviceCodeId, token);
                if (masterServiceCode != null)
                {
                    MasterServiceCodesModel masterServiceCodeModel = new MasterServiceCodesModel();
                    AutoMapper.Mapper.Map(masterServiceCode, masterServiceCodeModel);
                    List<ModifierModel> modifierModels = masterServiceCode.MasterServiceCodeModifiers.Where(z => z.IsDeleted == false && z.IsActive == true).Select(a => new ModifierModel { ModifierID = a.Id, Modifier = a.Modifier, Rate = a.Rate, ServiceCodeId = a.ServiceCodeID, Key = "MSCM", IsUsedModifier = false }).ToList();
                    masterServiceCodeModel.ModifierModel = modifierModels;
                    Result.data = masterServiceCodeModel;
                    Result.Message = StatusMessage.FetchMessage;
                    Result.StatusCode = (int)HttpStatusCodes.OK;
                }
                else
                {
                    Result.data = new object();
                    Result.Message = StatusMessage.NotFound;
                    Result.StatusCode = (int)HttpStatusCodes.NotFound;
                }

            }
            catch (Exception e)
            {
                Result.data = new object();
                Result.Message = StatusMessage.ServerError;
                Result.StatusCode = (int)HttpStatusCodes.InternalServerError;
                Result.AppError = e.Message;
            }
            return Result;
        }


    }
}
