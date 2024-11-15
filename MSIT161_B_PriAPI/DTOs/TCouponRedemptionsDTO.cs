namespace MSIT161_B_PriAPI.DTOs
{
    public class TCouponRedemptionsDTO
    {
        public int FCouponRedemptionId { get; set; }

        public int FCouponCodeId { get; set; }

        public int FUserId { get; set; }

        public DateTime? FCouponGetDate { get; set; }

        public DateTime? FCouponUseDate { get; set; }

        public bool? FCouponUseState { get; set; }

        public int? FCouponUsageCount { get; set; }
    }
}
