using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ChatEngine
{
    public class FileEncoder
    {
        public static void DecryptToFile(string inputFilePath, string outputfilePath)
        {
            FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create);

            DecryptToStream(inputFilePath, fsOutput);
        }

        public static MemoryStream DecryptToMemory(string inputFilePath)
        {
            MemoryStream memoryOutput = new MemoryStream();

            DecryptToStream(inputFilePath, memoryOutput);

            return memoryOutput;
        }

        private static void DecryptToStream(string inputFilePath, Stream outputStream )
        {
            string EncryptionKey = "PAKCHOLJIN1977530";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        int data;
                        while ((data = cs.ReadByte()) != -1)
                        {
                            outputStream.WriteByte((byte)data);
                        }
                    }
                }
            }
        }

    }
}
