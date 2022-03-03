using Common.DTOs.Users;

namespace IServices.TrueHome
{
    public interface IUserService
    {
        UserLoginData Login(UserLoginData userLoginData);
        InitUsuariosData GetInitUserViewModel(int idUsuario);
        UsuariosData RegisterUser(UsuariosData usuariosData);
        UsuariosData UpdateUser(UsuariosData usuariosData);
        List<UsuariosData> GetUsersPagination();
    }
}
