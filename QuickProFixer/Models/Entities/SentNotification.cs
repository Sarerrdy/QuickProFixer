using QuickProFixer.Models.UtilityModels;

namespace QuickProFixer.Models.Entities
{
    public class SentNotification
    {
        public SentNotification()
        {
            ResponseStatuses = new NotificationResponseStatus();
        }
        public int SentNotificationId { get; set; }     
        public int SenderId { get; set; }        
        public DateTime DateSent { get; set; }      

        public int RequestId { get; set; }
        public int ReceiverId { get; set; }
        public virtual Request  Request { get; set; }       
        // public virtual Quote Quote { get; set; }      
        public virtual NotificationResponseStatus? ResponseStatuses { get; set; }
    }
}
