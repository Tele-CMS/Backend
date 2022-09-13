using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Staff;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class StaffLeaveService : BaseService, IStaffLeaveService
    {
        private readonly HCOrganizationContext _context;
        private readonly IStaffLeaveRepository _staffLeaveRepository;
        private readonly IUserCommonRepository _userCommonRepository;
        private JsonModel response;
        public StaffLeaveService(IStaffLeaveRepository staffLeaveRepository, HCOrganizationContext context, IUserCommonRepository userCommonRepository)
        {
            _staffLeaveRepository = staffLeaveRepository;
            _userCommonRepository = userCommonRepository;
            _context = context;
        }

        public JsonModel GetStaffLeaveList(SearchFilterModel staffLeaveFilterModel, int staffId, TokenModel token)
         {
            List<StaffLeaveModel> staffLeaveModelList = _staffLeaveRepository.GetStaffLeaveList<StaffLeaveModel>(staffLeaveFilterModel, staffId, token).ToList();
            //if (staffLeaveModelList != null && staffLeaveModelList.Count > 0)
            //{
                response = new JsonModel(staffLeaveModelList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
                response.meta = new Meta()
                {
                    TotalRecords = staffLeaveModelList != null && staffLeaveModelList.Count > 0 ? staffLeaveModelList[0].TotalRecords : 0,
                    CurrentPage = staffLeaveFilterModel.pageNumber,
                    PageSize = staffLeaveFilterModel.pageSize,
                    DefaultPageSize = staffLeaveFilterModel.pageSize,
                    TotalPages = Math.Ceiling(Convert.ToDecimal((staffLeaveModelList != null && staffLeaveModelList.Count > 0 ? staffLeaveModelList[0].TotalRecords : 0) / staffLeaveFilterModel.pageSize))
                };
            //}
            return response;
        }
        public JsonModel AddUpdateStaffAppliedLeave(StaffLeaveModel staffLeaveModel, TokenModel token)
        {
            StaffLeave staffLeave = null;
            if (staffLeaveModel.Id == 0)
            {
                staffLeave = new StaffLeave();
                AutoMapper.Mapper.Map(staffLeaveModel, staffLeave);
                staffLeave.CreatedBy = token.UserID;
                staffLeave.CreatedDate = DateTime.UtcNow;
                staffLeave.IsDeleted = false;
                staffLeave.LeaveStatusId = _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeValue.ToLower() == "pending" && a.GlobalCodeCategory.GlobalCodeCategoryName.ToUpper() == "leavestatus".ToUpper() && a.OrganizationID == token.OrganizationID).OrderBy(a => a.DisplayOrder).FirstOrDefault().Id;
                _context.StaffLeave.Add(staffLeave);
                _context.SaveChanges();
                if (staffLeave.Id != 0)
                    response = new JsonModel(staffLeave, StatusMessage.StaffLeaveApplied, (int)HttpStatusCodes.OK, string.Empty);
            }
            else
            {
                if (staffLeaveModel != null)
                {
                    staffLeave = _staffLeaveRepository.GetAppliedLeaveByID(staffLeaveModel.Id, token);
                    if (staffLeave != null)
                    {
                        staffLeave.Description = staffLeaveModel.Description;
                        staffLeave.FromDate = staffLeaveModel.FromDate;
                        staffLeave.LeaveReasonId = staffLeaveModel.LeaveReasonId;
                        staffLeave.LeaveTypeId = staffLeaveModel.LeaveTypeId;
                        staffLeave.OtherLeaveReason = staffLeaveModel.OtherLeaveReason;
                        staffLeave.OtherLeaveType = staffLeaveModel.OtherLeaveType;
                        //staffLeave.StaffId = staffLeaveModel.StaffId;
                        staffLeave.ToDate = staffLeaveModel.ToDate;
                        staffLeave.UpdatedBy = token.UserID;
                        staffLeave.UpdatedDate = DateTime.UtcNow;
                        _staffLeaveRepository.Update(staffLeave);
                        _context.SaveChanges();
                        //response
                        response = new JsonModel(null, StatusMessage.StaffLeaveAppliedUpdated, (int)HttpStatusCodes.OK, string.Empty);
                    }
                    else
                        response = new JsonModel(null, StatusMessage.StaffLeaveAppliedDoesNotExist, (int)HttpStatusCodes.BadRequest, string.Empty);
                }
            }
            return response;
        }
        public JsonModel GetAppliedStaffLeaveById(int StaffLeaveId, TokenModel token)
        {
            StaffLeave staffLeave = _staffLeaveRepository.GetAppliedLeaveByID(StaffLeaveId, token);
            if (staffLeave != null)
            {
                StaffLeaveModel staffLeaveModel = new StaffLeaveModel();
                AutoMapper.Mapper.Map(staffLeave, staffLeaveModel);
                response = new JsonModel(staffLeaveModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
            }
            else
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCodes.NotFound, string.Empty);
            return response;
        }
        public JsonModel DeleteAppliedLeave(int staffLeaveId, TokenModel token)
        {
            StaffLeave staffLeave = _staffLeaveRepository.Get(x => x.Id == staffLeaveId && x.IsActive == true && x.IsDeleted == false);
            if (!ReferenceEquals(staffLeave, null))
            {
                staffLeave.IsDeleted = true;
                staffLeave.DeletedBy = token.UserID;
                staffLeave.DeletedDate = DateTime.UtcNow;
                _staffLeaveRepository.Update(staffLeave);
                _context.SaveChanges();
                response = new JsonModel(null, StatusMessage.StaffAppliedLeaveDelete, (int)HttpStatusCodes.OK, string.Empty);

            }
            else
                response = new JsonModel(null, StatusMessage.StaffLeaveAppliedDoesNotExist, (int)HttpStatusCodes.BadRequest, string.Empty);
            return response;
        }

        public JsonModel UpdateLeaveStatus(List<LeaveStatusModel> leaveStatusModel, TokenModel token)
        {
            if (leaveStatusModel != null)
            {
                List<int> leaveIds = leaveStatusModel.Select(a => a.staffLeaveId).ToList();
                List<StaffLeave> staffLeaveList = _staffLeaveRepository.GetAll(x => leaveIds.Contains(x.Id) && x.IsActive == true && x.IsDeleted == false).ToList();
                foreach (LeaveStatusModel leaveStatus in leaveStatusModel)
                {
                    StaffLeave staffLeave = staffLeaveList.Where(a => a.Id == leaveStatus.staffLeaveId).FirstOrDefault();
                    staffLeave.LeaveStatusId = leaveStatus.leaveStatusId;
                    staffLeave.DeclineReason = leaveStatus.declineReason;
                    staffLeave.ApprovalDate = DateTime.UtcNow;
                    
                }
                _staffLeaveRepository.Update(staffLeaveList.ToArray());
                _context.SaveChanges();
                response = new JsonModel(null, StatusMessage.LeaveStatusUpdated, (int)HttpStatusCodes.OK, string.Empty);
            }
            else
                response = new JsonModel(null, StatusMessage.ErrorOccured, (int)HttpStatusCodes.UnprocessedEntity, string.Empty);
            return response;
        }
    }
}


