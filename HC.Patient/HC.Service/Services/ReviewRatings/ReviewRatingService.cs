using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.ReviewRatings;
using HC.Patient.Repositories.IRepositories.Appointment;
using HC.Patient.Repositories.IRepositories.ReviewRating;
using HC.Patient.Service.IServices.ReviewRatings;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using static HC.Common.Enums.CommonEnum;
using HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Patient.Repositories.IRepositories.Organizations;
using Microsoft.AspNetCore.Hosting;
using HC.Patient.Model;
using HC.Patient.Service.IServices;
using HC.Common.Services;
using Microsoft.AspNetCore.Http;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Model.Staff;

namespace HC.Patient.Service.Services.ReviewRatings
{
    public class ReviewRatingService : BaseService, IReviewRatingService
    {
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IReviewRatingRepository _reviewRatingRepository;
        private readonly IAppointmentStaffRepository _appointmentStaffRepository;
        private readonly ITokenService _tokenService;
        private readonly IHostingEnvironment _env;
        private readonly IEmailService _emailSender;
        private readonly IEmailWriteService _emailWriteService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IConfiguration _configuration;
        private readonly IStaffService _staffService;
        public ReviewRatingService(IReviewRatingRepository reviewRatingRepository, IAppointmentStaffRepository  appointmentStaffRepository, ITokenService tokenService, IOrganizationSMTPRepository organizationSMTPRepository,IHostingEnvironment env, IEmailService emailSender,IConfiguration configuration, IStaffService staffService,
            IEmailWriteService emailWriteService)
        {
            _reviewRatingRepository = reviewRatingRepository;
            _appointmentStaffRepository = appointmentStaffRepository;
            _tokenService = tokenService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _env = env;
            _emailWriteService = emailWriteService;
            _emailSender = emailSender;
            _configuration = configuration;
            _staffService = staffService;
        }
        public JsonModel SaveUpdateReviewRating(ReviewRatingsModel reviewRatingsModel, TokenModel tokenModel)
        {

            Entity.ReviewRatings reviewRating =null;
            if (reviewRatingsModel.Id == 0)
            {


                reviewRating = new Entity.ReviewRatings();
                    AutoMapper.Mapper.Map(reviewRatingsModel, reviewRating);
                reviewRating.CreatedBy = tokenModel.UserID;
                reviewRating.CreatedDate= DateTime.UtcNow;
                reviewRating.IsDeleted = false;
                reviewRating.IsActive = true;
                _reviewRatingRepository.Create(reviewRating);
                _reviewRatingRepository.SaveChanges();
                 response = new JsonModel(reviewRating, StatusMessage.AddReview, (int)HttpStatusCode.OK);
                
            }
            else
            {
                reviewRating = _reviewRatingRepository.Get(l => l.Id == reviewRatingsModel.Id && l.IsDeleted == false);
             
                    if (reviewRating != null)
                    {
                        AutoMapper.Mapper.Map(reviewRatingsModel, reviewRating);
                    reviewRating.UpdatedBy = tokenModel.UserID;
                    reviewRating.UpdatedDate = DateTime.UtcNow;
                    _reviewRatingRepository.Update(reviewRating);
                    _reviewRatingRepository.SaveChanges();
                     response = new JsonModel(reviewRating, StatusMessage.updateReview, (int)HttpStatusCode.OK);
                   
                }
            }
            
            var staffResponse = _staffService.GetStaffProfileData(reviewRatingsModel.StaffId, tokenModel);
            StaffProfileModel staff;

            if (staffResponse.StatusCode == (int)HttpStatusCodes.OK)
                staff = (StaffProfileModel)staffResponse.data;
            else
                staff = null;
            if (staff != null)
            {
                string error = SendAppointmentEmail(reviewRatingsModel,
                    staff.Email,
                     staff.FirstName + " " + staff.LastName,
                     (int)EmailType.BookAppointment,
                     (int)EmailSubType.BookAppointmentToProvider,
                     reviewRatingsModel.PatientAppointmentId,
                     "/templates/review-rating.html",
                     "Review Rating Added",
                     tokenModel,
                     tokenModel.Request.Request
                     );

            }
            return response;
        }
        private string SendAppointmentEmail(ReviewRatingsModel reviewRatingsModel, string toEmail, string username, int emailType, int emailSubType, int primaryId, string templatePath, string subject, TokenModel tokenModel, HttpRequest Request)
        {
            //Get Current Login User Organization
            tokenModel.Request.Request.Headers.TryGetValue("BusinessToken", out StringValues businessName);
            Organization organization = _tokenService.GetOrganizationByOrgId(tokenModel.OrganizationID, tokenModel);

            //Get Current User Smtp Details
            OrganizationSMTPDetails organizationSMTPDetail = _organizationSMTPRepository.Get(a => a.OrganizationID == tokenModel.OrganizationID && a.IsDeleted == false && a.IsActive == true);
            OrganizationSMTPCommonModel organizationSMTPDetailModel = new OrganizationSMTPCommonModel();
            AutoMapper.Mapper.Map(organizationSMTPDetail, organizationSMTPDetailModel);
            organizationSMTPDetailModel.SMTPPassword = CommonMethods.Decrypt(organizationSMTPDetailModel.SMTPPassword);

            var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            var emailHtml = System.IO.File.ReadAllText(_env.WebRootPath + templatePath);
            //#if DEBUG

            //#else
            //            emailHtml = emailHtml.Replace("{{action_url}}",tokenModel.DomainName+ "/register" + "?token=" + invitaionId);
            //#endif
            var hostingServer = _configuration.GetSection("HostingServer").Value;
            emailHtml = emailHtml.Replace("{{img_url}}", hostingServer + "img/cbimage.jpg");
            emailHtml = emailHtml.Replace("{{username}}", username);
            emailHtml = emailHtml.Replace("{{operating_system}}", osNameAndVersion);
            emailHtml = emailHtml.Replace("{{browser_name}}", Request.Headers["User-Agent"].ToString());
            emailHtml = emailHtml.Replace("{{organizationName}}", organization.OrganizationName);
            emailHtml = emailHtml.Replace("{{organizationEmail}}", organization.Email);
            emailHtml = emailHtml.Replace("{{organizationPhone}}", organization.ContactPersonPhoneNumber);
            emailHtml = emailHtml.Replace("{{AppReview}}", reviewRatingsModel.Review);
            emailHtml = emailHtml.Replace("{{AppRating}}",Convert.ToString(reviewRatingsModel.Rating));
            EmailModel emailModel = new EmailModel
            {
                EmailBody = CommonMethods.Encrypt(emailHtml),
                ToEmail = CommonMethods.Encrypt(toEmail),
                EmailSubject = CommonMethods.Encrypt(subject),
                EmailType = emailType,
                EmailSubType = emailSubType,
                PrimaryId = primaryId,
                CreatedBy = tokenModel.UserID
            };
            ////Send Email
            
            var error = _emailSender.SendEmails(toEmail, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName).Result; //_emailSender.SendEmail(organizationSMTPDetailModel.SMTPUserName, subject, emailHtml, organizationSMTPDetailModel, organization.OrganizationName, toEmail);
            if (!string.IsNullOrEmpty(error))
                emailModel.EmailStatus = false;
            else
                emailModel.EmailStatus = true;
            //Maintain Email log into Db
            var email = _emailWriteService.SaveEmailLog(emailModel, tokenModel);
            return error;
        }


