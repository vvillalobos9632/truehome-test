using Common.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.FluentValidations
{
    public class UserDataValidator : AbstractValidator<UsuariosData>
    {
        public UserDataValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(300).WithMessage("El nombre ingresado sobrepasa el limite permitido"); ;
            RuleFor(x => x.NombreUsuario).NotEmpty().MaximumLength(50).WithMessage("El nombre de usuario ingresado sobrepasa el limite permitido");
            RuleFor(x => x.Contraseña).NotEmpty().WithMessage("Ingrese una contraseña valida");
        }
    }
}
