using QuickProFixer.Models.Entities;

namespace QuickProFixer.ViewModels
{
    public class FixerMiniTemplateViewModel
    {
        public FixerMiniTemplateViewModel()
        {
            this.sentNotification = new SentNotification();
        }
        public string Title { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? FullName { get; set; }
        public string ShortDescription { get; set; } = null!;

        public double RatingScore { get; set; }
        public double TotalRatingCount { get; set; }

        public int NoOfCompletedFixes { get; set; }
        public bool IsNINVerified { get; set; }
        public bool IsCompanyRegistrationVerified { get; set; }

        public string Location { get; set; } = null!;
        public string FixerImgUrl { get; set; } = null!;
        public int FixerId { get; set; }

        public string BtnClassName { get; set; }

        public SentNotification sentNotification { get; set; }
        
        public string FeedBackMsg { get; set; }

    }
}
