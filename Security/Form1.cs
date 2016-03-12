using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Security
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CustomEncrypt.Run();

            //string encName = "d:\\encrypt.png";
            //string decName = "d:\\decrypt.png";

            //File.Delete( encName );
            //File.Delete( decName );

            //Encrypt("d:\\test.png", encName);
            //Decrypt(encName, decName);

        }

        private void Encrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "PAKCHOLJIN1977530";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        private void Decrypt(string inputFilePath, string outputfilePath)
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
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo sourceFolderInfo = new DirectoryInfo(SourceFolderPath.Text);
                UpdatedBox.Items.Clear();

                //DateTime StartOf2009 = new DateTime(2009, 01, 01);

                // LINQ query for all files created before 2009.
                var files = from f in sourceFolderInfo.EnumerateFiles()
                            //where f.CreationTimeUtc < StartOf2009
                            select f;

                // Show results.
                int curIndex = 1;
                int updatedCount = 0;

                int curYear = DateTime.Now.Year;
                int curMonth = DateTime.Now.Month;
                int curDay = DateTime.Now.Day;

                foreach (var fileInfo in files)
                {
                    if (fileInfo.LastWriteTime.Year == curYear && fileInfo.LastWriteTime.Month == curMonth &&
                        fileInfo.LastWriteTime.Day == curDay)
                    {
                        //UpdatedBox.Items.Add(fileInfo.Name);
                        updatedCount++;
                    }
                    else
                    {
                        string encName = string.Format("{0}\\{1}_enc", fileInfo.Directory, fileInfo.Name);

                        if (File.Exists(encName))
                        {
                            File.SetAttributes(encName, FileAttributes.Normal);
                            File.Delete(encName);
                        }

                        Encrypt(fileInfo.FullName, encName);

                        fileInfo.IsReadOnly = false;
                        fileInfo.Delete();

                        File.Move(encName, fileInfo.FullName);
                    }

                    StateLabel.Text = string.Format("{0}/{1} - {2}", curIndex, updatedCount, fileInfo.FullName);
                    
                    curIndex++;

                    if (curIndex % 10 == 0)
                    {
                        this.Invalidate();
                        this.Update();
                        Application.DoEvents();
                    }
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(SourceFolderPath.Text);


            foreach (FileInfo fileInfo in sourceFolderInfo.GetFiles())
            {
                string decName = string.Format("{0}\\{1}_dec", fileInfo.Directory, fileInfo.Name.Substring(0, fileInfo.Name.Length-4));

                if (File.Exists(decName))
                {
                    File.SetAttributes(decName, FileAttributes.Normal); 
                    File.Delete(decName);
                }

                Decrypt(fileInfo.FullName, decName);

                fileInfo.IsReadOnly = false;
                fileInfo.Delete();

                File.Move(decName, fileInfo.FullName);
            }
        }
    }
}
