namespace QuickProFixer.Models.Entities
{
    public class Contact
    {
        public int ContactId { get; set; }

        public string Address { get; set; }
        public string Town { get; set; }
        public string? Landmarks { get; set; }

        public int LgaId { get; set; }
        public string LGA { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }

        public int CountryId { get; set; }
        public string Country { get; set; }



        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
