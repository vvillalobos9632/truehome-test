using Common.DTOs.Activity;
using Domain.DataModel;
using Framework.Core.GenericRepository;

namespace IRepositories.TrueHome
{
    public interface IActivityRepository : IBaseRepository<TrueHomeContext, Activity, int>
    {
        Activity GetActivity(int id);
        bool IsValidateShedule(int id, DateTime? schedule);
        List<ActivityData> GetActivitysFilter(ActivityFilterData activityFilterData);
    }
}
