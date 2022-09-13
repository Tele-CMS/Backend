using HC.Model;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Service.IServices;
using HC.Service;
using System;
using AutoMapper;
using HC.Patient.Entity;
using HC.Patient.Service.IServices.Organizations;
using System.Net;
using HC.Patient.Model.Organizations;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Service.IServices.MasterData;
using HC.Patient.Model.MasterData;
using System.Linq;
using HC.Patient.Service.Token.Interfaces;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Collections.Generic;

namespace HC.Patient.Service.Services
{
    public class AppointmentPaymentService : BaseService, IAppointmentPaymentService
    {
        private readonly IAppointmentPaymentRepository _appointmentPaymentRepository;
        private JsonModel _response;
        private readonly IMapper _mapper;
        private readonly IOrganizationService _organizationService;
        private readonly ILocationService _locationService;
        private readonly ITokenService _tokenService;
        public AppointmentPaymentService(
            IAppointmentPaymentRepository appointmentPaymentRepository,
            IMapper mapper,
            IOrganizationService organizationService,
            ILocationService locationService,
            ITokenService tokenService
            )
        {
            _appointmentPaymentRepository = appointmentPaymentRepository;
            _mapper = mapper;
            _organizationService = organizationService;
            _locationService = locationService;
            _tokenService = tokenService;
        }
        public JsonModel SaveUpdateAppointmentPayment(AppointmentPaymentModel appointmentPaymentModel, TokenModel tokenModel)
        {
            if (appointmentPaymentModel.PaymentId == 0)
            {
                var tokenPayment = _appointmentPaymentRepository.GetAppointmentPaymentsByPaymentToken(appointmentPaymentModel.PaymentToken, tokenModel);
                if (tokenPayment != null)
                    return new JsonModel(null, StatusMessage.AppointmentPaymentTokenExisted, (int)HttpStatusCodes.Created, string.Empty);
            }
            var appointmentPayment = _mapper.Map<AppointmentPayments>(appointmentPaymentModel);
            var organizationJsonModel = _organizationService.GetOrganizationDetailsById(tokenModel);
            if (organizationJsonModel.StatusCode == (int)HttpStatusCode.OK)
            {
                var organization = (OrganizationDetailModel)organizationJsonModel.data;
                if (organization != null)
                {
                    appointmentPayment.OrganizationId = tokenModel.OrganizationID;
                    appointmentPayment.CommissionPercentage = organization.BookingCommision;
                    var payment = _appointmentPaymentRepository.SaveUpdatePayment(appointmentPayment, tokenModel);
                    if (payment != null)
                        _response = new JsonModel(null, StatusMessage.FetchMessage, (int)HttpStatusCodes.OK, string.Empty);
                    else
                        _response = new JsonModel(null, StatusMessage.AppointmentPaymentNotSaved, (int)HttpStatusCodes.InternalServerError, string.Empty);
                }
            }
            return _response;
        }

        public JsonModel GetAppointmentPaymentList(PaymentFilterModel paymentFilterModel, TokenModel tokenModel)
        {
            var dbUser = _tokenService.GetUserById(tokenModel);

            if (dbUser == null)
                return new JsonModel(null, StatusMessage.UserNotFound, (int)HttpStatusCodes.NotFound, string.Empty);

            if (dbUser.UserRoles.RoleName != OrganizationRoles.Admin.ToString())
                paymentFilterModel.StaffId = tokenModel.StaffID.ToString();

            LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);

            if (!string.IsNullOrEmpty(paymentFilterModel.PayDate))
                paymentFilterModel.PayDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.PayDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            if (!string.IsNullOrEmpty(paymentFilterModel.AppDate))
                paymentFilterModel.AppDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.AppDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            if (!string.IsNullOrEmpty(paymentFilterModel.RangeStartDate))
                paymentFilterModel.RangeStartDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.RangeStartDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();
            
            if (!string.IsNullOrEmpty(paymentFilterModel.RangeEndDate))
                paymentFilterModel.RangeEndDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.RangeEndDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();


            var appointmentPayments = _appointmentPaymentRepository.GetAppointmentPayments<AppointmentPaymentListingModel>(paymentFilterModel, tokenModel).ToList();

