using System;
using System.Linq;
using System.Text;

namespace ISO8583
{
    public class DataString
    {
        public string Data { get; private set; }
        public int Length => Data.Length;


        public DataString(string data)
        {
            Data = data;
        }


        public static DataString FromBinaryString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return new DataString(result.ToString());
        }

        public DataString Clone()
        {
            return new DataString(Data.Clone().ToString());
        }

        public DataString SubString(int startIndex, int length)
        {
            return new DataString(Data.Substring(startIndex, length));
        }

        public DataString SubString(int startIndex)
        {
            return new DataString(Data.Substring(startIndex));
        }

        public string ToBibnaryString()
        {
            string binarystring = string.Join(string.Empty,
                          Data.Select(
                            c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                          )
                        );

            return binarystring;
        }

        public override string ToString()
        {
            return Data;
        }
    }
}
