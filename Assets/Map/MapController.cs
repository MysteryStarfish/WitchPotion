using System.Linq;
using Map;
using UnityEngine;
using UpdateButtonViewRequest;

public class MapController
{
    private static MapController _instance;

    private static UpdateButtonViewPublisher _publisher;
    public MapController(UpdateButtonViewPublisher publisher)
    {
        _publisher = publisher;
    }
    public static MapController Instance
    {
        get
        {
            if (_instance == null) _instance = new MapController(_publisher);
            return _instance;
        }
    }

    private MapNode _currentNode;
    public MapNode CurrentNode => _currentNode;
    private MapNode[][] _mapNodes;
    private readonly int _collectionChance = 80;
    private readonly int _levelMax = 20;
    private MapNode[] _lastNode = new MapNode[] {};


    public void GenerateMap()
    {
        GenerateMapInit();
        for (int level = 2; level < _levelMax; level++)
        {
            char idAlphabet = (char)((int)'A' + level-1);
            int idNumber = 0;
            
            var nodesAmount = new int[_mapNodes[level - 1].Length];
            int currentLevelTotalNode = 0;
            currentLevelTotalNode = HandleCurrentLevelNodesAmount(nodesAmount, currentLevelTotalNode);
            
            _mapNodes[level] = new MapNode[currentLevelTotalNode];
            for (int i = 0; i < _mapNodes[level - 1].Length; i++)
            {
                int nextNodesAmount = nodesAmount[i];
                var nodes = SetupNextNodes(nextNodesAmount, idAlphabet, level, i, ref idNumber);
                if (_mapNodes[level - 1][i] != null) Debug.Log(_mapNodes[level - 1][i].ID + ": " + _mapNodes[level - 1][i].NextNode.Length);
            }
        }
        for (int level = 2; level <= _levelMax; level++)
        {
            for (int i = 0; i < _mapNodes[level - 1].Length; i++)
            {
                var nodes = _mapNodes[level-1][i].NextNode;
                SetupActions(nodes, level, i);
                if (_mapNodes[level - 1][i] != null) Debug.Log(_mapNodes[level - 1][i].ID + ": " + _mapNodes[level - 1][i].NextNode.Length);
            }
        }
    }
    private void GenerateMapInit()
    {
        _mapNodes = new MapNode[_levelMax+1][];
        
        _mapNodes[0] = new MapNode[1];
        _currentNode = CreatNode("house", 0, 0);
        MapNode[] houseNodes = new MapNode[3];
        
        SetupNextNodes(houseNodes);
        SetupNodeActions();
    }

