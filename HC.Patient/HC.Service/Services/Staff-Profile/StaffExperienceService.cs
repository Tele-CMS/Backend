using AutoMapper;
using HC.Common.HC.Common;
using HC.Common.Model.Staff;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Net;

namespace HC.Patient.Service.Services
{
    public class StaffExperienceService : BaseService, IStaffExperienceService
    {
        private readonly IStaffExperienceRepository _staffExperienceRepository;
        private readonly IMapper _mapper;
        private JsonModel response;
        private JsonModel _response;
        public StaffExperienceService(IStaffExperienceRepository staffExperienceRepository, IMapper mapper)
        {
            _staffExperienceRepository = staffExperienceRepository;
            _mapper = mapper;
        }

        public JsonModel getStaffExperiences(TokenModel tokenModel, string staffId)
        {
            response = new JsonModel();
            if (string.IsNullOrEmpty(staffId))
                return new JsonModel(new object(), StatusMessage.StaffIdRequired, (int)HttpStatusCode.BadRequest);
            int.TryParse(staffId, out int id);
            var staffExperiences = _staffExperienceRepository.GetStaffExperience(id, tokenModel);
            if (staffExperiences != null && staffExperiences.Count > 0)
            {
                List<StaffExperienceModel> staffExperienceModels = new List<StaffExperienceModel>();
                _mapper.Map(staffExperiences, staffExperienceModels);
                response.data = staffExperienceModels;
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
        public JsonModel SaveUpdateExperience(StaffExperienceRequestModel staffExperienceModels, TokenModel tokenModel)
        {
            try
            {
                int result = 0;
                Int32.TryParse(staffExperienceModels.staffId.ToString(), out int staffId);
                int totalRecord = staffExperienceModels.staffExperiences.Count;
                StaffExperience experience;
                if (staffExperienceModels != null)
                {
                    staffExperienceModels.staffExperiences.ForEach(expModel =>
                    {
                        experience = new StaffExperience();
                        expModel.StaffId = staffId.ToString();
                        if (!string.IsNullOrEmpty(expModel.Id) && expModel.Id != "0")
                        {
                            Int32.TryParse(Common.CommonMethods.Decrypt(expModel.Id), out int expId);
                            experience = _staffExperienceRepository.GetStaffExperienceById(expId, tokenModel);
                            experience.UpdatedBy = tokenModel.UserID;
                            experience.UpdatedDate = DateTime.UtcNow;
                        }
                        else
                            experience.CreatedBy = tokenModel.UserID;
                        _mapper.Map(expModel, experience);
                        var staffExperience = _staffExperienceRepository.SaveUpdateStaffExperience(experience, tokenModel);
                        if (staffExperience != null)
                            result++;
                    });

                }
                if (result > 0)
                    _response = new JsonModel(new object(), string.Format(StatusMessage.ExperienceSaved, result, totalRecord), (int)HttpStatusCode.OK);
                else
                    _response = new JsonModel(new object(), StatusMessage.ExperienceSaved, (int)HttpStatusCode.BadRequest);
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
