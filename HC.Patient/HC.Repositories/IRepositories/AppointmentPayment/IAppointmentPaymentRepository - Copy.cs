using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories
{
   public interface IAppointmentPaymentRefundRepository : IRepositoryBase<AppointmentPaymentRefund>
    {
        IQueryable<T> GetTotalAppointmentRefund<T>(TokenModel token, int staffId=0) where T : class, new();
    }
}
