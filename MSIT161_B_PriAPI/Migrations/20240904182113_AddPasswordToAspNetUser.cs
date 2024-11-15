using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSIT161_B_PriAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToAspNetUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tColor",
                columns: table => new
                {
                    fColorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fColorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tColor", x => x.fColorId);
                });

            migrationBuilder.CreateTable(
                name: "tConditions",
                columns: table => new
                {
                    fConditionsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fConditionsName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tConditions", x => x.fConditionsId);
                });

            migrationBuilder.CreateTable(
                name: "tNotifyType",
                columns: table => new
                {
                    fNotifyType_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fNotifyType = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tNotifyType", x => x.fNotifyType_Id);
                });

            migrationBuilder.CreateTable(
                name: "tOrderChangeReason",
                columns: table => new
                {
                    fOrderChangeReason_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fOrderChangeReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tOrderChangeReason", x => x.fOrderChangeReason_Id);
                });

            migrationBuilder.CreateTable(
                name: "tPart",
                columns: table => new
                {
                    fPart_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fPartName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPart", x => x.fPart_Id);
                });

            migrationBuilder.CreateTable(
                name: "tPayMethod",
                columns: table => new
                {
                    fPayMethod_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fPayMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPayMethod", x => x.fPayMethod_Id);
                });

            migrationBuilder.CreateTable(
                name: "tRTWProductImage",
                columns: table => new
                {
                    fProductImage_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fProduct_Id = table.Column<int>(type: "int", nullable: true),
                    fProductImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tProductImage", x => x.fProductImage_Id);
                });

            migrationBuilder.CreateTable(
                name: "tShipMethod",
                columns: table => new
                {
                    fShipMethod_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fShipMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fNeedTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tShipMethod", x => x.fShipMethod_Id);
                });

            migrationBuilder.CreateTable(
                name: "tShipState",
                columns: table => new
                {
                    fShipState_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fShipState = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tShipState", x => x.fShipState_Id);
                });

            migrationBuilder.CreateTable(
                name: "tSizes",
                columns: table => new
                {
                    fSizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fSizeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tSizes", x => x.fSizeId);
                });

            migrationBuilder.CreateTable(
                name: "tUsers",
                columns: table => new
                {
                    fUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fGender = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fBirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fAddress = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    fUserLevel = table.Column<int>(type: "int", nullable: false),
                    fInvitationCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fRegistrationTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    fUpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    fUserIsSeller = table.Column<byte>(type: "tinyint", nullable: false),
                    fProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tUser", x => x.fUserId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tTag",
                columns: table => new
                {
                    fTag_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fTagName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fPart_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tTag", x => x.fTag_Id);
                    table.ForeignKey(
                        name: "FK_tTag_tPart",
                        column: x => x.fPart_Id,
                        principalTable: "tPart",
                        principalColumn: "fPart_Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BfUserId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_tUsers",
                        column: x => x.BfUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tCoupon",
                columns: table => new
                {
                    fCoupon_Code_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fCoupon_Code_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    coupon_description = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true),
                    fCoupon_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fCoupon_Discount = table.Column<double>(type: "float", nullable: false),
                    fCoupon_Creatday = table.Column<DateTime>(type: "datetime", nullable: false),
                    fCoupon_endday = table.Column<DateTime>(type: "datetime", nullable: true),
                    fCoupon_state = table.Column<bool>(type: "bit", nullable: true),
                    fCoupon_from = table.Column<int>(type: "int", nullable: false),
                    minSellMoney = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.fCoupon_Code_ID);
                    table.ForeignKey(
                        name: "FK_tCoupon_tUsers",
                        column: x => x.fCoupon_from,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tFriends",
                columns: table => new
                {
                    fInvitation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fUserId = table.Column<int>(type: "int", nullable: false),
                    fFriendUserId = table.Column<int>(type: "int", nullable: false),
                    fInvitationTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tFriends", x => x.fInvitation_Id);
                    table.ForeignKey(
                        name: "FK_tFriends_tUser",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                    table.ForeignKey(
                        name: "FK_tFriends_tUser1",
                        column: x => x.fFriendUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tLoginRecord",
                columns: table => new
                {
                    fLoginRecord_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fUserId = table.Column<int>(type: "int", nullable: false),
                    fLoginIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fLoginTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    fLoginState = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tLoginRecord", x => x.fLoginRecord_Id);
                    table.ForeignKey(
                        name: "FK_tLoginRecord_tUser",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tRTWNotify",
                columns: table => new
                {
                    fNotify_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fUserId = table.Column<int>(type: "int", nullable: true),
                    fNotify = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fIsNotRead = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    fNotifyType_Id = table.Column<int>(type: "int", nullable: true),
                    fNotifyState = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tRTWNotify", x => x.fNotify_Id);
                    table.ForeignKey(
                        name: "FK_tRTWNotify_tNotifyType",
                        column: x => x.fNotifyType_Id,
                        principalTable: "tNotifyType",
                        principalColumn: "fNotifyType_Id");
                    table.ForeignKey(
                        name: "FK_tRTWNotify_tUser",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tRTWProduct",
                columns: table => new
                {
                    fProduct_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fGender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fPart_Id = table.Column<int>(type: "int", nullable: false),
                    fTag_Id = table.Column<int>(type: "int", nullable: false),
                    fConditionsId = table.Column<int>(type: "int", nullable: true),
                    fProductIllustrate = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fOriginPrice = table.Column<decimal>(type: "money", nullable: false),
                    fUserId = table.Column<int>(type: "int", nullable: false),
                    fstate = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tRTWProduct", x => x.fProduct_Id);
                    table.ForeignKey(
                        name: "FK_tRTWProduct_tConditions",
                        column: x => x.fConditionsId,
                        principalTable: "tConditions",
                        principalColumn: "fConditionsId");
                    table.ForeignKey(
                        name: "FK_tRTWProduct_tPart",
                        column: x => x.fPart_Id,
                        principalTable: "tPart",
                        principalColumn: "fPart_Id");
                    table.ForeignKey(
                        name: "FK_tRTWProduct_tTag",
                        column: x => x.fTag_Id,
                        principalTable: "tTag",
                        principalColumn: "fTag_Id");
                    table.ForeignKey(
                        name: "FK_tRTWProduct_tUsers",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tUserTag",
                columns: table => new
                {
                    fUserTag_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fUserId = table.Column<int>(type: "int", nullable: false),
                    fTagId = table.Column<int>(type: "int", nullable: false),
                    fTagNotification = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tUserTag", x => x.fUserTag_Id);
                    table.ForeignKey(
                        name: "FK_tUserTag_tTag",
                        column: x => x.fTagId,
                        principalTable: "tTag",
                        principalColumn: "fTag_Id");
                    table.ForeignKey(
                        name: "FK_tUserTag_tUser",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tCoupon_Redemption",
                columns: table => new
                {
                    fCoupon_Redemption_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fCoupon_Code_ID = table.Column<int>(type: "int", nullable: false),
                    fUser_ID = table.Column<int>(type: "int", nullable: false),
                    fCoupon_Get_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    fCoupon_Use_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    fCoupon_UseState = table.Column<bool>(type: "bit", nullable: true),
                    fCoupon_Usage_Count = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tCoupon_Redemption", x => x.fCoupon_Redemption_ID);
                    table.ForeignKey(
                        name: "FK_tCoupon_Redemption_tCoupon",
                        column: x => x.fCoupon_Code_ID,
                        principalTable: "tCoupon",
                        principalColumn: "fCoupon_Code_ID");
                    table.ForeignKey(
                        name: "FK_tCoupon_Redemption_tUser",
                        column: x => x.fUser_ID,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tRTWOrder",
                columns: table => new
                {
                    fOrder_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fOrder_Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fUserId = table.Column<int>(type: "int", nullable: true),
                    fCoupon_Code_Id = table.Column<int>(type: "int", nullable: true),
                    fPrice = table.Column<decimal>(type: "money", nullable: true),
                    fTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    fShipMethod_Id = table.Column<int>(type: "int", nullable: false),
                    fPayMethod_Id = table.Column<int>(type: "int", nullable: false),
                    fAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fShipState_Id = table.Column<int>(type: "int", nullable: false),
                    fFinished = table.Column<bool>(type: "bit", nullable: false),
                    fOrderChangeReason_Id = table.Column<int>(type: "int", nullable: true),
                    fFinishedTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    fCancel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tOrder", x => x.fOrder_Id);
                    table.ForeignKey(
                        name: "FK_tRTWOrder_tCoupon",
                        column: x => x.fCoupon_Code_Id,
                        principalTable: "tCoupon",
                        principalColumn: "fCoupon_Code_ID");
                    table.ForeignKey(
                        name: "FK_tRTWOrder_tOrderChangeReason",
                        column: x => x.fOrderChangeReason_Id,
                        principalTable: "tOrderChangeReason",
                        principalColumn: "fOrderChangeReason_Id");
                    table.ForeignKey(
                        name: "FK_tRTWOrder_tPayMethod",
                        column: x => x.fPayMethod_Id,
                        principalTable: "tPayMethod",
                        principalColumn: "fPayMethod_Id");
                    table.ForeignKey(
                        name: "FK_tRTWOrder_tShipMethod",
                        column: x => x.fShipMethod_Id,
                        principalTable: "tShipMethod",
                        principalColumn: "fShipMethod_Id");
                    table.ForeignKey(
                        name: "FK_tRTWOrder_tShipState",
                        column: x => x.fShipState_Id,
                        principalTable: "tShipState",
                        principalColumn: "fShipState_Id");
                    table.ForeignKey(
                        name: "FK_tRTWOrder_tUser1",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tRTWComment",
                columns: table => new
                {
                    fComment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fProduct_Id = table.Column<int>(type: "int", nullable: true),
                    fUserId = table.Column<int>(type: "int", nullable: true),
                    fScore = table.Column<double>(type: "float", nullable: true),
                    fComment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fCommentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    anonymousUser = table.Column<bool>(type: "bit", nullable: true),
                    fSellerReply = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tCommand", x => x.fComment_Id);
                    table.ForeignKey(
                        name: "FK_tRTWCommand_tRTWProduct",
                        column: x => x.fProduct_Id,
                        principalTable: "tRTWProduct",
                        principalColumn: "fProduct_Id");
                    table.ForeignKey(
                        name: "FK_tRTWComment_tUsers",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tRTWProductDetail",
                columns: table => new
                {
                    fProductDetail_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fProduct_Id = table.Column<int>(type: "int", nullable: true),
                    fColorId = table.Column<int>(type: "int", nullable: true),
                    fSizeId = table.Column<int>(type: "int", nullable: true),
                    fQTY = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    fProductImage_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tRTWProductDetail", x => x.fProductDetail_Id);
                    table.ForeignKey(
                        name: "FK_tRTWProductDetail_tColor",
                        column: x => x.fColorId,
                        principalTable: "tColor",
                        principalColumn: "fColorId");
                    table.ForeignKey(
                        name: "FK_tRTWProductDetail_tRTWProduct",
                        column: x => x.fProduct_Id,
                        principalTable: "tRTWProduct",
                        principalColumn: "fProduct_Id");
                    table.ForeignKey(
                        name: "FK_tRTWProductDetail_tRTWProductImage",
                        column: x => x.fProductImage_Id,
                        principalTable: "tRTWProductImage",
                        principalColumn: "fProductImage_Id");
                    table.ForeignKey(
                        name: "FK_tRTWProductDetail_tSizes",
                        column: x => x.fSizeId,
                        principalTable: "tSizes",
                        principalColumn: "fSizeId");
                });

            migrationBuilder.CreateTable(
                name: "tRTWOrderDetail",
                columns: table => new
                {
                    fOrderDetail_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fOrder_Id = table.Column<int>(type: "int", nullable: true),
                    fProductDetail_Id = table.Column<int>(type: "int", nullable: true),
                    fQTY = table.Column<int>(type: "int", nullable: true),
                    fPrice = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tOrderDetail", x => x.fOrderDetail_Id);
                    table.ForeignKey(
                        name: "FK_tRTWOrderDetail_tRTWOrder",
                        column: x => x.fOrder_Id,
                        principalTable: "tRTWOrder",
                        principalColumn: "fOrder_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tRTWOrderDetail_tRTWProductDetail",
                        column: x => x.fProductDetail_Id,
                        principalTable: "tRTWProductDetail",
                        principalColumn: "fProductDetail_Id");
                });

            migrationBuilder.CreateTable(
                name: "tRTWShoppingCart",
                columns: table => new
                {
                    fShoppingCart_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fUserId = table.Column<int>(type: "int", nullable: true),
                    fProductDetail_Id = table.Column<int>(type: "int", nullable: true),
                    fQTY = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    fAddTime = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tRTWShoppingCart", x => x.fShoppingCart_Id);
                    table.ForeignKey(
                        name: "FK_tRTWShoppingCart_tRTWProductDetail",
                        column: x => x.fProductDetail_Id,
                        principalTable: "tRTWProductDetail",
                        principalColumn: "fProductDetail_Id");
                    table.ForeignKey(
                        name: "FK_tRTWShoppingCart_tUser",
                        column: x => x.fUserId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateTable(
                name: "tChatImages",
                columns: table => new
                {
                    fChatImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fChatId = table.Column<int>(type: "int", nullable: false),
                    fImageData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatImages", x => x.fChatImageId);
                });

            migrationBuilder.CreateTable(
                name: "tMemberChat",
                columns: table => new
                {
                    fChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fSenderId = table.Column<int>(type: "int", nullable: false),
                    fReceiverId = table.Column<int>(type: "int", nullable: false),
                    fChatImageId = table.Column<int>(type: "int", nullable: true),
                    fSendTime = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    fMessage = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberChat", x => x.fChatId);
                    table.ForeignKey(
                        name: "FK_tMemberChat_tChatImages1",
                        column: x => x.fChatImageId,
                        principalTable: "tChatImages",
                        principalColumn: "fChatImageId");
                    table.ForeignKey(
                        name: "FK_tMemberChat_tUsers",
                        column: x => x.fSenderId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                    table.ForeignKey(
                        name: "FK_tMemberChat_tUsers1",
                        column: x => x.fReceiverId,
                        principalTable: "tUsers",
                        principalColumn: "fUserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BfUserId",
                table: "AspNetUsers",
                column: "BfUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tChatImages_fChatId",
                table: "tChatImages",
                column: "fChatId");

            migrationBuilder.CreateIndex(
                name: "IX_tCoupon_fCoupon_from",
                table: "tCoupon",
                column: "fCoupon_from");

            migrationBuilder.CreateIndex(
                name: "IX_tCoupon_Redemption_fCoupon_Code_ID",
                table: "tCoupon_Redemption",
                column: "fCoupon_Code_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tCoupon_Redemption_fUser_ID",
                table: "tCoupon_Redemption",
                column: "fUser_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tFriends_fFriendUserId",
                table: "tFriends",
                column: "fFriendUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tFriends_fUserId",
                table: "tFriends",
                column: "fUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tLoginRecord_fUserId",
                table: "tLoginRecord",
                column: "fUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tMemberChat_fChatImageId",
                table: "tMemberChat",
                column: "fChatImageId");

            migrationBuilder.CreateIndex(
                name: "IX_tMemberChat_fReceiverId",
                table: "tMemberChat",
                column: "fReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_tMemberChat_fSenderId",
                table: "tMemberChat",
                column: "fSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWComment_fProduct_Id",
                table: "tRTWComment",
                column: "fProduct_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWComment_fUserId",
                table: "tRTWComment",
                column: "fUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWNotify_fNotifyType_Id",
                table: "tRTWNotify",
                column: "fNotifyType_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWNotify_fUserId",
                table: "tRTWNotify",
                column: "fUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrder_fCoupon_Code_Id",
                table: "tRTWOrder",
                column: "fCoupon_Code_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrder_fOrderChangeReason_Id",
                table: "tRTWOrder",
                column: "fOrderChangeReason_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrder_fPayMethod_Id",
                table: "tRTWOrder",
                column: "fPayMethod_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrder_fShipMethod_Id",
                table: "tRTWOrder",
                column: "fShipMethod_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrder_fShipState_Id",
                table: "tRTWOrder",
                column: "fShipState_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrder_fUserId",
                table: "tRTWOrder",
                column: "fUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrderDetail_fOrder_Id",
                table: "tRTWOrderDetail",
                column: "fOrder_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWOrderDetail_fProductDetail_Id",
                table: "tRTWOrderDetail",
                column: "fProductDetail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProduct_fConditionsId",
                table: "tRTWProduct",
                column: "fConditionsId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProduct_fPart_Id",
                table: "tRTWProduct",
                column: "fPart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProduct_fTag_Id",
                table: "tRTWProduct",
                column: "fTag_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProduct_fUserId",
                table: "tRTWProduct",
                column: "fUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProductDetail_fColorId",
                table: "tRTWProductDetail",
                column: "fColorId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProductDetail_fProduct_Id",
                table: "tRTWProductDetail",
                column: "fProduct_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProductDetail_fProductImage_Id",
                table: "tRTWProductDetail",
                column: "fProductImage_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWProductDetail_fSizeId",
                table: "tRTWProductDetail",
                column: "fSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWShoppingCart_fProductDetail_Id",
                table: "tRTWShoppingCart",
                column: "fProductDetail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tRTWShoppingCart_fUserId",
                table: "tRTWShoppingCart",
                column: "fUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tTag_fPart_Id",
                table: "tTag",
                column: "fPart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tUserTag_fTagId",
                table: "tUserTag",
                column: "fTagId");

            migrationBuilder.CreateIndex(
                name: "IX_tUserTag_fUserId",
                table: "tUserTag",
                column: "fUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tChatImages_tMemberChat1",
                table: "tChatImages",
                column: "fChatId",
                principalTable: "tMemberChat",
                principalColumn: "fChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tMemberChat_tUsers",
                table: "tMemberChat");

            migrationBuilder.DropForeignKey(
                name: "FK_tMemberChat_tUsers1",
                table: "tMemberChat");

            migrationBuilder.DropForeignKey(
                name: "FK_tChatImages_tMemberChat1",
                table: "tChatImages");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "tCoupon_Redemption");

            migrationBuilder.DropTable(
                name: "tFriends");

            migrationBuilder.DropTable(
                name: "tLoginRecord");

            migrationBuilder.DropTable(
                name: "tRTWComment");

            migrationBuilder.DropTable(
                name: "tRTWNotify");

            migrationBuilder.DropTable(
                name: "tRTWOrderDetail");

            migrationBuilder.DropTable(
                name: "tRTWShoppingCart");

            migrationBuilder.DropTable(
                name: "tUserTag");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "tNotifyType");

            migrationBuilder.DropTable(
                name: "tRTWOrder");

            migrationBuilder.DropTable(
                name: "tRTWProductDetail");

            migrationBuilder.DropTable(
                name: "tCoupon");

            migrationBuilder.DropTable(
                name: "tOrderChangeReason");

            migrationBuilder.DropTable(
                name: "tPayMethod");

            migrationBuilder.DropTable(
                name: "tShipMethod");

            migrationBuilder.DropTable(
                name: "tShipState");

            migrationBuilder.DropTable(
                name: "tColor");

            migrationBuilder.DropTable(
                name: "tRTWProduct");

            migrationBuilder.DropTable(
                name: "tRTWProductImage");

            migrationBuilder.DropTable(
                name: "tSizes");

            migrationBuilder.DropTable(
                name: "tConditions");

            migrationBuilder.DropTable(
                name: "tTag");

            migrationBuilder.DropTable(
                name: "tPart");

            migrationBuilder.DropTable(
                name: "tUsers");

            migrationBuilder.DropTable(
                name: "tMemberChat");

            migrationBuilder.DropTable(
                name: "tChatImages");
        }
    }
}
