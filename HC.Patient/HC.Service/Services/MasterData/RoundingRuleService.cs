using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Service.IServices.MasterData;
using System;
using System.Collections.Generic;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using System.Linq;
using HC.Patient.Data;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Service;

namespace HC.Patient.Service.Services.MasterData
{
    public class RoundingRuleService : BaseService, IRoundingRuleService
    {
        private HCOrganizationContext _context;
        private IRoundingRuleRepository _roundingRuleRepository;
        private IRoundingRuleDetailRepository _roundingRuleDetailRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        string Message = string.Empty;
        public RoundingRuleService(HCOrganizationContext context, IRoundingRuleRepository roundingRuleRepository, IRoundingRuleDetailRepository roundingRuleDetailRepository, IAuditLogRepository auditLogRepository)
        {
            _context = context;
            _roundingRuleRepository = roundingRuleRepository;
            _roundingRuleDetailRepository = roundingRuleDetailRepository;
            _auditLogRepository = auditLogRepository;
        }
        public JsonModel SaveRoundingRules(RoundingRuleModel roundingRules, TokenModel token)
        {
            MasterRoundingRules requestObj = null; ;
            RoundingRuleDetails requestObjDetail = null;
            List<RoundingRuleDetails> requestObjDetailList = new List<RoundingRuleDetails>();
            IEnumerable<RoundingRuleDetails> queryDetails = null;
            RoundingRuleDetails roundingRuleReqObj = null;
            List<RoundingRuleDetails> newObjList = null;

            //using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime CurrentDate = DateTime.UtcNow;
                    if (!ReferenceEquals(roundingRules, null) && roundingRules.Id == 0)
                    {
                        requestObj = new MasterRoundingRules();
                        requestObjDetail = new RoundingRuleDetails();
                        requestObjDetailList = new List<RoundingRuleDetails>();
                        AutoMapper.Mapper.Map(roundingRules, requestObj);
                        AutoMapper.Mapper.Map(roundingRules.RoundingRuleDetail, requestObjDetailList);
                        requestObj.CreatedBy = token.UserID;
                        requestObj.OrganizationID = token.OrganizationID;
                        requestObj.CreatedDate = CurrentDate;
                        requestObj.IsActive = true;
                        requestObj.IsDeleted = false;
                        _roundingRuleRepository.Create(requestObj);
                        //_roundingRuleRepository.SaveChanges();
                        _roundingRuleRepository.SaveChanges();
                        // _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.CreateRoundingRule, AuditLogAction.Create, null, token.UserID, "" + null, token);
                        if (requestObj.Id > 0)
                        {
                            requestObjDetailList.ForEach(x => { x.RuleID = requestObj.Id; x.CreatedBy = token.UserID; x.CreatedDate = CurrentDate; x.IsActive = true; x.IsDeleted = false; });
                            _roundingRuleDetailRepository.Create(requestObjDetailList.ToArray());
                            _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.CreateRoundingRuleDetails, AuditLogAction.Create, null, token.UserID, "" + null, token);
                            _roundingRuleDetailRepository.SaveChanges();
                        }
                        Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "Rounding rule");
                    }
                    else
                    {
                        requestObj = _roundingRuleRepository.Get(x => x.Id == roundingRules.Id && x.IsActive == true && x.IsDeleted == false);
                        if (requestObj != null)
                        {
                            newObjList = new List<RoundingRuleDetails>();
                            requestObj.RuleName = roundingRules.RuleName;
                            requestObj.UpdatedBy = token.UserID;
                            requestObj.UpdatedDate = CurrentDate;
                            _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateRoundingRule, AuditLogAction.Modify, null, token.UserID, "" + null, token);
                            _roundingRuleRepository.Update(requestObj);
                            _roundingRuleRepository.SaveChanges();
                            queryDetails = _roundingRuleDetailRepository.GetAll(x => x.RuleID == requestObj.Id && x.IsActive == true && x.IsDeleted == false).ToList();
                            foreach (RoundingRuleDetailsModel model in roundingRules.RoundingRuleDetail)
                            {
                                roundingRuleReqObj = queryDetails.Where(x => x.Id == model.Id).FirstOrDefault();

                                if (roundingRuleReqObj != null)
                                {
                                    roundingRuleReqObj.StartRange = model.StartRange;
                                    roundingRuleReqObj.EndRange = model.EndRange;
                                    roundingRuleReqObj.Unit = model.Unit;
                                    roundingRuleReqObj.UpdatedBy = token.UserID;
                                    roundingRuleReqObj.UpdatedDate = CurrentDate;
                                    roundingRuleReqObj.IsDeleted = model.IsDeleted;
                                    roundingRuleReqObj.DeletedBy = token.UserID;
                                }
                                else //insert new in update case 
                                {
                                    roundingRuleReqObj = new RoundingRuleDetails();
                                    roundingRuleReqObj.StartRange = model.StartRange;
                                    roundingRuleReqObj.EndRange = model.EndRange;
                                    roundingRuleReqObj.Unit = model.Unit;
                                    roundingRuleReqObj.CreatedBy = token.UserID;
                                    roundingRuleReqObj.CreatedDate = CurrentDate;
                                    roundingRuleReqObj.IsDeleted = false;
                                    roundingRuleReqObj.IsActive = true;
                                    roundingRuleReqObj.RuleID = roundingRules.Id;
                                    newObjList.Add(roundingRuleReqObj);
                                }
                            }

                            if (queryDetails.Count() > 0)//update
                            {
                                _roundingRuleDetailRepository.Update(queryDetails.ToArray());
                                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateRoundingRuleDetails, AuditLogAction.Modify, null, token.UserID, "" + null, token);
                            }
                            if (newObjList.Count() > 0)//new
                            {
                                _roundingRuleDetailRepository.Create(newObjList.ToArray());
                                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.CreateRoundingRuleDetails, AuditLogAction.Create, null, token.UserID, "" + null, token);
                            }
                            _roundingRuleDetailRepository.SaveChanges();


                            //Message
                            Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "Rounding rule");
                        }
                    }
                    //transaction commit
                    try
                    {
                        //transaction.Commit();
                    }
                    catch (Exception)
                    {
                    }
                    //transaction.Commit();
                    //
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = Message,
                        StatusCode = (int)HttpStatusCodes.OK//Success
                    };
                }
                catch (Exception e)
                {
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = StatusMessage.ServerError,
                        StatusCode = (int)HttpStatusCodes.InternalServerError//(Not Found)
                    };
                }
            }
        }

        public JsonModel GetRoundingRuleById(int Id)
        {
            RoundingRuleModel response = new RoundingRuleModel();
            response.RoundingRuleDetail = new List<RoundingRuleDetailsModel>();
            MasterRoundingRules masterRoundingRules = _roundingRuleRepository.Get(a => a.Id == Id && a.IsActive == true && a.IsDeleted == false);
            if (masterRoundingRules != null)
            {
                List<RoundingRuleDetails> roundingRuleDetails = _roundingRuleDetailRepository.GetAll(a => a.RuleID == Id && a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.StartRange).ToList();
                AutoMapper.Mapper.Map(masterRoundingRules, response);
                AutoMapper.Mapper.Map(roundingRuleDetails, response.RoundingRuleDetail);

                return new JsonModel()
                {
                    data = response,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK//Success
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//Success
                };
            }
        }

        public JsonModel GetRoundingRules(TokenModel token)
        {
            //response model
            List<RoundingRuleModel> response = new List<RoundingRuleModel>();
            //get all masterRoundingRule and ruleDetails
            List<MasterRoundingRules> masterRoundingRules = _roundingRuleRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false && x.OrganizationID == token.OrganizationID).OrderByDescending(h => h.Id).ToList();
            if (masterRoundingRules != null && masterRoundingRules.Count > 0)
            {
                List<RoundingRuleDetails> roundingRuleDetails = _roundingRuleDetailRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false).OrderBy(h => h.Id).ToList();
                foreach (var item in masterRoundingRules)
                {
                    //response model
                    RoundingRuleModel roundingRuleobj = new RoundingRuleModel();
                    roundingRuleobj.RoundingRuleDetail = new List<RoundingRuleDetailsModel>();

                    List<RoundingRuleDetailsModel> roundingRuleDetailsObj = new List<RoundingRuleDetailsModel>();
                    //initializing values
                    roundingRuleobj.Id = item.Id;
                    roundingRuleobj.RuleName = item.RuleName;

                    List<RoundingRuleDetails> roundingRuleObj = roundingRuleDetails.Where(a => a.RuleID == item.Id).ToList();

                    AutoMapper.Mapper.Map(roundingRuleObj, roundingRuleDetailsObj);
                    response.Add(roundingRuleobj);

                    foreach (var ob in roundingRuleDetailsObj)
                    {
                        roundingRuleobj.RoundingRuleDetail.Add(ob);
                    }
                }
                return new JsonModel()
                {
                    data = response.OrderByDescending(a => a.Id),
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK//Success
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound//Success
                };
            }
        }

        public JsonModel DeleteRoundingRule(int Id, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime CurrentDate = DateTime.UtcNow;
                    MasterServiceCode masterServiceCode = _context.MasterServiceCode.Where(j => j.RuleID == Id && j.IsDeleted == false).FirstOrDefault();

                    if (masterServiceCode == null)
                    {

                        MasterRoundingRules masterRoundingRules = _roundingRuleRepository.Get(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id);
                        List<RoundingRuleDetails> roundingRuleDetails = _roundingRuleDetailRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false && x.RuleID == Id).ToList();
                        //set IsDeleted = 1(true)
                        roundingRuleDetails.ForEach(x => { x.IsDeleted = true; x.DeletedBy = token.UserID; x.DeletedDate = CurrentDate; });
                        masterRoundingRules.IsDeleted = true;
                        masterRoundingRules.DeletedDate = CurrentDate;
                        masterRoundingRules.DeletedBy = token.UserID;

                        //update            
                        _roundingRuleDetailRepository.Update(roundingRuleDetails.ToArray());
                        _roundingRuleRepository.Update(masterRoundingRules);

                        _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteRoundingRule, AuditLogAction.Delete, null, token.UserID, "" + null, token);
                        //save
                        _roundingRuleDetailRepository.SaveChanges();
                        //_roundingRuleRepository.SaveChanges();
                        _roundingRuleRepository.SaveChanges();

                        transaction.Commit();
                        return new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.RoundingRuleDeleted,
                            StatusCode = (int)HttpStatusCodes.OK//Success
                        };
                    }
                    else
                    {
                        return new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.RoundingRuleNotDeleted,
                            StatusCode = (int)HttpStatusCodes.UnprocessedEntity
                        };
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = e.Message,
                        StatusCode = (int)HttpStatusCodes.InternalServerError//Error
                    };
                }
            }
        }


        public JsonModel GetRoundingRules(string RuleName, int OrganizationID, int PageNumber, int PageSize, string SortColumn, string SortOrder)
        {
            //using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = _roundingRuleRepository.GetRoundingRules(RuleName, OrganizationID, PageNumber, PageSize, SortColumn, SortOrder);
                    // transaction.Commit();
                    return new JsonModel()
                    {
                        data = result,

                        meta = new Meta()
                        {
                            TotalRecords = Convert.ToDecimal(result != null && result.Count > 0 ? result[0].TotalRecords : 0)
                        ,
                            CurrentPage = PageNumber,
                            PageSize = PageSize,
                            DefaultPageSize = PageSize,
                            TotalPages = Convert.ToDecimal(result != null && result.Count > 0 ? result[0].TotalPages : 0)
                        },
                        Message = StatusMessage.FetchMessage,
                        StatusCode = (int)HttpStatusCodes.OK
                    };

                }
                catch (Exception e)
                {
                    //transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = e.Message,
                        StatusCode = (int)HttpStatusCodes.InternalServerError//Error
                    };
                }
            }
        }
    }
}
