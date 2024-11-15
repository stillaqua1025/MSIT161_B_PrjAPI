﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT161_B_PriAPI.Models;

public partial class TRtworder
{
    public int FOrderId { get; set; }

    public string FOrderNumber { get; set; }

    public int? FUserId { get; set; }

    public int? FCouponCodeId { get; set; }

    public decimal? FPrice { get; set; }

    public DateTime FTime { get; set; }

    public int FShipMethodId { get; set; }

    public int FPayMethodId { get; set; }

    public string FAddress { get; set; }

    public int FShipStateId { get; set; }

    public bool FFinished { get; set; }

    public int? FOrderChangeReasonId { get; set; }

    public DateTime? FFinishedTime { get; set; }

    public bool FCancel { get; set; }

    public virtual TCoupon FCouponCode { get; set; }

    public virtual TOrderChangeReason FOrderChangeReason { get; set; }

    public virtual TPayMethod FPayMethod { get; set; }

    public virtual TShipMethod FShipMethod { get; set; }

    public virtual TShipState FShipState { get; set; }

    public virtual TUser FUser { get; set; }

    public virtual ICollection<TRtworderDetail> TRtworderDetails { get; set; } = new List<TRtworderDetail>();
}