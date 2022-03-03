using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Activity
{
    public class ActivityFilterData
    {
        public string StartShedule { get; set; }
        
        public string EndShedule { get; set; }

        public SelectItemModel? StatusShedule { get; set; }

    }
}
