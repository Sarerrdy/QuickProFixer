using QuickProFixer.Models.Entities;
using QuickProFixer.Models.UtilityModels;

namespace QuickProFixer.ViewModels
{
    public class SendRequestViewModel
    {
        public SendRequestViewModel()
        {
            this.FixerMiniTemplateViews = new List<FixerMiniTemplateViewModel>();
            //this.FixerIds = new List<NotificationResponseStatus>();
            this.BtnClasses = new List<string>();
            this.SentNotificationList = new List<SentNotification>();
        }
        public int RequestId { get; set; }
        public string RequestTitle { get; set; } = null!;

        public int ClientId { get; set; }

        public List<SentNotification> SentNotificationList { get; set; }
        //public List<NotificationResponseStatus> FixerIds { get; set; }

        public List<string> BtnClasses { get; set; } 

        public List<FixerMiniTemplateViewModel> FixerMiniTemplateViews { get; set; }
    }
}
