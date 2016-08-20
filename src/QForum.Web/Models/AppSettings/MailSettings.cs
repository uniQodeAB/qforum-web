namespace QForum.Web.Models.AppSettings
{
    public class MailSettings
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderPassword { get; set; }
        public string DefaultRecipientEmail { get; set; }
        public string DefaultRecipientName { get; set; }
    }
}