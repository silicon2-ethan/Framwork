using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace SL.Framework.Utility
{
    public class MailSenderHelper : IDisposable
    {
        readonly string _smtpserver = "mail.siliconii.net";
        string _fromAdress { get; set; }   //보내는 사람 메일 주소(관리부서에서 메일 정보 받아야됨)
        string _uid { get; set; }  //메일 계정(관리부서에서 메일 정보 받아야됨)
        string _pwd { get; set; }  //메일 비번(관리부서에서 메일 정보 받아야됨)

        List<string> to { get; set; }
        List<string> cc { get; set; }

        #region [Init]
        public MailSenderHelper(List<string> _to, List<string> _cc = null) : base()
        {
            #region [To]
            if (_to == null)
            {
                _to = new List<string>();
                to = _to;
            }
            else
            {
                to = _to;
            }
            #endregion

            #region [CC]
            if (_cc == null)
            {
                _cc = new List<string>();
                cc = _cc;
            }
            else
            {
                cc = _cc;
            }
            #endregion
        }

        public MailSenderHelper(List<string> _to, List<string> _cc, string fromA, string id, string pwd) : this(_to, _cc)
        {
            _fromAdress = fromA;
            _uid = id;
            _pwd = pwd;
        }
        #endregion

        /// <summary>
        /// 실제 메일 발송
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void Send(string title, string content, List<Attachment> attachMents = null)
        {
            if (title.vIsEmpty()) return;
            if (content.vIsEmpty()) return;

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    SmtpClient SmtpServer = new SmtpClient(_smtpserver);
                    mail.From = new MailAddress(_fromAdress);

                    #region [To]
                    if (to.Count > 0)
                    {
                        foreach (var item in to)
                        {
                            mail.To.Add(item);
                        }
                    }
                    #endregion

                    mail.Subject = title;
                    mail.Body = content;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.Normal;
                    if(attachMents != null)
                    {
                        if(attachMents.Count > 0)
                        {
                            foreach (var item in attachMents)
                            {
                                mail.Attachments.Add(item);
                            }
                        }                            
                    }
                    
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(_uid, _pwd);
                    SmtpServer.EnableSsl = false;  //서버에 인증서가 설치되어 있지 않으므로, false

                    #region [CC]
                    if (cc.Count > 0)
                    {
                        foreach (var item in cc)
                        {
                            mail.CC.Add(item);
                        }
                    }
                    #endregion

                    SmtpServer.Send(mail);
                } //using (MailMessage mail = new MailMessage())
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {

        }
    }
}
