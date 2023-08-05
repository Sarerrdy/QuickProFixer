using Microsoft.AspNetCore.Mvc.Rendering;
using QuickProFixer.Models.Entities;
using QuickProFixer.Models.UtilityModels;
using QuickProFixer.ViewModels;

namespace QuickProFixer.Services
{
    public interface IQuickProFixerRepository
    {
       
        IEnumerable<ServiceType> GetCategories(int? CategoryId);
        IEnumerable<PriceRateType> GetPriceRate(int? PriceRateTypeId);
        void PostRequest(Request request);
       
        CSActiveRequestViewModel GetRequestByClientId(int ClienteleId);
        CSActiveRequestViewModel GetCSReceivedQuotesByClienteleId(int clienteleId);
        RequestTemplateViewModel GetRequestPreviewById(int RequestId);
        CSActiveRequestViewModel GetActiveRequestByClientId(int clienteleId);
        CSActiveRequestViewModel GetCompletedFixesByClientId(int clientId);
        RequestTemplateViewModel GetActiveRequestDetailsById(int RequestId);




        // Defining Fetch Request Notifications associated to a particular Fixer
        FSRequestViewModel GetFSDashboardByFixerId(int fixerId);
        FSRequestViewModel GetFSQuotesWithOpenedRequest(int fixerId);
        FSRequestViewModel GetFSQuotesWithClosedRequest(int fixerId);
        FSRequestViewModel GetFSFixesCompletedFixesByFixerId(int fixerId);
        FSRequestViewModel GetFSActiveFixesByFixerId(int fixerId);
        FSRequestViewModel GetFSFixesRejectedQuotesByFixerId(int fixerId);
        public FSRequestViewModel GetFSFixesRejectedRequestByFixerId(int fixerId);

        Request GetSingleRequest(int requestId);


        RequestTemplateViewModel GetRequestById(int RequestId);
        CreateQuoteViewModel CreateQuoteViewModel(int RequestId, int fixerId);
        CreateQuoteViewModel CreateQuotePreview(int QuoteId);

        void PostQuote(Quote quote);
        IEnumerable<Material> GetMaterials();
        void PostMaterial (Material material);

        IEnumerable<Country> GetCountryById(int CountryId);
        IEnumerable<Country> GetCountries();

        IEnumerable<State> GetStates(string countryId);

        IEnumerable<LGA> GetLGAs(string stateId);
        SendRequestViewModel GetFixersFromClosedProximity(string lgaName, int serviceId, int requestId, string requestTitle, int senderId);

        IEnumerable<User> GetUser(int Id);
        FSFixerDetailsViewModel GetUserDatails(int userId);


        string UpdateRequest(Request request);
        void DeleteRequest(Request request);

        string CreateSentNotifications(List<SentNotification> sentNotifications);
        void DeleteSentNotifications(List<SentNotification> sentNotifications);

        string CreateSingleSentNotification(SentNotification sentNotification);
        void DeleteSingleSentNotification(SentNotification sentNotification);
        string UpdateSentNotification(int sentNotificationId, int fixerStatuseCode, int clientStatuseCode);
        string UpdateRequestIsContractedStatus(bool isContracted, bool isClientMarkCompleted, int requestId, int fixerId, int quoteId);


        ///Ratings and Reviews
        IEnumerable<Rating> GetStarRatingFromDb(int userId);
        void AddStarRatingToDB(Rating starRating, int one, int two, int three, int four, int five);
        void AddUserReviewComment(Review rev);
        bool IsExpertRatedByCurrentUser(int RevieweeId, int ReviewerId);

        //Messages
        void AddMessageToDB(Message msg);
    }
}
