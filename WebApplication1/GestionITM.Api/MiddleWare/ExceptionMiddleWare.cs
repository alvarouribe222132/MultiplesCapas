
using GestionITM.Domain.Modelos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;


namespace GestionITM.Api.Middleware
{
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
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //codigo de error 500
			var response = new ErrorResponse
			{
				StatusCode = context.Response.StatusCode,
				Message = "Ocurrio un error inesperado. Por favor intente nuevamente mas tarde.",
				Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null //solo muestra en desarrollo NO en Produccion
			};


			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; //opciones para que las propiedades del JSON sean camelCase
			var json = JsonSerializer.Serialize(response, options); //serializamos el objeto de respuesta a JSON

			await context.Response.WriteAsync(json); //escribimos la respuesta JSON al cliente
		}

	}
		
}
