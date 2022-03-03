using Common.Types.Utils;
using System.ComponentModel;

namespace Common.Types.Roles
{
    public enum SystemRoles
    {
        [StringValue("COMMON")]
        [Description("COMMON METHODS")]
        ALL = 0,

        [StringValue("OWNER")]
        [Description("OWNER")]
        OWNER = 1,
       
    }
}
