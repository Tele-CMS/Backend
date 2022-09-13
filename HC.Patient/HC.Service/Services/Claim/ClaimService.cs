using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.PatientLedger;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.Claim;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.Payment;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.Claim;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Claim
{
    public class ClaimService : BaseService, IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimServiceLineRepository _claimServiceLineRepository;
        private readonly IClaimDiagnosisCodeRepository _claimDiagnosisCodeRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IPatientAddressRepository _patientAddressRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMasterPatientLocationRepository _masterPatientLocationRepository;
        private readonly ILocationService _locationService;

        private JsonModel response;
        private Nullable<int> pId; //The name is just pId (for patientId) because we may need patientId in inner section from controller method
        private readonly IPaymentRepository _paymentRepository;
        XElement inputXML = null;
        public ClaimService(IClaimRepository claimRepository, IClaimServiceLineRepository claimServiceLineRepository, IClaimDiagnosisCodeRepository claimDiagnosisCodeRepository, IAuditLogRepository auditLogRepository, IPaymentRepository paymentRepository, IUserCommonRepository userCommonRepository, IStaffRepository staffRepository, IPatientAddressRepository patientAddressRepository, ILocationRepository locationRepository, IMasterPatientLocationRepository masterPatientLocationRepository, ILocationService locationService)

        {
            _claimRepository = claimRepository;
            _claimServiceLineRepository = claimServiceLineRepository;
            _claimDiagnosisCodeRepository = claimDiagnosisCodeRepository;
            _auditLogRepository = auditLogRepository;
            _userCommonRepository = userCommonRepository;
            _paymentRepository = paymentRepository;
            _staffRepository = staffRepository;
            _patientAddressRepository = patientAddressRepository;
            _locationRepository = locationRepository;
            _masterPatientLocationRepository = masterPatientLocationRepository;
            _locationService = locationService;
        }

        public JsonModel CreateClaim(int patientEncounterId, bool isAdmin, TokenModel token)
        {

            SQLResponseModel response = _claimRepository.CreateClaim<SQLResponseModel>(patientEncounterId, isAdmin, token.UserID, token.OrganizationID).FirstOrDefault();
            return new JsonModel()
            {
                data = new object(),
                Message = response.Message,
                StatusCode = response.StatusCode
            };

        }

        public JsonModel GetClaims(int organizationId, int pageNumber, int pageSize, int? claimId, string lastName, string firstName, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, Nullable<int> claimStatusId)
        {

            List<ClaimModel> listClaims = _claimRepository.GetClaims<ClaimModel>(organizationId, pageNumber, pageSize, claimId, lastName, firstName, fromDate, toDate, payerName, sortColumn, sortOrder, claimStatusId).ToList();
            if (listClaims != null && listClaims.Count > 0)
            {
                listClaims.ForEach(x =>
                {
                    x.ClaimEncounters = XDocument.Parse(x.XmlString).Descendants("Child").Select(y => new ClaimEncountersModel()
                    {
                        PatientEncounterId = Convert.ToInt32(y.Element("PatientEncounterId").Value),
                        DOS = Convert.ToDateTime(y.Element("DOS").Value),
                        StartDateTime = Convert.ToDateTime(y.Element("StartDateTime").Value),
                        EndDateTime = Convert.ToDateTime(y.Element("EndDateTime").Value),
                    }).ToList(); x.XmlString = null;
                });
                return new JsonModel()
                {
                    data = listClaims,
                    meta = new Meta()
                    {
                        TotalRecords = listClaims[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(listClaims[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }

        }

        public JsonModel GetClaimServiceLines(int claimId)
        {

            List<ClaimServiceLineModel> listClaimsServiceLines = _claimRepository.GetClaimServiceLines<ClaimServiceLineModel>(claimId).ToList();
            return new JsonModel()
            {
                data = listClaimsServiceLines,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };

        }
        public JsonModel GetOpenChargesForPatient(int patientId, int payerId, TokenModel token)
        {

            List<ClaimServiceLineModel> listClaimsServiceLines = _claimRepository.GetOpenChargesForPatient<ClaimServiceLineModel>(patientId, payerId, token).ToList();
           return response = new JsonModel(listClaimsServiceLines, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);

        }
        public JsonModel SaveClaimServiceLine(int claimId, ClaimServiceLineModel requestObj, TokenModel token)
        {

            response = new JsonModel();
            ClaimServiceLine claimServiceLine = null;

            pId = GetPatientIdFromClaimId(claimId);
            decimal modifierAmount = 0;
            //  Convert.ToDecimal((requestObj.RateModifier1 != null ? requestObj.RateModifier1 : 0)
            //+ (requestObj.RateModifier2 != null ? requestObj.RateModifier2 : 0)
            //+ (requestObj.RateModifier3 != null ? requestObj.RateModifier3 : 0)
            //+ (requestObj.RateModifier4 != null ? requestObj.RateModifier4 : 0));

            if (requestObj.Id == 0)
            {
                List<ClaimDiagnosisCode> dxCodeList = _claimDiagnosisCodeRepository.GetAll(x => x.ClaimId == claimId && x.IsDeleted == false && x.IsActive == true).ToList();
                claimServiceLine = _claimServiceLineRepository.Get(x => x.ServiceCode == requestObj.ServiceCode.Trim() && x.ClaimId == claimId && x.IsDeleted == false && x.IsActive == true);
                if (ReferenceEquals(claimServiceLine, null))
                {
                    claimServiceLine = new ClaimServiceLine();
                    AutoMapper.Mapper.Map(requestObj, claimServiceLine);
                    claimServiceLine.Rate = Convert.ToDecimal(requestObj.Rate.ToString("0.00"));//requestObj.Rate;
                    claimServiceLine.TotalAmount = Convert.ToDecimal(((requestObj.Quantity * requestObj.Rate) + modifierAmount).ToString("0.00"));
                    claimServiceLine.ClaimId = claimId;
                    claimServiceLine.CreatedBy = token.UserID;
                    claimServiceLine.CreatedDate = DateTime.UtcNow;
                    claimServiceLine.IsDeleted = false;
                    claimServiceLine.IsActive = true;
                    claimServiceLine.DiagnosisCodePointer1 = dxCodeList != null && dxCodeList.Count > 0 ? Convert.ToInt32(dxCodeList[0].Id) : (int?)null;
                    claimServiceLine.DiagnosisCodePointer2 = dxCodeList != null && dxCodeList.Count > 1 ? Convert.ToInt32(dxCodeList[1].Id) : (int?)null;
                    claimServiceLine.DiagnosisCodePointer3 = dxCodeList != null && dxCodeList.Count > 2 ? Convert.ToInt32(dxCodeList[2].Id) : (int?)null;
                    claimServiceLine.DiagnosisCodePointer4 = dxCodeList != null && dxCodeList.Count > 3 ? Convert.ToInt32(dxCodeList[3].Id) : (int?)null;
                    _claimServiceLineRepository.Create(claimServiceLine);
                    _claimServiceLineRepository.SaveChanges();
                    //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.AddServiceLine, AuditLogAction.Create, pId, token.UserID,"ClaimId/CL"+claimId.ToString());
                    inputXML = new XElement("Parent");
                    {
                        inputXML.Add(new XElement("Child",
                            new XElement("ClaimId", claimId),
                            new XElement("ColumnName", "ServiceCode"),
                            new XElement("OldValue", string.Empty),
                            new XElement("NewValue", claimServiceLine.ServiceCode)
                            ));
                    }
                    _claimRepository.SaveClaimHistory<SQLResponseModel>(claimId, inputXML, ClaimHistoryAction.AddServiceLine, DateTime.UtcNow, token).FirstOrDefault();
                    response.data = new object();
                    response.Message = StatusMessage.ServiceCodeAdded;
                    response.StatusCode = (int)HttpStatusCodes.OK;
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.ServiceCodeAlreadyExists;
                    response.StatusCode = (int)HttpStatusCodes.OK;
                }
            }
            else
            {
                claimServiceLine = _claimServiceLineRepository.Get(x => x.Id == requestObj.Id && x.IsDeleted == false && x.IsActive == true);
                if (!ReferenceEquals(claimServiceLine, null))
                {

                    inputXML = GetClaimServiceLineModifications(claimServiceLine, requestObj);
                    claimServiceLine.Quantity = requestObj.Quantity;
                    claimServiceLine.Rate = Convert.ToDecimal(requestObj.Rate.ToString("0.00"));//requestObj.Rate;
                    claimServiceLine.TotalAmount = Convert.ToDecimal(((requestObj.Quantity * requestObj.Rate) + modifierAmount).ToString("0.00"));
                    claimServiceLine.IsBillable = requestObj.IsBillable;
                    claimServiceLine.Modifier1 = string.IsNullOrEmpty(requestObj.Modifier1) ? null : requestObj.Modifier1;
                    claimServiceLine.Modifier2 = string.IsNullOrEmpty(requestObj.Modifier2) ? null : requestObj.Modifier2;
                    claimServiceLine.Modifier3 = string.IsNullOrEmpty(requestObj.Modifier3) ? null : requestObj.Modifier3;
                    claimServiceLine.Modifier4 = string.IsNullOrEmpty(requestObj.Modifier4) ? null : requestObj.Modifier4;
                    claimServiceLine.RateModifier1 = requestObj.RateModifier1;
                    claimServiceLine.RateModifier2 = requestObj.RateModifier2;
                    claimServiceLine.RateModifier3 = requestObj.RateModifier3;
                    claimServiceLine.RateModifier4 = requestObj.RateModifier4;
                    claimServiceLine.ClinicianId = requestObj.ClinicianId;
                    claimServiceLine.RenderingProviderId = requestObj.RenderingProviderId;
                    claimServiceLine.PatientAddressID = requestObj.PatientAddressID;
                    claimServiceLine.OfficeAddressID = requestObj.OfficeAddressID;
                    claimServiceLine.CustomAddressID = requestObj.CustomAddressID;
                    claimServiceLine.CustomAddress = requestObj.CustomAddress;
                    claimServiceLine.UpdatedBy = token.UserID;
                    claimServiceLine.UpdatedDate = DateTime.UtcNow;

                    _claimServiceLineRepository.Update(claimServiceLine);
                    //_claimServiceLineRepository.SaveChanges();
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateServiceLine, AuditLogAction.Modify, pId, token.UserID, "ClaimId/CL" + claimId.ToString(), token);

                    _claimRepository.SaveClaimHistory<SQLResponseModel>(claimId, inputXML, ClaimHistoryAction.UpdateServiceLine + "(" + claimServiceLine.ServiceCode + ")", DateTime.UtcNow, token).FirstOrDefault();
                    response.data = new object();
                    response.Message = StatusMessage.ServiceCodeUpdated;
                    response.StatusCode = (int)HttpStatusCodes.OK;
                }
            }
            return response;
        }

        public XElement GetClaimServiceLineModifications(ClaimServiceLine claimServiceLine, ClaimServiceLineModel claimServiceLineModel)
        {
            inputXML = new XElement("Parent");
            
            if (claimServiceLine.Rate != claimServiceLineModel.Rate)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "Rate"),
                    new XElement("OldValue", claimServiceLine.Rate.ToString("0.00")),
                    new XElement("NewValue", claimServiceLineModel.Rate.ToString("0.00"))
                    ));
            }

            if (claimServiceLine.Modifier1 != claimServiceLineModel.Modifier1)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "Modifer1"),
                    new XElement("OldValue", claimServiceLine.Modifier1 != null ? claimServiceLine.Modifier1 : string.Empty),
                    new XElement("NewValue", claimServiceLineModel.Modifier1 != null ? claimServiceLineModel.Modifier1 : string.Empty)
                    ));
            }
            if (claimServiceLine.Modifier2 != claimServiceLineModel.Modifier2)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "Modifer2"),
                    new XElement("OldValue", claimServiceLine.Modifier2 != null ? claimServiceLine.Modifier2 : string.Empty),
                    new XElement("NewValue", claimServiceLineModel.Modifier2 != null ? claimServiceLineModel.Modifier2 : string.Empty)
                    ));
            }
            if (claimServiceLine.Modifier3 != claimServiceLineModel.Modifier3)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "Modifier3"),
                    new XElement("OldValue", claimServiceLine.Modifier3 != null ? claimServiceLine.Modifier3 : string.Empty),
                    new XElement("NewValue", claimServiceLineModel.Modifier3 != null ? claimServiceLineModel.Modifier3 : string.Empty)
                    ));
            }
            if (claimServiceLine.Modifier4 != claimServiceLineModel.Modifier4)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "Modifier4"),
                    new XElement("OldValue", claimServiceLine.Modifier4 != null ? claimServiceLine.Modifier4 : string.Empty),
                    new XElement("NewValue", claimServiceLineModel.Modifier4 != null ? claimServiceLineModel.Modifier4 : string.Empty)
                    ));
            }

            if (claimServiceLine.ClinicianId != claimServiceLineModel.ClinicianId)
            {
                Staffs modelStaff = claimServiceLineModel.ClinicianId != null ? _staffRepository.GetByID(claimServiceLineModel.ClinicianId) : null;
                Staffs entityStaff = claimServiceLine.ClinicianId != null ? _staffRepository.GetByID(claimServiceLine.ClinicianId) : null;
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "Clinician"),
                    new XElement("OldValue", entityStaff != null ? entityStaff.FirstName + " " + entityStaff.LastName : string.Empty),
                    new XElement("NewValue", modelStaff != null ? modelStaff.FirstName + " " + modelStaff.LastName : string.Empty)
                    ));
            }
            if (claimServiceLine.Quantity != claimServiceLineModel.Quantity)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "Quantity"),
                    new XElement("OldValue", claimServiceLine.Quantity > 0 ? claimServiceLine.Quantity : 0),
                    new XElement("NewValue", claimServiceLineModel.Quantity > 0 ? claimServiceLineModel.Quantity : 0)
                    ));
            }
            if (claimServiceLine.TotalAmount != claimServiceLineModel.TotalAmount)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "TotalAmount"),
                    new XElement("OldValue", claimServiceLine.TotalAmount > 0 ? claimServiceLine.TotalAmount : 0),
                    new XElement("NewValue", claimServiceLineModel.TotalAmount > 0 ? claimServiceLineModel.TotalAmount : 0)
                    ));
            }
            if (claimServiceLine.RenderingProviderId != claimServiceLineModel.RenderingProviderId)
            {
                Staffs modelStaff = claimServiceLineModel.RenderingProviderId != null ? _staffRepository.GetByID(claimServiceLineModel.RenderingProviderId) : null;
                Staffs entityStaff = claimServiceLine.RenderingProviderId != null ? _staffRepository.GetByID(claimServiceLine.RenderingProviderId) : null;
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "RenderingProvider"),
                   new XElement("OldValue", entityStaff != null ? entityStaff.FirstName + " " + entityStaff.LastName : string.Empty),
                    new XElement("NewValue", modelStaff != null ? modelStaff.FirstName + " " + modelStaff.LastName : string.Empty)
                    ));
            }
            if (claimServiceLine.OfficeAddressID != claimServiceLineModel.OfficeAddressID)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "OfficeAddress"),
                    new XElement("OldValue", claimServiceLine.OfficeAddressID != null ? _locationRepository.GetByID(claimServiceLine.OfficeAddressID).Address : string.Empty),
                    new XElement("NewValue", claimServiceLineModel.OfficeAddressID != null ? _locationRepository.GetByID(claimServiceLineModel.OfficeAddressID).Address : string.Empty)
                    ));
            }
            //if (claimServiceLine.PatientAddressID != claimServiceLineModel.PatientAddressID)
            //{
            //    inputXML.Add(new XElement("Child",
            //        new XElement("ColumnName", "PatientAddress")//,
            //        //new XElement("OldValue", claimServiceLine.PatientAddressID != null ? _patientAddressRepository.GetByID(claimServiceLine.PatientAddressID).Address1 : string.Empty),
            //        //new XElement("NewValue", claimServiceLineModel.PatientAddressID != null ? _patientAddressRepository.GetByID(claimServiceLineModel.PatientAddressID).Address1 : string.Empty)
            //        ));
            //}
            if (claimServiceLine.CustomAddress != claimServiceLineModel.CustomAddress)
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimServiceLine.ClaimId),
                    new XElement("ColumnName", "CustomAddress"),
                    new XElement("OldValue", claimServiceLine.CustomAddress != null ? claimServiceLine.CustomAddress : string.Empty),
                    new XElement("NewValue", claimServiceLineModel.CustomAddress != null ? claimServiceLineModel.CustomAddress : string.Empty)
                    ));
            }
            //if (claimServiceLine.CustomAddressID != claimServiceLineModel.CustomAddressID)
            //{
            //    inputXML.Add(new XElement("Child",
            //        new XElement("ColumnName", "CustomAddress"),
            //        new XElement("OldValue", _masterPatientLocationRepository.GetByID(claimServiceLine.CustomAddressID).Location),
            //        new XElement("NewValue", _masterPatientLocationRepository.GetByID(claimServiceLineModel.CustomAddressID).Location)
            //        ));
            //}
            return inputXML;
        }

        private object GetSatffDetails(int? clinicianId)
        {
            throw new NotImplementedException();
        }

        public JsonModel GetClaimServiceLineDetails(int claimId, int serviceLineId)
        {

            ClaimServiceLineModel serviceLineModel = new ClaimServiceLineModel();
            if (serviceLineId > 0)
            {
                ClaimServiceLine serviceLine = _claimServiceLineRepository.Get(x => x.Id == serviceLineId && x.IsActive == true && x.IsDeleted == false);
                if (!ReferenceEquals(serviceLine, null))
                    AutoMapper.Mapper.Map(serviceLine, serviceLineModel);
            }
            else
            {
                serviceLineModel = _claimRepository.GetAll(x => x.Id == claimId && x.IsActive == true && x.IsDeleted == false).Select(y => new ClaimServiceLineModel()
                {
                    ClinicianId = y.ClinicianId,
                    RenderingProviderId = y.RenderingProviderId,
                    PatientAddressID = y.PatientAddressID,
                    OfficeAddressID = y.OfficeAddressID,
                    CustomAddressID = y.CustomAddressID,
                    CustomAddress = y.CustomAddress
                }).FirstOrDefault();
            }
            return response = new JsonModel()
            {
                data = serviceLineModel,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };

        }

        public JsonModel DeleteClaimServiceLine(int serviceLineId, TokenModel token)
        {

            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(serviceLineId, DatabaseTables.ClaimServiceLine, false, token).FirstOrDefault();
            if (recordDependenciesModel == null || (recordDependenciesModel != null && recordDependenciesModel.TotalCount == 0))
            {
                ClaimServiceLine claimServiceLine = _claimServiceLineRepository.Get(x => x.Id == serviceLineId && x.IsActive == true && x.IsDeleted == false);
                if (!ReferenceEquals(claimServiceLine, null))
                {
                    pId = GetPatientIdFromClaimId(claimServiceLine.ClaimId);
                    claimServiceLine.IsDeleted = true;
                    claimServiceLine.DeletedBy = token.UserID;
                    claimServiceLine.DeletedDate = DateTime.UtcNow;
                    _claimServiceLineRepository.Update(claimServiceLine);
                    //_claimServiceLineRepository.SaveChanges();
                    inputXML = new XElement("Parent");
                    {
                        inputXML.Add(new XElement("Child",
                            new XElement("ClaimId", claimServiceLine.ClaimId),
                            new XElement("ColumnName", "ServiceCode"),
                            new XElement("OldValue", claimServiceLine.ServiceCode),
                            new XElement("NewValue", string.Empty)
                            ));
                    }
                    _claimRepository.SaveClaimHistory<SQLResponseModel>(claimServiceLine.ClaimId, inputXML, ClaimHistoryAction.DeleteServiceLine, DateTime.UtcNow, token).FirstOrDefault();
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteServiceLine, AuditLogAction.Delete, pId, token.UserID, "ClaimId/CL" + claimServiceLine.ClaimId.ToString(), token);
                    response = new JsonModel() { data = new object(), Message = StatusMessage.ServiceCodeDelete, StatusCode = (int)HttpStatusCodes.OK };
                }
                else
                {
                    response = new JsonModel() { data = new object(), Message = StatusMessage.ServiceCodeNotExists, StatusCode = (int)HttpStatusCodes.OK };
                }
                return response;
            }
            else
            {
                return response = new JsonModel() { data = new object(), Message = StatusMessage.AlreadyExists, StatusCode = (int)HttpStatusCodes.Unauthorized };
            }

        }
        public JsonModel DeleteClaim(int claimId, TokenModel token)
        {

            SQLResponseModel responseModel = _claimRepository.DeleteClaim<SQLResponseModel>(claimId, token).FirstOrDefault();
            //_auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteClaim, AuditLogAction.Delete, pId = null, token.UserID, "ClaimId/CL" + claimId.ToString(), token);

            return new JsonModel()
            {
                data = new object(),
                Message = responseModel.Message,
                StatusCode = responseModel.StatusCode
            };

        }
        public JsonModel GetClaimDetailsById(int claimId)
        {

            ClaimModel claimModel = _claimRepository.GetClaimDetailsById<ClaimModel>(claimId).FirstOrDefault();
            return response = new JsonModel()
            {
                data = claimModel,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };

        }

        public JsonModel UpdateClaimDetails(ClaimModel claimModel, TokenModel token)
        {

            pId = GetPatientIdFromClaimId(claimModel.ClaimId);
            Claims claim = _claimRepository.Get(x => x.Id == claimModel.ClaimId && x.IsDeleted == false && x.IsActive == true);
            if (!ReferenceEquals(claim, null))
            {
                //claim.DOS = claimModel.DOS;
                //claim.RenderingProviderId = claimModel.RenderingProviderId;
                //claim.ClinicianId = claimModel.ClinicianId;
                //claim.ServiceLocationID = claimModel.ServiceLocationId;
                claim.AdditionalClaimInfo = claimModel.AdditionalClaimInfo;
                claim.UpdatedBy = token.UserID;
                claim.UpdatedDate = DateTime.UtcNow;
                //_claimRepository.SaveChanges();
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateClaim, AuditLogAction.Modify, pId, token.UserID, "ClaimId/CL" + claimModel.ClaimId.ToString(), token);
                response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ClaimUpdated,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ClaimNotExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                };
            }
            return response;

        }

        public JsonModel GetAllClaimsWithServiceLines(int pageNumber, int pageSize, int? claimId, string patientIds, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, Nullable<int> claimStatusId, TokenModel token)
        {

            List<ClaimModel> claimList = new List<ClaimModel>();
            ClaimsFullDetailModel allClaims = _claimRepository.GetAllClaimsWithServiceLines(token.OrganizationID, pageNumber, pageSize, claimId, patientIds, fromDate, toDate, payerName, sortColumn, sortOrder, claimStatusId);
            if (allClaims != null && allClaims.Claims.Count > 0)
            {
                foreach (ClaimModel claim in allClaims.Claims)
                {

                    claim.ClaimEncounters = !string.IsNullOrEmpty(claim.XmlString) ? XDocument.Parse(claim.XmlString).Descendants("Child").Select(y => new ClaimEncountersModel()
                    {
                        PatientEncounterId = Convert.ToInt32(y.Element("PatientEncounterId").Value),
                        DOS = Convert.ToDateTime(y.Element("DOS").Value),
                        StartDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Element("StartDateTime").Value), Convert.ToDecimal(y.Element("DaylightOffset").Value), Convert.ToDecimal(y.Element("StandardOffset").Value),y.Element("TimeZoneName").Value, token),
                        EndDateTime = CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(y.Element("EndDateTime").Value), Convert.ToDecimal(y.Element("DaylightOffset").Value), Convert.ToDecimal(y.Element("StandardOffset").Value), y.Element("TimeZoneName").Value, token),
                        AppointmentType = Convert.ToString(y.Element("AppointmentType").Value)
                    }).ToList() : new List<ClaimEncountersModel>();

                    claim.XmlString = null;
                    claim.ClaimServiceLines = allClaims.ClaimServiceLines.FindAll(x => x.ClaimId == claim.ClaimId);
                    claimList.Add(claim);
                }
                return new JsonModel()
                {
                    data = claimList,
                    meta = new Meta()
                    {
                        TotalRecords = claimList[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(claimList[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ClaimNotExist,
                    StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                };
            }
        }

        public int GetPatientIdFromClaimId(int claimId)
        {
            var res = _claimRepository.GetAll(x => x.Id == claimId && x.IsActive == true && x.IsDeleted == false).Select(x => new ClaimModel()
            {
                PatientId = x.PatientId
            }).FirstOrDefault();
            if (res != null)
                return res.PatientId;
            else
                return 0;
        }

        public JsonModel GetClaimsForPatientLedger(int patientId, int pageNumber, int pageSize, string sortColumn, string sortOrder, string claimBalanceStatus, TokenModel token)
        {
            List<PatientLedgerClaimModel> ledgerClaims = _claimRepository.GetClaimsForPatientLedger<PatientLedgerClaimModel>(patientId, pageNumber, pageSize, sortColumn, sortOrder, claimBalanceStatus, token).ToList();
            return new JsonModel()
            {
                data = ledgerClaims,
                meta = new Meta()
                {
                    TotalRecords = ledgerClaims != null && ledgerClaims.Count > 0 ? ledgerClaims[0].TotalRecords : 0,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    DefaultPageSize = pageSize,
                    TotalPages = Math.Ceiling(Convert.ToDecimal((ledgerClaims != null && ledgerClaims.Count > 0 ? ledgerClaims[0].TotalRecords : 0) / pageSize))
                },
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel GetClaimServiceLinesForPatientLedger(int claimId, TokenModel token)
        {
            Dictionary<string, object> responseDic = new Dictionary<string, object>();
            List<PatientLedgerClaimServiceLineModel> listClaimsServiceLines = _claimRepository.GetClaimServiceLinesForPatientLedger<PatientLedgerClaimServiceLineModel>(claimId, token).ToList();
            responseDic.Add("ServiceLines", listClaimsServiceLines);
            if (listClaimsServiceLines != null && listClaimsServiceLines.Count > 0)
                responseDic.Add("PaymentList", _paymentRepository.GetServiceLinePaymentDetailsForPatientLedger<PatientLedgerClaimServiceLinePaymentDetailModel>(string.Join(",", listClaimsServiceLines.Select(x => x.Id.ToString())), token).ToList());
            else
                responseDic.Add("PaymentList", new object());
            return new JsonModel()
            {
                data = responseDic,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel GetClaimHistory(Nullable<int> claimId, int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            Dictionary<string, object> responseData = new Dictionary<string, object>();
            responseData.Add("ClaimDetail", _claimRepository.GetClaimDetailsById<ClaimHistoryModel>((int)claimId).FirstOrDefault());
            List<ClaimHistoryDetailModel> claimHistory = _claimRepository.GetClaimHistory<ClaimHistoryDetailModel>(claimId, pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
            if (claimHistory != null && claimHistory.Count > 0)
            {
                claimHistory.ForEach(x =>
                {
                    var location = _locationService.GetLocationOffsets(x.ServiceLocationId,token);
                    decimal daylightOffset = 0;
                    decimal standardOffset = 0;
                    if (location != null)
                    {
                        daylightOffset = (((decimal)location.DaylightSavingTime) * 60);
                        standardOffset = (((decimal)location.StandardTime) * 60);
                    }
                    x.LogDate = CommonMethods.ConvertFromUtcTimeWithOffset(x.LogDate, daylightOffset, standardOffset, location.TimeZoneName, token);
                });
                responseData.Add("ClaimHistory", claimHistory);
                return response = new JsonModel()
                {
                    data = responseData,
                    meta = new Meta()
                    {
                        TotalRecords = claimHistory[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(claimHistory[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        public JsonModel GetClaimsForLedger(int pageNumber, int pageSize, string sortColumn, string sortOrder, string patientIds, string payerIds, string tags, string fromDate, string toDate, string claimBalanceStatus, TokenModel token)
        {
            List<PatientLedgerClaimModel> ledgerClaims = _claimRepository.GetClaimsForLedger<PatientLedgerClaimModel>(pageNumber, pageSize, sortColumn, sortOrder, patientIds, payerIds, tags, fromDate, toDate, claimBalanceStatus, token).ToList();
            if (ledgerClaims != null && ledgerClaims.Count > 0)
            {
                return new JsonModel()
                {
                    data = ledgerClaims,
                    meta = new Meta()
                    {
                        TotalRecords = ledgerClaims[0].TotalRecords,
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal(ledgerClaims[0].TotalRecords / pageSize))
                    },
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else {
                return new JsonModel()
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        public JsonModel GetClaimServiceLinesForLedger(int claimId, TokenModel token)
        {
            Dictionary<string, object> responseDic = new Dictionary<string, object>();
            List<PatientLedgerClaimServiceLineModel> listClaimsServiceLines = _claimRepository.GetClaimServiceLinesForLedger<PatientLedgerClaimServiceLineModel>(claimId, token).ToList();
            responseDic.Add("ServiceLines", listClaimsServiceLines);
            if (listClaimsServiceLines != null && listClaimsServiceLines.Count > 0)
                responseDic.Add("PaymentList", _paymentRepository.GetServiceLinePaymentDetailsForLedger<PatientLedgerClaimServiceLinePaymentDetailModel>(string.Join(",", listClaimsServiceLines.Select(x => x.Id.ToString())), token).ToList());
            else
                responseDic.Add("PaymentList", new object());
            return new JsonModel()
            {
                data = responseDic,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel UpdatePaymentStatus(int? claimId, int? paymentStatusId, TokenModel token)
        {
            Claims claim = null;
            claim = _claimRepository.GetByID(claimId);
            if (claim != null)
            {
                claim.ClaimPaymentStatusId = paymentStatusId;
                claim.UpdatedBy = token.UserID;
                claim.UpdatedDate = DateTime.UtcNow;
                _claimRepository.Update(claim);
                response = new JsonModel(new object(), StatusMessage.PaymentStatusUpdated, (int)HttpStatusCodes.OK, string.Empty);
            }
            _claimRepository.SaveChanges();
            return response;
        }

        
    }
}