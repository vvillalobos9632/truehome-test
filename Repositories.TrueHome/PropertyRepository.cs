using Common.DTOs;
using Common.DTOs.Settings;
using Domain.DataModel;
using Framework.Core.GenericRepository;
using IRepositories.TrueHome;
using Microsoft.AspNetCore.Http;

namespace Repositories.TrueHome
{
    public class PropertyRepository : Repository<TrueHomeContext, Property, int>, IPropertyRepository
    {
        public PropertyRepository(IConnectionStringsSettings connectionStringsSettings, IHttpContextAccessor accessor) : base(connectionStringsSettings, accessor)
        {
        }

        public List<SelectItemModel> GetActivePropertiesItems()
        {
            return FindBy(x => x.Status == "ACTIVO")
                .Select(x => new SelectItemModel
                {
                    Value = x.Id,
                    Label = $"{x.Title} - {x.Description}"
                }).ToList();
        }

        public Property GetProperty(int id)
        {
            return FindBy(x => x.Id == id)
                   .FirstOrDefault();
        }

        public bool IsPropertyActive(int property_id)
        {
            return Exist(x => x.Id == property_id && x.Status == "ACTIVO");
        }
    }
}
