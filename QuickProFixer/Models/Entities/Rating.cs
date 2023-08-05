namespace QuickProFixer.Models.Entities
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int RaterId { get; set; }
        public string RatingFor { get; set; }

        public int One { get; set; }
        public int Two { get; set; }
        public int Three { get; set; }
        public int Four { get; set; }
        public int Five { get; set; }

        public virtual User? User { get; set; } 
    }
}
