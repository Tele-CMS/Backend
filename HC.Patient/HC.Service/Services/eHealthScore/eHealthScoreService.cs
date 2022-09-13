using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.eHealthScore;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Repositories.IRepositories.eHealthScore;
using HC.Patient.Service.IServices.eHealthScore;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Patient;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
//using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.Services.eHealthScore
{
    public class eHealthScoreService : BaseService, IeHealthScoreService
    {
        JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IeHealthScoreRepository _eHealthScoreRepository;
        private readonly IHostingEnvironment _env;
        private readonly ILocationService _locationService;
        private readonly IPatientService _IPatientService;
        public eHealthScoreService(IPatientService patientService, IeHealthScoreRepository eHealthScoreRepository, IHostingEnvironment env, ILocationService locationService)
        {
            _eHealthScoreRepository = eHealthScoreRepository;
            _env = env;
            _locationService = locationService;
            _IPatientService = patientService;
        }
        public JsonModel GetMemberHealtheScoreListing(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel)
        {
            List<PatientHealtheScoreListModel> patientHealtheScoreListModel = _eHealthScoreRepository.GetMemberHealtheScoreListing<PatientHealtheScoreListModel>(filterModelForMemberHealtheScore, tokenModel).ToList();
            response = patientHealtheScoreListModel != null && patientHealtheScoreListModel.Count > 0 ?
            new JsonModel(patientHealtheScoreListModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK) :
            new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
            response.meta = new Meta(patientHealtheScoreListModel != null ? patientHealtheScoreListModel : new List<PatientHealtheScoreListModel>(), filterModelForMemberHealtheScore);
            return response;
        }

        public JsonModel AssignHealtheScoreToMember(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel)
        {
            LocationModel locationModal = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
            DateTime localDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, locationModal.TimeZoneName);
            filterModelForMemberHealtheScore.HealtheScoreDate = localDateTime;
            SQLResponseModel response = _eHealthScoreRepository.AssignHealtheScoreToMember<SQLResponseModel>(filterModelForMemberHealtheScore, tokenModel).FirstOrDefault();
            return new JsonModel(null, response.Message, response.StatusCode);
        }
        public JsonModel GetHealtheScoreDataForBulkUpdate(string patientHealtheScoreIdArray, TokenModel tokenModel)
        {
            List<PatientHealtheScoreListModel> patientHealtheScoreListModel = _eHealthScoreRepository.GetHealtheScoreDataForBulkUpdate<PatientHealtheScoreListModel>(patientHealtheScoreIdArray, tokenModel).ToList();
            response = patientHealtheScoreListModel != null && patientHealtheScoreListModel.Count > 0 ? new JsonModel(patientHealtheScoreListModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK)
                : new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
            return response;
        }
        public JsonModel GetAssignedHealtheScore(PatientFilterModel patientFilterModel, TokenModel tokenModel)
        {
            //if (!_IPatientService.IsValidUserForDataAccess(tokenModel, patientFilterModel.PatientId))
            //{
            //    return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
            //}
            List<PatientHealtheScoreModel> patientHealtheScoreModel = _eHealthScoreRepository.GetAssignedHealtheScore<PatientHealtheScoreModel>(patientFilterModel, tokenModel).ToList();
            response = patientHealtheScoreModel != null && patientHealtheScoreModel.Count > 0 ? new JsonModel(patientHealtheScoreModel, StatusMessage.FetchMessage, (int)HttpStatusCode.OK)
                : new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
            response.meta = new Meta(patientHealtheScoreModel != null ? patientHealtheScoreModel : new List<PatientHealtheScoreModel>(), patientFilterModel);

            return response;
        }
        public JsonModel BulkUpdateHealtheScore(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel)
        {
            SQLResponseModel response = _eHealthScoreRepository.BulkUpdateHealtheScore<SQLResponseModel>(filterModelForMemberHealtheScore, tokenModel).FirstOrDefault();
            return new JsonModel(null, response.Message, response.StatusCode);
        }
        public JsonModel UpdateHealtheScoreForMemberTab(PatientHealtheScoreUpdateModel patientHealtheScoreUpdateModel, TokenModel token)
        {
            PatientHealtheScore patientHealtheScore = null;

            if (patientHealtheScoreUpdateModel.Id != 0)
            {
                patientHealtheScore = _eHealthScoreRepository.Get(a => a.Id == patientHealtheScoreUpdateModel.Id && a.IsActive == true && a.IsDeleted == false);
                if (patientHealtheScore != null)
                {
                    patientHealtheScore.Status = patientHealtheScoreUpdateModel.UpdatedStatusId;
                    patientHealtheScore.CreatedBy = token.UserID;
                    patientHealtheScore.CreatedDate = DateTime.UtcNow;
                    patientHealtheScore.IsDeleted = false;
                    patientHealtheScore.IsActive = true;
                    _eHealthScoreRepository.Update(patientHealtheScore);
                    _eHealthScoreRepository.SaveChanges();
                    response = new JsonModel(new object(), StatusMessage.MemberHealtheScoreDataUpdated, (int)HttpStatusCode.OK);
                }
            }
            else
            {
                response = new JsonModel(null, StatusMessage.NoContent, (int)HttpStatusCode.NoContent);
            }

            return response;
        }
            public MemoryStream PrinteHealthScoreReport(int patientId, int patientHealtheScoreId, TokenModel token)
        {
            //if (!_IPatientService.IsValidUserForDataAccess(token, patientId))
            //{
            //    return new MemoryStream();
            //}
            MemoryStream ms = new MemoryStream();
            eHealthScoreModel eHealthScoreModel = _eHealthScoreRepository.PrinteHealthScoreReport<eHealthScoreModel>(patientId, patientHealtheScoreId, token);
            MemoryStream memoryStream = GenerateeHealthScorePDF(eHealthScoreModel, token);
            return memoryStream; ;
        }
        public MemoryStream GenerateeHealthScorePDF(eHealthScoreModel eHealthScoreModel, TokenModel token)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
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
            topHeaderParagraph.AddText("Health-e Scores Report");
            topHeaderParagraph.Format.Font.Size = 12;
            topHeaderParagraph.Format.Font.Bold = true;
            topHeaderParagraph.Format.Alignment = ParagraphAlignment.Right;
            topHeaderParagraph.Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);

            //Document Footer
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

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
            firstPageRow.Cells[0].AddParagraph("Health-e Scores Individual Report").Format.Font.Bold = true;
            firstPageRow.Cells[0].Format.Font.Size = 25;
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
            firstPageRow.Cells[0].AddParagraph("Prepared For: " + eHealthScoreModel.PatientDetailsForeHealthScoreModel.PatientName);

            firstPageRow = firstPageTable.AddRow();

            firstPageRow = firstPageTable.AddRow();
            firstPageRow.Format.Alignment = ParagraphAlignment.Center;
            firstPageRow.Format.Font.Size = 14;
            firstPageRow.Cells[0].AddParagraph("Report Date: " + DateTime.UtcNow.ToString("MM/dd/yy"));
            section.AddPageBreak();

            //eHealth Score Doc
            Paragraph eHealthScoreDescPara = section.AddParagraph();
            eHealthScoreDescPara.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.FirstDescriptionParagraph);
            eHealthScoreDescPara.Format.Alignment = ParagraphAlignment.Left;
            eHealthScoreDescPara.Format.Font.Size = 11;
            if (eHealthScoreModel.PatientDetailsForeHealthScoreModel.SecondDescriptionParagraph!="")
            {
                Paragraph emptyeHealthScoreDescPara = section.AddParagraph();  
            Paragraph conditionDescPara = section.AddParagraph();
            conditionDescPara.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.SecondDescriptionParagraph);
            conditionDescPara.Format.Alignment = ParagraphAlignment.Left;
            conditionDescPara.Format.Font.Size = 11;
            }
            if (eHealthScoreModel.PatientConditionForeHealthScoreModel.Count > 0)
            {
                if (eHealthScoreModel.PatientConditionForeHealthScoreModel[0].IsConditionShowFlag != true)
                {
                    if (eHealthScoreModel.PatientDetailsForeHealthScoreModel.ObservedConditionText != "")
                    {
                        Paragraph emptyconditionDescPara = section.AddParagraph();

                        Paragraph sourceDescPara = section.AddParagraph();
                        sourceDescPara.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.ObservedConditionText);
                        sourceDescPara.Format.Alignment = ParagraphAlignment.Left;
                        sourceDescPara.Format.Font.Size = 11;
                    }
                    foreach (var conditionObj in eHealthScoreModel.PatientConditionForeHealthScoreModel)
                    {
                        if (conditionObj != null && conditionObj.Condition != "")
                        {
                            if (eHealthScoreModel.PatientDetailsForeHealthScoreModel.ObservedConditionText != "")
                            {
                                Paragraph emptySourceDescPara = section.AddParagraph();
                            }
                            Paragraph conditionListParagraph = section.AddParagraph();
                            conditionListParagraph.AddText(conditionObj.Condition != "" ? conditionObj.Condition : " - ");
                            conditionListParagraph.Format.Font.Size = 11;
                            conditionListParagraph.Format.LeftIndent = "1.3cm";
                            conditionListParagraph.Format.FirstLineIndent = "-0.5cm";
                            conditionListParagraph.Format.ListInfo.ListType = ListType.BulletList1;
                        }

                    }
                }
            }
                else //if (eHealthScoreModel.PatientConditionForeHealthScoreModel[0].IsConditionShowFlag == false)
                {
                    Paragraph emptySourceDescPara = section.AddParagraph();
                    Paragraph noneConditionListParagraph = section.AddParagraph();
                    noneConditionListParagraph.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.NoneObserveConditions);
                    noneConditionListParagraph.Format.Font.Size = 11;

                }
            if (eHealthScoreModel.PatientDetailsForeHealthScoreModel.ThirdDescriptionParagraph != "")
            {
                Paragraph emptyConditionParagraph = section.AddParagraph();

            Paragraph thirdDescPara = section.AddParagraph();
            thirdDescPara.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.ThirdDescriptionParagraph);
            thirdDescPara.Format.Alignment = ParagraphAlignment.Left;
            thirdDescPara.Format.Font.Size = 11;
            
            }
            if (eHealthScoreModel.PatientDetailsForeHealthScoreModel.FourthDescriptionParagraph!="")
            {
                Paragraph emptyThirdDescPara = section.AddParagraph();
            }
            Paragraph fourthDescPara = section.AddParagraph();
            fourthDescPara.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.FourthDescriptionParagraph);
            fourthDescPara.Format.Alignment = ParagraphAlignment.Left;
            fourthDescPara.Format.Font.Size = 11;
            if (eHealthScoreModel.PatientDetailsForeHealthScoreModel.FourthDescriptionParagraph != "")
            {
                Paragraph emptyFourthDescPara = section.AddParagraph();
            }

            Paragraph fifthDescPara = section.AddParagraph();
            fifthDescPara.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.FifthDescriptionParagraph);
            fifthDescPara.Format.Alignment = ParagraphAlignment.Left;
            fifthDescPara.Format.Font.Size = 11;

            section.AddPageBreak();


            if (eHealthScoreModel.BiometricsAndHRAResultModel.Count > 0)
            {

                Paragraph biometricsHeaderDescPara = section.AddParagraph();
                biometricsHeaderDescPara.AddText("Your Health-e Scores");
                biometricsHeaderDescPara.Format.Alignment = ParagraphAlignment.Left;
                biometricsHeaderDescPara.Format.Font.Size = 12;
                biometricsHeaderDescPara.Format.Font.Bold = true;

                Paragraph biometricsHeaderEmptyPara = section.AddParagraph();

                Table biometricsTable = section.AddTable();
                biometricsTable.Borders.Visible = true;
                Column biometricsColumn = biometricsTable.AddColumn();
                biometricsColumn.Format.Alignment = ParagraphAlignment.Left;

                biometricsColumn = biometricsTable.AddColumn();
                biometricsColumn.Format.Alignment = ParagraphAlignment.Left;

                biometricsColumn = biometricsTable.AddColumn();
                biometricsColumn.Format.Alignment = ParagraphAlignment.Left;
                biometricsColumn = biometricsTable.AddColumn();
                biometricsColumn.Format.Alignment = ParagraphAlignment.Left;

                biometricsTable.Columns[0].Width = 120;
                biometricsTable.Columns[1].Width = 100;
                biometricsTable.Columns[2].Width = 115;
                biometricsTable.Columns[3].Width = 115;

                Row biometricsRow = biometricsTable.AddRow();
                biometricsRow.TopPadding = 5;
                biometricsRow.BottomPadding = 5;
                biometricsRow.Format.Alignment = ParagraphAlignment.Center;
                biometricsRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
                biometricsRow.Format.Font.Color = Colors.White;
                biometricsRow.Cells[0].AddParagraph("Screening Test").Format.Font.Bold = true;
                biometricsRow.Cells[1].AddParagraph("Your Value").Format.Font.Bold = true;
                biometricsRow.Cells[2].AddParagraph("Risk Levels").Format.Font.Bold = true;
                biometricsRow.Cells[2].MergeRight = 1;
                int objIndex = 0;
                foreach (var obj in eHealthScoreModel.BiometricsAndHRAResultModel)
                {
                    //biometricsRow = biometricsTable.AddRow();
                    //biometricsRow.TopPadding = 5;
                    //biometricsRow.BottomPadding = 5;
                    //biometricsRow.Cells[0].AddParagraph(obj.BiometricDate != null ? obj.BiometricName + " " + obj.BiometricDate.Value.ToString("MM/dd/yy") : obj.BiometricName);
                    //biometricsRow.Cells[1].AddParagraph(obj.BiometricValue != null ? obj.BiometricValue.ToString() : "Value Not Available").Format.Alignment = ParagraphAlignment.Center;
                    
                    foreach (var benchmarkObj in eHealthScoreModel.BiometricsBenchmarkModel.FindAll(x => x.LOINCCode == obj.LOINCCode).ToList())
                    {
                        int benchmarkObjCount = eHealthScoreModel.BiometricsBenchmarkModel.FindAll(x => x.LOINCCode == obj.LOINCCode).Count;
                            int []colorArray = benchmarkObj.FontColor.Split(",").Select(x => int.Parse(x)).ToArray();
                        biometricsRow = biometricsTable.AddRow();
                        biometricsRow.TopPadding = 2;
                        biometricsRow.BottomPadding = 2;
                        biometricsRow.Cells[0].AddParagraph(obj.BiometricDate != null ? obj.BiometricName + " " + obj.BiometricDate.Value.ToString("MM/dd/yy") : obj.BiometricName);
                        biometricsRow.Cells[0].MergeDown = benchmarkObjCount - 1;
                        biometricsRow.Cells[1].AddParagraph(obj.BiometricValueString != null ? obj.BiometricValueString : "Value Not Available").Format.Alignment = ParagraphAlignment.Center;
                        biometricsRow.Cells[1].MergeDown = benchmarkObjCount -1;
                        if (!string.IsNullOrEmpty(obj.Risk) && obj.Risk == Benchmarks.Normal_Risk)
                        {
                            biometricsRow.Cells[1].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(124, 173, 94);
                        }
                        else if (!string.IsNullOrEmpty(obj.Risk) && obj.Risk == Benchmarks.Moderate_Risk)
                        {
                            biometricsRow.Cells[1].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(221, 134, 79);

                        }
                        else if (!string.IsNullOrEmpty(obj.Risk) && obj.Risk == Benchmarks.High_Risk)
                        {
                            biometricsRow.Cells[1].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color(198, 61, 70);

                        }
                        biometricsRow.Cells[2].AddParagraph(benchmarkObj.Risk).Format.Font.Size = 10;
                        biometricsRow.Cells[3].AddParagraph(benchmarkObj.DisplayRiskRanges).Format.Font.Size = 10;
                        biometricsRow.Cells[2].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color((byte)colorArray[0], (byte)colorArray[1], (byte)colorArray[2]);
                        biometricsRow.Cells[3].Format.Font.Color = new MigraDoc.DocumentObjectModel.Color((byte)colorArray[0], (byte)colorArray[1], (byte)colorArray[2]);
                    }
                    if (objIndex < eHealthScoreModel.BiometricsAndHRAResultModel.Count)
                    {
                        biometricsRow = biometricsTable.AddRow();
                        biometricsRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
                    }
                    objIndex++;
                }
                Paragraph notesEmptyParagraph = section.AddParagraph(); 

                Paragraph notesParagraph = section.AddParagraph();
                notesParagraph.AddText(eHealthScoreModel.PatientDetailsForeHealthScoreModel.NoteDescriptionParagraph);
                //notesParagraph.AddText("Please Note:For more information about your biometric scores, please refer to “Appendix A – Understanding Your Scores” at the end of this document.");
                notesParagraph.Format.Font.Size = 14;
                notesParagraph.Format.Font.Bold = true;

                section.AddPageBreak();
                //Biometrics recommended guidlines
                Paragraph topRightHeadingParagraph = section.AddParagraph();
                topRightHeadingParagraph.AddText("APPENDIX A");
                topRightHeadingParagraph.Format.Font.Size = 16;
                topRightHeadingParagraph.Format.Font.Bold = true;
                topRightHeadingParagraph.Format.Alignment = ParagraphAlignment.Right;

                Paragraph topHeadingParagraph = section.AddParagraph();
                topHeadingParagraph.AddText("Understanding Your Scores");
                topHeadingParagraph.Format.Font.Size = 14;

                Table biometricsDescriptionTable = section.AddTable();
                biometricsDescriptionTable.Borders.Visible = true;
                Column biometricsDescriptionColumn = biometricsDescriptionTable.AddColumn();
                biometricsDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

                biometricsDescriptionColumn = biometricsDescriptionTable.AddColumn();
                biometricsDescriptionColumn.Format.Alignment = ParagraphAlignment.Left;

                biometricsDescriptionTable.Columns[0].Width = 150;
                biometricsDescriptionTable.Columns[1].Width = 300;

                Row biometricsDescriptionRow = biometricsDescriptionTable.AddRow();
                biometricsDescriptionRow.TopPadding = 5;
                biometricsDescriptionRow.BottomPadding = 5;
                biometricsDescriptionRow.Format.Alignment = ParagraphAlignment.Center;
                biometricsDescriptionRow.Shading.Color = new MigraDoc.DocumentObjectModel.Color(101, 102, 101);
                biometricsDescriptionRow.Format.Font.Color = Colors.White;
                biometricsDescriptionRow.Cells[0].AddParagraph("Lab Test").Format.Font.Bold = true;
                biometricsDescriptionRow.Cells[1].AddParagraph("Recommended National Guidelines").Format.Font.Bold = true;
                int guidlinesIndex = 0;
                foreach (var guidlinesObj in eHealthScoreModel.BiometricsAndHRAResultModel)
                {
                    if (guidlinesIndex == 0 && (guidlinesObj.LOINCCode == "8480-6" || guidlinesObj.LOINCCode == "8462-4"))
                    {
                        biometricsDescriptionRow = biometricsDescriptionTable.AddRow();
                        biometricsDescriptionRow.TopPadding = 3;
                        biometricsDescriptionRow.BottomPadding = 3;
                        biometricsDescriptionRow.Format.Font.Size = 11;
                        biometricsDescriptionRow.Cells[0].AddParagraph("Blood Pressure");
                        biometricsDescriptionRow.Cells[1].AddParagraph(guidlinesObj.RecommendedGuidelines) ;
                    }
                    else if (guidlinesIndex != 0 && (guidlinesObj.LOINCCode != "8480-6" && guidlinesObj.LOINCCode != "8462-4"))
                    {
                        biometricsDescriptionRow = biometricsDescriptionTable.AddRow();
                        biometricsDescriptionRow.TopPadding = 3;
                        biometricsDescriptionRow.BottomPadding = 3;
                        biometricsDescriptionRow.Format.Font.Size = 11;
                        biometricsDescriptionRow.Cells[0].AddParagraph(guidlinesObj.BiometricName);
                        biometricsDescriptionRow.Cells[1].AddParagraph(guidlinesObj.RecommendedGuidelines.Replace("{addspace}", "\u00A0"));
                    }
                    guidlinesIndex++;
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
            //pdf

            //if (isEncrypt)
            //    pdfRenderer.PdfDocument.SecuritySettings.UserPassword = encryptionPassword;
            pdfRenderer.PdfDocument.Save(tempStream, false);

            return tempStream;
        }
        public MemoryStream ExportHealtheScoreToExcel(FilterModelForHealtheScore filterModelForMemberHealtheScore, TokenModel tokenModel)
        {
            DataTable dynamiceQueryDataModel = _eHealthScoreRepository.ExportHealtheScoreToExcel(filterModelForMemberHealtheScore, tokenModel);

            MemoryStream stream = null;
            if (dynamiceQueryDataModel != null)
            {
               // stream = GenerateExcel(dynamiceQueryDataModel);
            }
            return stream;
        }
        //private MemoryStream GenerateExcel(DataTable dynamiceQueryDataModel)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    using (var excel = new ExcelPackage())
        //    {
        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        worksheet.DefaultRowHeight = 12;

        //        worksheet.Cells[1, 1].LoadFromDataTable(dynamiceQueryDataModel, true);
        //        byte[] data = excel.GetAsByteArray();

        //        memoryStream = new MemoryStream(data);

        //    }
        //    return memoryStream;
        //}

    }
}
