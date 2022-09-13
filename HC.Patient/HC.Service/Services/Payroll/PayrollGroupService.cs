using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Payroll;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Patient.Service.IServices.Payroll;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payroll
{
    public class PayrollGroupService :BaseService, IPayrollGroupService
    {
        private readonly IPayrollGroupRepository _payrollGroupRepository;
        private JsonModel response;
        public PayrollGroupService(IPayrollGroupRepository payrollGroupRepository)
        {
            _payrollGroupRepository = payrollGroupRepository;
        }

        public JsonModel DeletePayrollGroup(int Id, TokenModel token)
        {
            PayrollGroup payrollEntity = _payrollGroupRepository.GetByID(Id);
            if (payrollEntity != null)
            {
                payrollEntity.IsDeleted = true;
                payrollEntity.DeletedBy = token.UserID;
                payrollEntity.DeletedDate = DateTime.UtcNow;
                _payrollGroupRepository.Update(payrollEntity);
                _payrollGroupRepository.SaveChanges();
                response = new JsonModel(null, StatusMessage.PayrollGroupDeleted, (int)HttpStatusCodes.OK, string.Empty);
            }
            else
                response = new JsonModel(null, StatusMessage.PayrollGroupDoesNotExist, (int)HttpStatusCodes.BadRequest, string.Empty);
            return response;
        }
        public JsonModel GetPayrollGroupById(int Id, TokenModel token)
        {
            PayrollGroup payrollEntity = _payrollGroupRepository.GetByID(Id);
            PayrollGroupModel payrollGroup = new PayrollGroupModel();
            AutoMapper.Mapper.Map(payrollEntity, payrollGroup);
            response = new JsonModel(payrollGroup, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            return response;
        }

        public JsonModel GetPayrollGroupList(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            List<PayrollGroupModel> payrollGroupList = _payrollGroupRepository.GetPayrollGroupList<PayrollGroupModel>(pageNumber, pageSize, sortColumn, sortOrder, token).ToList();
            response = new JsonModel(payrollGroupList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK,string.Empty);
            response.meta = new Meta()
            {
                TotalRecords = payrollGroupList != null && payrollGroupList.Count > 0 ? payrollGroupList.Count : 0,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                DefaultPageSize = pageSize,
                TotalPages = Math.Ceiling(Convert.ToDecimal((payrollGroupList != null && payrollGroupList.Count > 0 ? payrollGroupList.Count : 0) / pageSize))
            };
            return response;
        }
        public JsonModel GetPayrollGroupListForDropdown(TokenModel token)
        {
            List<PayrollGroupDropdownModel> payrollGroupList = _payrollGroupRepository.GetPayrollGroupListForDropdown<PayrollGroupDropdownModel>(token).ToList();
            response = new JsonModel(payrollGroupList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            return response;
        }

        public JsonModel SavePayrollGroup(PayrollGroupModel payrollGroup, TokenModel token)
        {
            PayrollGroup payrollEntity = null;
            if (payrollGroup.Id == 0)
            {
                payrollEntity = new PayrollGroup();
                AutoMapper.Mapper.Map(payrollGroup, payrollEntity);
                payrollEntity.CreatedBy = token.UserID;
                payrollEntity.CreatedDate = DateTime.UtcNow;
                payrollEntity.IsActive = true;
                payrollEntity.IsDeleted = false;
                payrollEntity.OrganizationId = token.OrganizationID;
                _payrollGroupRepository.Create(payrollEntity);
                response = new JsonModel(null, StatusMessage.PayrollGroupAdded, (int)HttpStatusCodes.OK, string.Empty);
            }
            else {
                payrollEntity = _payrollGroupRepository.GetByID(payrollGroup.Id);
                if (payrollEntity != null)
                {
                    payrollEntity.GroupName = payrollGroup.GroupName;
                    payrollEntity.DailyLimit = payrollGroup.DailyLimit;
                    payrollEntity.WeeklyLimit = payrollGroup.WeeklyLimit;
                    payrollEntity.OverTime = payrollGroup.OverTime;
                    payrollEntity.DoubleOverTime = payrollGroup.DoubleOverTime;
                    payrollEntity.PayPeriodId = payrollGroup.PayPeriodId;
                    payrollEntity.WorkWeekId = payrollGroup.WorkWeekId;
                    payrollEntity.PayrollBreakTimeId = payrollGroup.PayrollBreakTimeId;
                    payrollEntity.UpdatedBy =token.UserID;
                    payrollEntity.UpdatedDate = DateTime.UtcNow;
                    _payrollGroupRepository.Update(payrollEntity);
                    response = new JsonModel(null, StatusMessage.PayrollGroupUpdated, (int)HttpStatusCodes.OK, string.Empty);
                }
                    response = new JsonModel(null, StatusMessage.PayrollGroupDoesNotExist, (int)HttpStatusCodes.BadRequest, string.Empty);
            }
            _payrollGroupRepository.SaveChanges();
            return response;
        }
    }
}
