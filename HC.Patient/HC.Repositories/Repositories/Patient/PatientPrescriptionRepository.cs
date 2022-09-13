using HC.Model;
using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientPrescriptionRepository : RepositoryBase<Entity.PatientPrescription>, IPatientPrescriptionRepository
    {
        private HCOrganizationContext _context;
        public PatientPrescriptionRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetprescriptionDrugList<T>() where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@OrganizationId", 0),
            };
            return _context.ExecStoredProcedureListWithOutput<T>("GetPrescriptionDrugList", parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetPatientPrescriptions<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetPrescription.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetMasterprescriptionDrugsList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
            new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
            new SqlParameter("@PageSize",searchFilterModel.pageSize),
            new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
            new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_MasterprescriptionDrugs, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetMasterPharmacyList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
            new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
            new SqlParameter("@PageSize",searchFilterModel.pageSize),
            new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
            new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_Masterpharmacy, parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetPatientSentPrescriptions<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)};
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetSentPrescription.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
