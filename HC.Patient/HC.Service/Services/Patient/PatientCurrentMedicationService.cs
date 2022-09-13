using HC.Common.HC.Common;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.MasterData;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientCurrentMedicationService : BaseService, IPatientCurrentMedicationService
    {
        private readonly IPatientCurrentMedicationRepository _patientCurrentMedicationRepository;
        private readonly IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private IEmailService _emailService;
        private readonly IHostingEnvironment _env;
        private readonly IPatientService _IPatientService;
        private readonly IMasterMedicationRepository _masterMedicationRepository;
        private readonly IPatientMedicationRepository _patientMedicationRepository;
        JsonModel response = new JsonModel(new object(), StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
        public PatientCurrentMedicationService(IPatientService patientService, IHostingEnvironment env, IPatientCurrentMedicationRepository patientCurrentMedicationRepository, 
            IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository, IEmailService emailService, IOrganizationSMTPRepository organizationSMTPRepository,
            IMasterMedicationRepository masterMedicationRepository, IPatientMedicationRepository patientMedicationRepository)
        {
            _patientCurrentMedicationRepository = patientCurrentMedicationRepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
            _emailService = emailService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _env = env;
            _IPatientService = patientService;
            _masterMedicationRepository = masterMedicationRepository;
            _patientMedicationRepository = patientMedicationRepository;
        }

        public JsonModel GetCurrentMedication(PatientFilterModel patientFilterModel, bool isShowAlert, TokenModel tokenModel)
        {
            if (!_IPatientService.IsValidUserForDataAccess(tokenModel, patientFilterModel.PatientId))
            {
                return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
            }
            List<PatientsCurrentMedicationModel> patientCurrentMedicationModel = _patientCurrentMedicationRepository.GetCurrentMedication<PatientsCurrentMedicationModel>(patientFilterModel, isShowAlert, tokenModel).ToList();
            if (patientCurrentMedicationModel != null && patientCurrentMedicationModel.Count > 0)
            {
                response = new JsonModel(patientCurrentMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientCurrentMedicationModel, patientFilterModel);
            }
            return response;
        }

        public JsonModel GetCurrentMedicationStength(string medicationName, TokenModel tokenModel)
        {
            List<MasterCurrentMedicationModel> patientCurrentMedicationModel = _patientCurrentMedicationRepository.GetCurrentMedicationStength<MasterCurrentMedicationModel>(medicationName, tokenModel).ToList();
            if (patientCurrentMedicationModel != null && patientCurrentMedicationModel.Count > 0)
            {
                response = new JsonModel(patientCurrentMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                //response.meta = new Meta(patientCurrentMedicationModel, patientFilterModel);
            }
            return response;
        }
        public JsonModel GetCurrentMedicationUnit(string medicationName, TokenModel tokenModel)
        {
            List<MasterCurrentMedicationModel> patientCurrentMedicationModel = _patientCurrentMedicationRepository.GetCurrentMedicationUnit<MasterCurrentMedicationModel>(medicationName, tokenModel).ToList();
            if (patientCurrentMedicationModel != null && patientCurrentMedicationModel.Count > 0)
            {
                response = new JsonModel(patientCurrentMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                //response.meta = new Meta(patientCurrentMedicationModel, patientFilterModel);
            }
            return response;
        }
        public JsonModel GetCurrentMedicationForm(string medicationName, TokenModel tokenModel)
        {
            List<MasterCurrentMedicationModel> patientCurrentMedicationModel = _patientCurrentMedicationRepository.GetCurrentMedicationForm<MasterCurrentMedicationModel>(medicationName, tokenModel).ToList();
            if (patientCurrentMedicationModel != null && patientCurrentMedicationModel.Count > 0)
            {
                response = new JsonModel(patientCurrentMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                //response.meta = new Meta(patientCurrentMedicationModel, patientFilterModel);
            }
            return response;
        }

        public JsonModel SaveCurrentMedication(PatientsCurrentMedicationModel patientCurrentMedicationModel, TokenModel tokenModel)
        {
            using (var transaction = _patientCurrentMedicationRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();
                    PatientCurrentMedication patientCurrentMedication = null;
                    MasterMedication masterMedication = null;
                    if (patientCurrentMedicationModel.IsManualEntry == true)
                    {
                        masterMedication = new Entity.MasterMedication();
                        masterMedication.DrugName = patientCurrentMedicationModel.Medication;
                        masterMedication.dosage_form = patientCurrentMedicationModel.DosageForm;
                        masterMedication.strength = patientCurrentMedicationModel.Dose;
                        masterMedication.strength_unit_of_measure = patientCurrentMedicationModel.Unit;
                        masterMedication.IsActive = true;
                        masterMedication.IsDeleted = false;
                        masterMedication.OrganizationID = tokenModel.OrganizationID;
                        masterMedication.CreatedBy = tokenModel.UserID;
                        masterMedication.CreatedDate = DateTime.UtcNow;
                        _masterMedicationRepository.Create(masterMedication);
                        _masterMedicationRepository.SaveChanges();
                        patientCurrentMedicationModel.MedicationId = masterMedication.Id;
                    }
                    if (patientCurrentMedicationModel.Id == 0)
                    {
                        patientCurrentMedication = new Entity.PatientCurrentMedication();
                        AutoMapper.Mapper.Map(patientCurrentMedicationModel, patientCurrentMedication);
                        patientCurrentMedication.CreatedBy = tokenModel.UserID;
                        patientCurrentMedication.CreatedDate = DateTime.UtcNow;
                        patientCurrentMedication.IsDeleted = false;
                        patientCurrentMedication.IsActive = true;
                        _patientCurrentMedicationRepository.Create(patientCurrentMedication);
                        //changesLogs = _patientCurrentMedicationRepository.GetChangesLogData(tokenModel);
                        _patientCurrentMedicationRepository.SaveChanges();
                        response = new JsonModel(patientCurrentMedication, StatusMessage.MedicationSave, (int)HttpStatusCode.OK);
                    }
                    else
                    {

                        patientCurrentMedication = _patientCurrentMedicationRepository.Get(a => a.Id == patientCurrentMedicationModel.Id && a.IsDeleted == false && a.IsActive == true);
                        if (patientCurrentMedication != null)
                        {
                            AutoMapper.Mapper.Map(patientCurrentMedicationModel, patientCurrentMedication);
                            patientCurrentMedication.UpdatedBy = tokenModel.UserID;
                            patientCurrentMedication.UpdatedDate = DateTime.UtcNow;
                            _patientCurrentMedicationRepository.Update(patientCurrentMedication);
                            //changesLogs = _patientCurrentMedicationRepository.GetChangesLogData(tokenModel);
                            _patientCurrentMedicationRepository.SaveChanges();
                            response = new JsonModel(patientCurrentMedication, StatusMessage.MedicationUpdated, (int)HttpStatusCode.OK);
                        }
                    }
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientCurrentMedication.Id, patientCurrentMedicationModel.LinkedEncounterId, tokenModel);
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return response;
        }

        public JsonModel GetCurrentMedicationById(int id, TokenModel tokenModel)
        {

            PatientsCurrentMedicationModel patientCurrentMedicationModel = _patientCurrentMedicationRepository.GetCurrentMedicationById<PatientsCurrentMedicationModel>(id, tokenModel).FirstOrDefault();
            if (patientCurrentMedicationModel != null)
            {
                response = new JsonModel(patientCurrentMedicationModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
            }
            return response;
        }

        public JsonModel DeleteCurrentMedication(int id, int LinkedEncounterId, TokenModel tokenModel)
        {
            using (var transaction = _patientCurrentMedicationRepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();
                    PatientCurrentMedication patientCurrentMedication = _patientCurrentMedicationRepository.Get(a => a.Id == id && a.IsDeleted == false && a.IsActive == true);
                    if (patientCurrentMedication != null)
                    {
                        patientCurrentMedication.IsDeleted = true;
                        patientCurrentMedication.DeletedBy = tokenModel.UserID;
                        patientCurrentMedication.DeletedDate = DateTime.UtcNow;
                        _patientCurrentMedicationRepository.Update(patientCurrentMedication);
                        //changesLogs = _patientCurrentMedicationRepository.GetChangesLogData(tokenModel);
                        _patientCurrentMedicationRepository.SaveChanges();
                        response = new JsonModel(new object(), StatusMessage.MedicationDeleted, (int)HttpStatusCodes.OK);
                    }
                    _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, patientCurrentMedication.Id, LinkedEncounterId, tokenModel);
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

        public MemoryStream PrintPatientCurrentMedication(int patientID, TokenModel tokenModel, PatientFilterModel patientFilterModel)
        {
            //if (!_IPatientService.IsValidUserForDataAccess(tokenModel, patientID))
            //{
            //    return new MemoryStream();
            //}
            PatientCurrentMedicationModel patientCurrentMedicationModel = _patientCurrentMedicationRepository.PrintPatientCurrentMedication<PatientCurrentMedicationModel>(patientID, tokenModel);
            patientFilterModel.PatientId = patientID;


            List<PatientsMedicationModelForPrint> patientMedicationModel = _patientMedicationRepository.GetMedicationForPrint<PatientsMedicationModelForPrint>(patientFilterModel, tokenModel).ToList();

            //if (patientCurrentMedicationModel != null && patientCurrentMedicationModel.PatientDetailsModel != null && patientCurrentMedicationModel.PrintPatientCurrentMedicationModel.Count() > 0)
            //{
            //    return GeneratePatientCurrentMedicationPDF(patientCurrentMedicationModel, tokenModel);
            //}
            //return null;
            MemoryStream memoryStream = GeneratePatientCurrentMedicationPDF(patientMedicationModel, tokenModel);
            return memoryStream;
        }

        public MemoryStream GeneratePatientCurrentMedicationPDF(List<PatientsMedicationModelForPrint> printPatientCurrentMedicationModels, TokenModel token)

        {
            List<int> chklistIdsList = new List<int>();
            MemoryStream tempStream = null;
            Document document = new Document();


            //Header Section

            Section section = document.AddSection();
           
            Paragraph headerParagraph = section.AddParagraph();

            headerParagraph.AddText(ConstantStringMessage.PrintPatientCurrentMedicationHederLine1);
            headerParagraph.AddLineBreak();
            headerParagraph.AddText(ConstantStringMessage.PrintPatientCurrentMedicationHederLine2);
            headerParagraph.Format.Font.Name = "Arial";
            headerParagraph.Format.Font.Size = 12;
            headerParagraph.Format.Alignment = ParagraphAlignment.Center;
            headerParagraph.Format.Font.Bold = true;

            Table patientInfoTable = section.AddTable();
            patientInfoTable.TopPadding = 10;
            
            Column patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoTable.Format.Font.Size = 11;

            patientInfoTable.Columns[0].Width = 170;
            patientInfoTable.Columns[1].Width = 150;
            patientInfoTable.Columns[2].Width = 120;

            Paragraph patientInfoPara = new Paragraph();
            MigraDoc.DocumentObjectModel.Tables.Row patientInfoRow = patientInfoTable.AddRow();

            patientInfoPara = patientInfoRow.Cells[0].AddParagraph();


            //need to work for patient details
            patientInfoPara.AddFormattedText(ConstantStringMessage.PrintPatientCurrentMedicationMemberName).Font.Bold = true;
            patientInfoPara.AddText(printPatientCurrentMedicationModels[0].PatientName);

            patientInfoPara = patientInfoRow.Cells[1].AddParagraph();

            patientInfoPara.AddFormattedText(ConstantStringMessage.PrintPatientCurrentMedicationMemberDOB).Font.Bold = true;
            patientInfoPara.AddText(printPatientCurrentMedicationModels[0].DOB);

            patientInfoPara = patientInfoRow.Cells[2].AddParagraph();

            patientInfoPara.AddFormattedText(ConstantStringMessage.PrintPatientCurrentMedicationMemberGender).Font.Bold = true;
            patientInfoPara.AddText(printPatientCurrentMedicationModels[0].Gender);


            // Patient details

            Table reportTable = section.AddTable();
            //reportTable.Borders.Visible = true;
            reportTable.LeftPadding = 0;
            reportTable.RightPadding = 0;
            reportTable.Format.Font.Size = 8;
            reportTable.TopPadding = 10;

            Column reportColumn = reportTable.AddColumn();
            reportColumn.LeftPadding = 0;
            reportColumn.RightPadding = 0;
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;
            reportColumn = reportTable.AddColumn();
            reportColumn.Format.Alignment = ParagraphAlignment.Left;

            reportTable.Columns[0].Width = 50;
            reportTable.Columns[1].Width = 50;
            reportTable.Columns[2].Width = 25;
            reportTable.Columns[3].Width = 25;
            reportTable.Columns[4].Width = 25;
            reportTable.Columns[5].Width = 45;
            reportTable.Columns[6].Width = 60;
            reportTable.Columns[7].Width = 75;
            reportTable.Columns[8].Width = 50;
            reportTable.Columns[9].Width = 50;
            reportTable.Columns[10].Width = 50;

            Row reportHeadingRow = reportTable.AddRow();
            reportHeadingRow.Cells[0].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberMedication);
            reportHeadingRow.Cells[2].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberDose);
           // reportHeadingRow.Cells[5].AddParagraph(ConstantStringMessage.PrintPatientCurrentMedicationMemberFrequency);
            reportHeadingRow.Cells[7].AddParagraph("Strength");
            reportHeadingRow.Cells[8].AddParagraph("Start Date");
            reportHeadingRow.Cells[9].AddParagraph("End Date");
            reportHeadingRow.Cells[10].AddParagraph("Status");
            reportHeadingRow.Format.Font.Bold = true;

            if (printPatientCurrentMedicationModels.Count > 0)
            {
                foreach (var item in printPatientCurrentMedicationModels)
                {
                    Row reportRow = reportTable.AddRow();
                    reportRow.Cells[0].AddParagraph(item.Medicine != null ? item.Medicine : "");
                    reportRow.Cells[2].AddParagraph(item.Dose != null ? item.Dose.ToString() : "");
                   // reportRow.Cells[5].AddParagraph(item.Frequency != null ? item.Frequency : "");
                    reportRow.Cells[7].AddParagraph(item.Strength != null ? item.Strength : "");
                    reportRow.Cells[8].AddParagraph(item.StartDate != null ? item.StartDate.ToString("MM/dd/yyyy") : "");
                    reportRow.Cells[9].AddParagraph(item.EndDate != null ? item.EndDate.ToString("MM/dd/yyyy") : "");
                    reportRow.Cells[10].AddParagraph(item.IsActive == true ? "Active" : "InActive");
                }
            }
            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...     

            pdfRenderer.PdfDocument.Save(tempStream, false);
            return tempStream;
        }

        public JsonModel GetCurrentAndClaimMedicationList(PatientFilterModel patientFilterModel, TokenModel tokenModel)
        {
            List<PatientCurrentAndClaimMedicationModel> patientCurrentAndClaimMedications = _patientCurrentMedicationRepository.GetCurrentAndClaimMedicationList<PatientCurrentAndClaimMedicationModel>(patientFilterModel, tokenModel).ToList();
            JsonModel response = new JsonModel(patientCurrentAndClaimMedications, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
            if (patientCurrentAndClaimMedications != null && patientCurrentAndClaimMedications.Count > 0)
            {
                response = new JsonModel(patientCurrentAndClaimMedications, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientCurrentAndClaimMedications, patientFilterModel);
            }
            else
            {
                 response.meta = new Meta(patientCurrentAndClaimMedications, patientFilterModel);                
            }
            return response;
        }
    }
}
