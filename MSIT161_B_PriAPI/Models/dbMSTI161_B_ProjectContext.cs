﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MSIT161_B_PriAPI.Models;

public partial class dbMSTI161_B_ProjectContext : DbContext
{
    public dbMSTI161_B_ProjectContext()
    {
    }

    public dbMSTI161_B_ProjectContext(DbContextOptions<dbMSTI161_B_ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<TChatImage> TChatImages { get; set; }

    public virtual DbSet<TColor> TColors { get; set; }

    public virtual DbSet<TCondition> TConditions { get; set; }

    public virtual DbSet<TCoupon> TCoupons { get; set; }

    public virtual DbSet<TCouponRedemption> TCouponRedemptions { get; set; }

    public virtual DbSet<TFriend> TFriends { get; set; }

    public virtual DbSet<TLoginRecord> TLoginRecords { get; set; }

    public virtual DbSet<TMemberChat> TMemberChats { get; set; }

    public virtual DbSet<TNotifyType> TNotifyTypes { get; set; }

    public virtual DbSet<TOrderChangeReason> TOrderChangeReasons { get; set; }

    public virtual DbSet<TPart> TParts { get; set; }

    public virtual DbSet<TPayMethod> TPayMethods { get; set; }

    public virtual DbSet<TRtwcomment> TRtwcomments { get; set; }

    public virtual DbSet<TRtwnotify> TRtwnotifies { get; set; }

    public virtual DbSet<TRtworder> TRtworders { get; set; }

    public virtual DbSet<TRtworderDetail> TRtworderDetails { get; set; }

    public virtual DbSet<TRtwproduct> TRtwproducts { get; set; }

    public virtual DbSet<TRtwproductDetail> TRtwproductDetails { get; set; }

    public virtual DbSet<TRtwproductImage> TRtwproductImages { get; set; }

    public virtual DbSet<TRtwshoppingCart> TRtwshoppingCarts { get; set; }

    public virtual DbSet<TSendNotify> TSendNotifies { get; set; }

    public virtual DbSet<TShipMethod> TShipMethods { get; set; }

    public virtual DbSet<TShipState> TShipStates { get; set; }

    public virtual DbSet<TSize> TSizes { get; set; }

    public virtual DbSet<TTag> TTags { get; set; }

    public virtual DbSet<TUser> TUsers { get; set; }

    public virtual DbSet<TUserTag> TUserTags { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbMSTI161_B_Project;Integrated Security=True;Encrypt=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.BfUser).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.BfUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AspNetUsers_tUsers");

            // 已經通過 AspNetUserRole 設置了多對多關係，不需要直接設置 Roles 和 Users 的多對多關係
        });


        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // 確保 UserId 和 Id 的類型一致
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserLogins)
                .HasForeignKey(d => d.UserId);  // 確保 UserId 類型為 string
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            // 設置複合主鍵
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            // 定義外鍵關係
            entity.HasOne(ur => ur.User)
                  .WithMany(u => u.AspNetUserRoles)
                  .HasForeignKey(ur => ur.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                  .WithMany(r => r.AspNetUserRoles)
                  .HasForeignKey(ur => ur.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);

            // 設置索引
            entity.HasIndex(ur => ur.RoleId, "IX_AspNetUserRoles_RoleId");
        });


        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TChatImage>(entity =>
        {
            entity.HasKey(e => e.FChatImageId).HasName("PK_ChatImages");

            entity.ToTable("tChatImages");

            entity.Property(e => e.FChatImageId).HasColumnName("fChatImageId");
            entity.Property(e => e.FChatId).HasColumnName("fChatId");
            entity.Property(e => e.FImageData)
                .IsRequired()
                .HasColumnName("fImageData");

            entity.HasOne(d => d.FChat).WithMany(p => p.TChatImages)
                .HasForeignKey(d => d.FChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tChatImages_tMemberChat1");
        });

        modelBuilder.Entity<TColor>(entity =>
        {
            entity.HasKey(e => e.FColorId);

            entity.ToTable("tColor");

            entity.Property(e => e.FColorId).HasColumnName("fColorId");
            entity.Property(e => e.FColorName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fColorName");
        });

        modelBuilder.Entity<TCondition>(entity =>
        {
            entity.HasKey(e => e.FConditionsId);

            entity.ToTable("tConditions");

            entity.Property(e => e.FConditionsId).HasColumnName("fConditionsId");
            entity.Property(e => e.FConditionsName)
                .HasMaxLength(50)
                .HasColumnName("fConditionsName");
        });

        modelBuilder.Entity<TCoupon>(entity =>
        {
            entity.HasKey(e => e.FCouponCodeId).HasName("PK_Coupon");

            entity.ToTable("tCoupon");

            entity.Property(e => e.FCouponCodeId).HasColumnName("fCoupon_Code_ID");
            entity.Property(e => e.CouponDescription)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("coupon_description");
            entity.Property(e => e.FCouponCode)
                .HasMaxLength(50)
                .HasColumnName("fCoupon_code");
            entity.Property(e => e.FCouponCodeName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fCoupon_Code_Name");
            entity.Property(e => e.FCouponCreatday)
                .HasColumnType("datetime")
                .HasColumnName("fCoupon_Creatday");
            entity.Property(e => e.FCouponDiscount).HasColumnName("fCoupon_Discount");
            entity.Property(e => e.FCouponEndday)
                .HasColumnType("datetime")
                .HasColumnName("fCoupon_endday");
            entity.Property(e => e.FCouponFrom).HasColumnName("fCoupon_from");
            entity.Property(e => e.FCouponState).HasColumnName("fCoupon_state");
            entity.Property(e => e.MinSellMoney)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("minSellMoney");

            entity.HasOne(d => d.FCouponFromNavigation).WithMany(p => p.TCoupons)
                .HasForeignKey(d => d.FCouponFrom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tCoupon_tUsers");
        });

        modelBuilder.Entity<TCouponRedemption>(entity =>
        {
            entity.HasKey(e => e.FCouponRedemptionId);

            entity.ToTable("tCoupon_Redemption");

            entity.Property(e => e.FCouponRedemptionId).HasColumnName("fCoupon_Redemption_ID");
            entity.Property(e => e.FCouponCodeId).HasColumnName("fCoupon_Code_ID");
            entity.Property(e => e.FCouponGetDate)
                .HasColumnType("datetime")
                .HasColumnName("fCoupon_Get_Date");
            entity.Property(e => e.FCouponUsageCount).HasColumnName("fCoupon_Usage_Count");
            entity.Property(e => e.FCouponUseDate)
                .HasColumnType("datetime")
                .HasColumnName("fCoupon_Use_Date");
            entity.Property(e => e.FCouponUseState).HasColumnName("fCoupon_UseState");
            entity.Property(e => e.FUserId).HasColumnName("fUser_ID");

            entity.HasOne(d => d.FCouponCode).WithMany(p => p.TCouponRedemptions)
                .HasForeignKey(d => d.FCouponCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tCoupon_Redemption_tCoupon");

            entity.HasOne(d => d.FUser).WithMany(p => p.TCouponRedemptions)
                .HasForeignKey(d => d.FUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tCoupon_Redemption_tUser");
        });

        modelBuilder.Entity<TFriend>(entity =>
        {
            entity.HasKey(e => e.FInvitationId);

            entity.ToTable("tFriends");

            entity.Property(e => e.FInvitationId).HasColumnName("fInvitation_Id");
            entity.Property(e => e.FFriendUserId).HasColumnName("fFriendUserId");
            entity.Property(e => e.FInvitationTime)
                .HasColumnType("datetime")
                .HasColumnName("fInvitationTime");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FFriendUser).WithMany(p => p.TFriendFFriendUsers)
                .HasForeignKey(d => d.FFriendUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tFriends_tUser1");

            entity.HasOne(d => d.FUser).WithMany(p => p.TFriendFUsers)
                .HasForeignKey(d => d.FUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tFriends_tUser");
        });

        modelBuilder.Entity<TLoginRecord>(entity =>
        {
            entity.HasKey(e => e.FLoginRecordId);

            entity.ToTable("tLoginRecord");

            entity.Property(e => e.FLoginRecordId).HasColumnName("fLoginRecord_Id");
            entity.Property(e => e.FLoginIp)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fLoginIp");
            entity.Property(e => e.FLoginState).HasColumnName("fLoginState");
            entity.Property(e => e.FLoginTime)
                .HasColumnType("datetime")
                .HasColumnName("fLoginTime");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FUser).WithMany(p => p.TLoginRecords)
                .HasForeignKey(d => d.FUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tLoginRecord_tUser");
        });

        modelBuilder.Entity<TMemberChat>(entity =>
        {
            entity.HasKey(e => e.FChatId).HasName("PK_MemberChat");

            entity.ToTable("tMemberChat");

            entity.Property(e => e.FChatId).HasColumnName("fChatId");
            entity.Property(e => e.FChatImageId).HasColumnName("fChatImageId");
            entity.Property(e => e.FMessage)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("fMessage");
            entity.Property(e => e.FReceiverId).HasColumnName("fReceiverId");
            entity.Property(e => e.FReport).HasColumnName("fReport");
            entity.Property(e => e.FSendTime)
                .HasColumnType("datetime")
                .HasColumnName("fSendTime");
            entity.Property(e => e.FSenderId).HasColumnName("fSenderId");

            entity.HasOne(d => d.FChatImage).WithMany(p => p.TMemberChats)
                .HasForeignKey(d => d.FChatImageId)
                .HasConstraintName("FK_tMemberChat_tChatImages1");

            entity.HasOne(d => d.FReceiver).WithMany(p => p.TMemberChatFReceivers)
                .HasForeignKey(d => d.FReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tMemberChat_tUsers1");

            entity.HasOne(d => d.FSender).WithMany(p => p.TMemberChatFSenders)
                .HasForeignKey(d => d.FSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tMemberChat_tUsers");
        });

        modelBuilder.Entity<TNotifyType>(entity =>
        {
            entity.HasKey(e => e.FNotifyTypeId);

            entity.ToTable("tNotifyType");

            entity.Property(e => e.FNotifyTypeId).HasColumnName("fNotifyType_Id");
            entity.Property(e => e.FNotifyType)
                .HasMaxLength(15)
                .HasColumnName("fNotifyType");
        });

        modelBuilder.Entity<TOrderChangeReason>(entity =>
        {
            entity.HasKey(e => e.FOrderChangeReasonId);

            entity.ToTable("tOrderChangeReason");

            entity.Property(e => e.FOrderChangeReasonId).HasColumnName("fOrderChangeReason_Id");
            entity.Property(e => e.FOrderChangeReason)
                .HasMaxLength(50)
                .HasColumnName("fOrderChangeReason");
        });

        modelBuilder.Entity<TPart>(entity =>
        {
            entity.HasKey(e => e.FPartId);

            entity.ToTable("tPart");

            entity.Property(e => e.FPartId).HasColumnName("fPart_Id");
            entity.Property(e => e.FPartName)
                .HasMaxLength(15)
                .HasColumnName("fPartName");
        });

        modelBuilder.Entity<TPayMethod>(entity =>
        {
            entity.HasKey(e => e.FPayMethodId);

            entity.ToTable("tPayMethod");

            entity.Property(e => e.FPayMethodId).HasColumnName("fPayMethod_Id");
            entity.Property(e => e.FPayMethod)
                .HasMaxLength(50)
                .HasColumnName("fPayMethod");
        });

        modelBuilder.Entity<TRtwcomment>(entity =>
        {
            entity.HasKey(e => e.FCommentId).HasName("PK_tCommand");

            entity.ToTable("tRTWComment");

            entity.Property(e => e.FCommentId).HasColumnName("fComment_Id");
            entity.Property(e => e.AnonymousUser).HasColumnName("anonymousUser");
            entity.Property(e => e.FComment)
                .HasMaxLength(100)
                .HasColumnName("fComment");
            entity.Property(e => e.FCommentDate).HasColumnName("fCommentDate");
            entity.Property(e => e.FSellerId).HasColumnName("fSeller_Id");
            entity.Property(e => e.FScore).HasColumnName("fScore");
            entity.Property(e => e.FSellerReply)
                .HasMaxLength(100)
                .HasColumnName("fSellerReply");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            //這裡要更改
            entity.HasOne(d => d.FUser).WithMany(p => p.TRtwcomments)
              .HasForeignKey(d => d.FUserId)
              .HasConstraintName("FK_tRTWComment_tUsers2");
        });

        modelBuilder.Entity<TRtwnotify>(entity =>
        {
            entity.HasKey(e => e.FNotifyId);

            entity.ToTable("tRTWNotify");

            entity.Property(e => e.FNotifyId).HasColumnName("fNotify_Id");
            entity.Property(e => e.FIsNotRead)
                .HasDefaultValue(false)
                .HasColumnName("fIsNotRead");
            entity.Property(e => e.FNotify)
                .HasMaxLength(50)
                .HasColumnName("fNotify");
            entity.Property(e => e.FNotifyState)
                .HasDefaultValue(false)
                .HasColumnName("fNotifyState");
            entity.Property(e => e.FNotifyTypeId).HasColumnName("fNotifyType_Id");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FNotifyType).WithMany(p => p.TRtwnotifies)
                .HasForeignKey(d => d.FNotifyTypeId)
                .HasConstraintName("FK_tRTWNotify_tNotifyType");

            entity.HasOne(d => d.FUser).WithMany(p => p.TRtwnotifies)
                .HasForeignKey(d => d.FUserId)
                .HasConstraintName("FK_tRTWNotify_tUser");
        });

        modelBuilder.Entity<TRtworder>(entity =>
        {
            entity.HasKey(e => e.FOrderId).HasName("PK_tOrder");

            entity.ToTable("tRTWOrder");

            entity.Property(e => e.FOrderId).HasColumnName("fOrder_Id");
            entity.Property(e => e.FAddress)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fAddress");
            entity.Property(e => e.FCancel).HasColumnName("fCancel");
            entity.Property(e => e.FCouponCodeId).HasColumnName("fCoupon_Code_Id");
            entity.Property(e => e.FFinished).HasColumnName("fFinished");
            entity.Property(e => e.FFinishedTime)
                .HasColumnType("datetime")
                .HasColumnName("fFinishedTime");
            entity.Property(e => e.FOrderChangeReasonId).HasColumnName("fOrderChangeReason_Id");
            entity.Property(e => e.FOrderNumber)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fOrder_Number");
            entity.Property(e => e.FPayMethodId).HasColumnName("fPayMethod_Id");
            entity.Property(e => e.FPrice)
                .HasColumnType("money")
                .HasColumnName("fPrice");
            entity.Property(e => e.FShipMethodId).HasColumnName("fShipMethod_Id");
            entity.Property(e => e.FShipStateId).HasColumnName("fShipState_Id");
            entity.Property(e => e.FTime)
                .HasColumnType("datetime")
                .HasColumnName("fTime");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FCouponCode).WithMany(p => p.TRtworders)
                .HasForeignKey(d => d.FCouponCodeId)
                .HasConstraintName("FK_tRTWOrder_tCoupon");

            entity.HasOne(d => d.FOrderChangeReason).WithMany(p => p.TRtworders)
                .HasForeignKey(d => d.FOrderChangeReasonId)
                .HasConstraintName("FK_tRTWOrder_tOrderChangeReason");

            entity.HasOne(d => d.FPayMethod).WithMany(p => p.TRtworders)
                .HasForeignKey(d => d.FPayMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRTWOrder_tPayMethod");

            entity.HasOne(d => d.FShipMethod).WithMany(p => p.TRtworders)
                .HasForeignKey(d => d.FShipMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRTWOrder_tShipMethod");

            entity.HasOne(d => d.FShipState).WithMany(p => p.TRtworders)
                .HasForeignKey(d => d.FShipStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRTWOrder_tShipState");

            entity.HasOne(d => d.FUser).WithMany(p => p.TRtworders)
                .HasForeignKey(d => d.FUserId)
                .HasConstraintName("FK_tRTWOrder_tUser1");
        });

        modelBuilder.Entity<TRtworderDetail>(entity =>
        {
            entity.HasKey(e => e.FOrderDetailId).HasName("PK_tOrderDetail");

            entity.ToTable("tRTWOrderDetail");

            entity.Property(e => e.FOrderDetailId).HasColumnName("fOrderDetail_Id");
            entity.Property(e => e.FOrderId).HasColumnName("fOrder_Id");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductDetailId).HasColumnName("fProductDetail_Id");
            entity.Property(e => e.FQty).HasColumnName("fQTY");

            entity.HasOne(d => d.FOrder).WithMany(p => p.TRtworderDetails)
                .HasForeignKey(d => d.FOrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tRTWOrderDetail_tRTWOrder");

            entity.HasOne(d => d.FProductDetail).WithMany(p => p.TRtworderDetails)
                .HasForeignKey(d => d.FProductDetailId)
                .HasConstraintName("FK_tRTWOrderDetail_tRTWProductDetail");
        });

        modelBuilder.Entity<TRtwproduct>(entity =>
        {
            entity.HasKey(e => e.FProductId);

            entity.ToTable("tRTWProduct");

            entity.Property(e => e.FProductId).HasColumnName("fProduct_Id");
            entity.Property(e => e.FConditionsId).HasColumnName("fConditionsId");
            entity.Property(e => e.FGender)
                .HasMaxLength(50)
                .HasColumnName("fGender");
            entity.Property(e => e.FOriginPrice)
                .HasColumnType("money")
                .HasColumnName("fOriginPrice");
            entity.Property(e => e.FPartId).HasColumnName("fPart_Id");
            entity.Property(e => e.FProductIllustrate)
                .HasMaxLength(500)
                .HasColumnName("fProductIllustrate");
            entity.Property(e => e.FProductName)
                .HasMaxLength(50)
                .HasColumnName("fProductName");
            entity.Property(e => e.FTagId).HasColumnName("fTag_Id");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");
            entity.Property(e => e.Fstate).HasColumnName("fstate");

            entity.HasOne(d => d.FConditions).WithMany(p => p.TRtwproducts)
                .HasForeignKey(d => d.FConditionsId)
                .HasConstraintName("FK_tRTWProduct_tConditions");

            entity.HasOne(d => d.FPart).WithMany(p => p.TRtwproducts)
                .HasForeignKey(d => d.FPartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRTWProduct_tPart");

            entity.HasOne(d => d.FTag).WithMany(p => p.TRtwproducts)
                .HasForeignKey(d => d.FTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRTWProduct_tTag");

            entity.HasOne(d => d.FUser).WithMany(p => p.TRtwproducts)
                .HasForeignKey(d => d.FUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tRTWProduct_tUsers");
        });

        modelBuilder.Entity<TRtwproductDetail>(entity =>
        {
            entity.HasKey(e => e.FProductDetailId);

            entity.ToTable("tRTWProductDetail");

            entity.Property(e => e.FProductDetailId).HasColumnName("fProductDetail_Id");
            entity.Property(e => e.FColorId).HasColumnName("fColorId");
            entity.Property(e => e.FProductId).HasColumnName("fProduct_Id");
            entity.Property(e => e.FProductImageId).HasColumnName("fProductImage_Id");
            entity.Property(e => e.FQty)
                .HasDefaultValue(0)
                .HasColumnName("fQTY");
            entity.Property(e => e.FSizeId).HasColumnName("fSizeId");

            entity.HasOne(d => d.FColor).WithMany(p => p.TRtwproductDetails)
                .HasForeignKey(d => d.FColorId)
                .HasConstraintName("FK_tRTWProductDetail_tColor");

            entity.HasOne(d => d.FProduct).WithMany(p => p.TRtwproductDetails)
                .HasForeignKey(d => d.FProductId)
                .HasConstraintName("FK_tRTWProductDetail_tRTWProduct");

            entity.HasOne(d => d.FProductImage).WithMany(p => p.TRtwproductDetails)
                .HasForeignKey(d => d.FProductImageId)
                .HasConstraintName("FK_tRTWProductDetail_tRTWProductImage");

            entity.HasOne(d => d.FSize).WithMany(p => p.TRtwproductDetails)
                .HasForeignKey(d => d.FSizeId)
                .HasConstraintName("FK_tRTWProductDetail_tSizes");
        });

        modelBuilder.Entity<TRtwproductImage>(entity =>
        {
            entity.HasKey(e => e.FProductImageId).HasName("PK_tProductImage");

            entity.ToTable("tRTWProductImage");

            entity.Property(e => e.FProductImageId).HasColumnName("fProductImage_Id");
            entity.Property(e => e.FProductId).HasColumnName("fProduct_Id");
            entity.Property(e => e.FProductImage).HasColumnName("fProductImage");
        });

        modelBuilder.Entity<TRtwshoppingCart>(entity =>
        {
            entity.HasKey(e => e.FShoppingCartId);

            entity.ToTable("tRTWShoppingCart");

            entity.Property(e => e.FShoppingCartId).HasColumnName("fShoppingCart_Id");
            entity.Property(e => e.FAddTime).HasColumnName("fAddTime");
            entity.Property(e => e.FProductDetailId).HasColumnName("fProductDetail_Id");
            entity.Property(e => e.FQty)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("fQTY");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FProductDetail).WithMany(p => p.TRtwshoppingCarts)
                .HasForeignKey(d => d.FProductDetailId)
                .HasConstraintName("FK_tRTWShoppingCart_tRTWProductDetail");

            entity.HasOne(d => d.FUser).WithMany(p => p.TRtwshoppingCarts)
                .HasForeignKey(d => d.FUserId)
                .HasConstraintName("FK_tRTWShoppingCart_tUser");
        });

        modelBuilder.Entity<TSendNotify>(entity =>
        {
            entity.HasKey(e => e.FSendNotifyId);

            entity.ToTable("tSendNotify");

            entity.Property(e => e.FSendNotifyId).HasColumnName("fSendNotify_Id");
            entity.Property(e => e.FIsNotRead)
                .HasDefaultValue(false)
                .HasColumnName("fIsNotRead");
            entity.Property(e => e.FNotify)
                .HasMaxLength(50)
                .HasColumnName("fNotify");
            entity.Property(e => e.FNotifyId).HasColumnName("fNotify_Id");
            entity.Property(e => e.FSendTime)
                .HasColumnType("datetime")
                .HasColumnName("fSendTime");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FNotifyNavigation).WithMany(p => p.TSendNotifies)
                .HasForeignKey(d => d.FNotifyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tSendNotify_tRTWNotify");
        });

        modelBuilder.Entity<TShipMethod>(entity =>
        {
            entity.HasKey(e => e.FShipMethodId);

            entity.ToTable("tShipMethod");

            entity.Property(e => e.FShipMethodId).HasColumnName("fShipMethod_Id");
            entity.Property(e => e.FNeedTime)
                .HasMaxLength(20)
                .HasColumnName("fNeedTime");
            entity.Property(e => e.FShipMethod)
                .HasMaxLength(50)
                .HasColumnName("fShipMethod");
        });

        modelBuilder.Entity<TShipState>(entity =>
        {
            entity.HasKey(e => e.FShipStateId);

            entity.ToTable("tShipState");

            entity.Property(e => e.FShipStateId).HasColumnName("fShipState_Id");
            entity.Property(e => e.FShipState)
                .HasMaxLength(20)
                .HasColumnName("fShipState");
        });

        modelBuilder.Entity<TSize>(entity =>
        {
            entity.HasKey(e => e.FSizeId);

            entity.ToTable("tSizes");

            entity.Property(e => e.FSizeId).HasColumnName("fSizeId");
            entity.Property(e => e.FSizeName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fSizeName");
        });

        modelBuilder.Entity<TTag>(entity =>
        {
            entity.HasKey(e => e.FTagId);

            entity.ToTable("tTag");

            entity.Property(e => e.FTagId).HasColumnName("fTag_Id");
            entity.Property(e => e.FPartId).HasColumnName("fPart_Id");
            entity.Property(e => e.FTagName)
                .HasMaxLength(20)
                .HasColumnName("fTagName");

            entity.HasOne(d => d.FPart).WithMany(p => p.TTags)
                .HasForeignKey(d => d.FPartId)
                .HasConstraintName("FK_tTag_tPart");
        });

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(e => e.FUserId).HasName("PK_tUser");

            entity.ToTable("tUsers");

            entity.Property(e => e.FUserId).HasColumnName("fUserId");
            entity.Property(e => e.FAddress)
                .HasMaxLength(70)
                .HasColumnName("fAddress");
            entity.Property(e => e.FBirthDate).HasColumnName("fBirthDate");
            entity.Property(e => e.FGender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fGender");
            entity.Property(e => e.FInvitationCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fInvitationCode");
            entity.Property(e => e.FName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("fName");
            entity.Property(e => e.FProfileImage).HasColumnName("fProfileImage");
            entity.Property(e => e.FRegistrationTime)
                .HasColumnType("datetime")
                .HasColumnName("fRegistrationTime");
            entity.Property(e => e.FUpdatedTime)
                .HasColumnType("datetime")
                .HasColumnName("fUpdatedTime");
            entity.Property(e => e.FUserIsSeller).HasColumnName("fUserIsSeller");
            entity.Property(e => e.FUserLevel).HasColumnName("fUserLevel");
        });

        modelBuilder.Entity<TUserTag>(entity =>
        {
            entity.HasKey(e => e.FUserTagId);

            entity.ToTable("tUserTag");

            entity.Property(e => e.FUserTagId).HasColumnName("fUserTag_Id");
            entity.Property(e => e.FTagId).HasColumnName("fTagId");
            entity.Property(e => e.FTagNotification).HasColumnName("fTagNotification");
            entity.Property(e => e.FUserId).HasColumnName("fUserId");

            entity.HasOne(d => d.FTag).WithMany(p => p.TUserTags)
                .HasForeignKey(d => d.FTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tUserTag_tTag");

            entity.HasOne(d => d.FUser).WithMany(p => p.TUserTags)
                .HasForeignKey(d => d.FUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tUserTag_tUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}