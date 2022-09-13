using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Service.Interfaces;

namespace HC.Patient.Service.IServices.MasterData
{
    public interface IRoundingRuleService : IBaseService
    {
        JsonModel SaveRoundingRules(RoundingRuleModel roundingRules,TokenModel token);
        JsonModel GetRoundingRuleById(int Id);
        JsonModel GetRoundingRules(TokenModel token);
        JsonModel DeleteRoundingRule(int Id, TokenModel token);
        JsonModel GetRoundingRules(string RuleName,int OrganizationID, int PageNumber, int PageSize, string SortColumn, string SortOrder);
    }
}
