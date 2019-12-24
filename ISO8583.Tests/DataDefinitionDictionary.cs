using System.Collections.Generic;

namespace ISO8583.Tests
{
    public class DataDefinitionDictionary : DataElementsDefinition
    {
        public override Dictionary<int, DataDefinition> CreateDefinitions()
        {
            Dictionary<int, DataDefinition> DEs = new Dictionary<int, DataDefinition>
            {
                [2] = new VariableLengthDataDefinition(DataType.n_numeric, VariableLenthType.LLVAR, 19),

                [3] = new FixedLengthDataDefinition(DataType.n_numeric, 6,
                new Dictionary<int, DataDefinition>() {
                    { 1, new FixedLengthDataDefinition(DataType.n_numeric, 2) },
                    { 2, new FixedLengthDataDefinition(DataType.n_numeric, 2) },
                    { 3, new FixedLengthDataDefinition(DataType.n_numeric, 2) }
                }),

                [4] = new FixedLengthDataDefinition(DataType.n_numeric, 12),

                [7] = new FixedLengthDataDefinition(DataType.n_numeric, 10,
                new Dictionary<int, DataDefinition>() {
                    { 1, new FixedLengthDataDefinition(DataType.n_numeric, 4)},
                    { 2, new FixedLengthDataDefinition(DataType.n_numeric, 6)}
                }),

                [11] = new FixedLengthDataDefinition(DataType.n_numeric, 6),

                [41] = new FixedLengthDataDefinition(DataType.ans_alphaNumericSpecial, 8),

                [44] = new VariableLengthDataDefinition(DataType.ans_alphaNumericSpecial, VariableLenthType.LLVAR, 25),

                [105] = new VariableLengthDataDefinition(DataType.ans_alphaNumericSpecial, VariableLenthType.LLLVAR, 999)
            };

            return DEs;
        }
    }
}
