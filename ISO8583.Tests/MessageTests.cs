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



        [Fact]
        void Can_Create_Empty_Message()
        {
            //Arrange
            DataElementsDefinition dataElementsDefinition = new DataDefinitionDictionary();

            //Act
            Message message = new Message(new MessageTypeIdentifier("0800"), dataElementsDefinition);
            string messageString = message.ToString();

            //Assert
            Assert.Equal("08000000000000000000", messageString);
        }


        [Fact]
        void Can_Add_Data_Element()
        {
            //Arrange
            DataElementsDefinition dataElementsDefinition = new DataDefinitionDictionary();
            DataString elementData = new DataString("09114212233");
            DataDefinition def = new VariableLengthDataDefinition(DataType.n_numeric, VariableLenthType.LLVAR, 20,
                new Dictionary<int, DataDefinition>() {
                    { 1, new FixedLengthDataDefinition(DataType.n_numeric, 2) },
                    { 2, new VariableLengthDataDefinition(DataType.n_numeric,VariableLenthType.LVAR, 4,
                          new Dictionary<int, DataDefinition> {
                                { 1, new FixedLengthDataDefinition(DataType.n_numeric, 2) },
                                { 2, new FixedLengthDataDefinition(DataType.n_numeric, 2) }
                           }
                    )},
                    { 3, new FixedLengthDataDefinition(DataType.n_numeric, 2) }
                });
            DataElement mfDE74 = new DataElement(74, def, elementData);
            DataElement mfDE7 = new DataElement(7, def, elementData);
            Message message = new Message(new MessageTypeIdentifier("0800"), dataElementsDefinition);
            
            //Act
            message.AddOrReplaceDataElement(mfDE74);
            message.AddOrReplaceDataElement(mfDE7);
            List<int> des = message.BitMaps.GetPresentDataElements();
            string DE74Data = string.Empty;
            message.TryGetFieldData(out DE74Data, 74, 2, 1);

            //Assert
            Assert.Contains(74, des);
            Assert.Contains(7, des);
            Assert.Equal("21", DE74Data);
        }

    }
}
