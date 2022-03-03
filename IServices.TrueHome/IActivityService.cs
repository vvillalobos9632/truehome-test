using Common.DTOs.Activity;

namespace IServices.TrueHome
{
    public interface IActivityService
    {
        List<ActivityData> GetActivitysPagination(ActivityFilterData activityFilterData);
        InitActivityData GetInitActivityViewModel(int id);
        ActivityData RegisterActivity(ActivityData activityData);
        ActivityData UpdateActivity(ActivityData activityData);
        bool CancelActivity(int id);
        bool CompletelActivity(int id);
    }
}
