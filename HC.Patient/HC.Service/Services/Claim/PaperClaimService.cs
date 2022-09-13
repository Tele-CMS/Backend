using HC.Common.Enums;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Repositories.IRepositories.Claim;
using HC.Patient.Service.IServices.Claim;
using HC.Service;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services.Claim
{

    public class PaperClaimService : BaseService, IPaperClaimService
    {
        private IClaimRepository _claimRepository;
        XElement inputXML = null;
        public PaperClaimService(IClaimRepository claimRepository)
        {
            _claimRepository = claimRepository;
        }

        public MemoryStream GenerateBatchPaperClaims(string claimIds, string payerPreference, bool markSubmitted, int printFormat, TokenModel token)
        {
            try
            {
                PaperClaimModel paperClaim = _claimRepository.GetBatchPaperClaimInfo(claimIds, payerPreference, token.OrganizationID);
                MemoryStream memoryStream = GenerateBatchPaperClaim(paperClaim, printFormat); //GenerateBatchPaperClaim(paperClaim, printFormat);
                if (memoryStream != null)
                {
                    inputXML = GetClaimHistoryForPaperClaim(claimIds);
                    if (!markSubmitted)
                        _claimRepository.SaveClaimHistory<SQLResponseModel>(null, inputXML, ClaimHistoryAction.PrintPaperClaim, DateTime.UtcNow, token).FirstOrDefault();
                    else
                    {
                        List<int> claimids = new List<int>();
                        claimids = claimIds.Split(',').Select(Int32.Parse).ToList();
                        var claims = _claimRepository.GetAll(a => claimids.Contains(a.Id)).AsQueryable();
                        if (claims != null)
                        {
                            claims.ToList().ForEach(x =>
                            {
                                x.ClaimStatusId = (int)CommonEnum.MasterStatusClaim.Submitted;
                                x.UpdatedBy = token.UserID;
                                x.SubmissionType = Convert.ToInt16(CommonEnum.ClaimSubmissionType.PaperClaim);
                                x.UpdatedDate = DateTime.UtcNow;
                                x.SubmittedDate = DateTime.UtcNow;                                
                            });
                            _claimRepository.Update(claims.ToArray());
                            _claimRepository.SaveChanges();
                        }                        
                        _claimRepository.SaveClaimHistory<SQLResponseModel>(null, inputXML, ClaimHistoryAction.PrintAndSubmitPaperClaim, DateTime.UtcNow, token).FirstOrDefault();
                    }
                }
                return memoryStream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public MemoryStream GeneratePaperClaim(int claimId, int patientInsuranceId, bool markSubmitted, int printFormat, TokenModel token)
        {
            try
            {
                PaperClaimModel paperClaim = _claimRepository.GetPaperClaimInfo(claimId, patientInsuranceId, token.OrganizationID);
                MemoryStream memoryStream = GenerateBatchPaperClaim(paperClaim, printFormat);
                if (memoryStream != null)
                {
                    inputXML = GetClaimHistoryForPaperClaim(claimId.ToString());
                    if (!markSubmitted)
                        _claimRepository.SaveClaimHistory<SQLResponseModel>(claimId, inputXML, ClaimHistoryAction.PrintPaperClaim, DateTime.UtcNow, token).FirstOrDefault();
                    else
                    {
                        Claims claim = _claimRepository.GetByID(claimId);
                        if (claim != null)
                        {
                            claim.ClaimStatusId = (int)CommonEnum.MasterStatusClaim.Submitted;
                            claim.UpdatedBy = token.UserID;
                            claim.SubmissionType = Convert.ToInt16(CommonEnum.ClaimSubmissionType.PaperClaim);
                            claim.UpdatedDate = DateTime.UtcNow;
                            claim.SubmittedDate = DateTime.UtcNow;
                            _claimRepository.Update(claim);
                            _claimRepository.SaveChanges();
                        }
                        _claimRepository.SaveClaimHistory<SQLResponseModel>(claimId, inputXML, ClaimHistoryAction.PrintAndSubmitPaperClaim, DateTime.UtcNow, token).FirstOrDefault();
                    }
                }
                return memoryStream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public MemoryStream GeneratePaperClaim_Secondary(int claimId, int patientInsuranceId, int printFormat, TokenModel token)
        {
            try
            {
                PaperClaimModel paperClaim = _claimRepository.GetPaperClaimInfo_Secondary(claimId, patientInsuranceId, token.OrganizationID);
                MemoryStream memoryStream = GenerateBatchPaperClaim(paperClaim, printFormat);
                if (memoryStream != null)
                {
                    inputXML = GetClaimHistoryForPaperClaim(claimId.ToString());
                    _claimRepository.SaveClaimHistory<SQLResponseModel>(claimId, inputXML, ClaimHistoryAction.PrintPaperClaimForSecondary, DateTime.UtcNow, token).FirstOrDefault();

                }
                return memoryStream;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public XElement GetClaimHistoryForPaperClaim(string claimIds)
        {
            inputXML = new XElement("Parent");
            {
                claimIds.Split(",").ToList().ForEach(x =>
                {
                    inputXML.Add(new XElement("Child",
                     new XElement("ClaimId", Convert.ToInt32(x)),
                     new XElement("ColumnName", string.Empty),
                     new XElement("OldValue", string.Empty),
                     new XElement("NewValue", string.Empty)
                    ));
                });
            }
            return inputXML;
        }
        //public MemoryStream GenerateBatchPaperClaim(PaperClaimModel paperClaim)
        public MemoryStream GenerateBatchPaperClaim(PaperClaimModel paperClaim, int printFormat)

        {   //pdf
            //int x = 45;
            //int y = 35;
            //int x = 49;
            //int y = 35;
            int x = 0;
            int y = 0;
            MemoryStream tempStream = null;
            var fullPath = Directory.GetCurrentDirectory();
            PdfDocument PDFDoc = null;
            if (printFormat == 1)
            {
                PDFDoc = PdfReader.Open(fullPath + "\\SEFFiles\\HCFA_1500.pdf", PdfDocumentOpenMode.Import);
            }
            else
            {
                PDFDoc = PdfReader.Open(fullPath + "\\SEFFiles\\PrePrintedHCFA_1500.pdf", PdfDocumentOpenMode.Import);
                x = 45;
                y = 35;
            }
            PdfDocument PDFNewDoc = new PdfSharp.Pdf.PdfDocument();
            //return null;
            // int addpage = 0;
            ClaimDetailsModel claim = null;
            InsuredModel insuredDetails = null;
            InsuredModel otherInsuredDetails = null;
            List<ServiceCodesModel> serviceCodeList = null;
            List<DiagnosisCodesModel> diagnosisCodeList = null;
            OrganizationDetailsModel organizationDetailsModel = null;
            LocationAddressModel serviceAddress = null;
            LocationAddressModel billingAddress = null;
            List<PayerPaymentModel> payerPayment = null;
            int pages = 0;
            foreach (ClaimDetailsModel responseClaimDetais in paperClaim.ClaimDetails)
            {
                claim = responseClaimDetais;
                insuredDetails = paperClaim.InsuredDetails.Find(p => p.ClaimId == claim.ClaimId);
                otherInsuredDetails = paperClaim.OtherInsuredDetails.Find(p => p.ClaimId == claim.ClaimId);
                serviceCodeList = paperClaim.ServiceCodes.FindAll(p => p.ClaimId == claim.ClaimId);
                diagnosisCodeList = paperClaim.DiagnosisCodes.FindAll(p => p.ClaimId == claim.ClaimId);
                organizationDetailsModel = paperClaim.OrganizationDetails;
                serviceAddress = paperClaim.ServiceLocation;
                billingAddress = paperClaim.BillingLocation;
                payerPayment = (paperClaim.PayerPayments != null && paperClaim.PayerPayments.Count > 0) ? paperClaim.PayerPayments.FindAll(p => p.ClaimId == claim.ClaimId).ToList() : null;
                int noOfPages = (int)Math.Ceiling((decimal)serviceCodeList.Count / (decimal)6);
                int count = 0;
                for (int Pg = 0; Pg < noOfPages; Pg++)
                {
                    if (count > 0)
                        serviceCodeList.RemoveRange(0, 6);
                    count = count + 1;

                    //If you want to create multiple forms with more than 6 cptcodes
                    PDFNewDoc.AddPage(PDFDoc.Pages[0]);
                    PdfPage page = PDFNewDoc.Pages[pages];
                    pages = pages + 1;
                    //PdfPage page = PDFDoc.Pages[0];
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont font = new XFont("Courier New", 9, XFontStyle.Regular);
                    XFont fontForCross = new XFont("Courier New", 11, XFontStyle.Regular);
                    //details to be inserted only one time
                    if (claim != null)
                    {

                        #region Insurance Address
                        gfx.DrawString(String.Format("{0}", claim.PayerName).ToUpper(), font, XBrushes.Black, 380, 50, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0}", claim.PayerAddress + (!string.IsNullOrEmpty(claim.PayerApartmentNumber) ? " " + claim.PayerApartmentNumber : "")).ToUpper(), font, XBrushes.Black, 380, 60, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0}, {1} {2}", claim.PayerCity, claim.PayerState, claim.PayerPostalCode).ToUpper(), font, XBrushes.Black, 380, 70, XStringFormats.TopLeft);
                        //1.a
                        gfx.DrawString(Convert.ToString(claim.InsuredIDNumber == null ? "" : claim.InsuredIDNumber), font, XBrushes.Black, 415 - x, 144 - y, XStringFormats.TopLeft);//
                        #endregion
                        //first row
                        switch (claim.InsurancePlanType)
                        {
                            case "MEDICARE":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 63 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "MEDICAID":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 112 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "TRICARE":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 165 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "CHAMPVA":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 226 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "GROUP HEALTH PLAN":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 278 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "FECA":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 335 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            default:
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 378 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                        }
                        ////Second Row
                        //patient information
                        gfx.DrawString(string.Format("{0}, {1} {2}", claim.SubsLastName, claim.SubsFirstName, claim.SubsMiddleName).ToUpper(), font, XBrushes.Black, 63 - x, 166 - y, XStringFormats.TopLeft);
                        //third row
                        gfx.DrawString(String.Format("{0}", claim.SubsAddress1 + (!string.IsNullOrEmpty(claim.SubsApartmentNumber) ? " " + claim.SubsApartmentNumber : "")).ToUpper(), font, XBrushes.Black, 63 - x, 190 - y, XStringFormats.TopLeft);

                        gfx.DrawString(String.Format("{0}", claim.SubsCity).ToUpper(), font, XBrushes.Black, 63 - x, 215 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0}", claim.SubsState).ToUpper(), font, XBrushes.Black, 245 - x, 215 - y, XStringFormats.TopLeft);

                        gfx.DrawString(String.Format("{0:00}", claim.SubsDOB.Month), font, XBrushes.Black, 279 - x, 169 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0:00}", claim.SubsDOB.Day), font, XBrushes.Black, 303 - x, 169 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0:00}", claim.SubsDOB.Year.ToString().Substring(2, 2)), font, XBrushes.Black, 329 - x, 169 - y, XStringFormats.TopLeft);

                        if (claim.SubsGender.ToUpper() == "MALE")
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 357.5 - x, 168 - y, XStringFormats.TopLeft);
                        else if (claim.SubsGender.ToUpper() == "FEMALE")
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 393.5 - x, 168 - y, XStringFormats.TopLeft);

                        //insured information
                        if (insuredDetails != null)
                        {

                            ////Seventh Row
                            //chkBoxTop += 24.3;              
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredCity).ToUpper(), font, XBrushes.Black, 415 - x, 215 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredState).ToUpper(), font, XBrushes.Black, 585 - x, 215 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}, {1} {2}", insuredDetails.InsuredLastName, insuredDetails.InsuredFirstName, insuredDetails.InsuredMiddleName).ToUpper(), font, XBrushes.Black, 420 - x, 168 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredZip).ToUpper(), font, XBrushes.Black, 415 - x, 240 - y, XStringFormats.TopLeft);
                            if (insuredDetails.InsuredPhone != null)
                            {
                                string strInsuredAreaCode = string.Empty;
                                string strInsuredRestPhoneNo = string.Empty;

                                if (!insuredDetails.InsuredPhone.Contains(")"))
                                {
                                    string[] arrInsuredPhoneNo = insuredDetails.InsuredPhone.Split('-');
                                    if (arrInsuredPhoneNo.Length > 0)
                                    {
                                        if (arrInsuredPhoneNo.Length == 3)
                                        {
                                            strInsuredAreaCode = arrInsuredPhoneNo[0];
                                            strInsuredRestPhoneNo = arrInsuredPhoneNo[1] + "-" + arrInsuredPhoneNo[2];
                                        }
                                        else
                                        {
                                            strInsuredRestPhoneNo = insuredDetails.InsuredPhone;
                                        }
                                    }
                                }
                                else
                                {
                                    string[] arrInsuredPhoneNo = insuredDetails.InsuredPhone.Split(')');
                                    strInsuredAreaCode = Regex.Match(arrInsuredPhoneNo[0], @"\d+").Value;
                                    strInsuredRestPhoneNo = string.Empty;
                                    strInsuredRestPhoneNo = arrInsuredPhoneNo == null ? "" : arrInsuredPhoneNo[1];
                                }
                                gfx.DrawString(strInsuredAreaCode, font, XBrushes.Black, 525 - x, 240 - y, XStringFormats.TopLeft);
                                gfx.DrawString(strInsuredRestPhoneNo, font, XBrushes.Black, 555 - x, 240 - y, XStringFormats.TopLeft);
                            }
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredAddress1 + (!string.IsNullOrEmpty(insuredDetails.InsuredApartmentNumber) ? " " + insuredDetails.InsuredApartmentNumber : "")).ToUpper(), font, XBrushes.Black, 418 - x, 190 - y, XStringFormats.TopLeft);

                            //if (insuredDetails.InsurancePersonSameAsPatient == false)
                            //{
                            if (insuredDetails.InsuredDOB != DateTime.MinValue)
                            {
                                gfx.DrawString(String.Format("{0:00}", insuredDetails.InsuredDOB.Month), font, XBrushes.Black, 439 - x, 289 - y, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0:00}", insuredDetails.InsuredDOB.Day), font, XBrushes.Black, 461 - x, 289 - y, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0:00}", insuredDetails.InsuredDOB.Year.ToString().Substring(2, 2)), font, XBrushes.Black, 488 - x, 289 - y, XStringFormats.TopLeft);
                            }
                            if (insuredDetails.InsuredGender.ToUpper() == "MALE")
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 544 - x, 288 - y, XStringFormats.TopLeft);
                            else if (insuredDetails.InsuredGender.ToUpper() == "FEMALE")
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 595 - x, 288 - y, XStringFormats.TopLeft);
                            //}

                            //relationship
                            if (insuredDetails.InsuredRelation.Contains("SELF"))
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 292 - x, 192 - y, XStringFormats.TopLeft);

                            }
                            else if (insuredDetails.InsuredRelation.Contains("SPOUSE"))
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 328.5 - x, 192 - y, XStringFormats.TopLeft);

                            }
                            else if (insuredDetails.InsuredRelation.Contains("CHILD"))
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 357.5 - x, 192 - y, XStringFormats.TopLeft);
                            }
                            else
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 393.5 - x, 192 - y, XStringFormats.TopLeft);
                            }

                            //eleventh
                            gfx.DrawString(insuredDetails.InsuranceGroupName == null ? "" : insuredDetails.InsuranceGroupName, font, XBrushes.Black, 415 - x, 263 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsurancePlanName).ToUpper(), font, XBrushes.Black, 415 - x, 335 - y, XStringFormats.TopLeft);
                            gfx.DrawString(insuredDetails.AuthorizationNumber == null ? string.Empty : insuredDetails.AuthorizationNumber, font, XBrushes.Black, 420 - x, 525 - y, XStringFormats.TopLeft);
                        }

                        if (otherInsuredDetails != null)
                        {

                            gfx.DrawString(String.Format("{0}, {1} {2}", otherInsuredDetails.InsuredLastName, otherInsuredDetails.InsuredFirstName, otherInsuredDetails.InsuredMiddleName).ToUpper(), font, XBrushes.Black, 63 - x, 263 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", otherInsuredDetails.InsuranceGroupName).ToUpper(), font, XBrushes.Black, 63 - x, 285 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", otherInsuredDetails.InsurancePlanName).ToUpper(), font, XBrushes.Black, 63 - x, 355 - y, XStringFormats.TopLeft);
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 429 - x, 359 - y, XStringFormats.TopLeft);

                        }
                        else
                        {
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 465 - x, 359 - y, XStringFormats.TopLeft);
                        }

                        //insured information
                        //third row

                        gfx.DrawString(String.Format("{0}", claim.SubsEmail).ToUpper(), font, XBrushes.Black, 63 - x, 190 - y, XStringFormats.TopLeft);

                        //fifth row
                        //chkBoxTop += 24.1;
                        gfx.DrawString(String.Format("{0}", claim.SubsPostalCode).ToUpper(), font, XBrushes.Black, 63 - x, 240 - y, XStringFormats.TopLeft);
                        string strPhone = !String.IsNullOrEmpty(claim.SubsPhoneHome) ? claim.SubsPhoneHome.Trim() : (!string.IsNullOrEmpty(claim.SubsPhoneMobile) ? claim.SubsPhoneMobile.Trim() : string.Empty);
                        string strRestPhoneNo = string.Empty;
                        string strAreaCode = string.Empty;
                        if (strPhone.Contains("("))
                        {
                            string[] arrPhoneNo = strPhone.Split(')');
                            if (arrPhoneNo[0] != null)
                                strAreaCode = Regex.Match(arrPhoneNo[0], @"\d+").Value;
                            if (arrPhoneNo[1] != null)
                            {
                                strRestPhoneNo = arrPhoneNo[1];
                            }
                        }
                        else
                        {

                            string[] arrPhoneNo = strPhone.Split('-');

                            if (arrPhoneNo.Length > 0)
                            {
                                if (arrPhoneNo.Length == 3)
                                {
                                    strAreaCode = arrPhoneNo[0];
                                    strRestPhoneNo = arrPhoneNo[1] + "-" + arrPhoneNo[2];
                                }
                                else
                                {
                                    strRestPhoneNo = strPhone;
                                }
                            }
                        }
                        gfx.DrawString(strAreaCode, font, XBrushes.Black, 167 - x, 240 - y, XStringFormats.TopLeft);
                        gfx.DrawString(strRestPhoneNo, font, XBrushes.Black, 197 - x, 240 - y, XStringFormats.TopLeft);

                        #region 10 Row Old
                        ////Tenth row
                        //chkBoxTop += 23.9;

                        //if (Claim.IsAccident)
                        //{
                        //    switch (Claim.AccidentReasonId)
                        //    {
                        //        case 1:
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 307 - x, 287 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 312 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 335 - y, XStringFormats.TopLeft);

                        //            break;
                        //        case 2:
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 307 - x, 312 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 287 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - y, 335 - x, XStringFormats.TopLeft);
                        //            gfx.DrawString(Claim.State, fontForCross, XBrushes.Black, 380 - x, 312 - y, XStringFormats.TopLeft);

                        //            break;
                        //        default:
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 307 - x, 335 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 287 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 312 - y, XStringFormats.TopLeft);
                        //            break;

                        //    }
                        //}
                        //else
                        //{
                        #endregion

                        gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 312 - y, XStringFormats.TopLeft);
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 287 - y, XStringFormats.TopLeft);
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 335 - y, XStringFormats.TopLeft);

                        //}
                        //Twelveth row
                        //chkBoxTop += 45;
                        gfx.DrawString("SIGNATURE ON FILE", font, XBrushes.Black, 120 - x, 406 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0:MM/dd/yyyy}", claim.ClientRecordInsertionDate), font, XBrushes.Black, 320 - x, 406 - y, XStringFormats.TopLeft);
                        gfx.DrawString("SIGNATURE ON FILE", font, XBrushes.Black, 460 - x, 406 - y, XStringFormats.TopLeft);


                        ////Fourteenth row

                        //chkBoxTop += 23;
                        //if (claim.DOS != null)
                        //{
                        //    gfx.DrawString(String.Format("{0:00}", claim.DOS.Month), font, XBrushes.Black, 70 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:00}", claim.DOS.Day), font, XBrushes.Black, 90 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:0000}", claim.DOS.Year), font, XBrushes.Black, 110 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString("431", font, XBrushes.Black, 180 - x, 434 - y, XStringFormats.TopLeft);
                        //}

                        ////Fourteenth row (box 15 

                        //if (Claim.XrayDate != null)
                        //{
                        //    gfx.DrawString("454", fontForCross, XBrushes.Black, 276 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:00}", Claim.XrayDate.Value.Month), font, XBrushes.Black, 324 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:00}", Claim.XrayDate.Value.Day), font, XBrushes.Black, 344 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:0000}", Claim.XrayDate.Value.Year), font, XBrushes.Black, 364 - x, 434 - y, XStringFormats.TopLeft);

                        //}

                        //Seventeenth row


                        //if (Claim.PayerInsType == "Medicare" && Claim.Xray)
                        //{
                        //gfx.DrawString(Convert.ToString(Claim.QualifierType), font, XBrushes.Black, 68 - x, 455 - y, XStringFormats.TopLeft);
                        //gfx.DrawString(Convert.ToString(Claim.ReferringProviderName), font, XBrushes.Black, 87 - x, 455 - y, XStringFormats.TopLeft);
                        //gfx.DrawString(Convert.ToString(Claim.RefNPINumber), font, XBrushes.Black, 300 - x, 457 - y, XStringFormats.TopLeft);

                        // }



                        ////Nineteenth row (box 19 -It will be included when the Include standard text bit from payer screen under practicemanagemnent
                        //is true and the value will be AdditionalClaimInfo from payer table)

                        //if (!string.IsNullOrEmpty(Claim.AdditionalClaimInfo))
                        gfx.DrawString(claim.AdditionalClaimInfo, font, XBrushes.Black, 63 - x, 478 - y, XStringFormats.TopLeft);

                        //Box 21
                        gfx.DrawString("0", font, XBrushes.Black, 358 - x, 495 - y, XStringFormats.TopLeft);
                        //chkBoxTop += 24;
                        for (int i = 0; i < diagnosisCodeList.Count; i++)
                        {
                            int marginleft = 79;
                            int margintop = 505;
                            switch (i)
                            {

                                case 1:
                                    marginleft = 170;
                                    break;
                                case 2:
                                    marginleft = 265;
                                    break;
                                case 3:
                                    marginleft = 358;
                                    break;
                                case 4:
                                    margintop = 518;
                                    break;
                                case 5:
                                    margintop = 518;
                                    marginleft = 170;
                                    break;
                                case 6:
                                    margintop = 518;
                                    marginleft = 265;
                                    break;
                                case 7:
                                    margintop = 518;
                                    marginleft = 358;
                                    break;
                                case 8:
                                    margintop = 530;
                                    break;
                                case 9:
                                    margintop = 530;
                                    marginleft = 170;
                                    break;
                                case 10:
                                    margintop = 530;
                                    marginleft = 265;
                                    break;
                                case 11:
                                    margintop = 530;
                                    marginleft = 358;
                                    break;

                            }

                            gfx.DrawString(diagnosisCodeList[i].DiagnosisCode.ToUpper(), font, XBrushes.Black, marginleft - x, margintop - y, XStringFormats.TopLeft);// diagnosis 1 a
                        }

                        gfx.DrawString((serviceCodeList != null && serviceCodeList.Count > 0) ? serviceCodeList.First().AuthorizationNumber : string.Empty, font, XBrushes.Black, 418 - x, 530 - y, XStringFormats.TopLeft);


                        //cptcodes
                        double lineItemHeight = 577;
                        decimal TotalCharges = 0;
                        decimal AmountPaid = 0;
                        XFont lineItemFont = new XFont("Courier New", 8, XFontStyle.Regular);
                        for (int j = 0; j < serviceCodeList.Count && j < 6; j++)
                        {
                            //if (j == 0)
                            //{
                            TotalCharges += serviceCodeList[j].TotalAmount;
                            AmountPaid += serviceCodeList[j].AmountPaid;//+= CptCodes[0].AmountPaid;
                                                                        //TotalCharges = CptCodes.Sum(a => a.TotalCharges); //+= CptCodes[0].TotalCharges;
                                                                        //AmountPaid = CptCodes.Sum(a => a.AmountPaid);//+= CptCodes[0].AmountPaid;

                            //}
                            if (serviceCodeList[j].DOS != null)
                            {
                                if (claim.IncludeServiceTimeWithServiceCode)
                                    gfx.DrawString(claim.AppointmentStartTime.ToString("hhmm tt") + " - " + claim.AppointmentEndTime.ToString("hhmm tt"), lineItemFont, XBrushes.Black, 65 - x, (lineItemHeight - 11) - y, XStringFormats.TopLeft);
                                if (DateTime.MinValue != serviceCodeList[j].DOS)
                                {
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Month), lineItemFont, XBrushes.Black, 65 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Day), lineItemFont, XBrushes.Black, 85 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:0000}", serviceCodeList[j].DOS.Year), lineItemFont, XBrushes.Black, 103 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                }

                                if (DateTime.MinValue != serviceCodeList[j].DOS)
                                {
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Month), lineItemFont, XBrushes.Black, 126 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Day), lineItemFont, XBrushes.Black, 147.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:0000}", serviceCodeList[j].DOS.Year), lineItemFont, XBrushes.Black, 166 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                }
                            }

                            gfx.DrawString(serviceCodeList[j].ServiceFacilityCode, lineItemFont, XBrushes.Black, 189.5 - x, lineItemHeight - y, XStringFormats.TopLeft);

                            //  gfx.DrawString(tempItem.EMG.ToUpper(), lineItemFont, XBrushes.Black, 211.5, lineItemHeight, XStringFormats.TopLeft);

                            if (serviceCodeList[j].ServiceCode != "")
                            {
                                //gfx.DrawString(String.Format("{0:0.##}", tempItem.CPTCode), lineItemFont, XBrushes.Black, 235.5, lineItemHeight, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0:0.##}", serviceCodeList[j].ServiceCode), lineItemFont, XBrushes.Black, 240.5 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                gfx.DrawString(String.Format("{0:0}", (int)serviceCodeList[j].TotalAmount), lineItemFont, XBrushes.Black, 420 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                int altChargeDecimal = (int)((serviceCodeList[j].TotalAmount - (int)serviceCodeList[j].TotalAmount) * 100);

                                gfx.DrawString(String.Format("{0:00}", altChargeDecimal), lineItemFont, XBrushes.Black, 460 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0}", serviceCodeList[j].Quantity), lineItemFont, XBrushes.Black, 480 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                #region Modifiers
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier1) ? serviceCodeList[j].Modifier1 : string.Empty, lineItemFont, XBrushes.Black, 290.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier2) ? serviceCodeList[j].Modifier2 : string.Empty, lineItemFont, XBrushes.Black, 310.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier3) ? serviceCodeList[j].Modifier3 : string.Empty, lineItemFont, XBrushes.Black, 330.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier4) ? serviceCodeList[j].Modifier4 : string.Empty, lineItemFont, XBrushes.Black, 350.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                #endregion


                                #region Pointers 24E
                                string allPointers = (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer1) ? serviceCodeList[j].DiagnosisCodePointer1 : string.Empty) + (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer2) ? serviceCodeList[j].DiagnosisCodePointer2 : string.Empty)
                                                    + (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer3) ? serviceCodeList[j].DiagnosisCodePointer3 : string.Empty) + (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer4) ? serviceCodeList[j].DiagnosisCodePointer4 : string.Empty);

                                gfx.DrawString(allPointers, lineItemFont, XBrushes.Black, 375 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                #endregion

                                //24H
                                //  gfx.DrawString(String.Format("{0}",Claim.NPINumber).ToUpper(), lineItemFont, XBrushes.Black, 505, lineItemHeight, XStringFormats.TopLeft);

                                #region 24i and 24j shaded area commented instructed by scott
                                ////24i unshaded id-qualifier TPI or SSN or EIN
                                //if (Claim.isSSN)
                                //{
                                //    gfx.DrawString(String.Format("{0}", "SY").ToUpper(), lineItemFont, XBrushes.Black, 525, lineItemHeight - 12, XStringFormats.TopLeft);
                                //}
                                //else
                                //{
                                //    gfx.DrawString(String.Format("{0}", "E1").ToUpper(), lineItemFont, XBrushes.Black, 525, lineItemHeight - 12, XStringFormats.TopLeft);


                                //}
                                //rendering provider id 24j NPI
                                //  gfx.DrawString(String.Format("{0}", Claim.TaxId).ToUpper(), lineItemFont, XBrushes.Black, 550, lineItemHeight - 12, XStringFormats.TopLeft);

                                gfx.DrawString(String.Format("{0}", serviceCodeList[j].NPINumber).ToUpper(), lineItemFont, XBrushes.Black, 550 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                #endregion
                            }
                            lineItemHeight += 24.2;

                        }
                        ////25. 

                        //chkBoxTop += 141.5;
                        // IsSSn or EIN Checkboxes
                        gfx.DrawString(String.Format("{0}", billingAddress.TaxId).ToUpper(), font, XBrushes.Black, 69 - x, 720 - y, XStringFormats.TopLeft);
                        //if (Claim.isSSN)
                        //{
                        //    gfx.DrawString("X", fontForCross, XBrushes.Black, 178 - x, 718 - y, XStringFormats.TopLeft);
                        //}
                        //else
                        //{
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 193 - x, 718 - y, XStringFormats.TopLeft);
                        //}

                        //sec 26 and 27
                        gfx.DrawString(claim.SubsAccountNumber, font, XBrushes.Black, 230 - x, 718 - y, XStringFormats.TopLeft);
                        //if (Claim.AcceptAssignment)
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 330 - x, 718 - y, XStringFormats.TopLeft);
                        // else
                        //   gfx.DrawString("X", fontForCross, XBrushes.Black, 365 - x, 718 - y, XStringFormats.TopLeft);

                        //sec 26 and 27
                        #region service location information
                        //chkBoxTop += 9;


                        if (serviceAddress != null)
                        {
                            //if (serviceAddress.Alias != null)
                            //    gfx.DrawString(serviceAddress.Alias.ToUpper(), lineItemFont, XBrushes.Black, 220, 743, XStringFormats.TopLeft);
                            //else
                            gfx.DrawString(serviceAddress.ServiceFacilityLocationName.ToUpper(), lineItemFont, XBrushes.Black, 220, 743, XStringFormats.TopLeft);

                            if (serviceAddress.Address1 != null)
                            {
                                string street = string.Empty;
                                street = serviceAddress.Address1;

                                if (!String.IsNullOrEmpty(serviceAddress.Address2))
                                {
                                    street += " " + serviceAddress.Address2;
                                }

                                street += (!string.IsNullOrEmpty(serviceAddress.ApartmentNumber) ? " " + serviceAddress.ApartmentNumber : "");

                                gfx.DrawString(street.ToUpper(), lineItemFont, XBrushes.Black, 220 - x, 752 - y, XStringFormats.TopLeft);//752
                                gfx.DrawString(String.Format("{0} {1} {2}", serviceAddress.City, serviceAddress.State, serviceAddress.Zip).ToUpper(), lineItemFont, XBrushes.Black, 220 - x, 764 - y, XStringFormats.TopLeft);//764
                            }
                        }
                        #endregion
                        #region billing address  33


                        if (billingAddress == null)
                        {
                            billingAddress = serviceAddress;
                        }
                        if (billingAddress != null)
                        {
                            string[] arrClinicPhone = billingAddress.Phone.Split(')');
                            string areaCode = Regex.Match(arrClinicPhone[0], @"\d+").Value;
                            string restPhoneNo = arrClinicPhone.Length < 2 ? "" : arrClinicPhone[1];
                            gfx.DrawString(areaCode, font, XBrushes.Black, 533 - x, 733 - y, XStringFormats.TopLeft);
                            gfx.DrawString(restPhoneNo, font, XBrushes.Black, 558 - x, 733 - y, XStringFormats.TopLeft);

                            //gfx.DrawString(claim.SubmitterLastName.ToUpper() + " " +claim.SubmitterFirstName.ToUpper(), lineItemFont, XBrushes.Black, 420 - x, 743 - y, XStringFormats.TopLeft);
                            gfx.DrawString(billingAddress.BillingProviderInfomation.ToUpper(), lineItemFont, XBrushes.Black, 420 - x, 743 - y, XStringFormats.TopLeft);
                            if (billingAddress.Address1 != null)
                            {
                                string street = string.Empty;
                                street = billingAddress.Address1.ToUpper();

                                if (!String.IsNullOrEmpty(billingAddress.Address2))
                                {
                                    street = street + " " + billingAddress.Address2.ToUpper();
                                }
                                street += (!string.IsNullOrEmpty(billingAddress.ApartmentNumber) ? " " + billingAddress.ApartmentNumber : "");

                                gfx.DrawString(street, lineItemFont, XBrushes.Black, 420 - x, 751 - y, XStringFormats.TopLeft);//752
                                gfx.DrawString(String.Format("{0} {1} {2}", billingAddress.City, billingAddress.State, billingAddress.Zip).ToUpper(), lineItemFont, XBrushes.Black, 420 - x, 766 - y, XStringFormats.TopLeft);//764
                            }
                            //if (Claim.IsTaxonomyNeeded && !string.IsNullOrEmpty(Claim.TaxonomyCode))
                            //{
                            //    gfx.DrawString(Claim.TaxonomyCode, lineItemFont, XBrushes.Black, 420 - x, 758 - y, XStringFormats.TopLeft);//752

                            //}
                        }
                        // }
                        #endregion
                        #region Signature and billing provider(sec 31 and 33a,b)
                        XTextFormatter tf = new XTextFormatter(gfx);
                        XRect rect = new XRect(63 - x, 759 - y, 106, 30);
                        gfx.DrawRectangle(XBrushes.Transparent, rect);
                        //signature sec 31


                        tf.DrawString(claim.SubmitterLastName.ToUpper() + " " + claim.SubmitterFirstName.ToUpper(), lineItemFont, XBrushes.Black, rect, XStringFormats.TopLeft);
                        //gfx.DrawString(tempBilling.InvoiceDate != DateTime.MinValue ? String.Format("{0:MM/dd/yyyy}", tempBilling.InvoiceDate) : "", lineItemFont, XBrushes.Black, 168, 759, XStringFormats.TopLeft);
                        gfx.DrawString(DateTime.Today != DateTime.MinValue ? String.Format("{0:MM/dd/yyyy}", DateTime.Today) : "", lineItemFont, XBrushes.Black, 168 - x, 759 - x, XStringFormats.TopLeft);

                        //signature sec 33a and 33b
                        gfx.DrawString(serviceAddress.NPINumber, lineItemFont, XBrushes.Black, 430 - x, 781 - y, XStringFormats.TopLeft);
                        gfx.DrawString("", lineItemFont, XBrushes.Black, 510 - x, 781 - y, XStringFormats.TopLeft);



                        //32 a, b              
                        gfx.DrawString(billingAddress.NPINumber, lineItemFont, XBrushes.Black, 230 - x, 781 - y, XStringFormats.TopLeft);
                        gfx.DrawString("", lineItemFont, XBrushes.Black, 310 - x, 781 - y, XStringFormats.TopLeft);

                        #endregion

                        if (TotalCharges != 0)
                        {

                            gfx.DrawString(String.Format("{0:0}", Math.Floor(TotalCharges)), font, XBrushes.Black, 425 - x, 718 - y, XStringFormats.TopLeft);

                            int tempChargeDecimal = (int)((TotalCharges - (int)TotalCharges) * 100);

                            gfx.DrawString(String.Format("{0:00}", tempChargeDecimal), font, XBrushes.Black, 475 - x, 718 - y, XStringFormats.TopLeft);
                        }
                        if (AmountPaid != -1)
                        {
                            if (payerPayment != null && payerPayment.Count > 0)
                            {
                                gfx.DrawString(String.Format("{0:00}", Math.Floor(payerPayment.Sum(z => z.Amount))), font, XBrushes.Black, 510 - x, 718 - y, XStringFormats.TopLeft);
                                int amountPaid = (int)((payerPayment.Sum(z => z.Amount) - (int)payerPayment.Sum(z => z.Amount)) * 100);
                                gfx.DrawString(String.Format("{0:00}", amountPaid), font, XBrushes.Black, 545 - x, 718 - y, XStringFormats.TopLeft);
                            }
                            else
                                gfx.DrawString(String.Format("{0:00}", 0), font, XBrushes.Black, 510 - x, 718 - y, XStringFormats.TopLeft);
                        }
                    }
                }
                tempStream = new MemoryStream();
                //If you want to 
                PDFNewDoc.Save(tempStream, false);
            }
            return tempStream;
        }

        public MemoryStream GenerateBatchPaperClaim_Clubbed(PaperClaimModel paperClaim, int printFormat)

        {   //pdf
            //int x = 45;
            //int y = 35;
            //int x = 49;
            //int y = 35;
            int x = 0;
            int y = 0;
            MemoryStream tempStream = null;
            var fullPath = Directory.GetCurrentDirectory();
            PdfDocument PDFDoc = null;
            if (printFormat == 1)
            {
                PDFDoc = PdfReader.Open(fullPath + "\\SEFFiles\\HCFA_1500.pdf", PdfDocumentOpenMode.Import);
            }
            else
            {
                PDFDoc = PdfReader.Open(fullPath + "\\SEFFiles\\PrePrintedHCFA_1500.pdf", PdfDocumentOpenMode.Import);
                x = 45;
                y = 35;
            }
            PdfDocument PDFNewDoc = new PdfSharp.Pdf.PdfDocument();
            //return null;
            // int addpage = 0;
            ClaimDetailsModel claim = null;
            InsuredModel insuredDetails = null;
            InsuredModel otherInsuredDetails = null;
            List<ServiceCodesModel> serviceCodeList = null;
            List<DiagnosisCodesModel> diagnosisCodeList = null;
            OrganizationDetailsModel organizationDetailsModel = null;
            LocationAddressModel serviceAddress = null;
            LocationAddressModel billingAddress = null;
            List<PayerPaymentModel> payerPayment = null;
            int pages = 0;
            foreach (ClaimDetailsModel responseClaimDetais in paperClaim.ClaimDetails)
            {
                List<int> claimIds = new List<int>();
                claimIds = responseClaimDetais.ClaimIds.Split(',').Select(Int32.Parse).ToList();
                claim = responseClaimDetais;
                insuredDetails = paperClaim.InsuredDetails.Find(p => p.ClaimIds==claim.ClaimIds);
                otherInsuredDetails = paperClaim.OtherInsuredDetails.Find(p => p.ClaimIds == claim.ClaimIds);
                serviceCodeList = paperClaim.ServiceCodes.FindAll(p => claimIds.Contains(p.ClaimId));
                diagnosisCodeList = paperClaim.DiagnosisCodes.FindAll(p => claimIds.Contains(p.ClaimId));
                organizationDetailsModel = paperClaim.OrganizationDetails;
                serviceAddress = paperClaim.ServiceLocation;
                billingAddress = paperClaim.BillingLocation;
                payerPayment = (paperClaim.PayerPayments != null && paperClaim.PayerPayments.Count > 0) ? paperClaim.PayerPayments.FindAll(p => claimIds.Contains(p.ClaimId)).ToList() : null;
                int noOfPages = (int)Math.Ceiling((decimal)serviceCodeList.Count / (decimal)6);
                int count = 0;
                for (int Pg = 0; Pg < noOfPages; Pg++)
                {
                    if (count > 0)
                        serviceCodeList.RemoveRange(0, 6);
                    count = count + 1;

                    //If you want to create multiple forms with more than 6 cptcodes
                    PDFNewDoc.AddPage(PDFDoc.Pages[0]);
                    PdfPage page = PDFNewDoc.Pages[pages];
                    pages = pages + 1;
                    //PdfPage page = PDFDoc.Pages[0];
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont font = new XFont("Courier New", 9, XFontStyle.Regular);
                    XFont fontForCross = new XFont("Courier New", 11, XFontStyle.Regular);
                    //details to be inserted only one time
                    if (claim != null)
                    {

                        #region Insurance Address
                        gfx.DrawString(String.Format("{0}", claim.PayerName).ToUpper(), font, XBrushes.Black, 380, 50, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0}", claim.PayerAddress + (!string.IsNullOrEmpty(claim.PayerApartmentNumber) ? " " + claim.PayerApartmentNumber : "")).ToUpper(), font, XBrushes.Black, 380, 60, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0}, {1} {2}", claim.PayerCity, claim.PayerState, claim.PayerPostalCode).ToUpper(), font, XBrushes.Black, 380, 70, XStringFormats.TopLeft);
                        //1.a
                        gfx.DrawString(Convert.ToString(claim.InsuredIDNumber == null ? "" : claim.InsuredIDNumber), font, XBrushes.Black, 415 - x, 144 - y, XStringFormats.TopLeft);//
                        #endregion
                        //first row
                        switch (claim.InsurancePlanType)
                        {
                            case "MEDICARE":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 63 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "MEDICAID":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 112 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "TRICARE":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 165 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "CHAMPVA":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 226 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "GROUP HEALTH PLAN":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 278 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            case "FECA":
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 335 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                            default:
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 378 - x, 144 - y, XStringFormats.TopLeft);

                                break;
                        }
                        ////Second Row
                        //patient information
                        gfx.DrawString(string.Format("{0}, {1} {2}", claim.SubsLastName, claim.SubsFirstName, claim.SubsMiddleName).ToUpper(), font, XBrushes.Black, 63 - x, 166 - y, XStringFormats.TopLeft);
                        //third row
                        gfx.DrawString(String.Format("{0}", claim.SubsAddress1 + (!string.IsNullOrEmpty(claim.SubsApartmentNumber) ? " " + claim.SubsApartmentNumber : "")).ToUpper(), font, XBrushes.Black, 63 - x, 190 - y, XStringFormats.TopLeft);

                        gfx.DrawString(String.Format("{0}", claim.SubsCity).ToUpper(), font, XBrushes.Black, 63 - x, 215 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0}", claim.SubsState).ToUpper(), font, XBrushes.Black, 245 - x, 215 - y, XStringFormats.TopLeft);

                        gfx.DrawString(String.Format("{0:00}", claim.SubsDOB.Month), font, XBrushes.Black, 279 - x, 169 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0:00}", claim.SubsDOB.Day), font, XBrushes.Black, 303 - x, 169 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0:00}", claim.SubsDOB.Year.ToString().Substring(2, 2)), font, XBrushes.Black, 329 - x, 169 - y, XStringFormats.TopLeft);

                        if (claim.SubsGender.ToUpper() == "MALE")
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 357.5 - x, 168 - y, XStringFormats.TopLeft);
                        else if (claim.SubsGender.ToUpper() == "FEMALE")
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 393.5 - x, 168 - y, XStringFormats.TopLeft);

                        //insured information
                        if (insuredDetails != null)
                        {

                            ////Seventh Row
                            //chkBoxTop += 24.3;              
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredCity).ToUpper(), font, XBrushes.Black, 415 - x, 215 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredState).ToUpper(), font, XBrushes.Black, 585 - x, 215 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}, {1} {2}", insuredDetails.InsuredLastName, insuredDetails.InsuredFirstName, insuredDetails.InsuredMiddleName).ToUpper(), font, XBrushes.Black, 420 - x, 168 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredZip).ToUpper(), font, XBrushes.Black, 415 - x, 240 - y, XStringFormats.TopLeft);
                            if (insuredDetails.InsuredPhone != null)
                            {
                                string strInsuredAreaCode = string.Empty;
                                string strInsuredRestPhoneNo = string.Empty;

                                if (!insuredDetails.InsuredPhone.Contains(")"))
                                {
                                    string[] arrInsuredPhoneNo = insuredDetails.InsuredPhone.Split('-');
                                    if (arrInsuredPhoneNo.Length > 0)
                                    {
                                        if (arrInsuredPhoneNo.Length == 3)
                                        {
                                            strInsuredAreaCode = arrInsuredPhoneNo[0];
                                            strInsuredRestPhoneNo = arrInsuredPhoneNo[1] + "-" + arrInsuredPhoneNo[2];
                                        }
                                        else
                                        {
                                            strInsuredRestPhoneNo = insuredDetails.InsuredPhone;
                                        }
                                    }
                                }
                                else
                                {
                                    string[] arrInsuredPhoneNo = insuredDetails.InsuredPhone.Split(')');
                                    strInsuredAreaCode = Regex.Match(arrInsuredPhoneNo[0], @"\d+").Value;
                                    strInsuredRestPhoneNo = string.Empty;
                                    strInsuredRestPhoneNo = arrInsuredPhoneNo[1];
                                }
                                gfx.DrawString(strInsuredAreaCode, font, XBrushes.Black, 525 - x, 240 - y, XStringFormats.TopLeft);
                                gfx.DrawString(strInsuredRestPhoneNo, font, XBrushes.Black, 555 - x, 240 - y, XStringFormats.TopLeft);
                            }
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsuredAddress1 + (!string.IsNullOrEmpty(insuredDetails.InsuredApartmentNumber) ? " " + insuredDetails.InsuredApartmentNumber : "")).ToUpper(), font, XBrushes.Black, 418 - x, 190 - y, XStringFormats.TopLeft);

                            //if (insuredDetails.InsurancePersonSameAsPatient == false)
                            //{
                            if (insuredDetails.InsuredDOB != DateTime.MinValue)
                            {
                                gfx.DrawString(String.Format("{0:00}", insuredDetails.InsuredDOB.Month), font, XBrushes.Black, 439 - x, 289 - y, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0:00}", insuredDetails.InsuredDOB.Day), font, XBrushes.Black, 461 - x, 289 - y, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0:00}", insuredDetails.InsuredDOB.Year.ToString().Substring(2, 2)), font, XBrushes.Black, 488 - x, 289 - y, XStringFormats.TopLeft);
                            }
                            if (insuredDetails.InsuredGender.ToUpper() == "MALE")
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 544 - x, 288 - y, XStringFormats.TopLeft);
                            else if (insuredDetails.InsuredGender.ToUpper() == "FEMALE")
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 595 - x, 288 - y, XStringFormats.TopLeft);
                            //}

                            //relationship
                            if (insuredDetails.InsuredRelation.Contains("SELF"))
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 292 - x, 192 - y, XStringFormats.TopLeft);

                            }
                            else if (insuredDetails.InsuredRelation.Contains("SPOUSE"))
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 328.5 - x, 192 - y, XStringFormats.TopLeft);

                            }
                            else if (insuredDetails.InsuredRelation.Contains("CHILD"))
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 357.5 - x, 192 - y, XStringFormats.TopLeft);
                            }
                            else
                            {
                                gfx.DrawString("X", fontForCross, XBrushes.Black, 393.5 - x, 192 - y, XStringFormats.TopLeft);
                            }

                            //eleventh
                            gfx.DrawString(insuredDetails.InsuranceGroupName == null ? "" : insuredDetails.InsuranceGroupName, font, XBrushes.Black, 415 - x, 263 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", insuredDetails.InsurancePlanName).ToUpper(), font, XBrushes.Black, 415 - x, 335 - y, XStringFormats.TopLeft);
                            gfx.DrawString(insuredDetails.AuthorizationNumber == null ? string.Empty : insuredDetails.AuthorizationNumber, font, XBrushes.Black, 420 - x, 525 - y, XStringFormats.TopLeft);
                        }

                        if (otherInsuredDetails != null)
                        {

                            gfx.DrawString(String.Format("{0}, {1} {2}", otherInsuredDetails.InsuredLastName, otherInsuredDetails.InsuredFirstName, otherInsuredDetails.InsuredMiddleName).ToUpper(), font, XBrushes.Black, 63 - x, 263 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", otherInsuredDetails.InsuranceGroupName).ToUpper(), font, XBrushes.Black, 63 - x, 285 - y, XStringFormats.TopLeft);
                            gfx.DrawString(String.Format("{0}", otherInsuredDetails.InsurancePlanName).ToUpper(), font, XBrushes.Black, 63 - x, 355 - y, XStringFormats.TopLeft);
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 429 - x, 359 - y, XStringFormats.TopLeft);

                        }
                        else
                        {
                            gfx.DrawString("X", fontForCross, XBrushes.Black, 465 - x, 359 - y, XStringFormats.TopLeft);
                        }

                        //insured information
                        //third row

                        gfx.DrawString(String.Format("{0}", claim.SubsEmail).ToUpper(), font, XBrushes.Black, 63 - x, 190 - y, XStringFormats.TopLeft);

                        //fifth row
                        //chkBoxTop += 24.1;
                        gfx.DrawString(String.Format("{0}", claim.SubsPostalCode).ToUpper(), font, XBrushes.Black, 63 - x, 240 - y, XStringFormats.TopLeft);
                        string strPhone = !String.IsNullOrEmpty(claim.SubsPhoneHome) ? claim.SubsPhoneHome.Trim() : (!string.IsNullOrEmpty(claim.SubsPhoneMobile) ? claim.SubsPhoneMobile.Trim() : string.Empty);
                        string strRestPhoneNo = string.Empty;
                        string strAreaCode = string.Empty;
                        if (strPhone.Contains("("))
                        {
                            string[] arrPhoneNo = strPhone.Split(')');
                            if (arrPhoneNo[0] != null)
                                strAreaCode = Regex.Match(arrPhoneNo[0], @"\d+").Value;
                            if (arrPhoneNo[1] != null)
                            {
                                strRestPhoneNo = arrPhoneNo[1];
                            }
                        }
                        else
                        {

                            string[] arrPhoneNo = strPhone.Split('-');

                            if (arrPhoneNo.Length > 0)
                            {
                                if (arrPhoneNo.Length == 3)
                                {
                                    strAreaCode = arrPhoneNo[0];
                                    strRestPhoneNo = arrPhoneNo[1] + "-" + arrPhoneNo[2];
                                }
                                else
                                {
                                    strRestPhoneNo = strPhone;
                                }
                            }
                        }
                        gfx.DrawString(strAreaCode, font, XBrushes.Black, 167 - x, 240 - y, XStringFormats.TopLeft);
                        gfx.DrawString(strRestPhoneNo, font, XBrushes.Black, 197 - x, 240 - y, XStringFormats.TopLeft);

                        #region 10 Row Old
                        ////Tenth row
                        //chkBoxTop += 23.9;

                        //if (Claim.IsAccident)
                        //{
                        //    switch (Claim.AccidentReasonId)
                        //    {
                        //        case 1:
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 307 - x, 287 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 312 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 335 - y, XStringFormats.TopLeft);

                        //            break;
                        //        case 2:
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 307 - x, 312 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 287 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - y, 335 - x, XStringFormats.TopLeft);
                        //            gfx.DrawString(Claim.State, fontForCross, XBrushes.Black, 380 - x, 312 - y, XStringFormats.TopLeft);

                        //            break;
                        //        default:
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 307 - x, 335 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 287 - y, XStringFormats.TopLeft);
                        //            gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 312 - y, XStringFormats.TopLeft);
                        //            break;

                        //    }
                        //}
                        //else
                        //{
                        #endregion

                        gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 312 - y, XStringFormats.TopLeft);
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 287 - y, XStringFormats.TopLeft);
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 350 - x, 335 - y, XStringFormats.TopLeft);

                        //}
                        //Twelveth row
                        //chkBoxTop += 45;
                        gfx.DrawString("SIGNATURE ON FILE", font, XBrushes.Black, 120 - x, 406 - y, XStringFormats.TopLeft);
                        gfx.DrawString(String.Format("{0:MM/dd/yyyy}", claim.ClientRecordInsertionDate), font, XBrushes.Black, 320 - x, 406 - y, XStringFormats.TopLeft);
                        gfx.DrawString("SIGNATURE ON FILE", font, XBrushes.Black, 460 - x, 406 - y, XStringFormats.TopLeft);


                        ////Fourteenth row

                        //chkBoxTop += 23;
                        //if (claim.DOS != null)
                        //{
                        //    gfx.DrawString(String.Format("{0:00}", claim.DOS.Month), font, XBrushes.Black, 70 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:00}", claim.DOS.Day), font, XBrushes.Black, 90 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:0000}", claim.DOS.Year), font, XBrushes.Black, 110 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString("431", font, XBrushes.Black, 180 - x, 434 - y, XStringFormats.TopLeft);
                        //}

                        ////Fourteenth row (box 15 

                        //if (Claim.XrayDate != null)
                        //{
                        //    gfx.DrawString("454", fontForCross, XBrushes.Black, 276 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:00}", Claim.XrayDate.Value.Month), font, XBrushes.Black, 324 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:00}", Claim.XrayDate.Value.Day), font, XBrushes.Black, 344 - x, 434 - y, XStringFormats.TopLeft);
                        //    gfx.DrawString(String.Format("{0:0000}", Claim.XrayDate.Value.Year), font, XBrushes.Black, 364 - x, 434 - y, XStringFormats.TopLeft);

                        //}

                        //Seventeenth row


                        //if (Claim.PayerInsType == "Medicare" && Claim.Xray)
                        //{
                        //gfx.DrawString(Convert.ToString(Claim.QualifierType), font, XBrushes.Black, 68 - x, 455 - y, XStringFormats.TopLeft);
                        //gfx.DrawString(Convert.ToString(Claim.ReferringProviderName), font, XBrushes.Black, 87 - x, 455 - y, XStringFormats.TopLeft);
                        //gfx.DrawString(Convert.ToString(Claim.RefNPINumber), font, XBrushes.Black, 300 - x, 457 - y, XStringFormats.TopLeft);

                        // }



                        ////Nineteenth row (box 19 -It will be included when the Include standard text bit from payer screen under practicemanagemnent
                        //is true and the value will be AdditionalClaimInfo from payer table)

                        //if (!string.IsNullOrEmpty(Claim.AdditionalClaimInfo))
                        gfx.DrawString(claim.AdditionalClaimInfo, font, XBrushes.Black, 63 - x, 478 - y, XStringFormats.TopLeft);

                        //Box 21
                        gfx.DrawString("0", font, XBrushes.Black, 358 - x, 495 - y, XStringFormats.TopLeft);
                        //chkBoxTop += 24;
                        for (int i = 0; i < diagnosisCodeList.Count; i++)
                        {
                            int marginleft = 79;
                            int margintop = 505;
                            switch (i)
                            {

                                case 1:
                                    marginleft = 170;
                                    break;
                                case 2:
                                    marginleft = 265;
                                    break;
                                case 3:
                                    marginleft = 358;
                                    break;
                                case 4:
                                    margintop = 518;
                                    break;
                                case 5:
                                    margintop = 518;
                                    marginleft = 170;
                                    break;
                                case 6:
                                    margintop = 518;
                                    marginleft = 265;
                                    break;
                                case 7:
                                    margintop = 518;
                                    marginleft = 358;
                                    break;
                                case 8:
                                    margintop = 530;
                                    break;
                                case 9:
                                    margintop = 530;
                                    marginleft = 170;
                                    break;
                                case 10:
                                    margintop = 530;
                                    marginleft = 265;
                                    break;
                                case 11:
                                    margintop = 530;
                                    marginleft = 358;
                                    break;

                            }

                            gfx.DrawString(diagnosisCodeList[i].DiagnosisCode.ToUpper(), font, XBrushes.Black, marginleft - x, margintop - y, XStringFormats.TopLeft);// diagnosis 1 a
                        }

                        gfx.DrawString((serviceCodeList != null && serviceCodeList.Count > 0) ? serviceCodeList.First().AuthorizationNumber : string.Empty, font, XBrushes.Black, 418 - x, 530 - y, XStringFormats.TopLeft);


                        //cptcodes
                        double lineItemHeight = 577;
                        decimal TotalCharges = 0;
                        decimal AmountPaid = 0;
                        XFont lineItemFont = new XFont("Courier New", 8, XFontStyle.Regular);
                        for (int j = 0; j < serviceCodeList.Count && j < 6; j++)
                        {
                            //if (j == 0)
                            //{
                            TotalCharges += serviceCodeList[j].TotalAmount;
                            AmountPaid += serviceCodeList[j].AmountPaid;//+= CptCodes[0].AmountPaid;
                                                                        //TotalCharges = CptCodes.Sum(a => a.TotalCharges); //+= CptCodes[0].TotalCharges;
                                                                        //AmountPaid = CptCodes.Sum(a => a.AmountPaid);//+= CptCodes[0].AmountPaid;

                            //}
                            if (serviceCodeList[j].DOS != null)
                            {
                                if (claim.IncludeServiceTimeWithServiceCode)
                                    gfx.DrawString(serviceCodeList[j].AppointmentStartTime.ToString("hhmm tt") + " - " + serviceCodeList[j].AppointmentEndTime.ToString("hhmm tt"), lineItemFont, XBrushes.Black, 65 - x, (lineItemHeight - 11) - y, XStringFormats.TopLeft);
                                if (DateTime.MinValue != serviceCodeList[j].DOS)
                                {
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Month), lineItemFont, XBrushes.Black, 65 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Day), lineItemFont, XBrushes.Black, 85 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:0000}", serviceCodeList[j].DOS.Year), lineItemFont, XBrushes.Black, 103 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                }

                                if (DateTime.MinValue != serviceCodeList[j].DOS)
                                {
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Month), lineItemFont, XBrushes.Black, 126 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:00}", serviceCodeList[j].DOS.Day), lineItemFont, XBrushes.Black, 147.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                    gfx.DrawString(String.Format("{0:0000}", serviceCodeList[j].DOS.Year), lineItemFont, XBrushes.Black, 166 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                }
                            }

                            gfx.DrawString(serviceCodeList[j].ServiceFacilityCode, lineItemFont, XBrushes.Black, 189.5 - x, lineItemHeight - y, XStringFormats.TopLeft);

                            //  gfx.DrawString(tempItem.EMG.ToUpper(), lineItemFont, XBrushes.Black, 211.5, lineItemHeight, XStringFormats.TopLeft);

                            if (serviceCodeList[j].ServiceCode != "")
                            {
                                //gfx.DrawString(String.Format("{0:0.##}", tempItem.CPTCode), lineItemFont, XBrushes.Black, 235.5, lineItemHeight, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0:0.##}", serviceCodeList[j].ServiceCode), lineItemFont, XBrushes.Black, 240.5 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                gfx.DrawString(String.Format("{0:0}", (int)serviceCodeList[j].TotalAmount), lineItemFont, XBrushes.Black, 420 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                int altChargeDecimal = (int)((serviceCodeList[j].TotalAmount - (int)serviceCodeList[j].TotalAmount) * 100);

                                gfx.DrawString(String.Format("{0:00}", altChargeDecimal), lineItemFont, XBrushes.Black, 460 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(String.Format("{0}", serviceCodeList[j].Quantity), lineItemFont, XBrushes.Black, 480 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                #region Modifiers
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier1) ? serviceCodeList[j].Modifier1 : string.Empty, lineItemFont, XBrushes.Black, 290.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier2) ? serviceCodeList[j].Modifier2 : string.Empty, lineItemFont, XBrushes.Black, 310.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier3) ? serviceCodeList[j].Modifier3 : string.Empty, lineItemFont, XBrushes.Black, 330.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                gfx.DrawString(!string.IsNullOrEmpty(serviceCodeList[j].Modifier4) ? serviceCodeList[j].Modifier4 : string.Empty, lineItemFont, XBrushes.Black, 350.5 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                #endregion


                                #region Pointers 24E
                                string allPointers = (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer1) ? serviceCodeList[j].DiagnosisCodePointer1 : string.Empty) + (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer2) ? serviceCodeList[j].DiagnosisCodePointer2 : string.Empty)
                                                    + (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer3) ? serviceCodeList[j].DiagnosisCodePointer3 : string.Empty) + (!string.IsNullOrEmpty(serviceCodeList[j].DiagnosisCodePointer4) ? serviceCodeList[j].DiagnosisCodePointer4 : string.Empty);

                                gfx.DrawString(allPointers, lineItemFont, XBrushes.Black, 375 - x, lineItemHeight - y, XStringFormats.TopLeft);

                                #endregion

                                //24H
                                //  gfx.DrawString(String.Format("{0}",Claim.NPINumber).ToUpper(), lineItemFont, XBrushes.Black, 505, lineItemHeight, XStringFormats.TopLeft);

                                #region 24i and 24j shaded area commented instructed by scott
                                ////24i unshaded id-qualifier TPI or SSN or EIN
                                //if (Claim.isSSN)
                                //{
                                //    gfx.DrawString(String.Format("{0}", "SY").ToUpper(), lineItemFont, XBrushes.Black, 525, lineItemHeight - 12, XStringFormats.TopLeft);
                                //}
                                //else
                                //{
                                //    gfx.DrawString(String.Format("{0}", "E1").ToUpper(), lineItemFont, XBrushes.Black, 525, lineItemHeight - 12, XStringFormats.TopLeft);


                                //}
                                //rendering provider id 24j NPI
                                //  gfx.DrawString(String.Format("{0}", Claim.TaxId).ToUpper(), lineItemFont, XBrushes.Black, 550, lineItemHeight - 12, XStringFormats.TopLeft);

                                gfx.DrawString(String.Format("{0}", serviceCodeList[j].NPINumber).ToUpper(), lineItemFont, XBrushes.Black, 550 - x, lineItemHeight - y, XStringFormats.TopLeft);
                                #endregion
                            }
                            lineItemHeight += 24.2;

                        }
                        ////25. 

                        //chkBoxTop += 141.5;
                        // IsSSn or EIN Checkboxes
                        gfx.DrawString(String.Format("{0}", billingAddress.TaxId).ToUpper(), font, XBrushes.Black, 69 - x, 720 - y, XStringFormats.TopLeft);
                        //if (Claim.isSSN)
                        //{
                        //    gfx.DrawString("X", fontForCross, XBrushes.Black, 178 - x, 718 - y, XStringFormats.TopLeft);
                        //}
                        //else
                        //{
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 193 - x, 718 - y, XStringFormats.TopLeft);
                        //}

                        //sec 26 and 27
                        gfx.DrawString(claim.SubsAccountNumber, font, XBrushes.Black, 230 - x, 718 - y, XStringFormats.TopLeft);
                        //if (Claim.AcceptAssignment)
                        gfx.DrawString("X", fontForCross, XBrushes.Black, 330 - x, 718 - y, XStringFormats.TopLeft);
                        // else
                        //   gfx.DrawString("X", fontForCross, XBrushes.Black, 365 - x, 718 - y, XStringFormats.TopLeft);

                        //sec 26 and 27
                        #region service location information
                        //chkBoxTop += 9;


                        if (serviceAddress != null)
                        {
                            //if (serviceAddress.Alias != null)
                            //    gfx.DrawString(serviceAddress.Alias.ToUpper(), lineItemFont, XBrushes.Black, 220, 743, XStringFormats.TopLeft);
                            //else
                            gfx.DrawString(serviceAddress.ServiceFacilityLocationName.ToUpper(), lineItemFont, XBrushes.Black, 220, 743, XStringFormats.TopLeft);

                            if (serviceAddress.Address1 != null)
                            {
                                string street = string.Empty;
                                street = serviceAddress.Address1;

                                if (!String.IsNullOrEmpty(serviceAddress.Address2))
                                {
                                    street += " " + serviceAddress.Address2;
                                }

                                street += (!string.IsNullOrEmpty(serviceAddress.ApartmentNumber) ? " " + serviceAddress.ApartmentNumber : "");

                                gfx.DrawString(street.ToUpper(), lineItemFont, XBrushes.Black, 220 - x, 752 - y, XStringFormats.TopLeft);//752
                                gfx.DrawString(String.Format("{0} {1} {2}", serviceAddress.City, serviceAddress.State, serviceAddress.Zip).ToUpper(), lineItemFont, XBrushes.Black, 220 - x, 764 - y, XStringFormats.TopLeft);//764
                            }
                        }
                        #endregion
                        #region billing address  33


                        if (billingAddress == null)
                        {
                            billingAddress = serviceAddress;
                        }
                        if (billingAddress != null)
                        {
                            string[] arrClinicPhone = billingAddress.Phone.Split(')');
                            string areaCode = Regex.Match(arrClinicPhone[0], @"\d+").Value;
                            string restPhoneNo = arrClinicPhone[1];
                            gfx.DrawString(areaCode, font, XBrushes.Black, 533 - x, 733 - y, XStringFormats.TopLeft);
                            gfx.DrawString(restPhoneNo, font, XBrushes.Black, 558 - x, 733 - y, XStringFormats.TopLeft);

                            //gfx.DrawString(claim.SubmitterLastName.ToUpper() + " " +claim.SubmitterFirstName.ToUpper(), lineItemFont, XBrushes.Black, 420 - x, 743 - y, XStringFormats.TopLeft);
                            gfx.DrawString(billingAddress.BillingProviderInfomation.ToUpper(), lineItemFont, XBrushes.Black, 420 - x, 743 - y, XStringFormats.TopLeft);
                            if (billingAddress.Address1 != null)
                            {
                                string street = string.Empty;
                                street = billingAddress.Address1.ToUpper();

                                if (!String.IsNullOrEmpty(billingAddress.Address2))
                                {
                                    street = street + " " + billingAddress.Address2.ToUpper();
                                }
                                street += (!string.IsNullOrEmpty(billingAddress.ApartmentNumber) ? " " + billingAddress.ApartmentNumber : "");

                                gfx.DrawString(street, lineItemFont, XBrushes.Black, 420 - x, 751 - y, XStringFormats.TopLeft);//752
                                gfx.DrawString(String.Format("{0} {1} {2}", billingAddress.City, billingAddress.State, billingAddress.Zip).ToUpper(), lineItemFont, XBrushes.Black, 420 - x, 766 - y, XStringFormats.TopLeft);//764
                            }
                            //if (Claim.IsTaxonomyNeeded && !string.IsNullOrEmpty(Claim.TaxonomyCode))
                            //{
                            //    gfx.DrawString(Claim.TaxonomyCode, lineItemFont, XBrushes.Black, 420 - x, 758 - y, XStringFormats.TopLeft);//752

                            //}
                        }
                        // }
                        #endregion
                        #region Signature and billing provider(sec 31 and 33a,b)
                        XTextFormatter tf = new XTextFormatter(gfx);
                        XRect rect = new XRect(63 - x, 759 - y, 106, 30);
                        gfx.DrawRectangle(XBrushes.Transparent, rect);
                        //signature sec 31


                        tf.DrawString(claim.SubmitterLastName.ToUpper() + " " + claim.SubmitterFirstName.ToUpper(), lineItemFont, XBrushes.Black, rect, XStringFormats.TopLeft);
                        //gfx.DrawString(tempBilling.InvoiceDate != DateTime.MinValue ? String.Format("{0:MM/dd/yyyy}", tempBilling.InvoiceDate) : "", lineItemFont, XBrushes.Black, 168, 759, XStringFormats.TopLeft);
                        gfx.DrawString(DateTime.Today != DateTime.MinValue ? String.Format("{0:MM/dd/yyyy}", DateTime.Today) : "", lineItemFont, XBrushes.Black, 168 - x, 759 - x, XStringFormats.TopLeft);

                        //signature sec 33a and 33b
                        gfx.DrawString(serviceAddress.NPINumber, lineItemFont, XBrushes.Black, 430 - x, 781 - y, XStringFormats.TopLeft);
                        gfx.DrawString("", lineItemFont, XBrushes.Black, 510 - x, 781 - y, XStringFormats.TopLeft);



                        //32 a, b              
                        gfx.DrawString(billingAddress.NPINumber, lineItemFont, XBrushes.Black, 230 - x, 781 - y, XStringFormats.TopLeft);
                        gfx.DrawString("", lineItemFont, XBrushes.Black, 310 - x, 781 - y, XStringFormats.TopLeft);

                        #endregion

                        if (TotalCharges != 0)
                        {

                            gfx.DrawString(String.Format("{0:0}", Math.Floor(TotalCharges)), font, XBrushes.Black, 425 - x, 718 - y, XStringFormats.TopLeft);

                            int tempChargeDecimal = (int)((TotalCharges - (int)TotalCharges) * 100);

                            gfx.DrawString(String.Format("{0:00}", tempChargeDecimal), font, XBrushes.Black, 475 - x, 718 - y, XStringFormats.TopLeft);
                        }
                        if (AmountPaid != -1)
                        {
                            if (payerPayment != null && payerPayment.Count > 0)
                            {
                                gfx.DrawString(String.Format("{0:00}", Math.Floor(payerPayment.Sum(z => z.Amount))), font, XBrushes.Black, 510 - x, 718 - y, XStringFormats.TopLeft);
                                int amountPaid = (int)((payerPayment.Sum(z => z.Amount) - (int)payerPayment.Sum(z => z.Amount)) * 100);
                                gfx.DrawString(String.Format("{0:00}", amountPaid), font, XBrushes.Black, 545 - x, 718 - y, XStringFormats.TopLeft);
                            }
                            else
                                gfx.DrawString(String.Format("{0:00}", 0), font, XBrushes.Black, 510 - x, 718 - y, XStringFormats.TopLeft);
                        }
                    }
                }
                tempStream = new MemoryStream();
                //If you want to 
                PDFNewDoc.Save(tempStream, false);
            }
            return tempStream;
        }

        public MemoryStream GenerateBatchPaperClaims_Clubbed(string claimIds, string payerPreference, bool markSubmitted, int printFormat, TokenModel token)
        {
            try
            {
                PaperClaimModel paperClaim = _claimRepository.GetBatchPaperClaimInfo_Clubbed(claimIds, payerPreference, token.OrganizationID);
                MemoryStream memoryStream = GenerateBatchPaperClaim_Clubbed(paperClaim, printFormat); //GenerateBatchPaperClaim(paperClaim, printFormat);
                if (memoryStream != null)
                {
                    inputXML = GetClaimHistoryForPaperClaim(claimIds);
                    if (!markSubmitted)
                        _claimRepository.SaveClaimHistory<SQLResponseModel>(null, inputXML, ClaimHistoryAction.PrintPaperClaim, DateTime.UtcNow, token).FirstOrDefault();
                    else
                    {
                        List<int> claimids = new List<int>();
                        claimids = claimIds.Split(',').Select(Int32.Parse).ToList();
                        var claims = _claimRepository.GetAll(a => claimids.Contains(a.Id)).AsQueryable();
                        if (claims != null)
                        {
                            claims.ToList().ForEach(x =>
                            {
                                x.ClaimStatusId = (int)CommonEnum.MasterStatusClaim.Submitted;
                                x.UpdatedBy = token.UserID;
                                x.SubmissionType = Convert.ToInt16(CommonEnum.ClaimSubmissionType.PaperClaim);
                                x.UpdatedDate = DateTime.UtcNow;
                                x.SubmittedDate = DateTime.UtcNow;
                            });
                            _claimRepository.Update(claims.ToArray());
                            _claimRepository.SaveChanges();
                        }
                        _claimRepository.SaveClaimHistory<SQLResponseModel>(null, inputXML, ClaimHistoryAction.PrintAndSubmitPaperClaim, DateTime.UtcNow, token).FirstOrDefault();
                    }
                }
                return memoryStream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
