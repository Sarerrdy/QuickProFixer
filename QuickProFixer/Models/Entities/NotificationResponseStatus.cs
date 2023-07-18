using QuickProFixer.Models.UtilityModels;

namespace QuickProFixer.Models.Entities
{
    public class NotificationResponseStatus
    {
        public NotificationResponseStatus()
        {
           // SentNotification = new SentNotification();
        }
        public int NotificationResponseStatusId { get; set; }
        public int RecieverId { get; set; }       
        public DateTime Date { get; set; }

        public int FixerStatuseCode { get; set; } // 1==QuoteSubmitted; 2==RequestRejected; 3==RequestNoResponseYet/Default; 4==FixCompleted  
        public int ClientStatuseCode { get; set; } //0==Sent/Default; 1==QuoteAccepted; 2== QuoteRejected; 3==QuoteNoResponseYet; 4==FixCompleted 
        public int SentNotificationId { get; set; }
       // public virtual SentNotification SentNotification { get; set; }
    }
}