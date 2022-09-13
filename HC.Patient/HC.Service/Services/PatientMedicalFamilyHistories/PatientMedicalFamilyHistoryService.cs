using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.PatientMedicalFamilyHistory;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Patient.Repositories.IRepositories.PatientMedicalFamilyHistories;
using HC.Patient.Service.IServices.PatientMedicalFamilyHistories;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.PatientMedicalFamilyHistories
{
    public class PatientMedicalFamilyHistoryService : BaseService, IPatientMedicalFamilyHistoryService
    {
        private HCMasterContext _context;
        private HCOrganizationContext _organizationContext;
        private IPatientMedicalFamilyHistoryRepository _patientMedicalFamilyHistoryRepository;
        private IPatientMedicalFamilyHistoryDiseasesRepository _patientMedicalFamilyHistoryDiseasesRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        string Message = string.Empty;        
        public PatientMedicalFamilyHistoryService(HCMasterContext context, HCOrganizationContext organizationContext, IPatientMedicalFamilyHistoryRepository patientMedicalFamilyHistoryRepository, IPatientMedicalFamilyHistoryDiseasesRepository patientMedicalFamilyHistoryDiseasesRepository,IAuditLogRepository auditLogRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository)
        {
            _context = context;
            _organizationContext = organizationContext;
            _patientMedicalFamilyHistoryRepository = patientMedicalFamilyHistoryRepository;
            _patientMedicalFamilyHistoryDiseasesRepository = patientMedicalFamilyHistoryDiseasesRepository;
            _auditLogRepository = auditLogRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
        }

        /// <summary>
        /// save patient medical family history
        /// </summary>
        /// <param name="organizationModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel SavePatientMedicalFamilyHistory(PatientMedicalFamilyHistoryModel patientMedicalFamilyHistory, TokenModel token)
        {
            PatientMedicalFamilyHistory requestPatientMedicalFamilyHistory = null;
            List<PatientMedicalFamilyHistoryDiseases> requestPatientMedicalFamilyHistoryDiseases = null;
            //new
            List<PatientMedicalFamilyHistoryDiseases> newPatientMedicalFamilyHistoryDiseases = new List<PatientMedicalFamilyHistoryDiseases>();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();
                    //already check
                    PatientMedicalFamilyHistory getPatMedFamHis = _patientMedicalFamilyHistoryRepository.Get(x => x.PatientID == patientMedicalFamilyHistory.PatientID && x.FirstName == patientMedicalFamilyHistory.FirstName && x.LastName == patientMedicalFamilyHistory.LastName && x.IsActive == true && x.IsDeleted == false);
                    if (getPatMedFamHis == null && !ReferenceEquals(patientMedicalFamilyHistory, null) && patientMedicalFamilyHistory.Id == 0)//New Insert
                    {
                        requestPatientMedicalFamilyHistory = new PatientMedicalFamilyHistory();
                        requestPatientMedicalFamilyHistoryDiseases = new List<PatientMedicalFamilyHistoryDiseases>();
                        AutoMapper.Mapper.Map(patientMedicalFamilyHistory, requestPatientMedicalFamilyHistory);
                        AutoMapper.Mapper.Map(patientMedicalFamilyHistory.PatientMedicalFamilyHistoryDiseases, requestPatientMedicalFamilyHistoryDiseases);
                        //save patient medical family history
                        requestPatientMedicalFamilyHistory.CreatedBy = token.UserID;
                        requestPatientMedicalFamilyHistory.CreatedDate = DateTime.UtcNow;
                        requestPatientMedicalFamilyHistory.IsActive = true;
                        requestPatientMedicalFamilyHistory.IsDeleted = false;
                        _patientMedicalFamilyHistoryRepository.Create(requestPatientMedicalFamilyHistory);
                        changesLogs = _patientMedicalFamilyHistoryRepository.GetChangesLogData(token);
                        _patientMedicalFamilyHistoryRepository.SaveChanges();
                        //save patient medical family history disesase
                        if (requestPatientMedicalFamilyHistory.Id > 0)
                        {
                            requestPatientMedicalFamilyHistoryDiseases.ForEach(x => { x.MedicalFamilyHistoryId = requestPatientMedicalFamilyHistory.Id; x.CreatedBy = token.UserID; x.CreatedDate = DateTime.UtcNow; x.IsActive = true; x.IsDeleted = false; });
                            _patientMedicalFamilyHistoryDiseasesRepository.Create(requestPatientMedicalFamilyHistoryDiseases.ToArray());
                            _patientMedicalFamilyHistoryDiseasesRepository.SaveChanges();
                        }
                        //Message
                        Message = StatusMessage.APISavedSuccessfully.Replace("[controller]", "Client medical family history");
                        _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, requestPatientMedicalFamilyHistory.Id, patientMedicalFamilyHistory.LinkedEncounterId, token);
                        //transaction commit
                        transaction.Commit();
                    }
                    else if (patientMedicalFamilyHistory.Id > 0)//Update
                    {
                        requestPatientMedicalFamilyHistory = _patientMedicalFamilyHistoryRepository.Get(x => x.Id == patientMedicalFamilyHistory.Id && x.IsActive == true && x.IsDeleted == false);
                        if (requestPatientMedicalFamilyHistory != null)
                        {

                            requestPatientMedicalFamilyHistory.CauseOfDeath = patientMedicalFamilyHistory.CauseOfDeath;
                            requestPatientMedicalFamilyHistory.DateOfDeath = patientMedicalFamilyHistory.DateOfDeath;
                            requestPatientMedicalFamilyHistory.DOB = patientMedicalFamilyHistory.DOB;
                            requestPatientMedicalFamilyHistory.FirstName = patientMedicalFamilyHistory.FirstName;
                            requestPatientMedicalFamilyHistory.GenderID = patientMedicalFamilyHistory.GenderID;
                            requestPatientMedicalFamilyHistory.LastName = patientMedicalFamilyHistory.LastName;
                            requestPatientMedicalFamilyHistory.Observation = patientMedicalFamilyHistory.Observation;
                            requestPatientMedicalFamilyHistory.PatientID = patientMedicalFamilyHistory.PatientID;
                            requestPatientMedicalFamilyHistory.RelationshipID = patientMedicalFamilyHistory.RelationshipID;
                            requestPatientMedicalFamilyHistory.Others = patientMedicalFamilyHistory.Others;
                            requestPatientMedicalFamilyHistory.UpdatedBy = token.UserID;
                            requestPatientMedicalFamilyHistory.UpdatedDate = DateTime.UtcNow;
                            changesLogs = _patientMedicalFamilyHistoryRepository.GetChangesLogData(token);
                            _patientMedicalFamilyHistoryRepository.Update(requestPatientMedicalFamilyHistory);
                            _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateFamilyHistoryDetails, AuditLogAction.Modify, null, token.UserID, "" + null, token);
                            _patientMedicalFamilyHistoryRepository.SaveChanges();


                            requestPatientMedicalFamilyHistoryDiseases = new List<PatientMedicalFamilyHistoryDiseases>();
                            foreach (var item in patientMedicalFamilyHistory.PatientMedicalFamilyHistoryDiseases)
                            {
                                PatientMedicalFamilyHistoryDiseases updatePatMedFamHis = new PatientMedicalFamilyHistoryDiseases();

                                if (item.Id > 0)//edit case
                                {
                                    updatePatMedFamHis = _patientMedicalFamilyHistoryDiseasesRepository.Get(a => a.Id == item.Id && a.IsActive == true && a.IsDeleted == false);
                                    if (updatePatMedFamHis != null)
                                    {
                                        updatePatMedFamHis.UpdatedBy = token.UserID;
                                        updatePatMedFamHis.UpdatedDate = DateTime.UtcNow;
                                        updatePatMedFamHis.IsActive = item.IsActive;
                                        updatePatMedFamHis.IsDeleted = item.IsDeleted;
                                        updatePatMedFamHis.AgeOfDiagnosis = item.AgeOfDiagnosis;
                                        updatePatMedFamHis.DiseaseID = item.DiseaseID;
                                        updatePatMedFamHis.DiseaseStatus = item.DiseaseStatus;

                                        updatePatMedFamHis.MedicalFamilyHistoryId = item.MedicalFamilyHistoryId;
                                        updatePatMedFamHis.PatientID = item.PatientID;

                                    }
                                    requestPatientMedicalFamilyHistoryDiseases.Add(updatePatMedFamHis);//add new item into list
                                }
                                else//new insert
                                {
                                    PatientMedicalFamilyHistoryDiseases newPatMedFamHis = new PatientMedicalFamilyHistoryDiseases();
                                    newPatMedFamHis.CreatedBy = token.UserID;
                                    newPatMedFamHis.CreatedDate = DateTime.UtcNow;
                                    newPatMedFamHis.IsActive = true;
                                    newPatMedFamHis.IsDeleted = false;
                                    newPatMedFamHis.AgeOfDiagnosis = item.AgeOfDiagnosis;
                                    newPatMedFamHis.DiseaseID = item.DiseaseID;
                                    newPatMedFamHis.DiseaseStatus = item.DiseaseStatus;
                                    newPatMedFamHis.MedicalFamilyHistoryId = item.MedicalFamilyHistoryId;
                                    newPatMedFamHis.PatientID = item.PatientID;
                                    newPatientMedicalFamilyHistoryDiseases.Add(newPatMedFamHis);
                                }
                            }
                            _patientMedicalFamilyHistoryDiseasesRepository.Update(requestPatientMedicalFamilyHistoryDiseases.ToArray());
                            _patientMedicalFamilyHistoryDiseasesRepository.Create(newPatientMedicalFamilyHistoryDiseases.ToArray());
                            _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdateFamilyDeseaseHistoryDetails, AuditLogAction.Modify, null, token.UserID, "" + null, token);
                            //changesLogs = _patientMedicalFamilyHistoryDiseasesRepository.GetChangesLogData(token);
                            _patientMedicalFamilyHistoryDiseasesRepository.SaveChanges();
                        }
                        Message = StatusMessage.APIUpdatedSuccessfully.Replace("[controller]", "Client medical family history");
                        _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, requestPatientMedicalFamilyHistory.Id, patientMedicalFamilyHistory.LinkedEncounterId, token);
                        //transaction commit
                        transaction.Commit();
                    }
                    else
                    {
                        Message = "You already created this history";
                    }
                        return new JsonModel()
                        {
                            data = patientMedicalFamilyHistory,             //requestPatientMedicalFamilyHistory,
                            Message = Message,
                            StatusCode = (int)HttpStatusCodes.OK//Success
                        };
                    }
                
                catch (Exception ex)
                {
                    //on error transaction rollback
                    transaction.Rollback();
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = ex.Message,
                        StatusCode = (int)HttpStatusCodes.UnprocessedEntity//UnprocessedEntity
                    };
                }
            }





        }

        /// <summary>
        /// to get patient family medical history by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonModel GetPatientMedicalFamilyHistoryById(int Id = 0, int patientID = 0)
        {
            PatientMedicalFamilyHistoryModel responsePatientMedicalFamilyHistoryObj = new PatientMedicalFamilyHistoryModel();
            List<PatientMedicalFamilyHistoryModel> responsePatientMedicalFamilyHistoryObjList = new List<PatientMedicalFamilyHistoryModel>();
            responsePatientMedicalFamilyHistoryObj.PatientMedicalFamilyHistoryDiseases = new List<PatientMedicalFamilyHistoryDiseasesModel>();

            if (Id > 0)
            {
                PatientMedicalFamilyHistory patMedFamHisObj = _patientMedicalFamilyHistoryRepository.Get(a => a.Id == Id && a.IsActive == true && a.IsDeleted == false);
                patMedFamHisObj.RelationshipName = _organizationContext.MasterRelationship.Where(z => z.Id == patMedFamHisObj.RelationshipID).FirstOrDefault().RelationshipName;
                patMedFamHisObj.Gender = _organizationContext.MasterGender.Where(z => z.Id == patMedFamHisObj.GenderID).FirstOrDefault().Gender;
                patMedFamHisObj.PatientName = _organizationContext.Patients.Where(z => z.Id == patMedFamHisObj.PatientID).FirstOrDefault().FirstName + " " + _organizationContext.Patients.Where(z => z.Id == patMedFamHisObj.PatientID).FirstOrDefault().LastName;
                if (patMedFamHisObj != null)
                {
                    List<PatientMedicalFamilyHistoryDiseases> patMedFamHisDBObj = _patientMedicalFamilyHistoryDiseasesRepository.GetAll(a => a.MedicalFamilyHistoryId == Id && a.IsActive == true && a.IsDeleted == false).ToList();
                    AutoMapper.Mapper.Map(patMedFamHisObj, responsePatientMedicalFamilyHistoryObj);
                    AutoMapper.Mapper.Map(patMedFamHisDBObj, responsePatientMedicalFamilyHistoryObj.PatientMedicalFamilyHistoryDiseases);
                    return new JsonModel()
                    {
                        data = responsePatientMedicalFamilyHistoryObj,
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
            else if (patientID > 0)
            {
                List<PatientMedicalFamilyHistory> patMedFamHisObj = _patientMedicalFamilyHistoryRepository.GetAll(a => a.PatientID == patientID && a.IsActive == true && a.IsDeleted == false).ToList();
                if (patMedFamHisObj != null)
                {
                    patMedFamHisObj.ForEach(a => a.RelationshipName = _organizationContext.MasterRelationship.Where(z => z.Id == a.RelationshipID).FirstOrDefault().RelationshipName);
                    patMedFamHisObj.ForEach(a => a.Gender = _organizationContext.MasterGender.Where(z => z.Id == a.GenderID).FirstOrDefault().Gender);
                    patMedFamHisObj.ForEach(a => a.PatientName = _organizationContext.Patients.Where(z => z.Id == a.PatientID).FirstOrDefault().FirstName + " " + _organizationContext.Patients.Where(z => z.Id == a.PatientID).FirstOrDefault().LastName);

                    List<PatientMedicalFamilyHistoryDiseases> patMedFamHisDBObj = _patientMedicalFamilyHistoryDiseasesRepository.GetAll(a => a.PatientID == patientID && a.IsActive == true && a.IsDeleted == false).ToList();
                    AutoMapper.Mapper.Map(patMedFamHisObj, responsePatientMedicalFamilyHistoryObjList);
                    foreach (var item in responsePatientMedicalFamilyHistoryObjList)
                    {
                        AutoMapper.Mapper.Map(patMedFamHisDBObj.Where(a=>a.MedicalFamilyHistoryId==item.Id).ToList(), item.PatientMedicalFamilyHistoryDiseases);
                    }
                    return new JsonModel()
                    {
                        data = responsePatientMedicalFamilyHistoryObjList,
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

        /// <summary>
        /// get all family history
        /// </summary>
        /// <returns></returns>
        public JsonModel GetPatientMedicalFamilyHistory(string firstName = "", string lastName = "", string Disease = "", string sortColumn = "", int page = 0, int pageSize = 10)
        {
            //response model
            List<PatientMedicalFamilyHistoryModel> response = new List<PatientMedicalFamilyHistoryModel>();
            //get all records from db
            List<PatientMedicalFamilyHistory> patMedFamHisDBList = _patientMedicalFamilyHistoryRepository.GetAll(a => a.IsActive == true && a.IsDeleted == false).ToList();
            patMedFamHisDBList.ForEach(a => a.RelationshipName = _organizationContext.MasterRelationship.Where(z => z.Id == a.RelationshipID).FirstOrDefault().RelationshipName);
            patMedFamHisDBList.ForEach(a => a.Gender = _organizationContext.MasterGender.Where(z => z.Id == a.GenderID).FirstOrDefault().Gender);
            patMedFamHisDBList.ForEach(a => a.PatientName = _organizationContext.Patients.Where(z => z.Id == a.PatientID).FirstOrDefault().FirstName +" "+ _organizationContext.Patients.Where(z => z.Id == a.PatientID).FirstOrDefault().LastName);
            int Totalrecords = patMedFamHisDBList.Count();

            List<PatientMedicalFamilyHistoryDiseases> patmedFamHisDisDBList = _patientMedicalFamilyHistoryDiseasesRepository.GetAll(a => a.IsActive == true && a.IsDeleted == false).ToList();



            if (patMedFamHisDBList != null && patMedFamHisDBList.Count() > 0)
            {
                AutoMapper.Mapper.Map(patMedFamHisDBList, response);

                foreach (var item in response)
                {
                    AutoMapper.Mapper.Map(patmedFamHisDisDBList.Where(a => a.MedicalFamilyHistoryId == item.Id).ToList(), item.PatientMedicalFamilyHistoryDiseases);
                }               

                if (!string.IsNullOrEmpty(firstName))
                {
                    response = response.Where(a => a.FirstName.ToUpper().Contains(firstName.ToUpper())).ToList();
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    response = response.Where(a => a.LastName.ToUpper().Contains(lastName.ToUpper())).ToList();
                }

                //Sorting
                switch (sortColumn)
                {
                    case "-firstname":
                        response = response.OrderByDescending(s => s.FirstName).ToList();
                        break;
                    case "firstname":
                        response = response.OrderBy(s => s.FirstName).ToList(); ;
                        break;
                    case "-lastname":
                        response = response.OrderByDescending(s => s.LastName).ToList();
                        break;
                    case "lastname":
                        response = response.OrderBy(s => s.LastName).ToList(); ;
                        break;
                    //case "-email":
                    //    response = response.OrderByDescending(s => s).ToList();
                    //    break;
                    //case "email":
                    //    response = response.OrderBy(s => s.Email).ToList(); ;
                    //    break;
                    //case "-state":
                    //    response = response.OrderByDescending(s => s.StateID).ToList();
                    //    break;
                    //case "state":
                    //    response = response.OrderBy(s => s.StateID).ToList(); ;
                    //    break;
                    default:
                        response = response.OrderBy(s => s.Id).ToList();
                        break;
                }
                //pagination
                response = response.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                return new JsonModel()
                {
                    data = response,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK,//Success
                    meta = new Meta()
                    {
                        TotalRecords = response != null && response.Count > 0 ? Totalrecords : 0,
                        CurrentPage = page,
                        PageSize = pageSize,
                        DefaultPageSize = pageSize,
                        TotalPages = Math.Ceiling(Convert.ToDecimal((response != null && response.Count > 0 ? Totalrecords : 0) / pageSize))
                    }
                };
            }

            return new JsonModel()
            {
                data = new object(),
                Message = StatusMessage.NotFound,
                StatusCode = (int)HttpStatusCodes.NotFound//Success
            };
        }

        /// <summary>
        /// delete patient medical family history by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel DeletePatientMedicalFamilyHistory(int Id, TokenModel token)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    PatientMedicalFamilyHistory patMedObj = _patientMedicalFamilyHistoryRepository.Get(a => a.Id == Id && a.IsActive == true && a.IsDeleted == false);
                    List<PatientMedicalFamilyHistoryDiseases> patMedDisObj = _patientMedicalFamilyHistoryDiseasesRepository.GetAll(a => a.IsActive == true && a.IsDeleted == false).ToList();


                    if (patMedObj != null)
                    {
                        //set IsDeleted = 1(true)
                        if (patMedDisObj.Count() > 0) { patMedDisObj.ForEach(a => { a.IsDeleted = false; a.DeletedBy = token.UserID; a.DeletedDate = DateTime.UtcNow; }); }

                        patMedObj.IsDeleted = true;
                        patMedObj.DeletedDate = DateTime.UtcNow;
                        patMedObj.DeletedBy = token.UserID;
                        //update
                        _patientMedicalFamilyHistoryRepository.Update(patMedObj);
                        _patientMedicalFamilyHistoryDiseasesRepository.Update(patMedDisObj.ToArray());
                        _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeleteFamilyHistoryDetails, AuditLogAction.Delete, null, token.UserID, "" + null, token);
                        //save
                        _patientMedicalFamilyHistoryRepository.SaveChanges();
                        _patientMedicalFamilyHistoryDiseasesRepository.SaveChanges();

                        //transaction commited
                        transaction.Commit();
                        //message
                        Message = StatusMessage.DeletedSuccessfully.Replace("[controller]", "Client medical family history");
                    }
                    else
                    {
                        Message = StatusMessage.NotFound;
                    }
                    return new JsonModel()
                    {
                        data = new object(),
                        Message = Message,
                        StatusCode = (int)HttpStatusCodes.NoContent//Success
                    };
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
    }
}
