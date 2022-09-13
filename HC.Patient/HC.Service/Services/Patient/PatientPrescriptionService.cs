using AutoMapper;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories.AuditLog;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientPrescriptionService : BaseService, IPatientPrescriptionService
    {
        private readonly IPatientPrescriptionRepository _patientPrescriptionRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientPrescriptionFaxRepository _patientPrescriptionfaxRepository;
        private HCOrganizationContext _context;
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);

        public PatientPrescriptionService(IPatientRepository patientRepository, IPatientPrescriptionRepository patientPrescriptionRepository, IPatientPrescriptionFaxRepository patientPrescriptionfaxRepository, HCOrganizationContext context, IAuditLogRepository auditLogRepository)
        {
            _patientRepository = patientRepository;
            _patientPrescriptionRepository = patientPrescriptionRepository;
            _context = context;
            _patientPrescriptionfaxRepository = patientPrescriptionfaxRepository;
            _auditLogRepository = auditLogRepository;
        }


        public JsonModel GetprescriptionDrugList()
        {
            List<PatientPrescriptionModel> prescriptiondruglist = new List<PatientPrescriptionModel>();
            prescriptiondruglist = _patientPrescriptionRepository.GetprescriptionDrugList<PatientPrescriptionModel>().ToList();

            return response = new JsonModel()
            {
                data = prescriptiondruglist,
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel SavePrescription(PatientsPrescriptionModel patientsPrescriptionModel, TokenModel tokenModel)
        {
            PatientPrescription patientPrescription = null;
            if (patientsPrescriptionModel.Id == 0)
            {
                patientPrescription = new PatientPrescription();
                Mapper.Map(patientsPrescriptionModel, patientPrescription);
                patientPrescription.CreatedBy = tokenModel.UserID;
                patientPrescription.CreatedDate = DateTime.UtcNow;
                patientPrescription.IsDeleted = false;
                patientPrescription.IsActive = true;
                _patientPrescriptionRepository.Create(patientPrescription);
                _patientPrescriptionRepository.SaveChanges();
                response = new JsonModel(patientPrescription, StatusMessage.PrescriptionSave, (int)HttpStatusCode.OK);
            }
            else
            {

                patientPrescription = _patientPrescriptionRepository.Get(a => a.Id == patientsPrescriptionModel.Id && a.IsDeleted == false);
                if (patientPrescription != null)
                {
                    Mapper.Map(patientsPrescriptionModel, patientPrescription);
                    patientPrescription.UpdatedBy = tokenModel.UserID;
                    patientPrescription.UpdatedDate = DateTime.UtcNow;
                    _patientPrescriptionRepository.Update(patientPrescription);
                    _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.UpdatePrescriptionDetails, AuditLogAction.Modify, null, tokenModel.UserID, "" + null, tokenModel);
                    _patientPrescriptionRepository.SaveChanges();
                    response = new JsonModel(patientPrescription, StatusMessage.PrescriptionUpdated, (int)HttpStatusCode.OK);
                }
            }
            return response;
        }

        public JsonModel GetPrescription(PatientFilterModel patientFilterModel, TokenModel tokenModel)
        {
            List<PatientsPrescriptionModel> patientPrescription = _patientPrescriptionRepository.GetPatientPrescriptions<PatientsPrescriptionModel>(patientFilterModel, tokenModel).ToList();
            if (patientPrescription != null && patientPrescription.Count > 0)
            {
                response = new JsonModel(patientPrescription, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientPrescription, patientFilterModel);
            }
            return response;
        }

        public JsonModel GetPrescriptionById(int id, TokenModel tokenModel)
        {
            PatientPrescription patientPrescription = _patientPrescriptionRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (patientPrescription != null)
            {
                PatientsPrescriptionEditModel patientPrescriptionModel = new PatientsPrescriptionEditModel();
                Mapper.Map(patientPrescription, patientPrescriptionModel);
                response = new JsonModel(patientPrescriptionModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public JsonModel DeletePrescription(int id, TokenModel tokenModel)
        {
            PatientPrescription patientPrescription = _patientPrescriptionRepository.Get(a => a.Id == id && a.IsDeleted == false);
            if (patientPrescription != null)
            {
                patientPrescription.IsDeleted = true;
                patientPrescription.DeletedBy = tokenModel.UserID;
                patientPrescription.DeletedDate = DateTime.UtcNow;
                _patientPrescriptionRepository.Update(patientPrescription);
                _auditLogRepository.SaveChangesWithAuditLogs(AuditLogsScreen.DeletePrescriptionDetails, AuditLogAction.Delete, null, tokenModel.UserID, "" + null, tokenModel);
                _patientPrescriptionRepository.SaveChanges();
                response = new JsonModel(new object(), StatusMessage.PrescriptionDeleted, (int)HttpStatusCodes.OK);
            }
            return response;
        }

        public MemoryStream GetPrescriptionPdf(PatientFaxModel patientsPrescriptionfaxModel, TokenModel tokenModel)
        {
            List<int> presIds = patientsPrescriptionfaxModel.PrescriptionId.Split(',').Select(int.Parse).ToList();
            MemoryStream memoryStream = null;
            if (patientsPrescriptionfaxModel.Issentprescription == false)
            {
                SavePrescriptionLog(patientsPrescriptionfaxModel, tokenModel);
            }
            List<PatientPrescription> patientPrescriptionList = _context.PatientPrescription.Include(p => p.Patient).Include(j => j.Patient.MasterGender).Include(q => q.Prescription).Where(x => presIds.Contains(x.Id) && x.IsDeleted == false).ToList();
            if (patientPrescriptionList.Count > 0)
            {
                PatientPrescriptionPdf patientPrescriptionModel = new PatientPrescriptionPdf();
                patientPrescriptionModel.PatientsPrescriptionPdfModels = new List<PatientsPrescriptionPdfModel>();
                PatientPrescription patientProfile = patientPrescriptionList.FirstOrDefault();
                PHIDecryptedModel patientDecryptedProfile = _patientRepository.GetDecryptedPHIData<PHIDecryptedModel>(patientProfile.Patient.FirstName, patientProfile.Patient.MiddleName, patientProfile.Patient.LastName, patientProfile.Patient.DOB, patientProfile.Patient.Email, patientProfile.Patient.SSN, patientProfile.Patient.MRN, null, null, null, null, null, null, null).FirstOrDefault();

                foreach (var item in patientPrescriptionList)
                {
                    PatientsPrescriptionPdfModel patientsPrescriptionPdfModel = new PatientsPrescriptionPdfModel();
                    patientsPrescriptionPdfModel.Prescription = item.Prescription.DrugName;
                    patientsPrescriptionPdfModel.Dose = item.Dose;
                    patientsPrescriptionPdfModel.Strength = item.Strength;
                    patientsPrescriptionPdfModel.Directions = item.Directions;
                    patientsPrescriptionPdfModel.DateIssued = item.StartDate.ToString("MM/dd/yyyy");
                    patientsPrescriptionPdfModel.Expires = item.EndDate.ToString("MM/dd/yyyy");
                    patientPrescriptionModel.PatientsPrescriptionPdfModels.Add(patientsPrescriptionPdfModel);
                }

                patientPrescriptionModel.PatientName = patientDecryptedProfile.FirstName + " " + patientDecryptedProfile.LastName;
                patientPrescriptionModel.DOB = patientDecryptedProfile.DateOfBirth.Replace("00:00:00.0000000", "");
                if (patientDecryptedProfile.MRN != null)
                {
                    patientPrescriptionModel.MRN = patientDecryptedProfile.MRN;
                }
                else
                {
                    patientPrescriptionModel.MRN = "";
                }
                //patientPrescriptionModel.MRN = patientDecryptedProfile.MRN;
                patientPrescriptionModel.Gender = patientProfile.Patient.MasterGender.Gender;

                memoryStream = this.GenerateMemoryStream(patientPrescriptionModel);
            }

            return memoryStream;
        }
        public MemoryStream GenerateMemoryStream(PatientPrescriptionPdf patient)
        {
            MemoryStream tempStream = null;
            Document document = new Document();

            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;
            document.DefaultPageSetup.TopMargin = 30;
            document.DefaultPageSetup.BottomMargin = 80;

            Section section = document.AddSection();

            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            pageNumberFooterTable.BottomPadding = 0;
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;
            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;
            pageNumberFooterTable.Borders.Top.Visible = true;

            pageNumberFooterTable.Columns[0].Width = 350;
            pageNumberFooterTable.Columns[1].Width = 150;

            MigraDoc.DocumentObjectModel.Tables.Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            pageNumberFooterRow.Cells[0].MergeRight = 1;
            pageNumberFooterRow.Cells[0].AddParagraph("Page ").AddPageField();
            pageNumberFooterRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            pageNumberFooterRow.Format.Alignment = ParagraphAlignment.Right;
            pageNumberFooterRow.Format.Font.Size = 10;

            Paragraph headingTitle = section.AddParagraph();
            headingTitle.AddText("Prescription Report");
            headingTitle.Format.Alignment = ParagraphAlignment.Center;
            headingTitle.Format.Font.Size = 12;
            headingTitle.Format.Font.Bold = true;
            // headingTitle.Format.Font.Color = Colors.Blue;
            Paragraph emptyParagrapgh = section.AddParagraph();
            emptyParagrapgh = section.AddParagraph();
            emptyParagrapgh = section.AddParagraph();
            emptyParagrapgh = section.AddParagraph();

            // section.PageSetup.DifferentFirstPageHeaderFooter = true;
            // Paragraph drawLinePara = section.Footers.Primary.AddParagraph();
            Paragraph drawLinePara = section.AddParagraph();
            drawLinePara.AddFormattedText("_______________________________________________________________________________________").Font.Bold = true;


            Table patientInfoTable = section.AddTable();

            Column patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoColumn = patientInfoTable.AddColumn();
            patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

            patientInfoTable.Format.Font.Size = 11;

            patientInfoTable.Columns[0].Width = 80;
            patientInfoTable.Columns[1].Width = 70;

            Row patientInfoRow = patientInfoTable.AddRow();
            patientInfoRow = patientInfoTable.AddRow();
            patientInfoRow.Cells[0].MergeRight = 2;
            patientInfoRow.Cells[0].AddParagraph(patient.PatientName).Format.Font.Bold = true;
            patientInfoRow.BottomPadding = 5;

            patientInfoRow = patientInfoTable.AddRow();
            patientInfoRow.TopPadding = 5;
            patientInfoRow.BottomPadding = 5;
            patientInfoRow.Cells[0].AddParagraph("Gender :").Format.Alignment = ParagraphAlignment.Left;
            patientInfoRow.Cells[1].AddParagraph(patient.Gender).Format.Alignment = ParagraphAlignment.Left;

            patientInfoRow = patientInfoTable.AddRow();
            patientInfoRow.TopPadding = 5;
            patientInfoRow.BottomPadding = 5;
            patientInfoRow.Cells[0].AddParagraph("Date of Birth :").Format.Alignment = ParagraphAlignment.Left;
            patientInfoRow.Cells[1].AddParagraph(patient.DOB).Format.Alignment = ParagraphAlignment.Left;

            patientInfoRow = patientInfoTable.AddRow();
            patientInfoRow.TopPadding = 5;
            patientInfoRow.BottomPadding = 5;
            patientInfoRow.Cells[0].AddParagraph("MRN :").Format.Alignment = ParagraphAlignment.Left;
            patientInfoRow.Cells[1].AddParagraph(patient.MRN).Format.Alignment = ParagraphAlignment.Left;

            //patientInfoRow = patientInfoTable.AddRow();
            //patientInfoRow.TopPadding = 5;
            //patientInfoRow.BottomPadding = 5;
            //patientInfoRow.Cells[0].AddParagraph("Created Date :").Format.Alignment = ParagraphAlignment.Left;
            //patientInfoRow.Cells[1].AddParagraph(patient.CreatedDate?.ToString("MM/dd/yyyy")).Format.Alignment = ParagraphAlignment.Left;


            Paragraph emptyPara = section.AddParagraph();
            emptyPara = section.AddParagraph();
            emptyPara = section.AddParagraph();
            emptyPara.AddFormattedText("_______________________________________________________________________________________").Font.Bold = true;

            Table prescriptionListInfoTable = section.AddTable();
            //prescriptionListInfoTable.Borders.Visible = true;
            prescriptionListInfoTable.Borders.Visible = false;
            Column prescriptionListInfoColumn = prescriptionListInfoTable.AddColumn();
            prescriptionListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
            for (int ind = 0; ind < 4; ind++)
            {
                prescriptionListInfoColumn = prescriptionListInfoTable.AddColumn();
                prescriptionListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
            }

            prescriptionListInfoTable.Format.Font.Size = 9;
            for (int y = 0; y < 5; y++)
            {
                if (y == 0)
                    prescriptionListInfoTable.Columns[y].Width = 120;
                else if (y == 2)
                    prescriptionListInfoTable.Columns[y].Width = 60;
                else prescriptionListInfoTable.Columns[y].Width = 90;
            }

            Row prescriptionListInfoRow = prescriptionListInfoTable.AddRow();
            prescriptionListInfoRow = prescriptionListInfoTable.AddRow();
            prescriptionListInfoRow.HeadingFormat = true;
            //prescriptionListInfoRow.Format.Font.Size = 10;
            prescriptionListInfoRow.BottomPadding = 5;
            prescriptionListInfoRow.TopPadding = 5;
            prescriptionListInfoRow.Format.Alignment = ParagraphAlignment.Left;
            prescriptionListInfoRow.Cells[0].AddParagraph("DRUG NAME").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[1].AddParagraph("STRENGTH(mg)").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[2].AddParagraph("DOSE").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[3].AddParagraph("Date Issued").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[4].AddParagraph("Directions").Format.Font.Bold = true;

            foreach (var item in patient.PatientsPrescriptionPdfModels)
            {
                prescriptionListInfoRow = prescriptionListInfoTable.AddRow();
                prescriptionListInfoRow = prescriptionListInfoTable.AddRow();
                //prescriptionListInfoRow.Format.Font.Size = 10;
                prescriptionListInfoRow.BottomPadding = 5;
                prescriptionListInfoRow.TopPadding = 5;
                prescriptionListInfoRow.Format.Alignment = ParagraphAlignment.Left;
                prescriptionListInfoRow.Cells[0].AddParagraph(item.Prescription ?? "");
                prescriptionListInfoRow.Cells[1].AddParagraph(item.Strength ?? "");
                prescriptionListInfoRow.Cells[2].AddParagraph(item.Dose ?? "");
                prescriptionListInfoRow.Cells[3].AddParagraph(item.DateIssued ?? "");
                prescriptionListInfoRow.Cells[4].AddParagraph(item.Directions ?? "");
            }

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf

            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }

        public JsonModel SendFax(PatientFaxModel patientsPrescriptionfaxModel, TokenModel tokenModel)
        {
            PatientPrescriptionFaxLog patientPrescriptionfaxlog = null;
            if (patientsPrescriptionfaxModel.Issentprescription == false)
            {

                if (patientsPrescriptionfaxModel.Id == 0)
                {
                    patientPrescriptionfaxlog = new PatientPrescriptionFaxLog();
                    Mapper.Map(patientsPrescriptionfaxModel, patientPrescriptionfaxlog);
                    patientPrescriptionfaxlog.CreatedBy = tokenModel.UserID;
                    patientPrescriptionfaxlog.SentDate = DateTime.UtcNow;
                    patientPrescriptionfaxlog.IsFax = 1;
                    _patientPrescriptionfaxRepository.Create(patientPrescriptionfaxlog);
                    _patientPrescriptionfaxRepository.SaveChanges();
                    response = new JsonModel(patientPrescriptionfaxlog, StatusMessage.PrescriptionFaxSent, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                response = new JsonModel(patientPrescriptionfaxlog, StatusMessage.PrescriptionFaxSent, (int)HttpStatusCode.OK);
            }

            return response;
        }

        public JsonModel GetMasterprescriptionDrugs(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterPrescriptionDrugsModel> masterPrescriptionDrugsModel = _patientPrescriptionRepository.GetMasterprescriptionDrugsList<MasterPrescriptionDrugsModel>(searchFilterModel, tokenModel).ToList();
            if (masterPrescriptionDrugsModel != null && masterPrescriptionDrugsModel.Count > 0)
            {
                response = new JsonModel(masterPrescriptionDrugsModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterPrescriptionDrugsModel, searchFilterModel);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.MasterICDDoesNotExist, (int)HttpStatusCodes.BadRequest);
            }
            return response;
        }

        public JsonModel GetMasterPharmacy(SearchFilterModel searchFilterModel, TokenModel tokenModel)
        {
            List<MasterPharmacyModel> masterMasterPharmacyModel = _patientPrescriptionRepository.GetMasterPharmacyList<MasterPharmacyModel>(searchFilterModel, tokenModel).ToList();
            if (masterMasterPharmacyModel != null && masterMasterPharmacyModel.Count > 0)
            {
                response = new JsonModel(masterMasterPharmacyModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(masterMasterPharmacyModel, searchFilterModel);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.MasterICDDoesNotExist, (int)HttpStatusCodes.BadRequest);
            }
            return response;
        }
        private void SavePrescriptionLog(PatientFaxModel patientsPrescriptionfaxModel, TokenModel tokenModel)
        {
            PatientPrescriptionFaxLog patientPrescriptionfaxlog = null;
            if (patientsPrescriptionfaxModel.Id == 0)
            {
                patientPrescriptionfaxlog = new PatientPrescriptionFaxLog();
                Mapper.Map(patientsPrescriptionfaxModel, patientPrescriptionfaxlog);
                patientPrescriptionfaxlog.CreatedBy = tokenModel.UserID;
                patientPrescriptionfaxlog.SentDate = DateTime.UtcNow;
                patientPrescriptionfaxlog.IsFax = 0;
                _patientPrescriptionfaxRepository.Create(patientPrescriptionfaxlog);
                _patientPrescriptionfaxRepository.SaveChanges();
            }
        }

        public JsonModel GetSentPrescriptions(PatientFilterModel patientFilterModel, TokenModel tokenModel)
        {
            List<PatientsSentPrescriptionModel> patientPrescription = _patientPrescriptionRepository.GetPatientSentPrescriptions<PatientsSentPrescriptionModel>(patientFilterModel, tokenModel).ToList();
            if (patientPrescription != null && patientPrescription.Count > 0)
            {
                response = new JsonModel(patientPrescription, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(patientPrescription, patientFilterModel);
            }
            return response;
        }
    }
}
