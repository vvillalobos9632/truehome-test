using Common.DTOs.Common.ValidationResults;
using Common.DTOs.FluentValidations;
using Common.DTOs.Property;
using Common.Extensions.Utils;
using Domain.DataModel;
using IRepositories.TrueHome;
using IServices.TrueHome;
using Microsoft.EntityFrameworkCore;

namespace Services.TrueHome
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        public PropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public InitPropertyData GetInitPropertyViewModel(int id)
        {
            var result = new InitPropertyData();

            result.statusItems = new List<Common.DTOs.SelectItemModel> {
               new Common.DTOs.SelectItemModel{Value = "ACTIVO", Label = "ACTIVO" },
               new Common.DTOs.SelectItemModel{Value = "INACTIVO", Label = "INACTIVO" }
            };

            if (id == 0)
            {

            }
            else
            {
                var property = _propertyRepository.GetProperty(id);
                result.PropertyViewModel = property.MapTo<PropertyData>();
                result.statusItems.ForEach(item => {
                    if (item.Value.Equals(property.Status))
                        item.IsSelected = true;
                }); 
            }

            return result;
        }

        private void ValidateProperty(PropertyData property)
        {
            var validations = new ValidationResults();
            var propertyValidator = new PropertyDataValidator();
            propertyValidator.Validate(property).AddFluenValidationErrors(validations);
            validations.AddErrorIf(property == null, "El property no existe").AssertIsValid();
        }


        public PropertyData RegisterProperty(PropertyData propertyData)
        {
            ValidateProperty(propertyData);
            var property = propertyData.MapTo<Property>();
            property.CreateAt = DateTime.Now;
            property.UpdateAt = DateTime.Now;
            _propertyRepository.Add(property).SaveChanges();

            return propertyData;
        }

        public PropertyData UpdateProperty(PropertyData propertyData)
        {
            ValidateProperty(propertyData);

            var property = _propertyRepository.GetProperty(propertyData.id);
            property.Id = propertyData.id;
            property.Title = propertyData.title;
            property.Address = propertyData.address;
            property.Description = propertyData.description;
            property.UpdateAt = DateTime.Now;
            //property.disable_at = propertyData.disable_at;
            property.Status = propertyData.status;

            _propertyRepository.Update(property).SaveChanges();

            return propertyData;
        }

        public List<PropertyData> GetPropertysPagination()
        {
            return _propertyRepository.GetAll().OrderBy(x => x.Id).Select(x => new PropertyData
            {
                id = x.Id,
                address = x.Address,
                description = x.Description,
                create_at = x.CreateAt,
                disable_at = x.DisableAt,
                status = x.Status,
                title = x.Title,
                update_at = x.UpdateAt
            }).ToList();
        }
    }
}
