using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Settings
{
    public abstract class FriendlyException
      : InvalidOperationException
    {
        public FriendlyException(string message)
            : base(message)
        {
        }
    }
}
