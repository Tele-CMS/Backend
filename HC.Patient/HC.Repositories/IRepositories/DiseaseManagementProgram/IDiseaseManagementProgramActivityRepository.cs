using HC.Model;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.DiseaseManagementProgram
{
    public interface IDiseaseManagementProgramActivityRepository 
    {
        IQueryable<T> GetDiseaseManagementProgramActivitiesList<T>(int diseaseManagementProgramId, FilterModel filterModel, TokenModel token) where T : class, new();

    }
}
