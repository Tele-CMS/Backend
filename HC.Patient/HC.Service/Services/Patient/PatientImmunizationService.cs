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
    public class PatientImmunizationService : BaseService, IPatientImmunizationService
    {
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientImmunizationRepository _patientImmunizationRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IPatientAlertService _patientAlertService;
        private readonly ITokenService _tokenService;
        public PatientImmunizationService(IPatientImmunizationRepository patientImmunizationRepository,IAuditLogRepository auditLogRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository, IGlobalCodeService globalCodeService, IPatientAlertService patientAlertService, ITokenService tokenService)
        {
            _patientImmunizationRepository = patientImmunizationRepository;
            _auditLogRepository = auditLogRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
            _globalCodeService = globalCodeService;
            _patientAlertService = patientAlertService;
            _tokenService = tokenService;

        }

        public JsonModel SavePatientImmunization(PatientImmunizationModel patientImmunizationModel, TokenModel tokenModel)
        {
            PatientImmunization patientImmunization = null;
            using (var transaction = _patientImmunizationRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();

                    if (patientImmunizationModel.Id == 0)
            {
                patientImmunization = new PatientImmunization();
                AutoMapper.Mapper.Map(patientImmunizationModel, patientImmunization);
                patientImmunization.CreatedBy = tokenModel.UserID;
                patientImmunization.CreatedDate = DateTime.UtcNow;
                patientImmunization.IsActive = true;
                patientImmunization.IsDeleted = false;
                _patientImmunizationRepository.Create(patientImmunization);
                        changesLogs = _patientImmunizationRepository.GetChangesLogData(tokenModel);
                        _patientImmunizationRepository.SaveChanges();

                        //Save Alert for patient when provider adds new a value
                        var user = _tokenService.GetUserById(tokenModel);
                        if (user != null && user.UserRoles != null && user.UserRoles.RoleName != OrganizationRoles.Patient.ToString())
                        {
                            int masterAlertTypeId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.PATIENTALERTTYPE, "New immunizations", tokenModel, true);
                            bool isSuccess = _patientAlertService.SavePatientAlerts("New Immunization added", patientImmunization.PatientID, masterAlertTypeId, null, tokenModel);
                        }

                        response = new JsonModel(patientImmunization, StatusMessage.ClientImmunizationCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                patientImmunization = _patientImmunizationRepository.Get(a => a.Id == patientImmunizationModel.Id && a.IsActive == true && a.IsDeleted == false);
                if (patientImmunization != null)
                {
                    AutoMapper.Mapper.Map(patientImmunizationModel, patientImmunization);
                    patientImmunization.UpdatedBy = tokenModel.UserID;
                    patientImmunization.UpdatedDate = DateTime.UtcNow;
                            changesLogs = _patientImmunizationRepository.GetChangesLogData(tokenModel);
                            _patientImmunizationRepository.Update(patientImmunization);
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateImmunizationDetails, AuditLogAction.Modify, null, tokenModel.UserID, "" + null, tokenModel);
                            _patientImmunizationRepository.SaveChanges();
                    response = new JsonModel(patientImmunization, StatusMessage.ClientImmunizationUpdated, (int)HttpStatusCode.OK);
                }
            }
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientImmunization.Id, patientImmunizationModel.LinkedEncounterId, tokenModel);
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

        public JsonModel GetImmunization(PatientFilterModel filterModel, TokenModel tokenModel)
        {
            List<PatientImmunizationModel> patientImmunizationModel = _patientImmunizationRepository.GetImmunization<PatientImmunizationModel>(filterModel, tokenModel).ToList();
            if (patientImmunizationModel != null && patientImmunizationModel.Count > 0)
            {
                response.data = patientImmunizationModel;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.meta = new Meta(patientImmunizationModel, filterModel);
               // return response;
            }
            return response;
        }

        public JsonModel GetImmunizationById(int id, TokenModel tokenModel)
        {
            PatientImmunization patientImmunization = _patientImmunizationRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (patientImmunization != null )
            {
                PatientImmunizationModel patientImmunizationModel = new PatientImmunizationModel();
                AutoMapper.Mapper.Map(patientImmunization, patientImmunizationModel);
                response = new JsonModel(patientImmunizationModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteImmunization(int id,TokenModel tokenModel)
        {
            PatientImmunization patientImmunization = _patientImmunizationRepository.Get(a => a.Id == id && a.IsActive == true && a.IsDeleted == false);
            if (patientImmunization != null)
            {
                patientImmunization.IsDeleted = true;
                patientImmunization.DeletedBy = tokenModel.UserID;
                patientImmunization.DeletedDate = DateTime.UtcNow;
                _patientImmunizationRepository.Update(patientImmunization);
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteImmunizationDetails, AuditLogAction.Delete, null, tokenModel.UserID, "" + null, tokenModel);
                _patientImmunizationRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.ClientImmunizationDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
    }
}
