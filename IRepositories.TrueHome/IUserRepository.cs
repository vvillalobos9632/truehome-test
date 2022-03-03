using Domain.DataModel;
using Framework.Core.GenericRepository;

namespace IRepositories.TrueHome
{
    public interface IUserRepository : IBaseRepository<TrueHomeContext, Usuario, int>
    {
        Usuario GetUser(int idUsuario);

    }
}
