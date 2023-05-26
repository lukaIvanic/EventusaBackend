using System.Net.Mail;
using System.Net;
using System.Text;

namespace EventusaBackend.CalendarUtils
{
    public class Calendar
    {
        //Dodaje na kalendar
        public static void Generiraj(int id, DateTime start, DateTime end, string location, string subject, string description)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "email-smtp.eu-west-1.amazonaws.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("AKIASY3KMGVCHBHJE4MG", "BHsyns/ips5qRw+3QF4WQWaO9qAlNpEz2CVmUI5mac+D");

            // Now Contruct the ICS file using string builder
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("info@ri-ing.net", "EVENTUŠA");
            msg.To.Add(new MailAddress("riingnetdoo@gmail.com", "RI-ING NET"));
            //msg.To.Add(new MailAddress("luka.ivanic7@gmail.com", "Luka Ivanic"));
            msg.Subject = subject;
            msg.Body = description;

            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Novi sastanak");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("X-WR-RELCALID:" + id);
            str.AppendLine("METHOD:REQUEST");
            str.AppendLine("BEGIN:VEVENT");
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", start));//-2 hack time zone
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmss}", DateTime.UtcNow));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", end));
            if (location != "")
            {
                str.AppendLine("LOCATION:" + location);
            }
            str.AppendLine(string.Format("UID:{0}", id));
            str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            //str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
            contype.Parameters.Add("method", "REQUEST");
            contype.Parameters.Add("name", "Meeting.ics");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);

            msg.AlternateViews.Add(avCal);
            smtpClient.Send(msg);
        }

        //Briše s kalendara
        public static void Cancel(int id, DateTime start, DateTime end, string location, string subject, string description)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "email-smtp.eu-west-1.amazonaws.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("AKIASY3KMGVCHBHJE4MG", "BHsyns/ips5qRw+3QF4WQWaO9qAlNpEz2CVmUI5mac+D");

            // Now Contruct the ICS file using string builder
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("info@ri-ing.net", "EVENTUŠA");
            //msg.To.Add(new MailAddress("riingnetdoo@gmail.com", "RI-ING NET"));
            msg.To.Add(new MailAddress("luka.ivanic7@gmail.com", "Luka Ivanic"));
            msg.Subject = subject;
            msg.Body = description;

            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("PRODID:-//Novi sastanak");
            str.AppendLine("X-WR-RELCALID:" + id);
            str.AppendLine("METHOD:CANCEL");
            str.AppendLine("BEGIN:VEVENT");
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", start));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", end));
            if (location != "")
            {
                str.AppendLine("LOCATION:" + location);
            }
            str.AppendLine(string.Format("UID:{0}", id));
            str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            str.AppendLine("SEQUENCE:1");
            str.AppendLine("CLASS:PUBLIC");
            str.AppendLine("STATUS:CANCELLED");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
            contype.Parameters.Add("method", "CANCEL");
            contype.Parameters.Add("name", "Meeting.ics");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);

            msg.AlternateViews.Add(avCal);
            smtpClient.Send(msg);
        }
    }
}
