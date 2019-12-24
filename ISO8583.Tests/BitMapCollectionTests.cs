using System.Collections.Generic;
using Xunit;

namespace ISO8583.Tests
{
    public class BitMapCollectionTests
    {
        [Fact]
        private void Can_Create_BitMap_String()
        {
            //Arrange
            BitMapCollection bitMaps = new BitMapCollection(new DataString("B220000000100000"));

            //Act
            bitMaps.AddBitMap(new DataString("0000000000800000"));
            IEnumerable<int> dataElements = bitMaps.GetPresentDataElements();

            //Assert
            Assert.Contains(3, dataElements);
            Assert.Contains(4, dataElements);
            Assert.Contains(7, dataElements);
            Assert.Contains(44, dataElements);
            Assert.Contains(105, dataElements);
            Assert.DoesNotContain(65, dataElements);
            Assert.DoesNotContain(2, dataElements);
        }

        [Fact]
        private void Can_Convert_ToString()
        {
            //Act
            BitMapCollection bitMap = new BitMapCollection(new DataString("B220000000100000"));

            //Assert
            Assert.Equal("B220000000100000", bitMap.ToString());

        }
    }
}
