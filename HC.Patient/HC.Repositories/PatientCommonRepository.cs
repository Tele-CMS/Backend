using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HC.Patient.Repositories
{
    public class PatientCommonRepository : IPatientCommonRepository
    {
        private readonly HCOrganizationContext _context;
        public PatientCommonRepository(HCOrganizationContext context)
        {
            this._context = context;
        }
        public Patients PatientExist(Patients patientInfo)
        {
            try
            {
                Patients newPatinet = new Patients();
                if (patientInfo.Id > 0)
                {
                   newPatinet = _context.Patients.Where(a => a.FirstName == patientInfo.FirstName && a.LastName == patientInfo.LastName /*&& a.PrimaryProvider == patientInfo.PrimaryProvider*/ && a.SSN == patientInfo.SSN && a.Id != patientInfo.Id).FirstOrDefault();
                }
                else
                {
                    newPatinet = _context.Patients.Where(a => a.FirstName == patientInfo.FirstName && a.LastName == patientInfo.LastName /*&& a.PrimaryProvider == patientInfo.PrimaryProvider*/ && a.SSN == patientInfo.SSN).FirstOrDefault();
                }
                if (newPatinet != null)
                {
                    return newPatinet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Patients UpdatePatientData(int id, Patients patientInfo)
        {
            try
            {
                if (id > 0)
                {
                    var patient = _context.Patients.Find(id);
                    if (patient != null)
                    {
                        patient.MRN = patientInfo.MRN;
                        patient.FirstName = patientInfo.FirstName;
                        patient.MiddleName = patientInfo.MiddleName;
                        patient.LastName = patientInfo.LastName;                        
                        patient.Gender = patientInfo.Gender;
                        patient.DOB = patientInfo.DOB;
                        patient.SSN = patientInfo.SSN;
                        patient.Email = patientInfo.Email;
                        patient.MaritalStatus = patientInfo.MaritalStatus;
                        patient.Race = patientInfo.Race;
                        patient.SecondaryRaceID = patientInfo.SecondaryRaceID;
                        patient.Ethnicity = patientInfo.Ethnicity;                        
                        patient.PrimaryProvider = patientInfo.PrimaryProvider;
                        patient.EmergencyContactFirstName = patientInfo.EmergencyContactFirstName;
                        patient.EmergencyContactLastName = patientInfo.EmergencyContactLastName;
                        patient.EmergencyContactPhone = patientInfo.EmergencyContactPhone;
                        patient.EmergencyContactRelationship = patientInfo.EmergencyContactRelationship;                        
                        if (!string.IsNullOrEmpty(patientInfo.PhotoBase64))
                        {
                            patient.PhotoPath = patientInfo.PhotoPath;
                            patient.PhotoThumbnailPath = patientInfo.PhotoThumbnailPath;
                        }
                        patient.Citizenship = patientInfo.Citizenship;

                        _context.SaveChanges();
                    }
                    return patient;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PatientAddress UpdatePatientAddressData(int id, PatientAddress patientInfo)
        {
            try
            {
                if (id > 0)
                {
                    var patientAddress = _context.PatientAddress.Find(id);
                    if (patientAddress != null)
                    {
                        patientAddress.PatientID = patientInfo.PatientID;
                        patientAddress.IsMailingSame = patientInfo.IsMailingSame;
                        patientAddress.Address1 = patientInfo.Address1;
                        patientAddress.Address2 = patientInfo.Address2;
                        patientAddress.City = patientInfo.City;
                        patientAddress.StateID = patientInfo.StateID;
                        patientAddress.Zip = patientInfo.Zip;
                        patientAddress.Phone = patientInfo.Phone;
                        patientAddress.CountryID = patientInfo.CountryID;
                        patientAddress.IsActive = patientInfo.IsActive;
                        patientAddress.UpdatedDate = DateTime.UtcNow;
                        patientAddress.UpdatedBy = patientInfo.UpdatedBy;
                        _context.SaveChanges();
                    }
                    return patientAddress;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public PatientInsuranceDetails UpdatePatientInsuranceData(int id, PatientInsuranceDetails patientInfo)
        {
            try
            {
                if (id > 0)
                {
                    var patientInsuranceDetails = _context.PatientInsuranceDetails.Find(id);
                    if (patientInsuranceDetails != null)
                    {
                        patientInsuranceDetails.PatientID = patientInfo.PatientID;
                        patientInsuranceDetails.InsuranceCompanyID = patientInfo.InsuranceCompanyID;
                        patientInsuranceDetails.InsuranceCompanyAddress = patientInfo.InsuranceCompanyAddress;
                        patientInsuranceDetails.InsuranceIDNumber = patientInfo.InsuranceIDNumber;
                        patientInsuranceDetails.InsuranceGroupName = patientInfo.InsuranceGroupName;
                        patientInsuranceDetails.InsurancePlanName = patientInfo.InsurancePlanName;
                        patientInsuranceDetails.InsurancePlanTypeID = patientInfo.InsurancePlanTypeID;
                        patientInsuranceDetails.InsuranceTypeID = patientInfo.InsuranceTypeID;
                        patientInsuranceDetails.InsuranceClaimOfficeNumber = patientInfo.InsuranceClaimOfficeNumber;
                        patientInsuranceDetails.VisitsAllowedPerYear = patientInfo.VisitsAllowedPerYear;
                        patientInsuranceDetails.CardIssueDate = patientInfo.CardIssueDate;
                        patientInsuranceDetails.Notes = patientInfo.Notes;
                        if (!string.IsNullOrEmpty(patientInfo.Base64Front))
                        {
                            patientInsuranceDetails.InsurancePhotoPathFront = patientInfo.InsurancePhotoPathFront;
                            patientInsuranceDetails.InsurancePhotoPathThumbFront = patientInfo.InsurancePhotoPathThumbFront;

                        }
                        if (!string.IsNullOrEmpty(patientInfo.Base64Back))
                        {
                            patientInsuranceDetails.InsurancePhotoPathBack = patientInfo.InsurancePhotoPathBack;
                            patientInsuranceDetails.InsurancePhotoPathThumbBack = patientInfo.InsurancePhotoPathThumbBack;
                        }
                        patientInsuranceDetails.InsurancePersonSameAsPatient = patientInfo.InsurancePersonSameAsPatient;
                        patientInsuranceDetails.IsActive = patientInfo.IsActive;
                        patientInsuranceDetails.UpdatedDate = DateTime.UtcNow;
                        patientInsuranceDetails.UpdatedBy = patientInfo.UpdatedBy;
                        patientInsuranceDetails.IsVerified = patientInfo.IsVerified;
                        _context.SaveChanges();
                    }
                    return patientInsuranceDetails;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PhoneNumbers UpdatePhoneNumbersData(int id, PhoneNumbers patientInfo)
        {
            try
            {
                if (id > 0)
                {
                    var patientInsuranceDetails = _context.PhoneNumbers.Find(id);
                    if (patientInsuranceDetails != null)
                    {
                        patientInsuranceDetails.PatientID = patientInfo.PatientID;
                        patientInsuranceDetails.PhoneNumber = patientInfo.PhoneNumber;
                        patientInsuranceDetails.PhoneNumberTypeId = patientInfo.PhoneNumberTypeId;
                        patientInsuranceDetails.IsDeleted = patientInfo.IsDeleted;
                        patientInsuranceDetails.DeletedBy = patientInfo.DeletedBy;
                        patientInsuranceDetails.IsActive = patientInfo.IsActive;
                        _context.SaveChanges();
                    }
                    return patientInsuranceDetails;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }


        }        
        public PatientVitals UpdatePatientVitalseData(int id, PatientVitals patientInfo)
        {
            try
            {
                if (id > 0)
                {
                    var patientVitals = _context.PatientVitals.Find(id);
                    if (patientVitals != null)
                    {
                        patientVitals.BMI = patientInfo.BMI;
                        //patientVitals.BMI_Status = patientInfo.BMI_Status;
                        patientVitals.BPDiastolic = patientInfo.BPDiastolic;
                        patientVitals.BPSystolic = patientInfo.BPSystolic;
                        //patientVitals.EncounterID = patientInfo.EncounterID;
                        patientVitals.HeartRate = patientInfo.HeartRate;
                        //patientVitals.HeightFt = patientInfo.HeightFt;
                        patientVitals.HeightIn = patientInfo.HeightIn;
                        patientVitals.PatientID = patientInfo.PatientID;
                        patientVitals.Pulse = patientInfo.Pulse;
                        patientVitals.Respiration = patientInfo.Respiration;
                        patientVitals.UpdatedBy = patientInfo.UpdatedBy;
                        patientVitals.UpdatedDate = DateTime.UtcNow;
                        patientVitals.IsActive = patientInfo.IsActive;
                        //patientVitals.Weight_kg = patientInfo.Weight_kg;
                        patientVitals.WeightLbs = patientInfo.WeightLbs;
                        //patientVitals.Height_cm = patientInfo.Height_cm;
                        //patientVitals.FollowUp = patientInfo.FollowUp;
                        //save object
                        _context.SaveChanges();
                    }
                    return patientVitals;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Follow up removed as per DP sir told 24-8-2017
        //public bool UpdatePatientVitalsFollowUpData(Vitals patientInfo)
        //{
        //    foreach (var item in patientInfo.Ids)
        //    {
        //        var vitals = _context.PatientVitals.Find(item);
        //        if (vitals != null)
        //        {
        //            vitals.FollowUp = patientInfo.Followup_Flag;
        //            _context.SaveChanges();
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        public bool UpdatePatientDiagnosisData(int id, PatientDiagnosis patientInfo)
        {
            if (id > 0)
            {
                var patientDiagnosis = _context.PatientDiagnosis.Find(id);
                if (patientDiagnosis != null)
                {
                    patientDiagnosis.ICDID = patientInfo.ICDID;
                    patientDiagnosis.PatientID = patientInfo.PatientID;
                    patientDiagnosis.IsActive = patientInfo.IsActive;
                    patientDiagnosis.UpdatedBy = patientInfo.UpdatedBy;
                    patientDiagnosis.UpdatedDate = DateTime.UtcNow;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool UpdatePatientEncounterData(int id, PatientEncounter patientInfo)
        {
            if (id > 0)
            {
                var patientDiagnosis = _context.PatientEncounter.Find(id);
                if (patientDiagnosis != null)
                {
                    //patientDiagnosis.VisitStartDate = patientInfo.VisitStartDate;
                    //patientDiagnosis.VisitEndDate = patientInfo.VisitEndDate;
                    patientDiagnosis.StaffID = patientInfo.StaffID;
                    patientDiagnosis.PatientID = patientInfo.PatientID;
                    //patientDiagnosis.CPTID = patientInfo.CPTID;
                    //patientDiagnosis.LocationID = patientInfo.LocationID;
                    //patientDiagnosis.Subjective = patientInfo.Subjective;
                    // patientDiagnosis.Objective = patientInfo.Objective;
                    //patientDiagnosis.Assessment = patientInfo.Assessment;
                    // patientDiagnosis.Plans = patientInfo.Plans;
                    patientDiagnosis.IsActive = patientInfo.IsActive;
                    patientDiagnosis.UpdatedDate = DateTime.UtcNow;
                    patientDiagnosis.UpdatedBy = patientInfo.UpdatedBy;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool UpdatePatientMedicalFamilyHistoryData(int id, PatientMedicalFamilyHistory patientInfo)
        {
            if (id > 0)
            {
                var patientMedicalFamilyHistory = _context.PatientMedicalFamilyHistory.Find(id);
                if (patientMedicalFamilyHistory != null)
                {
                    //patientMedicalFamilyHistory.AgeOfDiagnosis = patientInfo.AgeOfDiagnosis;
                    patientMedicalFamilyHistory.CauseOfDeath = patientInfo.CauseOfDeath;
                    patientMedicalFamilyHistory.DOB = patientInfo.DOB;
                    patientMedicalFamilyHistory.DateOfDeath = patientInfo.DateOfDeath;
                    patientMedicalFamilyHistory.DeletedBy = patientInfo.DeletedBy;
                    //patientMedicalFamilyHistory.DiseaseID = patientInfo.DiseaseID;
                    patientMedicalFamilyHistory.FirstName = patientInfo.FirstName;
                    patientMedicalFamilyHistory.GenderID = patientInfo.GenderID;
                    patientMedicalFamilyHistory.IsActive = patientInfo.IsActive;
                    patientMedicalFamilyHistory.IsDeleted = patientInfo.IsDeleted;
                    patientMedicalFamilyHistory.LastName = patientInfo.LastName;
                    patientMedicalFamilyHistory.Observation = patientInfo.Observation;
                    patientMedicalFamilyHistory.PatientID = patientInfo.PatientID;
                    patientMedicalFamilyHistory.RelationshipID = patientInfo.RelationshipID;
                    patientMedicalFamilyHistory.UpdatedDate = DateTime.UtcNow;
                    patientMedicalFamilyHistory.UpdatedBy = patientInfo.UpdatedBy;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public PatientImmunization UpdatePatientImmunizationData(int id, PatientImmunization patientInfo)
        {
            if (id > 0)
            {
                var patientMedicalFamilyHistory = _context.PatientImmunization.Find(id);
                if (patientMedicalFamilyHistory != null)
                {
                    patientMedicalFamilyHistory.AdministeredBy = patientInfo.AdministeredBy;
                    patientMedicalFamilyHistory.AdministeredDate = patientInfo.AdministeredDate;
                    patientMedicalFamilyHistory.AdministrationSiteID = patientInfo.AdministrationSiteID;
                    patientMedicalFamilyHistory.AmountAdministered = patientInfo.AmountAdministered;
                    patientMedicalFamilyHistory.ExpireDate = patientInfo.ExpireDate;
                    patientMedicalFamilyHistory.ImmunityStatusID = patientInfo.ImmunityStatusID;
                    patientMedicalFamilyHistory.Immunization = patientInfo.Immunization;
                    patientMedicalFamilyHistory.IsActive = patientInfo.IsActive;
                    patientMedicalFamilyHistory.ManufactureID = patientInfo.ManufactureID;
                    patientMedicalFamilyHistory.OrderBy = patientInfo.OrderBy;
                    patientMedicalFamilyHistory.PatientID = patientInfo.PatientID;
                    patientMedicalFamilyHistory.RejectedImmunization = patientInfo.RejectedImmunization;
                    patientMedicalFamilyHistory.RejectionReasonID = patientInfo.RejectionReasonID;
                    patientMedicalFamilyHistory.RouteOfAdministrationID = patientInfo.RouteOfAdministrationID;
                    patientMedicalFamilyHistory.VFCID = patientInfo.VFCID;
                    patientMedicalFamilyHistory.VaccineLotNumber = patientInfo.VaccineLotNumber;
                    patientMedicalFamilyHistory.UpdatedDate = DateTime.UtcNow;
                    patientMedicalFamilyHistory.UpdatedBy = patientInfo.UpdatedBy;
                    _context.SaveChanges();
                    return patientMedicalFamilyHistory;
                }
            }
            return null;
        }
        public PatientSocialHistory UpdatePatientSocialHistoryData(int id, PatientSocialHistory patientInfo)
        {
            if (id > 0)
            {
                var patientDiagnosis = _context.PatientSocialHistory.Find(id);
                if (patientDiagnosis != null)
                {
                    patientDiagnosis.AlcohalID = patientInfo.AlcohalID;
                    patientDiagnosis.DrugID = patientInfo.DrugID;
                    patientDiagnosis.Occupation = patientInfo.Occupation;
                    patientDiagnosis.PatientID = patientInfo.PatientID;
                    patientDiagnosis.TobaccoID = patientInfo.TobaccoID;
                    patientDiagnosis.TravelID = patientInfo.TravelID;
                    patientDiagnosis.IsActive = patientInfo.IsActive;
                    patientDiagnosis.UpdatedBy = patientInfo.UpdatedBy;
                    patientDiagnosis.UpdatedDate = DateTime.UtcNow;
                    _context.SaveChanges();
                    return patientDiagnosis;
                }
            }
            return null;
        }

        public List<Authorization> GetAuthorizationByPatientID(int Id)
        {
            try
            {
                return _context.Authorization.Where(m => m.PatientID == Id && m.IsDeleted == false).Include(l => l.AuthorizationProcedures.Where(a => a.IsDeleted == false)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Patients> GetPatientByTag(List<string> tag)
        {
            try
            {
                //var patienLista = _context.Patients.FromSql("GetPatientByTags @tags", "Allergy");

                var patients = from P in _context.Patients
                               join PT in _context.PatientTags on P.Id equals PT.PatientID
                               join MT in _context.MasterTags on PT.TagID equals MT.Id
                               where tag.Contains(MT.Tag)
                               orderby P.FirstName
                               select new Patients { FirstName = P.FirstName, LastName = P.LastName, MiddleName = P.MiddleName, Id = P.Id, MRN = P.MRN, PhotoThumbnailPath = P.PhotoThumbnailPath };
                if (patients != null && patients.Count() > 0)
                {
                    List<Patients> patientList = new List<Patients>(patients.ToList());
                    return patientList;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IQueryable<T> GetFilteredPatients<T>(string searchKey, string operation, string startWith, string tags, string locationIDs, string isActive,int pageSize, TokenModel tokenModel) where T : class, new()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@SeachKey", searchKey),                                          
                                          new SqlParameter("@StartWith",startWith),
                                          new SqlParameter("@Tags",tags),
                                          new SqlParameter("@LocationIDs",locationIDs),
                                          new SqlParameter("@IsActive",isActive),
                                          new SqlParameter("@OrganizationID",tokenModel.OrganizationID),
                                          new SqlParameter("@SortColumn",string.Empty),
                                          new SqlParameter("@SortOrder",string.Empty),
                                          new SqlParameter("@PageNumber",1),
                                          new SqlParameter("@PageSize",pageSize),
                };
                return _context.ExecStoredProcedureListWithOutput<T>("GetFilteredPatients", parameters.Length, parameters).AsQueryable();                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
