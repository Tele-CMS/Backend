using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Patient.Service.Payer.Interfaces;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Payer
{
    public class PayerInformationService : BaseService, IPayerInformationService
    {
        #region Global Variables
        private readonly IPayerInformationRepository _payerServiceCodesRepository;
        private readonly IPayerActivityRepository _payerActivityRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        public readonly IUserCommonRepository _userCommonRepository;
        #endregion
        public PayerInformationService(IPayerInformationRepository payerServiceCodesRepository, IPayerActivityRepository payerActivityRepository, IAuditLogRepository auditLogRepository, IUserCommonRepository userCommonRepository)
        {
            this._payerServiceCodesRepository = payerServiceCodesRepository;
            this._payerActivityRepository = payerActivityRepository;
            _auditLogRepository = auditLogRepository;
            _userCommonRepository = userCommonRepository;
        }


        public List<PayerServiceCodes> UpdatePayerServiceCodesToDB(List<PayerServiceCodes> PayerServiceCodes, bool IsInsert = false, bool IsUpdate = false, bool IsDelete = false)
        {
            return _payerServiceCodesRepository.UpdatePayerServiceCodesToDB(PayerServiceCodes, IsInsert, IsUpdate, IsDelete);

        }

        public JsonModel GetPayerInformationByFilter(int organizationID, string payerName, int pageNumber, int pageSize, string sortColumn, string sortOrder, bool IsPayerInformation)
        {
            try
            {
                PayerSearchFilter payerSearchFilter = new PayerSearchFilter();
                payerSearchFilter.OrganizationID = organizationID;
                payerSearchFilter.Name = payerName;
                payerSearchFilter.PageNumber = pageNumber;
                payerSearchFilter.PageSize = pageSize;
                payerSearchFilter.SortColumn = sortColumn;
                payerSearchFilter.SortOrder = sortOrder;
                payerSearchFilter.IsPayerInformation = IsPayerInformation;
                List<PayerInformationModel> result = _payerServiceCodesRepository.GetPayerInformationByFilter(payerSearchFilter);
                return new JsonModel()
                {
                    data = result,
                    meta = new Meta(result, payerSearchFilter),
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
            //return 
        }

        public JsonModel GetPayerInformationByFilter(PayerSearchFilter payerSearchFilter)
        {
            var result = _payerServiceCodesRepository.GetPayerInformationByFilter(payerSearchFilter);
            return new JsonModel()
            {
                data = result,
                meta = new Meta()
                {
                    TotalRecords = result != null && result.Count > 0 ? Convert.ToDecimal(result[0].TotalRecords) : 0,
                    CurrentPage = Convert.ToInt32(payerSearchFilter.PageNumber),
                    PageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                    DefaultPageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                    TotalPages = Math.Ceiling(Convert.ToDecimal((result != null && result.Count > 0 ? result[0].TotalRecords : 0) / payerSearchFilter.PageSize))
                },
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public List<PayerInformationModel> GetMasterServiceCodesByFilter(PayerSearchFilter payerSearchFilter)
        {
            return _payerServiceCodesRepository.GetMasterServiceCodesByFilter(payerSearchFilter);
        }
        public List<PayerInformationModel> GetPayerActivityByFilter(PayerSearchFilter payerSearchFilter)
        {
            return _payerServiceCodesRepository.GetPayerActivityByFilter(payerSearchFilter);
        }
        public List<PayerAppTypeModel> GetPayerServiceCodesByFilter(PayerSearchFilter payerSearchFilter)
        {
            return _payerServiceCodesRepository.GetPayerServiceCodesByFilter(payerSearchFilter);
        }

        public List<PayerInformationModel> UpdatePayerInformation(PayerInfoUpdateModel payerInfoUpdateModel)
        {
            return _payerServiceCodesRepository.UpdatePayerInformation(payerInfoUpdateModel);

        }

        public JsonModel GetPayerServiceCodesByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID)
        {
            try
            {
                PayerSearchFilter payerSearchFilter = new PayerSearchFilter();
                payerSearchFilter.ID = id;
                payerSearchFilter.ID2 = id2;
                payerSearchFilter.Name = name;
                payerSearchFilter.PageNumber = pageNumber;
                payerSearchFilter.PageSize = pageSize;
                payerSearchFilter.SortColumn = sortColumn;
                payerSearchFilter.SortOrder = sortOrder;
                payerSearchFilter.OrganizationID = organizationID;
                var result = _payerServiceCodesRepository.GetPayerServiceCodesByFilter(payerSearchFilter);

                if (payerSearchFilter.PageSize == 0)
                {
                    payerSearchFilter.PageSize = result.Count;
                }

                return new JsonModel()
                {
                    data = result,
                    meta = new Meta()
                    {
                        TotalRecords = result != null && result.Count > 0 ? Convert.ToDecimal(result[0].TotalRecords) : 0,
                        CurrentPage = Convert.ToInt32(payerSearchFilter.PageNumber),
                        PageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        DefaultPageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        TotalPages = Math.Ceiling(Convert.ToDecimal((result != null && result.Count > 0 ? result[0].TotalRecords : 0) / payerSearchFilter.PageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return null;
            }
        }

        public JsonModel GetPayerActivityByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID)
        {
            try
            {
                PayerSearchFilter payerSearchFilter = new PayerSearchFilter();
                payerSearchFilter.ID = id;
                payerSearchFilter.ID2 = id2;
                payerSearchFilter.Name = name;
                payerSearchFilter.PageNumber = pageNumber;
                payerSearchFilter.PageSize = pageSize;
                payerSearchFilter.SortColumn = sortColumn;
                payerSearchFilter.SortOrder = sortOrder;
                payerSearchFilter.OrganizationID = organizationID;
                payerSearchFilter.IsPayerActivity = true;
                var result = _payerServiceCodesRepository.GetPayerActivityByFilter(payerSearchFilter);

                if (payerSearchFilter.PageSize == 0)
                {
                    payerSearchFilter.PageSize = result.Count;
                }

                return new JsonModel()
                {
                    data = result,
                    meta = new Meta()
                    {
                        TotalRecords = result != null && result.Count > 0 ? Convert.ToDecimal(result[0].TotalRecords) : 0,
                        CurrentPage = Convert.ToInt32(payerSearchFilter.PageNumber),
                        PageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        DefaultPageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        TotalPages = Math.Ceiling(Convert.ToDecimal((result != null && result.Count > 0 ? result[0].TotalRecords : 0) / payerSearchFilter.PageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return null;
            }
        }

        public JsonModel GetPayerActivityCodesByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID)
        {
            try
            {
                PayerSearchFilter payerSearchFilter = new PayerSearchFilter();
                payerSearchFilter.ID = id;
                payerSearchFilter.ID2 = id2;
                payerSearchFilter.Name = name;
                payerSearchFilter.PageNumber = pageNumber;
                payerSearchFilter.PageSize = pageSize;
                payerSearchFilter.SortColumn = sortColumn;
                payerSearchFilter.SortOrder = sortOrder;
                payerSearchFilter.OrganizationID = organizationID;
                payerSearchFilter.IsPayerActivityCode = true;
                var result = _payerServiceCodesRepository.GetPayerActivityByFilter(payerSearchFilter);

                if (payerSearchFilter.PageSize == 0)
                {
                    payerSearchFilter.PageSize = result.Count;
                }
                return new JsonModel()
                {
                    data = result,
                    meta = new Meta()
                    {
                        TotalRecords = result != null && result.Count > 0 ? Convert.ToDecimal(result[0].TotalRecords) : 0,
                        CurrentPage = Convert.ToInt32(payerSearchFilter.PageNumber),
                        PageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        DefaultPageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        TotalPages = Math.Ceiling(Convert.ToDecimal((result != null && result.Count > 0 ? result[0].TotalRecords : 0) / payerSearchFilter.PageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return null;
            }
        }

        public JsonModel GetMasterServiceCodesByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID)
        {
            try
            {
                PayerSearchFilter payerSearchFilter = new PayerSearchFilter();
                payerSearchFilter.ID = id;
                payerSearchFilter.ID2 = id2;
                payerSearchFilter.Name = name;
                payerSearchFilter.PageNumber = pageNumber;
                payerSearchFilter.PageSize = pageSize;
                payerSearchFilter.SortColumn = sortColumn;
                payerSearchFilter.SortOrder = sortOrder;
                payerSearchFilter.OrganizationID = organizationID;
                var result = _payerServiceCodesRepository.GetMasterServiceCodesByFilter(payerSearchFilter);

                if (payerSearchFilter.PageSize == 0)
                {
                    payerSearchFilter.PageSize = result.Count;
                }
                return new JsonModel()
                {
                    data = result,
                    meta = new Meta()
                    {
                        TotalRecords = result != null && result.Count > 0 ? Convert.ToDecimal(result[0].TotalRecords) : 0,
                        CurrentPage = Convert.ToInt32(payerSearchFilter.PageNumber),
                        PageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        DefaultPageSize = Convert.ToInt32(payerSearchFilter.PageSize),
                        TotalPages = Math.Ceiling(Convert.ToDecimal((result != null && result.Count > 0 ? result[0].TotalRecords : 0) / payerSearchFilter.PageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return null;
            }
        }

        public JsonModel GetMasterServiceCodesEx(int payerID, int organizationID)
        {
            try
            {
                var result = _payerServiceCodesRepository.GetMasterServiceCodesEx(payerID, organizationID);
                return new JsonModel()
                {
                    data = result,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return null;
            }
        }

        public JsonModel GetPayerServiceCodesByPayerId(int payerID, int organizationID)
        {
            try
            {
                var result = _payerServiceCodesRepository.GetPayerServiceCodesByPayerId(payerID, organizationID);
                return new JsonModel()
                {
                    data = result,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return null;
            }
        }


        public JsonModel GetPayerServiceCodesEx(int payerID, int activityID, int organizationID)
        {
            try
            {
                var result = _payerServiceCodesRepository.GetPayerServiceCodesEx(payerID, activityID, organizationID);
                return new JsonModel()
                {
                    data = result,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {

                return null;
            }
        }

        public JsonModel GetPayerServiceCodeDetailsById(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel token)
        {
            PayerServiceCodeDetailModel result = _payerServiceCodesRepository.GetPayerServiceCodeDetailsById<PayerServiceCodeDetailModel>(payerAppointmentTypeId, payerServiceCodeId, token).FirstOrDefault();
            if (result != null)
            {
                result.PayerModifierModel = _payerActivityRepository.GetPayerServiceCodeModifiers(result.Id).Select(z => new PayerModifierModel { Id = z.Id, Modifier = z.Modifier, PayerServiceCodeId = z.PayerServiceCodeId, Rate = z.Rate, Value = z.Modifier }).ToList();

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

        public JsonModel UpdatePayerAppointmentTypes(PayerAppointmentTypesModel payerAppointmentTypesModel, TokenModel token)
        {
            try
            {
                PayerAppointmentTypes payerAppointmentType = _payerActivityRepository.GetByID(payerAppointmentTypesModel.Id);
                
                payerAppointmentType.RatePerUnit = payerAppointmentTypesModel.RatePerUnit;
                payerAppointmentType.Modifier1 = payerAppointmentTypesModel.Modifier1;
                payerAppointmentType.Modifier2 = payerAppointmentTypesModel.Modifier2;
                payerAppointmentType.Modifier3 = payerAppointmentTypesModel.Modifier3;
                payerAppointmentType.Modifier4 = payerAppointmentTypesModel.Modifier4;

                _payerActivityRepository.Update(payerAppointmentType);
                _payerActivityRepository.SaveChanges();


                return new JsonModel()
                {
                    data = payerAppointmentType,
                    Message = StatusMessage.PayerActivityUpdated,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }



        }

        public JsonModel DeletePayerServiceCode(int id, TokenModel token)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, typeof(PayerServiceCodes).Name, false, token).FirstOrDefault();
            if (recordDependenciesModel == null || (recordDependenciesModel != null && recordDependenciesModel.TotalCount == 0))
            {

                PayerServiceCodes payerServiceCode = _payerServiceCodesRepository.Get(a => a.IsActive == true && a.IsDeleted == false && a.Id == id);
                if (payerServiceCode != null)
                {
                    payerServiceCode.IsDeleted = true;
                    payerServiceCode.DeletedBy = token.UserID;
                    payerServiceCode.DeletedDate = DateTime.UtcNow;
                    _payerServiceCodesRepository.Update(payerServiceCode);
                    //_payerServiceCodesRepository.SaveChanges();

                    //audit logs
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdatePayerServiceCodes, AuditLogAction.Modify, null, token.UserID, "", token);

                    //return
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.ServiceCodeDelete,
                        StatusCode = (int)HttpStatusCodes.OK,
                    };
                }
                else
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.NotFound,
                        StatusCode = (int)HttpStatusCodes.NotFound,
                    };
                }
            }
            else { return new JsonModel() { data = new object(), Message = StatusMessage.AlreadyExists, StatusCode = (int)HttpStatusCodes.Unauthorized }; }
        }

        public JsonModel GetMasterActivitiesForPayer(int payerId, TokenModel token)
        {

            List<MasterActivitiesModel> result = _payerServiceCodesRepository.GetMasterActivitiesForPayer<MasterActivitiesModel>(payerId, token).ToList();
            return new JsonModel()
            {
                data = result,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
    }
}