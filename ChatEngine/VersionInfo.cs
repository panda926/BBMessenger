using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class VersionInfo
    {
        public static string localVersionFileName = "local.version";
        public static string serverVersionFileName = "server.version";

        public static List<DownloadFileInfo> ReadInfoFile(string folderName, bool isLocal )
        {
            List<DownloadFileInfo> fileVersions = new List<DownloadFileInfo>();

            string versionFileName = localVersionFileName;
            if (isLocal == false)
                versionFileName = serverVersionFileName;

            string infoName = string.Format("{0}\\{1}", folderName, versionFileName);

            if (File.Exists(infoName))
            {
                string text = System.IO.File.ReadAllText(infoName);

                string[] fileList = text.Split('\n');

                foreach (string fileNameString in fileList)
                {
                    if (string.IsNullOrEmpty(fileNameString) == true)
                        continue;

                    string fileName = fileNameString.Split('\r')[0];

                    if (string.IsNullOrEmpty(fileName) == true)
                        continue;

                    string[] stringSeparators = new string[] { "||" };
                    string[] strFileNameAndVersions = fileName.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                    DownloadFileInfo fileInfo = new DownloadFileInfo();
                    fileInfo.FilePath = strFileNameAndVersions[0];
                    fileInfo.FileVersion = Convert.ToInt32(strFileNameAndVersions[1]);

                    fileVersions.Add(fileInfo);
                }

            }

            return fileVersions;
        }

        public static bool WriteInfoFile(string folderName, List<DownloadFileInfo> fileVersions, bool isLocal )
        {
            string versionFileName = localVersionFileName;

            if (isLocal == false)
                versionFileName = serverVersionFileName;

            string infoName = string.Format("{0}\\{1}", folderName, versionFileName);

            if (File.Exists(infoName))
                File.Delete(infoName);

            string infoText = string.Empty;

            foreach (DownloadFileInfo fileInfo in fileVersions)
            {
                infoText += string.Format("{0}||{1}\r\n", fileInfo.FilePath, fileInfo.FileVersion);
            }

            File.WriteAllText(infoName, infoText);

            return true;
        }
    }

    public class DownloadFileInfo
    {
        public string FilePath;
        public int FileVersion;
    }

}
