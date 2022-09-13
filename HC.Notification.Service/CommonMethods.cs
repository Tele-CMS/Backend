using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HC.Notification.Service
{
    public static class CommonMethods
    {
        public static DateTime ConvertFromUtcTimeWithOffset(DateTime Date, decimal DaylightOffset, decimal StandardOffset, string TimeZoneName)
        {
               TimeZoneInfo locationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneName);
            if ((Date > DateTime.MinValue && Date < DateTime.MaxValue))
            {
                if (DaylightOffset == StandardOffset)
                {
                    Date = Date.AddMinutes((double)StandardOffset);
                }
                else
                {
                    if (locationTimeZone.IsAmbiguousTime(Date) || locationTimeZone.IsDaylightSavingTime(Date))
                    {
                        Date = Date.AddMinutes((double)DaylightOffset);
                    }
                    else
                    {
                        Date = Date.AddMinutes((double)StandardOffset);
                    }
                }
                return Date;
            }
            else
            {
                return Date;
            }
        }

        /// <summary>
        /// encrypt the simple text 
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = EncryptDecryptKey.Key;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Dispose();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// decrypt the encrypt data
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = EncryptDecryptKey.Key;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Dispose();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static decimal GetMin(decimal number)
        {
            string s = number.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = s.Split('.');
            //
            decimal i1 = int.Parse(parts[0]) * 60;
            decimal i2 = int.Parse(parts[1]);
            return i1 + i2;
        }
    }
    public static class EncryptDecryptKey
    {
        public readonly static string Key = "MAKV2SPBNI99212";
        public readonly static string PHIKey = "~!@#$%^*HeaLthC@re$smaRtData~!@!!!=";
    }
}
