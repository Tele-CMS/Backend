using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HC.Patient.Repositories.IRepositories.Payer
{
    public interface IPayerActivityRepository : IRepositoryBase<PayerAppointmentTypes>
    {
        List<PayerAppointmentTypes> UpdatePayerActivityToDB(List<PayerAppointmentTypes> payerAppointmentTypes);
        List<PayerServiceCodeModifiers> GetPayerServiceCodeModifiers(int payerServiceCodeId);
        IQueryable<T> GetActivities<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetPayerActivityServiceCodes<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetMasterActivitiesForPayer<T>(SearchFilterModel searchFilterModel, TokenModel token) where T : class, new();
    }
}
