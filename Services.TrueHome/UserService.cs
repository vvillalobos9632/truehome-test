using Common.DTOs.Common.ValidationResults;
using Common.DTOs.FluentValidations;
using Common.DTOs.Users;
using Common.Extensions.Utils;
using Common.Types.Roles;
using Common.Utils.Utils;
using Domain.DataModel;
using Framework.Authorization.Util;
using IRepositories.TrueHome;
using IServices.TrueHome;

namespace Services.TrueHome
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public InitUsuariosData GetInitUserViewModel(int idUsuario)
        {
            var result = new InitUsuariosData();

            if (idUsuario == 0)
                result.UsuariosViewModel.Activo = true;
            else
            {
                var usuario = _userRepository.GetUser(idUsuario);
                result.UsuariosViewModel = usuario.MapTo<UsuariosData>();
                result.UsuariosViewModel.Contraseña = result.UsuariosViewModel.Contraseña.DesencriptarTexto(true);
            }

            return result;
        }

        private void ValidateUser(UsuariosData usuario)
        {
            var validations = new ValidationResults();
            var userDataValidator = new UserDataValidator();
            userDataValidator.Validate(usuario).AddFluenValidationErrors(validations).AssertIsValid();
            var existOtherUser = _userRepository.Exist(x => x.NombreUsuario == usuario.NombreUsuario && x.IdUsuario != usuario.IdUsuario);
            validations.AddErrorIf(existOtherUser, "El nombre de usuario ingresado ya se encuentra registrado.").AssertIsValid();
        }

        public UserLoginData Login(UserLoginData userLoginData)
        {
            var usuario = _userRepository.FindBy(x => x.NombreUsuario.ToLower() == userLoginData.NombreUsuario.ToLower()).FirstOrDefault();

            ValidateClientLogin(usuario, userLoginData);
            userLoginData.IdUsuario = usuario.IdUsuario;

            var expireDateToken = DateTime.Now.AddHours(-1);

            var roles = new List<SystemRoles>();
            roles.Add(SystemRoles.OWNER);
            userLoginData.Token = AuthenticationTokenConfig.GenerateJSONWebToken(userLoginData, roles, out expireDateToken);
            userLoginData.ExpireToken = expireDateToken;
            userLoginData.Contrasena = "";
            userLoginData.IsAuthorizedUser = true;

            return userLoginData;
        }

        private void ValidateClientLogin(Usuario usuario, UserLoginData userLoginData)
        {
            var validation = new ValidationResults();
            validation.AddErrorIf(usuario == null, "Usuario no registrado.").AssertIsValid();
            validation.AddErrorIf(!usuario.Activo, "Usuario no activo. Por favor pase a su sucursal.");
            validation.AddErrorIf(userLoginData.Contrasena.EncriptarTexto(true) != usuario.Contraseña, "Contraseña incorrecta. Por favor inténtalo nuevamente.");
            validation.AssertIsValid();
        }

        public UsuariosData RegisterUser(UsuariosData usuariosData)
        {
            ValidateUser(usuariosData);
            var user = usuariosData.MapTo<Usuario>();
            user.Contraseña = user.Contraseña.EncriptarTexto(true);
            _userRepository.Add(user).SaveChanges();
            usuariosData.IdUsuario = user.IdUsuario;
            return usuariosData;
        }

        public UsuariosData UpdateUser(UsuariosData usuariosData)
        {
            ValidateUser(usuariosData);

            var usuario = _userRepository.GetUser(usuariosData.IdUsuario); ;
            usuario.Activo = usuariosData.Activo;
            usuario.Nombre = usuariosData.Nombre;
            usuario.NombreUsuario = usuariosData.NombreUsuario;
            usuario.Contraseña = usuariosData.Contraseña.EncriptarTexto(true);
            _userRepository.Update(usuario).SaveChanges();
            return usuariosData;
        }

        public List<UsuariosData> GetUsersPagination()
        {
            return _userRepository.GetAll()
                .OrderBy(x=>x.IdUsuario)
                .Select(x => new UsuariosData
            {
                Nombre = x.Nombre,
                IdUsuario = x.IdUsuario,
                NombreUsuario = x.NombreUsuario,
                Activo = x.Activo
            }).ToList();
        }
    }
}
