using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoTools.Utilities;

namespace CryptoTools.Models
{
    public class ExportKeyModel : Observable
    {
        private String _path;
        public String Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }
    }
}
