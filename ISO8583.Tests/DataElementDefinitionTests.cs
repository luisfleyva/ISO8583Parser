using System.Collections.Generic;
using Xunit;

namespace ISO8583.Tests
{
    public class DataDefinitionTests
    {
        [Fact]
        private void Can_Get_Fixed_Length_Data()
        {
            //Arrange
            DataString allDEISO = new DataString("201234000000010000110722183012345606A5DFGR021ABCDEFGHIJ1234567890");//DE #3, n-6
            DataDefinition def = new FixedLengthDataDefinition(DataType.n_numeric, 6);
            int nextElementIndex = 0;

            //Act
            DataString dataElementData = def.GetAllData(allDEISO, ref nextElementIndex);

            //Assert
            Assert.Equal("201234", dataElementData.ToString());
            Assert.Equal(6, nextElementIndex);
        }


        [Fact]
        private void Can_Get_Variable_Length_Data()
        {
            //Arrange
            DataString allDEISO = new DataString("06A5DFGR021ABCDEFGHIJ1234567890");//DE #44, an ..25, LLVAR
            DataDefinition def = new VariableLengthDataDefinition(DataType.an_alphaNumeric, VariableLenthType.LLVAR, 25);
            int nextElementIndex = 0;

            //Act
            DataString dataElementData = def.GetAllData(allDEISO, ref nextElementIndex);

            //Assert
            Assert.Equal("06A5DFGR", dataElementData.ToString());
            Assert.Equal(8, nextElementIndex);
        }


        [Fact]
        private void Can_Get_FixedLength_Subfield_Data()
        {
            //Arrange
            DataString elementData = new DataString("112233");//DE #3, n-6, 3 SubFields
            DataDefinition def = new FixedLengthDataDefinition(DataType.n_numeric, 6,
                new Dictionary<int, DataDefinition>() {
                    { 1, new FixedLengthDataDefinition(DataType.n_numeric, 2) },
                    { 2, new FixedLengthDataDefinition(DataType.n_numeric, 2) },
                    { 3, new FixedLengthDataDefinition(DataType.n_numeric, 2) }
                });


            //Act
            DataString sf1Data = def.GetSubFieldData(elementData, 1);
            DataString sf2Data = def.GetSubFieldData(elementData, 2);
            DataString sf3Data = def.GetSubFieldData(elementData, 3);

            //Assert
            Assert.Equal("11", sf1Data.ToString());
            Assert.Equal("22", sf2Data.ToString());
            Assert.Equal("33", sf3Data.ToString());
        }


        [Fact]
        private void Can_Get_VariableLength_Subfield_Data()
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
            DataString sf1Data = def.GetSubFieldData(elementData, 1);
            DataString sf2Data = def.GetSubFieldData(elementData, 2);
            DataString sf3Data = def.GetSubFieldData(elementData, 3);



            //Assert
            Assert.Equal("11", sf1Data.ToString());
            Assert.Equal("42122", sf2Data.ToString());
            Assert.Equal("33", sf3Data.ToString());
        }
    }
}
