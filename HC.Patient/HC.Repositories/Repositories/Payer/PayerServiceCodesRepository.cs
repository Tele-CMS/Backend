using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payer
{
    public class PayerServiceCodesRepository : RepositoryBase<Entity.PayerServiceCodes>, IPayerServiceCodesRepository
    {
        private HCOrganizationContext _context;
        public PayerServiceCodesRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        public PayerServiceCodes AddPayerServiceCode(PayerServiceCodes payerServiceCode)
        {
            try
            {
                _context.PayerServiceCodes.Add(payerServiceCode);
                _context.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
            return payerServiceCode;
        }

        public PayerServiceCodes GetPayerServiceCodeByID(int payerServiceCodeID, TokenModel token)
        {
            PayerServiceCodes payerServiceCode = _context.PayerServiceCodes
                    .Where(x => x.Id == payerServiceCodeID && x.IsDeleted == false && x.IsActive == true)
                    .Include(x => x.PayerServiceCodeModifiers)
                    //.Where(f => f.PayerServiceCodeModifiers).Any(z => z.IsActive == true && z.IsDeleted == false))
                    .FirstOrDefault();

            return payerServiceCode;
        }

        public IQueryable<T> GetPayerServiceCodeModifiers<T>(int payerServiceCodeID, TokenModel token) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PayerServiceCodeId", payerServiceCodeID) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetPayerServiceCodeModifiers.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public PayerServiceCodes UpdatePayerServiceCode(PayerServiceCodes serviceCode, DateTime CurrentDate)
        {
            try
            {
                if (serviceCode.PayerServiceCodeModifiers.Any(x => x.DeletedDate == CurrentDate || x.UpdatedDate == CurrentDate))
                {
                    foreach (var item in serviceCode.PayerServiceCodeModifiers.Where(x => x.UpdatedDate == CurrentDate || x.DeletedDate == CurrentDate))
                    {
                        _context.Entry(item).State = EntityState.Modified;
                    }
                }

                if (serviceCode.PayerServiceCodeModifiers.Any(x => x.CreatedDate == CurrentDate))
                {
                    _context.PayerServiceCodeModifiers.AddRange(serviceCode.PayerServiceCodeModifiers.Where(x => x.CreatedDate == CurrentDate));
                }
                _context.Entry(serviceCode).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            return serviceCode;
        }

        public IQueryable<T> SavePayerServiceCode<T>(XElement headerElement) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Data", headerElement.ToString()) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_SaveUpdatePayerServiceCode.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPayerServiceCodes<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationID",tokenModel.OrganizationID)
                                         ,new SqlParameter("@PayerId",searchFilterModel.PayerId)
                                         ,new SqlParameter("@SearchText",searchFilterModel.SearchText)
                                         ,new SqlParameter("@PageNumber",searchFilterModel.pageNumber)
                                         ,new SqlParameter("@PageSize",searchFilterModel.pageSize)
                                         ,new SqlParameter("@SortColumn",searchFilterModel.sortColumn)
                                         ,new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetPayerOrMasterServiceCodes, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetExcludedServiceCodesFromActivity<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationID",tokenModel.OrganizationID)
                                         ,new SqlParameter("@PayerId",searchFilterModel.PayerId)
                                         ,new SqlParameter("@ActivityId",searchFilterModel.ActivityId) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetExcludedServiceCodesFromActivity, parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPayerActivityServiceCodeDetailsById<T>(int payerAppointmentTypeId, int payerServiceCodeId, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PayerAppointmentTypeId", payerAppointmentTypeId),
            new SqlParameter("@PayerServiceCodeId",payerServiceCodeId)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAY_GetPayerServiceCodeDetail.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetMasterServiceCodeExcludedFromPayerServiceCodes<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationID",tokenModel.OrganizationID)
                                         ,new SqlParameter("@PayerId",searchFilterModel.PayerId)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetMasterServiceCodeExcludedFromPayerServiceCodes, parameters.Length, parameters).AsQueryable();
        }

    }
}
