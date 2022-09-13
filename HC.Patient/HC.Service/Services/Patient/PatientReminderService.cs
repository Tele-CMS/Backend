using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Model.Tasks;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices;
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
    public class PatientReminderService : BaseService, IPatientReminderService
    {
        private readonly IPatientReminderRepository _patientReminderRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientAlertRepository _patientAlertRepository;

        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        public PatientReminderService(IPatientReminderRepository patientReminderRepository, IPatientRepository patientRepository, IPatientAlertRepository patientAlertRepository)
        {
            this._patientReminderRepository = patientReminderRepository;
            _patientRepository = patientRepository;
            _patientAlertRepository = patientAlertRepository;
        }

        /// <summary>
        /// Reminder for members page
        /// </summary>
        /// <param name="patientReminderModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        //public JsonModel SaveReminder(PatientReminderModel patientReminderModel, TokenModel tokenModel)
        //{
        //    //Filters
        //    ListingFiltterModel patientFiltterModel = new ListingFiltterModel();
        //    patientFiltterModel.LocationIDs = patientReminderModel.LocationIDs;
        //    patientFiltterModel.SearchKey = patientReminderModel.SearchText;
        //    patientFiltterModel.EligibilityStatus = patientReminderModel.EligibilityStatus;
        //    patientFiltterModel.DOB = patientReminderModel.DOB;
        //    patientFiltterModel.MedicalID = patientReminderModel.MedicalID;
        //    patientFiltterModel.StartAge = patientReminderModel.StartAge;
        //    patientFiltterModel.EndAge = patientReminderModel.EndAge;
        //    patientFiltterModel.CareManagerIds = patientReminderModel.CareManagerIds;
        //    patientFiltterModel.ProgramIds = patientReminderModel.ProgramIds;
        //    patientFiltterModel.GenderIds = patientReminderModel.GenderIds;
        //    patientFiltterModel.RelationshipIds = patientReminderModel.RelationshipIds;
        //    patientFiltterModel.PrimaryConditionId = patientReminderModel.PrimaryConditionId;
        //    patientFiltterModel.ComorbidConditionIds = patientReminderModel.ComorbidConditionIds;
        //    patientFiltterModel.RiskIds = patientReminderModel.RiskIds;
        //    patientFiltterModel.pageSize = int.MaxValue;
        //    //List<PatientModel> patientModels = _patientRepository.GetPatients<PatientModel>(patientFiltterModel, tokenModel).ToList();
        //    List<int> patientIds = _patientRepository.GetPatients<PatientModel>(patientFiltterModel, tokenModel).Select(x => x.PatientId).Distinct().ToList();

        //    PatientReminder patientReminder = null;
        //    using (var transaction = _patientReminderRepository.StartTransaction())
        //    {
        //        try
        //        {
        //            if (patientReminderModel.Id == 0 || patientReminderModel == null)
        //            {
        //                patientReminder = new PatientReminder();
        //                //AutoMapper.Mapper.Map(patientReminderModel, patientReminder);
        //                patientReminder.Id = 0;
        //                patientReminder.Title = patientReminderModel.Title;
        //                patientReminder.StartTime = patientReminderModel.StartDate;
        //                patientReminder.EndTime = patientReminderModel.EndDate;
        //                patientReminder.MasterReminderFrequencyTypeId = patientReminderModel.MasterReminderFrequencyTypeID;
        //                patientReminder.EnrollmentId = patientReminderModel.EnrollmentId;
        //                patientReminder.MasterReminderFrequencyTypeId = patientReminderModel.MasterReminderFrequencyTypeID;
        //                patientReminder.IsSendReminderToCareManager = patientReminderModel.IsSendReminderToCareManager;
        //                patientReminder.CareManagerMessage = patientReminderModel.CareManagerMessage;
        //                patientReminder.Message = patientReminderModel.Message;
        //                patientReminder.Notes = patientReminderModel.Notes;
        //                patientReminder.IsActive = true;

        //                patientReminder.OrganizationId = tokenModel.OrganizationID;
        //                patientReminder.CreatedBy = tokenModel.UserID;
        //                patientReminder.IsDeleted = false;
        //                _patientReminderRepository.Create(patientReminder);
        //                List<PatientReminderAndPatientIdMapping> patientsReminderObjectList = new List<PatientReminderAndPatientIdMapping>();
        //                //Check if filter results are empty
        //                if (patientIds != null && patientIds.Count() > 0)
        //                {
        //                    //Loop through each patientIds
        //                    for (int i = 0; i < patientIds.Count(); i++)
        //                    {
        //                        PatientReminderAndPatientIdMapping patientsObj = new PatientReminderAndPatientIdMapping();
        //                        patientsObj.IsActive = true;
        //                        patientsObj.IsDeleted = false;
        //                        patientsObj.OrganizationId = tokenModel.OrganizationID;
        //                        patientsObj.PatientReminderId = patientReminder.Id;
        //                        patientsObj.PatientId = patientIds[i];
        //                        patientsReminderObjectList.Add(patientsObj);
        //                    }
        //                    _patientReminderRepository.SavePatientReminderAndPatientIdMapping(patientsReminderObjectList, false);
        //                }

        //                List<PatientReminderAndMessageTypeMapping> patientMessageTpyeObjectList = new List<PatientReminderAndMessageTypeMapping>();
        //                if (patientReminderModel.MasterReminderMessageTypeIDs != null && patientReminderModel.MasterReminderMessageTypeIDs.Count() > 0)
        //                {
        //                    string[] masterReminderMessageTypeIDArray = patientReminderModel.MasterReminderMessageTypeIDs.Split(',');
        //                    for (int i = 0; i < masterReminderMessageTypeIDArray.Count(); i++)
        //                    {
        //                        PatientReminderAndMessageTypeMapping patientsObj = new PatientReminderAndMessageTypeMapping();
        //                        patientsObj.IsActive = true;
        //                        patientsObj.IsDeleted = false;
        //                        patientsObj.MasterMessageTypeID = Convert.ToInt32(masterReminderMessageTypeIDArray[i]);
        //                        patientsObj.PatientReminderId = patientReminder.Id;
        //                        patientMessageTpyeObjectList.Add(patientsObj);
        //                    }
        //                    _patientReminderRepository.SavePatientReminderAndMessageTypeMapping(patientMessageTpyeObjectList, false);
        //                }
        //                response = new JsonModel(patientReminder, StatusMessage.ReminderAdded, (int)HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //            }
        //            _patientReminderRepository.SaveChanges();
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw ex;
        //        }
        //    }
        //    return response;
        //}

        /// <summary>
        /// Reminder for tasks Pages
        /// </summary>
        /// <param name="patientReminderModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        //public JsonModel SaveReminder(TaskReminderModel patientReminderModel, TokenModel tokenModel)
        //{
        //    //Filters
        //    TasksFilterModel patientFiltterModel = new TasksFilterModel();
        //    patientFiltterModel.IsCompleted = patientReminderModel.IsCompleted;
        //    patientFiltterModel.IsMemberCompleted = patientReminderModel.IsMemberCompleted;
        //    patientFiltterModel.StartDate = patientReminderModel.FilterStartDate;
        //    patientFiltterModel.EndDate = patientReminderModel.FilterEndDate;
        //    patientFiltterModel.TimeIntervalFilterId = patientReminderModel.TimeIntervalFilterId;
        //    patientFiltterModel.CareManagerIds = patientReminderModel.CareManagerIds;
        //    patientFiltterModel.AllTasks = patientReminderModel.AllTasks;
        //    patientFiltterModel.ApptStartDate = patientReminderModel.ApptStartDate;
        //    patientFiltterModel.ApptEndDate = patientReminderModel.ApptEndDate;
        //    patientFiltterModel.CurrentDateTime = patientReminderModel.CurrentDateTime;
        //    patientFiltterModel.pageSize = int.MaxValue;

        //    List<int?> tasksPatientIds = _tasksRepository.GetTasksList<PatientCMTasksModel>(patientFiltterModel, 0, tokenModel).Select(x => x.AssignedPatientId).Distinct().ToList();

        //    PatientReminder patientReminder = null;
        //    using (var transaction = _patientReminderRepository.StartTransaction())
        //    {
        //        try
        //        {
        //            if (patientReminderModel.Id == 0 || patientReminderModel == null)
        //            {
        //                patientReminder = new PatientReminder();
        //                //AutoMapper.Mapper.Map(patientReminderModel, patientReminder);
        //                patientReminder.Id = 0;
        //                patientReminder.Title = patientReminderModel.Title;
        //                patientReminder.StartTime = patientReminderModel.StartDate;
        //                patientReminder.EndTime = patientReminderModel.EndDate;
        //                patientReminder.MasterReminderFrequencyTypeId = patientReminderModel.MasterReminderFrequencyTypeID;
        //                patientReminder.EnrollmentId = patientReminderModel.EnrollmentId;
        //                patientReminder.MasterReminderFrequencyTypeId = patientReminderModel.MasterReminderFrequencyTypeID;
        //                patientReminder.IsSendReminderToCareManager = patientReminderModel.IsSendReminderToCareManager;
        //                patientReminder.CareManagerMessage = patientReminderModel.CareManagerMessage;
        //                patientReminder.Message = patientReminderModel.Message;
        //                patientReminder.Notes = patientReminderModel.Notes;
        //                patientReminder.IsActive = true;

        //                patientReminder.OrganizationId = tokenModel.OrganizationID;
        //                patientReminder.CreatedBy = tokenModel.UserID;
        //                patientReminder.IsDeleted = false;
        //                _patientReminderRepository.Create(patientReminder);
        //                List<PatientReminderAndPatientIdMapping> patientsReminderObjectList = new List<PatientReminderAndPatientIdMapping>();
        //                //Check if filter results are empty
        //                if (tasksPatientIds != null && tasksPatientIds.Count() > 0)
        //                {
        //                    //Loop through each patientIds
        //                    for (int i = 0; i < tasksPatientIds.Count(); i++)
        //                    {
        //                        PatientReminderAndPatientIdMapping patientsObj = new PatientReminderAndPatientIdMapping();
        //                        patientsObj.IsActive = true;
        //                        patientsObj.IsDeleted = false;
        //                        patientsObj.OrganizationId = tokenModel.OrganizationID;
        //                        patientsObj.PatientReminderId = patientReminder.Id;
        //                        patientsObj.PatientId = tasksPatientIds[i] == null ? 0 : tasksPatientIds[i].Value;
        //                        patientsReminderObjectList.Add(patientsObj);
        //                    }
        //                    _patientReminderRepository.SavePatientReminderAndPatientIdMapping(patientsReminderObjectList, false);
        //                }

        //                List<PatientReminderAndMessageTypeMapping> patientMessageTpyeObjectList = new List<PatientReminderAndMessageTypeMapping>();
        //                if (patientReminderModel.MasterReminderMessageTypeIDs != null && patientReminderModel.MasterReminderMessageTypeIDs.Count() > 0)
        //                {
        //                    string[] masterReminderMessageTypeIDArray = patientReminderModel.MasterReminderMessageTypeIDs.Split(',');
        //                    for (int i = 0; i < masterReminderMessageTypeIDArray.Count(); i++)
        //                    {
        //                        PatientReminderAndMessageTypeMapping patientsObj = new PatientReminderAndMessageTypeMapping();
        //                        patientsObj.IsActive = true;
        //                        patientsObj.IsDeleted = false;
        //                        patientsObj.MasterMessageTypeID = Convert.ToInt32(masterReminderMessageTypeIDArray[i]);
        //                        patientsObj.PatientReminderId = patientReminder.Id;
        //                        patientMessageTpyeObjectList.Add(patientsObj);
        //                    }
        //                    _patientReminderRepository.SavePatientReminderAndMessageTypeMapping(patientMessageTpyeObjectList, false);
        //                }
        //                response = new JsonModel(patientReminder, StatusMessage.ReminderAdded, (int)HttpStatusCode.OK);
        //            }
        //            else
        //            {
        //            }
        //            _patientReminderRepository.SaveChanges();
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw ex;
        //        }
        //    }
        //    return response;
        //}

        /// <summary>
        /// Reminder for tasks Pages
        /// </summary>
        /// <param name="patientReminderModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public JsonModel SaveReminder(AlertReminderModel patientReminderModel, TokenModel tokenModel)
        {
            //Filters
            PatientAlertFilterModel patientFiltterModel = new PatientAlertFilterModel();
            patientFiltterModel.LocationIDs = patientReminderModel.LocationIDs;
            patientFiltterModel.AlertTypeIds = patientReminderModel.AlertTypeIds;
            patientFiltterModel.CareManagerIds = patientReminderModel.CareManagerIds;
            patientFiltterModel.ComorbidConditionIds = patientReminderModel.ComorbidConditionIds;
            patientFiltterModel.EndDate = patientReminderModel.FilterEndDate;
            patientFiltterModel.EnrollmentId = patientReminderModel.EnrollmentId;
            patientFiltterModel.GenderIds = patientReminderModel.GenderIds;
            patientFiltterModel.PrimaryConditionId = patientReminderModel.PrimaryConditionId;
            patientFiltterModel.ProgramIds = patientReminderModel.ProgramIds;
            patientFiltterModel.RelationshipIds = patientReminderModel.RelationshipIds;
            patientFiltterModel.StartDate = patientReminderModel.FilterStartDate;
            patientFiltterModel.DOB = patientReminderModel.Dob;
            patientFiltterModel.EligibilityStatus = patientReminderModel.EligibilityStatus;
            patientFiltterModel.EndAge = patientReminderModel.EndAge;
            patientFiltterModel.MedicalID = patientReminderModel.MedicalID;
            patientFiltterModel.RiskIds = patientReminderModel.riskIds;
            patientFiltterModel.SearchText = patientReminderModel.SearchText;
            patientFiltterModel.StartAge = patientReminderModel.StartAge;
            patientFiltterModel.pageSize = int.MaxValue;

            List<int?> tasksPatientIds = _patientAlertRepository.GetAllPatientAlertsUsers<PatientCMTasksModel>(patientFiltterModel, tokenModel).Select(x => x.PatientId).Distinct().ToList();

            PatientReminder patientReminder = null;
            using (var transaction = _patientReminderRepository.StartTransaction())
            {
                try
                {
                    if (patientReminderModel.Id == 0 || patientReminderModel == null)
                    {
                        patientReminder = new PatientReminder();
                        //AutoMapper.Mapper.Map(patientReminderModel, patientReminder);
                        patientReminder.Id = 0;
                        patientReminder.Title = patientReminderModel.Title;
                        patientReminder.StartTime = patientReminderModel.StartDate;
                        patientReminder.EndTime = patientReminderModel.EndDate;
                        patientReminder.MasterReminderFrequencyTypeId = patientReminderModel.MasterReminderFrequencyTypeID;
                        patientReminder.EnrollmentId = patientReminderModel.EnrollmentId;
                        patientReminder.MasterReminderFrequencyTypeId = patientReminderModel.MasterReminderFrequencyTypeID;
                        patientReminder.IsSendReminderToCareManager = patientReminderModel.IsSendReminderToCareManager;
                        patientReminder.CareManagerMessage = patientReminderModel.CareManagerMessage;
                        patientReminder.Message = patientReminderModel.Message;
                        patientReminder.Notes = patientReminderModel.Notes;
                        patientReminder.IsActive = true;

                        patientReminder.OrganizationId = tokenModel.OrganizationID;
                        patientReminder.CreatedBy = tokenModel.UserID;
                        patientReminder.IsDeleted = false;
                        _patientReminderRepository.Create(patientReminder);
                        List<PatientReminderAndPatientIdMapping> patientsReminderObjectList = new List<PatientReminderAndPatientIdMapping>();
                        //Check if filter results are empty
                        if (tasksPatientIds != null && tasksPatientIds.Count() > 0)
                        {
                            //Loop through each patientIds
                            for (int i = 0; i < tasksPatientIds.Count(); i++)
                            {
                                PatientReminderAndPatientIdMapping patientsObj = new PatientReminderAndPatientIdMapping();
                                patientsObj.IsActive = true;
                                patientsObj.IsDeleted = false;
                                patientsObj.OrganizationId = tokenModel.OrganizationID;
                                patientsObj.PatientReminderId = patientReminder.Id;
                                patientsObj.PatientId = tasksPatientIds[i] == null ? 0 : tasksPatientIds[i].Value;
                                patientsReminderObjectList.Add(patientsObj);
                            }
                            _patientReminderRepository.SavePatientReminderAndPatientIdMapping(patientsReminderObjectList, false);
                        }

                        List<PatientReminderAndMessageTypeMapping> patientMessageTpyeObjectList = new List<PatientReminderAndMessageTypeMapping>();
                        if (patientReminderModel.MasterReminderMessageTypeIDs != null && patientReminderModel.MasterReminderMessageTypeIDs.Count() > 0)
                        {
                            string[] masterReminderMessageTypeIDArray = patientReminderModel.MasterReminderMessageTypeIDs.Split(',');
                            for (int i = 0; i < masterReminderMessageTypeIDArray.Count(); i++)
                            {
                                PatientReminderAndMessageTypeMapping patientsObj = new PatientReminderAndMessageTypeMapping();
                                patientsObj.IsActive = true;
                                patientsObj.IsDeleted = false;
                                patientsObj.MasterMessageTypeID = Convert.ToInt32(masterReminderMessageTypeIDArray[i]);
                                patientsObj.PatientReminderId = patientReminder.Id;
                                patientMessageTpyeObjectList.Add(patientsObj);
                            }
                            _patientReminderRepository.SavePatientReminderAndMessageTypeMapping(patientMessageTpyeObjectList, false);
                        }
                        response = new JsonModel(patientReminder, StatusMessage.ReminderAdded, (int)HttpStatusCode.OK);
                    }
                    else
                    {
                    }
                    _patientReminderRepository.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return response;
        }
    }
}
