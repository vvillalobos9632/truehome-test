using Common.DTOs.Common;
using System.Collections.Generic;
using System;

namespace Common.DTOs.Activity
{
    public class InitActivityData
    {
        public InitActivityData()
        {
            ActivityViewModel = new ActivityData();
            property_idItems = new List<SelectItemModel>();
            statusItems = new List<SelectItemModel>();

        }

        public ActivityData ActivityViewModel { get; set; }

        public List<SelectItemModel> property_idItems { get; set; }
        public List<SelectItemModel> statusItems { get; set; }

    }
}
