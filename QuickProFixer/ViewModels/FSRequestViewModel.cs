namespace QuickProFixer.ViewModels
{
    public class FSRequestViewModel
    {
        public FSRequestViewModel()
        {
            RequestMiniTemplateViewModels = new List<RequestMiniTemplateViewModel>();
            QuoteMiniTemplateViewModels = new List<QuoteMiniTemplateViewModel>();
        }

        public int FixerId { get; set; }
        public int ClienteleId { get; set; }
        public string FixerName { get; set; }
        public string ClienteleName { get; set; }
        public string ErrorMsg { get; set; }
        public int SentNotificationId { get; set; }
        public string FeedBackMsg { get; set; }


        // Fixer: 1 == QuoteSubmitted; 2 == RequestRejected; 3 == RequestNoResponseYet / Default; 4 == FixCompleted
        //Client: 1 ==RequestSubmitted/Default;  2== QuoteRejected; 3==QuoteNoResponseYet; 4==FixCompleted 5==QuoteAccepted;
        //ActiveFixes: fixer = 1; client=5; //RejectedRequest: fixer = 2; client=1; 
        public enum ResponseCode
        {
            DefaultValue,//Fixer == 3 && Client == 1
            ClienteleMarksFixAsActive,//Fixer == 1 && Client == 5
            RejectRequest, //Fixer == 2 && Client == 1
            RejectQuote, //Fixer == 2 && Client == 1           
            FixerMarksAsCompleted, //Fixer == 4 && Client == 5
            ClieteleMarksAsCompleted //Fixer == 1 && Client == 4
        }

        public ResponseCode Flags { get; set; }

        public List<RequestMiniTemplateViewModel>? RequestMiniTemplateViewModels { get; set; }
        public List<QuoteMiniTemplateViewModel>? QuoteMiniTemplateViewModels { get; set; }
    }
}
