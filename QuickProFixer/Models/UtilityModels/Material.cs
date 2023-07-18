using QuickProFixer.Models.Entities;

namespace QuickProFixer.Models.UtilityModels
{
    public class Material
    {
        public Material() { }
        public int MaterialId { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public int CostPerItem { get; set; }
        public int Cost { get; set; }

       // public int QuoteId { get; set; }
        public virtual Quote Quote { get; set; }

    }
}
