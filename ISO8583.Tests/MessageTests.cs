using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ISO8583.Tests
{
    public class MessageTests
    {
        [Fact]
        public void Can_CreateMessage_From_String()
        {
            //Arrange
            DataString dataString = new DataString("0200B2200000001000000000000000800000201" +
                "234000000010000110722183012345606A5DFGR021ABCDEFGHIJ 1234567890");
            DataElementsDefinition dataElementsDefinition = new DataDefinitionDictionary();

            //Act
            Message message = new Message(dataString, dataElementsDefinition);

            //Assert
            Assert.Equal("0200", message.MessageTypeIdentifier.ToString());
            Assert.Contains(3, message.BitMaps.GetPresentDataElements());
            Assert.Contains(4, message.BitMaps.GetPresentDataElements());
            Assert.Contains(7, message.BitMaps.GetPresentDataElements());
            Assert.Contains(11, message.BitMaps.GetPresentDataElements());
            Assert.Contains(44, message.BitMaps.GetPresentDataElements());
            Assert.Contains(105, message.BitMaps.GetPresentDataElements());

            Assert.Equal("201234", message.DataElements[3].GetFieldData());
            Assert.Equal("000000010000", message.DataElements[4].GetFieldData());
            Assert.Equal("1107221830", message.DataElements[7].GetFieldData());
            Assert.Equal("123456", message.DataElements[11].GetFieldData());
            Assert.Equal("A5DFGR", message.DataElements[44].GetFieldData());
            Assert.Equal("ABCDEFGHIJ 1234567890", message.DataElements[105].GetFieldData());


            Assert.Equal("B2200000001000000000000000800000", message.BitMaps.ToString());

            Assert.Equal("0200B2200000001000000000000000800000201" +
                "234000000010000110722183012345606A5DFGR021ABCDEFGHIJ 1234567890", message.ToString());
        }



        [Fact]
        public void Can_Convert_ToString()
        {
            //Arramge
            DataString dataString = new DataString("0800202000000080000000000000000129110001");
            DataElementsDefinition dataElementsDefinition = new DataDefinitionDictionary();

            //Act
            Message message = new Message(dataString, dataElementsDefinition);

            //Assert
            Assert.Equal("0800202000000080000000000000000129110001", message.ToString());
            
        }

    }
}
