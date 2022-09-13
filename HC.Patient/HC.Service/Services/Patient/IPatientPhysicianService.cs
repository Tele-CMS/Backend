using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Service.IServices.Patient
{
   public interface IPatientPhysicianService : IBaseService
    {
        JsonModel GetPatientPhysicianById(int patientId, FilterModel filterModel, TokenModel token);
        //JsonModel SavePatientPhysician(PatientPhysicianModel patientPhysicianModel, TokenModel tokenModel);
        //JsonModel DeletePatientPhysician(int id, int linkedEncounterId, TokenModel tokenModel);
        //JsonModel GetPatientPhysicianDataById(int id, TokenModel tokenModel);
    }
}
