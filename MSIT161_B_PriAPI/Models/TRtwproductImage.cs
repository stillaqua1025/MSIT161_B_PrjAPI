﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT161_B_PriAPI.Models;

public partial class TRtwproductImage
{
    public int FProductImageId { get; set; }

    public int? FProductId { get; set; }

    public string FProductImage { get; set; }

    public virtual ICollection<TRtwproductDetail> TRtwproductDetails { get; set; } = new List<TRtwproductDetail>();
}