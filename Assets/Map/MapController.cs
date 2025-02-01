using System.Linq;
using UnityEngine;

public class MapController
{
    private static MapController _instance;
    public static MapController Instance
    {
        get
        {
            if (_instance == null) _instance = new MapController();
            return _instance;
        }
    }

    private MapNode _currentNode;
    public MapNode CurrentNode => _currentNode;
    private MapNode[][] _nodes;
    private int _collectionChance = 80;

    private void GenerateMapInit()
    {
        _nodes = new MapNode[20][];
        
        _nodes[0] = new MapNode[1];
        _currentNode = CreatNode("house", 0, 0);
        MapNode[] houseNodes = new MapNode[3];
        
        _nodes[1] = new MapNode[3];
        houseNodes[0] = CreatNode("A0", 1, 0);
        houseNodes[1] = CreatNode("A1", 1, 1);
        houseNodes[2] = CreatNode("A2", 1, 2);
        _nodes[0][0].SetNextNode(houseNodes);
        NodeAction[] actions = { NodeAction.NEXTNODE_0, NodeAction.NEXTNODE_1, NodeAction.NEXTNODE_2 };
        _nodes[0][0].SetNodeAction(actions);
    }
    public void GenerateMap()
    {
        GenerateMapInit();
        for (int level = 2; level < 20; level++)
        {
            char c = (char)((int)'A' + level-1);
            int index = 0;
            var nodeNum = new int[_nodes[level - 1].Length];
            int totalNode = 0;
            for (int i = 0; i < nodeNum.Length; i++)
            {
                nodeNum[i] = Random.Range(1, 3);
                totalNode += nodeNum[i];
            }
            if (totalNode > 25)
            {
                totalNode = 0;
                for (int i = 0; i < nodeNum.Length; i++)
                {
                    nodeNum[i] = 0;
                }
                for (int i = 0; i < 25; i++)
                {
                    int randomIndex = Random.Range(0, nodeNum.Length - 1);
                    if (nodeNum[randomIndex] >= 3) continue;
                    nodeNum[randomIndex]++;
                    totalNode++;
                }
            }
            _nodes[level] = new MapNode[totalNode];
            for (int i = 0; i < _nodes[level - 1].Length; i++)
            {
                int num = nodeNum[i];
                MapNode[] nodes = new MapNode[num];
                for (int j = 0; j < num; j++)
                {
                    string id = c + index.ToString();
                    nodes[j] = CreatNode(id, level, index);
                    _nodes[level][index] = nodes[j];
                    index++;
                }
                _nodes[level - 1][i].SetNextNode(nodes);
                NodeAction[] actions = new NodeAction[3];
                for (int nodeIndex = 0; nodeIndex < nodes.Length; nodeIndex++)
                {
                    actions[nodeIndex] = (NodeAction)nodeIndex;
                }
                for (int nodeIndex = nodes.Length; nodeIndex < 3; nodeIndex++)
                {
                    int r = Random.Range(1, 100);
                    if (r > 50)
                    {
                        MapNode[] nodess = _nodes[level - 1][i].NextNode.Append(_nodes[0][0]).ToArray();
                        _nodes[level - 1][i].SetNextNode(nodess);
                        actions[nodeIndex] = (NodeAction)(_nodes[level - 1][i].NextNode.Length - 1);
                    }
                    else
                    {
                        r = Random.Range(1, 100);
                        if (r < _collectionChance) actions[nodeIndex] = NodeAction.COLLECTION;
                        else actions[nodeIndex] = NodeAction.COLLECTION;
                    }
                }
                Shuffle<NodeAction>(actions);
                _nodes[level - 1][i].SetNodeAction(actions);
                if (_nodes[level - 1][i] != null) Debug.Log(_nodes[level - 1][i].ID + ": " + _nodes[level - 1][i].NextNode.Length);
            }
        }
    }
    void Shuffle<T>(T[] array)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < array.Length; t++ )
        {
            T tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
    }

    private MapNode CreatNode(string id, int level, int index)
    {
        MapNode node = new MapNode(id, level, index);
        _nodes[level][index] = node;
        return node;
    }

    public void DoNextStep(int chosenIndex)
    {
        if (_currentNode.NodeAction[chosenIndex] == NodeAction.NEXTNODE_0)
        {
            MapNode node = _currentNode.NextNode[0];
            Debug.Log(node.ID);
            _currentNode = node;
        }
        else if (_currentNode.NodeAction[chosenIndex] == NodeAction.NEXTNODE_1)
        {
            MapNode node = _currentNode.NextNode[1];
            Debug.Log(node.ID);
            _currentNode = node;
        }
        else if (_currentNode.NodeAction[chosenIndex] == NodeAction.NEXTNODE_2)
        {
            MapNode node = _currentNode.NextNode[2];
            Debug.Log(node.ID);
            _currentNode = node;
        }
        else if (_currentNode.NodeAction[chosenIndex] == NodeAction.COLLECTION)
        {
            Debug.Log("COLLECTION");
        }
        else if (_currentNode.NodeAction[chosenIndex] == NodeAction.RECTIFICATION)
        {
            Debug.Log("RECTIFICATION");
        }
    }
}
