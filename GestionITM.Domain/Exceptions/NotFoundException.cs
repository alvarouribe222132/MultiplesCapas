using System;
using System.Collections.Generic;
using System.Text;

namespace GestionITM.Domain.Exceptions
{
	public class NotFoundException :AppException
	{
		public NotFoundException(string message) : base(message, 404) 
		{
		}
		public NotFoundException(string name, object Key)
			: base($"{name} con id '{Key}' no fue encontrado. ", 404)
		{
		}
	}
	
}
