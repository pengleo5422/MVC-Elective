using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class build : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CourseAssignment",
                columns: table => new
                {
                    CourseAssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherID = table.Column<int>(type: "int", nullable: false),
                    TeachID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssignment", x => x.CourseAssignmentID);
                    table.ForeignKey(
                        name: "FK_CourseAssignment_Teach_TeachID",
                        column: x => x.TeachID,
                        principalTable: "Teach",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseAssignment_Teacher_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teacher",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignment_TeacherID",
                table: "CourseAssignment",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignment_TeachID",
                table: "CourseAssignment",
                column: "TeachID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseAssignment");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}
