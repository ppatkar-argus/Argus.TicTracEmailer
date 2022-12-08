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
    public static class Activity
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /*
        public static DateTime Start { get; set; } = new DateTime();

        public static DateTime End { get; set; } = new DateTime();

        public static int Weeks { get; set; } = 0;
        private static bool SetQuarterDates()
        {
            var quarter = CurrentQuarter();

            bool valid = true;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["OverrideQuarter"]))
            {
                Start = DateTime.Parse(ConfigurationManager.AppSettings["QuarterStart"]);
                End = DateTime.Parse(ConfigurationManager.AppSettings["QuarterEnd"]);
                Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
            }
            else if(Convert.ToBoolean(ConfigurationManager.AppSettings["RegularQuarter"]))
            {
                switch (quarter)
                {
                    case 1:
                        Start = new DateTime(DateTime.Today.Year - 1, 10, 1);
                        End = new DateTime(DateTime.Today.Year - 1, 12, 31);
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;
                    case 2:
                        Start = new DateTime(DateTime.Today.Year, 1, 1);
                        End = new DateTime(DateTime.Today.Year, 3, 31);
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;
                    case 3:
                        Start = new DateTime(DateTime.Today.Year, 4, 1);
                        End = new DateTime(DateTime.Today.Year, 6, 30);
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;
                    case 4:
                        Start = new DateTime(DateTime.Today.Year, 7, 1);
                        End = new DateTime(DateTime.Today.Year, 9, 30);
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;
                    default:
                        log.ErrorFormat("Quarter {0} not valid.", quarter);
                        valid = false;
                        break;
                }
            }
            else
            {
                switch (quarter)
                {
                    case 1:
                        var Q4Start = new DateTime(DateTime.Today.Year - 1, 10, 1);

                        while (Q4Start.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["StartDayOfQuarter"]))
                        {
                            Q4Start = Q4Start.AddDays(1);
                        }

                        Start = Q4Start;

                        var Q4End = new DateTime(DateTime.Today.Year - 1, 12, 31);

                        while (Q4End.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["EndDayOfQuarter"]))
                        {
                            Q4End = Q4End.AddDays(1);
                        }

                        End = Q4End;
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;

                    case 2:
                        var Q1Start = new DateTime(DateTime.Today.Year, 1, 1);

                        while (Q1Start.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["StartDayOfQuarter"]))
                        {
                            Q1Start = Q1Start.AddDays(1);
                        }

                        Start = Q1Start;

                        var Q1End = new DateTime(DateTime.Today.Year, 3, 31);

                        while (Q1End.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["EndDayOfQuarter"]))
                        {
                            Q1End = Q1End.AddDays(1);
                        }

                        End = Q1End;
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;

                    case 3:
                        var Q2Start = new DateTime(DateTime.Today.Year, 4, 1);

                        while (Q2Start.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["StartDayOfQuarter"]))
                        {
                            Q2Start = Q2Start.AddDays(1);
                        }

                        Start = Q2Start;

                        var Q2End = new DateTime(DateTime.Today.Year, 6, 30);

                        while (Q2End.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["EndDayOfQuarter"]))
                        {
                            Q2End = Q2End.AddDays(1);
                        }

                        End = Q2End;
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;
                    case 4:
                        var Q3Start = new DateTime(DateTime.Today.Year, 7, 1);

                        while (Q3Start.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["StartDayOfQuarter"]))
                        {
                            Q3Start = Q3Start.AddDays(1);
                        }

                        Start = Q3Start;

                        var Q3End = new DateTime(DateTime.Today.Year, 6, 30);

                        while (Q3End.DayOfWeek != (DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["EndDayOfQuarter"]))
                        {
                            Q3End = Q3End.AddDays(1);
                        }

                        End = Q3End;
                        Weeks = (int)(Math.Round((double)(End - Start).Days / 7));
                        break;
                    default:
                        log.ErrorFormat("Quarter {0} not valid.", quarter);
                        valid = false;
                        break;
                }
            }

            return valid;
        }

        public static int CurrentQuarter(DateTime? date = null)
        {
            int month;
            if (date.HasValue)
            {
                month = date.Value.Month;
            }
            else
            {
                month = DateTime.Now.Month;
            }
            return (month + 2) / 3;
        }

        public static DateTime GetStartDayOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        */
        
        public static void ProcessActivityEmails()
        {
            DirectoryInfo directory = new DirectoryInfo(Utilities.GetExecutingDirectoryPath() + "//" + "Activity");
            var files = directory.GetFiles();
            foreach (var file in files)
            {

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(file.FullName);

                var sheet = workbook.Worksheets[workbook.Worksheets.Count - 1];

                TicTracEntities entities = new TicTracEntities();
                var emailtemplate = entities.GetEmailTemplate(ConfigurationManager.AppSettings["ActivityEmailCode"]).FirstOrDefault();
                if (emailtemplate != null)
                {
                    //var valid = SetQuarterDates();

                    //if (valid)
                    //{
                        foreach (CellRange range in sheet.Columns[0])
                        {
                            if (range.Row > 1)
                            {
                                SendActivityEmail(sheet, range, emailtemplate.EmailBody, emailtemplate.EmailSubjectDescription);
                            }

                        }
                    //}
                }
                else
                {
                    log.ErrorFormat("Email Template not found for {0}.", ConfigurationManager.AppSettings["ActivityEmailCode"]);
                }

            }
        }

        public static void SendActivityEmail(Worksheet sheet, CellRange range, string emailBody, string emailSubject)
        {

            TicTracEntities entities = new TicTracEntities();
            int columnCount = sheet.Columns.Length;
            CellRange sourceRange = sheet.Range[range.Row, 1, range.Row, columnCount];

            var firstname = sourceRange.Rows[0].CellList[0].Value;
            var lastname = sourceRange.Rows[0].CellList[1].Value;
            var email = sourceRange.Rows[0].CellList[2].Value;

            if(string.IsNullOrWhiteSpace(email) == false)
            {
                var activeminutes = sourceRange.Rows[0].CellList[6].Value;
                //var rewardminutes = sourceRange.Rows[0].CellList[7].Value;
                var rewards = sourceRange.Rows[0].CellList[7].Value;
                var start = Convert.ToDateTime(sourceRange.Rows[0].CellList[8].Value);
                var end = Convert.ToDateTime(sourceRange.Rows[0].CellList[9].Value);
                var rewardweeks = sourceRange.Rows[0].CellList[10].Value;
                var weeks = (int)(Math.Round((double)(end - start).Days / 7));
                if (emailBody != null)
                {
                    emailBody = emailBody.Replace("#Name", firstname + " " + lastname);
                    emailBody = emailBody.Replace("#ActiveMinutes ", activeminutes);
                    emailBody = emailBody.Replace("#Rewards", rewards);
                    emailBody = emailBody.Replace("#Start", start.ToString("MM/dd/yyyy"));
                    emailBody = emailBody.Replace("#End", end.ToString("MM/dd/yyyy"));
                    emailBody = emailBody.Replace("#Weeks", weeks.ToString());
                    emailBody = emailBody.Replace("#RewardWeeks", rewardweeks);

                    int result = 0;
                    bool valid = int.TryParse(activeminutes, out result);
                    if (string.IsNullOrEmpty(activeminutes) == false && valid && result > 0)
                    {
                        entities.SendEmail(emailBody, emailSubject, email, null, null, ConfigurationManager.AppSettings["EmailFrom"], ConfigurationManager.AppSettings["EmailFromName"], true);
                    }
                    else
                    {
                        log.InfoFormat("Email not sent to {0} , email-id {1} as thier active minutes are {2}.", firstname + " " + lastname, email, activeminutes);
                    }

                }
            }
            
        }
    }
}
