using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ts.Infra.ShopIn.Data.Migrations
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
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
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
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    BlogLink = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    MainImage = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keyword1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Keyword2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Keyword3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Keyword4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Keyword5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    BlogWorkerId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    MainImageSource = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BlogSource = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NextOrderSettings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    NextOrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextOrderSettings", x => x.Id);
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
                name: "ProductDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ProductDetailLink = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    MainImage = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    ShopCategoryId = table.Column<int>(type: "int", nullable: false),
                    ExchangePolicyId = table.Column<int>(type: "int", nullable: false),
                    DeliveryPolicyId = table.Column<int>(type: "int", nullable: false),
                    ReturnPolicyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keyword1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Keyword2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Keyword3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Keyword4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Keyword5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Mrp = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    ProductDetailWorkerId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
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
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MainImage = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedById = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    ProductWorkerId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
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
                    table.PrimaryKey("PK_Products", x => x.Id);
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
                name: "Orders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ClientName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PaymentModeId = table.Column<int>(type: "int", nullable: false),
                    TotalItemCount = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    OrderedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                name: "BlogImages",
                schema: "dbo",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    ImageNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_BlogImages", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_BlogImages_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "dbo",
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogViews",
                schema: "dbo",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false),
                    LastViewedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogViews", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_BlogViews_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "dbo",
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "dbo",
                columns: table => new
                {
                    ProductDetailId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ItemCount = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyTimestamp = table.Column<byte[]>(type: "timestamp", rowVersion: true, nullable: true, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.ProductDetailId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalSchema: "dbo",
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetailDocuments",
                schema: "dbo",
                columns: table => new
                {
                    ProductDetailId = table.Column<int>(type: "int", nullable: false),
                    Document = table.Column<string>(type: "varchar(450)", nullable: false),
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
                    table.PrimaryKey("PK_ProductDetailDocuments", x => x.ProductDetailId);
                    table.ForeignKey(
                        name: "FK_ProductDetailDocuments_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalSchema: "dbo",
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetailImages",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_ProductDetailImages", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductDetailImages_ProductDetails_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetailViews",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false),
                    LastViewedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetailViews", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductDetailViews_ProductDetails_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                schema: "dbo",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_ProductImages", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                schema: "dbo",
                columns: table => new
                {
                    ProductDetailId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.ProductDetailId);
                    table.ForeignKey(
                        name: "FK_ProductVariants_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalSchema: "dbo",
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    ProductDetailId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ItemCount = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    ShopCategoryId = table.Column<int>(type: "int", nullable: false),
                    ExchangePolicyId = table.Column<int>(type: "int", nullable: false),
                    DeliveryPolicyId = table.Column<int>(type: "int", nullable: false),
                    ReturnPolicyId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalSchema: "dbo",
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "dbo",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Razorpay_Method = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    Razorpay_Payment_Id = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: true),
                    Razorpay_Order_Id = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: true),
                    Razorpay_Amount_Refunded = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Razorpay_Refund_Status = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Razorpay_Created_At = table.Column<long>(type: "bigint", nullable: false),
                    PaymentGatewayTypeId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "dbo",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetailStatusHistories",
                schema: "dbo",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false),
                    OrderStatuses = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetailStatusHistories", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetailStatusHistories_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalSchema: "dbo",
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatusHistories",
                schema: "dbo",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatuses = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatusHistories", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_PaymentStatusHistories_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "dbo",
                        principalTable: "Payments",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "NextOrderSettings",
                columns: new[] { "Id", "NextOrderNumber" },
                values: new object[] { "d95ecbe5-b708-4252-86be-2631fc6634b3", 1 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "NextUserSettings",
                columns: new[] { "Id", "NextUserNumber" },
                values: new object[] { "d95ecbe5-b708-4252-86be-2631fc6634b2", 1 });

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
                name: "IX_AspNetUsers_ProfileImageName",
                schema: "dbo",
                table: "AspNetUsers",
                column: "ProfileImageName",
                unique: true,
                filter: "[ProfileImageName] IS NOT NULL");

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
                name: "IX_BlogImages_CreatedOn",
                schema: "dbo",
                table: "BlogImages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_BlogImages_UpdatedOn",
                schema: "dbo",
                table: "BlogImages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogLink",
                schema: "dbo",
                table: "Blogs",
                column: "BlogLink",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CreatedOn",
                schema: "dbo",
                table: "Blogs",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Keyword1",
                schema: "dbo",
                table: "Blogs",
                column: "Keyword1");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Keyword2",
                schema: "dbo",
                table: "Blogs",
                column: "Keyword2");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Keyword3",
                schema: "dbo",
                table: "Blogs",
                column: "Keyword3");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Keyword4",
                schema: "dbo",
                table: "Blogs",
                column: "Keyword4");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Keyword5",
                schema: "dbo",
                table: "Blogs",
                column: "Keyword5");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Title",
                schema: "dbo",
                table: "Blogs",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UpdatedOn",
                schema: "dbo",
                table: "Blogs",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_BlogViews_LastViewedOn",
                schema: "dbo",
                table: "BlogViews",
                column: "LastViewedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CreatedOn",
                schema: "dbo",
                table: "Carts",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UpdatedOn",
                schema: "dbo",
                table: "Carts",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                schema: "dbo",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginLogs_UserId",
                schema: "dbo",
                table: "LoginLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CreatedOn",
                schema: "dbo",
                table: "OrderDetails",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                schema: "dbo",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductDetailId",
                schema: "dbo",
                table: "OrderDetails",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_UpdatedOn",
                schema: "dbo",
                table: "OrderDetails",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderedOn",
                schema: "dbo",
                table: "Orders",
                column: "OrderedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                schema: "dbo",
                table: "Orders",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                schema: "dbo",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedOn",
                schema: "dbo",
                table: "Payments",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UpdatedOn",
                schema: "dbo",
                table: "Payments",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailDocuments_CreatedOn",
                schema: "dbo",
                table: "ProductDetailDocuments",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailDocuments_UpdatedOn",
                schema: "dbo",
                table: "ProductDetailDocuments",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailImages_CreatedOn",
                schema: "dbo",
                table: "ProductDetailImages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailImages_UpdatedOn",
                schema: "dbo",
                table: "ProductDetailImages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_CreatedOn",
                schema: "dbo",
                table: "ProductDetails",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Keyword1",
                schema: "dbo",
                table: "ProductDetails",
                column: "Keyword1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Keyword2",
                schema: "dbo",
                table: "ProductDetails",
                column: "Keyword2");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Keyword3",
                schema: "dbo",
                table: "ProductDetails",
                column: "Keyword3");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Keyword4",
                schema: "dbo",
                table: "ProductDetails",
                column: "Keyword4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Keyword5",
                schema: "dbo",
                table: "ProductDetails",
                column: "Keyword5");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductDetailLink",
                schema: "dbo",
                table: "ProductDetails",
                column: "ProductDetailLink",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Title",
                schema: "dbo",
                table: "ProductDetails",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_UpdatedOn",
                schema: "dbo",
                table: "ProductDetails",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailViews_LastViewedOn",
                schema: "dbo",
                table: "ProductDetailViews",
                column: "LastViewedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_CreatedOn",
                schema: "dbo",
                table: "ProductImages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_UpdatedOn",
                schema: "dbo",
                table: "ProductImages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedOn",
                schema: "dbo",
                table: "Products",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Title",
                schema: "dbo",
                table: "Products",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdatedOn",
                schema: "dbo",
                table: "Products",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                schema: "dbo",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "dbo",
                table: "RefreshTokens",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "BlogImages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BlogViews",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LoginLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NextOrderSettings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NextUserSettings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderDetailStatusHistories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PaymentStatusHistories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductDetailDocuments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductDetailImages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductDetailViews",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductImages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductVariants",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Blogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "dbo");
        }
    }
}
