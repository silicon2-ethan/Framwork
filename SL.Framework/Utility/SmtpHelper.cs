using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace SL.Framework.Utility
{
    /// <summary>
    /// SMTP 도우미
    /// </summary>
    public class SmtpHelper
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailMessage _message;
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ssl"></param>
        public SmtpHelper(string host, int port, string username, string password, bool ssl)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                _smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = ssl,
                    Credentials = new NetworkCredential(username, password)
                };
            }
            else
            {
                _smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = ssl
                };
            }

            _message = new MailMessage();

            Subject = string.Empty;
            Body = string.Empty;
            IsBodyHtml = true;
        }

        /// <summary>
        /// 보내는사람
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public void FromAddress(string email, string name = null)
        {
            if (string.IsNullOrEmpty(email)) return;
            email = email.Replace(",", "").Replace(";", "");
            _message.From = new MailAddress(email, name ?? string.Empty);
        }

        /// <summary>
        /// 받는사람
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public void AddToAddress(string email, string name = null)
        {
            if (string.IsNullOrEmpty(email)) return;
            email = email.Replace(",", "").Replace(";", "");
            _message.To.Add(new MailAddress(email, name ?? string.Empty));
        }

        /// <summary>
        /// 참조
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public void AddCcAddress(string email, string name = null)
        {
            if (string.IsNullOrEmpty(email)) return;
            email = email.Replace(",", "").Replace(";", "");
            _message.CC.Add(new MailAddress(email, name ?? string.Empty));
        }

        /// <summary>
        /// 숨은참조
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        public void AddBccAddress(string email, string name = null)
        {
            if (string.IsNullOrEmpty(email)) return;
            email = email.Replace(",", "").Replace(";", "");
            _message.Bcc.Add(new MailAddress(email, name ?? string.Empty));
        }

        /// <summary>
        /// 첨부파일
        /// </summary>
        /// <param name="file"></param>
        /// <param name="mimeType"></param>
        public void AddAttachment(string file, string mimeType)
        {
            var attachment = new Attachment(file, mimeType);
            _message.Attachments.Add(attachment);
        }

        /// <summary>
        /// 첨부파일
        /// </summary>
        /// <param name="objAttachment"></param>
        public void AddAttachment(Attachment objAttachment)
        {
            _message.Attachments.Add(objAttachment);
        }

        /// <summary>
        /// 메일보내기
        /// </summary>
        public void SendMail()
        {
            try
            {
                if (_message.From == null)
                {
                    throw new Exception("From address not defined");
                }

                if (string.IsNullOrEmpty(_message.From.Address))
                {
                    throw new Exception("From address not defined");
                }

                if (_message.To.Count <= 0)
                {
                    throw new Exception("To address not defined");
                }

                _message.Subject = Subject;
                _message.Body = Body;
                _message.IsBodyHtml = IsBodyHtml;
                _smtpClient.Send(_message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetFileMimeType(string fileName)
        {
            var fileExt = Path.GetExtension(fileName.ToLower());
            string mimeType;
            switch (fileExt)
            {
                case ".htm":
                case ".html":
                    mimeType = "text/html";
                    break;
                case ".xml":
                    mimeType = "text/xml";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimeType = "image/jpeg";
                    break;
                case ".gif":
                    mimeType = "image/gif";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".bmp":
                    mimeType = "image/bmp";
                    break;
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".doc":
                    mimeType = "application/msword";
                    break;
                case ".docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    mimeType = "application/x-msexcel";
                    break;
                case ".xlsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".csv":
                    mimeType = "application/csv";
                    break;
                case ".ppt":
                    mimeType = "application/vnd.ms-powerpoint";
                    break;
                case ".pptx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case ".rar":
                    mimeType = "application/x-rar-compressed";
                    break;
                case ".zip":
                    mimeType = "application/x-zip-compressed";
                    break;
                default:
                    mimeType = "text/plain";
                    break;
            }
            return mimeType;
        }
    }
}