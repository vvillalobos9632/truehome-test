using Common.DTOs.Activity;
using Common.DTOs.Settings;
using Common.Extensions.Utils;
using Domain.DataModel;
using Framework.Core.GenericRepository;
using IRepositories.TrueHome;
using Microsoft.AspNetCore.Http;

namespace Repositories.TrueHome
{
    public class ActivityRepository : Repository<TrueHomeContext, Activity, int>, IActivityRepository
    {
        public ActivityRepository(IConnectionStringsSettings connectionStringsSettings, IHttpContextAccessor accessor) : base(connectionStringsSettings, accessor)
        {
        }

        public Activity GetActivity(int id)
        {
            return FindBy(x => x.Id == id)
                   .FirstOrDefault();
        }

        public List<ActivityData> GetActivitysFilter(ActivityFilterData activityFilterData)
        {
            var statusNull = activityFilterData.StatusShedule == null;
            var status = !statusNull ? activityFilterData.StatusShedule.Value.ToString() : "";
            var startDate = activityFilterData.StartShedule.ConvertToDateTime().Date;
            var endDate = activityFilterData.EndShedule.ConvertToDateTime().Date;

            return FindBy(x =>
            x.Schedule.Date >= startDate && 
            x.Schedule.Date <= endDate &&
            (statusNull || x.Status == status))
                .OrderBy(x => x.Id).Select(x => new ActivityData
                {
                    id = x.Id,
                    created_at = x.CreatedAt,
                    property_id = x.PropertyId,
                    schedule = x.Schedule,
                    status = x.Status,
                    title = x.Title,
                    updated_ad = x.UpdatedAd
                }).ToList();
        }

        public bool IsValidateShedule(int id, DateTime? schedule)
        {
            var oneHourBefore = schedule.GetValueOrDefault().AddHours(-1);
            var oneHourAfter = schedule.GetValueOrDefault().AddHours(1);
            var scheduleValue = schedule.GetValueOrDefault();
            return !Exist(x => x.Id != id && x.Schedule > oneHourBefore && x.Schedule < oneHourAfter && x.Status == "ACTIVO");
        }
    }
}
