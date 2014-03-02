using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Models;
using CryptoTools.Utilities;
namespace CryptoTools.ViewModels
{
    public class X509NameViewModel
    {
        private ICommand _addAddtnlOids;

        public ICommand AddAdditionalOIDsCommand
        {
            get
            {
                if (_addAddtnlOids == null)
                    _addAddtnlOids = new RelayCommand(param => this.AddAdditionalOIDs());

                return _addAddtnlOids;
            }
        }

        public X509NameViewModel()
        {
            Model = new X509NameModel();
        }
        public X509NameModel Model { get; set; }

        public bool IsModelValid()
        {
            return !Model.CommonName.IsNullOrEmpty() && Model.Country != null;
        }
        void AddAdditionalOIDs()
        {
            List<String> sb = null;
            //add existing AdditionalOIDs to the list
            if (!Model.AdditionalOIDs.IsNullOrEmpty())
                sb = Model.AdditionalOIDs.Split(new String[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries).ToList();
            //if no oid is selected, that means remove the item
            if (Model.SelectedOIDValue.IsNullOrEmpty())
            {
                if (sb == null)
                    return;
                foreach (var s in sb.Where(s => s.Contains(Model.SelectedOIDName)))
                {
                    sb.Remove(s);
                    break;
                }
                Model.AdditionalOIDs = String.Join(Environment.NewLine, sb);
            }
            else
            {
                //add the item
                if (!Model.AdditionalOIDs.IsNullOrEmpty() && Model.AdditionalOIDs.Contains(Model.SelectedOIDName))
                {
                    //delete the existing oid
                    foreach (var s in sb.Where(s => s.Contains(Model.SelectedOIDName)))
                    {
                        sb.Remove(s);
                        break;
                    }
                    Model.AdditionalOIDs = String.Join(Environment.NewLine, sb);
                    Model.AdditionalOIDs += Environment.NewLine;
                }
                Model.AdditionalOIDs += String.Format("{0}={1}{2}", Model.SelectedOIDName, Model.SelectedOIDValue,
                                                      Environment.NewLine);
            }
        }
    }
}
