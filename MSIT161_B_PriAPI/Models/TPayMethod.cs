﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT161_B_PriAPI.Models;

public partial class TPayMethod
{
    public int FPayMethodId { get; set; }

    public string FPayMethod { get; set; }

    public virtual ICollection<TRtworder> TRtworders { get; set; } = new List<TRtworder>();
}