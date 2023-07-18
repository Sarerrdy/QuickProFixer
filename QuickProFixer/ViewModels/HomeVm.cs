using Microsoft.AspNetCore.Mvc.Rendering;
using QuickProFixer.Models;
using QuickProFixer.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace QuickProFixer.ViewModels
{
    public class HomeIndexVm
    {
        public HomeIndexVm()
        {
            Request = new Request();
        }

        //[Required]
        //public int ServiceTypeId { get; set; }
        //[Required]
        //public string ServiceType { get; set; }
        public SelectList SelectCategories { get; set; }

        public string ReturnUrl { get; set; }

        ///// <summary>
        ///// Address Select List of country, state and Lga
        ///// </summary>

        //[Required]
        //public string CountryName { get; set; }
        //[Required]
        //public int CountryId { get; set; }
        public SelectList SelectCountries { get; set; }

        //[Required]
        //public int StateId { get; set; }
        //[Required]
        //public string StateName{ get; set; }
        public SelectList SelectStates { get; set; }

        //[Required]
        //public int LGAId { get; set; }
        //[Required]
        //public string LGAName{ get; set; }
        public SelectList SelectLGAs { get; set; }


        //[Required]
        //public string Title { get; set; }
        //[Required]
        //public string Description { get; set; }

       
        public DateTime PreferredStartDate { get; set; } = DateTime.Now.Date;
        //public DateTime DateOfRequest { get; set; }
        public IFormFile ImgUrl_1 { get; set; }
        public IFormFile ImgUrl_2 { get; set; }
        public IFormFile ImgUrl_3 { get; set; }
        public IFormFile VideoUrl { get; set; }
        public IFormFile AudioUrl { get; set; }
        public IFormFile PdfUrl { get; set; }

        //public List<string>? ResourcesUrls { get; set; }  ///Collection of urls for imgs, audios, video,pdf and .zip files

        //[Required]
        //public string Town { get; set; }
        //[Required]
        //public string Address { get; set; }
        //public string LandMark { get; set; }
        //public int LocationPicker { get; set; } // Device location, Map, registered Contact, or Entered contact
       
        public Request Request { get; set; }



        public string Feedback { get; set; }
        public bool IsPageReloading { get; set; }
    }
}
