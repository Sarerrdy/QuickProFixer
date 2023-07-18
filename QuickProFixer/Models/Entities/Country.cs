﻿namespace QuickProFixer.Models.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }


        public virtual ICollection<State> States { get; set; }
    }
}
