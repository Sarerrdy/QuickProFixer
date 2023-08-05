using QuickProFixer.Models.Entities;

namespace QuickProFixer.ViewModels
{
    public class FSFixerDetailsViewModel
    {
        public User ClienteleUser { get; set; }
        public User FixerUser { get; set; }       
        public Contact Contact { get; set; }

        public IEnumerable<User> AllUsers { get; set; }
        public Review Review { get; set; }
        public IEnumerable<Review> AllReviews { get; set; }
        public string CommentAddedNotifier { get; set; }

        public string CommentRemovedNotifier { get; set;}

        public bool IsClienteleLogin { get; set; }



        //stores comments from post or expert reviews
        public string Comment { get; set; }

        //  public User currReviewer { get; set; }




        ////// Contact Expert ///
        ///
        //Message stores text content of sents mails
        public Message Message { get; set; }



        public List<int> RatingRadios = new List<int>(5) { 1, 2, 3, 4, 5 };
        public int RatingRadio { get; set; }

        public List<Rating> StarRatings { get; set; }
        public bool IsRated { get; set; }


        public int Five { get; set; }
        public int Four { get; set; }
        public int Three { get; set; }
        public int Two { get; set; }
        public int One { get; set; }


        ///progressBar width
        public string FivePercent { get; set; }
        public string FourPercent { get; set; }
        public string ThreePercent { get; set; }
        public string TwoPercent { get; set; }
        public string OnePercent { get; set; }

        ///ProgressBar Value
        public int FivePercentValue { get; set; }
        public int FourPercentValue { get; set; }
        public int ThreePercentValue { get; set; }
        public int TwoPercentValue { get; set; }
        public int OnePercentValue { get; set; }
    }
}
