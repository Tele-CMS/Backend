using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Entity.Payments;
using HC.Patient.Model.Claim;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Payment;
using HC.Patient.Repositories.Interfaces;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.Claim;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.Payment;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Service.IServices.Payment;
using HC.Service;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Payment
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IClaimRepository _claimRepository;
        private readonly IStaffRepository _staffRepository;
        private JsonModel response;
        private readonly IAuditLogRepository _auditLogRepository;
        public readonly IUserCommonRepository _userCommonRepository;
        public readonly IClaimServiceLineRepository _claimServiceLineRepository;
        public object ClaimServiceLinePayment { get; private set; }

        public PaymentService(IPatientRepository patientRepository,IPaymentRepository paymentRepository, IAuditLogRepository auditLogRepository, IUserCommonRepository userCommonRepository, IClaimServiceLineRepository claimServiceLineRepository, IClaimRepository claimRepository,
            IStaffRepository staffRepository)
        {
            _patientRepository = patientRepository;
            _paymentRepository = paymentRepository;
            _auditLogRepository = auditLogRepository;
            _userCommonRepository = userCommonRepository;
            _claimServiceLineRepository = claimServiceLineRepository;
            _claimRepository = claimRepository;
            _staffRepository = staffRepository;
        }


        public JsonModel GetAllClaimsWithServiceLinesForPayer(string payerIds, string patientIds, string tags, string fromDate, string toDate, int locationId, string claimBalanceStatus, TokenModel token)
        {
            List<ClaimModel> claimList = new List<ClaimModel>();
            ClaimsFullDetailModel allClaims = _paymentRepository.GetAllClaimsWithServiceLinesForPayer(payerIds, patientIds, tags, fromDate, toDate, locationId, claimBalanceStatus, token);
            if (allClaims != null && allClaims.Claims.Count > 0)
            {
                foreach (ClaimModel claim in allClaims.Claims)
                {
                    claim.ClaimServiceLines = allClaims.ClaimServiceLines.FindAll(x => x.ClaimId == claim.ClaimId);
                    claimList.Add(claim);
                }
            }
            return new JsonModel()
            {
                data = allClaims,
                meta = null,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        public JsonModel ApplyPayment(PaymentApplyModel paymentCheckDetailModel, TokenModel token)
        {
            paymentCheckDetailModel = GetDetailForApplyPaymentsXml(paymentCheckDetailModel);
            //paymentCheckDetailModel.ClaimServiceLineAdjustment = GetDetailForApplyPaymentsXml(paymentCheckDetailModel);
            //paymentCheckDetailModel.ClaimServiceLineXml = GetDetailForApplyPaymentsXml(paymentCheckDetailModel);
            SQLResponseModel response = _paymentRepository.ApplyPayment<SQLResponseModel>(paymentCheckDetailModel, token).FirstOrDefault();
            return new JsonModel()
            {
                data = new object(),
                Message = response.Message,
                StatusCode = response.StatusCode
            };
        }
        public JsonModel EOBPayement(PaymentApplyModel paymentCheckDetailModel, TokenModel token)
        {
            paymentCheckDetailModel = GetPaymentDetailForEOBXml(paymentCheckDetailModel);
            //paymentCheckDetailModel.ClaimServiceLineXml = GetPaymentDetailForEOBXml(paymentCheckDetailModel);
            //paymentCheckDetailModel.ClaimsXml = GetPaymentDetailForEOBXml(paymentCheckDetailModel);
            SQLResponseModel response = _paymentRepository.EOBPayment<SQLResponseModel>(paymentCheckDetailModel, token).FirstOrDefault();
            return new JsonModel()
            {
                data = new object(),
                Message = response.Message,
                StatusCode = response.StatusCode
            };
        }
        private PaymentApplyModel GetPaymentDetailForEOBXml(PaymentApplyModel paymentCheckDetailModel)
        {
            //XElement payementCheckDetailElements = new XElement("Parent");
            if (paymentCheckDetailModel != null)
            {
                foreach (var claim in paymentCheckDetailModel.Claims)
                {
                    paymentCheckDetailModel.ClaimsXml = new XElement("Parent");
                    paymentCheckDetailModel.ClaimsXml.Add(new XElement("Child"
                    , new XElement("ClaimId", claim.ClaimId)
                    , new XElement("MarkSettled", claim.MarkSettled)
                    , new XElement("ClaimSettledDate", claim.MarkSettled ? DateTime.UtcNow.ToString() : null)
                ));
                    foreach (var claimServiceLine in claim.ClaimServiceLines)
                    {
                        paymentCheckDetailModel.ClaimServiceLineXml = new XElement("Parent");
                        paymentCheckDetailModel.ClaimServiceLineXml.Add(new XElement("Child"
                      , new XElement("ServiceLineId", claimServiceLine.Id)
                      , new XElement("Amount", claimServiceLine.PaymentAmount)
                      , new XElement("PatientInsuranceId", claim.PatientInsuranceId)
                  ));
                        if (claimServiceLine.ClaimServiceLineAdjustment != null)
                        {
                            foreach (var claimAdj in claimServiceLine.ClaimServiceLineAdjustment)
                            {
                                paymentCheckDetailModel.ClaimServiceLineAdjustment = new XElement("Parent");
                                paymentCheckDetailModel.ClaimServiceLineAdjustment.Add(new XElement("Child"
                                    , new XElement("ServiceLineId", claimAdj.ServiceLineId)
                                    , new XElement("AmountAdjusted", claimAdj.AmountAdjusted)
                                    , new XElement("AdjustmentGroupCode", claimAdj.AdjustmentGroupCode)
                                    , new XElement("AdjustmentReasonCode", claimAdj.AdjustmentReasonCode)
                                    , new XElement("PatientInsuranceId", claimAdj.PatientInsuranceId)
                                ));
                            }
                        }
                    }
                }
            }
            return paymentCheckDetailModel;
        }
        private PaymentApplyModel GetDetailForApplyPaymentsXml(PaymentApplyModel paymentCheckDetailModel)
        {
            //XElement payementCheckDetailElements = new XElement("Parent");
            if (paymentCheckDetailModel != null)
            {
                paymentCheckDetailModel.ClaimsXml = new XElement("Parent");
                paymentCheckDetailModel.ClaimServiceLineXml = new XElement("Parent");
                paymentCheckDetailModel.ClaimServiceLineAdjustment = new XElement("Parent");
                foreach (var claim in paymentCheckDetailModel.Claims)
                {
                    paymentCheckDetailModel.ClaimsXml.Add(new XElement("Child"
                    , new XElement("ClaimId", claim.ClaimId)
                    , new XElement("MarkSettled", claim.MarkSettled)
                    , new XElement("ClaimSettledDate", claim.MarkSettled ? DateTime.UtcNow.ToString() : null)
                    , new XElement("ClaimPaymentStatusId", claim.ClaimPaymentStatusId)
                ));
                    
                    foreach (var claimServiceLine in claim.ClaimServiceLines)
                    {
                        paymentCheckDetailModel.ClaimServiceLineXml.Add(new XElement("Child"
                      , new XElement("ServiceLineId", claimServiceLine.Id)
                      , new XElement("Amount", claimServiceLine.PaymentAmount)
                      , new XElement("PatientInsuranceId", claim.PatientInsuranceId)
                  ));
                        
                        if (claimServiceLine.ClaimServiceLineAdjustment != null)
                        {
                            
                            foreach (var claimAdj in claimServiceLine.ClaimServiceLineAdjustment)
                            {
                                paymentCheckDetailModel.ClaimServiceLineAdjustment.Add(new XElement("Child"
                                    , new XElement("ServiceLineId", claimAdj.ServiceLineId)
                                    , new XElement("AmountAdjusted", claimAdj.AmountAdjusted)
                                    , new XElement("AdjustmentGroupCode", claimAdj.AdjustmentGroupCode)
                                    , new XElement("AdjustmentReasonCode", claimAdj.AdjustmentReasonCode)
                                    , new XElement("PatientInsuranceId", claimAdj.PatientInsuranceId)
                                ));
                            }
                        }
                    }
                }
            }
            return paymentCheckDetailModel;
        }

        //private XElement GetClaimServiceLineXml(PaymentApplyModel paymentCheckDetailModel)
        //{
        //    XElement payementCheckDetailElements = new XElement("Parent");
        //    if (paymentCheckDetailModel != null)
        //    {
        //        foreach (var claim in paymentCheckDetailModel.Claims)
        //        {
        //            foreach (var claimServiceLine in claim.ClaimServiceLines)
        //            {
        //                payementCheckDetailElements.Add(new XElement("Child"
        //                    , new XElement("ServiceLineId", claimServiceLine.Id)
        //                    , new XElement("Amount", claimServiceLine.PaymentAmount)
        //                    , new XElement("PatientInsuranceId", claim.PatientInsuranceId)
        //                ));
        //            }
        //        }
        //    }
        //    return payementCheckDetailElements;
        //}
        //private XElement GetClaimsXmlForEOBPayments(PaymentApplyModel paymentCheckApplyModel)
        //{
        //    XElement payementCheckDetailElements = new XElement("Parent");
        //    foreach (var claim in paymentCheckApplyModel.Claims)
        //    {
        //        payementCheckDetailElements.Add(new XElement("Child"
        //            , new XElement("ClaimId", claim.ClaimId)
        //            , new XElement("MarkSettled", claim.MarkSettled)
        //            , new XElement("ClaimSettledDate", claim.MarkSettled ? DateTime.UtcNow.ToString() : null)
        //        ));
        //    }
        //    return payementCheckDetailElements;
        //}
        //private XElement GetClaimsXml(PaymentApplyModel paymentCheckApplyModel)
        //{
        //    XElement payementCheckDetailElements = new XElement("Parent");
        //    foreach (var claim in paymentCheckApplyModel.Claims)
        //    {
        //        payementCheckDetailElements.Add(new XElement("Child"
        //            , new XElement("ClaimId", claim.ClaimId)
        //            , new XElement("MarkSettled", claim.MarkSettled)
        //            , new XElement("ClaimSettledDate", claim.MarkSettled ? DateTime.UtcNow.ToString() : null)
        //            , new XElement("ClaimPaymentStatusId", claim.ClaimPaymentStatusId)
        //        ));
        //    }
        //    return payementCheckDetailElements;
        //}

        public JsonModel SaveServiceLinePayment(PaymentModel payment, TokenModel token)
        {
            ClaimServiceLinePaymentDetails paymentDetails = null;
            if (payment != null)
            {
                if (payment.Id > 0)
                {
                    paymentDetails = _paymentRepository.GetByID(payment.Id);
                    if (paymentDetails != null)
                    {
                        paymentDetails.Notes = !string.IsNullOrEmpty(payment.Notes) ? payment.Notes : null;
                        paymentDetails.PatientInsuranceId = payment.PatientInsuranceId;
                        paymentDetails.PatientId = payment.PatientId;
                        paymentDetails.GuarantorId = payment.GuarantorId;
                        paymentDetails.AdjustmentGroupCode = !string.IsNullOrEmpty(payment.AdjustmentGroupCode) ? payment.AdjustmentGroupCode : null;
                        paymentDetails.AdjustmentReasonCode = !string.IsNullOrEmpty(payment.AdjustmentReasonCode) ? payment.AdjustmentReasonCode : null;
                        paymentDetails.Amount = payment.Amount;
                        paymentDetails.DescriptionType = payment.DescriptionTypeId;
                        paymentDetails.PaymentTypeId = payment.PaymentTypeId;
                        paymentDetails.CustomReferenceNumber = payment.ChequeNo;
                        paymentDetails.UpdatedBy = token.UserID;
                        paymentDetails.UpdatedDate = DateTime.UtcNow;
                        _paymentRepository.Update(paymentDetails);
                        response = new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.PaymentUpdated,
                            StatusCode = (int)HttpStatusCodes.OK,
                        };
                    }
                    else
                    {
                        response = new JsonModel()
                        {
                            data = new object(),
                            Message = StatusMessage.PaymentDetailNotExists,
                            StatusCode = (int)HttpStatusCodes.NotFound,
                        };
                    }
                }
                else
                {
                    paymentDetails = new ClaimServiceLinePaymentDetails()
                    {
                        PaymentTypeId = payment.PaymentTypeId,
                        DescriptionType = payment.DescriptionTypeId,
                        Amount = payment.Amount,
                        Notes = !string.IsNullOrEmpty(payment.Notes) ? payment.Notes : null,
                        PatientInsuranceId = payment.PatientInsuranceId,
                        PatientId = payment.PatientId,
                        GuarantorId = payment.GuarantorId,
                        PaymentDate = payment.PaymentDate,
                        AdjustmentGroupCode = !string.IsNullOrEmpty(payment.AdjustmentGroupCode) ? payment.AdjustmentGroupCode : null,
                        AdjustmentReasonCode = !string.IsNullOrEmpty(payment.AdjustmentReasonCode) ? payment.AdjustmentReasonCode : null,
                        CustomReferenceNumber = payment.ChequeNo,
                        CreatedBy = token.UserID,
                        CreatedDate = DateTime.UtcNow,
                        ServiceLineId = payment.ServiceLineId
                    };
                    _paymentRepository.Create(paymentDetails);
                    response = new JsonModel()
                    {
                        data = null,
                        Message = StatusMessage.PaymentAdded,
                        StatusCode = (int)HttpStatusCodes.OK,
                    };
                }
                _paymentRepository.SaveChanges();

                ClaimBalanceModel resClaimBal = _claimRepository.GetClaimBalance<ClaimBalanceModel>(payment.ClaimId, token).FirstOrDefault();

                if (resClaimBal != null && resClaimBal.BalanceAmount == 0)
                {
                    Claims clm = _claimRepository.GetByID(payment.ClaimId);
                    if (clm != null)
                    {
                        clm.ClaimStatusId = Convert.ToInt32(MasterStatusClaim.Settled);
                        clm.ClaimSettledDate = DateTime.UtcNow;
                        _claimRepository.Update(clm);
                        _claimRepository.SaveChanges();
                    }
                }
            }
            return response;

        }
        public JsonModel DeleteServiceLinePayment(int paymentDetailId, TokenModel token)
        {
            ClaimServiceLinePaymentDetails claimServiceLinePaymentDetail = _paymentRepository.Get(a => a.IsActive == true && a.IsDeleted == false && a.Id == paymentDetailId);
            if (claimServiceLinePaymentDetail != null)
            {
                ClaimServiceLine claimServiceLine = _claimServiceLineRepository.Get(x => x.Id == claimServiceLinePaymentDetail.ServiceLineId);
                claimServiceLinePaymentDetail.IsDeleted = true;
                claimServiceLinePaymentDetail.DeletedBy = token.UserID;
                claimServiceLinePaymentDetail.DeletedDate = DateTime.UtcNow;
                _paymentRepository.Update(claimServiceLinePaymentDetail);

                //audit logs
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteServiceLinePayment + "(CL" + claimServiceLine.ClaimId + "/" + claimServiceLine.ServiceCode + ")", AuditLogAction.Delete, null, token.UserID, "", token);

                //return
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.ServiceLinePaymentDelete,
                    StatusCode = (int)HttpStatusCodes.OK,
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = new object(),
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound,
                };
            }
        }
        public JsonModel GetPaymentDetailsById(int paymentDetailId, TokenModel token)
        {
            PaymentModel payment = _paymentRepository.GetAll(x => x.Id == paymentDetailId && x.IsDeleted == false).Select(y => new PaymentModel()
            {
                Id = y.Id,
                PaymentTypeId = y.PaymentTypeId,
                DescriptionTypeId = y.DescriptionType,
                Amount = y.Amount,
                Notes = y.Notes,
                PatientInsuranceId = y.PatientInsuranceId,
                PaymentDate = y.PaymentDate,
                AdjustmentGroupCode = y.AdjustmentGroupCode,
                AdjustmentReasonCode = y.AdjustmentReasonCode,
                ServiceLineId = y.ServiceLineId,
                PatientId = y.PatientId,
                GuarantorId = y.GuarantorId
            }).FirstOrDefault();
            return new JsonModel()
            {
                data = payment,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel CreateCard(string cardmodel, TokenModel token)
        {
            string card = "";
            var localCustomer = _patientRepository.GetPatientDetailsForStripe<PatientStripeModel>(token.StaffID);
            StripeConfiguration.ApiKey = localCustomer.API_SK;
            if (string.IsNullOrEmpty(localCustomer.CustomerId))
            {
                var customer = new CustomerCreateOptions
                {
                    Description = localCustomer.FirstName + " created for smartTeleHealth Product",
                    Name = localCustomer.FirstName + " " + localCustomer.LastName,
                    Address = new AddressOptions { Line1 = localCustomer.Address1 },
                    Email = localCustomer.Email
                };
                var customerService = new CustomerService();
                var newCustomer = customerService.Create(customer);
                localCustomer.CustomerId = newCustomer.Id;
                _patientRepository.UpdateCustomerId(token.StaffID, localCustomer.CustomerId);
            }

            var options = new CardCreateOptions
            {
                Source = cardmodel,
            };
            var service = new CardService();
            service.Create(localCustomer.CustomerId, options);

            return new JsonModel()
            {
                data = card,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel SetDefaultCard(string cardId, TokenModel token)
        {
            try
            {
                var localCustomer = _patientRepository.GetPatientDetailsForStripe<PatientStripeModel>(token.StaffID);
                StripeConfiguration.ApiKey = localCustomer.API_SK;
                var options = new CustomerUpdateOptions
                {
                    DefaultSource = cardId
                };
                var service = new CustomerService();
                service.Update(localCustomer.CustomerId, options);
                return new JsonModel()
                {
                    data = StatusMessage.CardChanged,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = "",
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.NoContent

                };
            }
        }

        public JsonModel DeleteCard(string cardId, TokenModel token)
        {
            try
            {
                var localCustomer = _patientRepository.GetPatientDetailsForStripe<PatientStripeModel>(token.StaffID);
                StripeConfiguration.ApiKey = localCustomer.API_SK;
                var service = new CardService();
                service.Delete(
                  localCustomer.CustomerId,
                  cardId
                );
                return new JsonModel()
                {
                    data = StatusMessage.CardDeleted,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = "",
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.NoContent
                };
            }
        }

        public JsonModel CheckDefaultCard(TokenModel token)
        {
            var localCustomer = _patientRepository.GetPatientDetailsForStripe<PatientStripeModel>(token.StaffID);
            if (string.IsNullOrEmpty(localCustomer.CustomerId))
            {
                return new JsonModel()
                {
                    data = false
                };
            }
            StripeConfiguration.ApiKey = localCustomer.API_SK;
            var custService = new CustomerService();
            var custDetails = custService.Get(localCustomer.CustomerId);
            if (custDetails.DefaultSourceId == null)
            {
                return new JsonModel()
                {
                    data = false
                };
            }
            else
            {
                return new JsonModel()
                {
                    data = true
                };
            }
        }

        public JsonModel ListAllCards(TokenModel token)
        {

            try
            {
                var localCustomer = _patientRepository.GetPatientDetailsForStripe<PatientStripeModel>(token.StaffID);
                StripeConfiguration.ApiKey = localCustomer.API_SK;
                var service = new CardService();
                var custService = new CustomerService();
                var custDetails = custService.Get(localCustomer.CustomerId);

                var options = new CardListOptions
                {
                    Limit = 10,
                };
                var cards = service.List(localCustomer.CustomerId, options);
                if (cards.FirstOrDefault(z => z.Id == custDetails.DefaultSourceId) != null)
                {
                    cards.FirstOrDefault(z => z.Id == custDetails.DefaultSourceId).Description = "Default";
                }
                return new JsonModel()
                {
                    data = cards,
                    Message = StatusMessage.FetchMessage,
                    meta = new Meta()
                    {
                        TotalRecords = cards.Count(),
                        PageSize = 10,
                        CurrentPage = 1,
                        TotalPages = 1,
                        DefaultPageSize = 1
                    },
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            catch (Exception e)
            {
                return new JsonModel()
                {
                    data = "",
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.NoContent
                };
            }
        }
        public ExpiryCardNotification CardDetailsForNotification(TokenModel token)
        {
            ExpiryCardNotification expiryCard = new ExpiryCardNotification();
            var localCustomer = _patientRepository.GetPatientDetailsForStripe<PatientStripeModel>(token.StaffID);
            if (localCustomer != null && localCustomer.CustomerId != null)
            {
                StripeConfiguration.ApiKey = localCustomer.API_SK;
                var service = new CardService();
                var custService = new CustomerService();
                var custDetails = custService.Get(localCustomer.CustomerId);

                var cardId = custDetails.DefaultSourceId;
                if (cardId != null)
                {
                    var card = service.Get(localCustomer.CustomerId, cardId);

                    expiryCard.LastFourNumber = card.Last4;
                    expiryCard.ExpMonth = Convert.ToString(card.ExpMonth);
                    expiryCard.ExpYear = Convert.ToString(card.ExpYear);
                }

                return expiryCard;
            }
            else
            {
                return expiryCard;
            }
        }

        public JsonModel GetProvidersFeesAndRefundsSettings(List<int> providerIds, TokenModel token)
        {

            List<ManageFeesRefundsModel> list = _staffRepository.GetStaffsFeesAndRefundRules(providerIds).ToList();

            return new JsonModel()
            {
                data = list,
                meta = null,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }


        public JsonModel UpdateProvidersFeesAndRefundsSettings(ManageFeesRefundsModel model, TokenModel token)
        {
            var staffs = _staffRepository.GetAll(x => model.Providers.Contains(x.Id));

            var staffsList = staffs.ToList();
            staffsList = staffsList.Select(x =>
            {
                x.FTFpayRate = model.F2fFee;
                x.FollowUpDays = model.FolowupDays;
                x.FollowUpPayRate = model.FolowupFees;
                x.PayRate = model.NewOnlineFee;
                if (model.UrgentcareFee!=null)
                {
                    x.UrgentCarePayRate = model.UrgentcareFee;
                }
               
                return x;
            }).ToList();
            foreach (var s in staffsList)
            {
                _staffRepository.Update(s);

            }
            _staffRepository.RemoveStaffsCancellationRules(model.Providers);

            if (model.CancelationRules != null && model.CancelationRules.Count > 0 && model.Providers != null && model.Providers.Count>0)
            {
                List<ProviderCancellationRules> rulesDbAll = new List<ProviderCancellationRules>();
                foreach (var p in model.Providers)
                {
                    var rulesDBProvider = model.CancelationRules.Select(s => new ProviderCancellationRules
                    {
                        RefundPercentage = s.RefundPercentage,
                        StaffId = p,
                        UptoHour = s.UptoHours
                    }).ToList();

                    rulesDbAll.AddRange(rulesDBProvider);
                }

                _staffRepository.AddStaffsCancellationRules(rulesDbAll);
            }
            _staffRepository.SaveChanges();
            return new JsonModel()
            {
                data = "",
                meta = null,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }


       

        public JsonModel SaveUpdateProviderFeesforMobile(ProviderFeesModel providerfeemodel, TokenModel token)
        {
            try 
            {
                Staffs staffdetails = null;
                if (providerfeemodel != null)
                {
                    if (providerfeemodel.StaffId > 0)
                    {
                        staffdetails = _staffRepository.GetByID(providerfeemodel.StaffId);
                        if (staffdetails != null)
                        {
                            staffdetails.PayRate = providerfeemodel.NewOnlineFee;
                            staffdetails.FTFpayRate = providerfeemodel.F2fFee;
                            if (providerfeemodel.UrgentcareFee != null)
                            {
                                staffdetails.UrgentCarePayRate = providerfeemodel.UrgentcareFee;
                            }

                            _staffRepository.Update(staffdetails);
                            _staffRepository.SaveChanges();
                            response = new JsonModel()
                            {
                                data = new object(),
                                Message = StatusMessage.UpdatedSuccessfully,
                                StatusCode = (int)HttpStatusCodes.OK,
                            };
                        }
                    }

                }
                return response;
            }
            catch (Exception ex)
            {
                return new JsonModel()
                {
                    data = "",
                    Message = StatusMessage.ServerError,
                    StatusCode = (int)HttpStatusCodes.NoContent
                };
            }
        }


    }
}
