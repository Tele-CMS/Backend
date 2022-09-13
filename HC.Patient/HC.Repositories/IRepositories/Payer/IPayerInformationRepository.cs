using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.Payer;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Payer
{
    public interface IPayerInformationRepository : IRepositoryBase<PayerServiceCodes>
    {
        List<PayerServiceCodes> UpdatePayerServiceCodesToDB(List<PayerServiceCodes> PayerServiceCodes, bool IsInsert = false, bool IsUpdate = false, bool IsDelete = false);
        List<PayerInformationModel> GetPayerInformationByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerInformationModel> GetMasterServiceCodesByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerInformationModel> GetPayerActivityByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerAppTypeModel> GetPayerServiceCodesByFilter(PayerSearchFilter payerSearchFilter);
        List<PayerInformationModel> UpdatePayerInformation(PayerInfoUpdateModel payerInfoUpdateModel);
        List<MasterDropDown> GetMasterServiceCodesEx(int payerID, int organizationID);
        List<MasterDropDown> GetPayerServiceCodesByPayerId(int payerID, int organizationID);
        List<MasterDropDown> GetPayerServiceCodesEx(int payerID, int activityID, int organizationID);
        IQueryable<T> GetPayerServiceCodeDetailsById<T>(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetMasterActivitiesForPayer<T>(int payerId, TokenModel token) where T : class, new();
    }
}
