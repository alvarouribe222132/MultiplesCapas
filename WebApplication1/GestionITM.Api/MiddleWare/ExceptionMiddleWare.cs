
using Microsoft.AspNetCore.Http;
using GestionITM.Domain.Exceptions;
using GestionITM.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;


namespace GestionITM.Api.Middleware
{
//Nota: este milddware vive en la capa API porque:
//trabaja directamennte con HttpContext, RequestDelegate y el pipeline HTTP de ASP.NET Core.
//forma parte de la capa de presentacion: se encarga de como respondemos al cliente(status code, JSON de error).
//la capa Infraestructure se enfoca en acceso a datos (DbContext, repositorios) y no deberia depender de ASP.NET Core.
	public class ExceptionMiddleWare 
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleWare> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
		{
			_next = next; //El siguiente paso en la tuberia de middlewares
			_logger = logger;
			_env = env;
		}


		public async Task InvokeAsync(HttpContext context)
		{
			//En este Bloque. Si en cualquier parte del camino,llamese (Controlador, servicio, repositorio) ocurre un error, el codigo salta inmediatamente al bloque catch de este Middleware
			//evistando asi un try cath en cada metodo
			try
			{
				await _next(context); //intenta seguir el flujo normal de la petición
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);// GUARDA EL ERRROR EN EL LOG
				await HandleExceptionAsync(context, ex); //MANEJA la excepcion y responde al cliente
			}
		}
		private async Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			context.Response.ContentType = "application/json"; //le decimos al cliente que la respuesta sera en formato JSON

			var statusCode = ex switch
			{
				NotFoundException => HttpStatusCode.NotFound, //404
				BadRequestException => HttpStatusCode.BadRequest, //400
				UnauthorizedException => HttpStatusCode.Unauthorized, //401
				ConflictException => HttpStatusCode.Conflict, //409
				_ => HttpStatusCode.InternalServerError //500 para cualquier otro error no manejado especificamente
			};

			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //codigo de error

			var message = ex is AppException
				? ex.Message //Mensaje de la excepcion personalizada (NotFound, BadRequest, etc)
				: "Ocurrio un error inesperado. Por favor intente nuevamente mas tarde."; //Mensaje generico para errores no manejados especificamente

			var response = new ErrorResponse
			{
				StatusCode = context.Response.StatusCode,
				Message = message,
				Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null //solo muestra en desarrollo NO en Produccion
			};


			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; //opciones para que las propiedades del JSON sean camelCase
			var json = JsonSerializer.Serialize(response, options); //serializamos el objeto de respuesta a JSON

			await context.Response.WriteAsync(json); //escribimos la respuesta JSON al cliente
		}

	}
		
}
