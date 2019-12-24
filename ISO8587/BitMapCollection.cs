using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO8583
{
    public class BitMapCollection
    {
        private List<BitMap> _bitMaps = new List<BitMap>();

        public BitMap this[int number]
        {
            get
            {
                if (number < 1)
                    throw new ArgumentOutOfRangeException(nameof(number));

                return _bitMaps.Where(b => b.Number == number).Single();
            }
        }


        public BitMapCollection(DataString firstBitmapString)
        {
            AddBitMap(firstBitmapString);
        }


        public List<int> GetPresentDataElements()
        {
            List<int> presentDataElements = new List<int>();

            int index = 0;
            foreach (BitMap bitMap in _bitMaps)
            {
                foreach (int dataElement in bitMap.PresentDataElements)
                {
                    if (((64 * index) + 1) != dataElement)
                        presentDataElements.Add(dataElement);
                }

                index++;
            }

            return presentDataElements;
        }

        public int Count()
        {
            return _bitMaps.Count;
        }

        public void AddBitMap(DataString stringBitMap)
        {
            AddBitMap(stringBitMap.ToBibnaryString());
        }


        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            if (_bitMaps.Count == 0)
                return string.Empty.PadLeft(16, '0');

            _bitMaps.ForEach(
                bitMap => result.Append(bitMap.ToString())
                );

            return result.ToString();
        }
        private void AddBitMap(string binaryString)
        {
            if (binaryString.Length != 64)
                throw new ArgumentException(nameof(BitMap));

            _bitMaps.Add(new BitMap(binaryString, _bitMaps.Count + 1));
        }
    }
}
