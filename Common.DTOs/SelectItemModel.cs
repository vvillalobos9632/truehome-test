using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs
{
    public class SelectItemModel
    {
        public string Label { get; set; }

        public object Value { get; set; }

        public bool IsSelected { get; set; }

    }
}
