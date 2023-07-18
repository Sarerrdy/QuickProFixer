using static QuickProFixer.ViewModels.FSRequestViewModel;

namespace QuickProFixer.ViewModels
{
    public class CSActiveRequestViewModel
    {
        public CSActiveRequestViewModel()
        {
            RequestMiniTemplateViewModels = new List<RequestMiniTemplateViewModel>();
        }

        public List<RequestMiniTemplateViewModel>? RequestMiniTemplateViewModels { get; set; }
        public string ClienteleName { get; set; }
        public ResponseCode Flags { get; set; }

    }
}
