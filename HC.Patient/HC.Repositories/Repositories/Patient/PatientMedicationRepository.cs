using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
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
    public class PatientMedicationRepository: RepositoryBase<PatientMedication>, IPatientMedicationRepository
    {
        private HCOrganizationContext _context;
        public PatientMedicationRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetMedication<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                        new SqlParameter("@SearchText", patientFilterModel.SearchText),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetMedication.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetMedicationForPrint<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                        new SqlParameter("@SearchText", patientFilterModel.SearchText),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetMedicationForPrint.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
