﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT161_B_PriAPI.Models;

public partial class TRtworderDetail
{
    public int FOrderDetailId { get; set; }

    public int? FOrderId { get; set; }

    public int? FProductDetailId { get; set; }

    public int? FQty { get; set; }

    public int? FPrice { get; set; }

    public virtual TRtworder FOrder { get; set; }

    public virtual TRtwproductDetail FProductDetail { get; set; }
}