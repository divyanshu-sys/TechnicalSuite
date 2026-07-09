using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ts.Infra.DotCom.Data.Migrations
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
                name: "Posts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PostLink = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    MainImage = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
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
                    PostWorkerId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    MainImageSource = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PostSource = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    StoryLink = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    MainImage = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    MainImageSource = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MainDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    StoryWorkerId = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    StorySource = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostImages",
                schema: "dbo",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_PostImages", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostImages_Posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "dbo",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostRelatives",
                schema: "dbo",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    RelativeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_PostRelatives", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostRelatives_Posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "dbo",
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostViews",
                schema: "dbo",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false),
                    LastViewedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostViews", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostViews_Posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "dbo",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryImages",
                schema: "dbo",
                columns: table => new
                {
                    StoryId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_StoryImages", x => x.StoryId);
                    table.ForeignKey(
                        name: "FK_StoryImages_Stories_StoryId",
                        column: x => x.StoryId,
                        principalSchema: "dbo",
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryRelatives",
                schema: "dbo",
                columns: table => new
                {
                    StoryId = table.Column<int>(type: "int", nullable: false),
                    RelativeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_StoryRelatives", x => x.StoryId);
                    table.ForeignKey(
                        name: "FK_StoryRelatives_Stories_StoryId",
                        column: x => x.StoryId,
                        principalSchema: "dbo",
                        principalTable: "Stories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoryViews",
                schema: "dbo",
                columns: table => new
                {
                    StoryId = table.Column<int>(type: "int", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false),
                    LastViewedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryViews", x => x.StoryId);
                    table.ForeignKey(
                        name: "FK_StoryViews_Stories_StoryId",
                        column: x => x.StoryId,
                        principalSchema: "dbo",
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_CreatedOn",
                schema: "dbo",
                table: "PostImages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_UpdatedOn",
                schema: "dbo",
                table: "PostImages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PostRelatives_CreatedOn",
                schema: "dbo",
                table: "PostRelatives",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PostRelatives_UpdatedOn",
                schema: "dbo",
                table: "PostRelatives",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedOn",
                schema: "dbo",
                table: "Posts",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Keyword1",
                schema: "dbo",
                table: "Posts",
                column: "Keyword1");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Keyword2",
                schema: "dbo",
                table: "Posts",
                column: "Keyword2");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Keyword3",
                schema: "dbo",
                table: "Posts",
                column: "Keyword3");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Keyword4",
                schema: "dbo",
                table: "Posts",
                column: "Keyword4");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Keyword5",
                schema: "dbo",
                table: "Posts",
                column: "Keyword5");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostLink",
                schema: "dbo",
                table: "Posts",
                column: "PostLink",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Title",
                schema: "dbo",
                table: "Posts",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UpdatedOn",
                schema: "dbo",
                table: "Posts",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_LastViewedOn",
                schema: "dbo",
                table: "PostViews",
                column: "LastViewedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_CreatedOn",
                schema: "dbo",
                table: "Stories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Keyword1",
                schema: "dbo",
                table: "Stories",
                column: "Keyword1");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Keyword2",
                schema: "dbo",
                table: "Stories",
                column: "Keyword2");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Keyword3",
                schema: "dbo",
                table: "Stories",
                column: "Keyword3");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Keyword4",
                schema: "dbo",
                table: "Stories",
                column: "Keyword4");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Keyword5",
                schema: "dbo",
                table: "Stories",
                column: "Keyword5");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_StoryLink",
                schema: "dbo",
                table: "Stories",
                column: "StoryLink",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Title",
                schema: "dbo",
                table: "Stories",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UpdatedOn",
                schema: "dbo",
                table: "Stories",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_StoryImages_CreatedOn",
                schema: "dbo",
                table: "StoryImages",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_StoryImages_UpdatedOn",
                schema: "dbo",
                table: "StoryImages",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_StoryRelatives_CreatedOn",
                schema: "dbo",
                table: "StoryRelatives",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_StoryRelatives_UpdatedOn",
                schema: "dbo",
                table: "StoryRelatives",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_StoryViews_LastViewedOn",
                schema: "dbo",
                table: "StoryViews",
                column: "LastViewedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostImages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PostRelatives",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PostViews",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StoryImages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StoryRelatives",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StoryViews",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Posts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Stories",
                schema: "dbo");
        }
    }
}
