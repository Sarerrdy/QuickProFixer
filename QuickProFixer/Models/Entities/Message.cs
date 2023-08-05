using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace QuickProFixer.Models.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public int RecieverId { get; set; }
        public string RecieverUserName { get; set; }
        public string SenderUserName { get; set; }
        public int SenderId { get; set; }
        public string AttachmentName { get; set; }
        public bool? MarkAsRead { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 20)]
        [Display(Name = "Content")]
        public string MsgContent { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Title")]
        public string MsgTitle { get; set; }
        public DateTime MsgDate { get; set; }
    }
}
