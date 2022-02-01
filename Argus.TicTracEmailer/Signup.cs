using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Argus.TicTracEmailer
{
    public static class Signup
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void SendSignupEmail(Worksheet sheet, CellRange range, string emailBody, string emailSubject)
        {

            TicTracEntities entities = new TicTracEntities();
            int columnCount = sheet.Columns.Length;
            CellRange sourceRange = sheet.Range[range.Row, 1, range.Row, columnCount];

            var email = sourceRange.Rows[0].CellList[4].Value;
            if (emailBody != null)
            {
                entities.SendEmail(emailBody, emailSubject, email, null, null, ConfigurationManager.AppSettings["EmailFrom"], ConfigurationManager.AppSettings["EmailFromName"], true);

            }
        }


        public static void ProcessSignupEmails()
        {

            DirectoryInfo directory = new DirectoryInfo(Utilities.GetExecutingDirectoryPath() + "//" + "Signup");
            var files = directory.GetFiles();
            foreach (var file in files)
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(file.FullName);

                var sheet = workbook.Worksheets[workbook.Worksheets.Count - 1];

                TicTracEntities entities = new TicTracEntities();
                var emailtemplate = entities.GetEmailTemplate(ConfigurationManager.AppSettings["SignupEmailCode"]).FirstOrDefault();

                if (emailtemplate != null)
                {
                    foreach (CellRange range in sheet.Columns[0])
                    {
                        if (range.Row > 1)
                        {
                            SendSignupEmail(sheet, range, emailtemplate.EmailBody, emailtemplate.EmailSubjectDescription);
                        }

                    }
                }
                else
                {
                    log.ErrorFormat("Email Template not found for {0}.", ConfigurationManager.AppSettings["SignupEmailCode"]);
                }

            }
        }

    }
}
