using QuickProFixer.Models.UtilityModels;

namespace QuickProFixer.ViewModels
{
    public class QuoteDetailsTemplateViewModel
    {
        public QuoteDetailsTemplateViewModel()
        {
            Materials = new List<Material>();
        }
        public int ResponseId { get; set; }
        public int RequestId { get; set; }
        public int ClienteleId { get; set; }
        public int FixerId { get; set; }
        public string FixerImgUrl { get; set; }


        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateofResponse { get; set; }


        public double RatingScore { get; set; }
        public double TotalRatingCount { get; set; }     
        public int EstimatedLabourCost { get; set; }
        public int? EstimatedCostOfMaterials { get; set; }
        public int? EstimatedOverallCost { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public string EstimatedFixDuration { get; set; }

        public string FixerFirstName { get; set; } = null!;
        public string FixerLastName { get; set; } = null!;
        public string FixerMiddleName { get; set; } = null!;

        public string Address { get; set; } = null!;

        ///Additional Responses Resources

        public string? VideoUrl { get; set; }
        public string? PdfUrl { get; set; }



        public List<ImgUrl>? ImgUrls { get; set; } ///Max of 3 resource images 
        public List<Material>? Materials { get; set; }
        public int SerialNo { get; set; } = 1;
    }
}
