using AutoMapper;
using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories.GlobalCodes;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Images;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HC.Patient.Service.Services.GlobalCodes
{
    public class GlobalCodeService : BaseService, IGlobalCodeService
    {
        private readonly IGlobalCodeRepository _globalCodeRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private JsonModel _response;
        public GlobalCodeService(IGlobalCodeRepository globalCodeRepository, IMapper mapper, IImageService imageService)
        {
            _globalCodeRepository = globalCodeRepository;
            _mapper = mapper;
            _imageService = imageService;
        }
            
        public JsonModel CheckGlobalCodeExistance(string name, TokenModel tokenModel)
        {
            GlobalCode globalCode = _globalCodeRepository.CheckGlobalCodeExistance(name, tokenModel);

            if (globalCode == null)
                return new JsonModel(new object(), GlobalCodeName.GlobalCodeNameAvailable, (int)HttpStatusCode.NotFound);
            return new JsonModel(new object(), GlobalCodeName.GlobalCodeNameTaken, (int)HttpStatusCode.OK);

        }

        public JsonModel GetGlobalCodeById(TokenModel tokenModel, string id)
        {
            _response = new JsonModel();
            if (string.IsNullOrEmpty(id))
                return new JsonModel(new object(), GlobalCodeName.GlobalCodeIdRequired, (int)HttpStatusCode.BadRequest);
            //int.TryParse(Common.CommonMethods.Decrypt(id.Replace(" ", "+")), out int serviceId);
            var globalCode = _globalCodeRepository.getGlobalCodeById(tokenModel, Convert.ToInt32(id));
            if (globalCode != null)
            {
                GlobalCodeModel globalCodeResponseModel = new GlobalCodeModel();
                //_mapper.Map(globalCode, globalCodeResponseModel);

                globalCodeResponseModel.Id =Convert.ToInt32(id);
                globalCodeResponseModel.PhotoPath = globalCode.PhotoPath;
                globalCodeResponseModel.PhotoThumbnailPath = globalCode.PhotoThumbnailPath;
                globalCodeResponseModel.OrganizationID = tokenModel.OrganizationID;
                globalCodeResponseModel.GlobalCodeName = globalCode.GlobalCodeName;
                globalCodeResponseModel.GlobalCodeCategoryID = globalCode.GlobalCodeCategoryID;
                globalCodeResponseModel.GlobalCodeValue = globalCode.GlobalCodeValue;
                globalCodeResponseModel.IsActive = globalCode.IsActive;

                //if (globalCode != null && !string.IsNullOrEmpty(globalCode.PhotoPath) && !string.IsNullOrEmpty(globalCode.PhotoThumbnailPath))
                //{
                //    globalCodeResponseModel.PhotoPath = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.SpecialityPhotos, globalCode.PhotoPath);
                //    globalCodeResponseModel.PhotoThumbnailPath = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.SpecialityThumbPhotos, globalCode.PhotoThumbnailPath);
                //}
                _response.data = globalCodeResponseModel;
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

        public JsonModel GetGlobalCodeByOrganizationId(TokenModel tokenModel, GlobalCodeFilterModel globalCodeFilterModel)
        {
            _response = new JsonModel();
            if (tokenModel.OrganizationID == 0)
                return new JsonModel(new object(), GlobalCodeName.OrganizationIdRequired, (int)HttpStatusCode.BadRequest);
            var globalCode = _globalCodeRepository.getGlobalCodeByOrganizationId(tokenModel, globalCodeFilterModel);
            //List<GlobalCode> globalCodeList = globalCode.Skip((globalCodeFilterModel.pageNumber - 1) * globalCodeFilterModel.pageSize)
            //                                   .Take(globalCodeFilterModel.pageSize).ToList<GlobalCode>();
            List<GlobalCode> globalCodeList = globalCode.ToList<GlobalCode>();
            if (globalCodeList != null && globalCodeList.Count > 0)
            {
                List<GlobalCodeModel> masterServicesResponseModel = new List<GlobalCodeModel>();
                foreach (var x in globalCodeList)
                {
                    masterServicesResponseModel.Add(new GlobalCodeModel
                    {
                        Id = x.Id,
                        GlobalCodeName = x.GlobalCodeName,
                        GlobalCodeCategoryID = x.GlobalCodeCategoryID,
                        GlobalCodeValue = x.GlobalCodeValue,
                        DisplayOrder = x.DisplayOrder,
                        value = x.value,
                        IsActive = x.IsActive,
                        IsDeleted = x.IsDeleted
                    });
                }
                //_mapper.Map(globalCodeList, masterServicesResponseModel);
                _response.data = masterServicesResponseModel;
                _response.Message = StatusMessage.FetchMessage;
                _response.StatusCode = (int)HttpStatusCode.OK;

                List<GlobalCodeModel> globalMetaModel = new List<GlobalCodeModel>();
                //_mapper.Map(globalCode.ToList(), globalMetaModel);

                foreach (var x in globalCode)
                {
                    globalMetaModel.Add(new GlobalCodeModel
                    {
                        Id = x.Id,
                        GlobalCodeName = x.GlobalCodeName,
                        GlobalCodeCategoryID = x.GlobalCodeCategoryID,
                        GlobalCodeValue = x.GlobalCodeValue,
                        DisplayOrder = x.DisplayOrder,
                        value = x.value,
                        IsActive = x.IsActive,
                        IsDeleted = x.IsDeleted
                    });
                }
                globalMetaModel[0].TotalRecords = globalMetaModel.Count;
                _response.meta = new Meta(globalMetaModel, globalCodeFilterModel);
            }
            else
            {
                _response.data = new object();
                _response.Message = StatusMessage.NotFound;
                _response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return _response;
        }
        public JsonModel GetGlobalServiceIconName(TokenModel tokenModel, GlobalCodeFilterModel globalCodeFilterModel)
        {
            _response = new JsonModel();
            //if (tokenModel.OrganizationID == 0)
            //    return new JsonModel(new object(), GlobalCodeName.OrganizationIdRequired, (int)HttpStatusCode.BadRequest);
            var globalCode = _globalCodeRepository.getGlobalServiceIconName(tokenModel, globalCodeFilterModel);
            //List<GlobalCode> globalCodeList = globalCode.Skip((globalCodeFilterModel.pageNumber - 1) * globalCodeFilterModel.pageSize)
            //                                   .Take(globalCodeFilterModel.pageSize).ToList<GlobalCode>();
            List<GlobalCode> globalCodeList = globalCode.ToList<GlobalCode>();
            if (globalCodeList != null && globalCodeList.Count > 0)
            {
                List<GlobalCodeModel> masterServicesResponseModel = new List<GlobalCodeModel>();
                foreach (var x in globalCodeList)
                {
                    masterServicesResponseModel.Add(new GlobalCodeModel
                    {
                        Id = x.Id,
                        GlobalCodeName = x.GlobalCodeName,
                        SpecialityIcon = x.PhotoThumbnailPath,
                        value = x.value,
                        IsActive = x.IsActive,
                        IsDeleted = x.IsDeleted
                    });
                }
                //_mapper.Map(globalCodeList, masterServicesResponseModel);
                _response.data = masterServicesResponseModel;
                _response.Message = StatusMessage.FetchMessage;
                _response.StatusCode = (int)HttpStatusCode.OK;

                //List<GlobalCodeModel> globalMetaModel = new List<GlobalCodeModel>();
                ////_mapper.Map(globalCode.ToList(), globalMetaModel);

                //foreach (var x in globalCode)
                //{
                //    globalMetaModel.Add(new GlobalCodeModel
                //    {
                //        Id = x.Id,
                //        GlobalCodeName = x.GlobalCodeName,
                //        GlobalCodeCategoryID = x.GlobalCodeCategoryID,
                //        GlobalCodeValue = x.GlobalCodeValue,
                //        DisplayOrder = x.DisplayOrder,
                //        value = x.value,
                //        IsActive = x.IsActive,
                //        IsDeleted = x.IsDeleted
                //    });
                //}
                //globalMetaModel[0].TotalRecords = globalMetaModel.Count;
                //_response.meta = new Meta(globalMetaModel, globalCodeFilterModel);
            }
            else
            {
                _response.data = new object();
                _response.Message = StatusMessage.NotFound;
                _response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return _response;
        }

        public int GetGlobalCodeValueId(string globalCodeCategoryName, string globalCodeValue, TokenModel token)
        {
            return _globalCodeRepository.Get(a => a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == globalCodeCategoryName.ToLower() && a.GlobalCodeValue.ToLower() == globalCodeValue.ToLower() && a.OrganizationID == token.OrganizationID && a.IsActive == true && a.IsDeleted == false).Id;
        }
        public int GetGlobalCodeValueId(string globalCodeCategoryName, string globalCodeValue, TokenModel token, bool isActive = true)
        {
            return _globalCodeRepository.Get(a => a.GlobalCodeCategory.GlobalCodeCategoryName.ToLower() == globalCodeCategoryName.ToLower() && a.GlobalCodeValue.ToLower() == globalCodeValue.ToLower() && a.OrganizationID == token.OrganizationID && a.IsActive == isActive && a.IsDeleted == false).Id;
        }
        //public JsonModel GetGlobalServiceIconName(GlobalCodeModel globalCodeModel)
        //{
        //    return _globalCodeRepository.Get(a => a.GlobalCode)
        //}

        public JsonModel SaveUpdateGlobalCode(GlobalCodeModel globalCodeModel, TokenModel tokenModel)
        {
            //if (!string.IsNullOrEmpty(globalCodeModel.SpecialityIcon))
            //{
            //    _imageService.ConvertBase64ToImageForDocSpeciality(globalCodeModel);
            //    //if(globalCodeModel.PhotoPath!= null && globalCodeModel.PhotoThumbnailPath != null)
            //    //{

            //    //}
            //}
            GlobalCode globalCode = _globalCodeRepository.CheckGlobalCodeExistanceWithPhoto(globalCodeModel, tokenModel);
            
            if (globalCode != null)
                if (string.IsNullOrEmpty(globalCodeModel.Id.ToString()) || globalCodeModel.Id.ToString() != Common.CommonMethods.Encrypt(globalCode.Id.ToString()))
                    return new JsonModel(new object(), GlobalCodeName.GlobalCodeNameAvailable, (int)HttpStatusCode.Found);

            globalCode = new GlobalCode();

            if (globalCodeModel.Id!=0)
            {
                globalCode = _globalCodeRepository.getGlobalCodeById(tokenModel, Convert.ToInt32(globalCodeModel.Id));
                globalCode.UpdatedBy = tokenModel.UserID;
                globalCode.UpdatedDate = DateTime.UtcNow;
                if (globalCodeModel.SpecialityIcon != null)
                {
                    globalCode.SpecialityIcon = globalCodeModel.SpecialityIcon;
                }
                globalCode.GlobalCodeName = globalCodeModel.GlobalCodeName;
                globalCode.GlobalCodeValue= globalCodeModel.GlobalCodeName;
                globalCode.GlobalCodeCategoryID = globalCodeModel.GlobalCodeCategoryID;
                globalCode.IsActive = globalCodeModel.IsActive;

                //if (!string.IsNullOrEmpty(globalCode.SpecialityIcon))
                //{
                //    _imageService.ConvertBase64ToImageForDocSpeciality(globalCodeModel);
                //}
                if (globalCodeModel.SpecialityIcon != null)
                {
                    if (globalCodeModel.SpecialityIcon != globalCode.PhotoPath || globalCodeModel.SpecialityIcon != globalCode.PhotoThumbnailPath)
                    {
                        globalCode.PhotoPath = globalCodeModel.SpecialityIcon;
                        globalCode.PhotoThumbnailPath = globalCodeModel.SpecialityIcon;
                    }
                }

            }
            else
            {
                globalCode.CreatedBy = tokenModel.UserID;
                globalCode.CreatedDate = DateTime.UtcNow;
                globalCode.OrganizationID = tokenModel.OrganizationID;
                globalCode.GlobalCodeName = globalCodeModel.GlobalCodeName;
               // globalCode.SpecialityIcon = globalCodeModel.SpecialityIcon;
                globalCode.GlobalCodeValue = globalCodeModel.GlobalCodeName;
                globalCode.GlobalCodeCategoryID = globalCodeModel.GlobalCodeCategoryID;
                globalCode.GlobalCodeValue = globalCode.GlobalCodeValue;
                globalCode.IsActive = globalCodeModel.IsActive;
                //globalCode.GlobalCodeId = masterServicesModel.GlobalCodeId;
                //_imageService.ConvertBase64ToImageForDocSpeciality(globalCodeModel);
                if (globalCodeModel.PhotoPath != null || globalCodeModel.PhotoThumbnailPath !=null)
                {
                    globalCode.PhotoPath = globalCodeModel.SpecialityIcon;
                    globalCode.PhotoThumbnailPath = globalCodeModel.SpecialityIcon;
                }
            }
            //_mapper.Map(globalCodeModel, globalCode);
            var services = _globalCodeRepository.saveUpdateGlobalCode(globalCode, tokenModel);

            if (services != null)
                return new JsonModel(new object(), GlobalCodeName.GlobalCodeSaved, (int)HttpStatusCode.OK);
            return new JsonModel(new object(), GlobalCodeName.GlobalCodeNotSaved, (int)HttpStatusCode.InternalServerError);

        }
        public JsonModel DeleteGlobalCode(TokenModel tokenModel, int id)
        {
            if (id==0)
                return new JsonModel(new object(), GlobalCodeName.GlobalCodeIdRequired, (int)HttpStatusCode.BadRequest);

            _response = new JsonModel();
            //int.TryParse(id, out int serviceId);
            var globalCode = _globalCodeRepository.getGlobalCodeById(tokenModel, id);
            if (globalCode != null)
            {
                globalCode.DeletedBy = tokenModel.UserID;
                globalCode.DeletedDate = DateTime.UtcNow;
                globalCode.IsDeleted = true;
                var services = _globalCodeRepository.saveUpdateGlobalCode(globalCode, tokenModel);
                if (services != null)
                    return new JsonModel(new object(), GlobalCodeName.GlobalCodeDeleted, (int)HttpStatusCode.OK);
                return new JsonModel(new object(), GlobalCodeName.GlobalCodeNotDeleted, (int)HttpStatusCode.InternalServerError);
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
