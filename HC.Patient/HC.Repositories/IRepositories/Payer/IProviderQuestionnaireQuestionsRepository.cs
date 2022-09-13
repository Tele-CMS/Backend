using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Payer;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.Payer
{
    public interface IProviderQuestionnaireQuestionsRepository : IRepositoryBase<ProviderQuestionnaireQuestions>
    {
        //IQueryable<T> GetQuestionnaireQuestionsList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
        IQueryable<T> GetQuestionnaireQuestionsList<T>(ProviderQuestionnaireFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new();
    }
}
