﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT161_B_PriAPI.Models;

public partial class TTag
{
    public int FTagId { get; set; }

    public string FTagName { get; set; }

    public int? FPartId { get; set; }

    public virtual TPart FPart { get; set; }

    public virtual ICollection<TRtwproduct> TRtwproducts { get; set; } = new List<TRtwproduct>();

    public virtual ICollection<TUserTag> TUserTags { get; set; } = new List<TUserTag>();
}