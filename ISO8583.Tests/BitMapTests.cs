using System.Collections.Generic;
using Xunit;

namespace ISO8583.Tests
{
    public class BitMapTests
    {
        [Fact]
        public void Can_Generate_DE_For_First_BitMap()
        {
            //Arrage
            DataString bitMapString = new DataString("B220000000100000");
            int bitMapNumeber = 1;
            BitMap bitMap = new BitMap(bitMapString.ToBibnaryString(), bitMapNumeber);

            //Act
            List<int> dataElements = new List<int>(bitMap.PresentDataElements);

            //Assert
            Assert.Contains(1, dataElements);
            Assert.Contains(3, dataElements);
            Assert.Contains(4, dataElements);
            Assert.Contains(7, dataElements);
            Assert.Contains(44, dataElements);
            Assert.DoesNotContain(2, dataElements);
        }

        [Fact]
        public void Can_Generate_DE_For_Second_BitMap()
        {
            //Arrage
            DataString bitMapString = new DataString("0000000000800000");
            int bitMapNumber = 2;

            BitMap bitMap = new BitMap(bitMapString.ToBibnaryString(), bitMapNumber);


            //Act
            List<int> dataElements = new List<int>(bitMap.PresentDataElements);

            //Assert
            Assert.Contains(105, dataElements);
            Assert.DoesNotContain(2, dataElements);
            Assert.DoesNotContain(65, dataElements);
        }
    }
}
