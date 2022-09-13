using EDIParser.IServices;
using EDIParser.Model;
using HC.Model;
using HC.Patient.Model.CustomMessage;
using HC.Patient.Model.EDI;
using HC.Patient.Model.MasterData;
using HC.Patient.Repositories.IRepositories.EDI;
using HC.Patient.Service.IServices.EDI;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System;
using System.IO;
using System.Linq;

namespace HC.Patient.Service.Services.EDI
{
    public class EDI999ParserService : BaseService, IEDI999ParserService
    {
        private IEDI999Service _edi999Service;
        private IEdiGatewayService _ediGatewayService;
        private IEDI999Repository _edi999Repository;
        public EDI999ParserService(IEDI999Service edi999Service, IEdiGatewayService ediGatewayService, IEDI999Repository edi999Repository)
        {
            _edi999Service = edi999Service;
            _ediGatewayService = ediGatewayService;
            _edi999Repository = edi999Repository;
        }
        public JsonModel ReadEDI999(TokenModel token)
        {
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            string fileText = string.Empty;
            EDI999SchemaModel ediResponseModel = null;
            SQLResponseModel responseModel = null;
            JsonModel response = null;
            {
                fileText = System.IO.File.ReadAllText(directoryPath + "\\1_EDI999.txt");
                ediResponseModel = _edi999Service.ParseEDI999(fileText);
                responseModel = SaveEDI999Response(token, fileText, ediResponseModel);
            }
            response = new JsonModel(responseModel, responseModel.Message, responseModel.StatusCode, string.Empty);


            //#region Real Code to be used in production
            //if (clearingHouse != null)
            //{
            //    SftpClient client = _ediGatewayService.CreateConnection(clearingHouse);
            //    if (client != null && client.ConnectionInfo.IsAuthenticated)
            //    {
            //        var ftpDirectory = "/" + clearingHouse.Path999 + "/";
            //        var files = client.ListDirectory(ftpDirectory);
            //        foreach (var file in files)
            //        {
            //            if (clearingHouse.FTPURL == "ftp.officeally.com")
            //            {
            //                bool containsZip = file.FullName.Contains(".zip");
            //                if (containsZip)
            //                {
            //                    FileStream fs = new FileStream(fullPath + "\\" + clearingHouse.Path999 + "\\" + file.Name, FileMode.OpenOrCreate);
            //                    client.DownloadFile(file.FullName, fs);
            //                    fs.Dispose();
            //                    using (ZipArchive archive = ZipFile.OpenRead(fullPath + "\\" + clearingHouse.Path999 + "\\" + file.Name))
            //                    {
            //                        foreach (ZipArchiveEntry entry in archive.Entries)
            //                        {
            //                            if (entry.FullName.EndsWith(".999", StringComparison.OrdinalIgnoreCase))
            //                            {
            //                                try
            //                                {
            //                                    entry.ExtractToFile(Path.Combine(fullPath + "\\" + clearingHouse.Path999, entry.FullName), true);
            //                                    using (StreamReader streamReader = new StreamReader(fullPath + "\\" + clearingHouse.Path999 + "\\" + entry.FullName, Encoding.UTF8))
            //                                    {
            //                                        ediResponseModel = _edi999Service.ParseEDI999(fileText);
            //                                        responseModel = SaveEDI999Response(token, fileText, ediResponseModel);
            //                                        client.Disconnect();
            //                                        client.Dispose();
            //                                    }
            //                                }
            //                                catch (Exception)
            //                                {
            //                                }
            //                            }
            //                        }
            //                        //client.DeleteFile(file.FullName);
            //                    }

            //                }
            //            }
            //            else
            //            {
            //                FileStream fs = new FileStream(fullPath + "\\" + clearingHouse.Path999 + "\\" + file.Name, FileMode.OpenOrCreate);
            //                client.DownloadFile(file.FullName, fs);
            //                fs.Close();
            //                using (StreamReader streamReader = new StreamReader(fullPath + "\\" + clearingHouse.Path999 + "\\" + file.Name, Encoding.UTF8))
            //                {
            //                    ediResponseModel = _edi999Service.ParseEDI999(fileText);
            //                    responseModel = SaveEDI999Response(token, fileText, ediResponseModel);
            //                    //client.DeleteFile(file.FullName);
            //                    client.Disconnect();
            //                    client.Dispose();
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion
            return response;
        }

        private SQLResponseModel SaveEDI999Response(TokenModel token, string fileText, EDI999SchemaModel ediResponseModel)
        {
            EDI999FileModel edi999FileModel = new EDI999FileModel();
            edi999FileModel.AcknowledgementType = ediResponseModel.AK2.AK201;
            edi999FileModel.ControlNumber = Convert.ToInt16(ediResponseModel.AK2.AK202);
            edi999FileModel.Status = ediResponseModel.IK5.IK501;
            edi999FileModel.EDIFileText = fileText.ToString();
            return _edi999Repository.SaveEDI999Acknowledgement<SQLResponseModel>(edi999FileModel, token).FirstOrDefault();
           
        }
    }
}
