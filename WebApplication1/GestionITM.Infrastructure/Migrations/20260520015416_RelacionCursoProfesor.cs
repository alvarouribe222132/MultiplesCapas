using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionITM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelacionCursoProfesor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfesorId",
                table: "Cursos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_ProfesorId",
                table: "Cursos",
                column: "ProfesorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cursos_Profesors_ProfesorId",
                table: "Cursos",
                column: "ProfesorId",
                principalTable: "Profesors",
                principalColumn: "ProfesorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cursos_Profesors_ProfesorId",
                table: "Cursos");

            migrationBuilder.DropIndex(
                name: "IX_Cursos_ProfesorId",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "ProfesorId",
                table: "Cursos");
        }
    }
}