            if (appointmentPayments != null && appointmentPayments.Count > 0)
            {
                appointmentPayments.ForEach(x =>
                {
                    LocationModel locModel = _locationService.GetLocationOffsets(x.ServiceLocationID, tokenModel);
                    x.StartTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locModel.DaylightOffset, locModel.StandardOffset, locModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.EndTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.EndDateTime), locModel.DaylightOffset, locModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.PaymentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.CreatedDate), locModel.DaylightOffset, locModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locModel.DaylightOffset, locModel.StandardOffset, locModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentTime = x.StartTime + " - " + x.EndTime;
                });

                _response = new JsonModel
                {
                    data = appointmentPayments,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCode.Found,
                    meta = new Meta(appointmentPayments, paymentFilterModel)
                };

            }
            else
                _response = new JsonModel(null, StatusMessage.AppointmentPaymentNotFound, (int)HttpStatusCodes.NotFound, string.Empty);
            return _response;
        }

        public MemoryStream GetPaymentPdf(PaymentFilterModel paymentFilterModel, TokenModel tokenModel)
        {
            MemoryStream memoryStream = null;
            var dbUser = _tokenService.GetUserById(tokenModel);

           

            if (dbUser.UserRoles.RoleName != OrganizationRoles.Admin.ToString())
                paymentFilterModel.StaffId = tokenModel.StaffID.ToString();

            LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);

            if (!string.IsNullOrEmpty(paymentFilterModel.PayDate))
                paymentFilterModel.PayDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.PayDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            if (!string.IsNullOrEmpty(paymentFilterModel.AppDate))
                paymentFilterModel.AppDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.AppDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            var appointmentPayments = _appointmentPaymentRepository.GetAppointmentPaymentsForReport<AppointmentPaymentListingModel>(paymentFilterModel, tokenModel).ToList();

            if (appointmentPayments != null && appointmentPayments.Count > 0)
            {
                appointmentPayments.ForEach(x =>
                {
                    LocationModel locModel = _locationService.GetLocationOffsets(x.ServiceLocationID, tokenModel);
                    x.StartTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locModel.DaylightOffset, locModel.StandardOffset, locModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.EndTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.EndDateTime), locModel.DaylightOffset, locModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.PaymentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.CreatedDate), locModel.DaylightOffset, locModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locModel.DaylightOffset, locModel.StandardOffset, locModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentTime = x.StartTime + " - " + x.EndTime;
                });

                _response = new JsonModel
                {
                    data = appointmentPayments,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCode.Found,
                    meta = new Meta(appointmentPayments, paymentFilterModel)
                };
                memoryStream = this.GenerateMemoryStream(appointmentPayments);
            }
            return memoryStream;
        }

         public MemoryStream GenerateMemoryStream(List<AppointmentPaymentListingModel> patient)
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
            headingTitle.AddText("Payment Report");
            headingTitle.Format.Alignment = ParagraphAlignment.Center;
            headingTitle.Format.Font.Size = 12;
            headingTitle.Format.Font.Bold = true;
            // headingTitle.Format.Font.Color = Colors.Blue;
            Paragraph emptyParagrapgh = section.AddParagraph();
            emptyParagrapgh = section.AddParagraph();
            emptyParagrapgh = section.AddParagraph();
            emptyParagrapgh = section.AddParagraph();

            
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

            
            Table prescriptionListInfoTable = section.AddTable();
            //prescriptionListInfoTable.Borders.Visible = true;
            prescriptionListInfoTable.Borders.Visible = false;
            Column prescriptionListInfoColumn = prescriptionListInfoTable.AddColumn();
            prescriptionListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
            for (int ind = 0; ind < 5; ind++)
            {
                prescriptionListInfoColumn = prescriptionListInfoTable.AddColumn();
                prescriptionListInfoColumn.Format.Alignment = ParagraphAlignment.Left;
            }

            prescriptionListInfoTable.Format.Font.Size = 9;
            for (int y = 0; y < 6; y++)
            {
                if (y == 0)
                    prescriptionListInfoTable.Columns[y].Width = 90;
                else if (y == 2)
                    prescriptionListInfoTable.Columns[y].Width = 100;
                else if (y == 3)
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
            prescriptionListInfoRow.Cells[0].AddParagraph("PATIENT NAME").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[1].AddParagraph("APPOITNMENT DATE").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[2].AddParagraph("APPOITNMENT TIME").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[3].AddParagraph("PAYMENT").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[4].AddParagraph("PAYMENT DATE").Format.Font.Bold = true;
            prescriptionListInfoRow.Cells[5].AddParagraph("PAYMENT MODE").Format.Font.Bold = true;

            foreach (var item in patient)
            {
                prescriptionListInfoRow = prescriptionListInfoTable.AddRow();
                prescriptionListInfoRow = prescriptionListInfoTable.AddRow();
                //prescriptionListInfoRow.Format.Font.Size = 10;
                prescriptionListInfoRow.BottomPadding = 5;
                prescriptionListInfoRow.TopPadding = 5;
                prescriptionListInfoRow.Format.Alignment = ParagraphAlignment.Left;
                prescriptionListInfoRow.Cells[0].AddParagraph(item.FullName ?? "");
                prescriptionListInfoRow.Cells[1].AddParagraph(item.AppointmentDate ?? "");
                prescriptionListInfoRow.Cells[2].AddParagraph(item.AppointmentTime ?? "");
                prescriptionListInfoRow.Cells[3].AddParagraph((item.NetAmount).ToString() ?? "");
                prescriptionListInfoRow.Cells[4].AddParagraph(item.PaymentDate ?? "");
                prescriptionListInfoRow.Cells[5].AddParagraph(item.PaymentMode ?? "");
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


        public JsonModel GetAppointmentRefundList(RefundFilterModel refundFilterModel, TokenModel tokenModel)
        {
            var dbUser = _tokenService.GetUserById(tokenModel);

            if (dbUser == null)
                return new JsonModel(null, StatusMessage.UserNotFound, (int)HttpStatusCodes.NotFound, string.Empty);

            if (dbUser.UserRoles.RoleName != OrganizationRoles.Admin.ToString())
                refundFilterModel.StaffId = tokenModel.StaffID.ToString();

            LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);

            if (!string.IsNullOrEmpty(refundFilterModel.RefundDate))
                refundFilterModel.RefundDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(refundFilterModel.RefundDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            if (!string.IsNullOrEmpty(refundFilterModel.AppDate))
                refundFilterModel.AppDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(refundFilterModel.AppDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            var appointmentRefunds = _appointmentPaymentRepository.GetAppointmentRefunds<AppointmentRefundListingModel>(refundFilterModel, tokenModel).ToList();

            if (appointmentRefunds != null && appointmentRefunds.Count > 0)
            {
                appointmentRefunds.ForEach(x =>
                {
                    LocationModel locModel = _locationService.GetLocationOffsets(x.ServiceLocationID, tokenModel);
                    x.StartTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locModel.DaylightOffset, locModel.StandardOffset, locModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.EndTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.EndDateTime), locModel.DaylightOffset, locModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.RefundDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.CreatedDate), locModel.DaylightOffset, locModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locModel.DaylightOffset, locModel.StandardOffset, locModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentTime = x.StartTime + " - " + x.EndTime;
                });

                _response = new JsonModel()
                {
                    data = appointmentRefunds,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCode.Found,
                    meta = new Meta(appointmentRefunds, refundFilterModel)
                };


            }
            else
                _response = new JsonModel(null, StatusMessage.AppointmentPaymentNotFound, (int)HttpStatusCodes.NotFound, string.Empty);
            return _response;
        }


        public JsonModel GetClientAppointmentPaymentList(PaymentFilterModel paymentFilterModel, TokenModel tokenModel)
        {

            paymentFilterModel.ClientId = tokenModel.StaffID.ToString();
            LocationModel locationModel = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);

           
            if (!string.IsNullOrEmpty(paymentFilterModel.PayDate))
                paymentFilterModel.PayDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.PayDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            if (!string.IsNullOrEmpty(paymentFilterModel.AppDate))
                paymentFilterModel.AppDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.AppDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            if (!string.IsNullOrEmpty(paymentFilterModel.RangeStartDate))
                paymentFilterModel.RangeStartDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.RangeStartDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();

            if (!string.IsNullOrEmpty(paymentFilterModel.RangeEndDate))
                paymentFilterModel.RangeEndDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(paymentFilterModel.RangeEndDate, '/'), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName).ToString();


            var appointmentPayments = _appointmentPaymentRepository.GetClientAppointmentPayments<AppointmentPaymentListingModel>(paymentFilterModel, tokenModel).ToList();

            if (appointmentPayments != null && appointmentPayments.Count > 0)
            {
                appointmentPayments.ForEach(x =>
                {
                    LocationModel locationModel = _locationService.GetLocationOffsets(x.ServiceLocationID, tokenModel);
                    x.StartTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.EndTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.EndDateTime), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.PaymentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.CreatedDate), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentTime = x.StartTime + " - " + x.EndTime;
                });

                _response = new JsonModel
                {
                    data = appointmentPayments,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCode.Found,
                    meta = new Meta(appointmentPayments, paymentFilterModel)
                };

            }
            else
                _response = new JsonModel(null, StatusMessage.AppointmentPaymentNotFound, (int)HttpStatusCodes.NotFound, string.Empty);
            return _response;
        }

        public JsonModel GetClientAppointmentRefundList(RefundFilterModel refundFilterModel, TokenModel tokenModel)
        {
            refundFilterModel.ClientId = tokenModel.StaffID.ToString();

            if (!string.IsNullOrEmpty(refundFilterModel.RefundDate))
                refundFilterModel.RefundDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(refundFilterModel.RefundDate, '/'), tokenModel.OffSet, 0, tokenModel.Timezone).ToString();

            if (!string.IsNullOrEmpty(refundFilterModel.AppDate))
                refundFilterModel.AppDate = Common.CommonMethods.ConvertToUtcTimeWithOffset(Common.CommonMethods.convertStringToTime(refundFilterModel.AppDate, '/'), tokenModel.OffSet, 0, tokenModel.Timezone).ToString();

            var appointmentRefunds = _appointmentPaymentRepository.GetClientAppointmentRefunds<AppointmentRefundListingModel>(refundFilterModel, tokenModel).ToList();

            if (appointmentRefunds != null && appointmentRefunds.Count > 0)
            {
                appointmentRefunds.ForEach(x =>
                {
                    LocationModel locationModel = _locationService.GetLocationOffsets(x.ServiceLocationID, tokenModel);
                    x.StartTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.EndTime = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.EndDateTime), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToShortTimeString();
                    x.RefundDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.CreatedDate), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(Convert.ToDateTime(x.StartDateTime), locationModel.DaylightOffset, locationModel.StandardOffset, locationModel.TimeZoneName, tokenModel).ToString("MMM dd, yyyy");
                    x.AppointmentTime = x.StartTime + " - " + x.EndTime;
                });

                _response = new JsonModel()
                {
                    data = appointmentRefunds,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCode.Found,
                    meta = new Meta(appointmentRefunds, refundFilterModel)
                };


            }
            else
                _response = new JsonModel(null, StatusMessage.AppointmentPaymentNotFound, (int)HttpStatusCodes.NotFound, string.Empty);
            return _response;
        }
        public JsonModel GetClientNetAppointmentPayment(int clientId, TokenModel tokenModel)
        {

            
            var appointmentPayments = _appointmentPaymentRepository.GetClientNetAppointmentPayment<ClientPaymentModel>(clientId, tokenModel);

            if (appointmentPayments != null)
            {


                _response = new JsonModel
                {
                    data = appointmentPayments,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCode.Found,
                    meta = new Meta(appointmentPayments, clientId)
                };

            }
            else
                _response = new JsonModel(null, StatusMessage.AppointmentPaymentNotFound, (int)HttpStatusCodes.NotFound, string.Empty);
            return _response;
        }
        public JsonModel GetPastUpcomingAppointment(string locationIds, string staffIds, string patientIds, TokenModel tokenModel)
        {
            var dbUser = _tokenService.GetUserById(tokenModel);

            if (dbUser == null)
                return new JsonModel(null, StatusMessage.UserNotFound, (int)HttpStatusCodes.NotFound, string.Empty);

            if (staffIds == null)
            { staffIds = ""; }
            var appointmentInfo= _appointmentPaymentRepository.GetPastUpcomingAppointment<ClientAppointmentPastUpcomingModel>(locationIds, staffIds, patientIds, tokenModel);

            if (appointmentInfo != null )
            {
               

                _response = new JsonModel
                {
                    data = appointmentInfo,
                    Message = StatusMessage.FetchMessage,
                    StatusCode = (int)HttpStatusCode.Found,
                    meta = new Meta(appointmentInfo, null)
                };

            }
            else
                _response = new JsonModel(null, StatusMessage.AppointmentPaymentNotFound, (int)HttpStatusCodes.NotFound, string.Empty);
            return _response;
        }


    }
}
