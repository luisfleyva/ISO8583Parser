using System;
using System.Collections.Generic;

namespace ISO8583
{
    public class FixedLengthDataDefinition : DataDefinition
    {
        public int Length { get; private set; }

        public FixedLengthDataDefinition(DataType elementType, int length, Dictionary<int, DataDefinition> subFieldsDefinitions = null)
            : base(elementType, subFieldsDefinitions)
        {
            Length = length;

            if (!IsValidSubFieldsLength())
            {
                throw new ArgumentException(nameof(subFieldsDefinitions));
            }
        }

        public override DataString GetAllData(DataString data, ref int nextFieldIndex)
        {
            nextFieldIndex = Length;
            return data.SubString(0, Length);
        }

        public override string FillWithLength(string data)
        {
            return data;
        }
        public override string GetFieldData(DataString data)
        {
            return GetAllData(data).ToString();
        }

        public override int GetLength()
        {
            return Length;
        }

        protected override bool IsEqualTo(DataDefinition other)
        {
            if (!(other is FixedLengthDataDefinition))
                return false;

            return Length == (other as FixedLengthDataDefinition).Length;
        }

        private bool IsValidSubFieldsLength()
        {
            if (HasSubfields())
            {
                int subFieldsLength = 0;
                foreach (KeyValuePair<int, DataDefinition> kvp in SubDefinitions)
                {
                    subFieldsLength += kvp.Value.GetLength();
                }

                return subFieldsLength == Length;
            }

            return true;
        }

    }
}
