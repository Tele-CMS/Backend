using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientMedicationService : BaseService, IPatientMedicationService
    {
        private readonly IPatientMedicationRepository _patientMedicationRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IPatientAlertService _patientAlertService;
        private readonly ITokenService _tokenService;

        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public PatientMedicationService(IPatientMedicationRepository patientMedicationRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository, IGlobalCodeService globalCodeService, IPatientAlertService patientAlertService, ITokenService tokenService)
        {
            _patientMedicationRepository = patientMedicationRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
            _globalCodeService = globalCodeService;
            _patientAlertService = patientAlertService;
            _tokenService = tokenService;
        }

        public JsonModel GetMedication(PatientFilterModel patientFilterModel, TokenModel tokenModel)
        {
            List<PatientsMedicationModel> patientMedicationModel = _patientMedicationRepository.GetMedication<PatientsMedicationModel>(patientFilterModel, tokenModel).ToList();
            if (patientMedicationModel != null && patientMedicationModel.Count > 0)
            {
                response = new JsonModel(patientMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientMedicationModel, patientFilterModel);
            }
            return response;
        }

        public JsonModel SaveMedication(PatientsMedicationModel patientMedicationModel, TokenModel tokenModel)
        {
            PatientMedication patientMedication = null;
            using (var transaction = _patientMedicationRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();

                    if (patientMedicationModel.Id == 0)
            {
                patientMedication = new Entity.PatientMedication();
                AutoMapper.Mapper.Map(patientMedicationModel, patientMedication);
                patientMedication.CreatedBy = tokenModel.UserID;
                patientMedication.CreatedDate = DateTime.UtcNow;
                patientMedication.IsDeleted = false;
                patientMedication.IsActive = true;
                _patientMedicationRepository.Create(patientMedication);
                        changesLogs = _patientMedicationRepository.GetChangesLogData(tokenModel);
                        _patientMedicationRepository.SaveChanges();

                        //Save Alert for patient when provider adds new a value
                        var user = _tokenService.GetUserById(tokenModel);
                        if (user != null && user.UserRoles != null && user.UserRoles.RoleName != OrganizationRoles.Patient.ToString())
                        {
                            int masterAlertTypeId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.PATIENTALERTTYPE, "New medication", tokenModel, true);
                            bool isSuccess = _patientAlertService.SavePatientAlerts("New Medication added", patientMedication.PatientID, masterAlertTypeId, null, tokenModel);
                        }
                response = new JsonModel(patientMedication, StatusMessage.MedicationSave, (int)HttpStatusCode.OK);
            }
            else
            {

                patientMedication = _patientMedicationRepository.Get(a => a.Id == patientMedicationModel.Id && a.IsDeleted == false && a.IsActive == true);
                if (patientMedication != null)
                {
                    AutoMapper.Mapper.Map(patientMedicationModel, patientMedication);
                    patientMedication.UpdatedBy = tokenModel.UserID;
                    patientMedication.UpdatedDate = DateTime.UtcNow;
                            changesLogs = _patientMedicationRepository.GetChangesLogData(tokenModel);
                            _patientMedicationRepository.Update(patientMedication);
                    patientMedication.IsActive = true;
                    patientMedication.IsDeleted = false;
                            _patientMedicationRepository.SaveChanges();
                    response = new JsonModel(patientMedication, StatusMessage.ClientDiagnosisUpdated, (int)HttpStatusCode.OK);
                }
            }
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientMedication.Id, patientMedicationModel.LinkedEncounterId, tokenModel);
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

        public JsonModel GetMedicationById(int id, TokenModel tokenModel)
        {
            PatientMedication patientMedication = _patientMedicationRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (patientMedication != null)
            {
                PatientsMedicationModel patientMedicationModel = new PatientsMedicationModel();
                AutoMapper.Mapper.Map(patientMedication, patientMedicationModel);
                response = new JsonModel(patientMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeleteMedication(int id, TokenModel tokenModel)
        {
            PatientMedication patientmedication = _patientMedicationRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive==true);
            if (patientmedication != null)
            {
                patientmedication.IsDeleted = true;
                patientmedication.DeletedBy = tokenModel.UserID;
                patientmedication.DeletedDate = DateTime.UtcNow;
                _patientMedicationRepository.Update(patientmedication);
                _patientMedicationRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.MedicationDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }
   
    }
}
