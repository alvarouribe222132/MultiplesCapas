using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionITM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorregirSeedCorreos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estudiantes",
                keyColumn: "EstudianteId",
                keyValue: 1,
                column: "Correo",
                value: "juan@correo.itm.edu.co");

            migrationBuilder.UpdateData(
                table: "Estudiantes",
                keyColumn: "EstudianteId",
                keyValue: 2,
                column: "Correo",
                value: "maria@correo.itm.edu.co");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Estudiantes",
                keyColumn: "EstudianteId",
                keyValue: 1,
                column: "Correo",
                value: "juan@itm.edu.co");

            migrationBuilder.UpdateData(
                table: "Estudiantes",
                keyColumn: "EstudianteId",
                keyValue: 2,
                column: "Correo",
                value: "maria@itm.edu.co");
        }
    }
}
