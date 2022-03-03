using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Types.Utils
{
    public class StringValueAttribute
  : Attribute
    {
        public string Description { get; private set; }

        public StringValueAttribute(string description)
        {
            Description = description;
        }
    }
}
