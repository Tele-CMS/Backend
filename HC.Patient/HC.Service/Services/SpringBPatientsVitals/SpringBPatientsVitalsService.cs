using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Common;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
using HC.Patient.Model.SpringBPatient;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
//using HC.Patient.Repositories.IRepositories.PatientsLab;
using HC.Patient.Repositories.IRepositories.SpringBPatientsVitals;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.SpringBPatientsVitals;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.Services.SpringBPatientsVitals
{
    public class SpringBPatientsVitalsService : BaseService, ISpringBPatientsVitalsService
    {
        private readonly ISpringBPatientsVitalsRepository _springBPatientsVitalsRepository;
        //private readonly IPatientsLabRepository _patientsLabRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IPatientMedicationRepository _patientMedicationRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        private readonly IPatientService _IPatientService;

        JsonModel response = new JsonModel(new object(), StatusMessage.NoContent, (int)HttpStatusCode.NoContent);

        

        public SpringBPatientsVitalsService(IPatientService patientService, ISpringBPatientsVitalsRepository springBPatientsVitalsRepository, IOrganizationRepository organizationRepository, 
            //IPatientsLabRepository patientsLabRepository,
            IPatientMedicationRepository patientMedicationRepository, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository)
        {
            _springBPatientsVitalsRepository = springBPatientsVitalsRepository;
            _organizationRepository = organizationRepository;
            //_patientsLabRepository = patientsLabRepository;
            _patientMedicationRepository = patientMedicationRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
            _IPatientService = patientService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public JsonModel GetSpringBPatientVitals(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel, TokenModel tokenModel)
        {
            if (!_IPatientService.IsValidUserForDataAccess(tokenModel, patientId))
            {
                return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
            }
            List<SpringBVitalModel> springBVital = new List<SpringBVitalModel>();
            springBVital = _springBPatientsVitalsRepository.GetSpringBPatientVital<SpringBVitalModel>(patientId, fromDate, toDate, filterModel, tokenModel).ToList();
            return new JsonModel
            {
                data = springBVital,
                meta = new Meta(springBVital, filterModel),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryInsuredMemberId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JsonModel GetSpringBPatientDiagnosis(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, bool isShowAlert ,TokenModel token)
        {
            if (!_IPatientService.IsValidUserForDataAccess(token, patientId))
            {
                return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
            }
            List<GePatientDiagnosisModel> patientDiagnosis = new List<GePatientDiagnosisModel>();
            patientDiagnosis = _springBPatientsVitalsRepository.GetSpringBPatientDiagnosis<GePatientDiagnosisModel>(patientId, fromDate, toDate, filterModel, isShowAlert, token).ToList();
            return new JsonModel
            {
                data = patientDiagnosis,
                meta = new Meta(patientDiagnosis, filterModel),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterModel"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        //public JsonModel GetSpringBPatientVitalsMobileView(int patientId, DateTime? fromDate, DateTime? toDate, FilterModel filterModel, TokenModel tokenModel)
        //{
        //    if (!_IPatientService.IsValidUserForDataAccess(tokenModel, patientId))
        //    {
        //        return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
        //    }
        //    List<SpringBVitalMobileView> springBVitalMobileView = new List<SpringBVitalMobileView>();
        //    springBVitalMobileView = _springBPatientsVitalsRepository.GetSpringBPatientVitalsMobileView<SpringBVitalMobileView>(patientId, fromDate, toDate, filterModel, tokenModel).ToList();

        //    return new JsonModel
        //    {
        //        data = springBVitalMobileView,
        //        meta = new Meta(springBVitalMobileView, filterModel),
        //        Message = StatusMessage.FetchMessage,
        //        StatusCode = (int)HttpStatusCodes.OK
        //    };
        //}
        ///// <summary>
        /// </summary>
        /// <param name="patientFilterModel"></param>
        /// <param name="tokenModels"></param>
        /// <returns></returns>
        public JsonModel GetMedication(PatientFilterModel patientFilterModel,bool isShowAlert, TokenModel tokenModels)
        {
            if (!_IPatientService.IsValidUserForDataAccess(tokenModels, patientFilterModel.PatientId))
            {
                return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
            }
            List<SpringB_PatietnMedication> patietnMedications = new List<SpringB_PatietnMedication>();
            patietnMedications = _springBPatientsVitalsRepository.GetMedication<SpringB_PatietnMedication>(patientFilterModel, isShowAlert,tokenModels).ToList();
            return new JsonModel
            {
                data = patietnMedications,
                meta = new Meta(patietnMedications, patientFilterModel),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        //public JsonModel GetSpringBPatientLabResults(int patientId, DateTime? fromDate, DateTime? toDate, CommonFilterModel filterModel, TokenModel tokenModel)
        //{
        //    if (!_IPatientService.IsValidUserForDataAccess(tokenModel, patientId))
        //    {
        //        return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
        //    }
        //    List<PatientLabResultsModel> patientLabResultsModel = new List<PatientLabResultsModel>();
        //    patientLabResultsModel = _springBPatientsVitalsRepository.GetSpringBPatientLabResults<PatientLabResultsModel>(patientId, fromDate, toDate, filterModel, tokenModel).ToList();
        //    return new JsonModel
        //    {
        //        data = patientLabResultsModel,
        //        meta = new Meta(patientLabResultsModel, filterModel),
        //        Message = StatusMessage.FetchMessage,
        //        StatusCode = (int)HttpStatusCodes.OK
        //    };
        //}

        //public JsonModel GetLoincCodeDetail(LabFilterModel labFilterModel, TokenModel tokenModel)
        //{
        //    List<LoincCodeDetailModel> loincCodeDetailModels = _springBPatientsVitalsRepository.GetLoincCodeDetail<LoincCodeDetailModel>(labFilterModel, tokenModel).ToList();
        //    if (loincCodeDetailModels != null && loincCodeDetailModels.Count > 0)
        //    {
        //        return new JsonModel
        //        {
        //            data = loincCodeDetailModels,
        //            Message = StatusMessage.FetchMessage,
        //            StatusCode = (int)HttpStatusCodes.OK
        //        };
        //    }
        //    else
        //    {
        //        return new JsonModel
        //        {
        //            data = null,
        //            Message = StatusMessage.NotFound,
        //            StatusCode = (int)HttpStatusCodes.NotFound
        //        };
        //    }
        //}

        //public JsonModel SaveLoincCodeDetails(List<LoincCodeDetailModel> loincCodeDetailModels, int LinkedEncounterId, TokenModel tokenModel)
        //{
        //    using (var transaction = _patientsLabRepository.StartTransaction())
        //    {
        //        try
        //        {
        //            List<ChangesLog> changesLogs = new List<ChangesLog>();
        //            List<int> loincIds = loincCodeDetailModels.Select(a => a.Id).ToList();
        //            List<PatientLab> patientLabs = _patientsLabRepository.GetAll(a => loincIds.Contains(a.Id) && a.IsDeleted == false).ToList();
        //            if (patientLabs != null && patientLabs.Count > 0)
        //            {
        //                patientLabs.ForEach(a =>
        //                {
        //                    LoincCodeDetailModel loincCodeDetailModel = loincCodeDetailModels.Where(b => b.Id == a.Id).FirstOrDefault();
        //                    if (loincCodeDetailModel != null)
        //                    {
        //                        a.Value =  loincCodeDetailModel.Value >0 ?(decimal)loincCodeDetailModel.Value: (decimal?)null;
        //                        a.TextResult = !string.IsNullOrEmpty(loincCodeDetailModel.TextResult) ? loincCodeDetailModel.TextResult : null;
        //                        a.LabTestDate = loincCodeDetailModel.LabTestDate;
        //                        a.IsDeleted = loincCodeDetailModel.IsDeleted;
        //                        a.UpdatedBy = tokenModel.UserID;
        //                        a.UpdatedDate = DateTime.UtcNow;
        //                    }

        //                });

        //                _patientsLabRepository.Update(patientLabs.ToArray());
        //                changesLogs = _patientsLabRepository.GetChangesLogData(tokenModel);
        //                _patientsLabRepository.SaveChanges();
        //                int index = 0;
        //                if (patientLabs != null)
        //                {
        //                    patientLabs.ForEach(x =>
        //                    {
        //                        index++;
        //                        changesLogs.FindAll(y => y.IndexNumber == index).ForEach(y => y.RecordID = x.Id);
        //                    });
        //                }
        //                _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, null, LinkedEncounterId, tokenModel);
        //                transaction.Commit();
        //                return new JsonModel
        //                {
        //                    data = patientLabs,
        //                    Message = StatusMessage.LoincResultUpdated,
        //                    StatusCode = (int)HttpStatusCodes.OK
        //                };
        //            }
        //        }catch(Exception ex)
        //        {
        //            transaction.Rollback();
        //            throw (ex);
        //        }
        //        }
        //    return new JsonModel
        //    {
        //        data = null,
        //        Message = StatusMessage.ErrorOccured,
        //        StatusCode = (int)HttpStatusCodes.BadRequest
        //    };
        //}

        public JsonModel GetDistinctDateOfVitals(int patientId, TokenModel tokenModel)
        {
            List<VitalDateModel> vitalDatesModel = _springBPatientsVitalsRepository.GetDistinctDateOfVitals<VitalDateModel>(patientId, tokenModel).ToList();
            if (vitalDatesModel != null)
            {
                return new JsonModel
                {
                    data = vitalDatesModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }

        #region Medication
        public JsonModel GetMasterMedicationAutoComplete(string SearchText, TokenModel tokenModel)
        {
            List<MasterDropDownForMedication> masterDropDowns = _springBPatientsVitalsRepository.GetMasterMedicationAutoComplete<MasterDropDownForMedication>(SearchText, tokenModel).ToList();
            if (masterDropDowns != null && masterDropDowns.Count>0)
            {
                return new JsonModel
                {
                    data = masterDropDowns,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }
        public JsonModel GetPatientMedicationDetail(int PatientMedicationID, TokenModel tokenModel)
        {
            PatMedicationModel patMedicationModel = _springBPatientsVitalsRepository.GetPatientMedicationDetail<PatMedicationModel>(PatientMedicationID, tokenModel).FirstOrDefault();
            if (patMedicationModel !=null)
            {
                return new JsonModel
                {
                    data = patMedicationModel,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCodes.OK
                };
            }
            else
            {
                return new JsonModel
                {
                    data = null,
                    Message = StatusMessage.NotFound,
                    StatusCode = (int)HttpStatusCodes.NotFound
                };
            }
        }
        public JsonModel SavePatientMedication(PatMedicationModel patMedicationModel,TokenModel tokenModel)
        {
            PatientMedication patientMedication = null;
            using (var transaction = _patientMedicationRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();
                    if (patMedicationModel.Id == 0)
                    {
                        patientMedication = new PatientMedication();
                        AutoMapper.Mapper.Map(patMedicationModel, patientMedication);
                        patientMedication.CreatedBy = tokenModel.UserID;
                        patientMedication.CreatedDate = DateTime.UtcNow;
                        patientMedication.IsDeleted = false;
                        patientMedication.IsActive = true;
                        _patientMedicationRepository.Create(patientMedication);
                        //changesLogs = _patientMedicationRepository.GetChangesLogData(tokenModel);
                        _patientMedicationRepository.SaveChanges();
                        response = new JsonModel(patientMedication, StatusMessage.MedicationSave, (int)HttpStatusCode.OK);
                    }
                    else
                    {
                        patientMedication = _patientMedicationRepository.Get(a => a.Id == patMedicationModel.Id && a.PatientID == patMedicationModel.PatientID && a.IsDeleted == false);
                        AutoMapper.Mapper.Map(patMedicationModel, patientMedication);
                        patientMedication.UpdatedBy = tokenModel.UserID;
                        patientMedication.UpdatedDate = DateTime.UtcNow;
                        _patientMedicationRepository.Update(patientMedication);
                        //changesLogs = _patientMedicationRepository.GetChangesLogData(tokenModel);
                        _patientMedicationRepository.SaveChanges();
                        response = new JsonModel(patientMedication, StatusMessage.MedicationUpdated, (int)HttpStatusCode.OK);
                    }
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientMedication.Id, patMedicationModel.LinkedEncounterId, tokenModel);
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

        public JsonModel DeletePatientMedication(int PatientMedicationID,int linkedEncounterId, TokenModel tokenModel)
        {
            using (var transaction = _patientMedicationRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();
                    PatientMedication patientMedication = _patientMedicationRepository.Get(a => a.Id == PatientMedicationID && a.IsDeleted == false);
                    patientMedication.IsDeleted = true;
                    patientMedication.DeletedBy = tokenModel.UserID;
                    _patientMedicationRepository.Update(patientMedication);
                    //changesLogs = _patientMedicationRepository.GetChangesLogData(tokenModel);
                    _patientMedicationRepository.SaveChanges();
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientMedication.Id, linkedEncounterId, tokenModel);
                    response = new JsonModel(patientMedication, StatusMessage.MedicationDeleted, (int)HttpStatusCode.OK);
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

        public JsonModel AddClaimsMedToCurrent(MedicationDataFilterModel medicationDataFilterModel, TokenModel tokenModel)
        {
            SQLResponseModel responseModel = _springBPatientsVitalsRepository.AddClaimsMedToCurrent<SQLResponseModel>(medicationDataFilterModel, tokenModel).FirstOrDefault();
            return new JsonModel(null, responseModel.Message, responseModel.StatusCode);
        }

        public JsonModel GetLatestPatientVitalDetail(int patientId, DateTime dateTime, TokenModel tokenModel)
        {
            PatientVitalModel springBVitalMobileView = new PatientVitalModel();
            springBVitalMobileView = _springBPatientsVitalsRepository.GetLatestPatientVitalDetail<PatientVitalModel>(patientId, dateTime, tokenModel).FirstOrDefault();

            return new JsonModel
            {
                data = springBVitalMobileView,              
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
        #endregion
    }
}