using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using static HC.Common.Enums.CommonEnum;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel.Shapes;
using System.Text.RegularExpressions;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using HC.Patient.Repositories.IRepositories.PatientEncLinkedDataChanges;
using System.Data;
using static HC.Model.ProgramsFilterModel;
using HtmlAgilityPack;

namespace HC.Patient.Service.Services.Patient
{
    public class PatientHRAService : BaseService, IPatientHRAService
    {
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientHRARepository _patientHRARepository;
        private readonly IHostingEnvironment _env;
        private IPatientEncLinkedDataChangesRepository _patientEncLinkedDataChangesRepository;
        private readonly IPatientService _IPatientService;
        public PatientHRAService(IPatientService patientService, IPatientHRARepository patientHRARepository, IHostingEnvironment env, IPatientEncLinkedDataChangesRepository patientEncLinkedDataChangesRepository)
        {
            _patientHRARepository = patientHRARepository;
            _patientEncLinkedDataChangesRepository = patientEncLinkedDataChangesRepository;
            _env = env;
            _IPatientService = patientService;
        }

        //public JsonModel GetMemberHealthPlanForHRA(string searchText, TokenModel tokenModel)
        //{
        //    List<PatientHealthPlanHRAModel> patientHealthPlanHRA = _patientHRARepository.GetMemberHealthPlanForHRA<PatientHealthPlanHRAModel>(searchText, tokenModel).ToList();
        //    response = patientHealthPlanHRA != null && patientHealthPlanHRA.Count > 0 ? new JsonModel(patientHealthPlanHRA, StatusMessage.FetchMessage, (int)HttpStatusCode.OK)
        //        : new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
        //    return response;
        //}
        public JsonModel GetMemberHRAListing(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel)
        {
            List<PatientHRAModel> patientHRAModel = _patientHRARepository.GetMemberHRAListing<PatientHRAModel>(filterModelForMemberHRA, tokenModel).ToList();
            response = patientHRAModel != null && patientHRAModel.Count > 0 ?
            new JsonModel(patientHRAModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK) :
            new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
            response.meta = new Meta(patientHRAModel != null ? patientHRAModel : new List<PatientHRAModel>(), filterModelForMemberHRA);
            return response;
        }

        //public JsonModel BulkAssignHRA(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel)
        //{
        //    SQLResponseModel response = _patientHRARepository.BulkAssignHRA<SQLResponseModel>(filterModelForMemberHRA, tokenModel).FirstOrDefault();
        //    return new JsonModel(null, response.Message, response.StatusCode);
        //}
        //public JsonModel BulkUpdateHRA(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel)
        //{
        //    SQLResponseModel response = _patientHRARepository.BulkUpdateHRA<SQLResponseModel>(filterModelForMemberHRA, tokenModel).FirstOrDefault();
        //    return new JsonModel(null, response.Message, response.StatusCode);
        //}
        public JsonModel GetPatientHRAData(string patDocIdArray, TokenModel tokenModel)
        {
            List<PatientHRAModel> patientHealthPlanHRA = _patientHRARepository.GetPatientHRAData<PatientHRAModel>(patDocIdArray, tokenModel).ToList();
            response = patientHealthPlanHRA != null && patientHealthPlanHRA.Count > 0 ? new JsonModel(patientHealthPlanHRA, StatusMessage.FetchMessage, (int)HttpStatusCode.OK)
                : new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
            return response;
        }
        //public JsonModel GetEmailTemplatesForDD(TokenModel tokenModel)
        //{
        //    List<EmailTemplatesModel> emailTemplatesModel = _patientHRARepository.GetEmailTemplatesForDD<EmailTemplatesModel>(tokenModel).ToList();
        //    response = emailTemplatesModel != null && emailTemplatesModel.Count > 0 ? new JsonModel(emailTemplatesModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK)
        //        : new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
        //    return response;
        //}
        public JsonModel UpdatePatientHRAData(List<PatientHRAModel> patientHRAModel, TokenModel tokenModel)
        {
            using (var transaction = _patientHRARepository.StartTransaction())
            {
                try
                {
                    List<ChangesLog> changesLogs = new List<ChangesLog>();
                    List<DFA_PatientDocuments> patientHRAList = null;
                    patientHRAList = _patientHRARepository.GetAll(a => patientHRAModel.Any(x => x.PatientDocumentId == a.Id) && a.OrganizationID == tokenModel.OrganizationID && a.IsActive == true && a.IsDeleted == false).ToList();

                    foreach (var item in patientHRAList)
                    {
                        PatientHRAModel HRAModel = patientHRAModel.FirstOrDefault(x => x.PatientDocumentId == item.Id);
                        item.Status = HRAModel.StatusId;
                        item.ExpirationDate = HRAModel.ExpirationDate;
                        item.CompletionDate = HRAModel.CompletionDate;
                        item.CreatedBy = tokenModel.UserID;
                        item.CreatedDate = DateTime.UtcNow;
                        item.IsDeleted = false;
                        item.IsActive = true;
                        item.OrganizationID = tokenModel.OrganizationID;
                    }
                    if (patientHRAList.Count > 0)
                    {
                        _patientHRARepository.Update(patientHRAList.ToArray());
                      //  changesLogs = _patientHRARepository.GetChangesLogData(tokenModel);
                        _patientHRARepository.SaveChanges();

                        int index = 0;
                        patientHRAList.ForEach(x =>
                            {
                                index++;
                                changesLogs.FindAll(y => y.IndexNumber == index).ForEach(y => y.RecordID = x.Id);
                            });
                        _patientEncLinkedDataChangesRepository.savePatientEncounterChanges(changesLogs, null, patientHRAModel[0].LinkedEncounterId, tokenModel);
                        transaction.Commit();
                        response = new JsonModel(new object(), StatusMessage.MemberHRADataUpdated, (int)HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return response;
        }
        public MemoryStream PrintIndividualSummaryReport(int patientDocumentId, int patientId, TokenModel token)
        {
            //if (!_IPatientService.IsValidUserForDataAccess(token, patientId))
            //{
            //    return new MemoryStream();
            //}
            MemoryStream ms = new MemoryStream();
            //Get the type of Assessment
            PatientAssessmentModel patientAssessmentType = _patientHRARepository.GetPatientAssessmentTpye<PatientAssessmentModel>(patientId, patientDocumentId, token).FirstOrDefault();
            if (patientAssessmentType.DocumentName.Contains("WHO-5") && patientAssessmentType.DocumentDescription.Contains("WHO-5"))
            {
                PatientWHOReportModel patientWHOReportModel = _patientHRARepository.PrintWHOIndividualSummaryReport<PatientWHOReportModel>(patientDocumentId, patientId, token);
                MemoryStream memoryStream = GenerateWHOIndividualSummaryPDF(patientWHOReportModel, token, "WHO-5 Well-Being Index", "Additional Information and Resources");
                return memoryStream;
            }
            else if (patientAssessmentType.DocumentName.Contains("Asthma") && patientAssessmentType.DocumentDescription.Contains("Asthma"))
            {
                PatientAsthmaReportModel patientAsthmaReportModel = _patientHRARepository.PrintAsthmaIndividualSummaryReport<PatientAsthmaReportModel>(patientDocumentId, patientId, token);
                MemoryStream memoryStream = GenerateAsthmaIndividualSummaryPDF(patientAsthmaReportModel, token, "Asthma Control Test (ACT)", "For more information about asthma and asthma -related issues visit the websites below:");
                return memoryStream;
            }
            else if (patientAssessmentType.DocumentName.Contains("COPD") && patientAssessmentType.DocumentDescription.Contains("COPD"))
            {
                PatientCOPDReportModel patientCOPDReportModel = _patientHRARepository.PrintCOPDIndividualSummaryReport<PatientCOPDReportModel>(patientDocumentId, patientId, token);
                MemoryStream memoryStream = GenerateCOPDIndividualSummaryPDF(patientCOPDReportModel, token, "The COPD Assessment Test (CAT)", "For more information about COPD management and treatment visit the websites below:");
                return memoryStream;
            }
            else if (patientAssessmentType.DocumentName.Contains("Diabetes") && patientAssessmentType.DocumentDescription.Contains("Diabetes"))
            {
                PatientDiabetesReportModel patientDiabetesReportModel = _patientHRARepository.PrintDiabetesIndividualSummaryReport<PatientDiabetesReportModel>(patientDocumentId, patientId, token);
                MemoryStream memoryStream = GenerateDiabetesIndividualSummaryPDF(patientDiabetesReportModel, token, "The Diabetes Risk Test", "For more information on how to prevent diabetes and improve your diabetes risk, visit the websites below");
                return memoryStream;
            }
            else if (patientAssessmentType.DocumentName.Contains("Cardiovascular") && patientAssessmentType.DocumentDescription.Contains("Cardiovascular"))
            {
                PatientCardiovascularReportModel patientCardiovascularReportModel = _patientHRARepository.PrintCardiovascularIndividualSummaryReport<PatientCardiovascularReportModel>(patientDocumentId, patientId, token);
                MemoryStream memoryStream = GenerateCardiovascularIndividualSummaryPDF(patientCardiovascularReportModel, token);
                return memoryStream;
            }
            else if (patientAssessmentType.DocumentName.Contains("PHQ-9") && patientAssessmentType.DocumentDescription.Contains("PHQ-9"))
            {
                PatientDiabetesReportModel patientDiabetesReportModel = _patientHRARepository.PrintDiabetesIndividualSummaryReport<PatientDiabetesReportModel>(patientDocumentId, patientId, token);
                MemoryStream memoryStream = GenerateDiabetesIndividualSummaryPDF(patientDiabetesReportModel, token, "Depression Screening Patient Health Questionnaire(PHQ - 9)", "For more information on depression and where to get help visit the websites below:");
                return memoryStream;
            }
            else
            {
                PatientHRAReportModel patientHRAReportModel = _patientHRARepository.PrintIndividualSummaryReport<PatientHRAReportModel>(patientDocumentId, patientId, token);
                MemoryStream memoryStream = GenerateIndividualSummaryPDF(patientHRAReportModel, token);
                return memoryStream;
            }
        }
        public MemoryStream GenerateIndividualSummaryPDF(PatientHRAReportModel patientHRAReportModel, TokenModel token)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
            //document.DefaultPageSetup.PageFormat = PageFormat.Letter;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;

            var style = document.Styles["Normal"];
            style.Font.Name = "Neue Haas";
            style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            document.DefaultPageSetup.TopMargin = 70;
            document.DefaultPageSetup.BottomMargin = 80;
            Section section = document.AddSection();
            //Document Header
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Paragraph topHeaderParagraph = section.Headers.Primary.AddParagraph();
            topHeaderParagraph.AddText("Health Risk Assessment Individual Report");
            topHeaderParagraph.Format.Font.Size = 12;
            topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
            topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Document Footer
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 150;
            pageNumberFooterTable.Columns[1].Width = 200;
            pageNumberFooterTable.Columns[2].Width = 160;

            Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            //pageNumberFooterRow.BottomPadding = 5;
            pageNumberFooterRow.TopPadding = 15;
            pageNumberFooterRow.Cells[0].AddParagraph("Proprietary & Confidential").Format.Alignment = ParagraphAlignment.Left;
            //pageNumberFooterRow.Cells[0].Borders.DistanceFromTop = 50;
            pageNumberFooterRow.Cells[1].AddParagraph().AddPageField();
            pageNumberFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            pageNumberFooterRow.Format.Font.Size = 10;
            string path = _env.WebRootPath + "\\PDFImages\\ohc-logo-without-tag.png";
            pageNumberFooterRow.Cells[2].AddImage(path);
            pageNumberFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //First page

            Table firstPageTable = section.AddTable();

            Column firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left;

            firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left; ;

            firstPageTable.Columns[0].Width = 300;
            firstPageTable.Columns[1].Width = 150;


            firstPageTable.Format.Font.Size = 10;

            Row firstPageRow = firstPageTable.AddRow();
            string logoPath = _env.WebRootPath + "\\PDFImages\\overture-indreport.png";
            firstPageRow.Cells[0].AddImage(logoPath).Width = 300;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            //firstPageRow.Cells[1].MergeDown = 3;
            string impPath = _env.WebRootPath + "\\PDFImages\\individual-report-img.png";
            var myImage = section.Headers.FirstPage.AddImage(impPath);
            myImage.Height = "29.7cm";
            myImage.Width = "6cm";
            myImage.Left = 450;
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Cells[0].AddParagraph("Health Risk Assessment Individual Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 20;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + patientHRAReportModel.PatientDetailsModel.PatientName);

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + DateTime.UtcNow.ToString("MM/dd/yy"));

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            if (patientHRAReportModel.PatientIndividualReportModel.Count > 0)
            firstPageRow.Cells[0].AddParagraph(patientHRAReportModel.PatientIndividualReportModel[0].CompletionDate != null ? "Assessment Completion Date: " + patientHRAReportModel.PatientIndividualReportModel[0].CompletionDate.Value.ToString("MM/dd/yy") : "Assessment Completion Date: - ");
            section.AddPageBreak();

            //Individual Report Table of Contents
            Table toctable = section.AddTable();
            Column tocColumn = toctable.AddColumn();
            tocColumn.Format.Alignment = ParagraphAlignment.Left;

            tocColumn = toctable.AddColumn();
            tocColumn.Format.Alignment = ParagraphAlignment.Left;

            //tocColumn = toctable.AddColumn();
            //tocColumn.Format.Alignment = ParagraphAlignment.Left;

            //toctable.Columns[0].Width = 110;
            toctable.Columns[0].Width = 225;
            toctable.Columns[1].Width = 225;


            Row tocRow = toctable.AddRow();
            tocRow.Cells[0].MergeRight = 1;
            tocRow.Cells[0].AddParagraph("Table of Contents").Format.Font.Size = 22;
            tocRow.Format.Alignment = ParagraphAlignment.Center;
            tocRow.Format.Font.Bold = true;

            tocRow = toctable.AddRow();
            tocRow = toctable.AddRow();
            var tocIndex = 0;
            foreach (var obj in patientHRAReportModel.PatientIndividualReportModel)
            {

                if (tocIndex == 5)
                {
                    tocRow = toctable.AddRow();
                    tocRow.BottomPadding = 5;
                    tocRow.TopPadding = 5;
                    tocRow.Format.Font.Size = 14;
                    tocRow.Format.Alignment = ParagraphAlignment.Right;
                    tocRow.Cells[0].AddParagraph(patientHRAReportModel.PatientBMISectionModel.Category + "\t").Format.Alignment = ParagraphAlignment.Left;
                    tocRow.Cells[1].AddParagraph().AddPageRefField(patientHRAReportModel.PatientBMISectionModel.Category);
                }
                //else
                //{
                tocRow = toctable.AddRow();
                tocRow.BottomPadding = 5;
                tocRow.TopPadding = 5;
                tocRow.Format.Font.Size = 14;
                tocRow.Format.Alignment = ParagraphAlignment.Right;
                tocRow.Cells[0].AddParagraph(obj.Category + "\t").Format.Alignment = ParagraphAlignment.Left;
                tocRow.Cells[1].AddParagraph().AddPageRefField(obj.Category);
                //}
                tocIndex++;
            }
            //if (patientHRAReportModel.VitalDetailModelForReport.Count > 0 || patientHRAReportModel.LabDetailModelForReport.Count > 0)
            //{
            //    tocRow = toctable.AddRow();
            //    tocRow.BottomPadding = 5;
            //    tocRow.TopPadding = 5;
            //    tocRow.Format.Font.Size = 14;
            //    tocRow.Format.Alignment = ParagraphAlignment.Right;
            //    tocRow.Cells[0].AddParagraph("Biometrics\t").Format.Alignment = ParagraphAlignment.Left; ;
            //    tocRow.Cells[1].AddParagraph().AddPageRefField("Biometrics");
            //}

            section.AddPageBreak();

            Paragraph headerParagraph = section.AddParagraph();

            headerParagraph.AddText("Tele CMS");
            headerParagraph.AddLineBreak();
            headerParagraph.AddText("Health Risk Assessment");
            //headerParagraph.Format.Font.Name = "Arial";
            headerParagraph.Format.Font.Size = 18;
            headerParagraph.Format.Alignment = ParagraphAlignment.Center;
            headerParagraph.Format.Font.Bold = true;
            // var htmlContent = String.Format("<body>Hello world: {0}</body>", DateTime.Now);
            // var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter(); 
            //var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);
            Paragraph emptyHeaderPara = section.AddParagraph();

            //Paragraph bmiReportDetailEmptyParagraph = section.AddParagraph();


            var index = 0;
            foreach (var obj in patientHRAReportModel.PatientIndividualReportModel)
            {
                if (index == 5 && patientHRAReportModel.PatientBMISectionModel != null)
                    AddBMIToIndividualReport(patientHRAReportModel, section);
                Paragraph hraHeaderParagraph = section.AddParagraph();
                hraHeaderParagraph.AddText(obj.Category);
                hraHeaderParagraph.AddBookmark(obj.Category);
                hraHeaderParagraph.Format.Font.Bold = true;
                hraHeaderParagraph.Format.Font.Size = 18;
                Paragraph hraHeaderEmptyParagraph = section.AddParagraph();

                Table hraScoreAndBenchmarkTable = section.AddTable();
                hraScoreAndBenchmarkTable.Borders.Visible = true;
                hraScoreAndBenchmarkTable.TopPadding = 10;

                Column hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
                hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;

                hraScoreAndBenchmarkTable.Format.Font.Size = 11;

                hraScoreAndBenchmarkTable.Columns[0].Width = 200;
                //hraScoreAndBenchmarkTable.Columns[0].Borders.Top.Visible = false;
                //hraScoreAndBenchmarkTable.Columns[0].Borders.Bottom.Visible = false;
                //hraScoreAndBenchmarkTable.Columns[0].Borders.Right.Visible = false;
                //hraScoreAndBenchmarkTable.Columns[0].Borders.Left.Visible = false;

                for (int i = 1; i < 101; i++)
                {
                    hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
                    hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;
                    hraScoreAndBenchmarkTable.Columns[i].Width = 2;
                }

                Row hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
                hraScoreAndBenchmarkRow.Borders.Visible = false;
                hraScoreAndBenchmarkRow.Borders.Bottom.Visible = true;
                hraScoreAndBenchmarkRow.Cells[0].AddParagraph("Your Score : " + obj.QScore + "%").Format.Font.Bold = true;
                hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 13;
                hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(obj.QScore)].AddParagraph(obj.QScore.ToString() + "%").Format.Font.Bold = true;
                hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(obj.QScore)].Format.Alignment = ParagraphAlignment.Center;
                hraScoreAndBenchmarkRow.BottomPadding = 0;
                hraScoreAndBenchmarkRow.Cells[0].Borders.Bottom.Visible = false;

                hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
                //score on 0th cell.
                //hraScoreAndBenchmarkRow.TopPadding = 0;
                hraScoreAndBenchmarkRow.Cells[0].AddParagraph(obj.Benchmark).Format.Font.Bold = true;
                hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 13;
                hraScoreAndBenchmarkRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                hraScoreAndBenchmarkRow.Borders.Visible = false;
                hraScoreAndBenchmarkRow.Cells[0].Borders.Right.Visible = true;
                hraScoreAndBenchmarkRow.Height = 2;
                var benchmarkCellHighlight = 0;
                foreach (var benchmarkRange in patientHRAReportModel.BenchmarkModel)
                {
                    switch (benchmarkRange.Name)
                    {
                        case Benchmarks.Normal_Risk:
                            //  bmiCellIndex = 1;

                            for (var i = Convert.ToInt32(benchmarkRange.MinRange); i <= Convert.ToInt32(benchmarkRange.MaxRange); i++)
                            {
                                hraScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(124, 173, 94);
                                hraScoreAndBenchmarkRow.Cells[i].Borders.Top.Visible = true;
                                hraScoreAndBenchmarkRow.Cells[i].Borders.Bottom.Visible = true;
                                hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(benchmarkRange.MaxRange)].Borders.Right.Visible = true;
                                hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(benchmarkRange.MaxRange)].Shading.Color = new MigraDoc.DocumentObjectModel.Color(124, 173, 94);//green
                                hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(benchmarkRange.MaxRange)].Borders.Top.Visible = true;
                                if (benchmarkRange.Name == obj.Benchmark && i == Convert.ToInt32(obj.QScore))
                                    benchmarkCellHighlight = i;
                            }
                            break;
                        case Benchmarks.Moderate_Risk:
                            //bmiCellIndex = 4;
                            for (var i = Convert.ToInt32(benchmarkRange.MinRange); i <= Convert.ToInt32(benchmarkRange.MaxRange); i++)
                            {
                                hraScoreAndBenchmarkRow.Cells[i].Borders.Top.Visible = true;
                                hraScoreAndBenchmarkRow.Cells[i].Borders.Bottom.Visible = true;
                                //hraScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(221, 134, 79);//orange
                                hraScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(255, 220, 107); //yellow 
                                if (benchmarkRange.Name == obj.Benchmark && i == Convert.ToInt32(obj.QScore))
                                    benchmarkCellHighlight = i;
                            }
                            break;
                        case Benchmarks.High_Risk:
                            //bmiCellIndex = 3;
                            for (var i = Convert.ToInt32(benchmarkRange.MinRange) + 1; i <= Convert.ToInt32(benchmarkRange.MaxRange); i++)
                            {
                                hraScoreAndBenchmarkRow.Cells[i].Borders.Top.Visible = true;
                                hraScoreAndBenchmarkRow.Cells[1].Borders.Left.Visible = true;
                                hraScoreAndBenchmarkRow.Cells[i].Borders.Bottom.Visible = true;
                                hraScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(198, 61, 70);//red
                                if (obj.QScore != 0 && benchmarkRange.Name == obj.Benchmark && i == Convert.ToInt32(obj.QScore))
                                { benchmarkCellHighlight = i; }
                                else if (obj.QScore == 0 && benchmarkRange.Name == obj.Benchmark && i == Convert.ToInt32(obj.QScore) + 1)
                                {
                                    benchmarkCellHighlight = 1;
                                }
                            }
                            break;
                        default:
                            // bmiCellIndex = 0;
                            break;
                    }
                }
                hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Right.Visible = true;
                hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Left.Visible = true;
                hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Width = 2;
                hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
                hraScoreAndBenchmarkRow.Borders.Visible = false;
                hraScoreAndBenchmarkRow.TopPadding = 0;
                hraScoreAndBenchmarkRow.Cells[3].AddParagraph("0%").Format.Alignment = ParagraphAlignment.Right;
                hraScoreAndBenchmarkRow.Cells[97].AddParagraph("100%").Format.Alignment = ParagraphAlignment.Left;

