using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.Token.Interfaces;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientDiagnosisService :BaseService, IPatientDiagnosisService
    {
        private readonly IPatientDiagnosisRepository _patientDiagnosisRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IPatientAlertService _patientAlertService;
        private readonly ITokenService _tokenService;
        JsonModel response = new JsonModel(new object(),StatusMessage.NotFound,(int)HttpStatusCode.NotFound);
        public PatientDiagnosisService(IPatientDiagnosisRepository patientDiagnosisRepository, IAuditLogRepository auditLogRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository, IGlobalCodeService globalCodeService, IPatientAlertService patientAlertService, ITokenService tokenService)
        {
            _patientDiagnosisRepository = patientDiagnosisRepository;
            _auditLogRepository = auditLogRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
            _globalCodeService = globalCodeService;
            _patientAlertService = patientAlertService;
            _tokenService = tokenService;
        }

        public JsonModel SavePatientDiagnosis(PatientDiagnosisModel patientDiagnosisModel,TokenModel tokenModel)
        {
            PatientDiagnosis patientDiagnosis = null;
            using (var transaction = _patientDiagnosisRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();

                    if (patientDiagnosisModel.Id == 0)
            {
                patientDiagnosis = new PatientDiagnosis();
                AutoMapper.Mapper.Map(patientDiagnosisModel, patientDiagnosis);
                patientDiagnosis.CreatedBy = tokenModel.UserID;
                patientDiagnosis.CreatedDate = DateTime.UtcNow;                
                patientDiagnosis.IsDeleted = false;
                _patientDiagnosisRepository.Create(patientDiagnosis);
                        changesLogs = _patientDiagnosisRepository.GetChangesLogData(tokenModel);
                        _patientDiagnosisRepository.SaveChanges();

                        //Save Alert for patient when provider adds new a value
                        var user = _tokenService.GetUserById(tokenModel);
                        if (user != null && user.UserRoles != null && user.UserRoles.RoleName != OrganizationRoles.Patient.ToString())
                        {
                            int masterAlertTypeId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.PATIENTALERTTYPE, "New diagnosis", tokenModel, true);
                            bool isSuccess = _patientAlertService.SavePatientAlerts("New Diagnosis added", patientDiagnosis.PatientID, masterAlertTypeId, null, tokenModel);
                        }
                        
                        response = new JsonModel(patientDiagnosis, StatusMessage.ClientDiagnosisCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                patientDiagnosis = _patientDiagnosisRepository.Get(a => a.Id == patientDiagnosisModel.Id && a.IsDeleted == false);
                if (patientDiagnosis != null)
                {
                    AutoMapper.Mapper.Map(patientDiagnosisModel, patientDiagnosis);
                    patientDiagnosis.UpdatedBy = tokenModel.UserID;
                    patientDiagnosis.UpdatedDate = DateTime.UtcNow;
                            changesLogs = _patientDiagnosisRepository.GetChangesLogData(tokenModel);
                            _patientDiagnosisRepository.Update(patientDiagnosis);
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateDiagnosisDetails, AuditLogAction.Modify, null, tokenModel.UserID, "" + null, tokenModel);
                            _patientDiagnosisRepository.SaveChanges();
                    response = new JsonModel(patientDiagnosis, StatusMessage.ClientDiagnosisUpdated, (int)HttpStatusCode.OK);
                }
            }
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientDiagnosis.Id, patientDiagnosisModel.LinkedEncounterId, tokenModel);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return response;
        }

        public JsonModel GetDiagnosis(PatientFilterModel filterModel, TokenModel tokenModel)
        {

            response.data = new object();
            response.Message = StatusMessage.ErrorOccured;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            List<PatientDiagnosisModel> patientDiagnosisModel = _patientDiagnosisRepository.GetDiagnosis<PatientDiagnosisModel>(filterModel, tokenModel).ToList();
            if(patientDiagnosisModel!=null && patientDiagnosisModel.Count > 0)
            {
                response.data = patientDiagnosisModel;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.Created;
                response.meta = new Meta(patientDiagnosisModel, filterModel);
                return response;
            }
            return response;
        }

        public JsonModel GetDiagnosisById(int id, TokenModel tokenModel)
        {
            PatientDiagnosis patientDiagnosis = _patientDiagnosisRepository.Get(a=>a.Id==id && a.IsDeleted==false);
            if (patientDiagnosis != null)
            {
                PatientDiagnosisModel patientDiagnosisModel = new PatientDiagnosisModel();
                AutoMapper.Mapper.Map(patientDiagnosis, patientDiagnosisModel);
                response = new JsonModel(patientDiagnosisModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteDiagnosis(int id, TokenModel tokenModel)
        {
            PatientDiagnosis patientDiagnosis = _patientDiagnosisRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (patientDiagnosis != null)
            {
                patientDiagnosis.IsDeleted = true;
                patientDiagnosis.DeletedBy = tokenModel.UserID;
                patientDiagnosis.DeletedDate = DateTime.UtcNow;
                _patientDiagnosisRepository.Update(patientDiagnosis);
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteDiagnosisDetails, AuditLogAction.Delete, null, tokenModel.UserID, "" + null, tokenModel);
                _patientDiagnosisRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.ClientDiagnosisDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}
