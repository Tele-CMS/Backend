using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System.Net;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientLabService : BaseService, IPatientLabService
    {
        private readonly IPatientLabRepository _patientLabRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public PatientLabService(IPatientLabRepository patientLabRepository)
        {
            _patientLabRepository = patientLabRepository;
        }

        //public JsonModel GetMedication(PatientFilterModel patientFilterModel, TokenModel tokenModel)
        //{
        //    List<PatientsMedicationModel> patientMedicationModel = _patientMedicationRepository.GetMedication<PatientsMedicationModel>(patientFilterModel, tokenModel).ToList();
        //    if (patientMedicationModel != null && patientMedicationModel.Count > 0)
        //    {
        //        response = new JsonModel(patientMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
        //        response.meta = new Meta(patientMedicationModel, patientFilterModel);
        //    }
        //    return response;
        //}

        //public JsonModel SaveMedication(PatientsMedicationModel patientMedicationModel, TokenModel tokenModel)
        //{
        //    PatientMedication patientMedication = null;
        //    if (patientMedicationModel.Id == 0)
        //    {
        //        patientMedication = new Entity.PatientMedication();
        //        AutoMapper.Mapper.Map(patientMedicationModel, patientMedication);
        //        patientMedication.CreatedBy = tokenModel.UserID;
        //        patientMedication.CreatedDate = DateTime.UtcNow;
        //        patientMedication.IsDeleted = false;
        //        patientMedication.IsActive = true;
        //        _patientMedicationRepository.Create(patientMedication);
        //        _patientMedicationRepository.SaveChanges();
        //        response = new JsonModel(patientMedication, StatusMessage.MedicationSave, (int)HttpStatusCode.OK);
        //    }
        //    else
        //    {

        //        patientMedication = _patientMedicationRepository.Get(a => a.Id == patientMedicationModel.Id && a.IsDeleted == false && a.IsActive == true);
        //        if (patientMedication != null)
        //        {
        //            AutoMapper.Mapper.Map(patientMedicationModel, patientMedication);
        //            patientMedication.UpdatedBy = tokenModel.UserID;
        //            patientMedication.UpdatedDate = DateTime.UtcNow;
        //            _patientMedicationRepository.Update(patientMedication);
        //            _patientMedicationRepository.SaveChanges();
        //            response = new JsonModel(patientMedication, StatusMessage.ClientDiagnosisUpdated, (int)HttpStatusCode.OK);
        //        }
        //    }
        //    return response;
        //}

        //public JsonModel GetMedicationById(int id, TokenModel tokenModel)
        //{
        //    PatientMedication patientMedication = _patientMedicationRepository.Get(a => a.Id == id && a.IsDeleted == false);
        //    if (patientMedication != null)
        //    {
        //        PatientsMedicationModel patientMedicationModel = new PatientsMedicationModel();
        //        AutoMapper.Mapper.Map(patientMedication, patientMedicationModel);
        //        response = new JsonModel(patientMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
        //    }
        //    return response;
        //}

        //public JsonModel DeleteMedication(int id, TokenModel tokenModel)
        //{
        //    PatientMedication patientmedication = _patientMedicationRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive==true);
        //    if (patientmedication != null)
        //    {
        //        patientmedication.IsDeleted = true;
        //        patientmedication.DeletedBy = tokenModel.UserID;
        //        patientmedication.DeletedDate = DateTime.UtcNow;
        //        _patientMedicationRepository.Update(patientmedication);
        //        _patientMedicationRepository.SaveChanges();
        //        response = new JsonModel(new object(), StatusMessage.MedicationDeleted, (int)HttpStatusCodes.OK);
        //    }
        //    return response;
        //}
    }
}
