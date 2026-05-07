using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionITM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatosIniciales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Profesors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Estudiantes",
                columns: new[] { "EstudianteId", "Correo", "Documento", "FechaInscripcion", "Name", "Telefono" },
                values: new object[,]
                {
                    { 1, "juan@itm.edu.co", "1234567890", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Juan Pérez", "3001234567" },
                    { 2, "maria@itm.edu.co", "0987654321", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "María García", "3009876543" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Estudiantes",
                keyColumn: "EstudianteId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Estudiantes",
                keyColumn: "EstudianteId",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Especialidad",
                table: "Profesors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
