using System.Collections.Generic;

namespace ISO8583.Tests
{
    public class DataDefinitionDictionary : DataElementsDefinition
    {
        public override Dictionary<int, DataDefinition> CreateDefinitions()
        {
            Dictionary<int, DataDefinition> DEs = new Dictionary<int, DataDefinition>()
            {
                //DE2
                { 2, new VariableLengthDataDefinition(DataType.n_numeric, VariableLenthType.LLVAR, 19) },

                //DE3
                { 3,  new FixedLengthDataDefinition(DataType.n_numeric, 6,

                    new Dictionary<int, DataDefinition>() {
                        { 1, new FixedLengthDataDefinition(DataType.n_numeric, 2) },//DE3SF1                    
                        { 2, new FixedLengthDataDefinition(DataType.n_numeric, 2) },//DE3SF2                    
                        { 3, new FixedLengthDataDefinition(DataType.n_numeric, 2) } //DE3SF3

                })},

                //DE4
                { 4, new FixedLengthDataDefinition(DataType.n_numeric, 12) },

                //DE7
                { 7, new FixedLengthDataDefinition(DataType.n_numeric, 10,
                    new Dictionary<int, DataDefinition>() {
                        { 1, new FixedLengthDataDefinition(DataType.n_numeric, 4)},//DE7SF1
                        { 2, new FixedLengthDataDefinition(DataType.n_numeric, 6)} //DE7SF2
                })},

                //DE11
                { 11, new FixedLengthDataDefinition(DataType.n_numeric, 6) },

                //DE41
                { 41, new FixedLengthDataDefinition(DataType.ans_alphaNumericSpecial, 8) },

                //DE44
                { 44, new VariableLengthDataDefinition(DataType.ans_alphaNumericSpecial, VariableLenthType.LLVAR, 25) },

                //DE105
                { 105, new VariableLengthDataDefinition(DataType.ans_alphaNumericSpecial, VariableLenthType.LLLVAR, 999) }
            };

            return DEs;
        }
    }
}
