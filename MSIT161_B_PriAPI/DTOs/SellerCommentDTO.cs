using MSIT161_B_PriAPI.Models;

namespace MSIT161_B_PriAPI.DTOs
{
    public class SellerCommentDTO
    {
        public int FCommentId { get; set; }

        public int? FSellerid { get; set; }


        public string? FUserName { get; set; }

        public double? FScore { get; set; }

        public string FComment { get; set; }

        public DateOnly? FCommentDate { get; set; }

        public bool? AnonymousUser { get; set; }=false;

        public string FSellerReply { get; set; }
    }
}
