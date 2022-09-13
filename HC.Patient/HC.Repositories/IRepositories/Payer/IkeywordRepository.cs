using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Model.MasterData;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Payer
{
   public interface IkeywordRepository : IRepositoryBase<HealthcareKeywords>
    {
        IQueryable<T> GetKeywordList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        //List<AppointmentModel> GetProviderListToMakeAppointmentForMobile(TokenModel tokenModel, int categoryid);
        List<AppointmentModel> GetProviderListToMakeAppointmentForMobile(TokenModel tokenModel, LocationModel locationModel, AppointmentSearchModelForMobile appointmentSearchModel, int categoryid);
    }
}
