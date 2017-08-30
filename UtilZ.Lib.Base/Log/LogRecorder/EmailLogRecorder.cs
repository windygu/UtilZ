using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Config.Framework;
using UtilZ.Lib.Base.NLog.Config.Interface;
using UtilZ.Lib.Base.NLog.Layout;
using UtilZ.Lib.Base.NLog.LogRecorderInterface;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.LogRecorder
{
    /// <summary>
    /// 邮件日志记录器
    /// </summary>
    public class EmailLogRecorder : BaseLogRecorder, IEmailLogRecorder
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        public override IConfig BaseConfig
        {
            get { return this.Config; }
            set { this.Config = value as IEmailLogConfig; }
        }

        /// <summary>
        /// 配置
        /// </summary>
        private IEmailLogConfig _config = null;

        /// <summary>
        /// 配置
        /// </summary>
        public IEmailLogConfig Config
        {
            get { return _config; }
            set
            {
                this._config = value;
                if (this._config == null)
                {
                    return;
                }

                if (this._config.From != null)
                {
                    this._from = new MailAddress(this._config.From.Addr, this._config.From.DisplayName);
                }
                else
                {
                    this._config = null;
                    throw new ArgumentNullException("Value.From");
                }

                this.AddMailAddress(this._to, this._config.To);
                this.AddMailAddress(this._cc, this._config.CC);
                this.AddMailAddress(this._bcc, this._config.Bcc);
            }
        }

        /// <summary>
        /// 添加邮件地址到集合中
        /// </summary>
        /// <param name="mailAddrs">目标邮件地址集合</param>
        /// <param name="mailAddrConfigCollection">源配置邮件地址集合</param>
        private void AddMailAddress(List<MailAddress> mailAddrs, MailAddrConfigCollection mailAddrConfigCollection)
        {
            mailAddrs.Clear();
            if (mailAddrConfigCollection != null && mailAddrConfigCollection.Count > 0)
            {
                foreach (MailAddrConfigElement ma in mailAddrConfigCollection)
                {
                    mailAddrs.Add(new MailAddress(ma.Addr, ma.DisplayName));
                }
            }
        }

        /// <summary>
        /// 发送件人地址
        /// </summary>
        private MailAddress _from;

        /// <summary>
        /// 收件人地址集合
        /// </summary>
        private readonly List<MailAddress> _to = new List<MailAddress>();

        /// <summary>
        /// 抄送收件人集合
        /// </summary>
        private readonly List<MailAddress> _cc = new List<MailAddress>();

        /// <summary>
        /// 密件抄送收件人集合
        /// </summary>
        private readonly List<MailAddress> _bcc = new List<MailAddress>();

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(Model.LogItem item)
        {
            this.WriteLog(new List<LogItem> { item });
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        public override void WriteLog(List<Model.LogItem> items)
        {
            if (this.Config == null || !this.Config.Enable || items == null || items.Count == 0)
            {
                return;
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                string logRecorderName = this.Config.Name;
                foreach (var item in items)
                {
                    try
                    {
                        //输出日志
                        this.OutputLog(logRecorderName, item);
                        string body = LayoutManager.LayoutLog(item, this.Config, true);
                        sb.AppendLine(body);
                    }
                    catch (Exception exi)
                    {
                        LogSysInnerLog.OnRaiseLog(this, exi);
                    }
                }

                string subject = items[0].Logger + "日志";
                this.SendMail(this.Config.SmtpHost, this.Config.UserName, this.Config.Password, subject, false, sb.ToString(), null, this._from, this._to, this._cc, this._bcc);
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }

            //追加日志
            base.AppenderLog(items);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="smtpHost">邮件服务器主机名称</param>
        /// <param name="userName">发件人登录用户名</param>
        /// <param name="password">发件人登录密码</param>
        /// <param name="subject">主题</param>
        /// <param name="isBodyHtml">邮件格式[true:html;false:Text]</param>
        /// <param name="body">内容</param>
        /// <param name="attachmentFiles">附件文件路径集合</param>
        /// <param name="from">发送件人地址</param>
        /// <param name="to">收件人地址集合</param>
        /// <param name="cc">抄送收件人集合</param>
        /// <param name="bcc">密件抄送收件人集合</param>
        /// <param name="repeatCount">重试次数</param>
        /// <param name="repeatWaitTime">重试间隔，单位/毫秒</param>
        private void SendMail(string smtpHost, string userName, string password, string subject, bool isBodyHtml, string body, IEnumerable<string> attachmentFiles, MailAddress from, IEnumerable<MailAddress> to, IEnumerable<MailAddress> cc = null, IEnumerable<MailAddress> bcc = null, int repeatCount = 0, int repeatWaitTime = 0)
        {
            if (string.IsNullOrEmpty(smtpHost))
            {
                throw new ArgumentNullException("smtpHost");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            if (from == null)
            {
                throw new ArgumentNullException("from");
            }

            SmtpClient mailClient = new SmtpClient(smtpHost);
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = new System.Net.NetworkCredential(userName, password);
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mailMessage = new MailMessage();
            mailMessage.Priority = MailPriority.Normal;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Subject = subject;
            //内容
            mailMessage.IsBodyHtml = isBodyHtml;
            mailMessage.Body = body;

            try
            {
                mailMessage.From = from;//发件人
                this.AddMailAddress(mailMessage.To, to);//收件人集合
                this.AddMailAddress(mailMessage.CC, cc);//抄送收件人集合
                this.AddMailAddress(mailMessage.Bcc, bcc);//密件抄送收件人集合

                //添加附件
                if (attachmentFiles != null && attachmentFiles.Count() > 0)
                {
                    foreach (var attachmentFile in attachmentFiles)
                    {
                        Attachment attachment = new Attachment(attachmentFile, System.Net.Mime.MediaTypeNames.Application.Octet);
                        // Add time stamp information for the file.
                        attachment.ContentDisposition.CreationDate = System.IO.File.GetCreationTime(attachmentFile);
                        attachment.ContentDisposition.ModificationDate = System.IO.File.GetLastWriteTime(attachmentFile);
                        attachment.ContentDisposition.ReadDate = System.IO.File.GetLastAccessTime(attachmentFile);
                        // Add the file attachment to this e-mail message.
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                mailClient.Send(mailMessage);
            }
            //catch (SmtpFailedRecipientsException ex)
            //{
            //    int currentRepeatCount = 0;
            //    SmtpFailedRecipientsException smtpFailedRecipientsException = ex;
            //    while (true)
            //    {
            //        currentRepeatCount++;
            //        for (int i = 0; i < ex.InnerExceptions.Length; i++)
            //        {
            //            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
            //            if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
            //            {
            //                if (repeatWaitTime > 0)
            //                {
            //                    System.Threading.Thread.Sleep(repeatWaitTime);
            //                }

            //                try
            //                {
            //                    mailClient.Send(mailMessage);
            //                }
            //                catch (SmtpFailedRecipientsException exi)
            //                {
            //                    if (currentRepeatCount < repeatCount)
            //                    {
            //                        throw exi;
            //                    }
            //                }
            //            }
            //        }


            //    }
            //}
            finally
            {
                for (int i = 0; i < mailMessage.Attachments.Count; i++) //释放占用的资源
                {
                    mailMessage.Attachments[i].Dispose();
                }
            }
        }

        /// <summary>
        /// 将源邮件地址添加到目标邮件地址集合中
        /// </summary>
        /// <param name="targetCollection">目标邮件地址集合</param>
        /// <param name="sourceMailAddrs">源邮件地址集合</param>
        private void AddMailAddress(MailAddressCollection targetCollection, IEnumerable<MailAddress> sourceMailAddrs)
        {
            if (sourceMailAddrs == null || sourceMailAddrs.Count() == 0)
            {
                return;
            }

            foreach (var ma in sourceMailAddrs)
            {
                targetCollection.Add(ma);
            }
        }
    }
}
