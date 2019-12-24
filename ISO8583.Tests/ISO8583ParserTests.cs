using Xunit;

namespace ISO8583.Tests
{
    public class ISO8583ParserTests
    {
        [Fact]
        private void Can_Parse_ISOMessage()
        {
            //Arrange
            string ISO8583Message = "0200B2200000001000000000000000800000201" +
            "234000000010000110722183012345606A5DFGR021ABCDEFGHIJ 1234567890";
            DataElementsDefinition dataElementsDefinition = new DataDefinitionDictionary();
            ISO8583Parser parser = new ISO8583Parser(dataElementsDefinition);

            //Act
            Message message = parser.Parse(ISO8583Message);

            //Assert

            //MTI
            Assert.Equal(Version.ISO8583_1987, message.MessageTypeIdentifier.Version);
            Assert.Equal(MessageClass.Financial, message.MessageTypeIdentifier.MessageClass);
            Assert.Equal(MessageSubClass.Request, message.MessageTypeIdentifier.MessageSubClass);
            Assert.Equal("0200", message.MessageTypeIdentifier.ToString());

            //BitMap
            Assert.Contains(3, message.BitMaps.GetPresentDataElements());
            Assert.Contains(4, message.BitMaps.GetPresentDataElements());
            Assert.Contains(7, message.BitMaps.GetPresentDataElements());
            Assert.Contains(11, message.BitMaps.GetPresentDataElements());
            Assert.Contains(44, message.BitMaps.GetPresentDataElements());
            Assert.Contains(105, message.BitMaps.GetPresentDataElements());
            Assert.Equal("B220000000100000", message.BitMaps[1].ToString());
            Assert.Equal("0000000000800000", message.BitMaps[2].ToString());
            Assert.Equal("B2200000001000000000000000800000", message.BitMaps.ToString());

            //DEs
            Assert.Equal("201234", message.DataElements[3].GetFieldData());
            Assert.Equal("20", message.DataElements[3][1].GetFieldData());
            Assert.Equal("12", message.DataElements[3][2].GetFieldData());
            Assert.Equal("34", message.DataElements[3][3].GetFieldData());
            Assert.Equal("000000010000", message.DataElements[4].GetFieldData());
            Assert.Equal("1107221830", message.DataElements[7].GetFieldData());
            Assert.Equal("123456", message.DataElements[11].GetFieldData());
            Assert.Equal("A5DFGR", message.DataElements[44].GetFieldData());
            Assert.Equal("06A5DFGR", message.DataElements[44].ToString());
            Assert.Equal("ABCDEFGHIJ 1234567890", message.DataElements[105].GetFieldData());

            //Message
            Assert.Equal("0200B2200000001000000000000000800000201" +
                "234000000010000110722183012345606A5DFGR021ABCDEFGHIJ 1234567890", message.ToString());

        }

        [Fact]
        private void Can_Can_Get_Field_Data()
        {
            //Arrange
            string ISO8583Message = "0200B2200000001000000000000000800000201" +
            "234000000010000110722183012345606A5DFGR021ABCDEFGHIJ 1234567890";
            DataElementsDefinition dataElementsDefinition = new DataDefinitionDictionary();
            ISO8583Parser parser = new ISO8583Parser(dataElementsDefinition);

            //Act
            string DE3Data = string.Empty;
            bool couldGetDEData = parser.TryGetFieldData(ISO8583Message, out DE3Data, 3);

            string DE3SF1Data = string.Empty;
            bool couldGetDE3SF1Data = parser.TryGetFieldData(ISO8583Message, out DE3SF1Data, 3, 1);

            string DE3SF2Data = string.Empty;
            bool couldGetDE3SF2Data = parser.TryGetFieldData(ISO8583Message, out DE3SF2Data, 3, 2);

            string DE3SF3Data = string.Empty;
            bool couldGetDE3SF3Data = parser.TryGetFieldData(ISO8583Message, out DE3SF3Data, 3, 3);

            string DE44SF1Data = string.Empty;
            bool couldGetDE44SF1Data = parser.TryGetFieldData(ISO8583Message, out DE44SF1Data, 44, 1);

            //Assert
            Assert.True(couldGetDE3SF1Data && couldGetDE3SF2Data && couldGetDE3SF3Data);
            Assert.Equal("201234", DE3Data);
            Assert.Equal("20", DE3SF1Data);
            Assert.Equal("12", DE3SF2Data);
            Assert.Equal("34", DE3SF3Data);
            Assert.False(couldGetDE44SF1Data);
            Assert.Equal(string.Empty, DE44SF1Data);

        }

    }
}
