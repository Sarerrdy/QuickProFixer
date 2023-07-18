namespace QuickProFixer.Models.Entities
{
    public class Review
    {

        public int ReviewId { get; set; }
        public int RevieweeID { get; set; }
        public int ReviewerID { get; set; }

        //[Required]
        //[StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 20)]       
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
