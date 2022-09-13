using HC.Common;
using HC.Common.Enums;
using HC.Common.HC.Common;
using HC.Common.Model.OrganizationSMTP;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model.DiseaseManagementProgram;
using HC.Patient.Model.MasterData;
using HC.Patient.Model.Patient;
using HC.Patient.Model.PatientDiseaseManagementProgram;
using HC.Patient.Model.Questionnaire;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Patient.Repositories.IRepositories.DiseaseManagementProgram;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Repositories.IRepositories.PatientDiseaseManagementProgram;
using HC.Patient.Service.IServices.GlobalCodes;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.PatientDiseaseManagementProgram;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using static HC.Common.Enums.CommonEnum;
using static HC.Model.ProgramsFilterModel;

namespace HC.Patient.Service.Services.PatientDiseaseManagementProgram
{
    public class PatientDiseaseManagementProgramService : BaseService, IPatientDiseaseManagementProgramService
    {
        private readonly IPatientDiseaseManagementProgramRepository _patientDiseaseManagementProgramRepository;
        private readonly IDiseaseManagementProgramRepository _diseaseManagementProgramRepository;

        private readonly ILocationService _locationService;
        private readonly IChatRepository _chatRepository;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IEmailService _emailSender;
        private readonly IHostingEnvironment _env;
        private JsonModel response = new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
        private readonly IPatientService _IPatientService;
        private HCOrganizationContext _context;
        private readonly IGlobalCodeService _globalCodeService;
        private readonly IPatientAlertService _patientAlertService;
        private readonly ITokenService _tokenService;
        public PatientDiseaseManagementProgramService(IPatientService patientService, IPatientDiseaseManagementProgramRepository patientDiseaseManagementProgramRepository, IDiseaseManagementProgramRepository diseaseManagementProgramRepository
            , ILocationService locationService, IChatRepository chatRepository, IOrganizationSMTPRepository organizationSMTPRepository, IEmailService emailSender, IHostingEnvironment env, HCOrganizationContext context
            , IGlobalCodeService globalCodeService, IPatientAlertService patientAlertService, ITokenService tokenService)
        {
            _IPatientService = patientService;
            _patientDiseaseManagementProgramRepository = patientDiseaseManagementProgramRepository;
            _diseaseManagementProgramRepository = diseaseManagementProgramRepository;
            _locationService = locationService;
            _chatRepository = chatRepository;
            _organizationSMTPRepository = organizationSMTPRepository;
            _emailSender = emailSender;
            _env = env;
            _context = context;
            _globalCodeService = globalCodeService;
            _patientAlertService = patientAlertService;
            _tokenService = tokenService;
        }
        public JsonModel EnrollmentPatientInDiseaseManagementProgram(int patientDiseaseManagementProgramId, DateTime enrollmentDate, bool isEnrolled, TokenModel token)
        {
            Entity.PatientDiseaseManagementProgram patientDiseaseManagementProgram = _patientDiseaseManagementProgramRepository.GetByID(patientDiseaseManagementProgramId);
            if (patientDiseaseManagementProgram != null)
            {if (isEnrolled == true)
                    patientDiseaseManagementProgram.DateOfEnrollment = enrollmentDate;
                else
                    patientDiseaseManagementProgram.DateOfTermination = enrollmentDate;
                patientDiseaseManagementProgram.UpdatedBy = token.UserID;
                patientDiseaseManagementProgram.UpdatedDate = DateTime.UtcNow;
                _patientDiseaseManagementProgramRepository.Update(patientDiseaseManagementProgram);
                _patientDiseaseManagementProgramRepository.SaveChanges();
                response = isEnrolled ? new JsonModel(null, StatusMessage.PatientEnrolledInDMP, (int)HttpStatusCodes.OK) : new JsonModel(null, StatusMessage.PatientTerminatedInDMP, (int)HttpStatusCodes.OK);
            }
            else
                response = new JsonModel(null, StatusMessage.PatientDMPNotExist, (int)HttpStatusCodes.NotFound);
            return response;
        }
        public JsonModel AssignNewPrograms(AssignNewProgramModel assignNewProgramModel, TokenModel token)
        {


            //List<int> programIds = assignNewProgramModel.ProgramId.Split(',').Select(Int32.Parse).ToList();
            //List<Entity.PatientDiseaseManagementProgram> patientDiseaseManagementProgramListObj = _patientDiseaseManagementProgramRepository.GetAll(a=>programIds.Contains(a.DiseaseManagementPlanID) && a.DateOfTermination==null).ToList();
            //if (patientDiseaseManagementProgramListObj.Count > 0)
            //{
            //     response = new JsonModel(null, StatusMessage.RecordAlreadyExists, (int)HttpStatusCodes.BadRequest);
            //}
            //List<Entity.DiseaseManagementProgram> diseaseManagementPrograms = _diseaseManagementProgramRepository.GetAll(a => programIds.Contains(a.ID)).ToList();
            //if (diseaseManagementPrograms != null)
            //{
            //    diseaseManagementPrograms.ForEach(x =>
            //    {
            //        patientDiseaseManagementProgram = new Entity.PatientDiseaseManagementProgram();
            //        patientDiseaseManagementProgram.PatientID = assignNewProgramModel.PatientId;
            //        patientDiseaseManagementProgram.DiseaseManagementPlanID = x.ID;
            //        patientDiseaseManagementProgram.IsActive = true;
            //        patientDiseaseManagementProgram.IsDeleted = false;
            //        patientDiseaseManagementProgram.AssignedDate = DateTime.UtcNow;
            //        patientDiseaseManagementProgram.DateOfEnrollment = DateTime.UtcNow;
            //        patientDiseaseManagementProgramList.Add(patientDiseaseManagementProgram);
            //    });

            //    _patientDiseaseManagementProgramRepository.Create(patientDiseaseManagementProgramList.ToArray());
            //    _patientDiseaseManagementProgramRepository.SaveChanges();
            //    response = new JsonModel(null, StatusMessage.PatientProgramsAdded, (int)HttpStatusCodes.OK);
            //}
            //else
            //{
            //    response = new JsonModel(null, StatusMessage.RecordNotExists, (int)HttpStatusCodes.BadRequest);

            //}
            string statusMessage = string.Empty;
            List<Entity.PatientDiseaseManagementProgram> patientDiseaseManagementProgramList = new List<Entity.PatientDiseaseManagementProgram>();
            Entity.PatientDiseaseManagementProgram patientDiseaseManagementProgram = null;
            int[] Ids = assignNewProgramModel.PatientDiseaseManagementPrograms.Where(x => x.Id > 0).Select(x => x.Id).ToArray();

            patientDiseaseManagementProgramList = _patientDiseaseManagementProgramRepository.GetAll(x => Ids.Contains(x.ID)).ToList();

            foreach (ProgramModel programModel in assignNewProgramModel.PatientDiseaseManagementPrograms)
            {
                patientDiseaseManagementProgram = new Entity.PatientDiseaseManagementProgram();
                if (programModel.Id == 0)
                {
                    _patientDiseaseManagementProgramRepository.UpdatePrimaryCareManager(assignNewProgramModel.PatientId, programModel.CareManagerId);
                    var data = _patientDiseaseManagementProgramRepository.Get(x => x.PatientID == assignNewProgramModel.PatientId && x.DiseaseManagementPlanID == programModel.ProgramId && x.StatusId == programModel.StatusId && x.FrequencyId == programModel.FrequencyId);
                    if(data == null)
                    {
                        patientDiseaseManagementProgram.PatientID = assignNewProgramModel.PatientId;
                        patientDiseaseManagementProgram.DiseaseManagementPlanID = programModel.ProgramId;
                        patientDiseaseManagementProgram.DateOfEnrollment = programModel.DateOfEnrollment;
                        patientDiseaseManagementProgram.GraduationDate = programModel.GraduationDate;
                        patientDiseaseManagementProgram.OtherFrequencyDescription = programModel.OtherFrequencyDescription;
                        patientDiseaseManagementProgram.DateOfTermination = null;
                        patientDiseaseManagementProgram.StatusId = programModel.StatusId;
                        patientDiseaseManagementProgram.FrequencyId = programModel.FrequencyId;
                        patientDiseaseManagementProgram.CareManagerId = programModel.CareManagerId;
                        patientDiseaseManagementProgram.IsActive = true;
                        patientDiseaseManagementProgram.IsDeleted = false;
                        patientDiseaseManagementProgram.AssignedDate = DateTime.UtcNow;
                        patientDiseaseManagementProgram.CreatedDate = DateTime.UtcNow;
                        patientDiseaseManagementProgram.CreatedBy = token.UserID;
                        patientDiseaseManagementProgramList.Add(patientDiseaseManagementProgram);

                        //Save Alert for patient when provider adds new a value
                        var user = _tokenService.GetUserById(token);
                        if (user != null && user.UserRoles != null && user.UserRoles.RoleName != OrganizationRoles.Patient.ToString())
                        {
                            int masterAlertTypeId = _globalCodeService.GetGlobalCodeValueId(GlobalCodeName.PATIENTALERTTYPE, "New Program Enrollment", token, true);
                            bool isSuccess = _patientAlertService.SavePatientAlerts("New Program Enrollment", patientDiseaseManagementProgram.PatientID, masterAlertTypeId, null, token);
                        }
                    }
                    else
                    {
                        statusMessage = "Program Already Exists";
                        return new JsonModel(null, statusMessage, (int)HttpStatusCodes.BadRequest);
                    }
                  
                } else
                {
                    _patientDiseaseManagementProgramRepository.UpdatePrimaryCareManager(assignNewProgramModel.PatientId, programModel.CareManagerId);
                    patientDiseaseManagementProgram = patientDiseaseManagementProgramList.Where(x => x.ID == programModel.Id).FirstOrDefault();
                    patientDiseaseManagementProgram.DateOfEnrollment = programModel.DateOfEnrollment;
                    patientDiseaseManagementProgram.GraduationDate = programModel.GraduationDate;
                    patientDiseaseManagementProgram.DateOfTermination = programModel.DateOfTermination;
                    patientDiseaseManagementProgram.StatusId = programModel.StatusId;
                    patientDiseaseManagementProgram.FrequencyId = programModel.FrequencyId;
                    patientDiseaseManagementProgram.CareManagerId = programModel.CareManagerId;
                    patientDiseaseManagementProgram.OtherFrequencyDescription = programModel.OtherFrequencyDescription;
                    patientDiseaseManagementProgram.UpdatedBy = token.UserID;
                    patientDiseaseManagementProgram.UpdatedDate = DateTime.UtcNow;
                }
            }
            if (patientDiseaseManagementProgramList.Count > 0)
            {
                if (patientDiseaseManagementProgramList.Any(x => x.ID == 0))
                {
                    _patientDiseaseManagementProgramRepository.Create(patientDiseaseManagementProgramList.Where(x => x.ID == 0).ToArray());
                    statusMessage = StatusMessage.PatientProgramsAdded;
                }
                else
                {
                    _patientDiseaseManagementProgramRepository.Update(patientDiseaseManagementProgramList.Where(x => x.ID > 0).ToArray());
                    statusMessage = StatusMessage.PatientProgramsUpdated;
                }
                _patientDiseaseManagementProgramRepository.SaveChanges();

                foreach(var item in assignNewProgramModel.Logs)
                {
                    saveHRALogs(item, token);
                }
               
            }
            return new JsonModel(null, statusMessage, (int)HttpStatusCodes.OK);
        }

