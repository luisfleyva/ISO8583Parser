using System;
using System.Collections.Generic;
using System.Text;

namespace ISO8583
{
    public class BitMap
    {
        public int Number { get; private set; }

        private readonly List<int> _presentDataElements;
        public IReadOnlyList<int> PresentDataElements => _presentDataElements;


        public BitMap(string bitMapBinaryString, int number)
        {
            if (number < 1)
            {
                throw new IndexOutOfRangeException(nameof(BitMap));
            }

            Number = number;
            _presentDataElements = GetPresentDataElements(bitMapBinaryString, number);
        }

        public DataString ToISOString()
        {
            return DataString.FromBinaryString(ToBinaryString());
        }

        public string ToBinaryString()
        {
            string bitMapTemplate = "00000000000000000000000000000000" +
                 "00000000000000000000000000000000";

            StringBuilder bitMap = new StringBuilder(bitMapTemplate);

            foreach (int de in PresentDataElements)
            {
                int temp = de;

                while (temp > 64)
                {
                    temp = temp - 64;
                }

                bitMap[temp - 1] = '1';
            }

            return bitMap.ToString();
        }

        public override string ToString()
        {
            return ToISOString().ToString();
        }


        private List<int> GetPresentDataElements(string bitMapBinaryString, int number)
        {
            List<int> presentDataElements = new List<int>();

            int startIndex = ((number - 1) * 64) + 1;

            for (int i = 0; i < bitMapBinaryString.Length; i++)
            {
                if (bitMapBinaryString[i] == '1')
                {
                    presentDataElements.Add(i + startIndex);
                }
            }

            return presentDataElements;
        }

    }
}
