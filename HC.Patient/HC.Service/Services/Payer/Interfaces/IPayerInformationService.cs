using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Payer;
using HC.Service.Interfaces;
using System.Collections.Generic;

namespace HC.Patient.Service.Payer.Interfaces
{
    public interface IPayerInformationService :  IBaseService
    {
        List<PayerServiceCodes> UpdatePayerServiceCodesToDB(List<PayerServiceCodes> PayerServiceCodes, bool IsInsert = false, bool IsUpdate = false, bool IsDelete = false);
        JsonModel GetPayerInformationByFilter(int organizationID, string payerName, int pageNumber, int pageSize, string sortColumn, string sortOrder, bool IsPayerInformation);
        JsonModel GetPayerInformationByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerInformationModel> GetMasterServiceCodesByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerInformationModel> GetPayerActivityByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerAppTypeModel> GetPayerServiceCodesByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerInformationModel> UpdatePayerInformation(PayerInfoUpdateModel payerInfoUpdateModel);
        JsonModel GetPayerServiceCodesByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID);
        JsonModel GetMasterServiceCodesByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID);
        JsonModel GetPayerActivityByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID);
        JsonModel GetPayerActivityCodesByFilter(string id, int id2, string name, int pageNumber, int pageSize, string sortColumn, string sortOrder, int organizationID);
        JsonModel GetMasterServiceCodesEx(int payerID, int organizationID);
        JsonModel GetPayerServiceCodesByPayerId(int payerID, int organizationID);        
        JsonModel GetPayerServiceCodesEx(int payerID, int activityID, int organizationID);
        JsonModel GetPayerServiceCodeDetailsById(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel token);
        JsonModel UpdatePayerAppointmentTypes(PayerAppointmentTypesModel payerAppointmentTypesModel, TokenModel token);
        JsonModel DeletePayerServiceCode(int id, TokenModel token);
        JsonModel GetMasterActivitiesForPayer(int payerId, TokenModel token);

    }
}
