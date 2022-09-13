using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientPhysicianService : BaseService, IPatientPhysicianService
    {
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientPhysicianRepository _patientPhysicianRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        public PatientPhysicianService(IPatientPhysicianRepository patientPhysicianRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository)
        {
            _patientPhysicianRepository = patientPhysicianRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
        }
        public JsonModel GetPatientPhysicianById(int patientId, FilterModel filterModel, TokenModel token)
        {
            List<PatientPhysicianModel> patientPhysicianModel = _patientPhysicianRepository.GetPatientPhysicianById<PatientPhysicianModel>(patientId, filterModel, token).ToList();
            if (patientPhysicianModel != null && patientPhysicianModel.Count > 0)
            {
                response = new JsonModel(patientPhysicianModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientPhysicianModel, filterModel);
            }
            return response;
        }

        //public JsonModel SavePatientPhysician(PatientPhysicianModel patientPhysicianModel, TokenModel tokenModel)
        //{
        //    using (var transaction = _patientPhysicianRepository.StartTransaction())
        //    {
        //        try
        //        {
        //            List<ChangesLog> changesLogs = new List<ChangesLog>();
        //            PatientPhysician patientPhysician = null;
        //            if (patientPhysicianModel.Id == 0)
        //            {
        //                patientPhysician = new PatientPhysician();
        //                AutoMapper.Mapper.Map(patientPhysicianModel, patientPhysician);
        //                patientPhysician.CreatedBy = tokenModel.UserID;
        //                patientPhysician.IsDeleted = false;
        //                _patientPhysicianRepository.Create(patientPhysician);
        //                changesLogs = _patientPhysicianRepository.GetChangesLogData(tokenModel);
        //                response = new JsonModel(patientPhysician, StatusMessage.PatientPhysicianSave, (int)HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //                patientPhysician = _patientPhysicianRepository.Get(a => a.Id == patientPhysicianModel.Id && a.IsDeleted == false);
        //                if (patientPhysician != null)
        //                {
        //                    AutoMapper.Mapper.Map(patientPhysicianModel, patientPhysician);
        //                    patientPhysician.UpdatedBy = tokenModel.UserID;
        //                    patientPhysician.UpdatedDate = DateTime.UtcNow;
        //                    _patientPhysicianRepository.Update(patientPhysician);
        //                    changesLogs = _patientPhysicianRepository.GetChangesLogData(tokenModel);
        //                    response = new JsonModel(patientPhysician, StatusMessage.PatientPhysicianUpdated, (int)HttpStatusCode.OK);
        //                }
        //            }
        //            _patientPhysicianRepository.SaveChanges();
        //            _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientPhysician.Id, patientPhysicianModel.LinkedEncounterId, tokenModel);
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw ex;
        //        }
        //    }
        //    return response;
        //}
        //public JsonModel DeletePatientPhysician(int id, int linkedEncounterId, TokenModel tokenModel)
        //{
        //    using (var transaction = _patientPhysicianRepository.StartTransaction())
        //    {
        //        try
        //        {
        //            List<ChangesLog> changesLogs = new List<ChangesLog>();
        //            PatientPhysician patientPhysician = _patientPhysicianRepository.GetByID(id);
        //            if (!ReferenceEquals(patientPhysician, null))
        //            {
        //                patientPhysician.IsDeleted = true;
        //                patientPhysician.IsActive = false;
        //                patientPhysician.DeletedBy = tokenModel.UserID;
        //                patientPhysician.DeletedDate = DateTime.UtcNow;
        //                _patientPhysicianRepository.Update(patientPhysician);
        //                changesLogs = _patientPhysicianRepository.GetChangesLogData(tokenModel);
        //                _patientPhysicianRepository.SaveChanges();
        //                response = new JsonModel(null, StatusMessage.PatientPhysicianDeleted, (int)HttpStatusCode.OK);
        //            }
        //            _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientPhysician.Id, linkedEncounterId, tokenModel);
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw ex;
        //        }
        //    }
        //    return response;
        //}

        //public JsonModel GetPatientPhysicianDataById(int id, TokenModel tokenModel)
        //{
        //    PatientPhysician patientPhysician = _patientPhysicianRepository.GetByID(id);
        //    if (patientPhysician != null)
        //    {
        //        PatientPhysicianModel patientPhysicianModel = new PatientPhysicianModel();
        //            AutoMapper.Mapper.Map(patientPhysician, patientPhysicianModel);
        //            response = new JsonModel(patientPhysicianModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK, string.Empty);
        //        }
            
        //    return response;
        //}
    }
}
