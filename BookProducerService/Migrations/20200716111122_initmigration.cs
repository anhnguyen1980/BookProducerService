using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookProducerService.Migrations
{
    public partial class initmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint(4) unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "taskhistory",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    requested = table.Column<string>(nullable: true),
                    finish = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2020, 7, 16, 18, 11, 21, 986, DateTimeKind.Local).AddTicks(8686)),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    StatusId = table.Column<byte>(type: "tinyint(4) unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskhistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_taskhistory_status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(type: "varchar(255)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    TaskId = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.id);
                    table.ForeignKey(
                        name: "FK_book_author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_book_taskhistory_TaskId",
                        column: x => x.TaskId,
                        principalTable: "taskhistory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookgenre",
                columns: table => new
                {
                    BookId = table.Column<Guid>(nullable: false),
                    GenreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookgenre", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_bookgenre_book_BookId",
                        column: x => x.BookId,
                        principalTable: "book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookgenre_genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_book_AuthorId",
                table: "book",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_book_TaskId",
                table: "book",
                column: "TaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookgenre_BookId",
                table: "bookgenre",
                column: "BookId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookgenre_GenreId",
                table: "bookgenre",
                column: "GenreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_taskhistory_StatusId",
                table: "taskhistory",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookgenre");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "author");

            migrationBuilder.DropTable(
                name: "taskhistory");

            migrationBuilder.DropTable(
                name: "status");
        }
    }
}
