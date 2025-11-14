using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using static MySystem.Base.EmailAttributes;

namespace MySystem.Base.Helpers
{
  public static class MailHelper
  {
    public static readonly string SEND_SUCCESS = "SUCCESS!";
#if DEBUG
    public static bool MAIL_TEST_MODE = true;
#else
    public static bool MAIL_TEST_MODE = false;
#endif
    public static string SendEmail(EmailAttributes setting, string[] Attachments, string ReadReceipt = null, bool IsBodyHtml = false)
    {
      try
      {
        return SendEmail(setting, 
                         (Attachments?.Where(x => string.IsNullOrWhiteSpace(x) == false)?.ToList() ?? new List<string>())
                                     ?.Let(x => {
                                            if (x.Any())
                                              return x.Select(y => new Attachment(y));
                                            else
                                              return new List<Attachment>();
                                            }),
                         ReadReceipt,
                         IsBodyHtml);
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
    public static string SendEmail(EmailAttributes setting, Attachment attachment = null, string ReadReceipt = null, bool IsBodyHtml = false)
    {
      try
      {
        return SendEmail(setting, (attachment != null) ? new Attachment[] { attachment } : null, ReadReceipt, IsBodyHtml);
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public static string SendEmail(EmailAttributes setting, IEnumerable<Attachment> Attachments, string ReadReceipt, bool IsBodyHtml = false)
    {
      try
      {
        if (setting == null) return "Wrong setting";
        return MailHelper.SendEmail(
            (setting[C_RECIPIENT] as string)?.Split(';'),
            (setting[C_CCRECIPIENT] as string)?.Split(';'),
            (setting[C_BCCRECIPIENT] as string)?.Split(';'),
            setting[C_FROM] as string, setting[C_DISPLAYNAME] as string, 
            setting[C_SUBJECT] as string, 
            setting[C_BODY] as string, 
            setting[C_USERNAME] as string, setting[C_PASSWORD] as string, 
            setting[C_SERVER] as string, (setting[C_PORT]).ToInt(), Attachments, (setting[C_SSL]).ToBool(), 
            ReadReceipt, IsBodyHtml);
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }


    public static string SendEmail(string[] Recipients, string[] CCRecipients, string[] BCCRecipients, string FromAddress, string DisplayName, string Subject,
                                   string Body, string UserName, string Password, string Server,
                                   int Port, IEnumerable<Attachment> Attachments, bool EnableSSL, string ReadReceipt, bool IsBodyHtml = false)
    {
      if ((Recipients?.Length ?? 0) == 0 && (CCRecipients?.Length ?? 0) == 0 && (BCCRecipients?.Length ?? 0) == 0)
        return "No recipients";
      MailMessage msg = null;
      try
      {
        msg = new MailMessage { From = new MailAddress(FromAddress, DisplayName), Subject = Subject, Body = Body, IsBodyHtml = IsBodyHtml };

        if (MAIL_TEST_MODE)
          msg.To.Add("alex@naturoaid.com");
        else
        {
          Recipients?.Where(x => x.Length > 0).Action(x => msg.To.Add(x));
          CCRecipients?.Where(x => x.Length > 0).Action(x => msg.CC.Add(x));
          BCCRecipients?.Where(x => x.Length > 0).Action(x => msg.Bcc.Add(x));
        }

        Attachments?.Action(x => msg.Attachments.Add(x));

        SmtpClient SMTPServer = new SmtpClient { Host = Server, Port = Port, EnableSsl = EnableSSL };
        if (string.IsNullOrWhiteSpace(UserName))
          SMTPServer.UseDefaultCredentials = true;
        else
          SMTPServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
        if (string.IsNullOrWhiteSpace(ReadReceipt) == false)
        {
          msg.Headers.Add("X-Confirm-Reading-To", ReadReceipt);
          msg.Headers.Add("Return-Receipt-To", ReadReceipt);
          msg.Headers.Add("Disposition-Notification-To", ReadReceipt);
        }

        SMTPServer.Send(msg);

        msg.Dispose();
        return SEND_SUCCESS;
      }
      catch (SmtpException ex)
      {
        msg?.Dispose();
        return ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.Message : "");
      }
      catch (Exception ex)
      {
        msg?.Dispose();
        return ex.Message + " " + ((ex.InnerException != null) ? ex.InnerException.Message : "");
      }
    }
    static Regex ValidEmailRegex = CreateValidEmailRegex();

    /// <summary>
    /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
    /// </summary>
    /// <returns></returns>
    private static Regex CreateValidEmailRegex()
    {
      string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
          + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
          + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

      return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
    }

    public static bool EmailIsValid(string emailAddress)
    {
      bool isValid = ValidEmailRegex.IsMatch(emailAddress);

      return isValid;
    }
  }
}