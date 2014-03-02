using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoTools.ViewModels;

namespace CryptoTools.Utilities
{
   public interface IMessageBoxSupport
    {
        bool IsMessageBoxVisible { get; set; }

        object MessageBoxContent{ get; set; }
        
    }
}
