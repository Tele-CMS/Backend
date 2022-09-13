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

namespace HC.Patient.Service.Services.Patient
{
    public class PatientVitalService : BaseService, IPatientVitalService
    {
        private readonly IPatientVitalRepository _patientVitalRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public PatientVitalService(IPatientVitalRepository patientVitalRepository, IAuditLogRepository auditLogRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository)
        {
            _patientVitalRepository = patientVitalRepository;
            _auditLogRepository = auditLogRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
        }

        public JsonModel GetVitals(PatientFilterModel patientFilterModel, TokenModel tokenModel)
        {
            List<PatientVitalModel> patientVitalModel = _patientVitalRepository.GetVitals<PatientVitalModel>(patientFilterModel, tokenModel).ToList();
            if (patientVitalModel != null && patientVitalModel.Count > 0)
            {
                response = new JsonModel(patientVitalModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientVitalModel, patientFilterModel);
            }
            return response;
        }

        public JsonModel SaveVital(PatientVitalModel patientVitalModel, TokenModel tokenModel)
        {
            PatientVitals patientVitals = null;
            using (var transaction = _patientVitalRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();
                    if (patientVitalModel.Id == 0)
                    {
                        patientVitals = new Entity.PatientVitals();
                        AutoMapper.Mapper.Map(patientVitalModel, patientVitals);
                        patientVitals.CreatedBy = tokenModel.UserID;
                        patientVitals.CreatedDate = DateTime.UtcNow;
                        patientVitals.IsDeleted = false;
                        patientVitals.IsActive = true;
                        patientVitals = calculateBmi(patientVitals);
                        _patientVitalRepository.Create(patientVitals);
                        changesLogs = _patientVitalRepository.GetChangesLogData(tokenModel);
                        _patientVitalRepository.SaveChanges();
                        response = new JsonModel(patientVitals, StatusMessage.VitalSave, (int)HttpStatusCode.OK);
                    }
                    else
                    {

                        patientVitals = _patientVitalRepository.Get(a => a.Id == patientVitalModel.Id && a.IsDeleted == false && a.IsActive == true);
                        if (patientVitals != null)
                        {
                            AutoMapper.Mapper.Map(patientVitalModel, patientVitals);
                            patientVitals.UpdatedBy = tokenModel.UserID;
                            patientVitals.UpdatedDate = DateTime.UtcNow;
                            patientVitals = calculateBmi(patientVitals);
                            changesLogs = _patientVitalRepository.GetChangesLogData(tokenModel);
                            _patientVitalRepository.Update(patientVitals);
                            _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateVitalDetails, AuditLogAction.Modify, null, tokenModel.UserID, "" + null, tokenModel);
                            _patientVitalRepository.SaveChanges();
                            response = new JsonModel(patientVitals, StatusMessage.VitalUpdated, (int)HttpStatusCode.OK);
                        }
                    }
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientVitals.Id, patientVitalModel.LinkedEncounterId, tokenModel);
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

        public JsonModel GetVitalById(int id, TokenModel tokenModel)
        {
            PatientVitals patientVitals = _patientVitalRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (patientVitals != null)
            {
                PatientVitalModel patientVitalModel = new PatientVitalModel();
                AutoMapper.Mapper.Map(patientVitals, patientVitalModel);
                response = new JsonModel(patientVitalModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteVital(int id, TokenModel tokenModel)
        {
            PatientVitals patientVitals = _patientVitalRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
            if (patientVitals != null)
            {
                patientVitals.IsDeleted = true;
                patientVitals.DeletedBy = tokenModel.UserID;
                patientVitals.DeletedDate = DateTime.UtcNow;
                _patientVitalRepository.Update(patientVitals);
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.deleteVitalDetails, AuditLogAction.Delete, null, tokenModel.UserID, "" + null, tokenModel);
                _patientVitalRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.VitalDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }


        #region Helping Methods
        /// <summary>
        /// Calculate BMI and update relevant fields
        /// </summary>
        /// <param name="patientVitals"></param>
        /// <returns></returns>
        private static Entity.PatientVitals calculateBmi(Entity.PatientVitals patientVitals)
        {
            double? weightKg = 0;
            double? heightCm = 0;
            if (patientVitals.WeightLbs > 0)
            {
                //convert lbs into pound (.45 is 1kg value in pounds)
                weightKg = Math.Round((double)(patientVitals.WeightLbs * .45), 2);
            }

            if (patientVitals.HeightIn > 0)
            {
                ////convert height of feet and inches into cm
                //heightCm = Math.Round((double)((patientVitals.HeightFt * 12) + patientVitals.HeightIn) * 2.54, 2);
                //convert height of inches into cm
                heightCm = Math.Round((double)(patientVitals.HeightIn) * 2.54, 2);
            }

            //var height = patientVitals.Height_cm;
            //var weight = patientVitals.Weight_kg;

            if (heightCm > 0 && weightKg > 0)
            {
                //calculate BMI
                patientVitals.BMI = Math.Round((double)(weightKg / (heightCm / 100 * heightCm / 100)), 2);

                //if (patientVitals.BMI < 18.5)
                //{
                //    patientVitals.BMI_Status = "Below Normal";
                //}
                //if (patientVitals.BMI > 18.5 && patientVitals.BMI < 25)
                //{
                //    patientVitals.BMI_Status = "Normal";
                //}
                //if (patientVitals.BMI > 25)
                //{
                //    patientVitals.BMI_Status = "Overweight";
                //}
            }
            return patientVitals;
        }
        #endregion
    }
}
