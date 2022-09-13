using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HC.Patient.Repositories.IRepositories.Payer
{
    public interface IPayerServiceCodesRepository : IRepositoryBase<Entity.PayerServiceCodes>
    {
        PayerServiceCodes AddPayerServiceCode(PayerServiceCodes payerServiceCode);
        PayerServiceCodes GetPayerServiceCodeByID(int payerServiceCodeID, TokenModel token);
        PayerServiceCodes UpdatePayerServiceCode(PayerServiceCodes serviceCode, DateTime CurrentDate);
        IQueryable<T> GetPayerServiceCodeModifiers<T>(int payerServiceCodeID, TokenModel token) where T : class, new();
        IQueryable<T> SavePayerServiceCode<T>(XElement headerElement) where T : class, new();
        IQueryable<T> GetPayerServiceCodes<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetExcludedServiceCodesFromActivity<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetPayerActivityServiceCodeDetailsById<T>(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetMasterServiceCodeExcludedFromPayerServiceCodes<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
