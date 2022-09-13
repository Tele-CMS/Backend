using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientCustomLabelService : IBaseService
    {
        JsonModel GetPatientCustomLabels(int patientId, TokenModel tokenModel);
        JsonModel SaveCustomLabels(List<CustomLabelModel> customLabelModels, TokenModel tokenModel);
    }
}
