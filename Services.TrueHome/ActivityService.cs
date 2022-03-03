using Common.DTOs.Activity;
using Common.DTOs.Common.ValidationResults;
using Common.Extensions.Utils;
using Domain.DataModel;
using IRepositories.TrueHome;
using IServices.TrueHome;

namespace Services.TrueHome
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IPropertyRepository _propertyRepository;
        public ActivityService(IActivityRepository activityRepository,
            IPropertyRepository propertyRepository)
        {
            _activityRepository = activityRepository;
            _propertyRepository = propertyRepository;
        }

        public InitActivityData GetInitActivityViewModel(int id)
        {
            var result = new InitActivityData();
            result.property_idItems = _propertyRepository.GetActivePropertiesItems();
            result.statusItems = new List<Common.DTOs.SelectItemModel> {
               new Common.DTOs.SelectItemModel{Value = "ACTIVO", Label = "ACTIVO" },
               new Common.DTOs.SelectItemModel{Value = "INACTIVO", Label = "INACTIVO" }
            };
            if (id == 0)
            {

            }
            else
            {
                var activity = _activityRepository.GetActivity(id);
                result.ActivityViewModel = activity.MapTo<ActivityData>(allowNullableMap: true);
                result.property_idItems.ForEach(item =>
                {
                    if (item.Value.Equals(activity.PropertyId))
                        item.IsSelected = true;
                });

                result.statusItems.ForEach(item =>
                {
                    if (item.Value.Equals(activity.Status))
                        item.IsSelected = true;
                });
            }

            return result;
        }

        private void ValidateActivity(ActivityData activity)
        {
            var validations = new ValidationResults();
            validations.AddErrorIf(activity == null, "El activity no existe").AssertIsValid();

            if (activity.id > 0)
                validations.AddErrorIf(activity.status != "ACTIVO", "No es posible realizar cambios en actividades no activas").AssertIsValid();

            validations.AddErrorIf(!_propertyRepository.IsPropertyActive(activity.property_id), "La propiedad de esta actividad se encuentra inactiva").AssertIsValid();
            
            if (activity.id == 0 || activity.status == "ACTIVO")
                validations.AddErrorIf(!_activityRepository.IsValidateShedule(activity.id, activity.scheduleString.ConvertToDateTime()), "El horario asignado no se encuentra disponible, ya que existe una actividad previa o siguiente programada.").AssertIsValid();
        }

        public ActivityData RegisterActivity(ActivityData activityData)
        {
            ValidateActivity(activityData);
            var activity = activityData.MapTo<Activity>(allowNullableMap: true);
            activity.PropertyId = activityData.property_id;
            activity.CreatedAt = DateTime.Now;
            activity.UpdatedAd = DateTime.Now;
            activity.Status = "ACTIVO";
            activity.Schedule = activityData.scheduleString.ConvertToDateTime();
            _activityRepository.Add(activity).SaveChanges();

            return activityData;
        }

        public ActivityData UpdateActivity(ActivityData activityData)
        {
            ValidateActivity(activityData);

            var activity = _activityRepository.GetActivity(activityData.id);
            activity.UpdatedAd = DateTime.Now;
            activity.Schedule = activityData.scheduleString.ConvertToDateTime();
            _activityRepository.Update(activity).SaveChanges();

            return activityData;
        }

        public List<ActivityData> GetActivitysPagination(ActivityFilterData activityFilterData)
        {
            return _activityRepository.GetActivitysFilter(activityFilterData);
        }

        public bool CancelActivity(int id)
        {
            var activity = _activityRepository.GetActivity(id);
            ValidateCancelCompleteActivity(activity);
            activity.Status = "CANCELADA";
            activity.UpdatedAd = DateTime.Now;
            _activityRepository.Update(activity).SaveChanges();
            return true;
        }

        private void ValidateCancelCompleteActivity(Activity activity)
        {
            var validation = new ValidationResults();
            validation.AddErrorIf(activity == null, "No existe una actividad con el id proporcionado").AssertIsValid();
            validation.AddErrorIf(activity.Status != "ACTIVO", "La actividad no se encuentra en status activo").AssertIsValid();
        }

        public bool CompletelActivity(int id)
        {
            var activity = _activityRepository.GetActivity(id);
            ValidateCancelCompleteActivity(activity);
            activity.Status = "REALIZADA";
            activity.UpdatedAd = DateTime.Now;
            _activityRepository.Update(activity).SaveChanges();
            return true;
        }
    }
}
