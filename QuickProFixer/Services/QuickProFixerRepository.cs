using FluentAssertions.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuickProFixer.Models.Context;
using QuickProFixer.Models.Entities;
using QuickProFixer.Models.UtilityModels;
using QuickProFixer.ViewModels;

namespace QuickProFixer.Services
{
    public class QuickProFixerRepository : IQuickProFixerRepository
    {
        private readonly QuickProFixerContext _ctx;

        public QuickProFixerRepository(QuickProFixerContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ServiceType> GetCategories(int? CategoryId)
        {
            return _ctx.Category
                .ToList();                
        }

        public IEnumerable<PriceRateType> GetPriceRate(int? PriceRateTypeId)
        {
            return _ctx.priceRateTypes
                .ToList(); 
        }

        public void PostRequest(Request request)
        {
            _ctx.Requests.Add(request);
            _ctx.SaveChanges();
        }


        public IEnumerable<Country> GetCountryById(int CountryId)
        {
            return _ctx.Countries
                .Where(s=>s.CountryId == CountryId)
           .Include(s => s.States)
           .ThenInclude(s => s.LGAs)
           .ToList();
        }

        public IEnumerable<Country> GetCountries()
        {
            return _ctx.Countries                
                .ToList();
        }


        public IEnumerable<State> GetStates(string countryId)
        {
            if (!string.IsNullOrWhiteSpace(countryId))
            {
                IEnumerable<State> states = _ctx.States.AsNoTracking()
                    .OrderBy(n => n.StateName)
                    .Where(n => n.CountryId == int.Parse(countryId))
                    .Select(n =>
                       new State
                       {
                           StateId = n.StateId,
                           StateName = n.StateName
                       }).ToList();
               
                return states;
                //}
            }
            return null;
        }


        //public IEnumerable<SelectListItem> GetStates(string countryId)
        //{
        //    if (!String.IsNullOrWhiteSpace(countryId))
        //    {                
        //            IEnumerable<SelectListItem> states = _ctx.States.AsNoTracking()
        //                .OrderBy(n => n.StateName)
        //                .Where(n => n.CountryId == int.Parse(countryId))
        //                .Select(n =>
        //                   new SelectListItem
        //                   {
        //                       Value = n.StateId.ToString(),
        //                       Text = n.StateName
        //                   }).ToList();
        //        var req = new SelectList(states, "Value", "Text");
        //        return req;
        //        //}
        //    }
        //    return null;
        //}

      
        public IEnumerable<LGA> GetLGAs(string stateId)
        {
            if (!string.IsNullOrWhiteSpace(stateId))
            {
                //using (var context = new ApplicationDbContext())
                //{
                IEnumerable<LGA> lgas = _ctx.LGAs.AsNoTracking()
                    .OrderBy(n => n.LGAName)
                    .Where(n => n.StateId == int.Parse(stateId))
                    .Select(n =>
                       new LGA
                       {
                           LGAId = n.LGAId,
                           LGAName = n.LGAName
                       }).ToList();
                return lgas;
                //}
            }
            return null;
        }

        public SendRequestViewModel GetFixersFromClosedProximity( string lgaName, int serviceId, int requestId, string requestTitle, int senderId)
        {
            var FixersUser = _ctx.Users
                    .Where(p => p.IsFixer == true && p.Contact.LGA==lgaName && p.Profile.ServiceTypeId == serviceId)
                    .Include(p => p.Profile)
                    .Include(r => r.Contact)
                    .ToList();


            var fsVmodel = new SendRequestViewModel();
            //var sentNotifications = new List<SentNotification>();

            foreach (var user in FixersUser)
            {
                var sentNotification = new SentNotification()
                {
                    RequestId = requestId,
                    DateSent = DateTime.Now,
                    SenderId = senderId,
                    ReceiverId = user.Id,
                    ResponseStatuses = new NotificationResponseStatus()
                    {
                        RecieverId = user.Id,
                        Date = DateTime.Now.Date,
                        FixerStatuseCode = 3, // set to default/RequestNoResponseYet flag of 3; 
                        ClientStatuseCode = 1 // set to default/request submitted flag of 1;          
                    }
                };
                //sentNotifications.Add(sentNotification);

                FixerMiniTemplateViewModel thumb = new FixerMiniTemplateViewModel
                {
                    FixerId = user.Id,
                    BtnClassName = "BtnClassName" + user.Id,
                    FixerImgUrl = "/usersprofile/" + user.ProfileImgUrl,
                    Title = user.Title,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    FullName = user.Title + " " + user.FirstName + " " + user.LastName,
                    ShortDescription = user.Profile.ShortDescription,

                    RatingScore = user.RatingScore,
                    TotalRatingCount = user.TotalRatingCount,
                    NoOfCompletedFixes = user.Profile.NoOfCompletedFixes,

                    IsCompanyRegistrationVerified = user.IsCompanyRegistrationVerified,
                    IsNINVerified = user.IsNINVerified,
                    Location = user.Contact.Address + ", " + user.Contact.Town + ", " /*+ user.Contact.Landmarks + " "*/ + user.Contact.LGA + ", " + user.Contact.State + ", " + user.Contact.Country,
                    sentNotification = sentNotification
                };

                fsVmodel.FixerMiniTemplateViews.Add(thumb);
                fsVmodel.SentNotificationList.Add(sentNotification);
                
                fsVmodel.BtnClasses.Add("BtnClassName" + user.Id);
            }
           
            fsVmodel.RequestId = requestId;
            fsVmodel.ClientId = senderId;



            return fsVmodel;

        }


        public IEnumerable<User> GetUser(int Id)
        {
            return _ctx.Users
                .Where(p => p.Id == Id)
                .Include(p => p.Profile)
                .Include(p => p.Responses)
                .Include(p => p.StarRating)
                .Include(p => p.Reviews)
                .ToList();
        }

        public string UpdateRequest(Request request)
        {
            try
            {
                 _ctx.Requests.Update(request);
                int updateCount =  _ctx.SaveChanges();

                /// check whether saveChanges was successful by returning int higer than zero
                /// 
                if (updateCount > 0)                
                    return "1"; ///sucessful update flag
                else
                {
                    return "Sending Request Failed.."; ///failed to update flag
                }
            }
            catch(Exception ex)
            {
                return "Sending Request Failed.." + " " + ex.Message;
            }            
            
        }

        public void DeleteRequest(Request request)
        {
            _ctx.Requests.Remove(request);
            _ctx.SaveChanges();
        }


        public string CreateSingleSentNotification(SentNotification sentNotification)
        {
            try
            {
                _ctx.sentNotifications.Add(sentNotification);
                int updateCount = _ctx.SaveChanges();

                /// check whether saveChanges was successful by returning int higer than zero
                /// 
                if (updateCount > 0)
                    return "1"; ///sucessful update flag
                else
                {
                    return "Sending Request Failed.."; ///failed to update flag
                }
            }
            catch (Exception ex)
            {
                return "Sending Request Failed.." + " " + ex.Message;
            }
        }


        public void DeleteSingleSentNotification(SentNotification sentNotification)
        {
            _ctx.sentNotifications.Remove(sentNotification);
            _ctx.SaveChanges();
        }


        public string CreateSentNotifications(List<SentNotification> sentNotifications)
        {
            try
            {
                _ctx.sentNotifications.AddRange(sentNotifications);
                int updateCount = _ctx.SaveChanges();

                /// check whether saveChanges was successful by returning int higer than zero
                /// 
                if (updateCount > 0)
                    return "1"; ///sucessful update flag
                else
                {
                    return "Sending Request Failed.."; ///failed to update flag
                }
            }
            catch (Exception ex)
            {
                return "Sending Request Failed.." + " " + ex.Message;
            }
        }

        public void DeleteSentNotifications(List<SentNotification> sentNotifications)
        {
            _ctx.sentNotifications.RemoveRange(sentNotifications);
            _ctx.SaveChanges();
        }


        ////fetch all Requests by client that have not receive a quotes to populate ClienteleSpace index page
        public CSActiveRequestViewModel GetRequestByClientId(int clienteleId)
        {
            //var request = _ctx.sentNotifications
            //  .Where(p => p.SenderId == clienteleId && p.ResponseStatuses.FixerStatuseCode == 3 && p.Request.IsContracted == false)//responseCode of No Response Yet
            //  //.Include(q => q.Request.Quotes)
            //  .Select(r => r.Request).Distinct()
            //  .ToList();

            var request = _ctx.Requests
                .Where(p => p.ClienteleId == clienteleId && p.IsContracted == false && p.IsArchive == false && p.IsCompleted==false).Distinct()
                .Include(p => p.SentNotifications)
                .Include(q => q.Quotes)
                .ToList();

            var client = _ctx.Users
              .FirstOrDefault(p => p.Id == clienteleId);

            CSActiveRequestViewModel CSViewModel = FetchActiveRequestViewModel(request);
            CSViewModel.ClienteleName = client.FirstName + " " + client.LastName;

            return CSViewModel;

        }


        ////fetch all Requests by client who have accepted submitted quotes to populate ClienteleSpace ActiveRequest page
        public CSActiveRequestViewModel GetActiveRequestByClientId(int clienteleId)
        {            
            //var request = _ctx.sentNotifications
            //  .Where(p => p.SenderId == clienteleId && p.ResponseStatuses.FixerStatuseCode == 1 && p.ResponseStatuses.ClientStatuseCode == 5)//FixerStatusCode==1 submitted quote; clientstatuscode == 5 AcceptedQuote
            //  .Include(q => q.Request.Quotes.Where(s => s.ClienteleId == clienteleId))
            //  .Select(r => r.Request)//.Distinct()
            //  .ToList();


            var request = _ctx.Requests
               .Where(p => p.ClienteleId == clienteleId && p.IsContracted == true && p.IsArchive == false && p.IsCompleted==false).Distinct()
               .Include(p => p.SentNotifications)
               .Include(q => q.Quotes)
               .ToList();


            var client = _ctx.Users
              .FirstOrDefault(p => p.Id == clienteleId);

            CSActiveRequestViewModel CSViewModel = FetchActiveRequestViewModel(request);
            CSViewModel.ClienteleName = client.FirstName + " " + client.LastName;
            CSViewModel.Flags = FSRequestViewModel.ResponseCode.DefaultValue;

            return CSViewModel;
        }

        ////fetch all Requests by client that have a submitted quotes to populate ClienteleSpace ReceivedQuotes page
        public CSActiveRequestViewModel GetCSReceivedQuotesByClienteleId(int clienteleId)
        {
            var request = _ctx.sentNotifications
             .Where(p => p.SenderId == clienteleId && p.ResponseStatuses.FixerStatuseCode == 1 && p.ResponseStatuses.ClientStatuseCode == 1)//Quotes submitted but awaiting clientele acceptance to make them active
             .Include(q => q.Request.Quotes.Where(s => s.ClienteleId == clienteleId))
             .Select(r => r.Request).Distinct()
             .ToList();            


            var client = _ctx.Users
              .FirstOrDefault(p => p.Id == clienteleId);

            CSActiveRequestViewModel CSViewModel = FetchActiveRequestViewModel(request);
            CSViewModel.ClienteleName = client.FirstName + " " + client.LastName;

            return CSViewModel;

        }


        ////Get all active Requests sent to a fixer to populate FixerSpace index page
        public CSActiveRequestViewModel GetRequestByFixerId(int FixerId)
        {
            var request = _ctx.sentNotifications                
                .Where(p => p.ResponseStatuses.RecieverId == FixerId && p.ResponseStatuses.FixerStatuseCode == 3)//responseCode of No Response Yet
                .Include(q=>q.Request.Quotes)
                .Select(r => r.Request)
                .ToList();

            CSActiveRequestViewModel CSViewModel = FetchActiveRequestViewModel(request);

            return CSViewModel;
        }


        ////populate the createQuote page for a request
        public CreateQuoteViewModel CreateQuoteViewModel(int RequestId, int fixerId)
        {
            var req = _ctx.Requests
               .Where(p => p.RequestId == RequestId).FirstOrDefault();               
               //.ToList();

            CreateQuoteViewModel CreateQuoteVM = new CreateQuoteViewModel();
            //var req = request.FirstOrDefault();

            var client = _ctx.Users
               .FirstOrDefault(p => p.Id == req.ClienteleId);

            var fixer = _ctx.Users
              .FirstOrDefault(p => p.Id == fixerId);

            CreateQuoteVM.FixerId = fixerId;
            CreateQuoteVM.RequestId = req.RequestId;
            CreateQuoteVM.ClienteleId = req.ClienteleId;
            CreateQuoteVM.ClientImgUrl = client.ProfileImgUrl;

            CreateQuoteVM.ClienteleFirstName = client.FirstName;
            CreateQuoteVM.ClienteleLastName = client.LastName;
            CreateQuoteVM.FixerFirstNames = fixer.FirstName;
            CreateQuoteVM.FixerLastName = fixer.LastName;

            CreateQuoteVM.Title = req.Title;
            CreateQuoteVM.Description = req.Description;
            CreateQuoteVM.ServiceType = req.ServiceType;
            CreateQuoteVM.DateSummited = req.DateOfRequest;
            CreateQuoteVM.Urgency = req.PreferredStartDate;
            CreateQuoteVM.AddressofFix = req.Location;
            CreateQuoteVM.ImgUrl_1 = req.ImgUrl_1;
            CreateQuoteVM.ImgUrl_2 = req.ImgUrl_2;
            CreateQuoteVM.ImgUrl_3 = req.ImgUrl_3;
            CreateQuoteVM.AudioUrl = req.AudioUrl;
            CreateQuoteVM.VideoUrl = req.VideoUrl;
            CreateQuoteVM.PdfUrl = req.PdfUrl;

            return CreateQuoteVM;
        }

        ////populate the createQuote page for a request
        public CreateQuoteViewModel CreateQuotePreview(int QuoteId)
        {
            var quote = _ctx.Quotes
                .Where(p => p.QuoteId == QuoteId)
                .Include(q => q.Materials)
                .Include(q => q.Request).FirstOrDefault();


            CreateQuoteViewModel CreateQuoteVM = new CreateQuoteViewModel();          

            var client = _ctx.Users
               .FirstOrDefault(p => p.Id == quote.ClienteleId);

            var fixer = _ctx.Users
             .FirstOrDefault(p => p.Id == quote.FixerId);

            ///Request Section
            CreateQuoteVM.RequestId = quote.RequestId;
            CreateQuoteVM.ClienteleId = quote.ClienteleId;
            CreateQuoteVM.Title = quote.Title;                   

            CreateQuoteVM.ClientImgUrl = client.ProfileImgUrl;
            CreateQuoteVM.ClienteleFirstName = client.FirstName;
            CreateQuoteVM.ClienteleLastName = client.LastName;
            CreateQuoteVM.FixerFirstNames = fixer.FirstName;
            CreateQuoteVM.FixerLastName = fixer.LastName;
            CreateQuoteVM.FixerId = fixer.Id;
            CreateQuoteVM.FixerAddress = fixer.Contact.Address + ", " + fixer.Contact.Town + ", " + fixer.Contact.LGA + ", " + fixer.Contact.State + " " + fixer.Contact.Country;
           
            CreateQuoteVM.Description = quote.Request.Description;            
            CreateQuoteVM.ServiceType = quote.Request.ServiceType;
            CreateQuoteVM.DateSummited = quote.Request.DateOfRequest;
            CreateQuoteVM.Urgency = quote.StartUrgency;
            CreateQuoteVM.AddressofFix = quote.Request.Location;
            CreateQuoteVM.ImgUrl_1 = quote.Request.ImgUrl_1;
            CreateQuoteVM.ImgUrl_2 = quote.Request.ImgUrl_2;
            CreateQuoteVM.ImgUrl_3 = quote.Request.ImgUrl_3;
            CreateQuoteVM.AudioUrl = quote.Request.AudioUrl;
            CreateQuoteVM.VideoUrl = quote.Request.VideoUrl;
            CreateQuoteVM.PdfUrl = quote.Request.PdfUrl;

            ///Quote Section
            CreateQuoteVM.QuotePrevAudioUrl = quote.AudioUrl;
            CreateQuoteVM.QuotePrevVideoUrl = quote.VideoUrl;
            CreateQuoteVM.QuotePrevPdfUrl = quote.PdfUrl;
            CreateQuoteVM.QuotePrevImgUrl_1 = quote.ImgUrl_1;
            CreateQuoteVM.QuotePrevImgUrl_2 = quote.ImgUrl_2;  
            CreateQuoteVM.QuotePrevImgUrl_3 = quote.ImgUrl_3;
            CreateQuoteVM.QuoteDescription = quote.Description;
            CreateQuoteVM.QuoteDateofResponse = quote.DateofQuote;
            CreateQuoteVM.QuoteEstimatedCostOfMaterials = quote.EstimatedCostOfMaterials;
            CreateQuoteVM.QuoteEstimatedFixDuration = quote.EstimatedFixDuration;
            CreateQuoteVM.LabourRate = quote.LabourRate ??= 0;
            CreateQuoteVM.QuoteEstimatedLabourCost = quote.LabourCost;
            CreateQuoteVM.QuoteEstimatedOverallCost = quote.EstimatedOverallCost;
            CreateQuoteVM.QuoteProposedStartDate = quote.ProposedStartDate;
            CreateQuoteVM.MaterialsList = quote.Materials;

            return CreateQuoteVM;
        }

        ////Get all details of a Request to populate RequestDetails page
        public RequestTemplateViewModel GetRequestById(int RequestId)
        {
            var req = _ctx.Requests
               .Where(p => p.RequestId == RequestId)
               .Include(r => r.Quotes)/*.Where(s => s.RequestId == RequestId))*/
               .Include(q => q.SentNotifications)               
               .FirstOrDefault();


            RequestTemplateViewModel RequestTempVM = new RequestTemplateViewModel();
           // var req = request.FirstOrDefault();

            var client = _ctx.Users
               .Where(p => p.Id == req.ClienteleId).FirstOrDefault();

            RequestTempVM.RequestId = req.RequestId;
            RequestTempVM.ClienteleId = req.ClienteleId;
            RequestTempVM.ClienteleFirstName = client.FirstName;
            RequestTempVM.ClientImgUrl = client.ProfileImgUrl;
            RequestTempVM.Title = req.Title;
            RequestTempVM.Description = req.Description;
            RequestTempVM.ClienteleFirstName = client.FirstName;
            RequestTempVM.ClienteleLastName = client.LastName;
            RequestTempVM.ServiceType = req.ServiceType;
            RequestTempVM.DateSummited = req.DateOfRequest;
            RequestTempVM.Urgency = req.PreferredStartDate;
            RequestTempVM.Address = req.Location;
            RequestTempVM.ImgUrl_1 = req.ImgUrl_1;
            RequestTempVM.ImgUrl_2 = req.ImgUrl_2;
            RequestTempVM.ImgUrl_3 = req.ImgUrl_3;
            RequestTempVM.AudioUrl = req.AudioUrl;
            RequestTempVM.VideoUrl = req.VideoUrl;
            RequestTempVM.PdfUrl = req.PdfUrl;
            RequestTempVM.Quotes = req.Quotes;
            RequestTempVM.QuotesCount = req.Quotes.Count;
            RequestTempVM.sentNotifications = req.SentNotifications.Where(t => t.ResponseStatuses.FixerStatuseCode == 3).ToList();
            RequestTempVM.SentNotisficationCount = req.SentNotifications.Count();
            RequestTempVM.IsContracted = req.IsContracted;

           
            if (req.Quotes.Count > 0)
            {
                foreach (var quote in req.Quotes)
                {
                    QuoteMiniTemplateViewModel quoteMiniTemplateViewModel = new QuoteMiniTemplateViewModel();

                    quoteMiniTemplateViewModel.QuoteId = quote.QuoteId;
                    quoteMiniTemplateViewModel.RequestId = quote.RequestId;
                    quoteMiniTemplateViewModel.ClienteleId = quote.ClienteleId;
                    quoteMiniTemplateViewModel.FixerId = quote.FixerId;
                    quoteMiniTemplateViewModel.Title = quote.Title;
                    quoteMiniTemplateViewModel.ShortDescription = quote.Description;
                    quoteMiniTemplateViewModel.EstimatedStartDate = quote.ProposedStartDate.Date;
                    quoteMiniTemplateViewModel.EstimatedOverallCost = quote.EstimatedOverallCost;
                    quoteMiniTemplateViewModel.EstimatedFixDuration = quote.EstimatedFixDuration;
                    quoteMiniTemplateViewModel.FixerName = quote.FixerName;
                    quoteMiniTemplateViewModel.EstimatedLabourCost = quote.LabourCost;
                    quoteMiniTemplateViewModel.DateSubmitted = quote.DateofQuote.Date;
                    quoteMiniTemplateViewModel.sentNotificationId = quote.SentNotification.SentNotificationId;
                    RequestTempVM.QuoteMiniTemplateViewModels.Add(quoteMiniTemplateViewModel);
                }
            }


            if (req.SentNotifications.Count() > 0)
            {
                foreach (var notification in req.SentNotifications.Where(t => t.ResponseStatuses.FixerStatuseCode == 3))
                {
                    var user = _ctx.Users
                   .Where(p => p.IsFixer == true && p.Id == notification.ReceiverId)
                   .Include(p => p.Profile)
                   .Include(r => r.Contact)
                   .SingleOrDefault();


                    FixerMiniTemplateViewModel thumb = new FixerMiniTemplateViewModel
                    {
                        FixerId = user.Id,
                        BtnClassName = "BtnClassName" + user.Id,
                        FixerImgUrl = "/UsersProfile/" + user.ProfileImgUrl,
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName,
                        FullName = user.Title + " " + user.FirstName + " " + user.LastName,
                        ShortDescription = user.Profile.ShortDescription,

                        RatingScore = user.RatingScore,
                        TotalRatingCount = user.TotalRatingCount,
                        NoOfCompletedFixes = user.Profile.NoOfCompletedFixes,

                        IsCompanyRegistrationVerified = user.IsCompanyRegistrationVerified,
                        IsNINVerified = user.IsNINVerified,
                        Location = user.Contact.Address + ", " + user.Contact.Town + ", " /*+ user.Contact.Landmarks + " "*/ + user.Contact.LGA + ", " + user.Contact.State + ", " + user.Contact.Country,
                        sentNotification = notification
                    };
                    RequestTempVM.FixerMiniTemplateViewModels.Add(thumb);

                }
            }



            return RequestTempVM;
        }

        public RequestTemplateViewModel GetActiveRequestDetailsById(int RequestId)
        {
            var req = _ctx.Requests
               .Where(p => p.RequestId == RequestId)
               .Include(r => r.Quotes)
               .ThenInclude(t => t.Materials)
               .Include(q => q.SentNotifications/*.Where(t => t.ResponseStatuses.FixerStatuseCode == 3)*/)
               .FirstOrDefault();


            RequestTemplateViewModel RequestTempVM = new RequestTemplateViewModel();
            // var req = request.FirstOrDefault();

            var client = _ctx.Users
               .Where(p => p.Id == req.ClienteleId).FirstOrDefault();

            RequestTempVM.RequestId = req.RequestId;
            RequestTempVM.ClienteleId = req.ClienteleId;
            RequestTempVM.ClienteleFirstName = client.FirstName;
            RequestTempVM.ClientImgUrl = client.ProfileImgUrl;
            RequestTempVM.Title = req.Title;
            RequestTempVM.Description = req.Description;
            RequestTempVM.ClienteleFirstName = client.FirstName;
            RequestTempVM.ClienteleLastName = client.LastName;
            RequestTempVM.ServiceType = req.ServiceType;
            RequestTempVM.DateSummited = req.DateOfRequest;
            RequestTempVM.Urgency = req.PreferredStartDate;
            RequestTempVM.Address = req.Location;
            RequestTempVM.ImgUrl_1 = req.ImgUrl_1;
            RequestTempVM.ImgUrl_2 = req.ImgUrl_2;
            RequestTempVM.ImgUrl_3 = req.ImgUrl_3;
            RequestTempVM.AudioUrl = req.AudioUrl;
            RequestTempVM.VideoUrl = req.VideoUrl;
            RequestTempVM.PdfUrl = req.PdfUrl;
            RequestTempVM.Quotes = req.Quotes;
            RequestTempVM.QuotesCount = req.Quotes.Count;
            RequestTempVM.sentNotifications = req.SentNotifications.ToList();
            RequestTempVM.SentNotisficationCount = req.SentNotifications.Count();

            //List of  Received Quotes
            if (req.Quotes.Count > 0)
            {
                foreach (var quote in req.Quotes.Where(p => p.IsQuoteAccepted != true))
                {
                    QuoteMiniTemplateViewModel quoteMiniTemplateViewModel = new QuoteMiniTemplateViewModel();

                    quoteMiniTemplateViewModel.QuoteId = quote.QuoteId;
                    quoteMiniTemplateViewModel.RequestId = quote.RequestId;
                    quoteMiniTemplateViewModel.ClienteleId = quote.ClienteleId;
                    quoteMiniTemplateViewModel.FixerId = quote.FixerId;
                    quoteMiniTemplateViewModel.Title = quote.Title;
                    quoteMiniTemplateViewModel.ShortDescription = quote.Description;
                    quoteMiniTemplateViewModel.EstimatedStartDate = quote.ProposedStartDate;
                    quoteMiniTemplateViewModel.EstimatedOverallCost = quote.EstimatedOverallCost;
                    quoteMiniTemplateViewModel.FixerName = quote.FixerName;
                    quoteMiniTemplateViewModel.EstimatedLabourCost = quote.LabourCost;
                    quoteMiniTemplateViewModel.DateSubmitted = quote.DateofQuote;
                    quoteMiniTemplateViewModel.sentNotificationId = quote.SentNotification.SentNotificationId;

                    RequestTempVM.QuoteMiniTemplateViewModels.Add(quoteMiniTemplateViewModel);
                }
                
               // RequestTempVM.AcceptedQuote = req.Quotes.FirstOrDefault(p => p.IsQuoteAccepted == true);
            }


            //List of send Request Notifications
            if (req.SentNotifications.Count() > 0)
            {
                foreach (var notification in req.SentNotifications.Where(t => t.ResponseStatuses.FixerStatuseCode == 3))
                {
                    var user = _ctx.Users
                   .Where(p => p.IsFixer == true && p.Id == notification.ReceiverId)
                   .Include(p => p.Profile)
                   .Include(r => r.Contact)
                   .SingleOrDefault();


                    FixerMiniTemplateViewModel thumb = new FixerMiniTemplateViewModel
                    {
                        FixerId = user.Id,
                        BtnClassName = "BtnClassName" + user.Id,
                        FixerImgUrl = user.ProfileImgUrl,
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName,
                        FullName = user.Title + " " + user.FirstName + " " + user.LastName,
                        ShortDescription = user.Profile.ShortDescription,

                        RatingScore = user.RatingScore,
                        TotalRatingCount = user.TotalRatingCount,
                        NoOfCompletedFixes = user.Profile.NoOfCompletedFixes,

                        IsCompanyRegistrationVerified = user.IsCompanyRegistrationVerified,
                        IsNINVerified = user.IsNINVerified,
                        Location = user.Contact.Address + ", " + user.Contact.Town + ", " /*+ user.Contact.Landmarks + " "*/ + user.Contact.LGA + ", " + user.Contact.State + ", " + user.Contact.Country,
                        sentNotification = notification
                    };
                    RequestTempVM.FixerMiniTemplateViewModels.Add(thumb);

                }
            }



            ////Accepted Quote section
            var acceptedQuotes = req.Quotes.FirstOrDefault(p => p.IsQuoteAccepted == true);
            
            var acceptedFixer = _ctx.Users
               .Where(p => p.Id == acceptedQuotes.FixerId).Include(p=>p.Contact).FirstOrDefault();


            CreateQuoteViewModel CreateAcceptedQuoteVM = new CreateQuoteViewModel();
            CreateAcceptedQuoteVM.FixerFirstNames = acceptedFixer.FirstName;
            CreateAcceptedQuoteVM.FixerLastName = acceptedFixer.LastName;
            CreateAcceptedQuoteVM.FixerAddress = acceptedFixer.Contact.Address;

            CreateAcceptedQuoteVM.QuotePrevAudioUrl = acceptedQuotes.AudioUrl;
            CreateAcceptedQuoteVM.QuotePrevVideoUrl = acceptedQuotes.VideoUrl;
            CreateAcceptedQuoteVM.QuotePrevPdfUrl = acceptedQuotes.PdfUrl;
            CreateAcceptedQuoteVM.QuotePrevImgUrl_1 = acceptedQuotes.ImgUrl_1;
            CreateAcceptedQuoteVM.QuotePrevImgUrl_2 = acceptedQuotes.ImgUrl_2;
            CreateAcceptedQuoteVM.QuotePrevImgUrl_3 = acceptedQuotes.ImgUrl_3;
            CreateAcceptedQuoteVM.QuoteDescription = acceptedQuotes.Description;
            CreateAcceptedQuoteVM.QuoteDateofResponse = acceptedQuotes.DateofQuote;
            CreateAcceptedQuoteVM.QuoteEstimatedCostOfMaterials = acceptedQuotes.EstimatedCostOfMaterials;
            CreateAcceptedQuoteVM.QuoteEstimatedFixDuration = acceptedQuotes.EstimatedFixDuration;
            CreateAcceptedQuoteVM.QuoteEstimatedLabourCost = acceptedQuotes.LabourCost;
            CreateAcceptedQuoteVM.QuoteEstimatedOverallCost = acceptedQuotes.EstimatedOverallCost;
            CreateAcceptedQuoteVM.QuoteProposedStartDate = acceptedQuotes.ProposedStartDate;
            CreateAcceptedQuoteVM.MaterialsList = acceptedQuotes.Materials;
            RequestTempVM.AcceptedQuoteSentNotisficationId = req.SentNotifications.FirstOrDefault(r => r.ReceiverId == acceptedQuotes.FixerId).SentNotificationId;
            RequestTempVM.Flags = FSRequestViewModel.ResponseCode.DefaultValue;

            RequestTempVM.CreateQuoteViewModel = CreateAcceptedQuoteVM;


            return RequestTempVM;
        }

        

        ////Preview details of Requests  without quotes in RequestPreview Page
        public RequestTemplateViewModel GetRequestPreviewById(int RequestId)
        {
            var req = _ctx.Requests
              //.Include(r => r.Quotes)/*.Where(s => s.RequestId == RequestId))*/
              .Include(q => q.SentNotifications)/*.Where(t => t.RequestId == RequestId)*/
              .Where(p => p.RequestId == RequestId)
              .FirstOrDefault();


            RequestTemplateViewModel RequestTempVM = new RequestTemplateViewModel();
            // var req = request.FirstOrDefault();

            var client = _ctx.Users
               .Where(p => p.Id == req.ClienteleId).FirstOrDefault();

            RequestTempVM.RequestId = req.RequestId;
            RequestTempVM.ClienteleId = req.ClienteleId;
            RequestTempVM.ClienteleFirstName = client.FirstName;
            RequestTempVM.ClientImgUrl = client.ProfileImgUrl;
            RequestTempVM.Title = req.Title;
            RequestTempVM.Description = req.Description;
            RequestTempVM.ClienteleFirstName = client.FirstName;
            RequestTempVM.ClienteleLastName = client.LastName;
            RequestTempVM.ServiceType = req.ServiceType;
            RequestTempVM.DateSummited = req.DateOfRequest;
            RequestTempVM.Urgency = req.PreferredStartDate;
            RequestTempVM.Address = req.Location;
            RequestTempVM.ImgUrl_1 = req.ImgUrl_1;
            RequestTempVM.ImgUrl_2 = req.ImgUrl_2;
            RequestTempVM.ImgUrl_3 = req.ImgUrl_3;
            RequestTempVM.AudioUrl = req.AudioUrl;
            RequestTempVM.VideoUrl = req.VideoUrl;
            RequestTempVM.PdfUrl = req.PdfUrl;

            RequestTempVM.sentNotifications = req.SentNotifications.ToList();
            RequestTempVM.SentNotisficationCount = req.SentNotifications.Count();





            if (req.SentNotifications.Count() > 0)
            {
                foreach (var notification in req.SentNotifications)
                {
                    var user = _ctx.Users
                   .Where(p => p.IsFixer == true && p.Id == notification.ReceiverId)
                   .Include(p => p.Profile)
                   .Include(r => r.Contact)
                   .SingleOrDefault();


                    FixerMiniTemplateViewModel thumb = new FixerMiniTemplateViewModel
                    {
                        FixerId = user.Id,
                        BtnClassName = "BtnClassName" + user.Id,
                        FixerImgUrl = "~/UsersProfile/" + user.ProfileImgUrl,
                        Title = user.Title,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName,
                        FullName = user.Title + " " + user.FirstName + " " + user.LastName,
                        ShortDescription = user.Profile.ShortDescription,

                        RatingScore = user.RatingScore,
                        TotalRatingCount = user.TotalRatingCount,
                        NoOfCompletedFixes = user.Profile.NoOfCompletedFixes,

                        IsCompanyRegistrationVerified = user.IsCompanyRegistrationVerified,
                        IsNINVerified = user.IsNINVerified,
                        Location = user.Contact.Address + ", " + user.Contact.Town + ", " /*+ user.Contact.Landmarks + " "*/ + user.Contact.LGA + ", " + user.Contact.State + ", " + user.Contact.Country,
                        sentNotification = notification
                    };
                    RequestTempVM.FixerMiniTemplateViewModels.Add(thumb);

                }                
            }
            return RequestTempVM;
        }


        /// <summary>
        /// //Auxillary method section
        /// </summary>        
        /// //Auxillary method to handle creation of multiple request thumbs 
        private static CSActiveRequestViewModel FetchActiveRequestViewModel(List<Request> request)
        {
            var CSViewModel = new CSActiveRequestViewModel();
            
            foreach (var req in request)
            {
             
                var requestminiTemp = new RequestMiniTemplateViewModel();

                requestminiTemp.RequestId = req.RequestId;
                requestminiTemp.ClienteleId = req.ClienteleId;
                requestminiTemp.Title = req.Title;
                requestminiTemp.ShortDescription = req.Description;
                requestminiTemp.DateSummited = req.DateOfRequest;
                requestminiTemp.Urgency = req.PreferredStartDate;
                requestminiTemp.ServiceType = req.ServiceType;
                requestminiTemp.SentNotisficationCount = req.SentNotifications.Count(); 
                requestminiTemp.RejectedRequestCount = req.SentNotifications.Where(p => p.ResponseStatuses.FixerStatuseCode == 2).Count();

                if (req.Quotes != null && req.Quotes.Count > 0)
                {
                    requestminiTemp.SentNotificationId = req.SentNotifications.FirstOrDefault().SentNotificationId;
                    requestminiTemp.QuoteId = req.Quotes.FirstOrDefault().QuoteId;
                    requestminiTemp.QuoteCount = req.Quotes.Count;
                    requestminiTemp.FixerId = req.Quotes.FirstOrDefault().FixerId;                    
                }

                CSViewModel.RequestMiniTemplateViewModels.Add(requestminiTemp);
            }

            return CSViewModel;
        }

        public void PostQuote(Quote quote)
        {
            try
            {
                if (quote != null)
                {
                   // quote.SentNotification.ResponseStatuses.FirstOrDefault().FixerStatuseCode = 1;

                    _ctx.Quotes.Add(quote);
                    _ctx.SaveChanges();
                }
            }
            catch(Exception ex) 
            {
                throw;
            }
          
            
           
        }

        public IEnumerable<Material> GetMaterials()
        {
            throw new NotImplementedException();
        }

        public void PostMaterial(Material material)
        {
           _ctx.Materials.Add(material);
            _ctx.SaveChanges();
        }

        // Implementing Fetch RequestNotifications and request yet to get a quote by the current login fixer to populate fixer Space index page
        public FSRequestViewModel GetFSDashboardByFixerId(int fixerId)
        {           

            var request = _ctx.Requests
              .Where(p => p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ReceiverId == fixerId            //filter only for login fixer
              && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.FixerStatuseCode == 3    //only for fixer is yetToResponse 
              && p.IsContracted == false)                                                                                   //Opened request
              .Include(p => p.SentNotifications)
              .Include(q => q.Quotes)
              .ToList();


            FSRequestViewModel FSViewModel = GenerateRequestMiniTemplateVM(fixerId, request);
            var fixer = _ctx.Users.FirstOrDefault(p => p.Id == fixerId);
            FSViewModel.FixerName = fixer.FirstName + " " + fixer.LastName;

            return FSViewModel;
        }

        // Implementing Fetch for quotes awaiting clients actions for opened Requests
        public FSRequestViewModel GetFSQuotesWithOpenedRequest(int fixerId)
        {
             var request = _ctx.Requests
             .Where(p => p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ReceiverId == fixerId            //filter only for login fixer
             && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.FixerStatuseCode == 1    //only for fixer submitted a quote 
             && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.ClientStatuseCode == 1    //but client status still shows submitted Request
             && p.IsContracted == false)                                                                                   //Opened request
             //.Include(p => p.SentNotifications)
             .Include(q => q.Quotes)
             .ToList();


            var FSViewModel = new FSRequestViewModel();
            FSViewModel.FixerId = fixerId;
            var fixer = _ctx.Users.FirstOrDefault(p => p.Id == fixerId);
            FSViewModel.FixerName = fixer.FirstName + " " + fixer.LastName;



            if (request.Count > 0)
            {
                foreach (var req in request)
                {
                    if(req.Quotes.Count > 0)
                    {
                        var requestminiTemp = new RequestMiniTemplateViewModel();
                        var quote = req.Quotes.FirstOrDefault(r => r.FixerId == fixerId);

                        requestminiTemp.QuoteId = quote.QuoteId;
                        requestminiTemp.RequestId = req.RequestId;
                        requestminiTemp.ClienteleId = req.ClienteleId;
                        requestminiTemp.ClientName = req.ClientName;
                        requestminiTemp.Title = quote.Title;
                        requestminiTemp.ShortDescription = quote.Description;
                        requestminiTemp.DateSummited = quote.DateofQuote;
                        requestminiTemp.Urgency = quote.StartUrgency;
                        requestminiTemp.ServiceType = req.ServiceType;
                        //requestminiTemp.SentNotisficationCount = req.SentNotifications.Count();
                        //requestminiTemp.QuoteCount = req.Quotes.Count;

                        FSViewModel.RequestMiniTemplateViewModels.Add(requestminiTemp);
                    }                    
                }
                
                return FSViewModel;
            }
            FSViewModel.ErrorMsg = "No submitted Quote(s) awaiting response from the clientele";
            return FSViewModel;
        }

        // Implementing Fetch for quotes awaiting clients actions for Closed Request
        public FSRequestViewModel GetFSQuotesWithClosedRequest(int fixerId)
        {
            var request = _ctx.Requests
             .Where(p => p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ReceiverId == fixerId            //filter only for login fixer
             && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.FixerStatuseCode == 1    //only for fixer submitted a quote               
             && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.ClientStatuseCode == 1    //but client status still shows submitted Request
             && p.IsContracted == true)                                                                                   //Opened request
             //.Include(p => p.SentNotifications)
             .Include(q => q.Quotes)
             .ToList();


            var FSViewModel = new FSRequestViewModel();
            FSViewModel.FixerId = fixerId;
            var fixer = _ctx.Users.FirstOrDefault(p => p.Id == fixerId);
            FSViewModel.FixerName = fixer.FirstName + " " + fixer.LastName;


            if (request.Count > 0)
            {
                foreach (var req in request)
                {
                    if (req.Quotes.Count > 0)
                    {
                        var requestminiTemp = new RequestMiniTemplateViewModel();
                        var quote = req.Quotes.FirstOrDefault(r => r.FixerId == fixerId);

                        requestminiTemp.QuoteId = quote.QuoteId;
                        requestminiTemp.RequestId = req.RequestId;
                        requestminiTemp.ClienteleId = req.ClienteleId;
                        requestminiTemp.ClientName = req.ClientName;
                        requestminiTemp.Title = quote.Title;
                        requestminiTemp.ShortDescription = quote.Description;
                        requestminiTemp.DateSummited = quote.DateofQuote;
                        requestminiTemp.Urgency = quote.StartUrgency;
                        requestminiTemp.ServiceType = req.ServiceType;

                        FSViewModel.RequestMiniTemplateViewModels.Add(requestminiTemp);
                    }
                }

                return FSViewModel;
            }
            FSViewModel.ErrorMsg = "No submitted Quote(s) awaiting response from the clientele";
            return FSViewModel;
        }




        //list of fixes Quotes accepted by Clientele but not yet completed
        public FSRequestViewModel GetFSActiveFixesByFixerId(int fixerId)
        {           

            var request = _ctx.Requests
           .Where(p => p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ReceiverId == fixerId            //filter only for login fixer
           && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.FixerStatuseCode == 1    //only for fixer submitted a quote               
           && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.ClientStatuseCode == 5    //but client status still shows submitted Request
           && p.IsContracted == true && p.IsCompleted == false)                                                                                   //Opened request
           .Include(p => p.SentNotifications)
           .Include(q => q.Quotes)
           .ToList();



            FSRequestViewModel FSViewModel = GenerateRequestMiniTemplateVM(fixerId, request);
            var fixer = _ctx.Users.FirstOrDefault(p => p.Id == fixerId);
            FSViewModel.FixerName = fixer.FirstName + " " + fixer.LastName;

            return FSViewModel;
        }
        //list of completed fixes
        public CSActiveRequestViewModel GetCompletedFixesByClientId(int clienteleId)
        {
            //var request = _ctx.sentNotifications
            //  .Where(p => p.SenderId == clienteleId && p.ResponseStatuses.FixerStatuseCode == 1 && p.ResponseStatuses.ClientStatuseCode == 4)//FixerStatusCode==1 submitted quote; clientstatuscode == 5 AcceptedQuote
            //  .Include(q => q.Request.Quotes.Where(s => s.ClienteleId == clienteleId))
            //  .Select(r => r.Request)//.Distinct()
            //  .ToList();

            var request = _ctx.Requests
               .Where(p => p.ClienteleId == clienteleId && p.IsCompleted == true && p.IsArchive==false).Distinct()
               .Include(p => p.SentNotifications)
               .Include(q => q.Quotes)
               .ToList();


            var client = _ctx.Users
              .FirstOrDefault(p => p.Id == clienteleId);

            CSActiveRequestViewModel CSViewModel = FetchActiveRequestViewModel(request);
            CSViewModel.ClienteleName = client.FirstName + " " + client.LastName;
            CSViewModel.Flags = FSRequestViewModel.ResponseCode.DefaultValue;

            return CSViewModel;

        }








        //list of completed fixes
        public FSRequestViewModel GetFSFixesCompletedFixesByFixerId(int fixerId)
        {            
            var request = _ctx.Requests
            .Where(p => p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ReceiverId == fixerId            //filter only for login fixer
            //&& p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.FixerStatuseCode == 1    //only for fixer submitted a quote               
            //&& p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.ClientStatuseCode == 1    //but client status still shows submitted Request
            && p.IsCompleted == true)                                                                                   //Opened request
            .Include(p => p.SentNotifications)
            .Include(q => q.Quotes)
            .ToList();

            FSRequestViewModel FSViewModel = GenerateRequestMiniTemplateVM(fixerId, request);
            var fixer = _ctx.Users.FirstOrDefault(p => p.Id == fixerId);
            FSViewModel.FixerName = fixer.FirstName + " " + fixer.LastName;

            return FSViewModel;
        }

        //list of Quotes rejected by Clientele
        public FSRequestViewModel GetFSFixesRejectedQuotesByFixerId(int fixerId)
        {
          var request = _ctx.Requests
          .Where(p => p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ReceiverId == fixerId            //filter only for login fixer
          && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.FixerStatuseCode == 1    //only for fixer submitted a quote               
          && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.ClientStatuseCode == 2)    //but client status still shows submitted Request                                                                                   //Opened request
          .Include(p => p.SentNotifications)
          .Include(q => q.Quotes)
          .ToList();

            FSRequestViewModel FSViewModel = GenerateRequestMiniTemplateVM(fixerId, request);
            var fixer = _ctx.Users.FirstOrDefault(p => p.Id == fixerId);
            FSViewModel.FixerName = fixer.FirstName + " " + fixer.LastName;

            return FSViewModel;
        }

        //list of Request rejected by Fixer
        public FSRequestViewModel GetFSFixesRejectedRequestByFixerId(int fixerId)
        {

             var request = _ctx.Requests
             .Where(p => p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ReceiverId == fixerId            //filter only for login fixer
             && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.FixerStatuseCode == 2    //only for fixer rejected the request               
             && p.SentNotifications.FirstOrDefault(r => r.ReceiverId == fixerId).ResponseStatuses.ClientStatuseCode == 1)    //but client status still shows submitted Request                                                                                   //Opened request
             .Include(p => p.SentNotifications)
             .Include(q => q.Quotes)
             .ToList();


            FSRequestViewModel FSViewModel = GenerateRequestMiniTemplateVM(fixerId, request);
            var fixer = _ctx.Users.FirstOrDefault(p => p.Id == fixerId);
            FSViewModel.FixerName = fixer.FirstName + " " + fixer.LastName;

            return FSViewModel;
        }

        private static FSRequestViewModel GenerateRequestMiniTemplateVM(int fixerId, List<Request> request)
        {
            var FSViewModel = new FSRequestViewModel();
            FSViewModel.FixerId = fixerId;


            foreach (var req in request)
            {                
                var requestminiTemp = new RequestMiniTemplateViewModel();               

                if (req.Quotes!=null &&  req.Quotes.Count > 0)
                {
                    requestminiTemp.QuoteId = req.Quotes.FirstOrDefault().QuoteId;
                }

                requestminiTemp.SentNotificationId = req.SentNotifications.FirstOrDefault(p => p.ReceiverId == fixerId).SentNotificationId;

                requestminiTemp.RequestId = req.RequestId;
                requestminiTemp.ClienteleId = req.ClienteleId;
                requestminiTemp.ClientName = req.ClientName;
                requestminiTemp.Title = req.Title;
                requestminiTemp.ShortDescription = req.Description;
                requestminiTemp.DateSummited = req.DateOfRequest;
                requestminiTemp.Urgency = req.PreferredStartDate;
                requestminiTemp.ServiceType = req.ServiceType;               
                requestminiTemp.SentNotisficationCount = req.SentNotifications.Count();
                requestminiTemp.QuoteCount = req.Quotes.Count;

                FSViewModel.RequestMiniTemplateViewModels.Add(requestminiTemp);
            }

            return FSViewModel;
        }

        public Request GetSingleRequest(int requestId)
        {
            return _ctx.Requests.FirstOrDefault(x => x.RequestId == requestId);
        }

        public string UpdateSentNotification(int sentNotificationId, int fixerStatuseCode, int clientStatuseCode)
        {
            try
            {
                var responseStatus = _ctx.sentNotifications.Where(p => p.SentNotificationId == sentNotificationId).FirstOrDefault().ResponseStatuses;

                responseStatus.FixerStatuseCode = fixerStatuseCode;
                responseStatus.ClientStatuseCode = clientStatuseCode;
                _ctx.NotificationResponseStatuses.Update(responseStatus);
                int updateCount = _ctx.SaveChanges();

                /// check whether saveChanges was successful by returning int higer than zero
                /// 
                if (updateCount > 0)
                {
                    return "1"; ///sucessful update flag
                }
                else
                {
                    return "Update Failed.."; ///failed to update flag
                }
            }

            catch (Exception ex)
            {
                return "Update Failed..with the following errors" + " " + ex;
            }
        }


        public string UpdateRequestIsContractedStatus(bool isContracted, bool isClientMarkCompleted, int requestId, int fixerId, int quoteId)
        {
            try
            {
                var quote = _ctx.Quotes.FirstOrDefault(p => p.QuoteId == quoteId);
                if (quote != null)
                {
                    quote.IsQuoteAccepted = true;
                    _ctx.Quotes.Update(quote);
                }
                

                var responseStatus = _ctx.Requests.FirstOrDefault(p => p.RequestId == requestId);
                if (responseStatus != null)
                {
                    responseStatus.IsContracted = isContracted;
                    responseStatus.IsContractedTo = fixerId;
                    responseStatus.IsCompleted = isClientMarkCompleted;
                    _ctx.Requests.Update(responseStatus);
                }               

               // _ctx.SaveChanges();


                int updateCount = _ctx.SaveChanges();

                /// check whether saveChanges was successful by returning int higer than zero
                /// 
                if (updateCount > 0)
                {
                    return "1"; ///sucessful update flag
                }
                else
                {
                    return "Update Failed.."; ///failed to update flag
                }
            }

            catch (Exception ex)
            {
                return "Update Failed..with the following errors" + " " + ex;
            }
        }
    }
}
