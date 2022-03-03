using Common.DTOs.Common;
using System.Collections.Generic;
using System;

namespace Common.DTOs.Property
{
    public class InitPropertyData
    {
        public InitPropertyData()
        {
            PropertyViewModel = new PropertyData();
            statusItems = new List<SelectItemModel>();

        }

        public PropertyData PropertyViewModel { get; set; }

        public List<SelectItemModel> statusItems { get; set; }

    }
}
