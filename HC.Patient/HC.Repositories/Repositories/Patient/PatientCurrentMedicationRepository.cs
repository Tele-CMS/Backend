using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Repositories;
using System.Data.SqlClient;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Patient
{
    public class PatientCurrentMedicationRepository: RepositoryBase<PatientCurrentMedication>, IPatientCurrentMedicationRepository
    {
        private HCOrganizationContext _context;
        public PatientCurrentMedicationRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> GetCurrentMedication<T>(PatientFilterModel patientFilterModel, bool isShowAlert, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                         new SqlParameter("@isShowAlert", isShowAlert),
                                        new SqlParameter("@SearchText", patientFilterModel.SearchText),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetCurrentMedicationList.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetCurrentMedicationById<T>(int Id, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@Id", Id),
                                        new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                        new SqlParameter("@UserId",tokenModel.UserID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetCurrentMedicationByID.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public PatientCurrentMedicationModel PrintPatientCurrentMedication<T>(int patientID, TokenModel tokenModel)
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientID),
                                        new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                        new SqlParameter("@UserId",tokenModel.UserID),
            };
            return _context.ExecStoredProcedureForPrintPatientCurrentMedication(SQLObjects.PAT_GetPrintPatientCurrentMedication.ToString(), parameters.Length, parameters);

        }

        public IQueryable<T> GetCurrentAndClaimMedicationList<T>(PatientFilterModel patientFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@PatientId", patientFilterModel.PatientId),
                                        new SqlParameter("@OrganizationId", tokenModel.OrganizationID),
                                        new SqlParameter("@SearchText", patientFilterModel.SearchText),
                                        new SqlParameter("@PageNumber",patientFilterModel.pageNumber),
                                        new SqlParameter("@PageSize", patientFilterModel.pageSize),
                                        new SqlParameter("@SortColumn", patientFilterModel.sortColumn),
                                        new SqlParameter("@SortOrder ", patientFilterModel.sortOrder)
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.PAT_GetCurrentAndClaimMedicationList.ToString(), parameters.Length, parameters).AsQueryable();
        }

        public IQueryable<T> GetCurrentMedicationStength<T>(string medicationName, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@MedicationName", medicationName),
                                        new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetCurrentMedicationStrengthList.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetCurrentMedicationUnit<T>(string medicationName, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@MedicationName", medicationName),
                                        new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetCurrentMedicationUnit.ToString(), parameters.Length, parameters).AsQueryable();
        }
        public IQueryable<T> GetCurrentMedicationForm<T>(string medicationName, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@MedicationName", medicationName),
                                        new SqlParameter("@OrganizationID", tokenModel.OrganizationID),
            };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.MTR_GetCurrentMedicationForm.ToString(), parameters.Length, parameters).AsQueryable();
        }
    }
}
