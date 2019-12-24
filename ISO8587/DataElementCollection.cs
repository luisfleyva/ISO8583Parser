using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO8583
{
    public class DataElementCollection
    {
        private readonly Dictionary<int, DataElement> _dataElements = new Dictionary<int, DataElement>();

        public DataElement this[int number] => _dataElements[number];

        public bool HasValues()
        {
            return _dataElements.Count > 0;
        }
        public void AddOrReplaceDataElement(DataElement dataElement)
        {
            _dataElements.Add(dataElement.Number, dataElement);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            List<DataElement> dataElements = _dataElements.OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value)
                .ToList();

            foreach (DataElement de in dataElements)
            {
                result.Append(de.ToString());
            }

            return result.ToString();
        }

    }
}
