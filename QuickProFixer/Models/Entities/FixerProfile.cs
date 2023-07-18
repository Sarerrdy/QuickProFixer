namespace QuickProFixer.Models.Entities
{
    public class FixerProfile
    {
        public int FixerProfileId { get; set; }

        //public bool IsEmployee { get; set; }      
       // public bool IsEmployeeStatusVerified { get; set; }
        public bool IsBusinessAccountManager { get; set; }
        public bool IsBusinessOwner { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
        //public string BusinessName { get; set; }
      //  public bool IsCompanyRegistrationVerified { get; set; }

        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public string CACNumber { get; set; }
        public string CACUrl { get; set; }
        public DateTime CACExpiryDate { get; set; }


        //public string PrimarySkills { get; set; }
        public string? OtherSkills { get; set; }
        public int NoOfCompletedFixes { get; set; }
        public int StartingPrice { get; set; }
        public int PriceRateTypeId { get; set; }
        public string PriceRateName { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

    }
}