    private void SetupNodeActions()
    {
        NodeAction<Potion>[] actions = new NodeAction<Potion>[3];
        for (int nodeIndex = 0; nodeIndex < 3; nodeIndex++)
        {
            NodeActionType actionType = (NodeActionType)nodeIndex;
            Potion[] condition = new Potion[] {};
            NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition , 1);
            actions[nodeIndex] = action;
        }
        _mapNodes[0][0].SetNodeAction(actions);
    }

    private void SetupNextNodes(MapNode[] houseNodes)
    {
        _mapNodes[1] = new MapNode[3];
        houseNodes[0] = CreatNode("A0", 1, 0);
        houseNodes[1] = CreatNode("A1", 1, 1);
        houseNodes[2] = CreatNode("A2", 1, 2);
        _mapNodes[0][0].SetNextNode(houseNodes);
    }

    private MapNode[] SetupNextNodes(int nextNodesAmount, char idAlphabet, int level, int i, ref int idNumber)
    {
        MapNode[] nodes = new MapNode[nextNodesAmount];
        for (int j = 0; j < nextNodesAmount; j++)
        {
            string id = idAlphabet + idNumber.ToString();
            nodes[j] = CreatNode(id, level, idNumber);
            _mapNodes[level][idNumber] = nodes[j];
            idNumber++;
        }
        _mapNodes[level - 1][i].SetNextNode(nodes);
        return nodes;
    }
    private MapNode CreatNode(string id, int level, int index)
    {
        MapNode node = new MapNode(id, level, index);
        _mapNodes[level][index] = node;
        return node;
    }

    private void SetupActions(MapNode[] nodes, int level, int i)
    {
        NodeAction<Potion>[] actions = new NodeAction<Potion>[3];
        if (level == _levelMax)
        {
            SetGoal(nodes, level, i, actions);
            return;
        }

        int wayActionAmount = 0;
        for (int nodeIndex = 0; nodeIndex < nodes.Length; nodeIndex++)
        {
            SetNormalActions(nodeIndex, actions);
            wayActionAmount++;
        }
        int isWayIndex = Random.Range(nodes.Length, 3-1);
        for (int nodeIndex = nodes.Length; nodeIndex < 3; nodeIndex++)
        {
            if (nodeIndex < isWayIndex && level >= 3)
            {
                ConnectOtherNodes(level, i, actions, nodeIndex);
                wayActionAmount++;
            }
            else
            {
                SetSpecialActions(actions, nodeIndex);
            }
        }

        if (level >= 12)
        {
            foreach (var action in actions)
            {
                if (action.ActionType >= (NodeActionType)3) break;
                var potion = ScriptableObject.CreateInstance<Potion>();
                potion.potionName = "TestPotion";
                action.AddCondition(potion);
                action.Locked = true;
            }
        }
        else if (level >= 8)
        {
            int potionAction = Random.Range(0, wayActionAmount-1);
            foreach (var action in actions)
            {
                if (action.ActionType >= (NodeActionType)3) break;
                int addConditionChance = Random.Range(1, 100);
                if (addConditionChance >= 50 || action == actions[potionAction])
                {
                    var potion = ScriptableObject.CreateInstance<Potion>();
                    potion.potionName = "TestPotion";
                    action.AddCondition(potion);
                    action.Locked = true;
                }
            }
        }
        else if (level >= 3)
        {
            SetOneWayNeededCondition(wayActionAmount, actions);
        }
        foreach (var action in actions)
        {
            Debug.Log(action.Conditions.Length);
            if (action.Conditions.Length > 0) Debug.Log(action.Conditions[0].potionName);
        }
        
        Shuffle<NodeAction<Potion>>(actions);
        _mapNodes[level - 1][i].SetNodeAction(actions);
    }

    private static void SetOneWayNeededCondition(int wayActionAmount, NodeAction<Potion>[] actions)
    {
        if (wayActionAmount < 2) return;
        int potionAction = Random.Range(0, wayActionAmount-1);
        foreach (var action in actions)
        {
            if (action == actions[potionAction])
            {
                var potion = ScriptableObject.CreateInstance<Potion>();
                potion.potionName = "TestPotion";
                action.AddCondition(potion);
                action.Locked = true;
            }
        }
    }

    private void SetSpecialActions(NodeAction<Potion>[] actions, int nodeIndex)
    {
        int r = Random.Range(1, 100);
        NodeActionType actionType;
        if (r < _collectionChance) actionType = NodeActionType.COLLECTION;
        else actionType = NodeActionType.RECTIFICATION;
                
        Potion[] condition = new Potion[] {};
        NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1);
        actions[nodeIndex] = action;
    }

    private void ConnectOtherNodes(int level, int i, NodeAction<Potion>[] actions, int nodeIndex)
    {
        MapNode[] nodes;
        int randomLevel = Random.Range(level - 2, level);
        if (level >= 8) randomLevel = Random.Range(level - 2, level + 2);
        if (randomLevel >= _levelMax) randomLevel = Random.Range(level - 2, _levelMax-1);
        int randomIndex = Random.Range(0, _mapNodes[randomLevel].Length-1);
        nodes = _mapNodes[level - 1][i].NextNode.Append(_mapNodes[randomLevel][randomIndex]).ToArray();
        _mapNodes[level - 1][i].SetNextNode(nodes);
                
        NodeActionType actionType = (NodeActionType)(_mapNodes[level - 1][i].NextNode.Length - 1);
        Potion[] condition = new Potion[] {};
        NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1);
        actions[nodeIndex] = action;
    }

    private static void SetNormalActions(int nodeIndex, NodeAction<Potion>[] actions)
    {
        NodeActionType actionType = (NodeActionType)nodeIndex;
        Potion[] condition = new Potion[] {};
        NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1);
        actions[nodeIndex] = action;
    }

    private void SetGoal(MapNode[] nodes, int level, int i, NodeAction<Potion>[] actions)
    {
        for (int nodeIndex = 0; nodeIndex < nodes.Length; nodeIndex++)
        {
            NodeActionType actionType = NodeActionType.GOAL;
            Potion[] condition = new Potion[] {};
            NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition , 1)
            {
                Locked = true
            };
            actions[nodeIndex] = action;
        }
        Shuffle<NodeAction<Potion>>(actions);
        _mapNodes[level - 1][i].SetNodeAction(actions);
    }

    private static int HandleCurrentLevelNodesAmount(int[] nodesAmount, int currentLevelTotalNode)
    {
        for (int i = 0; i < nodesAmount.Length; i++)
        {
            nodesAmount[i] = Random.Range(1, 3);
            currentLevelTotalNode += nodesAmount[i];
        }

        if (currentLevelTotalNode > 25)
        {
            currentLevelTotalNode = ResetNodesAmount(nodesAmount);
            for (int i = 0; i < 25; i++)
            {
                int randomIndex = Random.Range(0, nodesAmount.Length - 1);
                if (nodesAmount[randomIndex] >= 3) continue;
                nodesAmount[randomIndex]++;
                currentLevelTotalNode++;
            }
        }

        return currentLevelTotalNode;
    }

    private static int ResetNodesAmount(int[] nodesAmount)
    {
        var currentLevelTotalNode = 0;
        for (int i = 0; i < nodesAmount.Length; i++)
        {
            nodesAmount[i] = 0;
        }

        return currentLevelTotalNode;
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

    public void DoNextStep(int chosenIndex)
    {
        if (chosenIndex == -1)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = _lastNode[^1];
            var lstLastNode = _lastNode.ToList();
            lstLastNode.RemoveAt(_lastNode.Length - 1);
            _lastNode = lstLastNode.ToArray();
            _currentNode = node;
            _publisher.RequestUpdateButtonView();
            return;
        }
        NodeAction<Potion> nodeAction = _currentNode.NodeAction[chosenIndex];
        if (nodeAction.Locked) return;
        nodeAction.UsedAction();
        if (nodeAction.ActionType == NodeActionType.NEXTNODE_0)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = _currentNode.NextNode[0];
            Debug.Log(node.ID);
            _lastNode = _lastNode.Append(_currentNode).ToArray();
            _currentNode = node;
            
        }
        else if (nodeAction.ActionType == NodeActionType.NEXTNODE_1)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = _currentNode.NextNode[1];
            Debug.Log(node.ID);
            _lastNode = _lastNode.Append(_currentNode).ToArray();
            _currentNode = node;
        }
        else if (nodeAction.ActionType == NodeActionType.NEXTNODE_2)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = _currentNode.NextNode[2];
            Debug.Log(node.ID);
            _lastNode = _lastNode.Append(_currentNode).ToArray();
            _currentNode = node;
        }
        else if (nodeAction.ActionType == NodeActionType.COLLECTION)
        {
            Debug.Log("COLLECTION");
        }
        else if (nodeAction.ActionType == NodeActionType.RECTIFICATION)
        {
            Debug.Log("RECTIFICATION");
        }
        _publisher.RequestUpdateButtonView();
    }

    private void ResetActionsTimes(NodeAction<Potion>[] nodeAction)
    {
        for (int i = 0; i < nodeAction.Length; i++)
        {
            nodeAction[i].ResetUsedTimes();
        }
    }
}
