using System;

namespace ISO8583
{
    public class ISO8583Parser
    {
        private readonly DataElementsDefinition _elementsDefinition;

        public ISO8583Parser(DataElementsDefinition elementsDefinition)
        {
            _elementsDefinition = elementsDefinition
                ?? throw new ArgumentNullException(nameof(DataElementsDefinition));
        }

        public Message Parse(string ISO8583Message)
        {
            try
            {
                Message message = new Message(new DataString(ISO8583Message), _elementsDefinition);

                return message;
            }
            catch (Exception ex)
            {
                throw new Exception("Incorrectly formated ISO5883Messsage.", ex);
            }
        }
        public bool TryGetFieldData(string ISO8583Message, out string fieldData, params int[] fields)
        {
            try
            {
                if (fields.Length <= 0)
                {
                    throw new ArgumentNullException(nameof(fields));
                }

                Message message = Parse(ISO8583Message);

                DataElement deField = message.DataElements[fields[0]];

                for (int i = 1; i < fields.Length; i++)
                {
                    deField = deField[fields[i]];
                }

                fieldData = deField.GetFieldData();
                return true;
            }
            catch
            {
                fieldData = null;
                return false;
            }
        }
    }
}
