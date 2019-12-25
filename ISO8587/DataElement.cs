using System;
using System.Collections.Generic;

namespace ISO8583
{
    public class DataElement : IEquatable<DataElement>
    {
        public int Number { get; private set; }
        public DataDefinition Definition { get; private set; }

        DataString _data;
        public DataElementCollection SubFields { get; private set; }

        public DataElement this[int subFieldNumber] => SubFields[subFieldNumber];

        public DataElement(DataDefinition dataDefinition, DataString data, int field)
        {
            SubFields = new DataElementCollection();
            Number = field;
            Definition = dataDefinition;

            if (dataDefinition.HasSubfields())
            {
                foreach (KeyValuePair<int, DataDefinition> kvp in dataDefinition.SubDefinitions)
                {
                    int fieldNumber = kvp.Key;
                    DataDefinition fieldDefinition = kvp.Value;
                    DataString fieldData = dataDefinition.GetSubFieldData(data, fieldNumber);
                    SubFields.AddOrReplaceDataElement(new DataElement(fieldDefinition, fieldData, fieldNumber));
                }
            }
            else
            {
                _data = data;
            }
        }

        public string GetFieldData()
        {
            return SubFields.HasValues() ?
                             SubFields.ToString() :
                             Definition.GetFieldData(_data);
        }

        public override string ToString()
        {
            string toString = GetFieldData();

            string append = string.Empty;

            if (Definition is VariableLengthDataDefinition)
            {
                VariableLengthDataDefinition dataDefinition = Definition as VariableLengthDataDefinition;
                int lenthFieldLength = (int)dataDefinition.LenthType;
                int fieldLength = toString.Length;
                append = fieldLength.ToString().PadLeft(lenthFieldLength, '0');
            }

            return append + toString;
        }

        public bool Equals(DataElement other)
        {
            return Number == other.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }
}
