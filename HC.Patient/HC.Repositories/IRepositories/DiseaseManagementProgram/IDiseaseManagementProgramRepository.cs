using HC.Model;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.DiseaseManagementProgram
{
    public interface IDiseaseManagementProgramRepository : IRepositoryBase<Entity.DiseaseManagementProgram>
    {
        IQueryable<T> GetDiseaseManagementProgramList<T>( FilterModel filterModel, TokenModel token) where T : class, new();
        IQueryable<T> GetDiseaseProgramsListWithEnrollments<T>(TokenModel token) where T : class, new();
        IQueryable<T> GetDiseaseConditionsFromProgramIds<T>(string ProgramIds, TokenModel token) where T : class, new();

    }
}
