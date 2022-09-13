using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Patient.Service.IServices.Payer;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payer
{
    public class PayerServiceCodesService : BaseService, IPayerServiceCodesService
    {
        private readonly IPayerServiceCodesRepository _payerServiceCodesRepository;
        private readonly IPayerActivityRepository _payerActivityRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = null;

        public PayerServiceCodesService(IPayerServiceCodesRepository payerServiceCodesRepository, IUserCommonRepository userCommonRepository, IPayerActivityRepository payerActivityRepository)
        {
            _payerServiceCodesRepository = payerServiceCodesRepository;
            _userCommonRepository = userCommonRepository;
            _payerActivityRepository = payerActivityRepository;
            response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
        }

        public JsonModel AddUpdatePayerServiceCode(PayerServiceCodesModel payerServiceCodes, TokenModel token)
        {
            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };

            try
            {
                PayerServiceCodes alreadyExist = _payerServiceCodesRepository.Get(l => l.ServiceCodeId == payerServiceCodes.ServiceCodeId && l.Id != payerServiceCodes.Id && l.IsDeleted == false && l.IsActive == true && l.PayerId == payerServiceCodes.PayerId);
                if (alreadyExist != null)// if service code already exist
                {
                    Result.data = new object();
                    Result.Message = StatusMessage.ServiceCodeAlreadyExist;
                    Result.StatusCode = (int)HttpStatusCodes.UnprocessedEntity;
                }
                else
                {
                    DateTime CurrentDate = DateTime.UtcNow;
                    PayerServiceCodes serviceCode = null;
                    PayerServiceCodeModifiers codeModifiers = null;
                    if (payerServiceCodes.Id == 0)
                    {
                        serviceCode = new PayerServiceCodes();
                        AutoMapper.Mapper.Map(payerServiceCodes, serviceCode);
                        serviceCode.PayerServiceCodeModifiers = new List<PayerServiceCodeModifiers>();
                        serviceCode.CreatedBy = token.UserID;
                        serviceCode.CreatedDate = CurrentDate;
                        serviceCode.IsDeleted = false;
                        if (payerServiceCodes.PayerModifierModel != null && payerServiceCodes.PayerModifierModel.Count > 0)
                        {
                            foreach (var item in payerServiceCodes.PayerModifierModel.Where(x => x.Id == 0 && !string.IsNullOrWhiteSpace(x.Modifier)))
                            {
                                codeModifiers = new PayerServiceCodeModifiers();
                                AutoMapper.Mapper.Map(item, codeModifiers);
                                codeModifiers.CreatedBy = token.UserID;
                                codeModifiers.CreatedDate = CurrentDate;
                                codeModifiers.IsActive = true;
                                codeModifiers.IsDeleted = false;
                                serviceCode.PayerServiceCodeModifiers.Add(codeModifiers);
                            }
                        }
                        _payerServiceCodesRepository.AddPayerServiceCode(serviceCode);
                        if (serviceCode.Id != 0)
                        {
                            Result.data = serviceCode;
                            Result.Message = StatusMessage.PayerServiceCodeAdded;
                            Result.StatusCode = (int)HttpStatusCodes.OK;
                        }
                    }
                    else
                    {
                        serviceCode = _payerServiceCodesRepository.GetPayerServiceCodeByID(payerServiceCodes.Id, token);
                        AutoMapper.Mapper.Map(payerServiceCodes, serviceCode);
                        if (payerServiceCodes.PayerModifierModel != null && payerServiceCodes.PayerModifierModel.Count > 0)
                        {

                            foreach (var item in payerServiceCodes.PayerModifierModel)
                            {

                                if (serviceCode.PayerServiceCodeModifiers.Any(x => x.IsDeleted == false && x.Id == item.Id && x.CreatedDate != CurrentDate))
                                {
                                    var Type = serviceCode.PayerServiceCodeModifiers.Where(x => x.IsDeleted == false && x.Id == item.Id && x.CreatedDate != CurrentDate).Single();
                                    AutoMapper.Mapper.Map(item, Type);
                                    Type.UpdatedBy = token.UserID;
                                    Type.UpdatedDate = CurrentDate;
                                }
                                else
                                {
                                    codeModifiers = new PayerServiceCodeModifiers();
                                    AutoMapper.Mapper.Map(item, codeModifiers);
                                    codeModifiers.CreatedBy = token.UserID;
                                    codeModifiers.CreatedDate = CurrentDate;
                                    codeModifiers.IsDeleted = false;
                                    serviceCode.PayerServiceCodeModifiers.Add(codeModifiers);
                                }
                            }
                            serviceCode.PayerServiceCodeModifiers
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
                        _payerServiceCodesRepository.UpdatePayerServiceCode(serviceCode, CurrentDate);
                        //response
                        Result.data = true;
                        Result.Message = StatusMessage.PayerServiceCodeUpdated;
                        Result.StatusCode = (int)HttpStatusCodes.OK;
                    }
                }
            }
            finally
            {

            }
            return Result;
        }

        public JsonModel DeletePayerServiceCode(int payerServiceCodeId, TokenModel token)
        {
            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };
            try
            {
                RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(payerServiceCodeId, DatabaseTables.PayerServiceCodes, false, token).FirstOrDefault();
                if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0 && recordDependenciesModel.TableName.ToLower() != DatabaseTables.PayerServiceCodeModifiers.ToLower())
                {

                    Result.Message = StatusMessage.AlreadyExists;
                    Result.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Already Exists)
                    Result.data = new object();
                }
                else
                {
                    DateTime CurrentDate = DateTime.UtcNow;
                    PayerServiceCodes payerServiceCode = _payerServiceCodesRepository.GetPayerServiceCodeByID(payerServiceCodeId, token);
                    if (payerServiceCode != null)
                    {
                        payerServiceCode.IsDeleted = true;
                        payerServiceCode.DeletedBy = token.UserID;
                        payerServiceCode.DeletedDate = CurrentDate;
                        foreach (var item in payerServiceCode.PayerServiceCodeModifiers.Where(x => x.IsDeleted == false))
                        {
                            item.DeletedBy = token.UserID;
                            item.DeletedDate = CurrentDate;
                            item.IsDeleted = true;
                        }

                        _payerServiceCodesRepository.UpdatePayerServiceCode(payerServiceCode, CurrentDate);

                        Result.data = new object();
                        Result.Message = StatusMessage.PayerServiceCodeDeleted;
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

        public JsonModel CheckPayerServiceCodeModifierDependency(int payerModifierId, TokenModel token)
        {
            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };
            try
            {
                RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(payerModifierId, DatabaseTables.PayerServiceCodeModifiers, false, token).FirstOrDefault();
                if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
                {

                    Result.Message = StatusMessage.AlreadyExists;
                    Result.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Already Exists)
                    Result.data = true;
                }
                else
                {
                    Result.Message = "SUCCESS";
                    Result.StatusCode = (int)HttpStatusCodes.OK;
                    Result.data = true;
                }
            }
            finally
            {

            }
            return Result;
        }

        public JsonModel GetPayerServiceCodeById(int payerServiceCodeId, TokenModel token)
        {
            JsonModel Result = new JsonModel()
            {
                data = false,
                Message = StatusMessage.ServerError,
                StatusCode = (int)HttpStatusCodes.InternalServerError
            };

            try
            {
                PayerServiceCodes payerServiceCode = _payerServiceCodesRepository.GetPayerServiceCodeByID(payerServiceCodeId, token);
                if (payerServiceCode != null)
                {
                    PayerServiceCodesModel payerServiceCodeModel = new PayerServiceCodesModel();
                    AutoMapper.Mapper.Map(payerServiceCode, payerServiceCodeModel);

                    payerServiceCodeModel.PayerModifierModel = _payerServiceCodesRepository.GetPayerServiceCodeModifiers<PayerModifierModel>(payerServiceCodeId, token).ToList();  //payerServiceCode.PayerServiceCodeModifiers.Where(z => z.IsDeleted == false && z.IsActive == true).Select(a => new PayerModifierModel { Id = a.Id, Modifier = a.Modifier, Rate = a.Rate, PayerServiceCodeId = a.PayerServiceCodeId, Value = a.Modifier, Key = "PSCM" }).ToList();

                    Result.data = payerServiceCodeModel;
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

        public JsonModel SavePayerServiceCode(List<PayerServiceCodesModel> payerServiceCodesModels, TokenModel tokenModel)
        {
            XElement headerElement = null;
            if (payerServiceCodesModels != null && payerServiceCodesModels.Count > 0)
            {
                headerElement = new XElement("Parent");
                payerServiceCodesModels.ForEach(x =>
                {
                    if (x.Id > 0)
                    {
                        headerElement.Add(new XElement("Child",
                            new XElement("Id", x.Id),
                            new XElement("UserId", tokenModel.UserID),                            
                            new XElement("Description", !string.IsNullOrEmpty(x.Description) ? x.Description : ""),
                            new XElement("ServiceCodeId", x.ServiceCodeId),
                            new XElement("IsBillable", x.IsBillable),
                            new XElement("PayerId", x.PayerId),                            
                            new XElement("RatePerUnit", x.RatePerUnit),
                            new XElement("UnitDuration", x.UnitDuration),
                            new XElement("UnitType", x.UnitType),
                            new XElement("IsRequiredAuthorization", x.IsRequiredAuthorization),
                            //new XElement("EffectiveDate", x.EffectiveDate),
                            //new XElement("NewRatePerUnit", x.NewRatePerUnit),
                            new XElement("RuleID", x.RuleID)
                            ));
                    }
                    else
                    {
                        headerElement.Add(new XElement("Child",
                         new XElement("Id", 0),
                         new XElement("UserId", tokenModel.UserID),                         
                         new XElement("Description", x.Description),
                         new XElement("IsBillable", x.IsBillable),
                         new XElement("PayerId", x.PayerId),
                         new XElement("RatePerUnit", x.RatePerUnit),
                         new XElement("UnitDuration", x.UnitDuration),
                         new XElement("UnitType", x.UnitType),
                         new XElement("ServiceCodeId", x.ServiceCodeId),
                         new XElement("IsRequiredAuthorization", x.IsRequiredAuthorization),
                         //new XElement("EffectiveDate", x.EffectiveDate),
                         //new XElement("NewRatePerUnit", x.NewRatePerUnit),
                         new XElement("RuleID", x.RuleID)
                         ));
                    }
                });
                SQLResponseModel sqlResponse = _payerServiceCodesRepository.SavePayerServiceCode<SQLResponseModel>(headerElement).FirstOrDefault();
                return new JsonModel(new object(), sqlResponse.Message, sqlResponse.StatusCode);
            }
            return response;
        }

        public JsonModel PayerOrMasterServiceCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<PayerOrMasterServiceCodesModel> payerServiceCodes = _payerServiceCodesRepository.GetPayerServiceCodes<PayerOrMasterServiceCodesModel>(searchFilterModel, tokenModel).ToList();
            if (payerServiceCodes != null && payerServiceCodes.Count>0)            
            {
                response = new JsonModel(payerServiceCodes, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(payerServiceCodes, searchFilterModel);
            }
            return response;
        }

        public JsonModel GetExcludedServiceCodesFromActivity(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterDropDown> result = _payerServiceCodesRepository.GetExcludedServiceCodesFromActivity<MasterDropDown>(searchFilterModel, tokenModel).ToList();
            if (result != null && result.Count > 0)
            {
                response = new JsonModel(result, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                response.meta = new Meta(result, searchFilterModel);
            }
            return response;
        }

        public JsonModel GetPayerActivityServiceCodeDetailsById(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel token)
        {
            PayerServiceCodeDetailModel result = _payerServiceCodesRepository.GetPayerActivityServiceCodeDetailsById<PayerServiceCodeDetailModel>(payerAppointmentTypeId, payerServiceCodeId, token).FirstOrDefault();
            if (result != null)
            {
                result.PayerModifierModel = _payerActivityRepository.GetPayerServiceCodeModifiers(result.Id).Select(z => new PayerModifierModel { Id = z.Id, Modifier = z.Modifier, Rate = z.Rate, PayerServiceCodeId = z.PayerServiceCodeId, Value = z.Modifier, Key = "PSCM" }).ToList();

                return new JsonModel()
                {
                    data = result,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        public JsonModel GetMasterServiceCodeExcludedFromPayerServiceCodes(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterDropDown> result = _payerServiceCodesRepository.GetMasterServiceCodeExcludedFromPayerServiceCodes<MasterDropDown>(searchFilterModel, tokenModel).ToList();
            if (result != null && result.Count > 0)
            {
                response = new JsonModel(result, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                response.meta = new Meta(result, searchFilterModel);
            }
            return response;
        }
    }
}
