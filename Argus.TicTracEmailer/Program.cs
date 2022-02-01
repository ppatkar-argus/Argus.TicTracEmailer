using Microsoft.Win32.SafeHandles;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Argus.TicTracEmailer
{
    class Program
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
       int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);


        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var param = args[0];
                log4net.Config.XmlConfigurator.Configure();

                Spire.License.LicenseProvider.SetLicenseKey(ConfigurationManager.AppSettings["SpireXlsLicenseKey"]);

                const int LOGON32_PROVIDER_DEFAULT = 0;
                //This parameter causes LogonUser to create a primary token.   
                const int LOGON32_LOGON_INTERACTIVE = 2;

                // Call LogonUser to obtain a handle to an access token.   
                SafeAccessTokenHandle safeAccessTokenHandle;
                bool returnValue = LogonUser(ConfigurationManager.AppSettings["fileshare:UserName"], ConfigurationManager.AppSettings["fileshare:DomainName"], ConfigurationManager.AppSettings["fileshare:Password"],
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    out safeAccessTokenHandle);

                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    Console.WriteLine("LogonUser failed with error code : {0}", ret);
                    throw new System.ComponentModel.Win32Exception(ret);
                }


                WindowsIdentity.RunImpersonated(
                    safeAccessTokenHandle,
                    // User action  
                    () =>
                    {
                        if (param == "SIGNUP" || param == "ALL")
                        {
                            log.InfoFormat("Copy Source Signup Files started.");
                            CopyFiles(ConfigurationManager.AppSettings["SignupFilePath"], Utilities.GetExecutingDirectoryPath() + "//" + "Signup");
                            log.InfoFormat("Copy Source Signup Files completed.");

                            log.InfoFormat("Archive Source Signup Files started.");
                            ArchiveFiles(ConfigurationManager.AppSettings["SignupFilePath"]);
                            log.InfoFormat("Archive Source Signup Files completed.");
                        }
                        else if (param == "ACTIVITY" || param == "ALL")
                        {
                            log.InfoFormat("Copy Source Activity Files started.");
                            CopyFiles(ConfigurationManager.AppSettings["ActivityFilePath"], Utilities.GetExecutingDirectoryPath() + "//" + "Activity");
                            log.InfoFormat("Copy Source Activity Files completed.");

                            log.InfoFormat("Archive Source Activity Files started.");
                            ArchiveFiles(ConfigurationManager.AppSettings["ActivityFilePath"]);
                            log.InfoFormat("Archive Source Activity Files completed.");
                        }

                    }
                    );


                if (param == "SIGNUP" || param == "ALL")
                {
                    log.InfoFormat("Copy Source Signup Files started.");
                    CopyFiles(ConfigurationManager.AppSettings["SignupFilePath"], Utilities.GetExecutingDirectoryPath() + "//" + "Signup");
                    log.InfoFormat("Copy Source Signup Files completed.");

                    log.InfoFormat("Archive Source Signup Files started.");
                    ArchiveFiles(ConfigurationManager.AppSettings["SignupFilePath"]);
                    log.InfoFormat("Archive Source Signup Files completed.");

                    log.InfoFormat("Process Signup Emails started.");
                    Signup.ProcessSignupEmails();
                    log.InfoFormat("Process Signup Emails completed.");

                    log.InfoFormat("Local Signup Files cleanup started.");
                    DeleteFiles(Utilities.GetExecutingDirectoryPath() + "//" + "Signup");
                    log.InfoFormat("Local Signup Files cleanup completed.");
                }
                else if (param == "ACTIVITY" || param == "ALL")
                {
                    log.InfoFormat("Copy Source Activity Files started.");
                    CopyFiles(ConfigurationManager.AppSettings["ActivityFilePath"], Utilities.GetExecutingDirectoryPath() + "//" + "Activity");
                    log.InfoFormat("Copy Source Activity Files completed.");

                    log.InfoFormat("Archive Source Activity Files started.");
                    ArchiveFiles(ConfigurationManager.AppSettings["ActivityFilePath"]);
                    log.InfoFormat("Archive Source Activity Files completed.");

                    log.InfoFormat("Process Activity Emails started.");
                    Activity.ProcessActivityEmails();
                    log.InfoFormat("Process Activity Emails completed.");

                    log.InfoFormat("Local Activity Files cleanup started.");
                    DeleteFiles(Utilities.GetExecutingDirectoryPath() + "//" + "Activity");
                    log.InfoFormat("Local Activity Files cleanup completed.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
       
        private static void CopyFiles(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            {
                DirectoryInfo directory = new DirectoryInfo(source);
                var files = directory.GetFiles();
                if(files.Length == 0)
                {
                    log.InfoFormat("No file found at {0}", source);
                    return;
                }
                foreach (var file in files)
                {
                    var destinationFile = destination + "\\" + DateTime.Now.ToString("MMddyyyyhhmmss") + "_" + file.Name;
                    File.Copy(file.FullName, destinationFile);
                    log.InfoFormat("File copied from {0} to {1}", file.FullName, destinationFile);
                }
            }
        }

        private static void DeleteFiles(string filepath)
        {
            DirectoryInfo directory = new DirectoryInfo(filepath);
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                file.Delete();
                log.InfoFormat("{0} file deleted.", file.FullName);
            }
        }

        private static void ArchiveFiles(string filepath)
        {
            DirectoryInfo directory = new DirectoryInfo(filepath + "\\Archive\\");
            if (!directory.Exists)
            {
                directory.Create();
            }

            directory = new DirectoryInfo(filepath);
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                var destination = filepath + "\\Archive\\" + DateTime.Now.ToString("MMddyyyyhhmmss") + "_" + file.Name;

                file.CopyTo(destination);
                file.Delete();
                log.InfoFormat("{0} file moved to Archive as {1}.", file.Name, destination);
            }
        }

    }
}

public static class Utilities
{
    public static string GetExecutingDirectoryPath()
    {
        var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
        return new FileInfo(location.AbsolutePath).Directory.FullName;
    }
}
