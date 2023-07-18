using QuickProFixer.Models.Entities;
using static QuickProFixer.ViewModels.FSRequestViewModel;

namespace QuickProFixer.ViewModels
{
    public class QuoteMiniTemplateViewModel
    {
        public QuoteMiniTemplateViewModel()
        {           
        }
        public int QuoteId { get; set; }
        public int RequestId { get; set; }
        public int ClienteleId { get; set; }
        public int FixerId { get; set; }
        public ResponseCode Flags { get; set; }

        public string Title { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public DateTime DateSubmitted { get; set; }
        public bool? IsQuoteAccepted { get; set; }


        public double RatingScore { get; set; }
        public double TotalRatingCount { get; set; }
        public int? EstimatedOverallCost { get; set; }
        public int EstimatedLabourCost { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public int EstimatedFixDuration { get; set; }

        public string FixerName { get; set; } = null!;
        public int sentNotificationId { get; set; }
    }
}
