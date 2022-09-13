using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Net;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using System.Collections.Generic;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientSocialHistoryService : BaseService, IPatientSocialHistoryService
    {
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientSocialHistoryRepository _patientSocialHistoryRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;

        public PatientSocialHistoryService(IPatientSocialHistoryRepository patientSocialHistoryRepository,IAuditLogRepository auditLogRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository)
        {
            _patientSocialHistoryRepository = patientSocialHistoryRepository;
            _auditLogRepository = auditLogRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
        }

        public JsonModel SavePatientSocialHistory(PatientSocialHistoryModel patientSocialHistoryModel, TokenModel tokenModel)
        {
            PatientSocialHistory patientSocialHistory = null;
            using (var transaction = _patientSocialHistoryRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();

                    if (patientSocialHistoryModel.Id == 0)
            {
                patientSocialHistory = new PatientSocialHistory();
                AutoMapper.Mapper.Map(patientSocialHistoryModel, patientSocialHistory);
                patientSocialHistory.CreatedDate = DateTime.UtcNow;
                patientSocialHistory.CreatedBy = tokenModel.UserID;
                patientSocialHistory.IsActive = true;
                patientSocialHistory.IsDeleted = false;
                _patientSocialHistoryRepository.Create(patientSocialHistory);
                        changesLogs = _patientSocialHistoryRepository.GetChangesLogData(tokenModel);
                        _patientSocialHistoryRepository.SaveChanges();
                response = new JsonModel(patientSocialHistory, StatusMessage.ClientSocialHistoryCreated, (int)HttpStatusCode.OK);
            }
            else
            {
                patientSocialHistory = _patientSocialHistoryRepository.Get(a => a.Id == patientSocialHistoryModel.Id && a.IsActive == true && a.IsDeleted == false);
                AutoMapper.Mapper.Map(patientSocialHistoryModel, patientSocialHistory);
                patientSocialHistory.UpdatedDate = DateTime.UtcNow;
                patientSocialHistory.UpdatedBy = tokenModel.UserID;
                        changesLogs = _patientSocialHistoryRepository.GetChangesLogData(tokenModel);
                        _patientSocialHistoryRepository.Update(patientSocialHistory);
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateSocialHistoryDetails, AuditLogAction.Modify, null, tokenModel.UserID, "" + null, tokenModel);
                        //changesLogs = _patientSocialHistoryRepository.GetChangesLogData(tokenModel);
                        _patientSocialHistoryRepository.SaveChanges();
                response = new JsonModel(patientSocialHistory, StatusMessage.ClientSocialHistoryUpdated, (int)HttpStatusCode.OK);
            }

                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientSocialHistory.Id, patientSocialHistoryModel.LinkedEncounterId, tokenModel);
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

        public JsonModel GetPatientSocialHistory(int patientId, TokenModel tokenModel)
        {
            PatientSocialHistory patientSocialHistory = _patientSocialHistoryRepository.Get(a => a.PatientID == patientId && a.IsActive == true && a.IsDeleted == false);
            if (patientSocialHistory != null)
            {
                PatientSocialHistoryModel patientSocialHistoryModel = new PatientSocialHistoryModel();
                AutoMapper.Mapper.Map(patientSocialHistory, patientSocialHistoryModel);
                response = new JsonModel(patientSocialHistoryModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }
    }
}
