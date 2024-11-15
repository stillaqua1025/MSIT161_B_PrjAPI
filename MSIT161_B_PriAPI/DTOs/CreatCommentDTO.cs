using MSIT161_B_PriAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSIT161_B_PriAPI.DTOs
{
	public class CreatCommentDTO
	{
		public int FCommentId { get; set; }
		public int? FSellerId { get; set; }
		public int? FUserId { get; set; }
		public double? FScore { get; set; }
        public string FComment { get; set; }
        public DateOnly? FCommentDate { get; set; }

		public bool? AnonymousUser { get; set; } = false;

		public string? FSellerReply { get; set; }
	
	}
}
