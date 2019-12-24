using System.Collections.Generic;
using Xunit;

namespace ISO8583.Tests
{
    public class DataElementsTests
    {
        [Fact]
        private void Can_Create_Multifield_DE()
        {
            //Arrange
            DataString elementData = new DataString("08114212233");
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


            //Act
            DataElement mfDE = new DataElement(def, elementData, 33);

            //Assert
            Assert.Equal("11", mfDE[1].GetFieldData());
            Assert.Equal("2122", mfDE[2].GetFieldData());
            Assert.Equal("42122", mfDE[2].ToString());
            Assert.Equal("21", mfDE[2][1].GetFieldData());
            Assert.Equal("33", mfDE[3].GetFieldData());
        }
    }
}
