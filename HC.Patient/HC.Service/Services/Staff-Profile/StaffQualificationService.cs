using AutoMapper;
using HC.Common.HC.Common;
using HC.Common.Model.Staff;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Net;
namespace HC.Patient.Service.Services
{
    public class StaffQualificationService: IStaffQualificationService
    {
        private readonly IStaffQualificationRepository _staffQualificationRepository;
        private readonly IMapper _mapper;
        private JsonModel response;
        private JsonModel _response;
        public StaffQualificationService(IStaffQualificationRepository staffQualificationRepository, IMapper mapper)
        {
            _staffQualificationRepository = staffQualificationRepository;
            _mapper = mapper;
        }

        public JsonModel getStaffQualifications(TokenModel tokenModel, string staffId)
        {
            response = new JsonModel();
            if (string.IsNullOrEmpty(staffId))
                return new JsonModel(new object(), StatusMessage.StaffIdRequired, (int)HttpStatusCode.BadRequest);
            int.TryParse(staffId, out int id);
            var staffQualifications = _staffQualificationRepository.GetStaffQualification(id, tokenModel);
            if (staffQualifications != null && staffQualifications.Count > 0)
            {
                List<StaffQualificationModel> staffQualificationModels = new List<StaffQualificationModel>();
                _mapper.Map(staffQualifications, staffQualificationModels);
                response.data = staffQualificationModels;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return response;
        }
        public JsonModel SaveUpdateQualifications(StaffQualificationRequestModel staffQualificationRequestModel, TokenModel tokenModel)
        {
            try
            {
                int result = 0;
                Int32.TryParse(staffQualificationRequestModel.staffId.ToString(), out int staffId);
                int totalRecord = staffQualificationRequestModel.staffQualification.Count;
                StaffQualification qualification;
                if (staffQualificationRequestModel != null)
                {
                    staffQualificationRequestModel.staffQualification.ForEach(qualiModel =>
                    {
                        qualification = new StaffQualification();
                        qualiModel.StaffId = staffId.ToString();
                        if (!string.IsNullOrEmpty(qualiModel.Id) && qualiModel.Id != "0")
                        {
                            Int32.TryParse(Common.CommonMethods.Decrypt(qualiModel.Id), out int qId);
                            qualification = _staffQualificationRepository.GetStaffQualificationById(qId, tokenModel);
                            qualification.UpdatedBy = tokenModel.UserID;
                            qualification.UpdatedDate = DateTime.UtcNow;
                        }
                        else
                            qualification.CreatedBy = tokenModel.UserID;
                        _mapper.Map(qualiModel, qualification);
                        var staffQualification = _staffQualificationRepository.SaveUpdateStaffQualification(qualification, tokenModel);
                        if (staffQualification != null)
                            result++;
                    });

                }
                if (result > 0)
                    _response = new JsonModel(new object(), string.Format(StatusMessage.QualificationSaved, result, totalRecord), (int)HttpStatusCode.OK);
                else
                    _response = new JsonModel(new object(), StatusMessage.QualificationNotSaved, (int)HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                _response = new JsonModel(new object(), e.Message, (int)HttpStatusCode.InternalServerError);
                throw e;
            }
            return _response;
        }
    }
}
