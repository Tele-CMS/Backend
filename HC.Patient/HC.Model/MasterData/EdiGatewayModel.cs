using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class ClearingHouseModel
    {
        public  int Id { get; set; }
        public string ClearingHouseName { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string InterchangeQualifier { get; set; }
        public string FTPURL { get; set; }
        public string PortNo { get; set; }
        public string FTPUsername { get; set; }
        public string FTPPassword { get; set; }
        public string Path837 { get; set; }
        public string Path835 { get; set; }
        public string Path277 { get; set; }
        public string Path999 { get; set; }
        public string Path270 { get; set; }
        public string Path271 { get; set; }
        public string Path276 { get; set; }
    }
}
