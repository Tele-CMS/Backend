using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using System.IO;
using EDIParser.IServices;
using EDIParser.Model;
using HC.Patient.Model.Claim;
using System.Xml.Linq;
using System.Linq;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Patient.Model.CustomMessage;
using Renci.SshNet;
using System.IO.Compression;
using HC.Patient.Repositories.IRepositories.MasterData;
using System.Linq.Dynamic.Core;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using HC.Common.HC.Common;
using static HC.Common.Enums.CommonEnum;
using HC.Service;
using HC.Patient.Service.IServices.EDI;

namespace HC.Patient.Service.Services.EDI
{
    public class EDI835ParserService : BaseService, IEDI835ParserService
    {
        private IEDI835Service _edi835Service;
        private IEdiGatewayService _ediGatewayService;
        private IClaim835BatchRepository _claim835BatchRepository;
        private IEdiGatewayRepository _ediGatewayRepository;
        public EDI835ParserService(IEDI835Service edi835Service, IClaim835BatchRepository claim835BatchRepository, IEdiGatewayRepository ediGatewayRepository, IEdiGatewayService ediGatewayService)
        {
            _edi835Service = edi835Service;
            _ediGatewayService = ediGatewayService;
            _claim835BatchRepository = claim835BatchRepository;
            _ediGatewayRepository = ediGatewayRepository;
        }
        public JsonModel ReadEDI835(TokenModel token)
        {
            StringBuilder notFoundClaimIds = new StringBuilder();
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            SQLResponseModel response = null;
            string fileText = string.Empty;
            EDI835SchemaModel ediResponseModel = null;
            EDI835ResponseXMLModel ediResponseDataXMLModel = null;
            //#region uncomment for  testing purpose
            //if (clearingHouse != null)
            //{
            //    SftpClient client = _ediGatewayService.CreateConnection(clearingHouse);
            //    //if (client != null && client.ConnectionInfo.IsAuthenticated)
            //    {
            //        fileText = System.IO.File.ReadAllText(fullPath + "\\1_MNCare.txt");
            //        ediResponseModel = _edi835Service.ParseEDI835(fileText);
            //        ediResponseDataXMLModel = MAPEDI835DataToXML(ediResponseModel, notFoundClaimIds);
            //        response = _claim835BatchRepository.Save835Response<SQLResponseModel>(ediResponseDataXMLModel.EDI835Headers.ToString(), ediResponseDataXMLModel.EDI835Claims.ToString(), ediResponseDataXMLModel.EDI835ClaimServiceLines.ToString(), ediResponseDataXMLModel.EDI835ClaimServiceLineAdjustments.ToString(), fileText, token).FirstOrDefault();
            //        if (response != null && response.StatusCode == 200 && response.ResponseIds.Trim() != string.Empty)
            //        {
            //            _claim835BatchRepository.Apply835PaymentsToPatientAccount<SQLResponseModel>(response.ResponseIds, token);
            //            client.Disconnect();
            //            client.Dispose();
            //        }
            //    }
            //}
            //#endregion

            #region Real Code to be used in production
            if (clearingHouse != null)
            {
                SftpClient client = _ediGatewayService.CreateConnection(clearingHouse);
                if (client != null && client.ConnectionInfo.IsAuthenticated)
                {
                    var ftpDirectory = "/" + clearingHouse.Path835 + "/";
                    var files = client.ListDirectory(ftpDirectory);
                    foreach (var file in files)
                    {
                        if (clearingHouse.FTPURL == "ftp.officeally.com")
                        {
                            bool containsZip = file.FullName.Contains(".zip");
                            if (containsZip)
                            {
                                FileStream fs = new FileStream(fullPath + "\\" + clearingHouse.Path835 + "\\" + file.Name, FileMode.OpenOrCreate);
                                client.DownloadFile(file.FullName, fs);
                                fs.Dispose();
                                using (ZipArchive archive = ZipFile.OpenRead(fullPath + "\\" + clearingHouse.Path835 + "\\" + file.Name))
                                {
                                    foreach (ZipArchiveEntry entry in archive.Entries)
                                    {
                                        if (entry.FullName.EndsWith(".835", StringComparison.OrdinalIgnoreCase))
                                        {
                                            try
                                            {
                                                entry.ExtractToFile(Path.Combine(fullPath + "\\" + clearingHouse.Path835, entry.FullName), true);
                                                using (StreamReader streamReader = new StreamReader(fullPath + "\\" + clearingHouse.Path835 + "\\" + entry.FullName, Encoding.UTF8))
                                                {
                                                    fileText = streamReader.ReadToEnd();
                                                    ediResponseModel = _edi835Service.ParseEDI835(fileText);
                                                    ediResponseDataXMLModel = MAPEDI835DataToXML(ediResponseModel, notFoundClaimIds);
                                                    response = _claim835BatchRepository.Save835Response<SQLResponseModel>(ediResponseDataXMLModel.EDI835Headers.ToString(), ediResponseDataXMLModel.EDI835Claims.ToString(), ediResponseDataXMLModel.EDI835ClaimServiceLines.ToString(), ediResponseDataXMLModel.EDI835ClaimServiceLineAdjustments.ToString(), fileText, token).FirstOrDefault();
                                                    if (response != null && response.StatusCode == 200 && response.ResponseIds.Trim() != string.Empty)
                                                    {
                                                        _claim835BatchRepository.Apply835PaymentsToPatientAccount<SQLResponseModel>(response.ResponseIds, token);
                                                        client.Disconnect();
                                                        client.Dispose();
                                                    }
                                                }
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                    }
                                    //client.DeleteFile(file.FullName);
                                }

                            }
                        }
                        else
                        {
                            FileStream fs = new FileStream(fullPath + "\\" + clearingHouse.Path835 + "\\" + file.Name, FileMode.OpenOrCreate);
                            client.DownloadFile(file.FullName, fs);
                            fs.Close();
                            using (StreamReader streamReader = new StreamReader(fullPath + "\\" + clearingHouse.Path835 + "\\" + file.Name, Encoding.UTF8))
                            {
                                fileText = streamReader.ReadToEnd();
                                ediResponseModel = _edi835Service.ParseEDI835(fileText);
                                ediResponseDataXMLModel = MAPEDI835DataToXML(ediResponseModel, notFoundClaimIds);
                                response = _claim835BatchRepository.Save835Response<SQLResponseModel>(ediResponseDataXMLModel.EDI835Headers.ToString(), ediResponseDataXMLModel.EDI835Claims.ToString(), ediResponseDataXMLModel.EDI835ClaimServiceLines.ToString(), ediResponseDataXMLModel.EDI835ClaimServiceLineAdjustments.ToString(), fileText, token).FirstOrDefault();
                                if (response != null && response.StatusCode == 200 && response.ResponseIds.Trim() != string.Empty)
                                {
                                    _claim835BatchRepository.Apply835PaymentsToPatientAccount<SQLResponseModel>(response.ResponseIds, token);
                                    client.Disconnect();
                                    client.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            return new JsonModel()
            {
                data = new object(),
                Message = !string.IsNullOrEmpty(response.Message) ? response.Message : "" + (notFoundClaimIds.ToString() != "" ? "ClaimIds " + notFoundClaimIds + " are not there in the system" : ""),
                StatusCode = response.StatusCode
            };
        }

        private EDI835ResponseModel MapEDI835DataToJSON(EDI835SchemaModel ediResponseModel)
        {
            int value;
            try
            {
                EDI835ResponseModel ediResponseDataModel = new EDI835ResponseModel();
                List<EDI835ResponseClaims> responseClaimsList = new List<EDI835ResponseClaims>();
                List<EDI835ResponseClaimServiceLine> responseClaimServiceLineList = new List<EDI835ResponseClaimServiceLine>();
                List<EDI835ResponseClaimServiceLineAdjustments> responseClaimServiceLineAdjustmentsList = new List<EDI835ResponseClaimServiceLineAdjustments>();
                EDI835ResponseClaims responseClaims = null;
                EDI835ResponseClaimServiceLine responseClaimServiceLine = null;
                #region Map JSON
                if (ediResponseModel != null && ediResponseModel.CLPList != null && ediResponseModel.CLPList.Count > 0)
                {
                    ediResponseDataModel.TransactionHandlingCode = ediResponseModel.BPR.BPR01;
                    ediResponseDataModel.MonetoryAmount = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR02) ? Convert.ToDecimal(ediResponseModel.BPR.BPR02) : 0;
                    ediResponseDataModel.CreditDebitFlagCode = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR03) ? ediResponseModel.BPR.BPR03 : null;
                    ediResponseDataModel.PaymentMethodCode = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR04) ? ediResponseModel.BPR.BPR04 : null;
                    ediResponseDataModel.PaymentFormatCode = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR05) ? ediResponseModel.BPR.BPR05 : null;
                    ediResponseDataModel.SenderDFIType = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR06) ? ediResponseModel.BPR.BPR06 : null;
                    ediResponseDataModel.SenderDFINumber = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR07) ? ediResponseModel.BPR.BPR07 : null;
                    ediResponseDataModel.SenderAccountNumberQualifier = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR08) ? ediResponseModel.BPR.BPR08 : null;
                    ediResponseDataModel.SenderAccountNumber = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR09) ? ediResponseModel.BPR.BPR09 : null;
                    ediResponseDataModel.ReceiverDFIType = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR12) ? ediResponseModel.BPR.BPR12 : null;
                    ediResponseDataModel.ReceiverDFINumber = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR13) ? ediResponseModel.BPR.BPR13 : null;
                    ediResponseDataModel.ReceiverAccountNumberQualifier = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR14) ? ediResponseModel.BPR.BPR14 : null;
                    ediResponseDataModel.ReceiverAccountNumber = !string.IsNullOrEmpty(ediResponseModel.BPR.BPR15) ? ediResponseModel.BPR.BPR15 : null;
                    ediResponseDataModel.EDIReferenceNumber = !string.IsNullOrEmpty(ediResponseModel.TRN.TRN02) ? ediResponseModel.TRN.TRN02 : null;
                    ediResponseDataModel.ProductionDate = DateTime.Now;//Use the below date by converting the string to datetime
                    //ediResponseDataModel.ProductionDate = !string.IsNullOrEmpty(ediResponseModel.DTM.DTM02) ? Convert.ToDateTime(ediResponseModel.DTM.DTM02) : new DateTime();
                    ediResponseModel.CLPList.ForEach(x =>
                    {
                        responseClaims = new EDI835ResponseClaims();
                        if (int.TryParse(x.CLP01, out value))
                        {
                            responseClaims.Claim837ClaimId = !string.IsNullOrEmpty(x.CLP01) ? Convert.ToInt32(x.CLP01) : 0;
                            responseClaims.ClaimStatusCode = !string.IsNullOrEmpty(x.CLP02) ? x.CLP02 : null;
                            responseClaims.AmountClaimed = !string.IsNullOrEmpty(x.CLP03) ? Convert.ToDecimal(x.CLP03) : 0;
                            responseClaims.AmountApproved = !string.IsNullOrEmpty(x.CLP04) ? Convert.ToDecimal(x.CLP04) : 0;
                            responseClaims.PatientResponsibilityAmount = !string.IsNullOrEmpty(x.CLP05) ? Convert.ToDecimal(x.CLP05) : 0;
                            responseClaims.ClaimFillingIndicatorCode = !string.IsNullOrEmpty(x.CLP06) ? x.CLP06 : null;
                            responseClaims.PayerClaimControlNumber = !string.IsNullOrEmpty(x.CLP07) ? x.CLP07 : null;
                            responseClaimsList.Add(responseClaims);
                            x.SVCList.ForEach(y =>
                            {
                                responseClaimServiceLine = new EDI835ResponseClaimServiceLine();
                                responseClaimServiceLine.Claim837ClaimId = responseClaims.Claim837ClaimId;
                                responseClaimServiceLine.Claim837ServiceLineId = !string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0;
                                responseClaimServiceLine.ServiceCode = !string.IsNullOrEmpty(y.SVC01) ? y.SVC01 : null;
                                responseClaimServiceLine.AmountCharged = !string.IsNullOrEmpty(y.SVC02) ? Convert.ToDecimal(y.SVC02) : 0;
                                responseClaimServiceLine.AmountApproved = !string.IsNullOrEmpty(y.SVC03) ? Convert.ToDecimal(y.SVC03) : 0;
                                responseClaimServiceLine.UnitApproved = !string.IsNullOrEmpty(y.SVC05) ? Convert.ToDecimal(y.SVC05) : 0;
                                responseClaimServiceLine.UnitCharged = !string.IsNullOrEmpty(y.SVC07) ? Convert.ToDecimal(y.SVC07) : 0;
                                responseClaimServiceLineList.Add(responseClaimServiceLine);
                                if (y.SVCAdjList != null && y.SVCAdjList.Count > 0)
                                {
                                    y.SVCAdjList.ForEach(z =>
                                    {
                                        GetServiceLineAdjustments(responseClaimServiceLine.Claim837ServiceLineId, z.CAS01, z.CAS02, z.CAS03, z.CAS04, new EDI835ResponseClaimServiceLineAdjustments(), responseClaimServiceLineAdjustmentsList);
                                        if (!string.IsNullOrEmpty(z.CAS05) || !string.IsNullOrEmpty(z.CAS06) || !string.IsNullOrEmpty(z.CAS07))
                                        {
                                            GetServiceLineAdjustments(responseClaimServiceLine.Claim837ServiceLineId, z.CAS01, z.CAS05, z.CAS06, z.CAS07, new EDI835ResponseClaimServiceLineAdjustments(), responseClaimServiceLineAdjustmentsList);
                                            if (!string.IsNullOrEmpty(z.CAS08) || !string.IsNullOrEmpty(z.CAS09) || !string.IsNullOrEmpty(z.CAS10))
                                            {
                                                GetServiceLineAdjustments(responseClaimServiceLine.Claim837ServiceLineId, z.CAS01, z.CAS08, z.CAS09, z.CAS10, new EDI835ResponseClaimServiceLineAdjustments(), responseClaimServiceLineAdjustmentsList);
                                                if (!string.IsNullOrEmpty(z.CAS11) || !string.IsNullOrEmpty(z.CAS12) || !string.IsNullOrEmpty(z.CAS13))
                                                {
                                                    GetServiceLineAdjustments(responseClaimServiceLine.Claim837ServiceLineId, z.CAS01, z.CAS11, z.CAS12, z.CAS13, new EDI835ResponseClaimServiceLineAdjustments(), responseClaimServiceLineAdjustmentsList);
                                                    if (!string.IsNullOrEmpty(z.CAS14) || !string.IsNullOrEmpty(z.CAS15) || !string.IsNullOrEmpty(z.CAS16))
                                                    {
                                                        GetServiceLineAdjustments(responseClaimServiceLine.Claim837ServiceLineId, z.CAS01, z.CAS14, z.CAS15, z.CAS16, new EDI835ResponseClaimServiceLineAdjustments(), responseClaimServiceLineAdjustmentsList);
                                                        if (!string.IsNullOrEmpty(z.CAS17) || !string.IsNullOrEmpty(z.CAS18) || !string.IsNullOrEmpty(z.CAS19))
                                                        {
                                                            GetServiceLineAdjustments(responseClaimServiceLine.Claim837ServiceLineId, z.CAS01, z.CAS17, z.CAS18, z.CAS19, new EDI835ResponseClaimServiceLineAdjustments(), responseClaimServiceLineAdjustmentsList);
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    });
                                }

                            });
                        }
                    });
                }
                ediResponseDataModel.EDI835ClaimsList = responseClaimsList;
                ediResponseDataModel.EDI835ResponseClaimServiceLineList = responseClaimServiceLineList;
                ediResponseDataModel.EDI835ResponseClaimServiceLineAdjustmentsList = responseClaimServiceLineAdjustmentsList;
                return ediResponseDataModel;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
            #endregion
        }


        private EDI835ResponseXMLModel MAPEDI835DataToXML(EDI835SchemaModel ediResponseModel, StringBuilder notFoundClaimIds)
        {
            int value;
            EDI835ResponseXMLModel ediResponseDataModel = new EDI835ResponseXMLModel(); ;
            XElement headerElement = null, claimElement = new XElement("Parent"), serviceLineElement = new XElement("Parent"), serviceLineAdjustment = new XElement("Parent");
            #region Map XML
            if (ediResponseModel != null && ediResponseModel.CLPList != null && ediResponseModel.CLPList.Count > 0)
            {
                headerElement = new XElement("Parent");
                headerElement.Add(new XElement("Child",
                    new XElement("TransactionHandlingCode", ediResponseModel.BPR.BPR01),
                    new XElement("MonetoryAmount", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR02) ? Convert.ToDecimal(ediResponseModel.BPR.BPR02) : 0),
                    new XElement("CreditDebitFlagCode", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR03) ? ediResponseModel.BPR.BPR03 : null),
                    new XElement("PaymentMethodCode", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR04) ? ediResponseModel.BPR.BPR04 : null),
                    new XElement("PaymentFormatCode", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR05) ? ediResponseModel.BPR.BPR05 : null),
                    new XElement("SenderDFIType", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR06) ? ediResponseModel.BPR.BPR06 : null),
                    new XElement("SenderDFINumber", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR07) ? ediResponseModel.BPR.BPR07 : null),
                    new XElement("SenderAccountNumberQualifier", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR08) ? ediResponseModel.BPR.BPR08 : null),
                    new XElement("SenderAccountNumber", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR09) ? ediResponseModel.BPR.BPR09 : null),
                    new XElement("ReceiverDFIType", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR12) ? ediResponseModel.BPR.BPR12 : null),
                    new XElement("ReceiverDFINumber", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR13) ? ediResponseModel.BPR.BPR13 : null),
                    new XElement("ReceiverAccountNumberQualifier", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR14) ? ediResponseModel.BPR.BPR14 : null),
                    new XElement("ReceiverAccountNumber", !string.IsNullOrEmpty(ediResponseModel.BPR.BPR15) ? ediResponseModel.BPR.BPR15 : null),
                    new XElement("EDIReferenceNumber", !string.IsNullOrEmpty(ediResponseModel.TRN.TRN02) ? ediResponseModel.TRN.TRN02 : null),
                    new XElement("ProductionDate", DateTime.Now)
                    //ediResponseDataModel.ProductionDate = !string.IsNullOrEmpty(ediResponseModel.DTM.DTM02) ? Convert.ToDateTime(ediResponseModel.DTM.DTM02) : new DateTime();
                    ));
                ediResponseModel.CLPList.ForEach(x =>
                {
                    if (int.TryParse(x.CLP01, out value))
                    {
                        claimElement.Add(new XElement("Child",
                        new XElement("Claim837ClaimId", !string.IsNullOrEmpty(x.CLP01) ? Convert.ToInt32(x.CLP01) : 0),
                        new XElement("ClaimStatusCode", !string.IsNullOrEmpty(x.CLP02) ? x.CLP02 : null),
                        new XElement("AmountClaimed", !string.IsNullOrEmpty(x.CLP03) ? Convert.ToDecimal(x.CLP03) : 0),
                        new XElement("AmountApproved", !string.IsNullOrEmpty(x.CLP04) ? Convert.ToDecimal(x.CLP04) : 0),
                        new XElement("PatientResponsibilityAmount", !string.IsNullOrEmpty(x.CLP05) ? Convert.ToDecimal(x.CLP05) : 0),
                        new XElement("ClaimFillingIndicatorCode", !string.IsNullOrEmpty(x.CLP06) ? x.CLP06 : null),
                        new XElement("PayerClaimControlNumber", !string.IsNullOrEmpty(x.CLP07) ? x.CLP07 : null)
                        ));
                        x.SVCList.ForEach(y =>
                        {
                            serviceLineElement.Add(new XElement("Child",
                            new XElement("Claim837ClaimId", !string.IsNullOrEmpty(x.CLP01) ? Convert.ToInt32(x.CLP01) : 0),
                            new XElement("Claim837ServiceLineId", !string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0),
                            new XElement("ServiceCode", !string.IsNullOrEmpty(y.SVC01) ? y.SVC01 : null),
                            new XElement("AmountCharged", !string.IsNullOrEmpty(y.SVC02) ? Convert.ToDecimal(y.SVC02) : 0),
                            new XElement("AmountApproved", !string.IsNullOrEmpty(y.SVC03) ? Convert.ToDecimal(y.SVC03) : 0),
                            new XElement("UnitApproved", !string.IsNullOrEmpty(y.SVC05) ? Convert.ToDecimal(y.SVC05) : 0),
                            new XElement("UnitCharged", !string.IsNullOrEmpty(y.SVC07) ? Convert.ToDecimal(y.SVC07) : 0)
                            ));
                            if (y.SVCAdjList != null && y.SVCAdjList.Count > 0)
                            {
                                y.SVCAdjList.ForEach(z =>
                                {
                                    serviceLineAdjustment.Add(GetServiceLineAdjustmentXMLElement((!string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0), z.CAS01, z.CAS02, z.CAS03, z.CAS04));
                                    if (!string.IsNullOrEmpty(z.CAS05) || !string.IsNullOrEmpty(z.CAS06) || !string.IsNullOrEmpty(z.CAS07))
                                    {
                                        serviceLineAdjustment.Add(GetServiceLineAdjustmentXMLElement((!string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0), z.CAS01, z.CAS05, z.CAS06, z.CAS07));
                                        if (!string.IsNullOrEmpty(z.CAS08) || !string.IsNullOrEmpty(z.CAS09) || !string.IsNullOrEmpty(z.CAS10))
                                        {
                                            serviceLineAdjustment.Add(GetServiceLineAdjustmentXMLElement((!string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0), z.CAS01, z.CAS08, z.CAS09, z.CAS10));
                                            if (!string.IsNullOrEmpty(z.CAS11) || !string.IsNullOrEmpty(z.CAS12) || !string.IsNullOrEmpty(z.CAS13))
                                            {
                                                serviceLineAdjustment.Add(GetServiceLineAdjustmentXMLElement((!string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0), z.CAS01, z.CAS11, z.CAS12, z.CAS13));
                                                if (!string.IsNullOrEmpty(z.CAS14) || !string.IsNullOrEmpty(z.CAS15) || !string.IsNullOrEmpty(z.CAS16))
                                                {
                                                    serviceLineAdjustment.Add(GetServiceLineAdjustmentXMLElement((!string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0), z.CAS01, z.CAS14, z.CAS15, z.CAS16));
                                                    if (!string.IsNullOrEmpty(z.CAS17) || !string.IsNullOrEmpty(z.CAS18) || !string.IsNullOrEmpty(z.CAS19))
                                                    {
                                                        serviceLineAdjustment.Add(GetServiceLineAdjustmentXMLElement((!string.IsNullOrEmpty(y.REF.REF02) ? Convert.ToInt32(y.REF.REF02) : 0), z.CAS01, z.CAS17, z.CAS18, z.CAS19));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                        });
                    }
                    else
                    {
                        notFoundClaimIds.Append(x.CLP01 + ',');
                    }
                });
            }
            ediResponseDataModel.EDI835Headers = headerElement;
            ediResponseDataModel.EDI835Claims = claimElement;
            ediResponseDataModel.EDI835ClaimServiceLines = serviceLineElement;
            ediResponseDataModel.EDI835ClaimServiceLineAdjustments = serviceLineAdjustment;
            return ediResponseDataModel;
            #endregion
        }

        private XElement GetEDI835Headers(EDI835ResponseModel ediResponseDataModel, XElement edi835HeaderElements)
        {
            if (ediResponseDataModel != null)
            {
                edi835HeaderElements = new XElement("Parent");
                edi835HeaderElements.Add(new XElement("Child"
                    , new XElement("TransactionHandlingCode", ediResponseDataModel.TransactionHandlingCode)
                    , new XElement("MonetoryAmount", ediResponseDataModel.MonetoryAmount)
                    , new XElement("CreditDebitFlagCode", ediResponseDataModel.CreditDebitFlagCode)
                    , new XElement("PaymentMethodCode", ediResponseDataModel.PaymentMethodCode)
                    , new XElement("PaymentFormatCode", ediResponseDataModel.PaymentFormatCode)
                    , new XElement("SenderDFIType", ediResponseDataModel.SenderDFIType)
                    , new XElement("SenderDFINumber", ediResponseDataModel.SenderDFINumber)
                    , new XElement("SenderAccountNumberQualifier", ediResponseDataModel.SenderAccountNumberQualifier)
                    , new XElement("SenderAccountNumber", ediResponseDataModel.SenderAccountNumber)
                    , new XElement("ReceiverDFIType", ediResponseDataModel.ReceiverDFIType)
                    , new XElement("ReceiverDFINumber", ediResponseDataModel.ReceiverDFINumber)
                    , new XElement("ReceiverAccountNumberQualifier", ediResponseDataModel.ReceiverAccountNumberQualifier)
                    , new XElement("ReceiverAccountNumber", ediResponseDataModel.ReceiverAccountNumber)
                    , new XElement("EDIReferenceNumber", ediResponseDataModel.EDIReferenceNumber)
                    , new XElement("ProductionDate", ediResponseDataModel.ProductionDate)
                    ));
            }
            return edi835HeaderElements;
        }
        private XElement GetClaimElements(EDI835ResponseModel ediResponseDataModel, XElement claimElements)
        {
            if (ediResponseDataModel.EDI835ClaimsList != null && ediResponseDataModel.EDI835ClaimsList.Count > 0)
            {
                claimElements = new XElement("Parent", ediResponseDataModel.EDI835ClaimsList.Select((c, index) => new XElement("Child",
                    new XElement("Claim837ClaimId", c.Claim837ClaimId),
                            new XElement("ClaimStatusCode", c.ClaimStatusCode),
                            new XElement("AmountCharged", c.AmountClaimed),
                            new XElement("AmountApproved", c.AmountApproved),
                            new XElement("PatientResponsibilityAmount", c.PatientResponsibilityAmount),
                             new XElement("ClaimFillingIndicatorCode", c.ClaimFillingIndicatorCode),
                              new XElement("PayerClaimControlNumber", c.PayerClaimControlNumber))));
            }
            return claimElements;
        }
        private XElement GetClaimServiceLineElements(EDI835ResponseModel ediResponseDataModel, XElement claimServiceLineElements)
        {
            if (ediResponseDataModel.EDI835ResponseClaimServiceLineList != null && ediResponseDataModel.EDI835ResponseClaimServiceLineList.Count > 0)
            {
                claimServiceLineElements = new XElement("Parent", ediResponseDataModel.EDI835ResponseClaimServiceLineList.Select((c, index) => new XElement("Child",
                    new XElement("Claim837ClaimId", c.Claim837ClaimId),
                            new XElement("Claim837ServiceLineId", c.Claim837ServiceLineId),
                            new XElement("ServiceCode", c.ServiceCode),
                            new XElement("AmountCharged", c.AmountCharged),
                            new XElement("AmountApproved", c.AmountApproved),
                            new XElement("UnitApproved", c.UnitApproved),
                             new XElement("UnitCharged", c.UnitCharged))));
            }

            return claimServiceLineElements;
        }
        private XElement GetClaimServiceLineElementsAdj(EDI835ResponseModel ediResponseDataModel, XElement claimServiceLineElementsAdj)
        {
            if (ediResponseDataModel.EDI835ResponseClaimServiceLineAdjustmentsList != null && ediResponseDataModel.EDI835ResponseClaimServiceLineAdjustmentsList.Count > 0)
            {
                claimServiceLineElementsAdj = new XElement("Parent", ediResponseDataModel.EDI835ResponseClaimServiceLineAdjustmentsList.Select((c, index) => new XElement("Child",
                    new XElement("Claim837ServiceLineId", c.Claim837ServiceLineId),
                            new XElement("AmountAdjusted", c.AmountAdjusted),
                            new XElement("AdjustmentGroupCode", c.AdjustmentGroupCode),
                            new XElement("AdjustmentReasonCode", c.AdjustmentReasonCode),
                            new XElement("UnitApproved", c.UnitApproved)
                             )));
            }
            return claimServiceLineElementsAdj;
        }

        private void GetServiceLineAdjustments(int claim837ServiceLineId, string adjGroupCode, string adjReasonCode, string amount, string quantity, EDI835ResponseClaimServiceLineAdjustments responseClaimServiceLineAdj, List<EDI835ResponseClaimServiceLineAdjustments> responseClaimServiceLineAdjList)
        {
            responseClaimServiceLineAdj.Claim837ServiceLineId = claim837ServiceLineId;
            responseClaimServiceLineAdj.AdjustmentGroupCode = !string.IsNullOrEmpty(adjGroupCode) ? adjGroupCode : string.Empty;
            responseClaimServiceLineAdj.AdjustmentReasonCode = !string.IsNullOrEmpty(adjReasonCode) ? adjReasonCode : string.Empty;
            responseClaimServiceLineAdj.AmountAdjusted = !string.IsNullOrEmpty(amount) ? Convert.ToDecimal(amount) : 0;
            responseClaimServiceLineAdj.UnitApproved = !string.IsNullOrEmpty(quantity) ? Convert.ToDecimal(quantity) : 0;
            responseClaimServiceLineAdjList.Add(responseClaimServiceLineAdj);
        }

        private XElement GetServiceLineAdjustmentXMLElement(int claim837ServiceLineId, string adjGroupCode, string adjReasonCode, string amount, string quantity)
        {
            XElement xElement = new XElement("Child",
                new XElement("Claim837ServiceLineId", claim837ServiceLineId),
                new XElement("AdjustmentGroupCode", !string.IsNullOrEmpty(adjGroupCode) ? adjGroupCode : string.Empty),
                new XElement("AdjustmentReasonCode", !string.IsNullOrEmpty(adjReasonCode) ? adjReasonCode : string.Empty),
                new XElement("AmountAdjusted", !string.IsNullOrEmpty(amount) ? Convert.ToDecimal(amount) : 0),
                new XElement("UnitApproved", !string.IsNullOrEmpty(quantity) ? Convert.ToDecimal(quantity) : 0)
                );
            return xElement;
        }

        public JsonModel Get835SegmentData(TokenModel token)
        {
            string fullPath = Directory.GetCurrentDirectory();
            string fileText = System.IO.File.ReadAllText(fullPath + "\\Agency\\517410536_ERA_835_4010_20180430.835");
            EDI835SchemaModel ediResponseModel = _edi835Service.ParseEDI835(fileText);
            EDI835ResponseModel ediResponseDataModel = MapEDI835DataToJSON(ediResponseModel);
            return new JsonModel()
            {
                data = ediResponseDataModel,
                Message = "Ok",
                StatusCode = 200
            };
        }

        public JsonModel GetProcessedClaims(int pageNumber, int pageSize, int? claimId, string patientIds, string adjustmentsGroupCodes, string fromDate, string toDate, string payerName, string sortColumn, string sortOrder, TokenModel token)
        {
            Dictionary<string, object> listProcessedClaims = _claim835BatchRepository.GetProcessedClaims(pageNumber, pageSize, claimId, patientIds, adjustmentsGroupCodes, fromDate, toDate, payerName, sortColumn, sortOrder, token);
            return new JsonModel()
            {
                data = listProcessedClaims,
                meta = new Meta()
                {
                    TotalRecords = listProcessedClaims["ProcessedClaims"] != null && ((List<ProcessedClaimModel>)listProcessedClaims["ProcessedClaims"]).Count > 0 ? ((List<ProcessedClaimModel>)listProcessedClaims["ProcessedClaims"])[0].TotalRecords : 0
                    ,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    DefaultPageSize = pageSize,
                    TotalPages = Math.Ceiling(Convert.ToDecimal((listProcessedClaims["ProcessedClaims"] != null && ((List<ProcessedClaimModel>)listProcessedClaims["ProcessedClaims"]).Count > 0 ? ((List<ProcessedClaimModel>)listProcessedClaims["ProcessedClaims"])[0].TotalRecords : 0) / pageSize))
                },
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel Apply835PaymentsToPatientAccount(string responseClaimIds, TokenModel token)
        {
            return new JsonModel()
            {
                data = _claim835BatchRepository.Apply835PaymentsToPatientAccount<SQLResponseModel>(responseClaimIds, token),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }

        public JsonModel Apply835ServiceLinePaymentsToPatientAccount(string responseClaimServiceLineIds, TokenModel token)
        {

            return new JsonModel()
            {
                data = _claim835BatchRepository.Apply835PaymentsToPatientAccount<SQLResponseModel>(responseClaimServiceLineIds, token),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }


        public JsonModel Apply835ServiceLineAdjustmentsToPatientAccount(string responseClaimServiceLineAdjIds, TokenModel token)
        {
            return new JsonModel()
            {
                data = _claim835BatchRepository.Apply835ServiceLineAdjustmentsToPatientAccount<SQLResponseModel>(responseClaimServiceLineAdjIds, token),
                Message = StatusMessage.FetchMessage,
                StatusCode = (int)HttpStatusCodes.OK
            };
        }
    }
}