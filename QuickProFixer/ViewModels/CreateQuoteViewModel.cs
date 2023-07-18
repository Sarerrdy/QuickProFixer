
using QuickProFixer.Models.UtilityModels;

namespace QuickProFixer.ViewModels
{   
    public class CreateQuoteViewModel
    {
        public CreateQuoteViewModel()
        {
            MaterialsList = new List<Material>();
        }
        public int QuoteId { get; set; }
        public int FixerId { get; set; }

        public string FixerFirstNames { get; set; }
        public string FixerLastName { get; set; }
        public string FixerAddress { get; set; }
        public string QuoteTitle { get; set; } = null!;
        public string QuoteDescription { get; set; } = null!;
        public DateTime QuoteDateofResponse { get; set; }
        public int LabourRate { get; set; }
        public int QuoteEstimatedLabourCost { get; set; }
        public int? QuoteEstimatedCostOfMaterials { get; set; }
        public int? QuoteEstimatedOverallCost { get; set; }
        public DateTime QuoteProposedStartDate { get; set; } = DateTime.Now.Date;
        public int QuoteEstimatedFixDuration { get; set; }

        //public double QuoteRatingScore { get; set; }
        //public double QuoteTotalRatingCount { get; set; }


        ///Additional Responses Resources
        public IFormFile? QuoteVideoUrl { get; set; }
        public IFormFile? QuotePdfUrl { get; set; }
        public IFormFile? QuoteImgUrl_1 { get; set; }
        public IFormFile? QuoteImgUrl_2 { get; set; }
        public IFormFile? QuoteImgUrl_3 { get; set; }
        public IFormFile? QuoteAudioUrl { get; set; }

        ///For QuotePreview Page
        public string? QuotePrevImgUrl_1 { get; set; }
        public string? QuotePrevImgUrl_2 { get; set; }
        public string? QuotePrevImgUrl_3 { get; set; }
        public string? QuotePrevAudioUrl { get; set; }
        public string? QuotePrevVideoUrl { get; set; }
        public string? QuotePrevPdfUrl { get; set; }

        public string? Materials { get; set; } ///To hold Serialized strings of items needed for the fix
        public List<Material>? MaterialsList { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemCostPerItem { get; set; }
        public int ItemCost { get; set; }
       

        //public int SerialNo { get; set; } = 1;


        ///Request section
        public int RequestId { get; set; }
        public int ClienteleId { get; set; }
        public string ClientImgUrl { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateSummited { get; set; }
        public string Urgency { get; set; } = "Urgent";


        public double RatingScore { get; set; }
        public double TotalRatingCount { get; set; }
        public string ServiceType { get; set; }

        public string ClienteleFirstName { get; set; } = null!;
        public string ClienteleLastName { get; set; } = null!;
        public string ClienteleMiddleName { get; set; } = null!;
        public bool IsCurrentUserClientele { get; set; }

        public string AddressofFix { get; set; } = null!;

        public int SentNotisficationCount { get; set; }
        public int ResponsesCount { get; set; }

        public string? ImgUrl_1 { get; set; }
        public string? ImgUrl_2 { get; set; }
        public string? ImgUrl_3 { get; set; }
        public string? AudioUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? PdfUrl { get; set; }


        public string Feedback { get; set; } ///feedback after post
        public bool IsPageReloading { get; set; }
    }
}

