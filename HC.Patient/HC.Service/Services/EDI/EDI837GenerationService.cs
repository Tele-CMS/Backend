using System;
using System.Collections.Generic;
using HC.Model;
using HC.Patient.Repositories.IRepositories.Claim;
using HC.Patient.Model.Claim;
using static HC.Common.Enums.CommonEnum;
using System.Linq;
using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Patient.Entity;
using HC.Common.HC.Common;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Model.MasterData;
using Renci.SshNet;
using System.IO;
using System.Xml.Linq;
using HC.Patient.Model.CustomMessage;
using HC.Common.Enums;
using HC.Service;
using HC.Patient.Service.IServices.EDI;

namespace HC.Patient.Service.Services.EDI
{
    public class EDI837GenerationService : BaseService, IEDI837GenerationService
    {
        private HCOrganizationContext _context;        
        private IClaimRepository _claimRepository;
        private EDIGenerator.IServices.IEDI837Service _edi837Service;
        private JsonModel response;
        private IClaim837BatchRepository _claim837BatchRepository;
        private IEdiGatewayService _ediGatewayService;
        XElement inputXML = null;
        public EDI837GenerationService(IClaimRepository claimRepository, EDIGenerator.IServices.IEDI837Service edi837Service, HCOrganizationContext context, IClaim837BatchRepository claim837BatchRepository,IEdiGatewayService ediGatewayService)
        {
            _ediGatewayService = ediGatewayService;
            _edi837Service = edi837Service;
            _claimRepository = claimRepository;            
            _context = context;
            _claim837BatchRepository = claim837BatchRepository;
        }
        public string DownloadSingleEDI837(int claimId, int patientInsuranceId, int locationId, TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForSingleSubmit<Claim837BatchModel>(claimId, patientInsuranceId, token.UserID, token.OrganizationID, InsurancePlanType.Primary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForSingleClaim(claimId, patientInsuranceId, claim837Batch.Claim837BatchId, token.OrganizationID, locationId);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateSingleEDI837(ediFileModel);
                    if (!string.IsNullOrEmpty(ediText))
                    {
                        _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimId.ToString(), "update");
                        response = new JsonModel() { data = new object(), Message = "Ok", StatusCode = (int)HttpStatusCodes.OK };
                    }
                    else
                    {
                        _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimId.ToString(), "delete");
                        response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                    }
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimId.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = "Some issue in clients data", StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return ediText;
        }

