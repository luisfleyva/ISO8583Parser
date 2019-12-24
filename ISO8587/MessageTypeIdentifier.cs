using System;
using System.Linq;

namespace ISO8583
{
    public class MessageTypeIdentifier
    {
        public Version Version { get; private set; }

        public MessageClass MessageClass { get; private set; }

        public MessageSubClass MessageSubClass { get; private set; }


        public MessageTypeIdentifier(string mti)
        {
            if (!ValidateMTIString(mti))
            {
                throw new ArgumentException(nameof(MessageTypeIdentifier));
            }

            Version = (Version)int.Parse(mti[0].ToString());
            MessageClass = (MessageClass)int.Parse(mti[1].ToString());
            MessageSubClass = (MessageSubClass)int.Parse(mti.Substring(2).ToString());
        }

        public MessageTypeIdentifier(Version version, MessageClass @class, MessageSubClass subClass)
        {
            Version = version;
            MessageClass = @class;
            MessageSubClass = subClass;
        }


        public override string ToString()
        {
            return ((int)Version).ToString()
                + ((int)MessageClass).ToString()
                + ((int)MessageSubClass).ToString().PadLeft(2, '0');
        }

        private bool ValidateMTIString(string mti)
        {
            if (string.IsNullOrWhiteSpace(mti) || mti.Length != 4)
            {
                return false;
            }

            char[] versionValues = new char[] { '0', '1' };
            //Version
            if (!versionValues.Contains(mti[0]))
            {
                return false;
            }

            char[] messageClassValues = new char[] { '1', '2', '3', '4', '5', '6', '7', '8' };
            //Message class
            if (!messageClassValues.Contains(mti[1]))
            {
                return false;
            }

            string[] messageSubClassValues = new string[] { "00", "10", "20", "30", "40" };
            //Message subclass
            if (messageSubClassValues.Where(s => s == mti.Substring(2))
                .Count() != 1)
            {
                return false;
            }

            return true;
        }
    }

}
