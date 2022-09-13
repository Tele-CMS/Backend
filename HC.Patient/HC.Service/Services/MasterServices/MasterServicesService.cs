using AutoMapper;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Service.IServices;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HC.Patient.Service.Services
{
    public class MasterServicesService : BaseService, IMasterServicesService
    {
        private readonly IMasterServicesRepository _masterServicesRepository;
        private readonly IMapper _mapper;
        private JsonModel _response;
        public MasterServicesService(IMasterServicesRepository masterServicesRepository, IMapper mapper)
        {
            _masterServicesRepository = masterServicesRepository;
            _mapper = mapper;
        }
        public JsonModel getMasterServicesByOrganizationId(TokenModel tokenModel, MasterServiceFilterModel masterServiceFilterModel)
        {
            _response = new JsonModel();
            if (tokenModel.OrganizationID == 0)
                return new JsonModel(new object(), StatusMessage.OrganizationIdRequired, (int)HttpStatusCode.BadRequest);
            var masterServices = _masterServicesRepository.getMasterServicesByOrganizationId(tokenModel, masterServiceFilterModel);
            List<MasterServices> masterServiceList = masterServices.Skip((masterServiceFilterModel.pageNumber - 1) * masterServiceFilterModel.pageSize)
                                               .Take(masterServiceFilterModel.pageSize).ToList<MasterServices>();
            if (masterServiceList != null && masterServiceList.Count > 0)
            {
                List<MasterServicesModel> masterServicesResponseModel = new List<MasterServicesModel>();
                _mapper.Map(masterServiceList, masterServicesResponseModel);
                _response.data = masterServicesResponseModel;
                _response.Message = StatusMessage.FetchMessage;
                _response.StatusCode = (int)HttpStatusCode.OK;

                List<MasterServicesModel> masterServicesMetaModel = new List<MasterServicesModel>();
                _mapper.Map(masterServices.ToList(), masterServicesMetaModel);
                masterServicesMetaModel[0].TotalRecords = masterServicesMetaModel.Count;
                _response.meta = new Meta(masterServicesMetaModel, masterServiceFilterModel);
            }
            else
            {
                _response.data = new object();
                _response.Message = StatusMessage.NotFound;
                _response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return _response;
        }

        public JsonModel getMasterServicesById(TokenModel tokenModel, string id)
        {
            _response = new JsonModel();
            if (string.IsNullOrEmpty(id))
                return new JsonModel(new object(), StatusMessage.ServiceIdRequired, (int)HttpStatusCode.BadRequest);
            int.TryParse(Common.CommonMethods.Decrypt(id.Replace(" ", "+")), out int serviceId);
            var masterServices = _masterServicesRepository.getMasterServicesById(tokenModel, serviceId);
            if (masterServices != null)
            {
                MasterServicesModel masterServicesResponseModel = new MasterServicesModel();
                _mapper.Map(masterServices, masterServicesResponseModel);
                _response.data = masterServicesResponseModel;
                _response.Message = StatusMessage.FetchMessage;
                _response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                _response.data = new object();
                _response.Message = StatusMessage.NotFound;
                _response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return _response;
        }

        public JsonModel ChecServiceNameExistance(TokenModel tokenModel, string name)
        {
            MasterServices masterServices = _masterServicesRepository.CheckServiceNameExistance(name, tokenModel);

            if (masterServices == null)
                return new JsonModel(new object(), StatusMessage.ServiceNameAvailable, (int)HttpStatusCode.NotFound);
            return new JsonModel(new object(), StatusMessage.ServiceNameTaken, (int)HttpStatusCode.OK);

        }

        public JsonModel SaveUpdateMasterService(TokenModel tokenModel, MasterServicesModel masterServicesModel)
        {
            MasterServices masterServices = _masterServicesRepository.CheckServiceNameExistance(masterServicesModel.ServiceName, tokenModel);

            if (masterServices != null)
                if (string.IsNullOrEmpty(masterServicesModel.Id) || masterServicesModel.Id != Common.CommonMethods.Encrypt(masterServices.Id.ToString()))
                    return new JsonModel(new object(), StatusMessage.ServiceNameAvailable, (int)HttpStatusCode.Found);

            masterServices = new MasterServices();

            if (!string.IsNullOrEmpty(masterServicesModel.Id))
            {
                masterServices = _masterServicesRepository.getMasterServicesById(tokenModel, Convert.ToInt16(Common.CommonMethods.Decrypt(masterServicesModel.Id)));
                masterServices.UpdatedBy = tokenModel.UserID;
                masterServices.UpdatedDate = DateTime.UtcNow;
                masterServices.GlobalCodeId = masterServicesModel.GlobalCodeId;

            }
            else
            {
                masterServices.CreatedBy = tokenModel.UserID;
                masterServices.CreatedDate = DateTime.UtcNow;
                masterServices.OrganizationId = tokenModel.OrganizationID;
                masterServices.GlobalCodeId = masterServicesModel.GlobalCodeId;
            }
            _mapper.Map(masterServicesModel, masterServices);
            var services = _masterServicesRepository.saveUpdateMasterServices(masterServices, tokenModel);

            if (services != null)
                return new JsonModel(new object(), StatusMessage.MasterServicesSaved, (int)HttpStatusCode.OK);
            return new JsonModel(new object(), StatusMessage.MasterServicesNotSaved, (int)HttpStatusCode.InternalServerError);

        }

        public JsonModel DeleteMasterService(TokenModel tokenModel, string id)
        {
            if (string.IsNullOrEmpty(id))
                return new JsonModel(new object(), StatusMessage.ServiceIdRequired, (int)HttpStatusCode.BadRequest);

            _response = new JsonModel();
            int.TryParse(id, out int serviceId);
            var masterServices = _masterServicesRepository.getMasterServicesById(tokenModel, serviceId);
            if (masterServices != null)
            {
                masterServices.DeletedBy = tokenModel.UserID;
                masterServices.DeletedDate = DateTime.UtcNow;
                masterServices.IsDeleted = true;
                var services = _masterServicesRepository.saveUpdateMasterServices(masterServices, tokenModel);
                if (services != null)
                    return new JsonModel(new object(), StatusMessage.MasterServicesDeleted, (int)HttpStatusCode.OK);
                return new JsonModel(new object(), StatusMessage.MasterServicesNotDeleted, (int)HttpStatusCode.InternalServerError);
            }
            else
            {
                _response.data = new object();
                _response.Message = StatusMessage.NotFound;
                _response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return _response;

        }
       
    }
}
