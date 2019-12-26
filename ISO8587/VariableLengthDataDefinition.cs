using System;
using System.Collections.Generic;

namespace ISO8583
{
    public class VariableLengthDataDefinition : DataDefinition
    {
        public VariableLenthType LenthType { get; private set; }

        private readonly int _maxLength;

        public VariableLengthDataDefinition(DataType elementType, VariableLenthType lenthType, int maxLength, Dictionary<int, DataDefinition> subFieldsDefinitions = null)
            : base(elementType, subFieldsDefinitions)
        {
            LenthType = lenthType;
            _maxLength = maxLength;

            if (!IsValidSubFieldsLength())
            {
                throw new ArgumentException(nameof(subFieldsDefinitions));
            }
        }

        public override string GetFieldData(DataString data)
        {
            int lengthType = (int)LenthType;
            int fieldLength = int.Parse(data.SubString(0, lengthType).ToString());

            return data.SubString(lengthType, fieldLength).ToString();
        }
        public override DataString GetAllData(DataString data, ref int nextFieldIndex)
        {
            int lengthType = (int)LenthType;

            int fieldLength = int.Parse(data.SubString(0, lengthType).ToString());

            if (fieldLength > _maxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(VariableLenthType));
            }

            nextFieldIndex = lengthType + fieldLength;

            return data.SubString(0, nextFieldIndex);
        }

        public override DataString GetSubFieldData(DataString elementData, int field)
        {
            if (HasSubfields())
            {
                DataString fieldsData = elementData.SubString((int)LenthType);

                return base.GetSubFieldData(fieldsData, field);
            }

            throw new ArgumentException(nameof(GetSubFieldData));
        }

        public override int GetLength()
        {
            return (int)LenthType;
        }

        protected override bool IsEqualTo(DataDefinition other)
        {
            if (!(other is VariableLengthDataDefinition))
                return false;

            VariableLengthDataDefinition theOther = other as VariableLengthDataDefinition;

            return LenthType == theOther.LenthType && _maxLength == theOther._maxLength;
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

                return subFieldsLength <= _maxLength;
            }

            return true;
        }

        
    }
}
