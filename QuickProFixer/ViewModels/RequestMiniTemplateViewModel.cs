using static QuickProFixer.ViewModels.FSRequestViewModel;

namespace QuickProFixer.ViewModels
{
    public class RequestMiniTemplateViewModel
    {
        public int RequestId { get; set; }
        public int QuoteId { get; set; }
        public int ClienteleId { get; set; }
        public int FixerId { get; set; }
        public string ClientName { get; set; }
        public string FixerName { get; set; }
        

        public string ServiceType { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public DateTime DateSummited { get; set; }
        public string Urgency { get; set; } = null!;
        
        public int SentNotisficationCount { get; set; }
        public int QuoteCount { get; set; }
        public int RejectedRequestCount { get; set; }
        public int RequestNoResponseYetCount { get; set; }
        


        public int SentNotificationId { get; set; }
       
    }
}
