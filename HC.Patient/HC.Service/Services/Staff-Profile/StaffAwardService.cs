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
    public class StaffAwardService : IStaffAwardService
    {
        private readonly IStaffAwardRepository _staffAwardRepository;
        private readonly IMapper _mapper;
        private JsonModel _response;
        public StaffAwardService(IStaffAwardRepository staffAwardRepository, IMapper mapper)
        {
            _staffAwardRepository = staffAwardRepository;
            _mapper = mapper;
        }

        public JsonModel getStaffAwards(TokenModel tokenModel, string staffId)
        {
            _response = new JsonModel();
            if (string.IsNullOrEmpty(staffId))
                return new JsonModel(new object(), StatusMessage.StaffIdRequired, (int)HttpStatusCode.BadRequest);
            int.TryParse(staffId, out int id);
            var staffAwards = _staffAwardRepository.GetStaffAward(id, tokenModel);
            if (staffAwards != null && staffAwards.Count > 0)
            {
                List<StaffAwardModel> staffAwardModels = new List<StaffAwardModel>();
                _mapper.Map(staffAwards, staffAwardModels);
                _response.data = staffAwardModels;
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
        public JsonModel SaveUpdateAwards(StaffAwardRequestModel staffAwardRequestModel, TokenModel tokenModel)
        {
            try
            {
                int result = 0;
                Int32.TryParse(staffAwardRequestModel.staffId.ToString(), out int staffId);
                int totalRecord = staffAwardRequestModel.staffAward.Count;
                StaffAward award;
                if (staffAwardRequestModel != null)
                {
                    staffAwardRequestModel.staffAward.ForEach(staffAward =>
                    {
                        award = new StaffAward();
                        staffAward.StaffId = staffId.ToString();
                        if (!string.IsNullOrEmpty(staffAward.Id) && staffAward.Id != "0")
                        {
                            Int32.TryParse(Common.CommonMethods.Decrypt(staffAward.Id), out int awardId);
                            award = _staffAwardRepository.GetStaffAwardById(awardId, tokenModel);
                            award.UpdatedBy = tokenModel.UserID;
                            award.UpdatedDate = DateTime.UtcNow;
                        }
                        else
                            award.CreatedBy = tokenModel.UserID;
                        _mapper.Map(staffAward, award);
                        var sAward = _staffAwardRepository.SaveUpdateStaffAward(award, tokenModel);
                        if (sAward != null)
                            result++;
                    });

                }
                if (result > 0)
                    _response = new JsonModel(new object(), string.Format(StatusMessage.AwardSaved, result, totalRecord), (int)HttpStatusCode.OK);
                else
                    _response = new JsonModel(new object(), StatusMessage.AwardNotSaved, (int)HttpStatusCode.BadRequest);
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
