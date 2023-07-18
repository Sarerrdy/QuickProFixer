using System.ComponentModel.DataAnnotations;

namespace QuickProFixer.Models.Entities
{
    public class Request
    {
        public Request()
        {
            Quotes = new List<Quote>(); 
            SentNotifications  = new List<SentNotification>();
        }
        public int RequestId { get; set; }
        public int ClienteleId { get; set; }
        public string ClientName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public bool IsContracted { get; set; }
        public bool IsArchive { get; set; }
        public int IsContractedTo { get; set; }
        public bool IsCompleted { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }
        [Required]
        public string ServiceType { get; set; }  // category of service
       
        public DateTime DateOfRequest { get; set; } = DateTime.Now;

        public string PreferredStartDate { get; set; }
        public string Location { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public string CountryName { get; set; }
        [Required]
        public int StateId { get; set; }

        [Required]
        public string StateName { get; set; }
        [Required]
        public int LGAId { get; set; }

        [Required]
        public string LGAName { get; set; }

        [Required]
        public string Town { get; set; }
        public string? Landmarks { get; set; }
        [Required]
        public string Address { get; set; }



        public string? ImgUrl_1 { get; set; }
        public string? ImgUrl_2 { get; set; }
        public string? ImgUrl_3 { get; set; }
        public string? AudioUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? PdfUrl { get; set; }

        //  public virtual List<string>? ImgUrl { get; set; }

        public virtual ICollection<Quote>? Quotes { get; set; }
        public virtual IEnumerable<SentNotification>  SentNotifications { get; set; }

    }
}
