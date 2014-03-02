using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CryptoTools.Utilities;
using Org.BouncyCastle.Math;

namespace CryptoTools.Helpers
{
    public class RevokedSerials
    {

        private readonly List<RevokedSerial> _revokedSerials;

        public RevokedSerials()
        {
            _revokedSerials = new List<RevokedSerial>();
        }

        public List<RevokedSerial> RevokedSerialCollection
        {
            get { return _revokedSerials; }
        }
        public void Add(String bi, int reason, DateTime date)
        {
            _revokedSerials.Add(new RevokedSerial() { Reason = reason, RevocationDate = date, Serial = bi });
        }

        public void Remove(String bi)
        {
            foreach (var revokedSerial in _revokedSerials.Where(revokedSerial => revokedSerial.Serial.Equals(bi)))
            {
                _revokedSerials.Remove(revokedSerial);
                break;
            }
        }

        public RevokedSerial this[String bi]
        {
            get
            {
                return _revokedSerials.FirstOrDefault(revokedSerial => revokedSerial.Serial.Equals(bi));
            }
            set
            {
                var rs = this[bi];
                //first see if already contains this serial

                if (rs != null)
                    _revokedSerials.Remove(rs);
                if (value != null)
                    _revokedSerials.Add(value);
            }
        }
        
        public static void Serialize(RevokedSerials rs,string path)
        {
            SerializeUtils.SerializeToXml(rs,path);
        }

        public static RevokedSerials Deserialize(string path)
        {
            return  SerializeUtils.DeserializeFromXml<RevokedSerials>(path) ?? new RevokedSerials();
        }
    }

    public class RevokedSerial : Observable
    {
        private String _serial;
        public int Reason { get; set; }
        public DateTime RevocationDate { get; set; }
        public String Serial
        {
            get { return _serial; }
            set
            {
                if (_serial != value)
                {
                    _serial = value;
                    OnPropertyChanged("Serial");
                }
            }
        }
        
    }
}
