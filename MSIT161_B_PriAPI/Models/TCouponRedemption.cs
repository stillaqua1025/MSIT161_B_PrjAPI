﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT161_B_PriAPI.Models;

public partial class TCouponRedemption
{
    public int FCouponRedemptionId { get; set; }

    public int FCouponCodeId { get; set; }

    public int FUserId { get; set; }

    public DateTime? FCouponGetDate { get; set; }

    public DateTime? FCouponUseDate { get; set; }

    public bool? FCouponUseState { get; set; }

    public int? FCouponUsageCount { get; set; }

    public virtual TCoupon FCouponCode { get; set; }

    public virtual TUser FUser { get; set; }
}