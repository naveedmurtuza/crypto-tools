using System;
using System.Collections.Generic;
using System.Globalization;

namespace CryptoTools.Helpers
{
    /// <summary>
    /// A helper class to list all countries
    /// </summary>
    public class CountryCollection
    {
        private static List<Country> _countries;

        public static List<Country> Countries
        {
            get
            {
                if (_countries == null)
                {
                    _countries = new List<Country>();
                    foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                    {
                        var country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);

                        var c = new Country { DisplayName = country.EnglishName, LocalizedName = country.NativeName, ISO2Code = country.TwoLetterISORegionName };
                        if (!_countries.Exists(cc => cc.ISO2Code == c.ISO2Code))
                        {
                            _countries.Add(c);
                        }

                    }
                    _countries.Sort((x, y) => String.CompareOrdinal(x.DisplayName, y.DisplayName));
                }
                return _countries;
            }
        }
    }


    public class Country
    {
        public String DisplayName { get; set; }
        public String LocalizedName { get; set; }
        public String ISO2Code { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Country)
            {
                return ((Country)obj).DisplayName == DisplayName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return DisplayName.GetHashCode();
        }
    }
}