                //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
                //hraScoreAndBenchmarkRow.Borders.Right.Visible = false;
                //hraScoreAndBenchmarkRow.Borders.Left.Visible = false;
                //hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
                //hraScoreAndBenchmarkRow.Borders.Top.Visible = true;
                //hraScoreAndBenchmarkRow.Cells[1].MergeRight = 99;
                //hraScoreAndBenchmarkRow.Cells[1].AddParagraph(obj.Benchmark).Format.Font.Bold = true;
                //hraScoreAndBenchmarkRow.Cells[1].Format.Font.Size = 12;
                //hraScoreAndBenchmarkRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

                Paragraph emptyScoreBenchmarkPara = section.AddParagraph();
                //Category Description
                Table categoryDescriptionTable = section.AddTable();
                //riskDescriptionTable.Borders.Visible = true;
                Column categoryDescriptionColumn = categoryDescriptionTable.AddColumn();
                categoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

                categoryDescriptionTable.Format.Font.Size = 10;
                categoryDescriptionTable.Columns[0].Width = 450;

                //categoryHeaderDescRow.Cells[0].AddParagraph("About Your " + obj.Category);
                //categoryHeaderDescRow.Cells[0].Format.Font.Bold = true;
                //categoryHeaderDescRow.Cells[0].Format.Font.Size = 12;
                //categoryHeaderDescRow.Cells[0].Borders.Right.Visible = false;
                //categoryHeaderDescRow.Cells[0].Borders.Left.Visible = false;
                //categoryHeaderDescRow.Cells[0].Borders.Top.Visible = false;
                //categoryHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
                //categoryHeaderDescRow.BottomPadding = 10;
                Paragraph addDescpara = new Paragraph();
                Row categoryHeaderDescRow = categoryDescriptionTable.AddRow();

                if (obj.CategoryDescription.Contains("<strong>") && obj.CategoryDescription.Contains("<p>"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(obj.CategoryDescription);
                    var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                    var para = doc.DocumentNode.SelectNodes("p").ToList();
                    if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                    {
                        for (int z = 0; z < headers.Count; z++)
                        {
                            if (string.IsNullOrEmpty(para[z].InnerHtml))
                            {
                                addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                                addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                                addDescpara.AddLineBreak();
                                addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                                //Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                            }
                            else
                            {
                                addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                                addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                                addDescpara.AddLineBreak();
                                addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                                Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                            }

                        }
                    }
                }
                else
                {
                    addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                    addDescpara.AddText(obj.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                }

                Paragraph categoryDescriptionEmptyPara = section.AddParagraph();


                //Risk Description
                Table riskDescriptionTable = section.AddTable();
                //riskDescriptionTable.Borders.Visible = true;
                Column riskDescriptionColumn = riskDescriptionTable.AddColumn();
                riskDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

                riskDescriptionTable.Format.Font.Size = 10;
                riskDescriptionTable.Columns[0].Width = 450;

                Paragraph addRiskDescpara = new Paragraph();
                Row riskHeaderDescRow = riskDescriptionTable.AddRow();

                if (obj.RiskDescription.Contains("<strong>") && obj.RiskDescription.Contains("<p>"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(obj.RiskDescription);
                    var headersr = doc.DocumentNode.SelectNodes("strong").ToList();
                    var parar = doc.DocumentNode.SelectNodes("p").ToList();
                    if (headersr != null && headersr.Count > 0 && parar != null && parar.Count > 0 && headersr.Count == parar.Count)
                    {
                        for (int z = 0; z < headersr.Count; z++)
                        {
                            if (string.IsNullOrEmpty(parar[z].InnerHtml))
                            {
                                addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                                addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                                addRiskDescpara.AddLineBreak();
                                addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                                //Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                            }
                            else
                            {
                                addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                                addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                                addRiskDescpara.AddLineBreak();
                                addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                                Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                            }

                        }
                    }
                }
                else
                {
                    addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                    addRiskDescpara.AddText(obj.RiskDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                    //addRiskDescpara.Format.FirstLineIndent = "-0.5cm";
                }
                //riskHeaderDescRow = riskDescriptionTable.AddRow();
                //riskHeaderDescRow.Cells[0].AddParagraph(obj.RiskDescription.Replace("##"," ")).Format.Font.Size = 10;

                Paragraph riskDescriptionEmptyPara = section.AddParagraph();

                //Referral Links
                Table referralLinksTable = section.AddTable();
                Column referralLinksColumn = referralLinksTable.AddColumn();
                referralLinksColumn.Format.Alignment = ParagraphAlignment.Left;

                referralLinksTable.Format.Font.Size = 10;
                referralLinksTable.Columns[0].Width = 450;


                if (patientHRAReportModel.MasterHRACategoryRiskReferralLinksModel.Count > 0 && patientHRAReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == obj.MasterHRACategoryRiskId).Count() > 0)
                {
                    Row referralLinksRow = referralLinksTable.AddRow();
                    referralLinksRow.Cells[0].AddParagraph("Additional Information and Resources");
                    referralLinksRow.Cells[0].Format.Font.Bold = true;
                    referralLinksRow.Cells[0].Format.Font.Size = 12;
                    referralLinksRow.BottomPadding = 5;
                    var iy = 0;
                    foreach (var linkObj in patientHRAReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == obj.MasterHRACategoryRiskId))
                    {

                        referralLinksRow = referralLinksTable.AddRow();
                        referralLinksRow.Cells[0].Format.Alignment = ParagraphAlignment.Justify;
                        referralLinksRow.Cells[0].AddParagraph().AddHyperlink(linkObj.RefLink, HyperlinkType.Web);
                        referralLinksRow.Cells[0].AddParagraph(linkObj.RefLink);
                        referralLinksRow.Cells[0].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(20, 106, 179);
                        referralLinksRow.Cells[0].Format.Font.Size = 10;
                        iy++;
                    }
                }
                // if (index != patientHRAReportModel.PatientIndividualReportModel.Count - 1)
                section.AddPageBreak();
                //Paragraph reportDetailEmptyParagraph = section.AddParagraph();
                index++;
            }


            Paragraph reportDetailEmptyParagraph = section.AddParagraph();
            Paragraph labsAndVitalPara = section.AddParagraph();
            labsAndVitalPara.AddFormattedText("Current labs and vitals can be found by logging into the member portal or mobile application.", TextFormat.Bold);
            labsAndVitalPara.Format.Font.Size = 13;

            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;

        }
        public MemoryStream GenerateWHOIndividualSummaryPDF(PatientWHOReportModel patientWHOReportModel, TokenModel token, string reportName, string linkSuggestion)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
            //document.DefaultPageSetup.PageFormat = PageFormat.Letter;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;

            var style = document.Styles["Normal"];
            style.Font.Name = "Neue Haas";
            style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            document.DefaultPageSetup.TopMargin = 70;
            document.DefaultPageSetup.BottomMargin = 80;
            Section section = document.AddSection();
            //Document Header
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Paragraph topHeaderParagraph = section.Headers.Primary.AddParagraph();
            topHeaderParagraph.AddText(reportName + " Individual Report");
            topHeaderParagraph.Format.Font.Size = 12;
            topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
            topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Document Footer
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 150;
            pageNumberFooterTable.Columns[1].Width = 200;
            pageNumberFooterTable.Columns[2].Width = 160;

            Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            //pageNumberFooterRow.BottomPadding = 5;
            pageNumberFooterRow.TopPadding = 15;
            pageNumberFooterRow.Cells[0].AddParagraph("Proprietary & Confidential").Format.Alignment = ParagraphAlignment.Left;
            //pageNumberFooterRow.Cells[0].Borders.DistanceFromTop = 50;
            pageNumberFooterRow.Cells[1].AddParagraph().AddPageField();
            pageNumberFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            pageNumberFooterRow.Format.Font.Size = 10;
            string path = _env.WebRootPath + "\\PDFImages\\ohc-logo-without-tag.png";
            pageNumberFooterRow.Cells[2].AddImage(path);
            pageNumberFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //First page

            Table firstPageTable = section.AddTable();

            Column firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left;

            firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left; ;

            firstPageTable.Columns[0].Width = 300;
            firstPageTable.Columns[1].Width = 150;


            firstPageTable.Format.Font.Size = 10;

            Row firstPageRow = firstPageTable.AddRow();
            string logoPath = _env.WebRootPath + "\\PDFImages\\overture-indreport.png";
            firstPageRow.Cells[0].AddImage(logoPath).Width = 300;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            //firstPageRow.Cells[1].MergeDown = 3;
            string impPath = _env.WebRootPath + "\\PDFImages\\individual-report-img.png";
            var myImage = section.Headers.FirstPage.AddImage(impPath);
            myImage.Height = "29.7cm";
            myImage.Width = "6cm";
            myImage.Left = 450;
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Cells[0].AddParagraph(reportName + " Individual Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 20;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + patientWHOReportModel.PatientDetailsModel.PatientName);

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + DateTime.UtcNow.ToString("MM/dd/yy"));

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.CompletionDate != null ? "Assessment Completion Date: " + patientWHOReportModel.PatientIndividualReportModel.CompletionDate.Value.ToString("MM/dd/yy") : "Assessment Completion Date: - ");
            section.AddPageBreak();
            if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
            {
                Paragraph headerParagraph = section.AddParagraph();

                headerParagraph.AddText("Overture Health Care");
                headerParagraph.AddLineBreak();
                headerParagraph.Format.Font.Size = 19;
                headerParagraph.Format.Alignment = ParagraphAlignment.Center;
                headerParagraph.Format.Font.Bold = true;
                Paragraph emptyHeaderPara = section.AddParagraph();


                Paragraph hraHeaderParagraph = section.AddParagraph();
                hraHeaderParagraph.AddText(patientWHOReportModel.PatientIndividualReportModel.Category.ToUpper());
                //hraHeaderParagraph.AddBookmark(patientWHOReportModel.PatientIndividualReportModel.Category);
                hraHeaderParagraph.Format.Alignment = ParagraphAlignment.Center;
                hraHeaderParagraph.Format.Font.Bold = true;
                hraHeaderParagraph.Format.Font.Size = 17;
            }


            Paragraph hraHeaderEmptyParagraph = section.AddParagraph();

            Table hraScoreAndBenchmarkTable = section.AddTable();
            hraScoreAndBenchmarkTable.Borders.Visible = true;
            hraScoreAndBenchmarkTable.TopPadding = 10;

            Column hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
            hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;

            hraScoreAndBenchmarkTable.Format.Font.Size = 11;

            hraScoreAndBenchmarkTable.Columns[0].Width = 200;

            for (int i = 1; i < 101; i++)
            {
                hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
                hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;
                hraScoreAndBenchmarkTable.Columns[i].Width = 2;
            }

            Row hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph("Your Score : " + patientWHOReportModel.PatientIndividualReportModel.QScore + "/25").Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.QScore.ToString() + "/25").Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].Format.Alignment = ParagraphAlignment.Center;
            //hraScoreAndBenchmarkRow.BottomPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Bottom.Visible = false;

            hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //score on 0th cell.
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.Benchmark).Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Right.Visible = true;
            hraScoreAndBenchmarkRow.Height = 2;

            //Percentage bar chart
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Right.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Left.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Width = 2;
            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[3].AddParagraph("0%").Format.Alignment = ParagraphAlignment.Right;
            //hraScoreAndBenchmarkRow.Cells[97].AddParagraph("100%").Format.Alignment = ParagraphAlignment.Left;

            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Right.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Left.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Top.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[1].MergeRight = 99;
            //hraScoreAndBenchmarkRow.Cells[1].AddParagraph(obj.Benchmark).Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Font.Size = 12;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            Paragraph emptyScoreBenchmarkPara = section.AddParagraph();
            //Category Description
            Table categoryDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column categoryDescriptionColumn = categoryDescriptionTable.AddColumn();
            categoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            categoryDescriptionTable.Format.Font.Size = 10;
            categoryDescriptionTable.Columns[0].Width = 450;

            //categoryHeaderDescRow.Cells[0].AddParagraph("About Your " + obj.Category);
            //categoryHeaderDescRow.Cells[0].Format.Font.Bold = true;
            //categoryHeaderDescRow.Cells[0].Format.Font.Size = 12;
            //categoryHeaderDescRow.Cells[0].Borders.Right.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Left.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Top.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
            //categoryHeaderDescRow.BottomPadding = 10;
            Paragraph addDescpara = new Paragraph();
            Row categoryHeaderDescRow = categoryDescriptionTable.AddRow();
            categoryHeaderDescRow = categoryDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription);
                var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                var para = doc.DocumentNode.SelectNodes("p").ToList();
                if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                {
                    for (int z = 0; z < headers.Count; z++)
                    {
                        if (string.IsNullOrEmpty(para[z].InnerHtml))
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                addDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
            }

            Paragraph categoryDescriptionEmptyPara = section.AddParagraph();


            //Risk Description
            Table riskDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column riskDescriptionColumn = riskDescriptionTable.AddColumn();
            riskDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            riskDescriptionTable.Format.Font.Size = 10;
            riskDescriptionTable.Columns[0].Width = 450;

            Paragraph addRiskDescpara = new Paragraph();
            Row riskHeaderDescRow = riskDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.RiskDescription);
                var headersr = doc.DocumentNode.SelectNodes("strong").ToList();
                var parar = doc.DocumentNode.SelectNodes("p").ToList();
                if (headersr != null && headersr.Count > 0 && parar != null && parar.Count > 0 && headersr.Count == parar.Count)
                {
                    for (int z = 0; z < headersr.Count; z++)
                    {
                        if (string.IsNullOrEmpty(parar[z].InnerHtml))
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                addRiskDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                //addRiskDescpara.Format.FirstLineIndent = "-0.5cm";
            }
            riskHeaderDescRow = riskDescriptionTable.AddRow();
            //riskHeaderDescRow.Cells[0].AddParagraph(obj.RiskDescription.Replace("##", " ")).Format.Font.Size = 10;

            Paragraph riskDescriptionEmptyPara = section.AddParagraph();

            //Referral Links
            Table referralLinksTable = section.AddTable();
            Column referralLinksColumn = referralLinksTable.AddColumn();
            referralLinksColumn.Format.Alignment = ParagraphAlignment.Left;

            referralLinksTable.Format.Font.Size = 10;
            referralLinksTable.Columns[0].Width = 450;


            if (patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Count > 0 && patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId).Count() > 0)
            {
                Row referralLinksRow = referralLinksTable.AddRow();
                referralLinksRow.Cells[0].AddParagraph(linkSuggestion);
                if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
                {
                    referralLinksRow.Cells[0].Format.Font.Bold = true;
                }
                referralLinksRow.Cells[0].Format.Font.Size = 12;
                referralLinksRow.BottomPadding = 5;
                var iy = 0;
                foreach (var linkObj in patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId))
                {

                    referralLinksRow = referralLinksTable.AddRow();
                    referralLinksRow.Cells[0].Format.Alignment = ParagraphAlignment.Justify;
                    referralLinksRow.Cells[0].AddParagraph().AddHyperlink(linkObj.RefLink, HyperlinkType.Web);
                    referralLinksRow.Cells[0].AddParagraph(linkObj.RefLink);
                    referralLinksRow.Cells[0].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(20, 106, 179);
                    referralLinksRow.Cells[0].Format.Font.Size = 10;
                    iy++;
                }
            }
            // if (index != patientHRAReportModel.PatientIndividualReportModel.Count - 1)
            //section.AddPageBreak();



            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }
        public MemoryStream GenerateAsthmaIndividualSummaryPDF(PatientAsthmaReportModel patientWHOReportModel, TokenModel token, string reportName, string linkSuggestion)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
            //document.DefaultPageSetup.PageFormat = PageFormat.Letter;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;

            var style = document.Styles["Normal"];
            style.Font.Name = "Neue Haas";
            style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            document.DefaultPageSetup.TopMargin = 70;
            document.DefaultPageSetup.BottomMargin = 80;
            Section section = document.AddSection();
            //Document Header
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Paragraph topHeaderParagraph = section.Headers.Primary.AddParagraph();
            topHeaderParagraph.AddText(reportName + " Individual Report");
            topHeaderParagraph.Format.Font.Size = 12;
            topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
            topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Document Footer
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 150;
            pageNumberFooterTable.Columns[1].Width = 200;
            pageNumberFooterTable.Columns[2].Width = 160;

            Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            //pageNumberFooterRow.BottomPadding = 5;
            pageNumberFooterRow.TopPadding = 15;
            pageNumberFooterRow.Cells[0].AddParagraph("Proprietary & Confidential").Format.Alignment = ParagraphAlignment.Left;
            //pageNumberFooterRow.Cells[0].Borders.DistanceFromTop = 50;
            pageNumberFooterRow.Cells[1].AddParagraph().AddPageField();
            pageNumberFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            pageNumberFooterRow.Format.Font.Size = 10;
            string path = _env.WebRootPath + "\\PDFImages\\ohc-logo-without-tag.png";
            pageNumberFooterRow.Cells[2].AddImage(path);
            pageNumberFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //First page

            Table firstPageTable = section.AddTable();

            Column firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left;

            firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left; ;

            firstPageTable.Columns[0].Width = 300;
            firstPageTable.Columns[1].Width = 150;


            firstPageTable.Format.Font.Size = 10;

            Row firstPageRow = firstPageTable.AddRow();
            string logoPath = _env.WebRootPath + "\\PDFImages\\overture-indreport.png";
            firstPageRow.Cells[0].AddImage(logoPath).Width = 300;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            //firstPageRow.Cells[1].MergeDown = 3;
            string impPath = _env.WebRootPath + "\\PDFImages\\individual-report-img.png";
            var myImage = section.Headers.FirstPage.AddImage(impPath);
            myImage.Height = "29.7cm";
            myImage.Width = "6cm";
            myImage.Left = 450;
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Cells[0].AddParagraph(reportName + " Individual Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 20;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + patientWHOReportModel.PatientDetailsModel.PatientName);

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + DateTime.UtcNow.ToString("MM/dd/yy"));

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.CompletionDate != null ? "Assessment Completion Date: " + patientWHOReportModel.PatientIndividualReportModel.CompletionDate.Value.ToString("MM/dd/yy") : "Assessment Completion Date: - ");
            section.AddPageBreak();
            if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
            {
                Paragraph headerParagraph = section.AddParagraph();

                headerParagraph.AddText("Overture Health Care");
                headerParagraph.AddLineBreak();
                headerParagraph.Format.Font.Size = 19;
                headerParagraph.Format.Alignment = ParagraphAlignment.Center;
                headerParagraph.Format.Font.Bold = true;
                Paragraph emptyHeaderPara = section.AddParagraph();


                Paragraph hraHeaderParagraph = section.AddParagraph();
                hraHeaderParagraph.AddText(patientWHOReportModel.PatientIndividualReportModel.Category.ToUpper());
                //hraHeaderParagraph.AddBookmark(patientWHOReportModel.PatientIndividualReportModel.Category);
                hraHeaderParagraph.Format.Alignment = ParagraphAlignment.Center;
                hraHeaderParagraph.Format.Font.Bold = true;
                hraHeaderParagraph.Format.Font.Size = 17;
            }


            Paragraph hraHeaderEmptyParagraph = section.AddParagraph();

            Table hraScoreAndBenchmarkTable = section.AddTable();
            hraScoreAndBenchmarkTable.Borders.Visible = true;
            hraScoreAndBenchmarkTable.TopPadding = 10;

            Column hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
            hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;

            hraScoreAndBenchmarkTable.Format.Font.Size = 11;

            hraScoreAndBenchmarkTable.Columns[0].Width = 200;

            for (int i = 1; i < 101; i++)
            {
                hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
                hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;
                hraScoreAndBenchmarkTable.Columns[i].Width = 2;
            }

            Row hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph("Your Score : " + patientWHOReportModel.PatientIndividualReportModel.QScore + "/25").Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.QScore.ToString() + "/25").Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].Format.Alignment = ParagraphAlignment.Center;
            //hraScoreAndBenchmarkRow.BottomPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Bottom.Visible = false;

            hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //score on 0th cell.
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.Benchmark).Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Right.Visible = true;
            hraScoreAndBenchmarkRow.Height = 2;

            //Percentage bar chart
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Right.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Left.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Width = 2;
            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[3].AddParagraph("0%").Format.Alignment = ParagraphAlignment.Right;
            //hraScoreAndBenchmarkRow.Cells[97].AddParagraph("100%").Format.Alignment = ParagraphAlignment.Left;

            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Right.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Left.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Top.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[1].MergeRight = 99;
            //hraScoreAndBenchmarkRow.Cells[1].AddParagraph(obj.Benchmark).Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Font.Size = 12;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            Paragraph emptyScoreBenchmarkPara = section.AddParagraph();
            //Category Description
            Table categoryDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column categoryDescriptionColumn = categoryDescriptionTable.AddColumn();
            categoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            categoryDescriptionTable.Format.Font.Size = 10;
            categoryDescriptionTable.Columns[0].Width = 450;

            //categoryHeaderDescRow.Cells[0].AddParagraph("About Your " + obj.Category);
            //categoryHeaderDescRow.Cells[0].Format.Font.Bold = true;
            //categoryHeaderDescRow.Cells[0].Format.Font.Size = 12;
            //categoryHeaderDescRow.Cells[0].Borders.Right.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Left.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Top.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
            //categoryHeaderDescRow.BottomPadding = 10;
            Paragraph addDescpara = new Paragraph();
            Row categoryHeaderDescRow = categoryDescriptionTable.AddRow();
            categoryHeaderDescRow = categoryDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription);
                var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                var para = doc.DocumentNode.SelectNodes("p").ToList();
                if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                {
                    for (int z = 0; z < headers.Count; z++)
                    {
                        if (string.IsNullOrEmpty(para[z].InnerHtml))
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                addDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
            }

            Paragraph categoryDescriptionEmptyPara = section.AddParagraph();


            //Risk Description
            Table riskDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column riskDescriptionColumn = riskDescriptionTable.AddColumn();
            riskDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            riskDescriptionTable.Format.Font.Size = 10;
            riskDescriptionTable.Columns[0].Width = 450;

            Paragraph addRiskDescpara = new Paragraph();
            Row riskHeaderDescRow = riskDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.RiskDescription);
                var headersr = doc.DocumentNode.SelectNodes("strong").ToList();
                var parar = doc.DocumentNode.SelectNodes("p").ToList();
                if (headersr != null && headersr.Count > 0 && parar != null && parar.Count > 0 && headersr.Count == parar.Count)
                {
                    for (int z = 0; z < headersr.Count; z++)
                    {
                        if (string.IsNullOrEmpty(parar[z].InnerHtml))
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                addRiskDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                //addRiskDescpara.Format.FirstLineIndent = "-0.5cm";
            }
            riskHeaderDescRow = riskDescriptionTable.AddRow();
            //riskHeaderDescRow.Cells[0].AddParagraph(obj.RiskDescription.Replace("##", " ")).Format.Font.Size = 10;

            Paragraph riskDescriptionEmptyPara = section.AddParagraph();

            //Referral Links
            Table referralLinksTable = section.AddTable();
            Column referralLinksColumn = referralLinksTable.AddColumn();
            referralLinksColumn.Format.Alignment = ParagraphAlignment.Left;

            referralLinksTable.Format.Font.Size = 10;
            referralLinksTable.Columns[0].Width = 450;


            if (patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Count > 0 && patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId).Count() > 0)
            {
                Row referralLinksRow = referralLinksTable.AddRow();
                referralLinksRow.Cells[0].AddParagraph(linkSuggestion);
                if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
                {
                    referralLinksRow.Cells[0].Format.Font.Bold = true;
                }
                referralLinksRow.Cells[0].Format.Font.Size = 12;
                referralLinksRow.BottomPadding = 5;
                var iy = 0;
                foreach (var linkObj in patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId))
                {

                    referralLinksRow = referralLinksTable.AddRow();
                    referralLinksRow.Cells[0].Format.Alignment = ParagraphAlignment.Justify;
                    referralLinksRow.Cells[0].AddParagraph().AddHyperlink(linkObj.RefLink, HyperlinkType.Web);
                    referralLinksRow.Cells[0].AddParagraph(linkObj.RefLink);
                    referralLinksRow.Cells[0].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(20, 106, 179);
                    referralLinksRow.Cells[0].Format.Font.Size = 10;
                    iy++;
                }
            }
            // if (index != patientHRAReportModel.PatientIndividualReportModel.Count - 1)
            //section.AddPageBreak();



            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }
        public MemoryStream GenerateCOPDIndividualSummaryPDF(PatientCOPDReportModel patientWHOReportModel, TokenModel token, string reportName, string linkSuggestion)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
            //document.DefaultPageSetup.PageFormat = PageFormat.Letter;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;

            var style = document.Styles["Normal"];
            style.Font.Name = "Neue Haas";
            style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            document.DefaultPageSetup.TopMargin = 70;
            document.DefaultPageSetup.BottomMargin = 80;
            Section section = document.AddSection();
            //Document Header
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Paragraph topHeaderParagraph = section.Headers.Primary.AddParagraph();
            topHeaderParagraph.AddText(reportName + " Individual Report");
            topHeaderParagraph.Format.Font.Size = 12;
            topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
            topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Document Footer
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 150;
            pageNumberFooterTable.Columns[1].Width = 200;
            pageNumberFooterTable.Columns[2].Width = 160;

            Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            //pageNumberFooterRow.BottomPadding = 5;
            pageNumberFooterRow.TopPadding = 15;
            pageNumberFooterRow.Cells[0].AddParagraph("Proprietary & Confidential").Format.Alignment = ParagraphAlignment.Left;
            //pageNumberFooterRow.Cells[0].Borders.DistanceFromTop = 50;
            pageNumberFooterRow.Cells[1].AddParagraph().AddPageField();
            pageNumberFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            pageNumberFooterRow.Format.Font.Size = 10;
            string path = _env.WebRootPath + "\\PDFImages\\ohc-logo-without-tag.png";
            pageNumberFooterRow.Cells[2].AddImage(path);
            pageNumberFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //First page

            Table firstPageTable = section.AddTable();

            Column firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left;

            firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left; ;

            firstPageTable.Columns[0].Width = 300;
            firstPageTable.Columns[1].Width = 150;


            firstPageTable.Format.Font.Size = 10;

            Row firstPageRow = firstPageTable.AddRow();
            string logoPath = _env.WebRootPath + "\\PDFImages\\overture-indreport.png";
            firstPageRow.Cells[0].AddImage(logoPath).Width = 300;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            //firstPageRow.Cells[1].MergeDown = 3;
            string impPath = _env.WebRootPath + "\\PDFImages\\individual-report-img.png";
            var myImage = section.Headers.FirstPage.AddImage(impPath);
            myImage.Height = "29.7cm";
            myImage.Width = "6cm";
            myImage.Left = 450;
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Cells[0].AddParagraph(reportName + " Individual Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 20;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + patientWHOReportModel.PatientDetailsModel.PatientName);

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + DateTime.UtcNow.ToString("MM/dd/yy"));

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.CompletionDate != null ? "Assessment Completion Date: " + patientWHOReportModel.PatientIndividualReportModel.CompletionDate.Value.ToString("MM/dd/yy") : "Assessment Completion Date: - ");
            section.AddPageBreak();
            if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
            {
                Paragraph headerParagraph = section.AddParagraph();

                headerParagraph.AddText("Overture Health Care");
                headerParagraph.AddLineBreak();
                headerParagraph.Format.Font.Size = 19;
                headerParagraph.Format.Alignment = ParagraphAlignment.Center;
                headerParagraph.Format.Font.Bold = true;
                Paragraph emptyHeaderPara = section.AddParagraph();


                Paragraph hraHeaderParagraph = section.AddParagraph();
                hraHeaderParagraph.AddText(patientWHOReportModel.PatientIndividualReportModel.Category.ToUpper());
                //hraHeaderParagraph.AddBookmark(patientWHOReportModel.PatientIndividualReportModel.Category);
                hraHeaderParagraph.Format.Alignment = ParagraphAlignment.Center;
                hraHeaderParagraph.Format.Font.Bold = true;
                hraHeaderParagraph.Format.Font.Size = 17;
            }


            Paragraph hraHeaderEmptyParagraph = section.AddParagraph();

            Table hraScoreAndBenchmarkTable = section.AddTable();
            hraScoreAndBenchmarkTable.Borders.Visible = true;
            hraScoreAndBenchmarkTable.TopPadding = 10;

            Column hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
            hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;

            hraScoreAndBenchmarkTable.Format.Font.Size = 11;

            hraScoreAndBenchmarkTable.Columns[0].Width = 200;

            for (int i = 1; i < 101; i++)
            {
                hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
                hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;
                hraScoreAndBenchmarkTable.Columns[i].Width = 2;
            }

            Row hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph("Your Score : " + patientWHOReportModel.PatientIndividualReportModel.QScore + "/25").Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.QScore.ToString() + "/25").Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].Format.Alignment = ParagraphAlignment.Center;
            //hraScoreAndBenchmarkRow.BottomPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Bottom.Visible = false;

            hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //score on 0th cell.
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.Benchmark).Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Right.Visible = true;
            hraScoreAndBenchmarkRow.Height = 2;

            //Percentage bar chart
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Right.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Left.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Width = 2;
            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[3].AddParagraph("0%").Format.Alignment = ParagraphAlignment.Right;
            //hraScoreAndBenchmarkRow.Cells[97].AddParagraph("100%").Format.Alignment = ParagraphAlignment.Left;

            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Right.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Left.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Top.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[1].MergeRight = 99;
            //hraScoreAndBenchmarkRow.Cells[1].AddParagraph(obj.Benchmark).Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Font.Size = 12;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            Paragraph emptyScoreBenchmarkPara = section.AddParagraph();
            //Category Description
            Table categoryDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column categoryDescriptionColumn = categoryDescriptionTable.AddColumn();
            categoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            categoryDescriptionTable.Format.Font.Size = 10;
            categoryDescriptionTable.Columns[0].Width = 450;

            //categoryHeaderDescRow.Cells[0].AddParagraph("About Your " + obj.Category);
            //categoryHeaderDescRow.Cells[0].Format.Font.Bold = true;
            //categoryHeaderDescRow.Cells[0].Format.Font.Size = 12;
            //categoryHeaderDescRow.Cells[0].Borders.Right.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Left.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Top.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
            //categoryHeaderDescRow.BottomPadding = 10;
            Paragraph addDescpara = new Paragraph();
            Row categoryHeaderDescRow = categoryDescriptionTable.AddRow();
            categoryHeaderDescRow = categoryDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription);
                var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                var para = doc.DocumentNode.SelectNodes("p").ToList();
                if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                {
                    for (int z = 0; z < headers.Count; z++)
                    {
                        if (string.IsNullOrEmpty(para[z].InnerHtml))
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                addDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
            }

            Paragraph categoryDescriptionEmptyPara = section.AddParagraph();


            //Risk Description
            Table riskDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column riskDescriptionColumn = riskDescriptionTable.AddColumn();
            riskDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            riskDescriptionTable.Format.Font.Size = 10;
            riskDescriptionTable.Columns[0].Width = 450;

            Paragraph addRiskDescpara = new Paragraph();
            Row riskHeaderDescRow = riskDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.RiskDescription);
                var headersr = doc.DocumentNode.SelectNodes("strong").ToList();
                var parar = doc.DocumentNode.SelectNodes("p").ToList();
                if (headersr != null && headersr.Count > 0 && parar != null && parar.Count > 0 && headersr.Count == parar.Count)
                {
                    for (int z = 0; z < headersr.Count; z++)
                    {
                        if (string.IsNullOrEmpty(parar[z].InnerHtml))
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                addRiskDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                //addRiskDescpara.Format.FirstLineIndent = "-0.5cm";
            }
            riskHeaderDescRow = riskDescriptionTable.AddRow();
            //riskHeaderDescRow.Cells[0].AddParagraph(obj.RiskDescription.Replace("##", " ")).Format.Font.Size = 10;

            Paragraph riskDescriptionEmptyPara = section.AddParagraph();

            //Referral Links
            Table referralLinksTable = section.AddTable();
            Column referralLinksColumn = referralLinksTable.AddColumn();
            referralLinksColumn.Format.Alignment = ParagraphAlignment.Left;

            referralLinksTable.Format.Font.Size = 10;
            referralLinksTable.Columns[0].Width = 450;


            if (patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Count > 0 && patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId).Count() > 0)
            {
                Row referralLinksRow = referralLinksTable.AddRow();
                referralLinksRow.Cells[0].AddParagraph(linkSuggestion);
                if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
                {
                    referralLinksRow.Cells[0].Format.Font.Bold = true;
                }
                referralLinksRow.Cells[0].Format.Font.Size = 12;
                referralLinksRow.BottomPadding = 5;
                var iy = 0;
                foreach (var linkObj in patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId))
                {

                    referralLinksRow = referralLinksTable.AddRow();
                    referralLinksRow.Cells[0].Format.Alignment = ParagraphAlignment.Justify;
                    referralLinksRow.Cells[0].AddParagraph().AddHyperlink(linkObj.RefLink, HyperlinkType.Web);
                    referralLinksRow.Cells[0].AddParagraph(linkObj.RefLink);
                    referralLinksRow.Cells[0].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(20, 106, 179);
                    referralLinksRow.Cells[0].Format.Font.Size = 10;
                    iy++;
                }
            }
            // if (index != patientHRAReportModel.PatientIndividualReportModel.Count - 1)
            //section.AddPageBreak();



            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }
        public MemoryStream GenerateDiabetesIndividualSummaryPDF(PatientDiabetesReportModel patientWHOReportModel, TokenModel token, string reportName, string linkSuggestion)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
            //document.DefaultPageSetup.PageFormat = PageFormat.Letter;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;

            var style = document.Styles["Normal"];
            style.Font.Name = "Neue Haas";
            style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            document.DefaultPageSetup.TopMargin = 70;
            document.DefaultPageSetup.BottomMargin = 80;
            Section section = document.AddSection();
            //Document Header
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Paragraph topHeaderParagraph = section.Headers.Primary.AddParagraph();
            topHeaderParagraph.AddText(reportName + " Individual Report");
            topHeaderParagraph.Format.Font.Size = 12;
            topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
            topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Document Footer
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 150;
            pageNumberFooterTable.Columns[1].Width = 200;
            pageNumberFooterTable.Columns[2].Width = 160;

            Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            //pageNumberFooterRow.BottomPadding = 5;
            pageNumberFooterRow.TopPadding = 15;
            pageNumberFooterRow.Cells[0].AddParagraph("Proprietary & Confidential").Format.Alignment = ParagraphAlignment.Left;
            //pageNumberFooterRow.Cells[0].Borders.DistanceFromTop = 50;
            pageNumberFooterRow.Cells[1].AddParagraph().AddPageField();
            pageNumberFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            pageNumberFooterRow.Format.Font.Size = 10;
            string path = _env.WebRootPath + "\\PDFImages\\ohc-logo-without-tag.png";
            pageNumberFooterRow.Cells[2].AddImage(path);
            pageNumberFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //First page

            Table firstPageTable = section.AddTable();

            Column firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left;

            firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left; ;

            firstPageTable.Columns[0].Width = 300;
            firstPageTable.Columns[1].Width = 150;


            firstPageTable.Format.Font.Size = 10;

            Row firstPageRow = firstPageTable.AddRow();
            string logoPath = _env.WebRootPath + "\\PDFImages\\overture-indreport.png";
            firstPageRow.Cells[0].AddImage(logoPath).Width = 300;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            //firstPageRow.Cells[1].MergeDown = 3;
            string impPath = _env.WebRootPath + "\\PDFImages\\individual-report-img.png";
            var myImage = section.Headers.FirstPage.AddImage(impPath);
            myImage.Height = "29.7cm";
            myImage.Width = "6cm";
            myImage.Left = 450;
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Cells[0].AddParagraph(reportName + " Individual Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 20;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + patientWHOReportModel.PatientDetailsModel.PatientName);

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + DateTime.UtcNow.ToString("MM/dd/yy"));

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            if(patientWHOReportModel.PatientIndividualReportModel != null)
            {
                firstPageRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.CompletionDate != null ? "Assessment Completion Date: " + patientWHOReportModel.PatientIndividualReportModel.CompletionDate.Value.ToString("MM/dd/yy") : "Assessment Completion Date: - ");

            }
            section.AddPageBreak();
            if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
            {
                Paragraph headerParagraph = section.AddParagraph();

                headerParagraph.AddText("Overture Health Care");
                headerParagraph.AddLineBreak();
                headerParagraph.Format.Font.Size = 19;
                headerParagraph.Format.Alignment = ParagraphAlignment.Center;
                headerParagraph.Format.Font.Bold = true;
                Paragraph emptyHeaderPara = section.AddParagraph();


                Paragraph hraHeaderParagraph = section.AddParagraph();
                hraHeaderParagraph.AddText(patientWHOReportModel.PatientIndividualReportModel.Category.ToUpper());
                //hraHeaderParagraph.AddBookmark(patientWHOReportModel.PatientIndividualReportModel.Category);
                hraHeaderParagraph.Format.Alignment = ParagraphAlignment.Center;
                hraHeaderParagraph.Format.Font.Bold = true;
                hraHeaderParagraph.Format.Font.Size = 17;
            }


            Paragraph hraHeaderEmptyParagraph = section.AddParagraph();

            Table hraScoreAndBenchmarkTable = section.AddTable();
            hraScoreAndBenchmarkTable.Borders.Visible = true;
            hraScoreAndBenchmarkTable.TopPadding = 10;

            Column hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
            hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;

            hraScoreAndBenchmarkTable.Format.Font.Size = 11;

            hraScoreAndBenchmarkTable.Columns[0].Width = 200;

            for (int i = 1; i < 101; i++)
            {
                hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
                hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;
                hraScoreAndBenchmarkTable.Columns[i].Width = 2;
            }

            Row hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph("Your Score : " + patientWHOReportModel.PatientIndividualReportModel.QScore + "/8").Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.QScore.ToString() + "/25").Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].Format.Alignment = ParagraphAlignment.Center;
            //hraScoreAndBenchmarkRow.BottomPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Bottom.Visible = false;

            hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //score on 0th cell.
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.Benchmark).Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Right.Visible = true;
            hraScoreAndBenchmarkRow.Height = 2;

            //Percentage bar chart
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Right.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Left.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Width = 2;
            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[3].AddParagraph("0%").Format.Alignment = ParagraphAlignment.Right;
            //hraScoreAndBenchmarkRow.Cells[97].AddParagraph("100%").Format.Alignment = ParagraphAlignment.Left;

            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Right.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Left.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Top.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[1].MergeRight = 99;
            //hraScoreAndBenchmarkRow.Cells[1].AddParagraph(obj.Benchmark).Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Font.Size = 12;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            Paragraph emptyScoreBenchmarkPara = section.AddParagraph();
            //Category Description
            Table categoryDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column categoryDescriptionColumn = categoryDescriptionTable.AddColumn();
            categoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            categoryDescriptionTable.Format.Font.Size = 10;
            categoryDescriptionTable.Columns[0].Width = 450;

            //categoryHeaderDescRow.Cells[0].AddParagraph("About Your " + obj.Category);
            //categoryHeaderDescRow.Cells[0].Format.Font.Bold = true;
            //categoryHeaderDescRow.Cells[0].Format.Font.Size = 12;
            //categoryHeaderDescRow.Cells[0].Borders.Right.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Left.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Top.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
            //categoryHeaderDescRow.BottomPadding = 10;
            Paragraph addDescpara = new Paragraph();
            Row categoryHeaderDescRow = categoryDescriptionTable.AddRow();
            categoryHeaderDescRow = categoryDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription);
                var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                var para = doc.DocumentNode.SelectNodes("p").ToList();
                if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                {
                    for (int z = 0; z < headers.Count; z++)
                    {
                        if (string.IsNullOrEmpty(para[z].InnerHtml))
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                addDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
            }

            Paragraph categoryDescriptionEmptyPara = section.AddParagraph();


            //Risk Description
            Table riskDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column riskDescriptionColumn = riskDescriptionTable.AddColumn();
            riskDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            riskDescriptionTable.Format.Font.Size = 10;
            riskDescriptionTable.Columns[0].Width = 450;

            Paragraph addRiskDescpara = new Paragraph();
            Row riskHeaderDescRow = riskDescriptionTable.AddRow();

            if (patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<strong>") && patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientWHOReportModel.PatientIndividualReportModel.RiskDescription);
                var headersr = doc.DocumentNode.SelectNodes("strong").ToList();
                var parar = doc.DocumentNode.SelectNodes("p").ToList();
                if (headersr != null && headersr.Count > 0 && parar != null && parar.Count > 0 && headersr.Count == parar.Count)
                {
                    for (int z = 0; z < headersr.Count; z++)
                    {
                        if (string.IsNullOrEmpty(parar[z].InnerHtml))
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                addRiskDescpara.AddText(patientWHOReportModel.PatientIndividualReportModel.RiskDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                //addRiskDescpara.Format.FirstLineIndent = "-0.5cm";
            }
            riskHeaderDescRow = riskDescriptionTable.AddRow();
            //riskHeaderDescRow.Cells[0].AddParagraph(obj.RiskDescription.Replace("##", " ")).Format.Font.Size = 10;

            Paragraph riskDescriptionEmptyPara = section.AddParagraph();

            //Referral Links
            Table referralLinksTable = section.AddTable();
            Column referralLinksColumn = referralLinksTable.AddColumn();
            referralLinksColumn.Format.Alignment = ParagraphAlignment.Left;

            referralLinksTable.Format.Font.Size = 10;
            referralLinksTable.Columns[0].Width = 450;


            if (patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Count > 0 && patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId).Count() > 0)
            {
                Row referralLinksRow = referralLinksTable.AddRow();
                referralLinksRow.Cells[0].AddParagraph(linkSuggestion);
                if (patientWHOReportModel.PatientIndividualReportModel.Category.Contains("WHO-5"))
                {
                    referralLinksRow.Cells[0].Format.Font.Bold = true;
                }
                referralLinksRow.Cells[0].Format.Font.Size = 12;
                referralLinksRow.BottomPadding = 5;
                var iy = 0;
                foreach (var linkObj in patientWHOReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientWHOReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId))
                {

                    referralLinksRow = referralLinksTable.AddRow();
                    referralLinksRow.Cells[0].Format.Alignment = ParagraphAlignment.Justify;
                    referralLinksRow.Cells[0].AddParagraph().AddHyperlink(linkObj.RefLink, HyperlinkType.Web);
                    referralLinksRow.Cells[0].AddParagraph(linkObj.RefLink);
                    referralLinksRow.Cells[0].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(20, 106, 179);
                    referralLinksRow.Cells[0].Format.Font.Size = 10;
                    iy++;
                }
            }
            // if (index != patientHRAReportModel.PatientIndividualReportModel.Count - 1)
            //section.AddPageBreak();



            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }
        public MemoryStream GenerateCardiovascularIndividualSummaryPDF(PatientCardiovascularReportModel patientCardiovascularReportModel, TokenModel token)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
            //document.DefaultPageSetup.PageFormat = PageFormat.Letter;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;

            var style = document.Styles["Normal"];
            style.Font.Name = "Neue Haas";
            style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            document.DefaultPageSetup.TopMargin = 70;
            document.DefaultPageSetup.BottomMargin = 80;
            Section section = document.AddSection();
            //Document Header
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Paragraph topHeaderParagraph = section.Headers.Primary.AddParagraph();
            topHeaderParagraph.AddText("Cardiovascular Risk Assessment Individual Report");
            topHeaderParagraph.Format.Font.Size = 12;
            topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
            topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Document Footer
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 150;
            pageNumberFooterTable.Columns[1].Width = 200;
            pageNumberFooterTable.Columns[2].Width = 160;

            Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            //pageNumberFooterRow.BottomPadding = 5;
            pageNumberFooterRow.TopPadding = 15;
            pageNumberFooterRow.Cells[0].AddParagraph("Proprietary & Confidential").Format.Alignment = ParagraphAlignment.Left;
            //pageNumberFooterRow.Cells[0].Borders.DistanceFromTop = 50;
            pageNumberFooterRow.Cells[1].AddParagraph().AddPageField();
            pageNumberFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            pageNumberFooterRow.Format.Font.Size = 10;
            string path = _env.WebRootPath + "\\PDFImages\\ohc-logo-without-tag.png";
            pageNumberFooterRow.Cells[2].AddImage(path);
            pageNumberFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //First page

            Table firstPageTable = section.AddTable();

            Column firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left;

            firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left; ;

            firstPageTable.Columns[0].Width = 300;
            firstPageTable.Columns[1].Width = 150;


            firstPageTable.Format.Font.Size = 10;

            Row firstPageRow = firstPageTable.AddRow();
            string logoPath = _env.WebRootPath + "\\PDFImages\\overture-indreport.png";
            firstPageRow.Cells[0].AddImage(logoPath).Width = 300;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            //firstPageRow.Cells[1].MergeDown = 3;
            string impPath = _env.WebRootPath + "\\PDFImages\\individual-report-img.png";
            var myImage = section.Headers.FirstPage.AddImage(impPath);
            myImage.Height = "29.7cm";
            myImage.Width = "6cm";
            myImage.Left = 450;
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Cells[0].AddParagraph("Cardiovascular Risk Assessment Individual Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 20;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + patientCardiovascularReportModel.PatientDetailsModel.PatientName);

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + DateTime.UtcNow.ToString("MM/dd/yy"));

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph(patientCardiovascularReportModel.PatientIndividualReportModel.CompletionDate != null ? "Assessment Completion Date: " + patientCardiovascularReportModel.PatientIndividualReportModel.CompletionDate.Value.ToString("MM/dd/yy") : "Assessment Completion Date: - ");
            section.AddPageBreak();

            Paragraph headerParagraph = section.AddParagraph();

            headerParagraph.AddText("Overture Health Care");
            headerParagraph.AddLineBreak();
            headerParagraph.Format.Font.Size = 19;
            headerParagraph.Format.Alignment = ParagraphAlignment.Center;
            headerParagraph.Format.Font.Bold = true;
            Paragraph emptyHeaderPara = section.AddParagraph();


            Paragraph hraHeaderParagraph = section.AddParagraph();
            hraHeaderParagraph.AddText(patientCardiovascularReportModel.PatientIndividualReportModel.Category.ToUpper());
            //hraHeaderParagraph.AddBookmark(patientWHOReportModel.PatientIndividualReportModel.Category);
            hraHeaderParagraph.Format.Alignment = ParagraphAlignment.Center;
            hraHeaderParagraph.Format.Font.Bold = true;
            hraHeaderParagraph.Format.Font.Size = 17;
            Paragraph hraHeaderEmptyParagraph = section.AddParagraph();

            Table hraScoreAndBenchmarkTable = section.AddTable();
            hraScoreAndBenchmarkTable.Borders.Visible = true;
            hraScoreAndBenchmarkTable.TopPadding = 10;

            Column hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
            hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;

            hraScoreAndBenchmarkTable.Format.Font.Size = 11;

            hraScoreAndBenchmarkTable.Columns[0].Width = 200;

            for (int i = 1; i < 101; i++)
            {
                hraHeaderAndScoreColumn = hraScoreAndBenchmarkTable.AddColumn();
                hraHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;
                hraScoreAndBenchmarkTable.Columns[i].Width = 2;
            }

            Row hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph("Your Score : " + patientCardiovascularReportModel.PatientIndividualReportModel.QScore + "/25").Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].AddParagraph(patientWHOReportModel.PatientIndividualReportModel.QScore.ToString() + "/25").Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[Convert.ToInt32(patientWHOReportModel.PatientIndividualReportModel.QScore)].Format.Alignment = ParagraphAlignment.Center;
            //hraScoreAndBenchmarkRow.BottomPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Bottom.Visible = false;

            hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //score on 0th cell.
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            hraScoreAndBenchmarkRow.Cells[0].AddParagraph(patientCardiovascularReportModel.PatientIndividualReportModel.Benchmark).Format.Font.Bold = true;
            hraScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 15;
            //hraScoreAndBenchmarkRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.Cells[0].Borders.Right.Visible = true;
            hraScoreAndBenchmarkRow.Height = 2;

            //Percentage bar chart
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Right.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Left.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[benchmarkCellHighlight].Borders.Width = 2;
            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Visible = false;
            //hraScoreAndBenchmarkRow.TopPadding = 0;
            //hraScoreAndBenchmarkRow.Cells[3].AddParagraph("0%").Format.Alignment = ParagraphAlignment.Right;
            //hraScoreAndBenchmarkRow.Cells[97].AddParagraph("100%").Format.Alignment = ParagraphAlignment.Left;

            //hraScoreAndBenchmarkRow = hraScoreAndBenchmarkTable.AddRow();
            //hraScoreAndBenchmarkRow.Borders.Right.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Left.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            //hraScoreAndBenchmarkRow.Borders.Top.Visible = true;
            //hraScoreAndBenchmarkRow.Cells[1].MergeRight = 99;
            //hraScoreAndBenchmarkRow.Cells[1].AddParagraph(obj.Benchmark).Format.Font.Bold = true;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Font.Size = 12;
            //hraScoreAndBenchmarkRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            Paragraph emptyScoreBenchmarkPara = section.AddParagraph();
            //Category Description
            Table categoryDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column categoryDescriptionColumn = categoryDescriptionTable.AddColumn();
            categoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            categoryDescriptionTable.Format.Font.Size = 10;
            categoryDescriptionTable.Columns[0].Width = 450;

            //categoryHeaderDescRow.Cells[0].AddParagraph("About Your " + obj.Category);
            //categoryHeaderDescRow.Cells[0].Format.Font.Bold = true;
            //categoryHeaderDescRow.Cells[0].Format.Font.Size = 12;
            //categoryHeaderDescRow.Cells[0].Borders.Right.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Left.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Top.Visible = false;
            //categoryHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
            //categoryHeaderDescRow.BottomPadding = 10;
            Paragraph addDescpara = new Paragraph();
            Row categoryHeaderDescRow = categoryDescriptionTable.AddRow();
            categoryHeaderDescRow = categoryDescriptionTable.AddRow();

            if (patientCardiovascularReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<strong>") && patientCardiovascularReportModel.PatientIndividualReportModel.CategoryDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientCardiovascularReportModel.PatientIndividualReportModel.CategoryDescription);
                var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                var para = doc.DocumentNode.SelectNodes("p").ToList();
                if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                {
                    for (int z = 0; z < headers.Count; z++)
                    {
                        if (string.IsNullOrEmpty(para[z].InnerHtml))
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara1 = categoryHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                addDescpara.AddText(patientCardiovascularReportModel.PatientIndividualReportModel.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
            }

            Paragraph categoryDescriptionEmptyPara = section.AddParagraph();


            //Risk Description
            Table riskDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column riskDescriptionColumn = riskDescriptionTable.AddColumn();
            riskDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            riskDescriptionTable.Format.Font.Size = 10;
            riskDescriptionTable.Columns[0].Width = 450;

            Paragraph addRiskDescpara = new Paragraph();
            Row riskHeaderDescRow = riskDescriptionTable.AddRow();

            if (patientCardiovascularReportModel.PatientIndividualReportModel.RiskDescription.Contains("<strong>") && patientCardiovascularReportModel.PatientIndividualReportModel.RiskDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientCardiovascularReportModel.PatientIndividualReportModel.RiskDescription);
                var headersr = doc.DocumentNode.SelectNodes("strong").ToList();
                var parar = doc.DocumentNode.SelectNodes("p").ToList();
                if (headersr != null && headersr.Count > 0 && parar != null && parar.Count > 0 && headersr.Count == parar.Count)
                {
                    for (int z = 0; z < headersr.Count; z++)
                    {
                        if (string.IsNullOrEmpty(parar[z].InnerHtml))
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                            addRiskDescpara.AddFormattedText(headersr[z].InnerText, TextFormat.Bold);
                            addRiskDescpara.AddLineBreak();
                            addRiskDescpara.AddText(parar[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara = riskHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addRiskDescpara = riskHeaderDescRow.Cells[0].AddParagraph();
                addRiskDescpara.AddText(patientCardiovascularReportModel.PatientIndividualReportModel.RiskDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                addRiskDescpara.Format.FirstLineIndent = "-0.5cm";
            }
            riskHeaderDescRow = riskDescriptionTable.AddRow();
            //riskHeaderDescRow.Cells[0].AddParagraph(obj.RiskDescription.Replace("##", " ")).Format.Font.Size = 10;

            Paragraph riskDescriptionEmptyPara = section.AddParagraph();

            //Referral Links
            Table referralLinksTable = section.AddTable();
            Column referralLinksColumn = referralLinksTable.AddColumn();
            referralLinksColumn.Format.Alignment = ParagraphAlignment.Left;

            referralLinksTable.Format.Font.Size = 10;
            referralLinksTable.Columns[0].Width = 450;


            if (patientCardiovascularReportModel.MasterHRACategoryRiskReferralLinksModel.Count > 0 && patientCardiovascularReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientCardiovascularReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId).Count() > 0)
            {
                Row referralLinksRow = referralLinksTable.AddRow();
                referralLinksRow.Cells[0].AddParagraph("Additional Information and Resources");
                referralLinksRow.Cells[0].Format.Font.Bold = true;
                referralLinksRow.Cells[0].Format.Font.Size = 12;
                referralLinksRow.BottomPadding = 5;
                var iy = 0;
                foreach (var linkObj in patientCardiovascularReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientCardiovascularReportModel.PatientIndividualReportModel.MasterHRACategoryRiskId))
                {

                    referralLinksRow = referralLinksTable.AddRow();
                    referralLinksRow.Cells[0].Format.Alignment = ParagraphAlignment.Justify;
                    referralLinksRow.Cells[0].AddParagraph().AddHyperlink(linkObj.RefLink, HyperlinkType.Web);
                    referralLinksRow.Cells[0].AddParagraph(linkObj.RefLink);
                    referralLinksRow.Cells[0].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(20, 106, 179);
                    referralLinksRow.Cells[0].Format.Font.Size = 10;
                    iy++;
                }
            }
            // if (index != patientHRAReportModel.PatientIndividualReportModel.Count - 1)
            //section.AddPageBreak();



            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();

            tempStream = new MemoryStream();
            // Save the document...
            //pdf

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }

        private void AddBMIToIndividualReport(PatientHRAReportModel patientHRAReportModel, Section section)
        {
            Paragraph bmiHeaderParagraph = section.AddParagraph();
            bmiHeaderParagraph.AddText(patientHRAReportModel.PatientBMISectionModel.Category);
            bmiHeaderParagraph.AddBookmark(patientHRAReportModel.PatientBMISectionModel.Category);
            bmiHeaderParagraph.Format.Font.Bold = true;
            bmiHeaderParagraph.Format.Font.Size = 18;
            Paragraph bmiHraHeaderEmptyParagraph = section.AddParagraph();

            Table bmiScoreAndBenchmarkTable = section.AddTable();
            bmiScoreAndBenchmarkTable.Borders.Visible = true;
            bmiScoreAndBenchmarkTable.TopPadding = 10;

            Column bmiHeaderAndScoreColumn = bmiScoreAndBenchmarkTable.AddColumn();
            bmiHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;

            bmiScoreAndBenchmarkTable.Format.Font.Size = 11;

            bmiScoreAndBenchmarkTable.Columns[0].Width = 250;
            //bmiScoreAndBenchmarkTable.Columns[0].Borders.Left.Visible = false;
            //bmiScoreAndBenchmarkTable.Columns[0].Borders.Top.Visible = false;
            //bmiScoreAndBenchmarkTable.Columns[0].Borders.Bottom.Visible = false;
            //bmiScoreAndBenchmarkTable.Columns[0].Borders.Right.Visible = false;

            //for (int i = 1; i < 101; i++)
            //{
            //    bmiHeaderAndScoreColumn = bmiScoreAndBenchmarkTable.AddColumn();
            //    bmiHeaderAndScoreColumn.Format.Alignment = ParagraphAlignment.Left;
            //    bmiScoreAndBenchmarkTable.Columns[i].Width = 2;
            //}


            Row bmiScoreAndBenchmarkRow = bmiScoreAndBenchmarkTable.AddRow();
            //score on 0th cell.
            bmiScoreAndBenchmarkRow.Borders.Visible = false;
            bmiScoreAndBenchmarkRow.Cells[0].AddParagraph("Your Score : " + patientHRAReportModel.PatientBMISectionModel.BMIScore).Format.Font.Bold = true;
            bmiScoreAndBenchmarkRow.Cells[0].Format.Font.Size = 13;
            //bmiScoreAndBenchmarkRow.Cells[0].Borders.Right.Visible = true;

            // var bmiCellIndex = 0;
            //var bmiCellHighlight = 0;
            //foreach (var bmiRange in patientHRAReportModel.BMIRangeList)
            //{
            //    switch (bmiRange.Risk)
            //    {
            //        case Benchmarks.Obese:
            //            //  bmiCellIndex = 1;

            //            for (var i = Convert.ToInt32(bmiRange.MinRange); i <= Convert.ToInt32(bmiRange.MaxRange); i++)
            //            {
            //                bmiScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(198, 61, 70);
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Top.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Bottom.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[Convert.ToInt32(bmiRange.MaxRange)].Borders.Right.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[Convert.ToInt32(bmiRange.MaxRange)].Shading.Color = new MigraDoc.DocumentObjectModel.Color(198, 61, 70);
            //                bmiScoreAndBenchmarkRow.Cells[Convert.ToInt32(bmiRange.MaxRange)].Borders.Top.Visible = true;
            //                if (bmiRange.Risk == patientHRAReportModel.PatientBMISectionModel.BMIBenchmark && i == Convert.ToInt32(patientHRAReportModel.PatientBMISectionModel.BMIScore))
            //                    bmiCellHighlight = i;
            //            }
            //            break;
            //        case Benchmarks.Overweight:
            //            //bmiCellIndex = 2;
            //            for (var i = Convert.ToInt32(bmiRange.MinRange); i <= Convert.ToInt32(bmiRange.MaxRange); i++)
            //            {
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Top.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Bottom.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(255, 220, 107);
            //                if (bmiRange.Risk == patientHRAReportModel.PatientBMISectionModel.BMIBenchmark && i == Convert.ToInt32(patientHRAReportModel.PatientBMISectionModel.BMIScore))
            //                    bmiCellHighlight = i;
            //            }
            //            break;
            //        case Benchmarks.Normal:
            //            //bmiCellIndex = 4;
            //            for (var i = Convert.ToInt32(bmiRange.MinRange); i <= Convert.ToInt32(bmiRange.MaxRange); i++)
            //            {
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Top.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Bottom.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(124, 173, 94);
            //                if (bmiRange.Risk == patientHRAReportModel.PatientBMISectionModel.BMIBenchmark && i == Convert.ToInt32(patientHRAReportModel.PatientBMISectionModel.BMIScore))
            //                    bmiCellHighlight = i;
            //            }
            //            break;
            //        case Benchmarks.Underweight:
            //            //bmiCellIndex = 3;
            //            for (var i = Convert.ToInt32(bmiRange.MinRange) + 1; i <= Convert.ToInt32(bmiRange.MaxRange); i++)
            //            {
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Top.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[1].Borders.Left.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[i].Borders.Bottom.Visible = true;
            //                bmiScoreAndBenchmarkRow.Cells[i].Shading.Color = new MigraDoc.DocumentObjectModel.Color(221, 134, 79);
            //                if (bmiRange.Risk == patientHRAReportModel.PatientBMISectionModel.BMIBenchmark && i == Convert.ToInt32(patientHRAReportModel.PatientBMISectionModel.BMIScore))
            //                    bmiCellHighlight = i;
            //            }
            //            break;
            //        default:
            //            // bmiCellIndex = 0;
            //            break;
            //    }
            //}
            //bmiScoreAndBenchmarkRow.Cells[bmiCellHighlight].Borders.Right.Visible = true;
            //bmiScoreAndBenchmarkRow.Cells[bmiCellHighlight].Borders.Left.Visible = true;
            //bmiScoreAndBenchmarkRow = bmiScoreAndBenchmarkTable.AddRow();
            //bmiScoreAndBenchmarkRow.Borders.Right.Visible = false;
            //bmiScoreAndBenchmarkRow.Borders.Left.Visible = false;
            //bmiScoreAndBenchmarkRow.Borders.Bottom.Visible = false;
            //bmiScoreAndBenchmarkRow.Borders.Top.Visible = true;
            //bmiScoreAndBenchmarkRow.Cells[1].MergeRight = 99;
            //bmiScoreAndBenchmarkRow.Cells[1].AddParagraph(patientHRAReportModel.PatientBMISectionModel.BMIBenchmark != null? patientHRAReportModel.PatientBMISectionModel.BMIBenchmark:"N/A").Format.Font.Bold = true;
            //bmiScoreAndBenchmarkRow.Cells[1].Format.Font.Size = 12;
            //bmiScoreAndBenchmarkRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            Paragraph emptyBMIScoreBenchmarkPara = section.AddParagraph();
            //Category Description
            Paragraph bmiDescParagraph = section.AddParagraph();
            bmiDescParagraph.Format.Font.Size = 10;
            bmiDescParagraph.Format.Alignment = ParagraphAlignment.Left;
            //bmiDescParagraph.AddText(patientHRAReportModel.PatientBMISectionModel.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));

            if (patientHRAReportModel.PatientBMISectionModel.CategoryDescription.Contains("<strong>") && patientHRAReportModel.PatientBMISectionModel.CategoryDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientHRAReportModel.PatientBMISectionModel.CategoryDescription);
                var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                var para = doc.DocumentNode.SelectNodes("p").ToList();
                if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                {
                    for (int z = 0; z < headers.Count; z++)
                    {
                        if (string.IsNullOrEmpty(para[z].InnerHtml))
                        {
                            //addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            bmiDescParagraph.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            bmiDescParagraph.AddLineBreak();
                            bmiDescParagraph.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara2 = section.AddParagraph();
                        }
                        else
                        {
                            //addDescpara = categoryHeaderDescRow.Cells[0].AddParagraph();
                            bmiDescParagraph.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            bmiDescParagraph.AddLineBreak();
                            bmiDescParagraph.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara2 = section.AddParagraph();
                        }

                    }
                }
            }
            else
            {
                bmiDescParagraph.AddText(patientHRAReportModel.PatientBMISectionModel.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
            }


            Paragraph bmiDescEmptyParagraph = section.AddParagraph();

            Table bmiCategoryDescriptionTable = section.AddTable();
            bmiCategoryDescriptionTable.Borders.Visible = true;
            Column bmiCategoryDescriptionColumn = bmiCategoryDescriptionTable.AddColumn();
            bmiCategoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            bmiCategoryDescriptionColumn = bmiCategoryDescriptionTable.AddColumn();
            bmiCategoryDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            bmiCategoryDescriptionTable.Format.Font.Size = 10;
            bmiCategoryDescriptionTable.Columns[0].Width = 100;
            bmiCategoryDescriptionTable.Columns[1].Width = 100;

            Row bmiCategoryHeaderDescRow = bmiCategoryDescriptionTable.AddRow();
            bmiCategoryHeaderDescRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            bmiCategoryHeaderDescRow.Format.Font.Color = Colors.White;
            bmiCategoryHeaderDescRow.Cells[0].AddParagraph("BMI Class").Format.Font.Bold = true;
            bmiCategoryHeaderDescRow.Cells[1].AddParagraph("BMI Score Range").Format.Font.Bold = true;
            bmiCategoryHeaderDescRow.TopPadding = 5;
            bmiCategoryHeaderDescRow.BottomPadding = 5;
            string categoryDesc = string.Empty;
            foreach (var item in patientHRAReportModel.BMIRangeList)
            {
                bmiCategoryHeaderDescRow = bmiCategoryDescriptionTable.AddRow();
                bmiCategoryHeaderDescRow.Cells[0].AddParagraph(item.Risk).Format.Font.Size = 10;
                categoryDesc = item.Risk.ToLower() == "underweight" ? "BMI below " + item.MaxRange : item.Risk.ToLower() == "obese" ? item.MinRange + " or above" : item.MinRange + " - " + item.MaxRange;
                bmiCategoryHeaderDescRow.Cells[1].AddParagraph(categoryDesc).Format.Font.Size = 10;
                bmiCategoryHeaderDescRow.TopPadding = 5;
                bmiCategoryHeaderDescRow.BottomPadding = 5;
            }


            Paragraph bmiCategoryDescriptionEmptyPara = section.AddParagraph();

            //Risk Description
            Table bmiRiskDescriptionTable = section.AddTable();
            //riskDescriptionTable.Borders.Visible = true;
            Column bmiRiskDescriptionColumn = bmiRiskDescriptionTable.AddColumn();
            bmiRiskDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

            bmiRiskDescriptionTable.Format.Font.Size = 10;
            bmiRiskDescriptionTable.Columns[0].Width = 450;

            //bmiRiskHeaderDescRow.Cells[0].AddParagraph("Risk Description");
            //bmiRiskHeaderDescRow.Cells[0].Format.Font.Bold = true;
            //bmiRiskHeaderDescRow.Cells[0].Format.Font.Size = 12;
            ////bmiRiskHeaderDescRow.Cells[0].Borders.Right.Visible = false;
            ////bmiRiskHeaderDescRow.Cells[0].Borders.Left.Visible = false;
            ////bmiRiskHeaderDescRow.Cells[0].Borders.Top.Visible = false;
            ////bmiRiskHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
            //bmiRiskHeaderDescRow.BottomPadding = 10;

            Row bmiRiskHeaderDescRow = bmiRiskDescriptionTable.AddRow();
            Paragraph addDescpara = new Paragraph();
            if (patientHRAReportModel.PatientBMISectionModel.RiskDescription.Contains("<strong>") && patientHRAReportModel.PatientBMISectionModel.RiskDescription.Contains("<p>"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(patientHRAReportModel.PatientBMISectionModel.RiskDescription);
                var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                var para = doc.DocumentNode.SelectNodes("p").ToList();
                if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                {
                    for (int z = 0; z < headers.Count; z++)
                    {
                        if (string.IsNullOrEmpty(para[z].InnerHtml))
                        {
                            addDescpara = bmiRiskHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            //Paragraph emptyPara1 = bmiRiskHeaderDescRow.Cells[0].AddParagraph();
                        }
                        else
                        {
                            addDescpara = bmiRiskHeaderDescRow.Cells[0].AddParagraph();
                            addDescpara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            addDescpara.AddLineBreak();
                            addDescpara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            Paragraph emptyPara1 = bmiRiskHeaderDescRow.Cells[0].AddParagraph();
                        }

                    }
                }
            }
            else
            {
                addDescpara = bmiRiskHeaderDescRow.Cells[0].AddParagraph();
                addDescpara.AddText(patientHRAReportModel.PatientBMISectionModel.RiskDescription != null ? patientHRAReportModel.PatientBMISectionModel.RiskDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0") : "N/A");
            }

            //bmiRiskHeaderDescRow = bmiRiskDescriptionTable.AddRow();
            //bmiRiskHeaderDescRow.Cells[0].AddParagraph(patientHRAReportModel.PatientBMISectionModel.RiskDescription != null ? patientHRAReportModel.PatientBMISectionModel.RiskDescription:"N/A").Format.Font.Size = 10;
            //bmiRiskHeaderDescRow.TopPadding = 10;
            //bmiRiskHeaderDescRow.BottomPadding = 10;
            //bmiRiskHeaderDescRow.Cells[0].Borders.Top.Visible = true;
            //bmiRiskHeaderDescRow.Cells[0].Borders.Right.Visible = true;
            //bmiRiskHeaderDescRow.Cells[0].Borders.Left.Visible = true;
            //bmiRiskHeaderDescRow.Cells[0].Borders.Bottom.Visible = true;
            Paragraph bmiRiskDescriptionEmptyPara = section.AddParagraph();

            //Referral Links
            Table bmiReferralLinksTable = section.AddTable();
            Column bmiReferralLinksColumn = bmiReferralLinksTable.AddColumn();
            bmiReferralLinksColumn.Format.Alignment = ParagraphAlignment.Left;

            bmiReferralLinksTable.Format.Font.Size = 10;
            bmiReferralLinksTable.Columns[0].Width = 450;


            Row bmiReferralLinksRow = bmiReferralLinksTable.AddRow();
            bmiReferralLinksRow.Cells[0].AddParagraph("Additional Information and Resources");
            bmiReferralLinksRow.Cells[0].Format.Font.Bold = true;
            bmiReferralLinksRow.Cells[0].Format.Font.Size = 12;
            bmiReferralLinksRow.BottomPadding = 5;
            if (patientHRAReportModel.MasterHRACategoryRiskReferralLinksModel.Count > 0 && patientHRAReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientHRAReportModel.PatientBMISectionModel.MasterHRACategoryRiskId).Count() > 0)
            {
                var iy = 0;
                foreach (var linkObj in patientHRAReportModel.MasterHRACategoryRiskReferralLinksModel.Where(x => x.MasterHRACategoryRiskId == patientHRAReportModel.PatientBMISectionModel.MasterHRACategoryRiskId))
                {

                    bmiReferralLinksRow = bmiReferralLinksTable.AddRow();
                    bmiReferralLinksRow.Cells[0].Format.Alignment = ParagraphAlignment.Justify;
                    bmiReferralLinksRow.Cells[0].AddParagraph().AddHyperlink(linkObj.RefLink, HyperlinkType.Web);
                    bmiReferralLinksRow.Cells[0].AddParagraph(linkObj.RefLink);
                    bmiReferralLinksRow.Cells[0].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(20, 106, 179);
                    bmiReferralLinksRow.Cells[0].Format.Font.Size = 10;
                    iy++;
                }
            }
            else
            {
                bmiReferralLinksRow = bmiReferralLinksTable.AddRow();
                bmiReferralLinksRow.Cells[0].AddParagraph("NA");
                bmiReferralLinksRow.TopPadding = 10;
                bmiReferralLinksRow.BottomPadding = 10;
                bmiReferralLinksRow.Cells[0].Format.Font.Size = 10;
            }
            section.AddPageBreak();
        }

        public MemoryStream PrintExecutiveSummaryReport(DateTime fromDate, DateTime toDate, int documentId, TokenModel token)
        {
            MemoryStream ms = new MemoryStream();
            PatientExecutiveReportModel patientHRAReportModel = _patientHRARepository.PrintExecutiveSummaryReport<PatientExecutiveReportModel>(fromDate, toDate, documentId, token);
            //MemoryStream memoryStream = new MemoryStream();
            MemoryStream memoryStream = GenerateExecutiveSummaryPDF(patientHRAReportModel, fromDate, toDate, token);
            return memoryStream;
        }
        public MemoryStream GenerateExecutiveSummaryPDF(PatientExecutiveReportModel patientExecutiveReportModel, DateTime fromDate, DateTime toDate, TokenModel token)
        {
            // string text = ConvertHtml("<html><head></head><body><b>Medha Joshi</b><br><p>You ranked as moderate risk in this category.Here are a few lifestyles changes you can make to lower your risk for developing diabetes:</p><ul><li> Choose fruits, vegetables, whole grains, legumes, and low - fat milk more often than sugary foods when choosing carbohydrate - rich foods.</li><li> Eat fiber - rich foods.</li><li> Keep saturated fats to less than 7 % of total daily calories.</li><li> Eat at least 2 servings of non - fried fish per week </li><li> Limit trans fats.</li><li> Restrict cholesterol intake to less than 200 mg/day.</li><li> Reduce sodium intake to about 1, 500 mg/day or less.</li><li> Increase physical exercise(which can help lower blood sugar, improve heart health, manage weight, strengthen bones and muscles and increase energy).</li></ul><ul><li> It's important to have regular checkups.</li></ul><p> Know your A1C: the A1C test measures your estimated average blood sugar level over the past 2 to 3 months.It's like a memory of your blood sugar levels </p></body></html>");
            MemoryStream tempStream = null;
            Document document = new Document();
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;
            var style = document.Styles["Normal"];
            style.Font.Name = "Neue Haas";
            style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            document.DefaultPageSetup.TopMargin = 100;
            document.DefaultPageSetup.BottomMargin = 80;
            Section section = document.AddSection();
            //Header Logo and heading
            Table logoHeaderTable = section.Headers.Primary.AddTable();
            logoHeaderTable.Borders.Visible = false;
            Column logoHeaderColumn = logoHeaderTable.AddColumn();
            logoHeaderColumn.Format.Alignment = ParagraphAlignment.Left;

            logoHeaderColumn = logoHeaderTable.AddColumn();
            logoHeaderColumn.Format.Alignment = ParagraphAlignment.Right;

            logoHeaderTable.Format.Font.Size = 12;

            logoHeaderTable.Columns[0].Width = 225;
            logoHeaderTable.Columns[1].Width = 225;

            Row logoHeaderRow = logoHeaderTable.AddRow();
            //string path = _env.WebRootPath + "\\PDFImages\\ohc-pdf-1.jpg";
            logoHeaderRow.Cells[0].AddParagraph("");
            logoHeaderRow.Cells[1].AddParagraph("Health Risk Assessment Executive Report").Format.Font.Bold = true;
            logoHeaderRow.Cells[1].Format.Font.Size = 16;
            logoHeaderRow.Cells[1].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Footer of the document
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            Table docFooterTable = section.Footers.Primary.AddTable();
            docFooterTable.Borders.Visible = false;
            Column docFooterColumn = docFooterTable.AddColumn();
            docFooterColumn.Format.Alignment = ParagraphAlignment.Center;

            docFooterColumn = docFooterTable.AddColumn();
            docFooterColumn.Format.Alignment = ParagraphAlignment.Center;

            docFooterColumn = docFooterTable.AddColumn();
            docFooterColumn.Format.Alignment = ParagraphAlignment.Center;

            docFooterTable.Format.Font.Size = 10;

            docFooterTable.Columns[0].Width = 150;
            docFooterTable.Columns[1].Width = 200;
            docFooterTable.Columns[2].Width = 160;


            Row docFooterRow = docFooterTable.AddRow();
            docFooterRow.TopPadding = 15;
            docFooterRow.Cells[0].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].OrganizationName).Format.Alignment = ParagraphAlignment.Left;
            docFooterRow.Cells[1].AddParagraph("").AddPageField();
            docFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            //string path = _env.WebRootPath + "\\PDFImages\\ohc-pdf-1.jpg";
            string path = _env.WebRootPath + "\\PDFImages\\ohc-logo-without-tag.png";
            docFooterRow.Cells[2].AddImage(path);
            docFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //First page

            Table firstPageTable = section.AddTable();

            Column firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left;

            firstPageColumn = firstPageTable.AddColumn();
            firstPageColumn.Format.Alignment = ParagraphAlignment.Left; ;

            firstPageTable.Columns[0].Width = 300;
            firstPageTable.Columns[1].Width = 150;


            firstPageTable.Format.Font.Size = 10;

            Row firstPageRow = firstPageTable.AddRow();
            string logoPath = _env.WebRootPath + "\\PDFImages\\overture-indreport.png";
            firstPageRow.Cells[0].AddImage(logoPath).Width = 300;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            //firstPageRow.Cells[1].MergeDown = 3;
            string impPath = _env.WebRootPath + "\\PDFImages\\individual-report-img.png";
            var myImage = section.Headers.FirstPage.AddImage(impPath);
            myImage.Height = "29.7cm";
            myImage.Width = "6cm";
            myImage.Left = 450;
            myImage.RelativeVertical = RelativeVertical.Page;
            myImage.RelativeHorizontal = RelativeHorizontal.Page;
            myImage.WrapFormat.Style = WrapStyle.Through;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Cells[0].AddParagraph("Health Risk Assessment Executive Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 20;
            firstPageRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + patientExecutiveReportModel.PatientExecutiveReportDataModel[0].OrganizationName);

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Run Date: " + DateTime.UtcNow.ToString("MM/dd/yyyy"));

            firstPageRow = firstPageTable.AddRow();
            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + fromDate.ToString("MM/dd/yyyy") + " - " + toDate.ToString("MM/dd/yyyy"));

            section.AddPageBreak();

            //Table of contents
            Paragraph tocHeader = section.AddParagraph();
            tocHeader.AddText("Table of Contents");
            tocHeader.Format.Font.Size = 22;
            tocHeader.Format.Alignment = ParagraphAlignment.Center;
            tocHeader.Format.Font.Bold = true;

            Paragraph tocEmptyHeader = section.AddParagraph();
            tocEmptyHeader = section.AddParagraph();
            tocEmptyHeader = section.AddParagraph();

            Table toctable = section.AddTable();
            //toctable.Borders.Visible = true;
            Column tocColumn = toctable.AddColumn();
            tocColumn.Format.Alignment = ParagraphAlignment.Left;

            tocColumn = toctable.AddColumn();
            tocColumn.Format.Alignment = ParagraphAlignment.Left;

            //tocColumn = toctable.AddColumn();
            //tocColumn.Format.Alignment = ParagraphAlignment.Left;

            //toctable.Columns[0].Width = 110;
            toctable.Columns[0].Width = 225;
            toctable.Columns[1].Width = 225;

            Row tocRow = toctable.AddRow();
            tocRow.BottomPadding = 5;
            tocRow.TopPadding = 5;
            tocRow.Format.Font.Size = 12;
            tocRow.Cells[0].Format.LeftIndent = "0.3cm";
            tocRow.Format.Alignment = ParagraphAlignment.Center;
            tocRow.Cells[0].AddParagraph("Summary Overview" + "\t").Format.Alignment = ParagraphAlignment.Left;
            tocRow.Cells[1].AddParagraph().AddPageRefField("Summary Overview");

            tocRow = toctable.AddRow();

            tocRow = toctable.AddRow();
            tocRow.BottomPadding = 5;
            tocRow.TopPadding = 5;
            tocRow.Format.Font.Size = 12;
            tocRow.Format.Alignment = ParagraphAlignment.Center;
            tocRow.Cells[0].Format.LeftIndent = "0.3cm";
            tocRow.Cells[0].AddParagraph("Aggregate Report Overview" + "\t").Format.Alignment = ParagraphAlignment.Left;
            tocRow.Cells[1].AddParagraph().AddPageRefField("Aggregate Report Overview");

            tocRow = toctable.AddRow();

            var tocIndex = 0;
            var distinctRiskArrayForTOC = patientExecutiveReportModel.CategoryPercentage.Select(x => x.Category).Distinct().ToList();
            for (int i = 0; i < distinctRiskArrayForTOC.Count(); i++)
            {
                if (tocIndex == 5)
                {
                    tocRow = toctable.AddRow();
                    tocRow.BottomPadding = 5;
                    tocRow.TopPadding = 5;
                    tocRow.Format.Font.Size = 12;
                    tocRow.Cells[0].Format.LeftIndent = "1.0cm";
                    tocRow.Format.Alignment = ParagraphAlignment.Center;
                    tocRow.Cells[0].AddParagraph("  • " + patientExecutiveReportModel.BMICategoryPercentageModel[0].Category + "\t").Format.Alignment = ParagraphAlignment.Justify;
                    tocRow.Cells[1].AddParagraph().AddPageRefField(patientExecutiveReportModel.BMICategoryPercentageModel[0].Category);
                    //tocRow = toctable.AddRow();
                }
                //else
                //{
                tocRow = toctable.AddRow();
                tocRow.BottomPadding = 5;
                tocRow.TopPadding = 5;
                tocRow.Format.Font.Size = 12;
                tocRow.Format.Alignment = ParagraphAlignment.Center;
                tocRow.Cells[0].Format.LeftIndent = "1.0cm";
                tocRow.Cells[0].AddParagraph("  • " + distinctRiskArrayForTOC[i] + "\t").Format.Alignment = ParagraphAlignment.Justify;
                tocRow.Cells[1].AddParagraph().AddPageRefField(distinctRiskArrayForTOC[i]);
                //}
                // tocRow = toctable.AddRow();
                tocIndex++;
            }
            tocRow = toctable.AddRow();
            tocRow.BottomPadding = 5;
            tocRow.TopPadding = 5;
            tocRow.Format.Font.Size = 12;
            tocRow.Cells[0].Format.LeftIndent = "0.3cm";
            tocRow.Format.Alignment = ParagraphAlignment.Center;
            tocRow.Cells[0].AddParagraph("Individual Question Overviews" + "\t").Format.Alignment = ParagraphAlignment.Left;
            tocRow.Cells[1].AddParagraph().AddPageRefField("Individual Question Overviews");
            section.AddPageBreak();
            // first page
            Paragraph headerParagraph = section.AddParagraph();
            headerParagraph.AddText("Health Risk Assessment Executive Report");
            headerParagraph.AddLineBreak();
            //headerParagraph.Format.Font.Name = "Arial";
            headerParagraph.Format.Font.Size = 16;
            headerParagraph.Format.Alignment = ParagraphAlignment.Center;
            headerParagraph.Format.Font.Bold = true;

            Paragraph spacingPara = section.AddParagraph();

            Paragraph subHeaderParagraph = section.AddParagraph();
            subHeaderParagraph.AddText("Prepared For " + patientExecutiveReportModel.PatientExecutiveReportDataModel[0].OrganizationName);
            subHeaderParagraph.Format.Font.Size = 16;
            subHeaderParagraph.Format.Alignment = ParagraphAlignment.Center;
            subHeaderParagraph.Format.Font.Bold = true;

            Paragraph subHeadingSpacingPara = section.AddParagraph();

            Paragraph secondHeaderParagraph = section.AddParagraph();
            secondHeaderParagraph.AddText("Summary Overview");
            secondHeaderParagraph.AddBookmark("Summary Overview");

            //secondHeaderParagraph.Format.Shading.Color = new MigraDoc.DocumentObjectModel.Color(243, 242, 242);
            secondHeaderParagraph.Format.Font.Size = 16;
            secondHeaderParagraph.Format.Alignment = ParagraphAlignment.Left;
            secondHeaderParagraph.Format.Font.Bold = true;
            Paragraph spacPara = section.AddParagraph();

            Paragraph headerDescriptionParagraph = section.AddParagraph();
            headerDescriptionParagraph.AddText(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].HeaderDescription.Replace("{addspace}", "\u00A0"));
            headerDescriptionParagraph.Format.Font.Size = 12;
            headerDescriptionParagraph.Format.Alignment = ParagraphAlignment.Justify;



            Paragraph empPara = section.AddParagraph();
            //Participations Count
            Paragraph participationDetailParagraph = section.AddParagraph();
            participationDetailParagraph.AddText("Participation Statistics");
            //headerDescriptionParagraph.Format.Font.Name = "Arial";
            participationDetailParagraph.Format.Font.Size = 14;
            participationDetailParagraph.Format.Font.Bold = true;
            participationDetailParagraph.Format.Alignment = ParagraphAlignment.Left;

            Paragraph participationEmpPara = section.AddParagraph();

            Table paticipationCountTable = section.AddTable();
            paticipationCountTable.TopPadding = 5;
            paticipationCountTable.Borders.Visible = true;
            Column paticipationCountColumn = paticipationCountTable.AddColumn();
            paticipationCountColumn.Format.Alignment = ParagraphAlignment.Center;

            paticipationCountColumn = paticipationCountTable.AddColumn();
            paticipationCountColumn.Format.Alignment = ParagraphAlignment.Center;

            paticipationCountColumn = paticipationCountTable.AddColumn();
            paticipationCountColumn.Format.Alignment = ParagraphAlignment.Center;

            paticipationCountTable.Columns[0].Width = 150;
            paticipationCountTable.Columns[1].Width = 150;
            paticipationCountTable.Columns[2].Width = 150;

            Row paticipationCountRow = paticipationCountTable.AddRow();
            paticipationCountRow.BottomPadding = 5;
            paticipationCountRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
            paticipationCountRow.Cells[0].MergeRight = 2;
            paticipationCountRow.Cells[0].AddParagraph("Overall Participation").Format.Font.Bold = true;
            paticipationCountRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            paticipationCountRow.Format.Font.Size = 12;
            paticipationCountRow.BottomPadding = 5;

            paticipationCountRow = paticipationCountTable.AddRow();
            paticipationCountRow.Format.Font.Color = Colors.White;
            paticipationCountRow.Format.Font.Size = 10;
            paticipationCountRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            paticipationCountRow.Cells[0].AddParagraph("Total HRAs Assigned").Format.Font.Bold = true;
            paticipationCountRow.Cells[1].AddParagraph("Total HRAs Completed").Format.Font.Bold = true;
            paticipationCountRow.Cells[2].AddParagraph("Total Participation").Format.Font.Bold = true;
            paticipationCountRow.BottomPadding = 5;

            paticipationCountRow = paticipationCountTable.AddRow();
            paticipationCountRow.Format.Alignment = ParagraphAlignment.Center;
            paticipationCountRow.Format.Font.Size = 10;
            paticipationCountRow.BottomPadding = 5;
            paticipationCountRow.Cells[0].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].TotalHRAAssigned.ToString());
            paticipationCountRow.Cells[1].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].TotalHRACompleted);
            paticipationCountRow.Cells[2].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].PercentageParticipationInHRA.ToString() + "%");

            Paragraph empParticipationPara = section.AddParagraph();
            //Participation by age

            Table ageCountTable = section.AddTable();
            ageCountTable.TopPadding = 5;
            ageCountTable.Borders.Visible = true;
            Column ageCountColumn = ageCountTable.AddColumn();
            ageCountColumn.Format.Alignment = ParagraphAlignment.Center;

            ageCountColumn = ageCountTable.AddColumn();
            ageCountColumn.Format.Alignment = ParagraphAlignment.Center;

            ageCountColumn = ageCountTable.AddColumn();
            ageCountColumn.Format.Alignment = ParagraphAlignment.Center;

            ageCountTable.Columns[0].Width = 150;
            ageCountTable.Columns[1].Width = 150;
            ageCountTable.Columns[2].Width = 150;

            Row ageCountRow = ageCountTable.AddRow();
            ageCountRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
            ageCountRow.Cells[0].MergeRight = 2;
            ageCountRow.Cells[0].AddParagraph("Participation by Age").Format.Font.Bold = true;
            ageCountRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            ageCountRow.Format.Font.Size = 12;
            ageCountRow.BottomPadding = 5;

            ageCountRow = ageCountTable.AddRow();
            ageCountRow.Format.Font.Color = Colors.White;
            ageCountRow.Format.Font.Size = 10;
            ageCountRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            ageCountRow.Cells[0].AddParagraph("0-18 Years Old").Format.Font.Bold = true;
            ageCountRow.Cells[1].AddParagraph("19-50 Years Old").Format.Font.Bold = true;
            ageCountRow.Cells[2].AddParagraph("50+ Years Old ").Format.Font.Bold = true;
            ageCountRow.BottomPadding = 5;

            ageCountRow = ageCountTable.AddRow();
            ageCountRow.Format.Alignment = ParagraphAlignment.Center;
            ageCountRow.Format.Font.Size = 10;
            ageCountRow.Cells[0].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].InfantsAgeGroup.ToString());
            ageCountRow.Cells[1].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].AdultAgeGroup.ToString());
            ageCountRow.Cells[2].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].SeniorCitizenAgeGroup.ToString());
            ageCountRow.BottomPadding = 5;

            Paragraph ageCountEmptyPara = section.AddParagraph();
            //Participation by gender
            Table genderCountTable = section.AddTable();
            genderCountTable.TopPadding = 5;
            genderCountTable.Borders.Visible = true;
            Column genderCountColumn = genderCountTable.AddColumn();
            genderCountColumn.Format.Alignment = ParagraphAlignment.Center;

            genderCountColumn = genderCountTable.AddColumn();
            genderCountColumn.Format.Alignment = ParagraphAlignment.Center;

            genderCountTable.Columns[0].Width = 225;
            genderCountTable.Columns[1].Width = 225;

            Row genderCountRow = genderCountTable.AddRow();
            genderCountRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
            genderCountRow.Cells[0].MergeRight = 1;
            genderCountRow.Cells[0].AddParagraph("Participation by Gender").Format.Font.Bold = true;
            genderCountRow.Format.Font.Size = 12;
            genderCountRow.Format.Alignment = ParagraphAlignment.Center;
            genderCountRow.BottomPadding = 5;

            genderCountRow = genderCountTable.AddRow();
            genderCountRow.Format.Font.Color = Colors.White;
            genderCountRow.Format.Font.Size = 10;
            genderCountRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            genderCountRow.Cells[0].AddParagraph("Total Female Participation").Format.Font.Bold = true;
            genderCountRow.Cells[1].AddParagraph("Total Male Participation").Format.Font.Bold = true;
            genderCountRow.BottomPadding = 5;

            genderCountRow = genderCountTable.AddRow();
            genderCountRow.Format.Alignment = ParagraphAlignment.Center;
            genderCountRow.Format.Font.Size = 10;
            genderCountRow.BottomPadding = 5;
            genderCountRow.Cells[0].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].TotalFemaleParticipation + " Participant(s)");
            genderCountRow.Cells[1].AddParagraph(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].TotalMaleParticipation + " Participant(s)");

            Paragraph empGenderParticipationPara = section.AddParagraph();

            // Score and Benchmark summary
            section.AddPageBreak();
            Paragraph averageScoreBenchkarkParagraph = section.AddParagraph();
            averageScoreBenchkarkParagraph.AddText("Average Wellness Score: " + patientExecutiveReportModel.PatientExecutiveReportDataModel[0].AverageScore + "% - " + patientExecutiveReportModel.PatientExecutiveReportDataModel[0].Benchmark);
            averageScoreBenchkarkParagraph.AddBookmark("Average Wellness Score");
            averageScoreBenchkarkParagraph.Format.Font.Size = 16;
            averageScoreBenchkarkParagraph.Format.Alignment = ParagraphAlignment.Left;
            averageScoreBenchkarkParagraph.Format.Font.Bold = true;
            Paragraph averageScoreSpacPara = section.AddParagraph();

            Paragraph averageScoreBenchkarkDescriptionParagraph = section.AddParagraph();
            averageScoreBenchkarkDescriptionParagraph.AddText(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].BenchmarkDescription);
            averageScoreBenchkarkDescriptionParagraph.Format.Font.Size = 12;
            averageScoreBenchkarkDescriptionParagraph.Format.Alignment = ParagraphAlignment.Left;

            //Scores Description
            Paragraph emptyPara = section.AddParagraph();
            // Patient Count Table
            Table scoreDescriptionTable = section.AddTable();
            scoreDescriptionTable.TopPadding = 5;
            scoreDescriptionTable.Borders.Visible = true;
            Column scoreDescriptionColumn = scoreDescriptionTable.AddColumn();
            scoreDescriptionColumn.Format.Alignment = ParagraphAlignment.Center;

            scoreDescriptionColumn = scoreDescriptionTable.AddColumn();
            scoreDescriptionColumn.Format.Alignment = ParagraphAlignment.Center;

            scoreDescriptionColumn = scoreDescriptionTable.AddColumn();
            scoreDescriptionColumn.Format.Alignment = ParagraphAlignment.Center;

            //scoreDescriptionColumn = scoreDescriptionTable.AddColumn();
            //scoreDescriptionColumn.Format.Alignment = ParagraphAlignment.Center;
            //scoreDescriptionTable.Format.Font.Name = "Arial";

            scoreDescriptionTable.Columns[0].Width = 145;
            scoreDescriptionTable.Columns[1].Width = 160;
            scoreDescriptionTable.Columns[2].Width = 145;
            //scoreDescriptionTable.Columns[3].Width = 100;


            Row scoreDescriptionRow = scoreDescriptionTable.AddRow();
            scoreDescriptionRow.Cells[0].MergeRight = 2;
            scoreDescriptionRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
            scoreDescriptionRow.Format.Font.Size = 12;
            scoreDescriptionRow.Format.Alignment = ParagraphAlignment.Center;
            scoreDescriptionRow.Cells[0].AddParagraph("Average Member Scores").Format.Font.Bold = true;
            scoreDescriptionRow.BottomPadding = 5;

            scoreDescriptionRow = scoreDescriptionTable.AddRow();
            scoreDescriptionRow.Format.Font.Size = 12;
            scoreDescriptionRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
            scoreDescriptionRow.Format.Alignment = ParagraphAlignment.Center;
            scoreDescriptionRow.Borders.Top.Visible = true;
            scoreDescriptionRow.Borders.Bottom.Visible = false;
            scoreDescriptionRow.Borders.Right.Visible = true;
            scoreDescriptionRow.Borders.Left.Visible = true;
            scoreDescriptionRow.Format.Font.Color = Colors.White;
            scoreDescriptionRow.Cells[0].AddParagraph("Normal Risk Participants").Format.Font.Bold = true;
            scoreDescriptionRow.Cells[1].AddParagraph("Moderate Risk Participants").Format.Font.Bold = true;
            scoreDescriptionRow.Cells[2].AddParagraph("High Risk Participants").Format.Font.Bold = true;
            scoreDescriptionRow.BottomPadding = 5;

            scoreDescriptionRow = scoreDescriptionTable.AddRow();

            for (int i = 0; i < patientExecutiveReportModel.Benchmark.Count; i++)
            {
                scoreDescriptionRow.Format.Font.Size = 12;
                scoreDescriptionRow.Format.Alignment = ParagraphAlignment.Center;
                scoreDescriptionRow.Borders.Top.Visible = false;
                scoreDescriptionRow.Borders.Bottom.Visible = true;
                scoreDescriptionRow.Borders.Right.Visible = true;
                scoreDescriptionRow.Borders.Left.Visible = true;
                scoreDescriptionRow.Format.Font.Color = Colors.White;
                scoreDescriptionRow.Format.Font.Size = 10;
                scoreDescriptionRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
                scoreDescriptionRow.Cells[i].Format.Font.Bold = true;
                scoreDescriptionRow.Cells[i].AddParagraph("Scored " + patientExecutiveReportModel.Benchmark[i].MinRange + " %-" + patientExecutiveReportModel.Benchmark[i].MaxRange + "% on HRA").Format.Font.Italic = true;
                scoreDescriptionRow.BottomPadding = 5;
            }

            scoreDescriptionRow = scoreDescriptionTable.AddRow();
            scoreDescriptionRow.Format.Font.Size = 10;
            scoreDescriptionRow.Format.Alignment = ParagraphAlignment.Center;
            scoreDescriptionRow.Cells[0].AddParagraph(patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "Normal Risk") != null ? patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "Normal Risk").TotalMembers + " Participant(s)" : "0 Participant(s)");
            //scoreDescriptionRow.Cells[1].AddParagraph(patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "Doing Well") != null ? patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "Doing Well").PercentageByBenchmark + "%" : "0%");
            scoreDescriptionRow.Cells[1].AddParagraph(patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "Moderate Risk") != null ? patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "Moderate Risk").TotalMembers + " Participant(s)" : "0 Participant(s)");
            scoreDescriptionRow.Cells[2].AddParagraph(patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "High Risk") != null ? patientExecutiveReportModel.PercentageByBenchmark.Find(x => x.Benchmark == "High Risk").TotalMembers + " Participant(s)" : "0 Participant(s)");
            scoreDescriptionRow.BottomPadding = 5;

            //Paragraph emptyParagraph = section.AddParagraph();
            //Paragraph paragraphDescription = section.AddParagraph();
            ////paragraphDescription.Format.Font.Name = "Arial";
            //paragraphDescription.Format.Font.Size = 12;
            //paragraphDescription.Format.Alignment = ParagraphAlignment.Left;
            //paragraphDescription.AddText(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].AggregateRiskDescription);
            Paragraph emptyNewParagraph = section.AddParagraph();
            //Health assessment aggregate report
            section.AddPageBreak();
            Paragraph assessmentHeadingParagraph = section.AddParagraph();
            assessmentHeadingParagraph.AddText("Aggregate Report Overview");
            assessmentHeadingParagraph.AddBookmark("Aggregate Report Overview");
            assessmentHeadingParagraph.Format.Font.Size = 16;
            assessmentHeadingParagraph.Format.Alignment = ParagraphAlignment.Left;
            assessmentHeadingParagraph.Format.Font.Bold = true;
            Paragraph paraForSpace = section.AddParagraph();

            Paragraph healthAssessmentAggrPara = section.AddParagraph();
            healthAssessmentAggrPara.Format.Font.Size = 12;
            healthAssessmentAggrPara.Format.Alignment = ParagraphAlignment.Left;
            healthAssessmentAggrPara.AddText(patientExecutiveReportModel.PatientExecutiveReportDataModel[0].HealthAssessmentDescription);
            Paragraph noDescParagraph = section.AddParagraph();
            //section.AddPageBreak();

            //Paragraph overallPara = section.AddParagraph();
            //overallPara.Format.Font.Size = 14;
            ////overallPara.Format.Font.Name = "Arial";
            //overallPara.Format.Alignment = ParagraphAlignment.Center;
            //overallPara.AddText("OVERALL");
            Paragraph emptPara = section.AddParagraph();
            //Overall Risk 
            var distinctRiskArray = patientExecutiveReportModel.CategoryPercentage.Select(x => x.Category).Distinct().ToList();
            int bulletNumber = 1;
            for (int i = 0; i < distinctRiskArray.Count(); i++)
            {
                if (i == 5 && patientExecutiveReportModel.BMICategoryPercentageModel != null && patientExecutiveReportModel.BMICategoryPercentageModel.Count > 0)
                    AddBMISectionToExecutiveReport(patientExecutiveReportModel, section);
                Paragraph overallCategoryHeadingParagraph = section.AddParagraph();
                if (bulletNumber != 6 && bulletNumber < 6)
                { overallCategoryHeadingParagraph.AddText(bulletNumber + ". " + distinctRiskArray[i]); }
                else if (bulletNumber == 6 || bulletNumber > 6)
                {
                    overallCategoryHeadingParagraph.AddText(bulletNumber + 1 + ". " + distinctRiskArray[i]);
                }
                else if (bulletNumber == 6 || bulletNumber > 6 && patientExecutiveReportModel.BMICategoryPercentageModel == null && patientExecutiveReportModel.BMICategoryPercentageModel.Count == 0)
                {
                    overallCategoryHeadingParagraph.AddText(bulletNumber + ". " + distinctRiskArray[i]);
                }
                overallCategoryHeadingParagraph.AddBookmark(distinctRiskArray[i]);
                overallCategoryHeadingParagraph.Format.Font.Size = 12;
                overallCategoryHeadingParagraph.Format.Alignment = ParagraphAlignment.Left;
                overallCategoryHeadingParagraph.Format.Font.Bold = true;
                Paragraph BMIparaForSpace = section.AddParagraph();

                var categoryDesc = patientExecutiveReportModel.CategoryPercentage.Where(a => a.Category == distinctRiskArray[i]).FirstOrDefault();
                Paragraph overallCategoryDescPara = section.AddParagraph();
                overallCategoryDescPara.Format.Font.Size = 12;
                overallCategoryDescPara.Format.Alignment = ParagraphAlignment.Left;
                //overallCategoryDescPara.AddText(categoryDesc != null ? categoryDesc.CategoryDescription : "N/A");

                if (categoryDesc != null && categoryDesc.CategoryDescription.Contains("<strong>") && categoryDesc.CategoryDescription.Contains("<p>"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(categoryDesc.CategoryDescription);
                    var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                    var para = doc.DocumentNode.SelectNodes("p").ToList();
                    if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                    {
                        for (int z = 0; z < headers.Count; z++)
                        {
                            overallCategoryDescPara.Format.Font.Size = 12;
                            overallCategoryDescPara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            overallCategoryDescPara.AddLineBreak();
                            overallCategoryDescPara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            overallCategoryDescPara = section.AddParagraph();
                        }
                    }
                }
                else
                {
                    overallCategoryDescPara.Format.Font.Size = 12;
                    overallCategoryDescPara.AddText(categoryDesc != null ? categoryDesc.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0") : "N/A");
                }

                Paragraph overallCategoryDescEmptyParagraph = section.AddParagraph();
                overallCategoryDescEmptyParagraph = section.AddParagraph();
                overallCategoryDescEmptyParagraph = section.AddParagraph();

                //Table
                Table overallTable = section.AddTable();
                overallTable.TopPadding = 5;

                Column overallColumn = overallTable.AddColumn();
                overallColumn.Format.Alignment = ParagraphAlignment.Left;

                overallColumn = overallTable.AddColumn();
                overallColumn.Format.Alignment = ParagraphAlignment.Left;

                //overallColumn = overallTable.AddColumn();
                //overallColumn.Format.Alignment = ParagraphAlignment.Left;

                overallTable.Format.Font.Size = 12;

                overallTable.Columns[0].Width = 225;
                overallTable.Columns[1].Width = 225;
                //overallTable.Columns[2].Width = 250;

                Row overallRow = overallTable.AddRow();
                //overallRow.BottomPadding = 5;
                overallRow.Borders.Visible = true;
                overallRow.Format.Alignment = ParagraphAlignment.Center;
                overallRow.Cells[0].Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
                overallRow.Cells[0].AddParagraph("Risk Category");
                overallRow.Cells[1].Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
                if (distinctRiskArray[i] != null && distinctRiskArray[i] == "Women's Health")
                    overallRow.Cells[1].AddParagraph("% of Female Participants");
                else
                    overallRow.Cells[1].AddParagraph("% of Participants");
                overallRow.BottomPadding = 5;

                int index = 0;
                foreach (var bench in patientExecutiveReportModel.Benchmark)
                {
                    var perc = patientExecutiveReportModel.CategoryPercentage.Where(x => x.Benchmark == bench.Name && x.Category == distinctRiskArray[i]).FirstOrDefault();
                    overallRow = overallTable.AddRow();
                    overallRow.BottomPadding = 5;
                    overallRow.Borders.Visible = true;
                    overallRow.Cells[0].AddParagraph(bench.Name);
                    overallRow.Cells[1].AddParagraph(perc == null ? 0 + "%" : perc.CategoryPercentage + "%").Format.Alignment = ParagraphAlignment.Center;
                    index++;
                }
                Paragraph newemptyPara = section.AddParagraph();
                Table pieChartTable = section.AddTable();
                pieChartTable.TopPadding = 5;

                Column piechartColumn = pieChartTable.AddColumn();
                piechartColumn.Format.Alignment = ParagraphAlignment.Left;

                piechartColumn = pieChartTable.AddColumn();
                piechartColumn.Format.Alignment = ParagraphAlignment.Left;

                pieChartTable.Columns[0].Width = 150;
                pieChartTable.Columns[1].Width = 300;


                Row pieChartRow = pieChartTable.AddRow();
                pieChartRow.Cells[0].AddParagraph();
                Chart chart3 = new Chart();
                chart3 = pieChartRow.Cells[1].AddChart(ChartType.Pie2D);
                chart3.Format.Alignment = ParagraphAlignment.Center;
                Series pieSeries = chart3.SeriesCollection.AddSeries();
                XSeries pieXSeries = chart3.XValues.AddXSeries();
                var ix = 0;
                foreach (var bench in patientExecutiveReportModel.Benchmark)
                {
                    var test = patientExecutiveReportModel.CategoryPercentage.Where(a => a.Benchmark == bench.Name && a.Category == distinctRiskArray[i]).FirstOrDefault();
                    if (test != null)
                    {
                        pieSeries.Add(Convert.ToDouble(test.CategoryPercentage));
                    }
                    else
                    {
                        pieSeries.Add(Convert.ToDouble(0));
                    }
                    pieXSeries.Add(bench.Name + " Participants");
                    ix++;
                }

                var legend = chart3.RightArea.AddLegend();
                //pieSeries.HasDataLabel = true;
                //chart3.DataLabel.Font.Color = Colors.Black;
                //chart3.DataLabel.Type = DataLabelType.Percent;
                //chart3.DataLabel.Position = DataLabelPosition.InsideEnd;
                //chart3.PivotChart = true;
                //chart3.HasDataLabel = true;

                chart3.Width = Unit.FromCentimeter(9);
                chart3.Height = Unit.FromCentimeter(7);
                //var legend = chart3.RightArea.AddLegend();
                var elements = pieSeries.Elements.Cast<MigraDoc.DocumentObjectModel.Shapes.Charts.Point>().ToArray();
                //if (bench.Name == "Normal Risk" && test != null && test.CategoryPercentage > 0)
                elements[0].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(124, 173, 94); //green --normal risk
                                                                                                     //if (bench.Name == "Moderate Risk" && test != null && test.CategoryPercentage > 0)                                                                                     //elements[1].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(221, 134, 79);
                elements[1].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(255, 220, 107); //yellow -- moderate risk
                                                                                                      //if (bench.Name == "High Risk" && test != null && test.CategoryPercentage > 0)
                elements[2].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(198, 61, 70); // red -- high risk

                bulletNumber++;
                section.AddPageBreak();
            }

            //ANNUAL- Questionnaire Report
            Paragraph questionaireHeaderParagraph = section.AddParagraph();
            questionaireHeaderParagraph.AddText("Individual HRA Question Overviews");
            questionaireHeaderParagraph.AddBookmark("Individual Question Overviews");
            questionaireHeaderParagraph.Format.Font.Bold = true;
            questionaireHeaderParagraph.Format.Font.Size = 16;

            Paragraph emptyHeaderQuestionnairePara = section.AddParagraph();

            Paragraph questionaireDesParagraph = section.AddParagraph();
            questionaireDesParagraph.AddText("Please Note:");
            questionaireDesParagraph.Format.Font.Size = 11;

            Paragraph questionaireDes1Paragraph = section.AddParagraph();
            questionaireDes1Paragraph.AddText("Responses marked with a star * indicate the optimal answer to the HRA question.");
            questionaireDes1Paragraph.Format.Font.Size = 11;
            questionaireDes1Paragraph.Format.LeftIndent = "1.3cm";
            questionaireDes1Paragraph.Format.FirstLineIndent = "-0.5cm";
            questionaireDes1Paragraph.Format.ListInfo.ListType = ListType.BulletList1;

            Paragraph questionaireDes2Paragraph = section.AddParagraph();
            questionaireDes2Paragraph.AddText("Percentage (%) values indicate the percentage of participants who responded to and completed the HRA.");
            questionaireDes2Paragraph.Format.Font.Size = 11;
            questionaireDes2Paragraph.Format.LeftIndent = "1.3cm";
            questionaireDes2Paragraph.Format.FirstLineIndent = "-0.5cm";
            questionaireDes2Paragraph.Format.ListInfo.ListType = ListType.BulletList1;


            var categoryIndex = 0;
            var distinctCategoryNameArray = patientExecutiveReportModel.Questions.Select(x => x.SectionName).Distinct().ToList();
            foreach (var category in distinctCategoryNameArray)
            {
                Paragraph spaceParagraph = section.AddParagraph();
                Paragraph questionAnswePara = section.AddParagraph();
                questionAnswePara.Format.Font.Size = 16;
                questionAnswePara.Format.Font.Bold = true;
                questionAnswePara.Format.Alignment = ParagraphAlignment.Center;
                questionAnswePara.AddText(category);
                Paragraph spacePara = section.AddParagraph();

                for (int k = 0; k < patientExecutiveReportModel.Questions.Count; k++)
                {
                    if (patientExecutiveReportModel.Questions[k].SectionName == category)
                    {
                        Table questionnaireTable = section.AddTable();
                        questionnaireTable.TopPadding = 5;
                        questionnaireTable.Borders.Visible = false;
                        Column questionnaireColumn = questionnaireTable.AddColumn();
                        questionnaireColumn.Format.Alignment = ParagraphAlignment.Center;

                        questionnaireColumn = questionnaireTable.AddColumn();
                        questionnaireColumn.Format.Alignment = ParagraphAlignment.Center;

                        questionnaireColumn = questionnaireTable.AddColumn();
                        questionnaireColumn.Format.Alignment = ParagraphAlignment.Center;

                        questionnaireTable.Format.Font.Size = 10;
                        // questionnaireTable.Format.Font.Name = "Arial";

                        questionnaireTable.Columns[0].Width = 40;
                        questionnaireTable.Columns[1].Width = 260;
                        questionnaireTable.Columns[2].Width = 150;



                        Row questionnaireRow = questionnaireTable.AddRow();
                        questionnaireRow.Cells[0].AddParagraph("");
                        questionnaireRow.Cells[1].MergeRight = 1;
                        questionnaireRow.Cells[1].Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
                        questionnaireRow.Cells[1].AddParagraph("QUESTION " + patientExecutiveReportModel.Questions[k].DisplayOrder);
                        questionnaireRow.Cells[1].AddParagraph(patientExecutiveReportModel.Questions[k].Question);
                        questionnaireRow.Cells[1].Format.Font.Bold = true;
                        questionnaireRow.Cells[1].Format.Font.Size = 12;
                        questionnaireRow.BottomPadding = 5;
                        questionnaireRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                        int ix = 0;
                        Paragraph paraForStar = new Paragraph();
                        foreach (var options in patientExecutiveReportModel.CategoryCodeWithPercentage.Where(t => t.SectionItemId == patientExecutiveReportModel.Questions[k].Id))
                        {
                            questionnaireRow = questionnaireTable.AddRow();
                            paraForStar = questionnaireRow.Cells[0].AddParagraph();   //AddParagraph(options.IsFavorite == true ? "*" : "").Format.Alignment = ParagraphAlignment.Right;
                            paraForStar.AddFormattedText(options.IsFavorite == true ? "\u00AB" : "", new MigraDoc.DocumentObjectModel.Font("Wingdings"));
                            paraForStar.Format.Alignment = ParagraphAlignment.Right;
                            questionnaireRow.Cells[0].Format.Shading.Color = Colors.White;
                            questionnaireRow.Cells[1].AddParagraph(options.Option).Format.Alignment = ParagraphAlignment.Left;
                            questionnaireRow.Cells[2].AddParagraph(options.IndividualCategoryCodePercentage + "%").Format.Alignment = ParagraphAlignment.Center;
                            if (ix % 2 != 0)
                                questionnaireRow.Format.Shading.Color = new MigraDoc.DocumentObjectModel.Color(243, 242, 242);
                            ix++;
                        }
                        Paragraph afterQuestionAnswerPara = section.AddParagraph();
                    }

                    //Paragraph questionEmptyPara = section.AddParagraph();
                }
                if (categoryIndex != distinctCategoryNameArray.Count - 1)
                    section.AddPageBreak();
                categoryIndex++;
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

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;

        }

        private static void AddBMISectionToExecutiveReport(PatientExecutiveReportModel patientExecutiveReportModel, Section section)
        {
            // BMI  
            var distinctBMIRiskArray = patientExecutiveReportModel.BMICategoryPercentageModel.Select(x => x.Category).Distinct().ToList();
            foreach (var item in distinctBMIRiskArray)
            {
                Paragraph BMIHeadingParagraph = section.AddParagraph();
                BMIHeadingParagraph.AddText("6. " + item);
                BMIHeadingParagraph.AddBookmark(item);
                BMIHeadingParagraph.Format.Font.Size = 12;
                BMIHeadingParagraph.Format.Alignment = ParagraphAlignment.Left;
                BMIHeadingParagraph.Format.Font.Bold = true;
                Paragraph BMIparaForSpace = section.AddParagraph();

                var categoryDesc = patientExecutiveReportModel.BMICategoryPercentageModel.Where(a => a.Category == item).FirstOrDefault();
                Paragraph BMIDescPara = section.AddParagraph();
                BMIDescPara.Format.Font.Size = 12;
                BMIDescPara.Format.Alignment = ParagraphAlignment.Left;
                //BMIDescPara.AddText(categoryDesc != null ? categoryDesc.CategoryDescription : "N/A");

                if (categoryDesc != null && categoryDesc.CategoryDescription.Contains("<strong>") && categoryDesc.CategoryDescription.Contains("<p>"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(categoryDesc.CategoryDescription);
                    var headers = doc.DocumentNode.SelectNodes("strong").ToList();
                    var para = doc.DocumentNode.SelectNodes("p").ToList();
                    if (headers != null && headers.Count > 0 && para != null && para.Count > 0 && headers.Count == para.Count)
                    {
                        for (int z = 0; z < headers.Count; z++)
                        {
                            BMIDescPara.AddFormattedText(headers[z].InnerText, TextFormat.Bold);
                            BMIDescPara.AddLineBreak();
                            BMIDescPara.AddText(para[z].InnerText.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0"));
                            BMIDescPara = section.AddParagraph();
                        }
                    }
                }
                else
                {
                    BMIDescPara.AddText(categoryDesc != null ? categoryDesc.CategoryDescription.Replace("{0}", "\u000A\t\u00A0\u00A0\u00A0").Replace("{1}", "\u000A\t\t\u00A0\u00A0\u00A0") : "N/A");
                }

                Paragraph BMIDescEmptyParagraph = section.AddParagraph();
                BMIDescEmptyParagraph = section.AddParagraph();
                BMIDescEmptyParagraph = section.AddParagraph();
                //Table
                Table bmiOverallTable = section.AddTable();
                bmiOverallTable.TopPadding = 5;
                bmiOverallTable.Borders.Visible = false;

                Column bmiOverallColumn = bmiOverallTable.AddColumn();
                bmiOverallColumn.Format.Alignment = ParagraphAlignment.Left;

                bmiOverallColumn = bmiOverallTable.AddColumn();
                bmiOverallColumn.Format.Alignment = ParagraphAlignment.Left;

                //bmiOverallColumn = bmiOverallTable.AddColumn();
                //bmiOverallColumn.Format.Alignment = ParagraphAlignment.Left;

                bmiOverallTable.Format.Font.Size = 12;

                bmiOverallTable.Columns[0].Width = 225;
                bmiOverallTable.Columns[1].Width = 225;
                //bmiOverallTable.Columns[2].Width = 250;

                Row bmiOverallRow = bmiOverallTable.AddRow();
                //bmiOverallRow.Cells[0].MergeRight = 1;
                bmiOverallRow.Borders.Visible = true;
                bmiOverallRow.Format.Alignment = ParagraphAlignment.Center;
                bmiOverallRow.Cells[0].Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
                bmiOverallRow.Cells[0].AddParagraph("Risk Category");
                bmiOverallRow.Cells[1].Shading.Color = new MigraDoc.DocumentObjectModel.Color(87, 194, 234);
                bmiOverallRow.Cells[1].AddParagraph("% of Participants");
                bmiOverallRow.BottomPadding = 5;

                int index = 0;
                foreach (var bmiBench in patientExecutiveReportModel.BMIBenchmarkModel)
                {
                    var perc = patientExecutiveReportModel.BMICategoryPercentageModel.Where(x => x.Benchmark == bmiBench.Risk && x.Category == item).FirstOrDefault();
                    bmiOverallRow = bmiOverallTable.AddRow();
                    bmiOverallRow.Borders.Visible = true;
                    bmiOverallRow.BottomPadding = 5;
                    bmiOverallRow.Cells[0].AddParagraph(bmiBench.Risk);
                    bmiOverallRow.Cells[1].AddParagraph(perc == null ? 0 + "%" : perc.CategoryPercentage + "%").Format.Alignment = ParagraphAlignment.Center;
                    index++;
                }
                Paragraph bminewemptyPara = section.AddParagraph();
                Table bmiPieChartTable = section.AddTable();
                bmiPieChartTable.TopPadding = 5;
                Column bmiPiechartColumn = bmiPieChartTable.AddColumn();
                bmiPiechartColumn.Format.Alignment = ParagraphAlignment.Left;

                bmiPiechartColumn = bmiPieChartTable.AddColumn();
                bmiPiechartColumn.Format.Alignment = ParagraphAlignment.Left;

                bmiPieChartTable.Columns[0].Width = 150;
                bmiPieChartTable.Columns[1].Width = 300;
                //Row bmiPieChartRow = bmiPieChartTable.AddRow();
                //bmiPieChartRow.Cells[0].AddParagraph();
                //Chart bmiChart3 = new Chart();
                //bmiChart3 = bmiPieChartRow.Cells[1].AddChart(ChartType.Pie2D);
                //bmiChart3.Format.Alignment = ParagraphAlignment.Center;
                //bmiChart3.Left = 50;


                Row bmiPieChartRow = bmiPieChartTable.AddRow();
                bmiPieChartRow.Cells[0].AddParagraph();
                Chart bmiChart3 = new Chart();
                bmiChart3 = bmiPieChartRow.Cells[1].AddChart(ChartType.Pie2D);
                bmiChart3.Format.Alignment = ParagraphAlignment.Center;
                Series bmiPieSeries = bmiChart3.SeriesCollection.AddSeries();
                XSeries bmiPpieXSeries = bmiChart3.XValues.AddXSeries();

                var ix = 0;
                foreach (var bmiBench in patientExecutiveReportModel.BMIBenchmarkModel)
                {
                    var test = patientExecutiveReportModel.BMICategoryPercentageModel.Where(a => a.Benchmark == bmiBench.Risk && a.Category == item).FirstOrDefault();
                    bmiPieSeries.Add(Convert.ToDouble(test == null ? 0 : test.CategoryPercentage));
                    bmiPpieXSeries.Add(bmiBench.Risk + " Participants");
                    ix++;
                }
                var legend = bmiChart3.RightArea.AddLegend();
                //bmiPieSeries.HasDataLabel = true;
                //bmiChart3.DataLabel.Font.Color = Colors.Black;
                //bmiChart3.DataLabel.Type = DataLabelType.Percent;
                //bmiChart3.DataLabel.Position = DataLabelPosition.InsideEnd;
                //bmiChart3.PivotChart = true;
                //bmiChart3.HasDataLabel = true;

                bmiChart3.Width = Unit.FromCentimeter(9);
                bmiChart3.Height = Unit.FromCentimeter(7);

                var elements = bmiPieSeries.Elements.Cast<MigraDoc.DocumentObjectModel.Shapes.Charts.Point>().ToArray();
                elements[0].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(255, 220, 107);//yellow -- underweight
                elements[1].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(124, 173, 94); //green -- normal
                elements[2].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(221, 134, 79);//orange -- overweight
                elements[3].FillFormat.Color = new MigraDoc.DocumentObjectModel.Color(198, 61, 70);//red -- obese
                section.AddPageBreak();
            }
        }

        // Assessment HRA
        //public MemoryStream PrintHRAAssessment(int? patientDocumentId, int? patientId, int documentId, TokenModel token)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    HRAAssessmentModel hraAssessmentModel = _patientHRARepository.PrintAssessmentPDF<HRAAssessmentModel>(patientDocumentId, patientId, documentId, token);
        //    //MemoryStream memoryStream = new MemoryStream();
        //    MemoryStream memoryStream = GenerateHRAAssessmentPDF(hraAssessmentModel, token);
        //    return memoryStream;
        //}
        //public MemoryStream GenerateHRAAssessmentPDF(HRAAssessmentModel hraAssessmentModel, TokenModel token)
        //{
        //    // string text = ConvertHtml("<html><head></head><body><b>Medha Joshi</b><br><p>You ranked as moderate risk in this category.Here are a few lifestyles changes you can make to lower your risk for developing diabetes:</p><ul><li> Choose fruits, vegetables, whole grains, legumes, and low - fat milk more often than sugary foods when choosing carbohydrate - rich foods.</li><li> Eat fiber - rich foods.</li><li> Keep saturated fats to less than 7 % of total daily calories.</li><li> Eat at least 2 servings of non - fried fish per week </li><li> Limit trans fats.</li><li> Restrict cholesterol intake to less than 200 mg/day.</li><li> Reduce sodium intake to about 1, 500 mg/day or less.</li><li> Increase physical exercise(which can help lower blood sugar, improve heart health, manage weight, strengthen bones and muscles and increase energy).</li></ul><ul><li> It's important to have regular checkups.</li></ul><p> Know your A1C: the A1C test measures your estimated average blood sugar level over the past 2 to 3 months.It's like a memory of your blood sugar levels </p></body></html>");
        //    MemoryStream tempStream = null;
        //    Document document = new Document();
        //    document.DefaultPageSetup.Orientation = Orientation.Portrait;
        //    document.DefaultPageSetup.PageHeight = 792;
        //    document.DefaultPageSetup.PageWidth = 612;
        //    var style = document.Styles["Normal"];
        //    style.Font.Name = "Neue Haas";
        //    style.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
        //    document.DefaultPageSetup.TopMargin = 100;
        //    document.DefaultPageSetup.BottomMargin = 100;
        //    Section section = document.AddSection();
        //    //Document Header
        //    section.PageSetup.DifferentFirstPageHeaderFooter = false;
        //    Paragraph topHeaderParagraph = section.Headers.Primary.AddParagraph();
        //    topHeaderParagraph.AddText("Health Risk Assessment Form");
        //    topHeaderParagraph.Format.Font.Size = 12;
        //    topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
        //    topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

        //    //Document Footer
        //    Table pageNumberFooterTable = section.Footers.Primary.AddTable();
        //    pageNumberFooterTable.BottomPadding = 0;
        //    Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

        //    pageNumberFooterTable.Columns[0].Width = 170;
        //    pageNumberFooterTable.Columns[1].Width = 200;
        //    pageNumberFooterTable.Columns[2].Width = 180;

        //    Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
        //    pageNumberFooterRow.Cells[0].AddParagraph().Format.Alignment = ParagraphAlignment.Left;
        //    pageNumberFooterRow.Cells[1].AddParagraph().AddPageField();
        //    pageNumberFooterRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
        //    pageNumberFooterRow.Format.Font.Size = 10;
        //    string path = _env.WebRootPath + "\\PDFImages\\logo-pdf.png";
        //    pageNumberFooterRow.Cells[2].AddImage(path).Width = 150;
        //    pageNumberFooterRow.Cells[2].Format.Alignment = ParagraphAlignment.Center;

        //    //Header
        //    Paragraph headerParagraph = section.AddParagraph();
        //    headerParagraph.AddText("Overture Health Care");
        //    headerParagraph.AddLineBreak();
        //    headerParagraph.AddText("Health Risk Assessment Form");
        //    headerParagraph.Format.Font.Size = 18;
        //    headerParagraph.Format.Alignment = ParagraphAlignment.Center;
        //    headerParagraph.Format.Font.Bold = true;
        //    Paragraph emptyHeaderPara = section.AddParagraph();
        //    // Patient details
        //    if (hraAssessmentModel.PatientDetailForAssessmentModel != null)
        //    {
        //        Table patientInfoTable = section.AddTable();

        //        Column patientInfoColumn = patientInfoTable.AddColumn();
        //        patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

        //        patientInfoColumn = patientInfoTable.AddColumn();
        //        patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

        //        patientInfoColumn = patientInfoTable.AddColumn();
        //        patientInfoColumn.Format.Alignment = ParagraphAlignment.Left;

        //        patientInfoTable.Format.Font.Size = 11;

        //        patientInfoTable.Columns[0].Width = 200;
        //        patientInfoTable.Columns[1].Width = 150;
        //        patientInfoTable.Columns[2].Width = 100;

        //        MigraDoc.DocumentObjectModel.Tables.Row patientInfoRow = patientInfoTable.AddRow();

        //        Paragraph patientInfoPara = new Paragraph();

        //        patientInfoPara = patientInfoRow.Cells[0].AddParagraph();

        //        patientInfoPara.AddFormattedText("Member Name : ").Font.Bold = true;
        //        patientInfoPara.AddText(hraAssessmentModel.PatientDetailForAssessmentModel.FirstName + " " + hraAssessmentModel.PatientDetailForAssessmentModel.LastName);

        //        patientInfoPara = patientInfoRow.Cells[1].AddParagraph();

        //        patientInfoPara.AddFormattedText("Date Of Birth : ").Font.Bold = true;
        //        patientInfoPara.AddText(hraAssessmentModel.PatientDetailForAssessmentModel.DOB);

        //        patientInfoPara = patientInfoRow.Cells[2].AddParagraph();

        //        patientInfoPara.AddFormattedText("Gender : ").Font.Bold = true;
        //        patientInfoPara.AddText(hraAssessmentModel.PatientDetailForAssessmentModel.Gender);
        //    }
        //    Paragraph emptyPatientDetailPara = section.AddParagraph();
        //    //DFA_Section
        //    int sectionIndex = 0;
        //    foreach (var obj in hraAssessmentModel.SectionForPDFModel)
        //    {
        //        Table assessmentFormTable = section.AddTable();
        //        assessmentFormTable.TopPadding = 5;
        //        assessmentFormTable.Borders.Visible = false;
        //        Column assessmentFormColumn = assessmentFormTable.AddColumn();
        //        assessmentFormColumn.Format.Alignment = ParagraphAlignment.Left;

        //        assessmentFormColumn = assessmentFormTable.AddColumn();
        //        assessmentFormColumn.Format.Alignment = ParagraphAlignment.Left;

        //        assessmentFormColumn = assessmentFormTable.AddColumn();
        //        assessmentFormColumn.Format.Alignment = ParagraphAlignment.Left;

        //        assessmentFormColumn = assessmentFormTable.AddColumn();
        //        assessmentFormColumn.Format.Alignment = ParagraphAlignment.Left;

        //        assessmentFormColumn = assessmentFormTable.AddColumn();
        //        assessmentFormColumn.Format.Alignment = ParagraphAlignment.Left;

        //        assessmentFormTable.Format.Font.Size = 10;

        //        assessmentFormTable.Columns[0].Width = 50;
        //        assessmentFormTable.Columns[1].Width = 100;
        //        assessmentFormTable.Columns[2].Width = 100;
        //        assessmentFormTable.Columns[3].Width = 100;
        //        assessmentFormTable.Columns[4].Width = 100;

        //        Row assessmentFormRow = assessmentFormTable.AddRow();
        //        assessmentFormRow.Cells[0].MergeRight = 4;
        //        assessmentFormRow.Cells[0].AddParagraph(obj.SectionName).Format.Font.Bold = true;
        //        assessmentFormRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        //        assessmentFormRow.Cells[0].Format.Font.Size = 14;

        //        assessmentFormRow = assessmentFormTable.AddRow();

        //        List<SectionItemAnswerForPDFModel> answersArray = null;
        //        //Section Item/Questions
        //        foreach (var questions in hraAssessmentModel.SectionItemForPDFModel.Where(x => x.SectionId == obj.Id).OrderBy(x => x.DisplayOrder))
        //        {
        //            assessmentFormRow = assessmentFormTable.AddRow();
        //            assessmentFormRow.Format.Font.Size = 12;
        //            assessmentFormRow.Cells[0].AddParagraph(questions.IsMandatory == true ? "*Q." + questions.DisplayOrder + " " : "Q" + questions.DisplayOrder + " ").Format.Font.Bold = true;
        //            assessmentFormRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        //            assessmentFormRow.Cells[1].MergeRight = 3;
        //            assessmentFormRow.Cells[1].AddParagraph(questions.Question).Format.Font.Bold = true;
        //            assessmentFormRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;

        //            answersArray = hraAssessmentModel.SectionItemAnswerForPDFModel != null && hraAssessmentModel.SectionItemAnswerForPDFModel.Count > 0 ? hraAssessmentModel.SectionItemAnswerForPDFModel.Where(x => questions.Id == x.SectionItemId).ToList() : null;
        //            // Section Items Codes/Options
        //            int index = 1;
        //            if (questions.InputType.ToLower() == "radiobutton")
        //            {
        //                assessmentFormRow = assessmentFormTable.AddRow();
        //                Paragraph paraForOptions = new Paragraph();
        //                foreach (var options in hraAssessmentModel.SectionItemCodesForPDFModel.Where(x => x.CategoryId == questions.CategoryId).OrderBy(x => x.DisplayOrder))
        //                {
        //                    bool isAnswered = answersArray != null && answersArray.Count > 0 ? answersArray.Find(q => q.SectionItemId == questions.Id && q.AnswerId == options.Id) != null ? true : false : false;
        //                    if (index <= 4)
        //                    {
        //                        assessmentFormRow.Format.Font.Size = 12;
        //                        assessmentFormRow.Format.Alignment = ParagraphAlignment.Left;
        //                        paraForOptions = assessmentFormRow.Cells[index].AddParagraph();
        //                        paraForOptions.AddSpace(1);
        //                        paraForOptions.AddFormattedText(isAnswered == true ? "\u00A4" : "\u00A1", new MigraDoc.DocumentObjectModel.Font("Wingdings"));
        //                        paraForOptions.AddText(options.Option);
        //                        paraForOptions.Format.Font.Size = 10;

        //                    }
        //                    else if (index > 4)
        //                    {
        //                        if (index == 5)
        //                            assessmentFormRow = assessmentFormTable.AddRow();
        //                        int i = 1;
        //                        assessmentFormRow.Format.Font.Size = 12;
        //                        assessmentFormRow.Format.Alignment = ParagraphAlignment.Left;
        //                        paraForOptions = assessmentFormRow.Cells[i].AddParagraph();
        //                        paraForOptions.AddSpace(1);
        //                        paraForOptions.AddFormattedText(isAnswered == true ? "\u00A4" : "\u00A1", new MigraDoc.DocumentObjectModel.Font("Wingdings"));
        //                        paraForOptions.AddText(options.Option);
        //                        paraForOptions.Format.Font.Size = 10;
        //                        i++;
        //                    }
        //                    index++;
        //                }
        //            }
        //            else if (questions.InputType.ToLower() == "textbox")
        //            {
        //                var textAnswer = answersArray != null ? answersArray.Find(q => q.SectionItemId == questions.Id).TextAnswer : "";
        //                assessmentFormRow = assessmentFormTable.AddRow();
        //                assessmentFormRow.Cells[1].MergeRight = 3;
        //                assessmentFormRow.Cells[1].AddParagraph(textAnswer != "" ? textAnswer + " lbs" : "_________________").Format.Font.Size = 10;
        //                assessmentFormRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
        //            }
        //            else if (questions.InputType.ToLower() == "dropdown")
        //            {
        //                assessmentFormRow = assessmentFormTable.AddRow();
        //                var reqAnswerId = answersArray != null && answersArray.Count > 0 ? answersArray.Find(q => q.SectionItemId == questions.Id).AnswerId : null;
        //                var options = reqAnswerId != null ? hraAssessmentModel.SectionItemCodesForPDFModel.Find(x => x.CategoryId == questions.CategoryId && x.Id == reqAnswerId).Option : "";
        //                //var optionName = hraAssessmentModel.SectionItemCodesForPDFModel.Find(x => x.CategoryId == questions.CategoryId).Option;
        //                assessmentFormRow.Format.Font.Size = 12;
        //                assessmentFormRow.Format.Alignment = ParagraphAlignment.Left;

        //                assessmentFormRow.Cells[1].AddParagraph(reqAnswerId != null ? options : "_________________");
        //                assessmentFormRow.Format.Font.Size = 10;
        //            }
        //        }
        //        if (sectionIndex != hraAssessmentModel.SectionForPDFModel.Count - 1)
        //            section.AddPageBreak();
        //        sectionIndex++;
        //    }

        //    // Create a renderer for the MigraDoc document.
        //    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

        //    // Associate the MigraDoc document with a renderer
        //    pdfRenderer.Document = document;

        //    // Layout and render document to PDF
        //    pdfRenderer.RenderDocument();

        //    tempStream = new MemoryStream();
        //    // Save the document...
        //    //pdf

        //    //if (isEncrypt)
        //    //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
        //    pdfRenderer.PdfDocument.Save(tempStream, false);

        //    return tempStream;

        //}


        //public MemoryStream ExportMemberHRAassessmentToExcel(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel tokenModel)
        //{
        //    DataTable dynamiceQueryDataModel = _patientHRARepository.ExportMemberHRAassessmentToExcel(filterModelForMemberHRA, tokenModel);
        //    MemoryStream stream = null;
        //    if (dynamiceQueryDataModel != null)
        //    {
        //        stream = GenerateExcel(dynamiceQueryDataModel);
        //    }
        //    return stream;
        //}

        //private MemoryStream GenerateExcel(DataTable dynamiceQueryDataModel)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    using (var excel = new ExcelPackage())
        //    {
        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        worksheet.DefaultRowHeight = 13;

        //        worksheet.Cells[1, 1].LoadFromDataTable(dynamiceQueryDataModel, true);
        //        byte[] data = excel.GetAsByteArray();

        //        memoryStream = new MemoryStream(data);

        //    }
        //    return memoryStream;
        //}
        //public MemoryStream PrintMemberHRAPDF(FilterModelForMemberHRA filterModelForMemberHRA, TokenModel token)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    List<MemberHRAModelForPDF> memberHRAModelForPDF = _patientHRARepository.PrintMemberHRAPDF<MemberHRAModelForPDF>(filterModelForMemberHRA, token).ToList();

        //    if (memberHRAModelForPDF != null && memberHRAModelForPDF.Count() > 0)
        //    {
        //        memoryStream = GenerateMemberAssessmentListPDF(memberHRAModelForPDF, token);
        //    }
        //    return memoryStream;
        //}
        //public MemoryStream GenerateMemberAssessmentListPDF(List<MemberHRAModelForPDF> memberHRAModelForPDF, TokenModel token)
        //{
        //    MemoryStream tempStream = null;
        //    Document document = new Document();
        //    document.DefaultPageSetup.Orientation = Orientation.Portrait;
        //    document.DefaultPageSetup.PageHeight = 792;
        //    document.DefaultPageSetup.PageWidth = 612;
        //    document.DefaultPageSetup.TopMargin = 30;
        //    document.DefaultPageSetup.BottomMargin = 80;
        //    document.DefaultPageSetup.LeftMargin = 22;
        //    document.DefaultPageSetup.RightMargin = 15;

        //    //Header Section

        //    Section section = document.AddSection();
        //    //LocationModel locationModal = _locationService.GetLocationOffsets(token.LocationID, token);
        //    //Document Footer
        //    //section.PageSetup.DifferentFirstPageHeaderFooter = true;
        //    Table pageNumberFooterTable = section.Footers.Primary.AddTable();
        //    pageNumberFooterTable.BottomPadding = 0;
        //    Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

        //    //pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
        //    //pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

        //    pageNumberFooterTable.Columns[0].Width = 100;
        //    pageNumberFooterTable.Columns[1].Width = 100;
        //    pageNumberFooterTable.Columns[2].Width = 80;
        //    pageNumberFooterTable.Columns[3].Width = 100;
        //    pageNumberFooterTable.Columns[4].Width = 70;

        //    Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
        //    pageNumberFooterRow.TopPadding = 5;
        //    pageNumberFooterRow.Cells[0].AddParagraph("");
        //    pageNumberFooterRow.Cells[1].AddParagraph("");
        //    pageNumberFooterRow.Cells[2].AddParagraph("");
        //    pageNumberFooterRow.Cells[3].AddParagraph("Page ").AddPageField();
        //    pageNumberFooterRow.Cells[3].Format.Alignment = ParagraphAlignment.Left;
        //    string path = _env.WebRootPath + "\\PDFImages\\overture-updated-bottom-logo.png";
        //    pageNumberFooterRow.Cells[4].AddImage(path).Height = 35;

        //    pageNumberFooterRow.Format.Alignment = ParagraphAlignment.Right;
        //    pageNumberFooterRow.Format.Font.Size = 10;
        //    int index = 0;
        //    if (index == 0)
        //    {
        //        Paragraph topHeaderLogoParagraph = section.AddParagraph();
        //        string logoPath = _env.WebRootPath + "\\PDFImages\\overture-updated-top-logo.png";
        //        topHeaderLogoParagraph.AddImage(logoPath).Width = 200;
        //        topHeaderLogoParagraph.Format.Alignment = ParagraphAlignment.Left;
        //        index++;
        //    }

        //    Paragraph topHeaderLogoEmptyParagraph = section.AddParagraph();
        //    Paragraph topHeaderLogoEmpty1Paragraph = section.AddParagraph();

        //    Paragraph headerParagraph = section.AddParagraph();
        //    //headerParagraph.AddText("Overture Health Care");
        //    //headerParagraph.AddLineBreak();
        //    headerParagraph.AddText("Member HRA");
        //    headerParagraph.Format.Font.Name = "Arial";
        //    headerParagraph.Format.Font.Size = 15;
        //    headerParagraph.Format.Alignment = ParagraphAlignment.Left;
        //    headerParagraph.Format.Font.Bold = true;

        //    Paragraph emptyParagraph = section.AddParagraph();
        //    // Patient details
        //    Table memberHRAInfoTable = section.AddTable();
        //    memberHRAInfoTable.Borders.Visible = true;
        //    Column memberHRAInfoColumn = memberHRAInfoTable.AddColumn();
        //    memberHRAInfoColumn.Format.Alignment = ParagraphAlignment.Left;
        //    for (int ind = 0; ind < 11; ind++)
        //    {
        //        memberHRAInfoColumn = memberHRAInfoTable.AddColumn();
        //        memberHRAInfoColumn.Format.Alignment = ParagraphAlignment.Left;
        //    }

        //    memberHRAInfoTable.Format.Font.Size = 6;
        //    for (int y = 0; y < 12; y++)
        //    {
        //        memberHRAInfoTable.Columns[y].Width = 50;
        //    }

        //    Row memberHRAInfoRow = memberHRAInfoTable.AddRow();
        //    memberHRAInfoRow.HeadingFormat = true;
        //    //memberHRAInfoRow.Format.Font.Size = 10;
        //    memberHRAInfoRow.BottomPadding = 5;
        //    memberHRAInfoRow.TopPadding = 5;
        //    memberHRAInfoRow.Format.Alignment = ParagraphAlignment.Left;
        //    memberHRAInfoRow.Cells[0].AddParagraph("MEMBER NAME").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[1].AddParagraph("RELATIONSHIP").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[2].AddParagraph("ELIGIB. START DATE").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[3].AddParagraph("ELIGIB. STATUS").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[4].AddParagraph("ASSESSMENT NAME").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[5].AddParagraph("ASSIGNED DATE").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[6].AddParagraph("COMPL. DATE").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[7].AddParagraph("DUE DATE").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[8].AddParagraph("STATUS").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[9].AddParagraph("DISEASE COND.").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[10].AddParagraph("PROGRAM NAME").Format.Font.Bold = true;
        //    memberHRAInfoRow.Cells[11].AddParagraph("Upcoming Appointments").Format.Font.Bold = true;

        //    foreach (var obj in memberHRAModelForPDF)
        //    {
        //        memberHRAInfoRow = memberHRAInfoTable.AddRow();
        //        //memberHRAInfoRow.Format.Font.Size = 10;
        //        memberHRAInfoRow.BottomPadding = 5;
        //        memberHRAInfoRow.TopPadding = 5;
        //        memberHRAInfoRow.Format.Alignment = ParagraphAlignment.Left;
        //        memberHRAInfoRow.Cells[0].AddParagraph(obj.MemberName != null ? obj.MemberName : "");
        //        memberHRAInfoRow.Cells[1].AddParagraph(obj.Relationship != null ? obj.Relationship : "");
        //        memberHRAInfoRow.Cells[2].AddParagraph(obj.EligibilityStartDate != null ? obj.EligibilityStartDate : "");
        //        memberHRAInfoRow.Cells[3].AddParagraph(obj.EligibilityStatus != null ? obj.EligibilityStatus : "");
        //        memberHRAInfoRow.Cells[4].AddParagraph(obj.AssessmentName != null ? obj.AssessmentName : "");
        //        memberHRAInfoRow.Cells[5].AddParagraph(obj.AssignedDate != null ? obj.AssignedDate : "");
        //        memberHRAInfoRow.Cells[6].AddParagraph(obj.CompletionDate != null ? obj.CompletionDate : "");
        //        memberHRAInfoRow.Cells[7].AddParagraph(obj.DueDate != null ? obj.DueDate : "");
        //        memberHRAInfoRow.Cells[8].AddParagraph(obj.Status != null ? obj.Status : "");
        //        memberHRAInfoRow.Cells[9].AddParagraph(obj.DiseaseConditions != null ? obj.DiseaseConditions : "");
        //        memberHRAInfoRow.Cells[10].AddParagraph(obj.ProgramName != null ? obj.ProgramName : "");
        //        memberHRAInfoRow.Cells[11].AddParagraph(obj.NextAppointmentDate != null ? obj.NextAppointmentDate : "");
        //    }

        //    // Create a renderer for the MigraDoc document.
        //    PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

        //    // Associate the MigraDoc document with a renderer
        //    pdfRenderer.Document = document;

        //    // Layout and render document to PDF
        //    pdfRenderer.RenderDocument();

        //    tempStream = new MemoryStream();
        //    // Save the document...
        //    //pdf
        //    pdfRenderer.PdfDocument.Save(tempStream, false);

        //    return tempStream;
        //}
    }
}
