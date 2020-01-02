using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ISO8583.Tests
{
    public class ISO8583BuilderTests
    {
        [Fact]
        void Can_Build_Message()
        {
            //Arrange
            DataElementsDefinition dataElementsDefinition = new DataDefinitionDictionary();
            ISO8583Builder builder = new ISO8583Builder("0200", dataElementsDefinition);

            //Act
            builder.AddOrReplaceField("20", 3, 1);
            builder.AddOrReplaceField("12", 3, 2);
            builder.AddOrReplaceField("34", 3, 3);
            builder.AddOrReplaceField("000000010000", 4);
            builder.AddOrReplaceField("1107221830", 7);
            builder.AddOrReplaceField("123456", 11);
            builder.AddOrReplaceField("A5DFGR", 44);
            builder.AddOrReplaceField("ABCDEFGHIJ 1234567890", 105);

            Message message = builder.Build();

            //Assert
            Assert.Equal("0200B2200000001000000000000000800000201" +
                        "234000000010000110722183012345606A5DFGR02" +
                        "1ABCDEFGHIJ 1234567890", message.ToString());
        }
    }
}
