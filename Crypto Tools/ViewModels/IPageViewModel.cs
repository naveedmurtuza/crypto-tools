using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoTools.ViewModels
{
    public  interface IPageViewModel
    {
        string Name { get; }
        string ImageSource
        {
            get;
        }

        bool CanClose { get; }
        bool HasBackStack { get; }
        void Back();
        //string InternalName { get; set; }
    }
}
