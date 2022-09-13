using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.PayrollBreaktime;
using HC.Patient.Repositories.IRepositories.Payroll;
using HC.Patient.Service.IServices.Payroll;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payroll
{
    public class PayrollBreaktimeService : BaseService, IPayrollBreaktimeService
    {
        private readonly IPayrollBreaktimeRepository _payrollBreaktimeRepository;
        private readonly IPayrollBreakTimeDetailsRepository _payrollBreakTimeDetailsRepository;        
        private HCOrganizationContext _context;
        public PayrollBreaktimeService(IPayrollBreaktimeRepository payrollBreaktimeRepository,IPayrollBreakTimeDetailsRepository payrollBreakTimeDetailsRepository, HCOrganizationContext  context)
        {
            _payrollBreaktimeRepository = payrollBreaktimeRepository;
            _payrollBreakTimeDetailsRepository = payrollBreakTimeDetailsRepository;
            _context = context;
        }

        public JsonModel DeletePayrollBreakTime(int Id, TokenModel token)
        {
            PayrollBreakTime payrollBreakTime = _payrollBreaktimeRepository.GetByID(Id);
            if (payrollBreakTime != null)
            {
                payrollBreakTime.IsDeleted = true;
                payrollBreakTime.DeletedBy = token.UserID;
                payrollBreakTime.DeletedDate = DateTime.UtcNow;
                _payrollBreaktimeRepository.Update(payrollBreakTime);
                _payrollBreaktimeRepository.SaveChanges();
                
            }
            return new JsonModel(new object(), StatusMessage.BreakTimeDelete, (int)HttpStatusCodes.OK, string.Empty);
        }

        public JsonModel GetPayrollBreakTimeById(int Id, TokenModel token)
        {
            PayrollBreaktimeModel payrollBreakTimeModel = new PayrollBreaktimeModel();
            List<PayrollBreaktimeDetailsModel> payrollBreakTimeDetails = new List<PayrollBreaktimeDetailsModel>();

            PayrollBreakTime payrollBreakTime = _payrollBreaktimeRepository.GetByID(Id);
            AutoMapper.Mapper.Map(payrollBreakTime, payrollBreakTimeModel);
            if(payrollBreakTimeModel!=null)
            {
                List<PayrollBreakTimeDetails> list = _payrollBreakTimeDetailsRepository.GetAll(x => x.PayrollBreakTimeId == Id && x.IsDeleted == false).OrderBy(x => x.StartRange).ToList();
                AutoMapper.Mapper.Map(list, payrollBreakTimeDetails);
                payrollBreakTimeModel.PayrollBreaktimeDetails = payrollBreakTimeDetails;
            }
            return new JsonModel(payrollBreakTimeModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK,string.Empty);
        }

        public JsonModel GetPayrollBreakTimeList(int pageNumber, int pageSize, string sortColumn, string sortOrder, TokenModel token)
        {
            var query = _payrollBreaktimeRepository.GetAll(x=>x.OrganizationId==token.OrganizationID && x.IsDeleted==false);
            decimal totalCount = query.ToList().Count;
            List<PayrollBreaktimeModel> payrollBreakTimeList = query.Select(x =>new PayrollBreaktimeModel() {
                Id=x.Id,
                Name=x.Name,
                Duration=x.Duration
            }).OrderByDescending(x => x.Id).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
            return new JsonModel()
            {
                data = payrollBreakTimeList,
                meta = new Meta()
                {
                    TotalRecords = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    DefaultPageSize = pageSize,
                    TotalPages = Math.Ceiling(totalCount / pageSize)
                },
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel SavePayrollBreakTime(PayrollBreaktimeModel payrollBreaktimeDetailsModel, TokenModel token)
        {
            List<PayrollBreaktimeDetailsModel> insertListModel = null;
            PayrollBreakTime payrollBreakTime = null;
            PayrollBreakTimeDetails payrollDetailedBreakTime = null;
            List<PayrollBreakTimeDetails> insertList = new List<PayrollBreakTimeDetails>();
            List<PayrollBreakTimeDetails> updateList = new List<PayrollBreakTimeDetails>();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (payrollBreaktimeDetailsModel.Id > 0)
                    {
                        payrollBreakTime = _payrollBreaktimeRepository.GetByID(payrollBreaktimeDetailsModel.Id);
                        if (payrollBreakTime != null)
                        {
                            payrollBreakTime.Name = payrollBreaktimeDetailsModel.Name;
                            payrollBreakTime.Duration = payrollBreaktimeDetailsModel.Duration;
                            payrollBreakTime.UpdatedBy = token.UserID;
                            payrollBreakTime.UpdatedDate = DateTime.UtcNow;
                            _payrollBreaktimeRepository.Update(payrollBreakTime);
                            _payrollBreaktimeRepository.SaveChanges();
                            if (payrollBreaktimeDetailsModel.PayrollBreaktimeDetails != null && payrollBreaktimeDetailsModel.PayrollBreaktimeDetails.Count > 0)
                            {
                                insertListModel = payrollBreaktimeDetailsModel.PayrollBreaktimeDetails.Where(x => x.Id == 0).ToList();
                                AutoMapper.Mapper.Map(payrollBreaktimeDetailsModel.PayrollBreaktimeDetails, insertList);
                                if (insertList != null && insertList.Count > 0)
                                {
                                    insertList.ForEach(x => { x.PayrollBreakTimeId = payrollBreakTime.Id; x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.IsActive = true; });
                                    _payrollBreakTimeDetailsRepository.Create(insertList.ToArray());
                                }
                                
                                if (payrollBreaktimeDetailsModel.PayrollBreaktimeDetails != null && payrollBreaktimeDetailsModel.PayrollBreaktimeDetails.Count > 0)
                                {
                                    payrollBreaktimeDetailsModel.PayrollBreaktimeDetails.ForEach(x =>
                                    {
                                        payrollDetailedBreakTime = _payrollBreakTimeDetailsRepository.GetByID(x.Id);
                                        if (payrollDetailedBreakTime != null)
                                        {
                                            payrollDetailedBreakTime.StartRange = x.StartRange;
                                            payrollDetailedBreakTime.EndRange = x.EndRange;
                                            payrollDetailedBreakTime.NumberOfBreaks = x.NumberOfBreaks;
                                            payrollDetailedBreakTime.UpdatedBy = token.UserID;
                                            payrollDetailedBreakTime.UpdatedDate = DateTime.UtcNow;
                                            payrollDetailedBreakTime.IsDeleted = x.IsDeleted;
                                            updateList.Add(payrollDetailedBreakTime);
                                        }
                                    });
                                    if (updateList != null && updateList.Count > 0)
                                        _payrollBreakTimeDetailsRepository.Update(updateList.ToArray());
                                    if((insertList!=null && insertList.Count>0) || (updateList!=null && updateList.Count>0))
                                    _payrollBreakTimeDetailsRepository.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        payrollBreakTime = new PayrollBreakTime();
                        AutoMapper.Mapper.Map(payrollBreaktimeDetailsModel, payrollBreakTime);
                        payrollBreakTime.OrganizationId = token.OrganizationID;
                        payrollBreakTime.CreatedBy = token.UserID;
                        payrollBreakTime.CreatedDate = DateTime.UtcNow;
                        payrollBreakTime.IsActive = true;
                        _payrollBreaktimeRepository.Create(payrollBreakTime);
                        _payrollBreaktimeRepository.SaveChanges();
                        if (payrollBreaktimeDetailsModel.PayrollBreaktimeDetails != null && payrollBreaktimeDetailsModel.PayrollBreaktimeDetails.Count > 0)
                        {
                            insertList = new List<PayrollBreakTimeDetails>();
                            AutoMapper.Mapper.Map(payrollBreaktimeDetailsModel.PayrollBreaktimeDetails, insertList);
                            insertList.ForEach(x => { x.PayrollBreakTimeId = payrollBreakTime.Id; x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.IsActive = true; });
                            _payrollBreakTimeDetailsRepository.Create(insertList.ToArray());
                            _payrollBreakTimeDetailsRepository.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    return new JsonModel(new object(), StatusMessage.BreakTimeAdd, (int)HttpStatusCodes.OK, string.Empty);
                }
                catch {
                    return new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, string.Empty);
                }
            }
        }
    }
}
