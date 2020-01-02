using System;
using System.Collections.Generic;
using System.Linq;

namespace ISO8583
{
    public class DataElement : IEquatable<DataElement>
    {
        public int Number { get; private set; }
        public DataDefinition Definition { get; private set; }

        DataString _data;
        public DataElementCollection SubElements { get; private set; }

        public DataElement this[int subFieldNumber] => SubElements[subFieldNumber];

        public DataElement(int number, DataDefinition dataDefinition, DataString data)
        {
            SubElements = new DataElementCollection();
            Number = number;
            Definition = dataDefinition;

            if (dataDefinition.HasSubfields())
            {
                foreach (KeyValuePair<int, DataDefinition> kvp in dataDefinition.SubDefinitions)
                {
                    int fieldNumber = kvp.Key;
                    DataDefinition fieldDefinition = kvp.Value;
                    DataString fieldData = dataDefinition.GetSubFieldData(data, fieldNumber);
                    SubElements.AddOrReplaceDataElement(new DataElement(fieldNumber, fieldDefinition, fieldData));
                }
            }
            else
            {
                _data = data;
            }
        }

        public DataElement(int number, DataDefinition dataDefinition, HashSet<DataElement> subElements)
        {
            SubElements = new DataElementCollection();
            Number = number;
            Definition = dataDefinition;

            if (AreValidSubElements(dataDefinition, subElements))
            {
                foreach (DataElement dataElement in subElements)
                {
                    SubElements.AddOrReplaceDataElement(dataElement);
                }
            }
            else
            {
                throw new ArgumentException("DataDefinition does not match subElements definition.");
            }
        }

        public string GetFieldData()
        {
            return SubElements.HasValues() ?
                             SubElements.ToString() :
                             Definition.GetFieldData(_data);
        }

        public override string ToString()
        {
            string toString = GetFieldData();

            return Definition.FillWithLength(toString);
        }

        public bool Equals(DataElement other)
        {
            return Number == other.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        private bool AreValidSubElements(DataDefinition definition, HashSet<DataElement> subEmelemnts)
        {
            if (definition.SubDefinitions.Count != subEmelemnts.Count)
                return false;

            foreach (DataElement de in subEmelemnts)
            {
                if (!definition.SubDefinitions[de.Number].Equals(de.Definition))
                    return false;
            }

            return true;
        }
    }
}
