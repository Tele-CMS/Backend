using EDIParser.IServices;
using EDIParser.Model;
using HC.Model;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.EDI;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System.IO;

namespace HC.Patient.Service.Services.EDI
{
    public class EDI271ParserService :BaseService, IEDI271ParserService
    {
        private readonly IEdiGatewayService _ediGatewayService;
        private readonly IEDI271Service _edi271Service;
        public EDI271ParserService(IEdiGatewayService ediGatewayService,IEDI271Service edi271Service)
        {
            _ediGatewayService = ediGatewayService;
            _edi271Service = edi271Service;
        }
        public JsonModel ReadEDI271(TokenModel token)
        {
            EDI271SchemaModel ediResponseModel = new EDI271SchemaModel();
            ClearingHouseModel clearingHouse = _ediGatewayService.GetActiveClearingHouseDetails(token);
            string directoryPath = Directory.GetCurrentDirectory() + "\\Agency";
            string fullPath = directoryPath + "\\" + token.DomainName;
            string fileText = string.Empty;
            #region uncomment for  testing purpose
            fileText = System.IO.File.ReadAllText(fullPath + "\\Test_271File.txt");
            ediResponseModel = _edi271Service.ParseEDI271(fileText);
            #endregion

            //#region Real Code to be used in production
            //if (clearingHouse != null)
            //{
            //    SftpClient client = _ediGatewayService.CreateConnection(clearingHouse);
            //    if (client != null && client.ConnectionInfo.IsAuthenticated)
            //    {
            //        var ftpDirectory = "/" + clearingHouse.Path271 + "/";
            //        var files = client.ListDirectory(ftpDirectory);
            //        foreach (var file in files)
            //        {
            //            if (clearingHouse.FTPURL == "ftp.officeally.com")
            //            {
            //                bool containsZip = file.FullName.Contains(".zip");
            //                if (containsZip)
            //                {
            //                    FileStream fs = new FileStream(fullPath + "\\" + clearingHouse.Path271 + "\\" + file.Name, FileMode.OpenOrCreate);
            //                    client.DownloadFile(file.FullName, fs);
            //                    fs.Dispose();
            //                    using (ZipArchive archive = ZipFile.OpenRead(fullPath + "\\" + clearingHouse.Path271 + "\\" + file.Name))
            //                    {
            //                        foreach (ZipArchiveEntry entry in archive.Entries)
            //                        {
            //                            if (entry.FullName.EndsWith(".271", StringComparison.OrdinalIgnoreCase))
            //                            {
            //                                try
            //                                {
            //                                    entry.ExtractToFile(Path.Combine(fullPath + "\\" + clearingHouse.Path271, entry.FullName), true);
            //                                    using (StreamReader streamReader = new StreamReader(fullPath + "\\" + entry.FullName, Encoding.UTF8))
            //                                    {
            //                                        fileText = streamReader.ReadToEnd();
            //                                        ediResponseModel = _edi271Service.ParseEDI271(fileText);
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
            //                FileStream fs = new FileStream(fullPath + "\\" + clearingHouse.Path271 + "\\" + file.Name, FileMode.OpenOrCreate);
            //                client.DownloadFile(file.FullName, fs);
            //                fs.Close();
            //                using (StreamReader streamReader = new StreamReader(fullPath + "\\" + clearingHouse.Path271 + "\\" + file.Name, Encoding.UTF8))
            //                {
            //                    fileText = streamReader.ReadToEnd();
            //                    ediResponseModel = _edi271Service.ParseEDI271(fileText);
            //                    //client.DeleteFile(file.FullName);
            //                    client.Disconnect();
            //                    client.Dispose();
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion

            return new JsonModel()
            {
                data = ediResponseModel,
                Message = "",
                StatusCode = 1
            };
        }
    }
}
