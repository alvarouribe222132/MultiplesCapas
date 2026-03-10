using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Exceptions
{
	public abstract class AppException : Exception
	{
		public int StatusCode { get; } //Codigo de estado HTTP asociado a esta excepcion
		protected AppException(string message, int statusCode) : base(message)
		{ 
			StatusCode = statusCode;
		}
	}
}
