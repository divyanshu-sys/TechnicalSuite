using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ts.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class One : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(3)", fixedLength: true, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPolicies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryInDays = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangePolicies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExchangeInDays = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangePolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NextUserSettings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    NextUserNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextUserSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentGatewayTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGatewayTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentModes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnPolicies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnInDays = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
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
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PhoneCode = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    ProfileImageName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    ChangePassword = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CanLogin = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
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
                        name: "FK_AspNetUsers_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalSchema: "dbo",
                        principalTable: "Genders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentGatewayTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentStatuses_PaymentGatewayTypes_PaymentGatewayTypeId",
                        column: x => x.PaymentGatewayTypeId,
                        principalSchema: "dbo",
                        principalTable: "PaymentGatewayTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(250)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(250)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(250)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Code2 = table.Column<string>(type: "varchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    Code3 = table.Column<string>(type: "varchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Countries_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    UserId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    LoggedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    UserId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    RefreshReloginId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopCategories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShopCategories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubCategories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Code2 = table.Column<string>(type: "varchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_States_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "dbo",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Districts_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Districts_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "dbo",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostOffices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Pincode = table.Column<string>(type: "varchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostOffices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostOffices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostOffices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostOffices_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "dbo",
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Address1 = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false),
                    Address2 = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: true),
                    PostOfficeId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_PostOffices_PostOfficeId",
                        column: x => x.PostOfficeId,
                        principalSchema: "dbo",
                        principalTable: "PostOffices",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsActive", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f81e050-9991-4913-bee1-7105a3108de3", null, true, "Administrator", "ADMINISTRATOR" },
                    { "3f81e050-9991-4913-bee1-7105a3108de4", null, true, "Employee", "EMPLOYEE" },
                    { "3f81e050-9991-4913-bee1-7105a3108de5", null, true, "DotInSiteUser", "DOTINSITEUSER" },
                    { "3f81e050-9991-4913-bee1-7105a3108de6", null, true, "DotComSiteUser", "DOTCOMSITEUSER" },
                    { "3f81e050-9991-4913-bee1-7105a3108de7", null, true, "ShopInUser", "SHOPINUSER" },
                    { "3f81e050-9991-4913-bee1-7105a3108de8", null, true, "EmployeeShopIn", "EMPLOYEESHOPIN" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CanLogin", "ChangePassword", "ConcurrencyStamp", "CreatedById", "CreatedOn", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "GenderId", "IpAddress", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneCode", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageName", "SecurityStamp", "TwoFactorEnabled", "UpdatedById", "UpdatedOn", "UserName" },
                values: new object[] { "214eccb4-402e-4004-807b-80e011db75d2", 0, true, true, "908a4aed-bf84-4558-b3b0-1814fd050858", null, new DateTime(2022, 10, 20, 12, 34, 26, 469, DateTimeKind.Unspecified).AddTicks(4431), null, "demoemailddj@gmail.com", false, "Demo", null, null, true, "D", true, null, "DEMOEMAILDDJ@GMAIL.COM", "DEM20231", "AQAAAAEAACcQAAAAELdnlJJYejLvTetcJEEkiCE7ENyDx6NG+qynlhYqXLvsxON0hriLoPA1mMtc8MUGQw==", null, null, false, null, "6ded5a7c-bacf-48db-a928-0b5f0dca8aee", false, null, null, "DEM20231" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "CurrencyTypes",
                columns: new[] { "Id", "Letter", "Symbol" },
                values: new object[] { 1, "INR", "₹" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "DeliveryPolicies",
                columns: new[] { "Id", "DeliveryInDays", "Name", "Title" },
                values: new object[] { 1, 15, "Downloadable15Days", "Download will be available for 15 days" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ExchangePolicies",
                columns: new[] { "Id", "ExchangeInDays", "Name" },
                values: new object[] { 1, 0, "No-Exchange" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Male" },
                    { 2, "Female" },
                    { 3, "TransGender" },
                    { 4, "Both" },
                    { 5, "Unknown" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "NextUserSettings",
                columns: new[] { "Id", "NextUserNumber" },
                values: new object[] { "d95ecbe5-b708-4252-86be-2631fc6634b1", 2 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "OrderStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Order-Initiated" },
                    { 2, "Order-Completed" },
                    { 3, "Payment-Pending" },
                    { 4, "Payment-Refunded" },
                    { 5, "Payment-Failed" },
                    { 6, "Payment-Refund-In-Process" },
                    { 7, "Payment-Refund-Failed" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PaymentGatewayTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Razorpay" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PaymentModes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Online" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ReturnPolicies",
                columns: new[] { "Id", "Name", "ReturnInDays" },
                values: new object[] { 1, "No-Return", 0 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "ViewUse", "true", "214eccb4-402e-4004-807b-80e011db75d2" },
                    { 2, "CreateUse", "true", "214eccb4-402e-4004-807b-80e011db75d2" },
                    { 3, "UpdateUse", "true", "214eccb4-402e-4004-807b-80e011db75d2" },
                    { 4, "UpdateUserPrivilegeUse", "true", "214eccb4-402e-4004-807b-80e011db75d2" },
                    { 5, "UpdateUserEmailUse", "true", "214eccb4-402e-4004-807b-80e011db75d2" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3f81e050-9991-4913-bee1-7105a3108de3", "214eccb4-402e-4004-807b-80e011db75d2" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PaymentStatuses",
                columns: new[] { "Id", "Name", "PaymentGatewayTypeId" },
                values: new object[,]
                {
                    { 1, "created", 1 },
                    { 2, "authorized", 1 },
                    { 3, "captured", 1 },
                    { 4, "refunded", 1 },
                    { 5, "failed", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatedById",
                schema: "dbo",
                table: "Addresses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CreatedOn",
                schema: "dbo",
                table: "Addresses",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PostOfficeId",
                schema: "dbo",
                table: "Addresses",
                column: "PostOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UpdatedById",
                schema: "dbo",
                table: "Addresses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UpdatedOn",
                schema: "dbo",
                table: "Addresses",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                schema: "dbo",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "dbo",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "dbo",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "dbo",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "dbo",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedById",
                schema: "dbo",
                table: "AspNetUsers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedOn",
                schema: "dbo",
                table: "AspNetUsers",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FirstName_LastName",
                schema: "dbo",
                table: "AspNetUsers",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GenderId",
                schema: "dbo",
                table: "AspNetUsers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileImageName",
                schema: "dbo",
                table: "AspNetUsers",
                column: "ProfileImageName",
                unique: true,
                filter: "[ProfileImageName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UpdatedById",
                schema: "dbo",
                table: "AspNetUsers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UpdatedOn",
                schema: "dbo",
                table: "AspNetUsers",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedById",
                schema: "dbo",
                table: "Categories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedOn",
                schema: "dbo",
                table: "Categories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                schema: "dbo",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UpdatedById",
                schema: "dbo",
                table: "Categories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UpdatedOn",
                schema: "dbo",
                table: "Categories",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code2",
                schema: "dbo",
                table: "Countries",
                column: "Code2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code3",
                schema: "dbo",
                table: "Countries",
                column: "Code3",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreatedById",
                schema: "dbo",
                table: "Countries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreatedOn",
                schema: "dbo",
                table: "Countries",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                schema: "dbo",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UpdatedById",
                schema: "dbo",
                table: "Countries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UpdatedOn",
                schema: "dbo",
                table: "Countries",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTypes_Letter",
                schema: "dbo",
                table: "CurrencyTypes",
                column: "Letter",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTypes_Symbol",
                schema: "dbo",
                table: "CurrencyTypes",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPolicies_Name",
                schema: "dbo",
                table: "DeliveryPolicies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CreatedById",
                schema: "dbo",
                table: "Districts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CreatedOn",
                schema: "dbo",
                table: "Districts",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name",
                schema: "dbo",
                table: "Districts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_StateId",
                schema: "dbo",
                table: "Districts",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_UpdatedById",
                schema: "dbo",
                table: "Districts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_UpdatedOn",
                schema: "dbo",
                table: "Districts",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangePolicies_Name",
                schema: "dbo",
                table: "ExchangePolicies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genders_Name",
                schema: "dbo",
                table: "Genders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoginLogs_UserId",
                schema: "dbo",
                table: "LoginLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_Name",
                schema: "dbo",
                table: "OrderStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGatewayTypes_Name",
                schema: "dbo",
                table: "PaymentGatewayTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentModes_Name",
                schema: "dbo",
                table: "PaymentModes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_Name",
                schema: "dbo",
                table: "PaymentStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_PaymentGatewayTypeId",
                schema: "dbo",
                table: "PaymentStatuses",
                column: "PaymentGatewayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_CreatedById",
                schema: "dbo",
                table: "PostOffices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_CreatedOn",
                schema: "dbo",
                table: "PostOffices",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_DistrictId",
                schema: "dbo",
                table: "PostOffices",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_Name",
                schema: "dbo",
                table: "PostOffices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_UpdatedById",
                schema: "dbo",
                table: "PostOffices",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostOffices_UpdatedOn",
                schema: "dbo",
                table: "PostOffices",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "dbo",
                table: "RefreshTokens",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnPolicies_Name",
                schema: "dbo",
                table: "ReturnPolicies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_CreatedById",
                schema: "dbo",
                table: "ShopCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_CreatedOn",
                schema: "dbo",
                table: "ShopCategories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_Name",
                schema: "dbo",
                table: "ShopCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_UpdatedById",
                schema: "dbo",
                table: "ShopCategories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_UpdatedOn",
                schema: "dbo",
                table: "ShopCategories",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_States_Code2",
                schema: "dbo",
                table: "States",
                column: "Code2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                schema: "dbo",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CreatedById",
                schema: "dbo",
                table: "States",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_States_CreatedOn",
                schema: "dbo",
                table: "States",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_States_Name",
                schema: "dbo",
                table: "States",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_UpdatedById",
                schema: "dbo",
                table: "States",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_States_UpdatedOn",
                schema: "dbo",
                table: "States",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CreatedById",
                schema: "dbo",
                table: "SubCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CreatedOn",
                schema: "dbo",
                table: "SubCategories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_Name",
                schema: "dbo",
                table: "SubCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_UpdatedById",
                schema: "dbo",
                table: "SubCategories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_UpdatedOn",
                schema: "dbo",
                table: "SubCategories",
                column: "UpdatedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CurrencyTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DeliveryPolicies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExchangePolicies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LoginLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NextUserSettings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderStatuses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentModes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentStatuses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReturnPolicies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ShopCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SubCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PostOffices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentGatewayTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Districts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "States",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Genders",
                schema: "dbo");
        }
    }
}
