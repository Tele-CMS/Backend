using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Model;
using HC.Patient.Model.Common;
using HC.Patient.Model.Payment;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HC.Patient.Service.Services
{
    public class ProviderAppointmentService : BaseService, IProviderAppointmentService
    {
        private readonly IProviderAppointmentRepository _providerAppointmentRepository;
        private readonly ILocationService _locationService;
        private JsonModel response;
        private readonly IStaffService _staffService;
        private readonly HCOrganizationContext _context;
        public ProviderAppointmentService(
            IProviderAppointmentRepository providerAppointmentRepository,
            ILocationService locationService,
            IStaffService staffService,
            HCOrganizationContext context
            )
        {
            _providerAppointmentRepository = providerAppointmentRepository;
            _staffService = staffService;
            _locationService = locationService;
            _context = context;
        }
        //public JsonModel GetProviderListToMakeAppointment(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel)
        //{
        //    response = new JsonModel();
        //    //var locationModal = _locationService.GetLocationOffsets(Convert.ToInt16(appointmentSearchModel.Locations), tokenModel);
        //    if (appointmentSearchModel.Rates == "" && appointmentSearchModel.ReviewRating=="")
        //    {
        //        var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointment(tokenModel, null, appointmentSearchModel);
        //        if (providerList != null && providerList.Count > 0)
        //        {
        //            response.data = providerList;
        //            response.Message = StatusMessage.FetchMessage;
        //            response.StatusCode = (int)HttpStatusCode.OK;
        //            response.meta = new Meta(providerList, appointmentSearchModel);
        //        }
        //        else
        //        {
        //            response.data = new object();
        //            response.Message = StatusMessage.NotFound;
        //            response.StatusCode = (int)HttpStatusCode.NotFound;
        //        }
        //        return response;
        //    }

        //    if (appointmentSearchModel.Rates=="11")
        //    {
        //        var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentLessRate(tokenModel, null, appointmentSearchModel);
        //        if (providerList != null && providerList.Count > 0)
        //        {
        //            response.data = providerList;
        //            response.Message = StatusMessage.FetchMessage;
        //            response.StatusCode = (int)HttpStatusCode.OK;
        //            response.meta = new Meta(providerList, appointmentSearchModel);
        //        }
        //        else
        //        {
        //            response.data = new object();
        //            response.Message = StatusMessage.NotFound;
        //            response.StatusCode = (int)HttpStatusCode.NotFound;
        //        }
        //        return response;
        //    }
        //    if (appointmentSearchModel.Rates == "12")
        //    {
        //        var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentForRate(tokenModel, null, appointmentSearchModel,"100","200");
        //        if (providerList != null && providerList.Count > 0)
        //        {
        //            response.data = providerList;
        //            response.Message = StatusMessage.FetchMessage;
        //            response.StatusCode = (int)HttpStatusCode.OK;
        //            response.meta = new Meta(providerList, appointmentSearchModel);
        //        }
        //        else
        //        {
        //            response.data = new object();
        //            response.Message = StatusMessage.NotFound;
        //            response.StatusCode = (int)HttpStatusCode.NotFound;
        //        }
        //        return response;
        //    }
        //    if (appointmentSearchModel.Rates == "13")
        //    {
        //        var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentForRate(tokenModel, null, appointmentSearchModel, "200", "300");
        //        if (providerList != null && providerList.Count > 0)
        //        {
        //            response.data = providerList;
        //            response.Message = StatusMessage.FetchMessage;
        //            response.StatusCode = (int)HttpStatusCode.OK;
        //            response.meta = new Meta(providerList, appointmentSearchModel);
        //        }
        //        else
        //        {
        //            response.data = new object();
        //            response.Message = StatusMessage.NotFound;
        //            response.StatusCode = (int)HttpStatusCode.NotFound;
        //        }
        //        return response;
        //    }
        //    if (appointmentSearchModel.Rates == "14")
        //    {
        //        var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentMoreRate(tokenModel, null, appointmentSearchModel);
        //        if (providerList != null && providerList.Count > 0)
        //        {
        //            response.data = providerList;
        //            response.Message = StatusMessage.FetchMessage;
        //            response.StatusCode = (int)HttpStatusCode.OK;
        //            response.meta = new Meta(providerList, appointmentSearchModel);
        //        }
        //        else
        //        {
        //            response.data = new object();
        //            response.Message = StatusMessage.NotFound;
        //            response.StatusCode = (int)HttpStatusCode.NotFound;
        //        }
        //        return response;
        //    }

        //    if (appointmentSearchModel.ReviewRating != "")
        //    {
        //        var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentForReviewrating(tokenModel, null, appointmentSearchModel, appointmentSearchModel.ReviewRating);
        //        if (providerList != null && providerList.Count > 0)
        //        {
        //            response.data = providerList;
        //            response.Message = StatusMessage.FetchMessage;
        //            response.StatusCode = (int)HttpStatusCode.OK;
        //            response.meta = new Meta(providerList, appointmentSearchModel);
        //        }
        //        else
        //        {
        //            response.data = new object();
        //            response.Message = StatusMessage.NotFound;
        //            response.StatusCode = (int)HttpStatusCode.NotFound;
        //        }
        //        return response;
        //    }

        //    return response;
        //}
      
        public JsonModel GetProvider(TokenModel tokenModel, string id)
        {
            int staffId = 0;
            bool decripted = int.TryParse(id, out staffId);
            if (!decripted)
            {
                Int32.TryParse(Common.CommonMethods.Decrypt(id.Replace(" ", "+")), out staffId);
            }
            response = new JsonModel();
            //var locationModal = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
            var provider = _staffService.GetStaffById(staffId, tokenModel);
            if (provider != null)
            {
                var staffs = (Entity.Staffs)provider.data;
                staffs.Users3 = null;
                staffs.providerId = Common.CommonMethods.Encrypt(staffs.Id.ToString());
                staffs.UserName = null;
                staffs.Password = null;
                staffs.DOB = new DateTime();
                //staffs.Email = null;
                //staffs.PhoneNumber = null;
                staffs.FollowUpDays = staffs.FollowUpDays ?? 0;
                staffs.FollowUpPayRate = staffs.FollowUpPayRate ?? 0;
                staffs.TimeInterval = staffs.TimeInterval ?? 30;
               
                response.data = staffs;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return response;
        }

        public JsonModel GetProviderListToMakeAppointment(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel)
        {
            response = new JsonModel();
            //var locationModal = _locationService.GetLocationOffsets(Convert.ToInt16(appointmentSearchModel.Locations), tokenModel);
            if (appointmentSearchModel.Rates == "" && appointmentSearchModel.ReviewRating == "")
            {
                var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointment(tokenModel, null, appointmentSearchModel);
                if (providerList != null && providerList.Count > 0)
                {
                    providerList = getStaffsCancelationRules(providerList);
                    response.data = providerList;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(providerList, appointmentSearchModel);
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return response;
            }

            if (appointmentSearchModel.Rates != "" && appointmentSearchModel.ReviewRating == "" )
            {
                var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentForRate(tokenModel, null, appointmentSearchModel, appointmentSearchModel.Rates, appointmentSearchModel.MinRate);
                if (providerList != null && providerList.Count > 0)
                {
                    providerList = getStaffsCancelationRules(providerList);
                    response.data = providerList;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(providerList, appointmentSearchModel);
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return response;
            }
            
            if (appointmentSearchModel.ReviewRating != "")
            {
                var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentForReviewrating(tokenModel, null, appointmentSearchModel, appointmentSearchModel.ReviewRating);
                if (providerList != null && providerList.Count > 0)
                {
                    providerList = getStaffsCancelationRules(providerList);
                    response.data = providerList;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(providerList, appointmentSearchModel);
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return response;
            }

            return response;
        }

        public JsonModel SortedProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel)
        {
            response = new JsonModel();
            //var locationModal = _locationService.GetLocationOffsets(Convert.ToInt16(appointmentSearchModel.Locations), tokenModel);
           
                var providerList = _providerAppointmentRepository.SortedProviderAvailableList(tokenModel, null, appointmentSearchModel);
                if (providerList != null && providerList.Count > 0)
                {
                providerList = getStaffsCancelationRules(providerList);
                response.data = providerList;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(providerList, appointmentSearchModel);
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return response;
           
        }

        public JsonModel SearchTextProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel)
        {
            
           response = new JsonModel();
            if(!string.IsNullOrEmpty(appointmentSearchModel.ProvidersearchText))
            {
                var specialitylist = _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeName.ToLower().Contains(appointmentSearchModel.ProvidersearchText.ToLower()) && a.OrganizationID == tokenModel.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                {
                    id = x.Id,
                    value = x.GlobalCodeValue,
                    key = x.GlobalCodeName
                }).ToList();

                var serviceslist = _context.MasterServices.Where(a => a.IsDeleted == false && a.IsActive == true && a.ServiceName.ToLower().Contains(appointmentSearchModel.ProvidersearchText.ToLower()) && a.OrganizationId == tokenModel.OrganizationID).OrderBy(a => a.ServiceName).Select(x => new MasterDropDown()
                {
                    id = x.Id,
                    value = x.ServiceName,
                    key = x.Id.ToString()
                }).ToList();

                //var taxonomylist = _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeName.ToLower() == appointmentSearchModel.ProvidersearchText && a.OrganizationID == tokenModel.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                //{
                //    id = x.Id,
                //    value = x.GlobalCodeName,
                //    key = x.GlobalCodeValue
                //}).ToList();

                if (specialitylist.Count > 0)
                {
                    //appointmentSearchModel.Specialities = (specialitylist[0].id).ToString();
                    appointmentSearchModel.Specialities = String.Join(",", specialitylist.Select(s => s.id.ToString()));
                    //appointmentSearchModel.ProvidersearchText = "";
                }
                else if (serviceslist.Count > 0)
                {
                   // appointmentSearchModel.Services = (serviceslist[0].id).ToString();
                    appointmentSearchModel.Services = String.Join(",", serviceslist.Select(s => s.id.ToString()));
                  //  appointmentSearchModel.ProvidersearchText = "";
                }
            }
            //var locationModal = _locationService.GetLocationOffsets(Convert.ToInt16(appointmentSearchModel.Locations), tokenModel);
           

           

            //else if (taxonomylist.Count > 0)
            //{
            //    appointmentSearchModel.Services = (taxonomylist[0].id).ToString();
            //}


            //if (specialitylist.Count > 0 || serviceslist.Count > 0)
            //{
                var providerList = _providerAppointmentRepository.GetSpecialitySearchTextProviderListToMakeAppointmentKeySearch(tokenModel, null, appointmentSearchModel);
                if (providerList != null && providerList.Count > 0)
                {
                providerList = getStaffsCancelationRules(providerList);
                response.data = providerList;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(providerList, appointmentSearchModel);
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return response;
           // }
           
            //else
            //{
            //    response.data = new object();
            //    response.Message = StatusMessage.NotFound;
            //    response.StatusCode = (int)HttpStatusCode.NotFound;
            //}
           // return response;

        }

        private List<AppointmentModel> getStaffsCancelationRules(List<AppointmentModel> providerList)
        
        {
            var cancellatationRulesRes = _staffService.GetCancelationRules(providerList.Select(x => x.StaffId).ToList());
            var cancellatationRules = (List<CancelationRuleModel>)cancellatationRulesRes.data;

            if (cancellatationRules.Count > 0)
            {
                providerList = providerList.Select(s =>
                {
                    s.CancelationRules = cancellatationRules.Where(x => x.StaffId == s.StaffId).ToList();
                    return s;
                }).ToList();

               
            }
            return providerList;
        }

        public JsonModel GetUrgentCareProviderListToMakeAppointment(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel)
        {
            response = new JsonModel();
            //var locationModal = _locationService.GetLocationOffsets(Convert.ToInt16(appointmentSearchModel.Locations), tokenModel);
            if (appointmentSearchModel.Rates == "" && appointmentSearchModel.ReviewRating == "")
            {
                var providerList = _providerAppointmentRepository.GetUrgentCareProviderListToMakeAppointment(tokenModel, null, appointmentSearchModel);
                if (providerList != null && providerList.Count > 0)
                {
                    providerList = getStaffsCancelationRules(providerList);
                    response.data = providerList;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(providerList, appointmentSearchModel);
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return response;
            }

            if (appointmentSearchModel.Rates != "" && appointmentSearchModel.ReviewRating == "")
            {
                var providerList = _providerAppointmentRepository.GetUrgentCareProviderListToMakeAppointmentForRate(tokenModel, null, appointmentSearchModel, appointmentSearchModel.Rates, appointmentSearchModel.MinRate);
                if (providerList != null && providerList.Count > 0)
                {
                    providerList = getStaffsCancelationRules(providerList);
                    response.data = providerList;
                    response.Message = StatusMessage.FetchMessage;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.meta = new Meta(providerList, appointmentSearchModel);
                }
                else
                {
                    response.data = new object();
                    response.Message = StatusMessage.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return response;
            }

            //if (appointmentSearchModel.ReviewRating != "")
            //{
            //    var providerList = _providerAppointmentRepository.GetProviderListToMakeAppointmentForReviewrating(tokenModel, null, appointmentSearchModel, appointmentSearchModel.ReviewRating);
            //    if (providerList != null && providerList.Count > 0)
            //    {
            //        providerList = getStaffsCancelationRules(providerList);
            //        response.data = providerList;
            //        response.Message = StatusMessage.FetchMessage;
            //        response.StatusCode = (int)HttpStatusCode.OK;
            //        response.meta = new Meta(providerList, appointmentSearchModel);
            //    }
            //    else
            //    {
            //        response.data = new object();
            //        response.Message = StatusMessage.NotFound;
            //        response.StatusCode = (int)HttpStatusCode.NotFound;
            //    }
            //    return response;
            //}

            return response;
        }

        public JsonModel SearchTextUrgentCareProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel)
        {

            response = new JsonModel();
            if (!string.IsNullOrEmpty(appointmentSearchModel.ProvidersearchText))
            {
                var specialitylist = _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeName.ToLower().Contains(appointmentSearchModel.ProvidersearchText.ToLower()) && a.OrganizationID == tokenModel.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                {
                    id = x.Id,
                    value = x.GlobalCodeValue,
                    key = x.GlobalCodeName
                }).ToList();

                var serviceslist = _context.MasterServices.Where(a => a.IsDeleted == false && a.IsActive == true && a.ServiceName.ToLower().Contains(appointmentSearchModel.ProvidersearchText.ToLower()) && a.OrganizationId == tokenModel.OrganizationID).OrderBy(a => a.ServiceName).Select(x => new MasterDropDown()
                {
                    id = x.Id,
                    value = x.ServiceName,
                    key = x.Id.ToString()
                }).ToList();

                //var taxonomylist = _context.GlobalCode.Where(a => a.IsDeleted == false && a.IsActive == true && a.GlobalCodeName.ToLower() == appointmentSearchModel.ProvidersearchText && a.OrganizationID == tokenModel.OrganizationID).OrderBy(a => a.DisplayOrder).Select(x => new MasterDropDown()
                //{
                //    id = x.Id,
                //    value = x.GlobalCodeName,
                //    key = x.GlobalCodeValue
                //}).ToList();

                if (specialitylist.Count > 0)
                {
                    //appointmentSearchModel.Specialities = (specialitylist[0].id).ToString();
                    appointmentSearchModel.Specialities = String.Join(",", specialitylist.Select(s => s.id.ToString()));
                    //appointmentSearchModel.ProvidersearchText = "";
                }
                else if (serviceslist.Count > 0)
                {
                    // appointmentSearchModel.Services = (serviceslist[0].id).ToString();
                    appointmentSearchModel.Services = String.Join(",", serviceslist.Select(s => s.id.ToString()));
                    //  appointmentSearchModel.ProvidersearchText = "";
                }
            }
            //var locationModal = _locationService.GetLocationOffsets(Convert.ToInt16(appointmentSearchModel.Locations), tokenModel);




            //else if (taxonomylist.Count > 0)
            //{
            //    appointmentSearchModel.Services = (taxonomylist[0].id).ToString();
            //}


            //if (specialitylist.Count > 0 || serviceslist.Count > 0)
            //{
            var providerList = _providerAppointmentRepository.GetSpecialitySearchTextUrgentCareProviderListToMakeAppointmentKeySearch(tokenModel, null, appointmentSearchModel);
            if (providerList != null && providerList.Count > 0)
            {
                providerList = getStaffsCancelationRules(providerList);
                response.data = providerList;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.meta = new Meta(providerList, appointmentSearchModel);
            }
            else
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return response;
            // }

            //else
            //{
            //    response.data = new object();
            //    response.Message = StatusMessage.NotFound;
            //    response.StatusCode = (int)HttpStatusCode.NotFound;
            //}
            // return response;

        }

        public JsonModel SortedUrgentCareProviderAvailableList(TokenModel tokenModel, AppointmentSearchModel appointmentSearchModel)
        {
            response = new JsonModel();
            //var locationModal = _locationService.GetLocationOffsets(Convert.ToInt16(appointmentSearchModel.Locations), tokenModel);

            var providerList = _providerAppointmentRepository.SortedUrgentCareProviderAvailableList(tokenModel, null, appointmentSearchModel);
            if (providerList != null && providerList.Count > 0)
            {
                providerList = getStaffsCancelationRules(providerList);
                response.data = providerList;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.meta = new Meta(providerList, appointmentSearchModel);
            }
            else
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return response;

        }

       
    }



}
