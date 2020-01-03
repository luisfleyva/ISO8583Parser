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
                throw new Exception($"Error: ISO8583Parser.Parse(" +
                    $"string ISO8583Message: {ISO8583Message})", ex);
            }
        }
        public bool TryGetFieldData(string ISO8583Message, out string fieldData, params int[] fields)
        {
            try
            {
                Message message = Parse(ISO8583Message);

                return message.TryGetFieldData(out fieldData, fields);
            }
            catch
            {
                fieldData = string.Empty;
                return false;
            }
        }
    }
}
