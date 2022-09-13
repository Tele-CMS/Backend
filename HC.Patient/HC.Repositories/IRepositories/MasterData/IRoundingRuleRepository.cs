using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Repositories.IRepositories.MasterData
{
    public interface IRoundingRuleRepository :IRepositoryBase<MasterRoundingRules>
    {
        List<RoundingRuleModelList> GetRoundingRules(string RuleName, int OrganizationID, int PageNumber, int PageSize, string SortColumn, string SortOrder);
    }
}
