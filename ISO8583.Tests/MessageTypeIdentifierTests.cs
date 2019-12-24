using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ISO8583.Tests
{
    public class MessageTypeIdentifierTests
    {
        [Fact]
        void Can_Create_MTI()
        {
            //Act
            MessageTypeIdentifier mti1 = new MessageTypeIdentifier("0100");
            MessageTypeIdentifier mti2 = new MessageTypeIdentifier("1110");
            MessageTypeIdentifier mti3 = new MessageTypeIdentifier(Version.ISO8583_1987, MessageClass.Authorization, MessageSubClass.Request);

            //Assert
            Assert.Equal(Version.ISO8583_1987, mti1.Version);
            Assert.Equal(MessageClass.Authorization, mti1.MessageClass);
            Assert.Equal(MessageSubClass.Request, mti1.MessageSubClass);
            Assert.Equal(MessageSubClass.Response, mti2.MessageSubClass);
            Assert.Equal("0100", mti3.ToString());
        }
    }
}
