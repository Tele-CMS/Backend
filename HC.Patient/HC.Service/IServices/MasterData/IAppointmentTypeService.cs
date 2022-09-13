using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IAppointmentTypeService : IBaseService
    {
        JsonModel GetAppointmentType(SearchFilterModel searchFilterModel, TokenModel tokenModel);
        JsonModel SaveAppointmentType(AppointmentTypesModel appointmentTypesModel, TokenModel tokenModel);
        JsonModel GetAppointmentTypeById(int id, TokenModel tokenModel);
        JsonModel DeleteAppointmentType(int id, TokenModel tokenModel);
    }
}
