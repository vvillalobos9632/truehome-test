using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.Core.GenericRepository
{
    public class DbParameters
    {

        public string Name { get; set; }

        public ParameterDirection Direction { get; set; }

        public object Value { get; set; }

        public bool IsStructuredParameter { get; set; }

        public DbParameters(string paramName, object paramValue, ParameterDirection paramDirection = ParameterDirection.Input, bool isStructuredParameter = false)
        {
            Name = paramName;
            Direction = paramDirection;
            Value = paramValue;
            IsStructuredParameter = isStructuredParameter;
        }
    }

}
