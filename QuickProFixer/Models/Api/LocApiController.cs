using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickProFixer.Models.Entities;
using QuickProFixer.Services;
using System.Collections.Generic;

namespace QuickProFixer.Models.Api
{
    [Route("{controller}/{action}/{id?}")]
    public class LocApiController : Controller
    {
        private readonly IQuickProFixerRepository _repo;
        public LocApiController(IQuickProFixerRepository repo)
        {
            _repo = repo;
        }



        [HttpGet]
        public SelectList GetStates(string selectedCountryId)
        {

            if (!string.IsNullOrWhiteSpace(selectedCountryId) /*&& selectedCountryId.Length == 3*/)
            {             

                return  new SelectList(_repo.GetStates(selectedCountryId), nameof(State.StateId), nameof(State.StateName));
            }
            return null;                    


        }



        [HttpGet]
        public SelectList GetLgas(string selectedStateId)
        {
            if (!string.IsNullOrWhiteSpace(selectedStateId) /*&& selectedCountryId.Length == 3*/)
            {
                var lgas = new SelectList(_repo.GetLGAs(selectedStateId), nameof(LGA.LGAId), nameof(LGA.LGAName));

                return lgas;
            }
            return null;
        }
        
    }
}
