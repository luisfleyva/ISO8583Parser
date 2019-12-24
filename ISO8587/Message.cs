using System;
using System.Collections.Generic;

namespace ISO8583
{
    public class Message
    {

        private int readerIndex = 0;

        private readonly DataElementsDefinition DataDefinitionDictionary;

        public MessageTypeIdentifier MessageTypeIdentifier { get; private set; }
        public BitMapCollection BitMaps { get; private set; }
        public DataElementCollection DataElements { get; private set; }


        public Message(DataString dataString, DataElementsDefinition dataElementDefinition)
        {
            DataDefinitionDictionary = dataElementDefinition
                ?? throw new ArgumentNullException(nameof(DataElementsDefinition));

            if (IsValidString(dataString))
            {
                CreateMTI(dataString);
                CreateBitMaps(dataString);
                CreateDEs(dataString);
            }
            else
            {
                throw new ArgumentException(nameof(DataString));
            }
        }


        public override string ToString()
        {
            return MessageTypeIdentifier.ToString()
                + BitMaps.ToString()
                + DataElements.ToString();
        }

        private bool IsValidString(DataString dataString)
        {
            if (dataString.Length < 20)
            {
                return false;
            }

            return true;
        }

        private void CreateMTI(DataString dataString)
        {
            string mti = dataString.Data.Substring(readerIndex, 4);
            readerIndex = 4;
            MessageTypeIdentifier = new MessageTypeIdentifier(mti); ;
        }

        private void CreateBitMaps(DataString dataString)
        {
            DataString bitMapString = dataString.SubString(readerIndex, 16);

            readerIndex = readerIndex + 16;

            BitMaps = new BitMapCollection(bitMapString);

            while (bitMapString.ToBibnaryString()[0] == '1')
            {
                if (dataString.Length < readerIndex + 16)
                {
                    throw new ArgumentException(nameof(DataString));
                }

                bitMapString = dataString.SubString(readerIndex, 16);

                readerIndex = readerIndex + 16;

                BitMaps.AddBitMap(bitMapString);
            }
        }

        private void CreateDEs(DataString dataString)
        {
            DataElements = new DataElementCollection();

            DataString dataElementsString = dataString.SubString(readerIndex);

            List<int> dataElementsBitMap = BitMaps.GetPresentDataElements();

            foreach (int de in dataElementsBitMap)
            {
                if (DataDefinitionDictionary.ContainsElementDefinition(de))
                {
                    DataDefinition dataDef = DataDefinitionDictionary.GetDataDefinition(de);

                    int dataElementsReaderIndex = 0;

                    try
                    {

                        DataString data = dataDef.GetAllData(dataElementsString, ref dataElementsReaderIndex);

                        dataElementsString = dataElementsString.SubString(dataElementsReaderIndex);

                        DataElements.AddOrReplaceDataElement(new DataElement(dataDef, data, de));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Missing or incorrectly defined DE: " + de + ".", ex);
                    }
                }
                else
                {
                    throw new Exception("Missing definition, DE: " + de + ".");
                }
            }

        }
    }
}
