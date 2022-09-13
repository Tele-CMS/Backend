using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.Interfaces;
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
    public class PatientGuardianService : BaseService, IPatientGuardianService
    {
        private readonly IPatientGuardianRepository _patientGuardianRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        JsonModel response = new JsonModel();

        public PatientGuardianService(IPatientGuardianRepository patientGuardianRepository, IUserCommonRepository userCommonRepository)
        {
            _patientGuardianRepository = patientGuardianRepository;
            _userCommonRepository = userCommonRepository;
        }

        public JsonModel GetPatientGuardian(PatientGuartdianFilterModel patientGuardianModel, TokenModel tokenModel)
        {
            try
            {
                List<PatientGuardianModel> patientGuardianModels = _patientGuardianRepository.GetPatientGuardian<PatientGuardianModel>(patientGuardianModel, tokenModel).ToList();
                if (patientGuardianModels != null && patientGuardianModels.Count > 0)
                {
                    response = new JsonModel(patientGuardianModels, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
                    response.meta = new Meta(patientGuardianModels, patientGuardianModel);
                }
                else
                {
                    response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
                }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCodes.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel CreateUpdatePatientGuardian(PatientGuardianModel patientGuardianModel, TokenModel tokenModel)
        {
            try
            {
                PatientGuardian patientGuardian = null;
                DateTime currentDate = DateTime.UtcNow;
                if (patientGuardianModel != null && patientGuardianModel.Id == 0)
                {
                    patientGuardian = new PatientGuardian();
                    patientGuardian.CreatedBy = tokenModel.UserID;
                    patientGuardian.CreatedDate = currentDate;
                    patientGuardian.GuardianAddress1 = patientGuardianModel.GuardianAddress1;
                    patientGuardian.GuardianAddress2 = patientGuardianModel.GuardianAddress2;
                    patientGuardian.GuardianCity = patientGuardianModel.GuardianCity;
                    patientGuardian.GuardianEmail = patientGuardianModel.GuardianEmail;
                    patientGuardian.GuardianFirstName = patientGuardianModel.GuardianFirstName;
                    patientGuardian.GuardianHomePhone = patientGuardianModel.GuardianHomePhone;
                    patientGuardian.GuardianLastName = patientGuardianModel.GuardianLastName;
                    patientGuardian.GuardianMiddleName = patientGuardianModel.GuardianMiddleName;
                    patientGuardian.GuardianMobile = patientGuardianModel.GuardianMobile;
                    patientGuardian.GuardianState = patientGuardianModel.GuardianState;
                    patientGuardian.GuardianWorkPhone = patientGuardianModel.GuardianWorkPhone;
                    patientGuardian.GuardianZip = patientGuardianModel.GuardianZip;
                    patientGuardian.IsActive = true;
                    patientGuardian.PatientID = patientGuardianModel.PatientID;
                    patientGuardian.IsDeleted = false;
                    patientGuardian.RelationshipID = patientGuardianModel.RelationshipID;
                    patientGuardian.OtherRelationshipName = patientGuardianModel.OtherRelationshipName;
                    patientGuardian.IsGuarantor = patientGuardianModel.IsGuarantor;
                    _patientGuardianRepository.Create(patientGuardian);
                    response = new JsonModel(patientGuardian, StatusMessage.ClientGuardianCreated, (int)HttpStatusCode.OK);
                }
                else
                {
                    patientGuardian = _patientGuardianRepository.Get(a => a.Id == patientGuardianModel.Id && a.IsDeleted == false);
                    patientGuardian.UpdatedBy = tokenModel.UserID;
                    patientGuardian.UpdatedDate = currentDate;
                    patientGuardian.GuardianAddress1 = patientGuardianModel.GuardianAddress1;
                    patientGuardian.GuardianAddress2 = patientGuardianModel.GuardianAddress2;
                    patientGuardian.GuardianCity = patientGuardianModel.GuardianCity;
                    patientGuardian.GuardianEmail = patientGuardianModel.GuardianEmail;
                    patientGuardian.GuardianFirstName = patientGuardianModel.GuardianFirstName;
                    patientGuardian.GuardianHomePhone = patientGuardianModel.GuardianHomePhone;
                    patientGuardian.GuardianLastName = patientGuardianModel.GuardianLastName;
                    patientGuardian.GuardianMiddleName = patientGuardianModel.GuardianMiddleName;
                    patientGuardian.GuardianMobile = patientGuardianModel.GuardianMobile;
                    patientGuardian.GuardianState = patientGuardianModel.GuardianState;
                    patientGuardian.GuardianWorkPhone = patientGuardianModel.GuardianWorkPhone;
                    patientGuardian.GuardianZip = patientGuardianModel.GuardianZip;
                    patientGuardian.IsActive = patientGuardianModel.IsActive;
                    patientGuardian.PatientID = patientGuardianModel.PatientID;
                    patientGuardian.RelationshipID = patientGuardianModel.RelationshipID;
                    patientGuardian.OtherRelationshipName = patientGuardianModel.OtherRelationshipName;
                    patientGuardian.IsGuarantor = patientGuardianModel.IsGuarantor;
                    _patientGuardianRepository.Update(patientGuardian);
                    response = new JsonModel(patientGuardian, StatusMessage.ClientGuardianUpdated, (int)HttpStatusCode.OK);
                }
                _patientGuardianRepository.SaveChanges();
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel GetPatientGuardianById(int id, TokenModel tokenModel)
        {
            try
            {
                PatientGuardian patientGuardian = _patientGuardianRepository.GetPatientGuardianById(id, tokenModel);
                if (patientGuardian != null)
                {
                    PatientGuardianModel patientGuardianModel = new PatientGuardianModel();
                    patientGuardianModel.Id = patientGuardian.Id;
                    patientGuardianModel.GuardianFirstName = patientGuardian.GuardianFirstName;
                    patientGuardianModel.GuardianLastName = patientGuardian.GuardianLastName;
                    patientGuardianModel.GuardianMiddleName = patientGuardian.GuardianMiddleName;
                    patientGuardianModel.GuardianEmail = patientGuardian.GuardianEmail;
                    patientGuardianModel.GuardianHomePhone = patientGuardian.GuardianHomePhone;
                    patientGuardianModel.GuardianWorkPhone = patientGuardian.GuardianWorkPhone;
                    patientGuardianModel.IsGuarantor = patientGuardian.IsGuarantor;
                    patientGuardianModel.RelationshipID = patientGuardian.RelationshipID;
                    patientGuardianModel.OtherRelationshipName = patientGuardian.OtherRelationshipName;
                    patientGuardianModel.IsActive = patientGuardian.IsActive;
                    response = new JsonModel(patientGuardianModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
                }
            }
            catch (Exception e)
            {
                response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message);
            }
            return response;
        }

        public JsonModel Delete(int id, TokenModel tokenModel)
        {
            try
            {
                List<RecordDependenciesModel> recordDependenciesModel = _userCommonRepository.CheckRecordDepedencies<RecordDependenciesModel>(id, DatabaseTables.PatientGuardian, true, tokenModel).ToList();
                if (recordDependenciesModel != null && recordDependenciesModel.Exists(a => a.TotalCount > 0))
                { response = new JsonModel(new object(), StatusMessage.AlreadyExists, (int)HttpStatusCodes.Unauthorized); }
                else
                {
                    PatientGuardian patientGuardian = _patientGuardianRepository.GetPatientGuardianById(id, tokenModel);
                    if (patientGuardian != null)
                    {
                        patientGuardian.IsDeleted = true;
                        patientGuardian.DeletedBy = tokenModel.UserID;
                        patientGuardian.UpdatedBy = tokenModel.UserID;
                        patientGuardian.DeletedDate = DateTime.UtcNow;
                        _patientGuardianRepository.Update(patientGuardian);
                        _patientGuardianRepository.SaveChanges();
                        response = new JsonModel(new object(), StatusMessage.ClientGuardianDelete, (int)HttpStatusCodes.OK);
                    }
                    else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound); }
                }
            }
            catch (Exception e)
            { response = new JsonModel(new object(), StatusMessage.ErrorOccured, (int)HttpStatusCode.InternalServerError, e.Message); }
            return response;
        }
    }
}
