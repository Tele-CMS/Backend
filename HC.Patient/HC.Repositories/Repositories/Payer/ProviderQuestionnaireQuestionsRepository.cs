using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Payer;
using HC.Patient.Repositories.IRepositories.Payer;
using HC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Repositories.Repositories.Payer
{
    public class ProviderQuestionnaireQuestionsRepository : RepositoryBase<ProviderQuestionnaireQuestions>, IProviderQuestionnaireQuestionsRepository
    {
        private HCOrganizationContext _context;
        public ProviderQuestionnaireQuestionsRepository(HCOrganizationContext context) : base(context)
        {
            this._context = context;
        }

        //public IQueryable<T> GetQuestionnaireQuestionsList<T>(SearchFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        //{
        //    SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
        //        new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
        //         new SqlParameter("@userid",tokenModel.UserID),
        //        new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
        //        new SqlParameter("@PageSize",searchFilterModel.pageSize),
        //        new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
        //        new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
        //    return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetQuestionnaireQuestionsList, parameters.Length, parameters).AsQueryable();
        //}

        public IQueryable<T> GetQuestionnaireQuestionsList<T>(ProviderQuestionnaireFilterModel searchFilterModel, TokenModel tokenModel) where T : class, new()
        {
            SqlParameter[] parameters = { new SqlParameter("@SearchText",searchFilterModel.SearchText),
                new SqlParameter("@OrganizationId",tokenModel.OrganizationID),
                 new SqlParameter("@userid",tokenModel.UserID),
                 new SqlParameter("@QuestionnaireId",searchFilterModel.QuestionnaireId),
                new SqlParameter("@PageNumber",searchFilterModel.pageNumber),
                new SqlParameter("@PageSize",searchFilterModel.pageSize),
                new SqlParameter("@SortColumn",searchFilterModel.sortColumn),
                new SqlParameter("@SortOrder",searchFilterModel.sortOrder) };
            return _context.ExecStoredProcedureListWithOutput<T>(SQLObjects.GetQuestionnaireQuestionsList, parameters.Length, parameters).AsQueryable();
        }
    }
}