        public JsonModel GenerateSingleEDI837(int claimId, int patientInsuranceId,TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForSingleSubmit<Claim837BatchModel>(claimId, patientInsuranceId, token.UserID, token.OrganizationID, InsurancePlanType.Primary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0) ///This will be a common method if and else
            {
                inputXML = new XElement("Parent");
                {
                    inputXML.Add(new XElement("Child",
                        new XElement("ClaimId", claimId),
                        new XElement("ColumnName", string.Empty),
                        new XElement("OldValue", string.Empty),
                        new XElement("NewValue", string.Empty)
                        ));
                }
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForSingleClaim(claimId, patientInsuranceId, claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel,"primary","single","original");
                    UpdateBatchRequest(token, claimId.ToString(), ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch, ClaimHistoryAction.PrimaryPayerSubmission);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimId.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }
        public JsonModel GenerateBatchEDI837(string claimIds, TokenModel token)
        {

            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() +"\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForBatchSubmit<Claim837BatchModel>(claimIds, token.UserID, token.OrganizationID, InsurancePlanType.Primary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                inputXML = new XElement("Parent");
                {
                    claimIds.Split(",").ToList().ForEach(x =>
                    {
                        inputXML.Add(new XElement("Child",
                            new XElement("ClaimId",Convert.ToInt32(x)),
                            new XElement("ColumnName", string.Empty),
                            new XElement("OldValue", string.Empty),
                            new XElement("NewValue", string.Empty)
                            ));
                    });

                } 
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForBatchClaim(claimIds, InsurancePlanType.Primary.ToString(), claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel, "primary", "batch", "original");
                    //ediText = _edi837Service.GenerateBatchEDI837(ediFileModel);
                    UpdateBatchRequest(token, claimIds, ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch,ClaimHistoryAction.PrimaryPayerSubmission);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimIds.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }

        public JsonModel ResubmitClaim(int claimId, int patientInsuranceId,string resubmissionReason, string payerControlReferenceNumber, TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            inputXML = new XElement("Parent");
            {
                inputXML.Add(new XElement("Child",
                    new XElement("ClaimId", claimId),
                    new XElement("ColumnName", "ResubmissionReason"),
                    new XElement("OldValue", string.Empty),
                    new XElement("NewValue", resubmissionReason)
                    ));

                if (!string.IsNullOrEmpty(payerControlReferenceNumber))
                {
                    inputXML.Add(new XElement("Child",
                         new XElement("ClaimId", claimId),
                        new XElement("ColumnName", "PayerControlReferenceNumber"),
                        new XElement("OldValue", string.Empty),
                        new XElement("NewValue", payerControlReferenceNumber)
                        ));
                }
            }
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForSingleSubmit<Claim837BatchModel>(claimId, patientInsuranceId, token.UserID, token.OrganizationID, InsurancePlanType.Primary.ToString(), EDISubmissionType.Edited.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                EDI837FileModel ediFileModel = _claimRepository.GetClaimInfoForResubmission(claimId, patientInsuranceId, claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID, resubmissionReason,string.IsNullOrEmpty(payerControlReferenceNumber) ? "" : payerControlReferenceNumber);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel, "primary", "batch", "edited");
                    //ediText = _edi837Service.GenerateSingleEDI837(ediFileModel);
                    UpdateBatchRequest(token, claimId.ToString(), ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch, ClaimHistoryAction.ResubmissionClaim);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimId.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }
        public JsonModel ResubmitBatchClaim(List<ResubmitInputModel> claimInfo, TokenModel token)
        {
            string claimIds =string.Join(",", claimInfo.Select(x => x.ClaimId));
            XElement claimXML = new XElement("Parent");
            inputXML = new XElement("Parent");
            claimInfo.ForEach(x =>
            {
                inputXML.Add(new XElement("Child",
                   new XElement("ClaimId", x.ClaimId),
                   new XElement("ColumnName", "ResubmissionReason"),
                   new XElement("OldValue", string.Empty),
                   new XElement("NewValue", x.ResubmissionReason)
                   ));

                if (!string.IsNullOrEmpty(x.PayerControlReferenceNumber))
                {
                    inputXML.Add(new XElement("Child",
                       new XElement("ClaimId", x.ClaimId),
                       new XElement("ColumnName", "PayerControlReferenceNumber"),
                       new XElement("OldValue", string.Empty),
                       new XElement("NewValue", x.PayerControlReferenceNumber)
                       ));
                }
            });
            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForBatchSubmit<Claim837BatchModel>(claimIds, token.UserID, token.OrganizationID, InsurancePlanType.Primary.ToString(), EDISubmissionType.Edited.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                EDI837FileModel ediFileModel = _claimRepository.GetClaimInfoForBatchResubmission(claimXML.ToString(), InsurancePlanType.Primary.ToString(), claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel, "primary", "batch", "edited");
                    //ediText = _edi837Service.GenerateBatchEDI837(ediFileModel);
                    UpdateBatchRequest(token, claimIds, ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch, ClaimHistoryAction.ResubmissionClaim);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimIds.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }

        public JsonModel GenerateSingleEDI837_Secondary(int claimId, int patientInsuranceId,TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForSingleSubmit<Claim837BatchModel>(claimId, patientInsuranceId, token.UserID, token.OrganizationID, InsurancePlanType.Secondary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                inputXML = new XElement("Parent");
                {
                    inputXML.Add(new XElement("Child",
                        new XElement("ClaimId", claimId),
                        new XElement("ColumnName", string.Empty),
                        new XElement("OldValue", string.Empty),
                        new XElement("NewValue", string.Empty)
                        ));
                }
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForSingleClaim_Secondary(claimId, patientInsuranceId, claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel, "secondary", "single", "original");
                    //ediText = _edi837Service.GenerateSingleEDI837_Secondary(ediFileModel);
                    UpdateBatchRequest(token, claimId.ToString(), ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch, ClaimHistoryAction.SecondaryPayerSubmission);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimId.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }
        public JsonModel GenerateBatchEDI837_Secondary(string claimIds, TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForBatchSubmit<Claim837BatchModel>(claimIds, token.UserID, token.OrganizationID, InsurancePlanType.Secondary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForBatchClaim_Secondary(claimIds, InsurancePlanType.Secondary.ToString(), claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel, "secondary", "batch", "original");
                    //ediText = _edi837Service.GenerateSingleEDI837_Secondary(ediFileModel);
                    UpdateBatchRequest(token, claimIds, ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch, ClaimHistoryAction.SecondaryPayerSubmission);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimIds.ToString(), "delete");
                    response = new JsonModel() { data = null, Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = null, Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }

        private bool CreateFile(ClearingHouseModel clearingHouse, string fileText, string fileName, string fullPath)
        {
#if DEBUG
            return true;
#endif
            bool fileCreationRes = false;
            try
            {
                SftpClient client = _ediGatewayService.CreateConnection(clearingHouse);
                string ftpDirectory = string.Empty;
                Stream stream = null;
                if (client != null && client.ConnectionInfo.IsAuthenticated)
                {
                    ftpDirectory = "/" + clearingHouse.Path837 + "/";
                    StreamWriter sw = new StreamWriter(fullPath + "\\" + clearingHouse.Path837 + "\\" + fileName);
                    sw.Write(fileText);
                    sw.Close();
                    stream = System.IO.File.OpenRead(fullPath + "\\" + clearingHouse.Path837 + "\\" + fileName);
                    if (!client.Exists(ftpDirectory + fileName))
                        client.UploadFile(stream, ftpDirectory + fileName);
                    if (stream != null)
                        stream.Close();
                    client.Disconnect();
                    fileCreationRes = true;
                }
                return fileCreationRes;
            }
            catch
            {
                return fileCreationRes;
            }
        }
        public JsonModel SubmitClaimsForNonEDI(string claimIds,TokenModel token)
        {
            try
            {
                int[] list = claimIds.Split(',').Select(s => int.Parse(s)).ToArray();
                List<Claims> listClaims = _claimRepository.GetAll().Where(x => list.Contains(x.Id)).ToList();
                if (listClaims != null && listClaims.Count > 0)
                {
                    listClaims.ForEach(x =>
                    {
                        x.ClaimStatusId = (int)CommonEnum.MasterStatusClaim.Submitted;
                        x.UpdatedBy = token.UserID;
                        x.SubmissionType = Convert.ToInt16(CommonEnum.ClaimSubmissionType.NonEDI);
                        x.UpdatedDate = DateTime.UtcNow;
                        x.SubmittedDate = DateTime.UtcNow;
                    });
                    _claimRepository.Update(listClaims.ToArray());
                    _claimRepository.SaveChanges();
                }
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.SubmitToNonEDIPayer,
                    StatusCode = (int)HttpStatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError,
                    AppError= ex.Message
                };
            }
        }

        public string DownloadBatchEDI837(string claimIds, int locationId, string path, TokenModel token)
        {

            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForBatchSubmit<Claim837BatchModel>(claimIds, token.UserID, token.OrganizationID, InsurancePlanType.Primary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForBatchClaim(claimIds, InsurancePlanType.Primary.ToString(), claim837Batch.Claim837BatchId, token.OrganizationID, locationId);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateBatchEDI837(ediFileModel);
                    if (!string.IsNullOrEmpty(ediText))
                    {
                        //fileName = Convert.ToString(claim837Batch.Claim837BatchId) + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".X12";
                        //Directory.CreateDirectory(fullPath + "\\" + clearingHouse.Path837);
                        //fileCreationRes = true; //CreateFile(clearingHouse, ediText, fileName, fullPath);
                        //if (fileCreationRes)
                        //{
                            _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimIds.ToString(), "update");
                            response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837SuccessfullyUploaded, StatusCode = (int)HttpStatusCodes.OK };
                        //}
                        //else
                        //{
                        //    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimIds.ToString(), "delete");
                        //    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837UploadError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                        //}
                    }
                    else
                    {
                        _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimIds.ToString(), "delete");
                        response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837GenerationError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                    }
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimIds.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return ediText;
        }

        private void UpdateBatchRequest(TokenModel token, string claimIds, string ediText, ref string fileName, ref bool fileCreationRes, string fullPath, ClearingHouseModel clearingHouse, Claim837BatchModel claim837Batch,string claimHistoryAction)
        {
            if (!string.IsNullOrEmpty(ediText))
            {
                fileName = Convert.ToString(claim837Batch.Claim837BatchId) + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".X12";
                Directory.CreateDirectory(fullPath + "\\" + clearingHouse.Path837);
                fileCreationRes = CreateFile(clearingHouse, ediText, fileName, fullPath);
                if (fileCreationRes)
                {
                    _claimRepository.SaveClaimHistory<SQLResponseModel>(null, inputXML, claimHistoryAction, DateTime.UtcNow, token).FirstOrDefault();
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimIds.ToString(), "update");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837SuccessfullyUploaded, StatusCode = (int)HttpStatusCodes.OK };
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimIds.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837UploadError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
            {
                _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimIds.ToString(), "delete");
                response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837GenerationError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            }
        }

