using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model.PatientAppointment;
using HC.Patient.Repositories.IRepositories.CareManager;
using HC.Patient.Service.IServices.CareManager;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.CareManager
{
    public class CareManagerService : BaseService, ICareManagerService
    {
        private readonly ICareManagerRepository _careManagerRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientService _IPatientService;
        public CareManagerService(IPatientService patientService, ICareManagerRepository careManagerRepository)
        {
            _careManagerRepository = careManagerRepository;
            _IPatientService = patientService;
        }
        public JsonModel GetCareManagerTeamList(int patientId, CommonFilterModel filterModel, TokenModel token)
        {
            //if (!_IPatientService.IsValidUserForDataAccess(token, patientId))
            //{
            //    return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
            //}
            List<CareManagerModel> careManagerModel = _careManagerRepository.GetCareManagerTeamList<CareManagerModel>(patientId, filterModel, token).ToList();
            careManagerModel.ForEach(a =>
            {
                if (!string.IsNullOrEmpty(a.PhotoThumbnailPath))
                    a.PhotoThumbnailPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, a.PhotoThumbnailPath);
                else
                    a.PhotoThumbnailPath = "";

                if (!string.IsNullOrEmpty(a.PhotoPath))
                    a.PhotoPath = CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffPhotos, a.PhotoPath);
                else
                    a.PhotoPath = "";
            });
            return response = new JsonModel()
            {
                data = careManagerModel,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };

        }
        public JsonModel GetCareManagerList(TokenModel token)
        {
            try
            {
                List<CareManagerModel> careManagerModels = _careManagerRepository.GetCareManagerList<CareManagerModel>(token).ToList();                
                return response = new JsonModel()
                {
                    data = careManagerModels,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception)
            {
                return response = new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }
        }
        public JsonModel AssignAndRemoveCareManagerToAllPatient(int careTeamMemberId, bool isAttached, TokenModel token)
        {
            JsonModel response = new JsonModel();
            bool result = _careManagerRepository.AssignAndRemoveCareManagerToAllPatient(careTeamMemberId, isAttached, token);

            if (result)
            {
                response = new JsonModel()
                {
                    data = result,
                    Message = isAttached == true ? StatusMessage.CareManagerAttachSuccess : StatusMessage.CareManagerRemoveSuccess,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }

            else
            {
                response = new JsonModel()
                {
                    data = result,
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.InternalServerError
                };
            }

            return response;
        }

    }
}
