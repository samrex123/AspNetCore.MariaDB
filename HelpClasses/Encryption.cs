﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.MariaDB.HelpClasses
{
    public class Encryption
    {
        /// <summary>
        /// Krypterar text
        /// </summary>
        /// <param name="clearText">Texten som ska krypteras</param>
        /// <param name="secretkey">Nyckel som används för kryptering</param>
        /// <returns></returns>
        public static string Encrypt(string clearText, string secretkey)
        {
            string EncryptionKey = secretkey;
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
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}
