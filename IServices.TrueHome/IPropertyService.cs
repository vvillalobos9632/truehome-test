using Common.DTOs.Property;

namespace IServices.TrueHome
{
    public interface IPropertyService
    {
        List<PropertyData> GetPropertysPagination();
        InitPropertyData GetInitPropertyViewModel(int id);
        PropertyData RegisterProperty(PropertyData propertyData);
        PropertyData UpdateProperty(PropertyData propertyData);
    }
}
