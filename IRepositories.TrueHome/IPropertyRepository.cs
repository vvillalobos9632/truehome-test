using Common.DTOs;
using Domain.DataModel;
using Framework.Core.GenericRepository;

namespace IRepositories.TrueHome
{
    public interface IPropertyRepository : IBaseRepository<TrueHomeContext, Property, int>
    {
        Property GetProperty(int id);
        List<SelectItemModel> GetActivePropertiesItems();
        bool IsPropertyActive(int property_id);
    }
}
