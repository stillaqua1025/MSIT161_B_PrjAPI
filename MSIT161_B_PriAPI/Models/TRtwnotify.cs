﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MSIT161_B_PriAPI.Models;

public partial class TRtwnotify
{
    public int FNotifyId { get; set; }

    public int? FUserId { get; set; }

    public string FNotify { get; set; }

    public bool? FIsNotRead { get; set; }

    public int? FNotifyTypeId { get; set; }

    public bool? FNotifyState { get; set; }

    public virtual TNotifyType FNotifyType { get; set; }

    public virtual TUser FUser { get; set; }
    public virtual ICollection<TSendNotify> TSendNotifies { get; set; } = new List<TSendNotify>();
}