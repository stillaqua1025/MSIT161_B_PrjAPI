using MSIT161_B_PriAPI.Models;

namespace MSIT161_B_PriAPI.DTOs
{
    public class CouponDTO
    {
        public int FCouponCodeId { get; set; }

        public string FCouponCodeName { get; set; }

        public string CouponDescription { get; set; }

        public string FCouponCode { get; set; }

        public double FCouponDiscount { get; set; }

        public DateTime FCouponCreatday { get; set; }

        public DateTime? FCouponEndday { get; set; }

        public int FCouponFrom { get; set; }

        public decimal? MinSellMoney { get; set; }
		
	}
}
