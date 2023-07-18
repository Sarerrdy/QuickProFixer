using QuickProFixer.Models.Entities;
using QuickProFixer.Models.UtilityModels;
using static QuickProFixer.ViewModels.FSRequestViewModel;

namespace QuickProFixer.ViewModels
{
    public class RequestTemplateViewModel
    {
        public RequestTemplateViewModel()
        {
            FixerMiniTemplateViewModels = new List<FixerMiniTemplateViewModel>();
            Quotes = new List<Quote>(); 
            QuoteMiniTemplateViewModels = new List<QuoteMiniTemplateViewModel>();
            sentNotifications = new List<SentNotification>();
            CreateQuoteViewModel   = new CreateQuoteViewModel();
        }
        public int RequestId { get; set; }
        public int ClienteleId { get; set; }
        public bool IsContracted { get; set; }
        public string ClientImgUrl{ get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateSummited { get; set; }
        public string Urgency { get; set; } = null!;


        public double RatingScore { get; set; }
        public double TotalRatingCount { get; set; }
        public string ServiceType { get; set; }

        public string ClienteleFirstName { get; set; } = null!;
        public string ClienteleLastName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int SentNotisficationCount { get; set; }
        public int AcceptedQuoteSentNotisficationId { get; set; }
        public Quote AcceptedQuote { get; set; }

        public int QuotesCount { get; set; }

        public string? ImgUrl_1 { get; set; }
        public string? ImgUrl_2 { get; set; }
        public string? ImgUrl_3 { get; set; }
        public string? AudioUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? PdfUrl { get; set; }
        public ResponseCode Flags { get; set; }

         public List<FixerMiniTemplateViewModel> FixerMiniTemplateViewModels { get; set; }
        public IEnumerable<Quote>? Quotes { get; set; }
        public List<SentNotification>? sentNotifications { get; set; }
        public List<QuoteMiniTemplateViewModel>? QuoteMiniTemplateViewModels { get; set; }
        public CreateQuoteViewModel CreateQuoteViewModel { get; set; }
    }
}
