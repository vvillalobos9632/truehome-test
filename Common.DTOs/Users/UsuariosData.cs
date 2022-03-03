using Common.DTOs.Common;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Common.DTOs.Users
{
    public class UsuariosData 
    {
        public UsuariosData()
        {
         
        }

        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string NombreUsuario { get; set; }

        public string Contraseña { get; set; }

        public bool Activo { get; set; }

     

    }
}
