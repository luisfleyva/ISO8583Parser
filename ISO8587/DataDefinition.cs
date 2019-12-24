using System.Collections.Generic;

namespace ISO8583
{
    public abstract class DataDefinition
    {
        private readonly Dictionary<int, DataDefinition> _subDefinitions;
        public IReadOnlyDictionary<int, DataDefinition> SubDefinitions => _subDefinitions;

        public DataType ElementType { get; private set; }

        public DataDefinition(DataType elementType, Dictionary<int, DataDefinition> subDefinitions)
        {
            ElementType = elementType;
            _subDefinitions = subDefinitions ?? new Dictionary<int, DataDefinition>();
        }


        public bool HasSubfields()
        {
            return SubDefinitions.Count > 0;
        }

        public abstract int GetLength();

        public abstract DataString GetAllData(DataString data, ref int nextFieldIndex);

        public abstract string GetFieldData(DataString data);

        public DataString GetAllData(DataString data)
        {
            int nextFieldIndex = 0;
            return GetAllData(data, ref nextFieldIndex);
        }

        public virtual DataString GetSubFieldData(DataString elementData, int field)
        {
            return GetSubFieldData(this, elementData, field);
        }

        private DataString GetSubFieldData(DataDefinition elementDef, DataString elementData, int field)
        {
            if (elementDef.HasSubfields())
            {
                int readerIndex = 0;

                DataString tempData = elementData.Clone();

                foreach (KeyValuePair<int, DataDefinition> kvp in elementDef.SubDefinitions)
                {
                    if (kvp.Key != field)
                    {
                        kvp.Value.GetAllData(tempData, ref readerIndex);
                        tempData = tempData.SubString(readerIndex);
                    }
                    else
                    {
                        DataDefinition fieldDefinition = kvp.Value;
                        return fieldDefinition.GetAllData(tempData);
                    }
                }
            }

            return elementDef.GetAllData(elementData);
        }

    }
}
