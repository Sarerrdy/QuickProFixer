using Microsoft.AspNetCore.Identity;

namespace QuickProFixer.Models.Entities
{
    public class User : IdentityUser<int>
    {
        public int UserId { get; set; }

        public string Title { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NIN { get; set; }
        public string NINUrl { get; set; } = null!;
        public string ProfileImgUrl { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public bool IsFixer { get; set; }
        public bool IsUserCompanyRep { get; set; }
        public string? OrganisationName { get; set; }
        public bool IsNINVerified { get; set; }
        public bool IsCompanyRegistrationVerified { get; set; }
        public bool IsEmployeeStatusVerified { get; set; }

        /// Ratings
        public double RatingScore { get; set; }
        public double TotalRatingCount { get; set; }
        public int ReviewsCount { get; set; }

        //Review Properties
        public virtual FixerProfile? Profile { get; set; }
        public virtual ICollection<Request>? Requests { get; set; }
        public virtual ICollection<Quote>? Responses { get; set; }
        public virtual Contact? Contact { get; set; }
        public virtual Rating? StarRating { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
