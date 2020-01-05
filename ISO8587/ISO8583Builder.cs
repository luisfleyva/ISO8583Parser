using System;
using System.Collections.Generic;
using System.Linq;

namespace ISO8583
{
    public class ISO8583Builder
    {
        private readonly MessageTypeIdentifier _messageTypeIdentifier;
        private readonly DataElementsDefinition _dataElementsDefinition;
        private readonly Dictionary<int, Node> messageTree = new Dictionary<int, Node>();


        public ISO8583Builder(string mti, DataElementsDefinition dataElementsDefinition)
            : this(new MessageTypeIdentifier(mti), dataElementsDefinition)
        { }

        public ISO8583Builder(MessageTypeIdentifier mti, DataElementsDefinition dataElementsDefinition)
        {
            _messageTypeIdentifier = mti
                ?? throw new ArgumentNullException(nameof(MessageTypeIdentifier));

            _dataElementsDefinition = dataElementsDefinition
                ?? throw new ArgumentNullException(nameof(DataElementsDefinition)); ;
        }


        public void SetField(string data, params int[] field)
        {
            try
            {
                if (field.Length <= 0)
                {
                    throw new ArgumentNullException(nameof(field));
                }

                int first = field[0];
                int[] rest = field.Skip(1).ToArray();

                if (field.Length == 1)
                {
                    messageTree[first] = new Node(first, data);
                }
                else
                {
                    Node node = (messageTree.ContainsKey(first))
                                ? messageTree[first]
                                : new Node(first);

                    messageTree[first] = CreateField(node, data, rest);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: ISO8583Builder.AddOrReplaceField(" +
                    $"string data: {data}, " +
                    $"params int[] field: {field.ConvertToString()})", ex);
            }
        }

        public Message Build()
        {
            try
            {
                Message message = new Message(_messageTypeIdentifier, _dataElementsDefinition);

                foreach (KeyValuePair<int, Node> kvp in messageTree)
                {
                    if (_dataElementsDefinition.ContainsElementDefinition(kvp.Key))
                    {
                        DataElement dataElement = CreateDataElement(kvp.Value,
                                                                    _dataElementsDefinition.GetDataDefinition(kvp.Key));
                        message.AddOrReplaceDataElement(dataElement);
                    }
                    else
                    {
                        throw new Exception($"Missing definition, DE: {kvp.Key}");
                    }
                }

                return message;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: ISO8583Builder.Build()", ex);
            }
        }


        private Node CreateField(Node node, string data, params int[] field)
        {
            if (field.Length == 1)
            {
                node.AddOrReplaceNode(new Node(field[0], data));
            }
            else
            {
                node.AddOrReplaceNode(CreateField(new Node(field[0]),
                                                  data,
                                                  field.Skip(1).ToArray()));
            }

            return node;
        }

        private DataElement CreateDataElement(Node node, DataDefinition definition)
        {
            if (!node.HasChilds())
            {
                string allData = definition.FillWithLength(node.Data);
                return new DataElement(node.Number, definition, new DataString(allData));
            }
            else
            {

                if (node.Childs.Count != definition.SubDefinitions?.Count)
                {
                    throw new Exception("Node childs Count must be equal to SubDefinitions Count.");
                }

                HashSet<DataElement> subDataElements = new HashSet<DataElement>();
                foreach (KeyValuePair<int, Node> kvp in node.Childs)
                {
                    DataElement innerDataElement = CreateDataElement(kvp.Value,
                        definition.SubDefinitions[kvp.Key]);

                    subDataElements.Add(innerDataElement);
                }

                return new DataElement(node.Number, definition, subDataElements);
            }
        }

    }

    internal class Node
    {
        public int Number { get; private set; }
        public string Data { get; private set; }

        private readonly Dictionary<int, Node> _childs;
        public IReadOnlyDictionary<int, Node> Childs => _childs;


        public Node(int number)
        {
            _childs = new Dictionary<int, Node>();
            Number = number;
        }
        public Node(int number, string data) : this(number)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            Data = data;
        }

        public bool HasChilds()
        {
            return _childs.Count != 0;
        }
        public void AddOrReplaceNode(Node node)
        {
            _childs[node.Number] = node;
        }
    }
}
