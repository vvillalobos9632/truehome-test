using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Common
{
    public class InfoFriendlyException : InvalidOperationException
    {
        public InfoFriendlyException(string message)
            : base(message)
        {
        }
    }
}
