using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientCustomLabelService : BaseService, IPatientCustomLabelService
    {
        private readonly IPatientCustomLabelRepository _patientCustomLabelRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        
        public PatientCustomLabelService(IPatientCustomLabelRepository patientCustomLabelRepository)
        {
            _patientCustomLabelRepository = patientCustomLabelRepository;        
        }

        public JsonModel GetPatientCustomLabels(int patientId, TokenModel tokenModel)
        {
            try
            {
                Dictionary<string, object> patientCustomLabels = _patientCustomLabelRepository.GetPatientCustomLabel(patientId, tokenModel);
                if ((List<MasterCustomLabelModel>)patientCustomLabels["MasterCustomLabels"] != null)
                {
                    response = new JsonModel(patientCustomLabels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                }
                else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound); }
            }
            catch (Exception e) { response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message); }
            return response;
        }

        public JsonModel SaveCustomLabels(List<CustomLabelModel> customLabelModels, TokenModel tokenModel)
        {
            try
            {
                List<PatientCustomLabels> PatientCustomLabelList = null;
                PatientCustomLabels patientCustomLabels = null;
                DateTime CurrentDate = DateTime.UtcNow;
                int? patientId = 0;
                if (customLabelModels != null && customLabelModels.Exists(a => a.Id > 0))
                {
                    patientId = customLabelModels.FirstOrDefault().PatientID;
                    PatientCustomLabelList = _patientCustomLabelRepository.GetAll(a => a.PatientID == patientId && a.IsDeleted == false && a.IsActive == true).ToList();
                    foreach (var item in customLabelModels)
                    {
                        if (item.Id > 0)
                        {
                            patientCustomLabels = PatientCustomLabelList.Where(a => a.Id == item.Id).FirstOrDefault();
                            patientCustomLabels.UpdatedBy = tokenModel.UserID;
                            patientCustomLabels.UpdatedDate = CurrentDate;
                            patientCustomLabels.CustomLabelValue = item.CustomLabelValue;
                            patientCustomLabels.CustomLabelID = item.CustomLabelID;
                            patientCustomLabels.CustomLabelDataType = CommonMethods.ParseString(item.CustomLabelValue).ToString();
                            _patientCustomLabelRepository.Update(patientCustomLabels);
                        }
                        else
                        {
                            patientCustomLabels = new PatientCustomLabels();
                            AutoMapper.Mapper.Map(item, patientCustomLabels);
                            patientCustomLabels.CreatedBy = tokenModel.UserID;
                            patientCustomLabels.CreatedDate = CurrentDate;
                            patientCustomLabels.IsActive = true;
                            patientCustomLabels.IsDeleted = false;
                            patientCustomLabels.CustomLabelDataType = CommonMethods.ParseString(item.CustomLabelValue).ToString();
                            _patientCustomLabelRepository.Create(patientCustomLabels);
                        }                        
                    }
                    response = new JsonModel(PatientCustomLabelList, StatusMessage.CustomLabelUpdated, (int)HttpStatusCode.OK);
                }
                else
                {
                    PatientCustomLabelList = new List<PatientCustomLabels>();
                    AutoMapper.Mapper.Map(customLabelModels, PatientCustomLabelList);
                    PatientCustomLabelList.ForEach(a => { a.CreatedBy = tokenModel.UserID; a.CreatedDate = CurrentDate; a.IsActive = true; a.IsDeleted = false; a.CustomLabelDataType = CommonMethods.ParseString(a.CustomLabelValue).ToString(); });
                    _patientCustomLabelRepository.Create(PatientCustomLabelList.ToArray());
                    response = new JsonModel(PatientCustomLabelList, StatusMessage.PatientProfile, (int)HttpStatusCode.OK);
                }
                _patientCustomLabelRepository.SaveChanges();
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message);
            }
            return response;
        }
    }
}
