using System;
using System.Collections.Generic;
using System.Text;

namespace ISO8583
{
    public class BitMapCollection
    {
        private readonly Dictionary<int, BitMap> _bitMaps = new Dictionary<int, BitMap>();

        public BitMap this[int number] => _bitMaps[number];


        public BitMapCollection(DataString firstBitmapString)
        {
            AddBitMap(firstBitmapString);
        }

        public BitMapCollection()
        {
            _bitMaps[1] = new BitMap(1);
        }

        public void SetPresentDataElement(int deNumber)
        {
            int bitMapNumber = 1;

            int aux = deNumber;
            while (aux > 64)
            {
                aux = aux - 64;
                bitMapNumber++;
            }
            
            while (Count() < bitMapNumber)
            {
                int number = Count() + 1;
                _bitMaps[number - 1].AddPresentDataElement((64 * (Count() -1)) + 1);
                _bitMaps.Add(number, new BitMap(number));                
            }

            _bitMaps[bitMapNumber].AddPresentDataElement(deNumber);
        }

        public List<int> GetPresentDataElements()
        {
            List<int> presentDataElements = new List<int>();

            int index = 0;
            foreach (BitMap bitMap in _bitMaps.Values)
            {
                foreach (int dataElement in bitMap.PresentDataElements)
                {
                    if (((64 * index) + 1) != dataElement)
                    {
                        presentDataElements.Add(dataElement);
                    }
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
            if (_bitMaps.Count == 0)
            {
                return string.Empty.PadLeft(16, '0');
            }

            StringBuilder result = new StringBuilder();
            foreach (BitMap bitMap in _bitMaps.Values)            
                result.Append(bitMap.ToString());            

            return result.ToString();
        }


        private void AddBitMap(string binaryString)
        {
            if (binaryString.Length != 64)
            {
                throw new ArgumentException(nameof(BitMap));
            }

            int bitMapNumber = _bitMaps.Count + 1;

            _bitMaps.Add(bitMapNumber, new BitMap(binaryString, bitMapNumber));
        }
    }
}
