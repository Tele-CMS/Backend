using HC.Common;
using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Service.IServices.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using HC.Service;
using HC.Common.HC.Common;
using System.Net;
using HC.Patient.Entity;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Repositories.Interfaces;

namespace HC.Patient.Service.Services.MasterData
{
    public class EdiGatewayService : BaseService, IEdiGatewayService
    {
        private readonly IEdiGatewayRepository _ediGatewayRepository;        
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public EdiGatewayService(IEdiGatewayRepository ediGatewayRepository, IUserCommonRepository userCommonRepository)
        {
            _ediGatewayRepository = ediGatewayRepository;
            _userCommonRepository = userCommonRepository;            
        }

        public SftpClient CreateConnection(ClearingHouseModel ediGateway)
        {
            string ftpURL = ediGateway.FTPURL;
            string userName = ediGateway.FTPUsername;
            string password = ediGateway.FTPPassword;
            int portNo = Convert.ToInt32(ediGateway.PortNo);
            SftpClient client = new SftpClient(ftpURL, portNo, userName, password);
            try
            {
                client.Connect();
            }
            catch (Exception)
            {
                //continue;
            }
            return client;
        }

        public ClearingHouseModel GetActiveClearingHouseDetails(TokenModel token)
        {
            ClearingHouseModel ediGateway = _ediGatewayRepository.GetAll(x => x.IsActive == true && x.OrganizationID == token.OrganizationID).Select(y => new ClearingHouseModel()
            {
                ClearingHouseName = y.ClearingHouseName,
                FTPURL = y.FTPURL,
                FTPPassword = CommonMethods.Decrypt(y.FTPPassword),
                FTPUsername = y.FTPUsername,
                SenderID = y.SenderID,
                ReceiverID = y.ReceiverID,
                PortNo = y.PortNo,
                Path837 = y.Path837,
                Path835 = y.Path835,
                Path277 = y.Path277,
                Path999 = y.Path999,
                Id = y.Id,
                InterchangeQualifier = y.InterchangeQualifier
            }).FirstOrDefault();
            return ediGateway;
        }

        #region Master EDI Service Methods
        public JsonModel GetEDIGateways(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<EDIModel> eDIModels = _ediGatewayRepository.GetEDIGateways<EDIModel>(searchFilterModel, tokenModel).ToList();
            if (eDIModels != null && eDIModels.Count > 0)
            {
                response = new JsonModel(eDIModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(eDIModels, searchFilterModel);
            }
            return response;
        }

        public JsonModel SaveUpdate(EDIModel eDIModel, TokenModel tokenModel)
        {
            EDIGateway eDIGateway = null;
            if (eDIModel.Id == 0)
            {
                eDIGateway = _ediGatewayRepository.Get(l => l.ClearingHouseName.ToLower() == eDIModel.ClearingHouseName.ToLower() && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (eDIGateway != null)//duplicate check on new insertion
                {
                    response = new JsonModel(new object(), StatusMessage.EdiAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else
                {
                    eDIGateway = new EDIGateway();
                    AutoMapper.Mapper.Map(eDIModel, eDIGateway);
                    eDIGateway.OrganizationID = tokenModel.OrganizationID;
                    eDIGateway.CreatedBy = tokenModel.UserID;
                    eDIGateway.CreatedDate = DateTime.UtcNow;
                    eDIGateway.IsDeleted = false;
                    eDIGateway.IsActive = true;
                    //Encrypt
                    if (!string.IsNullOrEmpty(eDIGateway.FTPPassword))
                    { eDIGateway.FTPPassword = CommonMethods.Encrypt(eDIGateway.FTPPassword); }


                    _ediGatewayRepository.Create(eDIGateway);
                    _ediGatewayRepository.SaveChanges();
                    response = new JsonModel(eDIGateway, StatusMessage.EDISave, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                eDIGateway = _ediGatewayRepository.Get(l => l.ClearingHouseName == eDIModel.ClearingHouseName && l.Id != eDIModel.Id && l.IsDeleted == false && l.IsActive == true && l.OrganizationID == tokenModel.OrganizationID);
                if (eDIGateway != null) //duplicate check
                {
                    response = new JsonModel(new object(), StatusMessage.EdiAlreadyExist, (int)HttpStatusCodes.UnprocessedEntity);
                }
                else //update existing
                {
                    eDIGateway = _ediGatewayRepository.Get(a => a.Id == eDIModel.Id && a.IsDeleted == false && a.IsActive == true);
                    if (eDIGateway != null)
                    {
                        AutoMapper.Mapper.Map(eDIModel, eDIGateway);
                        //Encrypt
                        if (!string.IsNullOrEmpty(eDIGateway.FTPPassword))
                        { eDIGateway.FTPPassword = CommonMethods.Encrypt(eDIGateway.FTPPassword); }
                        eDIGateway.UpdatedBy = tokenModel.UserID;
                        eDIGateway.UpdatedDate = DateTime.UtcNow;
                        _ediGatewayRepository.Update(eDIGateway);
                        _ediGatewayRepository.SaveChanges();
                        response = new JsonModel(eDIGateway, StatusMessage.EDIUpdated, (int)HttpStatusCode.OK);
                    }
                }
            }
            return response;
        }

        public JsonModel GetEDIGateWayById(int Id, TokenModel tokenModel)
        {
            EDIGateway eDIGateway = _ediGatewayRepository.Get(a => a.Id == Id && a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive == true);
            if (eDIGateway != null)
            {
                EDIModel eDIModel = new EDIModel();
                AutoMapper.Mapper.Map(eDIGateway, eDIModel);
                if (!string.IsNullOrEmpty(eDIModel.FTPPassword)) { eDIModel.FTPPassword = CommonMethods.Decrypt(eDIModel.FTPPassword); }
                response = new JsonModel(eDIModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteEDIGateway(int id, TokenModel tokenModel)
        {
            RecordDependenciesModel recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.EDIGateWay, false, tokenModel).FirstOrDefault();
            if (recordDependenciesModel != null && recordDependenciesModel.TotalCount > 0)
            {
                response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized);
            }
            else
            {
                EDIGateway eDIGateway = _ediGatewayRepository.Get(a => a.Id == id && a.IsDeleted == false);
                if (eDIGateway != null)
                {
                    eDIGateway.IsDeleted = true;
                    eDIGateway.DeletedBy = tokenModel.UserID;
                    eDIGateway.DeletedDate = DateTime.UtcNow;
                    _ediGatewayRepository.Update(eDIGateway);
                    _ediGatewayRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.EDIDeleted, (int)HttpStatusCodes.OK);
                }
            }
            return response;
        }
        #endregion
    }
}
