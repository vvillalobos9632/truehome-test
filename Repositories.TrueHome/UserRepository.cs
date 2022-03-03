using Common.DTOs.Settings;
using Domain.DataModel;
using Framework.Core.GenericRepository;
using IRepositories.TrueHome;
using Microsoft.AspNetCore.Http;

namespace Repositories.TrueHome
{
    public class UserRepository : Repository<TrueHomeContext, Usuario, int>, IUserRepository
    {
        public UserRepository(IConnectionStringsSettings connectionStringsSettings, IHttpContextAccessor accessor) : base(connectionStringsSettings, accessor)
        {
        }

        public Usuario GetUser(int idUsuario)
        {
            return FindBy(x => x.IdUsuario == idUsuario)
                   .FirstOrDefault();
        }
        
    }
}
