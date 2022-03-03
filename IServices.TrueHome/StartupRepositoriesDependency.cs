using Common.Extensions.Utils;
using IRepositories.TrueHome;
using Microsoft.Extensions.DependencyInjection;
using Repositories.TrueHome;

namespace IServices.TrueHome
{
    public static class StartupRepositoriesDependency
    {
        public static void Register(IServiceCollection provider)
        {
            var interfacesTypes = typeof(IUserRepository).GetAllInterfaces();
            var implementationTypes = typeof(UserRepository).GetAllImplementations();

            foreach (var interfaceType in interfacesTypes)
                foreach (var implementationType in implementationTypes.Where(impType => interfaceType.IsAssignableFrom(impType)))
                    provider.AddTransient(interfaceType, implementationType);
        }
    }

}