        public void saveHRALogs(LogModel logModel, TokenModel tokenModel)
        {
            HRALogs entity = new HRALogs();

            entity.FileName = logModel.DocName;
            entity.ProviderId = logModel.ProviderId;
            entity.OrganizationId = tokenModel.OrganizationID;
            entity.ReportType = "Programs";
            entity.ReportTypeId = Convert.ToInt32(CommonEnum.LogReportType.Programs);
            entity.CreatedBy = tokenModel.UserID;
            entity.ReportDate = DateTime.UtcNow.ToString("MM/dd/yyyy");
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;

            _context.HRALogs.Add(entity);
            _context.SaveChanges();

        }
        public JsonModel GetPatientDiseaseManagementProgramList(int patientId, FilterModel filterModel, TokenModel token)
        {
            //if (!_IPatientService.IsValidUserForDataAccess(token, patientId))
            //{
            //    return new JsonModel(null, StatusMessage.UnAuthorizedAccess, (int)HttpStatusCode.Unauthorized);
            //}
            List<PatientDiseaseManagementProgramModel> responseList = _patientDiseaseManagementProgramRepository.GetPatientDiseaseManagementProgramList<PatientDiseaseManagementProgramModel>(patientId, filterModel, token).ToList();
            if (responseList != null && responseList.Count > 0)
            {
                response = new JsonModel(responseList,StatusMessage.FetchMessage,(int)HttpStatusCodes.OK);
                response.meta = new Meta(responseList, filterModel);
            }
            else
                response = new JsonModel(new List<PatientDiseaseManagementProgramModel>(), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            return response;
        }

        public JsonModel GetPatientDiseaseManagementProgramDetails(int Id, TokenModel token)
        {
            AssignNewProgramModel assignNewProgramModel = new AssignNewProgramModel();
            ProgramModel programModel = new ProgramModel();
            Entity.PatientDiseaseManagementProgram patientDiseaseManagementProgram = _patientDiseaseManagementProgramRepository.GetByID(Id);
            if (patientDiseaseManagementProgram != null)
            {
                AutoMapper.Mapper.Map(patientDiseaseManagementProgram, programModel);
                programModel.Id = patientDiseaseManagementProgram.ID;
                programModel.ProgramId = patientDiseaseManagementProgram.DiseaseManagementPlanID;
                // assign this program object to AssignNewProgramModel
                assignNewProgramModel.PatientId = patientDiseaseManagementProgram.PatientID;
                assignNewProgramModel.PatientDiseaseManagementPrograms = new List<ProgramModel>();
                assignNewProgramModel.PatientDiseaseManagementPrograms.Add(programModel);
                response = new JsonModel(assignNewProgramModel, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            }
            return response;
        }

        public JsonModel deleteDiseaseManagementProgram(int Id, TokenModel token)
        {
            Entity.PatientDiseaseManagementProgram patientDiseaseManagementProgram = _patientDiseaseManagementProgramRepository.GetByID(Id);
            if (patientDiseaseManagementProgram != null)
            {
                patientDiseaseManagementProgram.IsActive = false;
                patientDiseaseManagementProgram.IsDeleted = true;
                patientDiseaseManagementProgram.DeletedBy = token.UserID;
                patientDiseaseManagementProgram.DeletedDate = DateTime.UtcNow;
                _patientDiseaseManagementProgramRepository.Update(patientDiseaseManagementProgram);
                _patientDiseaseManagementProgramRepository.SaveChanges();
                response = new JsonModel(null, StatusMessage.PatientProgramsDeleted, (int)HttpStatusCodes.OK);
            }
            else
            {
                response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCodes.NotFound);
            }
            return response;
        }
        public JsonModel GetAllPatientDiseaseManagementProgramsList(ProgramsFilterModel filterModel, TokenModel token)
        {
            //List<AllPatientsDiseaseManagementProgramModel> responseList = _patientDiseaseManagementProgramRepository.GetAllPatientDiseaseManagementProgramsList<AllPatientsDiseaseManagementProgramModel>(filterModel, token).ToList();
            //if (responseList != null && responseList.Count > 0)
            //{
            //    response = new JsonModel(responseList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            //    response.meta = new Meta(responseList, filterModel);
            //}
            //else
            //    response = new JsonModel(new List<AllPatientsDiseaseManagementProgramModel>(), StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
            //return response;

            Dictionary<string, object> responseList = _patientDiseaseManagementProgramRepository.GetAllPatientDiseaseManagementProgramsList(filterModel, token);

            responseList.Add("meta", new Meta()
            {
                TotalRecords = (List<DMPPatientModel>)responseList["DMPPatientModel"] != null && ((List<DMPPatientModel>)responseList["DMPPatientModel"]).Count > 0 ? ((List<DMPPatientModel>)responseList["DMPPatientModel"]).First().TotalRecords : 0
                   ,
                CurrentPage = filterModel.pageNumber,
                PageSize = filterModel.pageSize,
                DefaultPageSize = filterModel.pageSize,
                TotalPages = Math.Ceiling(Convert.ToDecimal(((List<DMPPatientModel>)responseList["DMPPatientModel"] != null && ((List<DMPPatientModel>)responseList["DMPPatientModel"]).Count > 0) ? ((List<DMPPatientModel>)responseList["DMPPatientModel"]).First().TotalRecords : 0) / filterModel.pageSize)
            });
            //List<PatientCareGapModel> patientCareGapsList = _patientCareGapsRepository.GetPatientCareGapsDetailedList<PatientCareGapModel>(patientId, filterModel, token).ToList();
            return new JsonModel(responseList, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK);
        }
        public MemoryStream GetProgramsEnrollPatientsForPDF(ProgramsFilterModel filterModel, TokenModel token)
        {
            MemoryStream memoryStream = new MemoryStream();
            List<DMPEnrolleesModel> dMPEnrolleesModel = _patientDiseaseManagementProgramRepository.GetProgramsEnrollPatientsForPDF<DMPEnrolleesModel>(filterModel, token).ToList();

            if (dMPEnrolleesModel != null && dMPEnrolleesModel.Count() > 0)
            {
                memoryStream = GenerateDMPEnrolleesPatientsListPDF(dMPEnrolleesModel, token);
            }
            return memoryStream;
        }
        public MemoryStream GenerateDMPEnrolleesPatientsListPDF(List<DMPEnrolleesModel> dMPEnrolleesModel, TokenModel token)
        {
            MemoryStream tempStream = null;
            Document document = new Document();
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.PageHeight = 792;
            document.DefaultPageSetup.PageWidth = 612;
            document.DefaultPageSetup.TopMargin = 30;
            document.DefaultPageSetup.BottomMargin =70;
            document.DefaultPageSetup.LeftMargin = 25;
            document.DefaultPageSetup.RightMargin = 15;

            //Header Section

            Section section = document.AddSection();
            LocationModel locationModal = _locationService.GetLocationOffsets(token.LocationID, token);
            //Document Footer
            //section.PageSetup.DifferentFirstPageHeaderFooter = true;
          
            Table pageNumberFooterTable = section.Footers.Primary.AddTable();
            pageNumberFooterTable.BottomPadding = 0;
            Column pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left;

            //pageNumberFooterColumn = pageNumberFooterTable.AddColumn();
            //pageNumberFooterColumn.Format.Alignment = ParagraphAlignment.Left; ;

            pageNumberFooterTable.Columns[0].Width = 100;
            pageNumberFooterTable.Columns[1].Width = 100;
            pageNumberFooterTable.Columns[2].Width = 80;
            pageNumberFooterTable.Columns[3].Width = 100;
            pageNumberFooterTable.Columns[4].Width = 70;

            Row pageNumberFooterRow = pageNumberFooterTable.AddRow();
            pageNumberFooterRow.TopPadding = 5;
            pageNumberFooterRow.Cells[0].AddParagraph("");
            pageNumberFooterRow.Cells[1].AddParagraph("");
            pageNumberFooterRow.Cells[2].AddParagraph("");
            pageNumberFooterRow.Cells[3].AddParagraph("Page ").AddPageField();
            pageNumberFooterRow.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            string path = _env.WebRootPath + "\\PDFImages\\overture-updated-bottom-logo.png";
            pageNumberFooterRow.Cells[4].AddImage(path).Height=35;
            
            pageNumberFooterRow.Format.Alignment = ParagraphAlignment.Right;
            pageNumberFooterRow.Format.Font.Size = 10;
            int index = 0;
            if (index == 0)
            {
                Paragraph topHeaderLogoParagraph = section.AddParagraph();
                string logoPath = _env.WebRootPath + "\\PDFImages\\overture-updated-top-logo.png";
                topHeaderLogoParagraph.AddImage(logoPath).Width = 200;
                topHeaderLogoParagraph.Format.Alignment = ParagraphAlignment.Left;
                index++;
            }

            Paragraph topHeaderLogoEmptyParagraph = section.AddParagraph();
            Paragraph topHeaderLogoEmpty1Paragraph = section.AddParagraph();

            Paragraph headerParagraph = section.AddParagraph();
            //headerParagraph.AddText("Overture Health Care");
            //headerParagraph.AddLineBreak();
            headerParagraph.AddText("Program Enrollments");
            headerParagraph.Format.Font.Name = "Arial";
            headerParagraph.Format.Font.Size = 15;
            headerParagraph.Format.Alignment = ParagraphAlignment.Left;
            headerParagraph.Format.Font.Bold = true;

            Paragraph emptyParagraph = section.AddParagraph();
            // Patient details
            Table dmpEnrolleesPatientListInfoTable = section.AddTable();
            dmpEnrolleesPatientListInfoTable.Borders.Visible = true;
            Column dmpEnrolleesPatientListInfoColumn = dmpEnrolleesPatientListInfoTable.AddColumn();
            dmpEnrolleesPatientListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
            for (int ind = 0; ind < 12; ind++)
            {
                dmpEnrolleesPatientListInfoColumn = dmpEnrolleesPatientListInfoTable.AddColumn();
                dmpEnrolleesPatientListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
            }

            dmpEnrolleesPatientListInfoTable.Format.Font.Size = 6;
            for (int y = 0; y < 13; y++)
            {
                dmpEnrolleesPatientListInfoTable.Columns[y].Width = 44;
            }

            Row dmpEnrolleesPatientListInfoRow = dmpEnrolleesPatientListInfoTable.AddRow();
            dmpEnrolleesPatientListInfoRow.HeadingFormat = true;
            //dmpEnrolleesPatientListInfoRow.Format.Font.Size = 10;
            dmpEnrolleesPatientListInfoRow.BottomPadding = 5;
            dmpEnrolleesPatientListInfoRow.TopPadding = 5;
            dmpEnrolleesPatientListInfoRow.Format.Alignment = ParagraphAlignment.Left;
            dmpEnrolleesPatientListInfoRow.Cells[0].AddParagraph("MEMBER NAME").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[1].AddParagraph("GENDER").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[2].AddParagraph("AGE").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[3].AddParagraph("ELIGIBILITY STATUS").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[4].AddParagraph("RELATIONSHIP").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[5].AddParagraph("DISEASE CONDITIONS").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[6].AddParagraph("PROGRAM NAME").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[7].AddParagraph("ENROLLMENT DATE").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[8].AddParagraph("GRADUATION DATE").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[9].AddParagraph("TERMINATION DATE").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[10].AddParagraph("CARE MANAGER").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[11].AddParagraph("STATUS").Format.Font.Bold = true;
            dmpEnrolleesPatientListInfoRow.Cells[12].AddParagraph("UPCOMING APPOINTMENT").Format.Font.Bold = true;

            foreach (var obj in dMPEnrolleesModel)
            {
                dmpEnrolleesPatientListInfoRow = dmpEnrolleesPatientListInfoTable.AddRow();
                //dmpEnrolleesPatientListInfoRow.Format.Font.Size = 10;
                dmpEnrolleesPatientListInfoRow.BottomPadding = 5;
                dmpEnrolleesPatientListInfoRow.TopPadding = 5;
                dmpEnrolleesPatientListInfoRow.Format.Alignment = ParagraphAlignment.Left;
                dmpEnrolleesPatientListInfoRow.Cells[0].AddParagraph(obj.PatientName != null ? obj.PatientName : "");
                dmpEnrolleesPatientListInfoRow.Cells[1].AddParagraph(obj.Gender != null ? obj.Gender : "");
                dmpEnrolleesPatientListInfoRow.Cells[2].AddParagraph(obj.Age != null ? obj.Age.ToString() : "");
                dmpEnrolleesPatientListInfoRow.Cells[3].AddParagraph(obj.IsEligible != null ? obj.IsEligible : "");
                dmpEnrolleesPatientListInfoRow.Cells[4].AddParagraph(obj.Relationship != null ? obj.Relationship : "");
                dmpEnrolleesPatientListInfoRow.Cells[5].AddParagraph(obj.DiseaseCondition != null ? obj.DiseaseCondition : "");
                dmpEnrolleesPatientListInfoRow.Cells[6].AddParagraph(obj.DiseaseManagePrograms != null ? obj.DiseaseManagePrograms : "");
                dmpEnrolleesPatientListInfoRow.Cells[7].AddParagraph(obj.EnrollmentDates != null ? obj.EnrollmentDates : "");
                dmpEnrolleesPatientListInfoRow.Cells[8].AddParagraph(obj.GraduationDates != null ? obj.GraduationDates : "");
                dmpEnrolleesPatientListInfoRow.Cells[9].AddParagraph(obj.TerminationDates != null ? obj.TerminationDates : "");
                dmpEnrolleesPatientListInfoRow.Cells[10].AddParagraph(obj.CareManagers != null ? obj.CareManagers : "");
                dmpEnrolleesPatientListInfoRow.Cells[11].AddParagraph(obj.Status != null ? obj.Status : "");
                dmpEnrolleesPatientListInfoRow.Cells[12].AddParagraph(obj.NextAppointmentDate != null ? obj.NextAppointmentDate : "");
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

        public JsonModel GetReportHRAProgram(HRALogFilterModel filterModel, TokenModel tokenModel)
        {

            response.data = new object();
            response.Message = StatusMessage.ErrorOccured;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            List<HRAProgramReportLogModel> data = _patientDiseaseManagementProgramRepository.GetReportsHRAPrograms<HRAProgramReportLogModel>(filterModel, tokenModel).ToList();
            if (data != null && data.Count > 0)
            {
                response.data = data;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.Created;
                response.meta = new Meta(data, filterModel);
                return response;
            }
            return response;
        }

    }
}
