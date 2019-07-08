using Shell32;
using System;
using System.IO;

namespace Mic.Repository.Utils
{
    public class ByShell32 : Duration
    {
        /// <summary>
        /// Get duration(ms) of audio or vedio by Shell32.dll
        /// </summary>
        /// <param name="filePath">audio/vedio's path</param>
        /// <returns>Duration in original format, duration in milliseconds</returns>
        /// <remarks>return value from Shell32.dll is in format of: "00:10:16"</remarks>
        public override Tuple<string, long> GetDuration(string filePath)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);

                // From Add Reference --> COM 
                Shell shell = new Shell32.Shell();
                Shell32.Folder folder = shell.NameSpace(dir);
                Shell32.FolderItem folderitem = folder.ParseName(Path.GetFileName(filePath));

                string duration = null;

                // Deal with different versions of OS
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    duration = folder.GetDetailsOf(folderitem, 27);
                }
                else
                {
                    duration = folder.GetDetailsOf(folderitem, 21);
                }

                duration = string.IsNullOrEmpty(duration) ? "00:00:00" : duration;
                return Tuple.Create(duration, GetTimeInMillisecond(duration));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
