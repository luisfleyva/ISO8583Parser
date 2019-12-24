using System.Collections.Generic;

namespace ISO8583
{
    public abstract class DataElementsDefinition
    {
        private readonly Dictionary<int, DataDefinition> _dictionary = new Dictionary<int, DataDefinition>();

        public DataElementsDefinition()
        {
            _dictionary = CreateDefinitions();
        }


        public abstract Dictionary<int, DataDefinition> CreateDefinitions();


        public bool ContainsElementDefinition(int dataElementNumber)
        {
            return _dictionary.ContainsKey(dataElementNumber);
        }

        public DataDefinition GetDataDefinition(int dataElementNumber)
        {
            return _dictionary.ContainsKey(dataElementNumber) ? _dictionary[dataElementNumber] : null;
        }
    }
}
