using System;
using System.Collections.Generic;
using System.Linq;

namespace ISO8583
{
    public class ISO8583Builder
    {
        MessageTypeIdentifier _messageTypeIdentifier;
        DataElementsDefinition _dataElementsDefinition;
        Dictionary<int, Node> messageTree = new Dictionary<int, Node>();


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
                    throw new ArgumentNullException(nameof(field));

                int first = field[0];
                int[] rest = field.Skip(1).ToArray();

                if (field.Length == 1)
                    messageTree[first] = new DataNode(first, data);
                else
                {
                    MultiNode node = (messageTree.ContainsKey(first) && messageTree[first] is MultiNode)
                        ? messageTree[first] as MultiNode
                        : new MultiNode(first);

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

                foreach (var kvp in messageTree)
                {
                    if (_dataElementsDefinition.ContainsElementDefinition(kvp.Key))
                    {
                        DataElement dataElement = CreateDataElement(kvp.Value,
                                                                    _dataElementsDefinition.GetDataDefinition(kvp.Key));
                        message.AddOrReplaceDataElement(dataElement);
                    }
                    else
                        throw new Exception($"Missing definition, DE: {kvp.Key}");
                }

                return message;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: ISO8583Builder.Build()", ex);
            }
        }


        private MultiNode CreateField(MultiNode node, string data, params int[] field)
        {
            if (field.Length == 1)
                node.AddOrReplaceNode(new DataNode(field[0], data));
            else
                node.AddOrReplaceNode(CreateField(new MultiNode(field[0]),
                                                  data,
                                                  field.Skip(1).ToArray()));
            return node;
        }

        private DataElement CreateDataElement(Node node, DataDefinition definition)
        {
            if (node is DataNode)
            {
                DataNode dataNode = node as DataNode;
                string allData = definition.FillWithLength(dataNode.Data);
                return new DataElement(dataNode.Number, definition, new DataString(allData));
            }
            else
            {
                MultiNode multiNode = node as MultiNode;

                if (multiNode.Childs.Count != definition.SubDefinitions?.Count)
                    throw new Exception("Node childs Count must be equal to SubDefinitions Count.");

                HashSet<DataElement> subDataElements = new HashSet<DataElement>();
                foreach (var kvp in multiNode.Childs)
                {
                    DataElement innerDataElement = CreateDataElement(kvp.Value,
                        definition.SubDefinitions[kvp.Key]);

                    subDataElements.Add(innerDataElement);
                }

                return new DataElement(multiNode.Number, definition, subDataElements);
            }
        }

    }

    internal abstract class Node
    {
        public int Number { get; private set; }
        public Node(int number)
        {
            Number = number;
        }
    }
    internal class MultiNode : Node
    {
        Dictionary<int, Node> _childs;
        public IReadOnlyDictionary<int, Node> Childs => _childs;
        public MultiNode(int number) : base(number)
        {
            _childs = new Dictionary<int, Node>();
        }
        public void AddOrReplaceNode(Node node)
        {
            _childs[node.Number] = node;
        }
    }
    internal class DataNode : Node
    {
        public string Data { get; private set; }
        public DataNode(int number, string data) : base(number)
        {
            Data = data;
        }
    }
}
