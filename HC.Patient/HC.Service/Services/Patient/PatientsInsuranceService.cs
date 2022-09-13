using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Images;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientsInsuranceService : BaseService, IPatientsInsuranceService
    {
        private HCOrganizationContext _context;
        private readonly IPatientInsuranceRepository _patientInsuranceRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IImageService _imageService;
        JsonModel response = new JsonModel();
        public PatientsInsuranceService(HCOrganizationContext context, IPatientInsuranceRepository patientInsuranceRepository, IPatientRepository patientRepository, IImageService imageService)
        {
            _context = context;
            _patientInsuranceRepository = patientInsuranceRepository;
            _patientRepository = patientRepository;
            _imageService = imageService;
        }

        public JsonModel SavePatientInsurance(List<PatientInsuranceModel> patientInsuranceListModel, TokenModel tokenModel)
        {
            if (patientInsuranceListModel != null && patientInsuranceListModel.Count > 0)
            {
                int index = 0;
                XElement xElement = new XElement("Parent");

                patientInsuranceListModel.ForEach(x =>
                {
                    xElement.Add(new XElement("Child",
                        new XElement("Index", index),
                        new XElement("Value", x.InsuranceIDNumber)
                        ));
                    index = index + 1;
                });
                List<PHIMultipleEncryptedModel> encryptedModel = _patientRepository.EncryptMultipleValues<PHIMultipleEncryptedModel>(xElement.ToString()).ToList();
                index = 0;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (PatientInsuranceModel patientInsuranceModel in patientInsuranceListModel)
                        {

                            PatientInsuranceDetails patientInsuranceDetails = null;
                            InsuredPerson insuredPerson = null;
                            DateTime CurrentDate = DateTime.UtcNow;
                            bool Updated = false;
                            if (patientInsuranceModel.Id == 0)//insert case
                            {
                                patientInsuranceDetails = _patientInsuranceRepository.Get(l => l.InsuranceCompanyID == patientInsuranceModel.InsuranceCompanyID && l.Id == 0 && l.PatientID == patientInsuranceModel.PatientID && l.IsDeleted == false && l.IsActive == true);
                                if (patientInsuranceDetails != null)//duplicate check
                                {
                                    //rollback insurance
                                    transaction.Rollback();
                                    return response = new JsonModel(new object(), StatusMessage.ClientInsuranceAlreadyLink, (int)HttpStatusCodes.UnprocessedEntity);
                                }
                                Updated = false;//Insert case
                                patientInsuranceDetails = new PatientInsuranceDetails();

                                //AutoMapper.Mapper.Map(patientInsuranceModel, patientInsuranceDetails);
                                MapPatientInsuranceToEntity(patientInsuranceModel, patientInsuranceDetails, encryptedModel[index].Value, tokenModel, "add");

                                if (patientInsuranceModel.InsuredPerson != null)
                                {
                                    patientInsuranceDetails.InsuredPerson = new InsuredPerson();
                                    AutoMapper.Mapper.Map(patientInsuranceModel.InsuredPerson, patientInsuranceDetails.InsuredPerson);
                                    patientInsuranceDetails.InsuredPerson.IsDeleted = false; patientInsuranceDetails.InsuredPerson.IsActive = true; patientInsuranceDetails.InsuredPerson.CreatedBy = tokenModel.UserID; patientInsuranceDetails.InsuredPerson.CreatedDate = CurrentDate;
                                }
                                //save images of insurance
                                _imageService.ConvertBase64ToImageForInsurance(patientInsuranceDetails);
                                response = _patientInsuranceRepository.SavePatientInsurance(patientInsuranceDetails, Updated);

                            }
                            else//update case
                            {
                                patientInsuranceDetails = _patientInsuranceRepository.Get(l => l.InsuranceCompanyID == patientInsuranceModel.InsuranceCompanyID && l.Id != patientInsuranceModel.Id && l.PatientID == patientInsuranceModel.PatientID && l.IsDeleted == false && l.IsActive == true);
                                if (patientInsuranceDetails != null)// if Patient already link with same insurance
                                {
                                    response = new JsonModel(new object(), StatusMessage.ClientInsuranceAlreadyLink, (int)HttpStatusCodes.UnprocessedEntity);
                                }
                                else
                                {
                                    Updated = true;
                                    patientInsuranceDetails = _patientInsuranceRepository.GetInsuranceDetail(patientInsuranceModel.Id);
                                    if (patientInsuranceDetails != null)
                                    {
                                        MapPatientInsuranceToEntity(patientInsuranceModel, patientInsuranceDetails, encryptedModel.Find(x => x.Index == index).Value, tokenModel, "update");
                                        if (patientInsuranceModel.InsuredPerson != null && patientInsuranceModel.InsuredPerson.Id > 0)
                                        {
                                            insuredPerson = new InsuredPerson();
                                            insuredPerson = patientInsuranceDetails.InsuredPerson;
                                            insuredPerson.UpdatedDate = CurrentDate;
                                            insuredPerson.UpdatedBy = tokenModel.UserID;
                                            insuredPerson.Address1 = patientInsuranceModel.InsuredPerson.Address1;
                                            insuredPerson.Address2 = patientInsuranceModel.InsuredPerson.Address2;
                                            insuredPerson.ApartmentNumber = patientInsuranceModel.InsuredPerson.ApartmentNumber;
                                            insuredPerson.City = patientInsuranceModel.InsuredPerson.City;
                                            insuredPerson.CountryID = patientInsuranceModel.InsuredPerson.CountryID;
                                            insuredPerson.Dob = patientInsuranceModel.InsuredPerson.Dob;
                                            insuredPerson.FirstName = patientInsuranceModel.InsuredPerson.FirstName;
                                            insuredPerson.GenderID = patientInsuranceModel.InsuredPerson.GenderID;
                                            insuredPerson.LastName = patientInsuranceModel.InsuredPerson.LastName;
                                            insuredPerson.Latitude = patientInsuranceModel.InsuredPerson.Latitude;
                                            insuredPerson.Longitude = patientInsuranceModel.InsuredPerson.Longitude;
                                            insuredPerson.MiddleName = patientInsuranceModel.InsuredPerson.MiddleName;
                                            insuredPerson.OtherRelationshipName = patientInsuranceModel.InsuredPerson.OtherRelationshipName;
                                            insuredPerson.PatientID = patientInsuranceModel.InsuredPerson.PatientID;
                                            insuredPerson.PatientInsuranceID = patientInsuranceModel.InsuredPerson.PatientInsuranceID;
                                            insuredPerson.Phone = patientInsuranceModel.InsuredPerson.Phone;
                                            insuredPerson.RelationshipID = patientInsuranceModel.InsuredPerson.RelationshipID;
                                            insuredPerson.StateID = patientInsuranceModel.InsuredPerson.StateID;
                                            insuredPerson.Title = patientInsuranceModel.InsuredPerson.Title;
                                            insuredPerson.Zip = patientInsuranceModel.InsuredPerson.Zip;
                                            if (patientInsuranceModel.InsuredPerson.IsDeleted)
                                            {
                                                insuredPerson.IsDeleted = true;
                                                insuredPerson.DeletedBy = tokenModel.UserID;
                                                insuredPerson.DeletedDate = CurrentDate;
                                            }
                                        }
                                        else
                                        {
                                            if (patientInsuranceModel.InsuredPerson != null)
                                            {
                                                insuredPerson = new InsuredPerson();
                                                AutoMapper.Mapper.Map(patientInsuranceModel.InsuredPerson, insuredPerson);
                                                patientInsuranceDetails.InsuredPerson = insuredPerson;
                                            }
                                        }
                                        //save images of insurance
                                        _imageService.ConvertBase64ToImageForInsurance(patientInsuranceDetails);
                                        response = _patientInsuranceRepository.SavePatientInsurance(patientInsuranceDetails, Updated);
                                    }
                                }
                            }
                            index = index + 1;
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        response = new JsonModel(null, StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message);
                        //rollback transaction
                        transaction.Rollback();
                    }
                    return response;
                }
            }
            else { return response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound); }

        }
        private void MapPatientInsuranceToEntity(PatientInsuranceModel patientInsuranceModel, PatientInsuranceDetails patientInsuranceDetails, byte[] insuranceIdNumber, TokenModel token, string action)
        {
            if (action == "update")
            {
                patientInsuranceDetails.UpdatedBy = token.UserID;
                patientInsuranceDetails.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                patientInsuranceDetails.IsActive = true;
                patientInsuranceDetails.IsDeleted = false;
                patientInsuranceDetails.CreatedBy = token.UserID;
                patientInsuranceDetails.CreatedDate = DateTime.UtcNow;
            }
            patientInsuranceDetails.InsurancePlanName = patientInsuranceModel.InsurancePlanName;
            patientInsuranceDetails.Notes = patientInsuranceModel.Notes;
            patientInsuranceDetails.InsurancePlanTypeID = patientInsuranceModel.InsurancePlanTypeID;
            patientInsuranceDetails.IsVerified = patientInsuranceModel.IsVerified;
            patientInsuranceDetails.InsurancePersonSameAsPatient = patientInsuranceModel.InsurancePersonSameAsPatient;
            patientInsuranceDetails.VisitsAllowedPerYear = patientInsuranceModel.VisitsAllowedPerYear;
            patientInsuranceDetails.CardIssueDate = patientInsuranceModel.CardIssueDate;
            patientInsuranceDetails.InsuranceClaimOfficeNumber = patientInsuranceModel.InsuranceClaimOfficeNumber;
            patientInsuranceDetails.InsuranceCompanyAddress = patientInsuranceModel.InsuranceCompanyAddress;
            patientInsuranceDetails.InsuranceCompanyID = patientInsuranceModel.InsuranceCompanyID;
            patientInsuranceDetails.InsuranceGroupName = patientInsuranceModel.InsuranceGroupName;
            patientInsuranceDetails.InsuranceIDNumber = insuranceIdNumber;
            patientInsuranceDetails.InsuranceTypeID = patientInsuranceModel.InsuranceTypeID;
            patientInsuranceDetails.CarrierPayerID = patientInsuranceModel.CarrierPayerID;
            patientInsuranceDetails.InsuranceGroupNumber = patientInsuranceModel.InsuranceGroupNumber;
            patientInsuranceDetails.Base64Back = patientInsuranceModel.Base64Back;
            patientInsuranceDetails.Base64Front = patientInsuranceModel.Base64Front;
            patientInsuranceDetails.PatientID = patientInsuranceModel.PatientID;
            if (patientInsuranceModel.IsDeleted)
            {
                patientInsuranceDetails.IsDeleted = true;
                patientInsuranceDetails.DeletedBy = token.UserID;
                patientInsuranceDetails.DeletedDate = DateTime.UtcNow;
            }
        }
        public JsonModel GetPatientInsurances(PatientFilterModel filterModel, TokenModel tokenModel)
        {
            try
            {
              //  Dictionary<string, object> patientInsuranceInsuredPersonModel = _patientInsuranceRepository.GetPatientInsurances(patientId, tokenModel);
                Dictionary<string, object> patientInsuranceInsuredPersonModel = _patientInsuranceRepository.GetPatientInsurancesWithPagination(filterModel, tokenModel);
                if ((List<PatientInsuranceModel>)patientInsuranceInsuredPersonModel["PatientInsurance"] != null && ((List<PatientInsuranceModel>)patientInsuranceInsuredPersonModel["PatientInsurance"]).Count > 0)
                {

                    var imageUrls = (List<PatientInsuranceModel>)patientInsuranceInsuredPersonModel["PatientInsurance"];
                    imageUrls.ForEach(a =>
                    {
                        if (!string.IsNullOrEmpty(a.InsurancePhotoPathFront)) { a.InsurancePhotoPathFront = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.PatientInsuranceFront, a.InsurancePhotoPathFront); } else { a.InsurancePhotoPathFront = ""; }
                        if (!string.IsNullOrEmpty(a.InsurancePhotoPathThumbFront)) { a.InsurancePhotoPathThumbFront = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.PatientInsuranceFrontThumb, a.InsurancePhotoPathThumbFront); } else { a.InsurancePhotoPathThumbFront = ""; }
                        if (!string.IsNullOrEmpty(a.InsurancePhotoPathBack)) { a.InsurancePhotoPathBack = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.PatientInsuranceBack, a.InsurancePhotoPathBack); } else { a.InsurancePhotoPathBack = ""; }
                        if (!string.IsNullOrEmpty(a.InsurancePhotoPathThumbBack)) { a.InsurancePhotoPathThumbBack = CommonMethods.CreateImageUrl(tokenModel.Request, ImagesPath.PatientInsuranceBackThumb, a.InsurancePhotoPathThumbBack); } else { a.InsurancePhotoPathThumbBack = ""; }
                    });
                    patientInsuranceInsuredPersonModel["PatientInsurance"] = imageUrls;
                    response = new JsonModel(patientInsuranceInsuredPersonModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                    response.meta = new Meta(patientInsuranceInsuredPersonModel, filterModel);
                }
                else { response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound); }
            }
            catch (Exception e) { response = new JsonModel(new object(), StatusMessage.ServerError, (int)HttpStatusCodes.InternalServerError, e.Message); }
            return response;
        }
    }
}
