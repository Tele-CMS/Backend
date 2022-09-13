using HC.Model;
using HC.Patient.Model.Patient;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.Patient
{
    public interface IPatientPhoneAddressService : IBaseService
    {
        JsonModel GetPatientPhoneAddress(int patientId, TokenModel tokenModel);
        JsonModel SavePhoneAddress(int patientId, PhoneAddressModel phoneAddressModel, TokenModel tokenModel);
    }
}