        public JsonModel GetSubmittedClaimsBatch(int pageNumber, int pageSize, DateTime fromDate, DateTime toDate, TokenModel token)
        {
            List<SubmittedClaimBatchModel> listClaims = _claim837BatchRepository.GetSubmittedClaimsBatch<SubmittedClaimBatchModel>(pageNumber, pageSize, fromDate, toDate, token).ToList();
            return new JsonModel()
            {
                data = listClaims,
                meta = new Meta()
                {
                    TotalRecords = (listClaims!=null && listClaims.Count>0)?listClaims[0].TotalRecords:0,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    DefaultPageSize = pageSize,
                    TotalPages = (listClaims != null && listClaims.Count > 0)? Math.Ceiling(Convert.ToDecimal(listClaims[0].TotalRecords / pageSize)):0
                },
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel GetSubmittedClaimsBatchDetails(string claim837BatchIds, TokenModel token)
        {
            return new JsonModel(_claim837BatchRepository.GetSubmittedClaimsBatchDetails(claim837BatchIds, token), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
        }

        public JsonModel GenerateSingleEDI837_Tertiary(int claimId, int patientInsuranceId, TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForSingleSubmit<Claim837BatchModel>(claimId, patientInsuranceId, token.UserID, token.OrganizationID, InsurancePlanType.Tertiary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                inputXML = new XElement("Parent");
                {
                    inputXML.Add(new XElement("Child",
                        new XElement("ClaimId", claimId),
                        new XElement("ColumnName", string.Empty),
                        new XElement("OldValue", string.Empty),
                        new XElement("NewValue", string.Empty)
                        ));
                }
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForSingleClaim_Tertiary(claimId, patientInsuranceId, claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel, "tertiary", "single", "original");
                    //ediText = _edi837Service.GenerateSingleEDI837_Secondary(ediFileModel);
                    UpdateBatchRequest(token, claimId.ToString(), ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch, ClaimHistoryAction.SecondaryPayerSubmission);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimId.ToString(), "delete");
                    response = new JsonModel() { data = null, Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = null, Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }

        public JsonModel GenerateBatchEDI837_Tertiary(string claimIds, TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            bool fileCreationRes = false;
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForBatchSubmit<Claim837BatchModel>(claimIds, token.UserID, token.OrganizationID, InsurancePlanType.Tertiary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                EDI837FileModel ediFileModel = _claimRepository.GetEDIInfoForBatchClaim_Tertiary(claimIds, InsurancePlanType.Secondary.ToString(), claim837Batch.Claim837BatchId, token.OrganizationID, token.LocationID);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateEDI837_005010X222A1(ediFileModel, "tertiary", "batch", "original");
                    //ediText = _edi837Service.GenerateSingleEDI837_Secondary(ediFileModel);
                    UpdateBatchRequest(token, claimIds, ediText, ref fileName, ref fileCreationRes, fullPath, clearingHouse, claim837Batch, ClaimHistoryAction.TertiaryPayerSubmission);
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimIds.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = StatusMessage.EDI837ClientDataError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return response;
        }

        public JsonModel GetEDIInfo(int claimId, int patientInsuranceId, TokenModel token)
        {
            string ediText = string.Empty;
            string fileName = string.Empty;
            EDI837FileModel ediFileModel = null;
            Claim837BatchModel claim837Batch = _claimRepository.SaveClaim837BatchRequestForSingleSubmit<Claim837BatchModel>(claimId, patientInsuranceId, token.UserID, token.OrganizationID, InsurancePlanType.Primary.ToString(), EDISubmissionType.Original.ToString()).FirstOrDefault();
            if (claim837Batch != null && claim837Batch.Claim837BatchId > 0)
            {
                 ediFileModel = _claimRepository.GetEDIInfoForSingleClaim(claimId, patientInsuranceId, claim837Batch.Claim837BatchId, token.OrganizationID, 1);
                if (ediFileModel != null && ediFileModel.EDIClaims != null && ediFileModel.EDIClaims.Count > 0)
                {
                    ediText = _edi837Service.GenerateSingleEDI837(ediFileModel);
                    if (!string.IsNullOrEmpty(ediText))
                    {
                        _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimId.ToString(), "update");
                        response = new JsonModel() { data = new object(), Message = "Ok", StatusCode = (int)HttpStatusCodes.OK };
                    }
                    else
                    {
                        _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, ediText, token.UserID, claimId.ToString(), "delete");
                        response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
                    }
                }
                else
                {
                    _claim837BatchRepository.UpdateBatchRequestStatus(claim837Batch.Claim837BatchId, string.Empty, token.UserID, claimId.ToString(), "delete");
                    response = new JsonModel() { data = new object(), Message = "Some issue in clients data", StatusCode = (int)HttpStatusCodes.InternalServerError };
                }
            }
            else
                response = new JsonModel() { data = new object(), Message = StatusMessage.ServerError, StatusCode = (int)HttpStatusCodes.InternalServerError };
            return new JsonModel(ediFileModel,"",1,""); 
        }
    }
}