        public JsonModel GetReviewRatingById(int id, TokenModel tokenModel)
        {
            Entity.ReviewRatings reviewRating = _reviewRatingRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (reviewRating != null)
            {
                ReviewRatingsModel reviewRatingsModel = new ReviewRatingsModel();
                AutoMapper.Mapper.Map(reviewRating, reviewRatingsModel);
                response = new JsonModel(reviewRatingsModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }
        public JsonModel GetReviewRatingByStaffId(string id,int pageNumber,int pageSize)
        {
            //Int32.TryParse(id.Replace(" ", "+"), out int staffId);
            Int32.TryParse(Common.CommonMethods.Decrypt(id.Replace(" ", "+")), out int staffId);
            List<ReviewRatingProviderModel> list = new List<ReviewRatingProviderModel>();
            list = _reviewRatingRepository.GetReviewRatingListByStaffId(staffId,pageNumber, pageSize);
               response = new JsonModel(list, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            FilterModel filterModel = new FilterModel()
            {
                pageNumber = pageNumber,
                pageSize = pageSize
            };
           
            response.meta= new Meta(list, filterModel);
            return response;
        }
        public JsonModel ReviewRatingAverageByStaffId(string id)
        {
            // string isd = Common.CommonMethods.Encrypt(id);
            // Int32.TryParse(Common.CommonMethods.Decrypt(id.Replace(" ", "+")), out int staffId);
            //// List<ReviewRatingProviderModel> list = new List<ReviewRatingProviderModel>();
            Int32.TryParse(id.Replace(" ", "+"), out int staffId);
            var data = _reviewRatingRepository.GetReviewRatingAverageByStaffId(staffId);
            response = new JsonModel(data, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
             return response;
        }


        
    }
}
