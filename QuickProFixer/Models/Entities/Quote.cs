using QuickProFixer.Models.UtilityModels;

namespace QuickProFixer.Models.Entities
{
    public class Quote
    {
        public Quote() {
            Materials = new List<Material>();
        }
        public int QuoteId { get; set; }
        public int RequestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateofQuote { get; set; }
        public DateTime ProposedStartDate { get; set; }
        public int EstimatedFixDuration { get; set; }
        public string StartUrgency { get; set; } = "yes";
        public int LabourCost { get; set; }
        public int? LabourRate { get; set; }
        public int? EstimatedCostOfMaterials { get; set; }
        public int? EstimatedOverallCost { get; set; }
        public bool? IsQuoteAccepted { get; set; }


        ///Additional Responses Resources


        public string? ImgUrl_1 { get; set; }
        public string? ImgUrl_2 { get; set; }
        public string? ImgUrl_3 { get; set; }
        public string? VideoUrl { get; set; }
        public string? PdfUrl { get; set; }
        public string? AudioUrl { get; set; }      

        public int ClienteleId { get; set; }
        public int FixerId { get; set; }
        public  string FixerName { get; set; }  

        public virtual List<Material>? Materials { get; set; }
        public virtual Request? Request { get; set; }
        public virtual SentNotification? SentNotification { get; set; }

    }

}
