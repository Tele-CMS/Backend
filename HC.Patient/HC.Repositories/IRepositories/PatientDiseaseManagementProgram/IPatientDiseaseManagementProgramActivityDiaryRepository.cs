using HC.Model;
using HC.Patient.Entity;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram
{
    public interface IPatientDiseaseManagementProgramActivityDiaryRepository : IRepositoryBase<DiseaseManagementProgramPatientActivityDiary>
    {
        //public 
        IQueryable<T> GetDMPPatientActivityDiary<T>(int diseaseManagementPlanPatientActivityId, FilterModel filterModel, TokenModel token) where T : class, new();
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@DiseaseManagementPlanPatientActivityId", diseaseManagementPlanPatientActivityId),
        //                                   new SqlParameter("@PageNumber", filterModel.pageNumber),
        //                                  new SqlParameter("@PageSize", filterModel.pageSize),
        //                                  new SqlParameter("@SortOrder", filterModel.sortOrder),
        //                                  new SqlParameter("@SortColumn", filterModel.sortColumn),
        //    };
        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.DMP_GetPatientDiseaseManagementProgramList.ToString(), parameters.Length, parameters).AsQueryable();
        //}
    }
}
