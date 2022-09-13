using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.IServices.Staff
{
    public interface IStaffCustomLabelService : IBaseService
    {
        JsonModel GetStaffCustomLabels(int staffId, TokenModel tokenModel);
        JsonModel SaveCustomLabels(List<StaffCustomLabelModel> staffCustomLabelModels, TokenModel tokenModel);
    }
}
