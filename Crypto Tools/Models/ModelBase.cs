using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CryptoTools.Utilities;

namespace CryptoTools.Models
{
    /// <summary>
    /// A convinient abstract class for Models
    /// </summary>
    public abstract class ModelBase : Observable, IDataErrorInfo
    {
        public abstract string Error
        {
            get;
        }

        public abstract string this[string columnName]
        {
            get;
        }
    }
}
