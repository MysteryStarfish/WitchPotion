using System;
using System.Linq;
using Map;
using Map.PlayStoryEvent;
using Map.PotionButton;
using MessagePipe;
using NodeChange;
using UnityEngine;
using VContainer;
using WitchPotion.Bag;
using WitchPotion.Story;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MapController
{

    [Inject] private IPublisher<UpdateButtonViewRequest.UpdateButtonViewRequest> _updateButtonViewRequestPublisher;
    [Inject] private IPublisher<PlayStoryRequest> _playStoryRequestPublisher;
    [Inject] private ISubscriber<NodeChangeRequest> _nodeSubscriber;
    [Inject] private ISubscriber<UsePotionRemoveObstacleRequest> _potionSubscriber;
    [Inject]
    private BagContext _bagContext;
    private HerbBag HerbBag => this._bagContext.HerbBag;
    private PotionBag PotionBag => this._bagContext.PotionBag;

    private void RequestUpdateButtonView()
    {
        Debug.Log($"Publish(UpdateButtonViewRequest");
        _updateButtonViewRequestPublisher.Publish(new UpdateButtonViewRequest.UpdateButtonViewRequest());
    }

    private MapController(ISubscriber<NodeChangeRequest> nodeSubscriber, IPublisher<UpdateButtonViewRequest.UpdateButtonViewRequest> updateButtonViewRequestPublisher, ISubscriber<UsePotionRemoveObstacleRequest> potionSubscriber)
    {
        _nodeSubscriber = nodeSubscriber;
        _updateButtonViewRequestPublisher = updateButtonViewRequestPublisher;
        _potionSubscriber = potionSubscriber;

        _nodeSubscriber.Subscribe(OnNodeChangeRequested);
        _potionSubscriber.Subscribe(OnUsePotionRemoveObstacleRequested);
        Debug.Log($"Subscribe OnNodeChangeRequested");
        GenerateMap();
    }
    private void OnNodeChangeRequested(NodeChangeRequest request)
    {
        Debug.Log($"Changing scene to: {request.ChosenIndex}");
        DoNextStep(request.ChosenIndex);
    }

    private void OnUsePotionRemoveObstacleRequested(UsePotionRemoveObstacleRequest request)
    {
        RemoveObstacle(request.PotionID, request.ButtonIndex);
    }

    private MapNode _currentNode;
    public MapNode CurrentNode => _currentNode;
    public MapNode[][] MapNodes { get; private set; }
    private readonly int _collectionChance = 80;
    private readonly int _roadChance = 80;
    private readonly int _levelMax = 20;
    private static int _nodeLimitIndex = 0;
    private static readonly int[] NodeLimit = { 10, 9, 8, 6, 4, 3 };
    private readonly int _nodeLimitChange = 15;
    
    private int[] _levelHideDoorAppearTimes;
    
    public readonly int MaxStep = 15;
    public int CurrentStep { get; private set; } = 15;
    public MapNode[] LastNode { get; private set; } = new MapNode[] { };


    public void GenerateMap()
    {
        GenerateMapInit();
        for (int level = 2; level < _levelMax; level++)
        {
            char idAlphabet = (char)((int)'A' + level - 1);
            int idNumber = 0;

            var nodesAmount = new int[MapNodes[level - 1].Length];
            int currentLevelTotalNode = 0;
            if (level > _nodeLimitChange) _nodeLimitIndex++;
            currentLevelTotalNode = HandleCurrentLevelNodesAmount(nodesAmount, currentLevelTotalNode);

            MapNodes[level] = new MapNode[currentLevelTotalNode];
            for (int i = 0; i < MapNodes[level - 1].Length; i++)
            {
                int nextNodesAmount = nodesAmount[i];
                var nodes = SetupNextNodes(nextNodesAmount, idAlphabet, level, i, ref idNumber);
                if (MapNodes[level - 1][i] != null) Debug.Log(MapNodes[level - 1][i].ID + ": " + MapNodes[level - 1][i].NextNode.Length);
            }
        }

        for (int level = 2; level <= _levelMax; level++)
        {
            for (int i = 0; i < MapNodes[level - 1].Length; i++)
            {
                var nodes = MapNodes[level - 1][i].NextNode;
                SetupActions(nodes, level, i);
                if (MapNodes[level - 1][i] != null) Debug.Log(MapNodes[level - 1][i].ID + ": " + MapNodes[level - 1][i].NextNode.Length);
            }
        }
    }
    private void GenerateMapInit()
    {
        MapNodes = new MapNode[_levelMax + 1][];

        _levelHideDoorAppearTimes = new int[_levelMax];

        MapNodes[0] = new MapNode[1];
        _currentNode = CreatNode("house", 0, 0);
        MapNode[] houseNodes = new MapNode[3];

        SetupInitNodes(houseNodes);
        SetupNodeActions();
    }

    private void SetupNodeActions()
    {
        NodeAction<Potion>[] actions = new NodeAction<Potion>[3];
        for (int nodeIndex = 0; nodeIndex < 3; nodeIndex++)
        {
            NodeActionType actionType = (NodeActionType)nodeIndex;
            Potion[] condition = new Potion[] { };
            NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1);
            actions[nodeIndex] = action;
        }
        MapNodes[0][0].SetNodeAction(actions);
    }
    private void SetupInitNodes(MapNode[] houseNodes)
    {
        MapNodes[1] = new MapNode[3];
        
        houseNodes[0] = CreatNode("A0", 1, 0);
        houseNodes[1] = CreatNode("A1", 1, 1);
        houseNodes[2] = CreatNode("A2", 1, 2);
        MapNodes[0][0].SetNextNode(houseNodes);
    }

    private MapNode[] SetupNextNodes(int nextNodesAmount, char idAlphabet, int level, int i, ref int idNumber)
    {
        MapNode[] nodes = new MapNode[nextNodesAmount];
        for (int j = 0; j < nextNodesAmount; j++)
        {
            string id = idAlphabet + idNumber.ToString();
            nodes[j] = CreatNode(id, level, idNumber);
            MapNodes[level][idNumber] = nodes[j];
            idNumber++;
        }
        MapNodes[level - 1][i].SetNextNode(nodes);
        return nodes;
    }
    private MapNode CreatNode(string id, int level, int index)
    {
        MapNode node = new MapNode(id, level, index);
        MapNodes[level][index] = node;
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
        
        Debug.Log("HideDoor");
        int[] nodesIndex = new int[MapNodes[level].Length];
        Debug.Log(MapNodes[level].Length);
        for (int _ = 0; _ < MapNodes[level].Length; _++)
        {
            nodesIndex[_] = _;
        }
        Shuffle<int>(nodesIndex);
        for (int _ = 0; _ < actions.Length; _++)
        {
            int idx = nodesIndex[_];
            MapNode node = MapNodes[level][idx];
            Debug.Log(node.HideChecked);
            if (node.HideChecked) continue;
            MapNodes[level][idx].HideChecked = true;
            if (_levelHideDoorAppearTimes[level] > 3) break;
            _levelHideDoorAppearTimes[level]++;
            float go = Random.Range(0, 100);
            int times = 0;
            while (go > 20 + times*10 && node != null)
            {
                go = Random.Range(0, 100);
                if (node.NextNode == null) break;
                if (node.NextNode.Length == 0) continue;
                int[] nextIdxs = new int[node.NextNode.Length];
                for (int __ = 0; __ < nextIdxs.Length; __++)
                {
                    nextIdxs[__] = __;
                }
                Shuffle<int>(nextIdxs);
                int nextIdx = 0;
                for (int __ = 0; __ < nextIdxs.Length; __++)
                {
                    Debug.Log("NextNode: " + __ + " " + node.ID);
                    if (node.NextNode[__] == null) continue;
                    nextIdx = __;
                    break;
                }
                for (int k = 0; k < node.NextNode.Length; k++)
                {
                    if (node.NextNode[k] == null) continue;
                    if (k == nextIdx) node.NextNode[k].IsHide = true;
                    node.NextNode[k].HideChecked = true;
                }
                Debug.Log("HideDoor" + node.NextNode.Length.ToString() + nextIdx.ToString());
                node = node.NextNode[nextIdx];
                times++;
                Debug.Log("next: " + go.ToString() +" "+ (times * 10).ToString());
                if (node != null ) Debug.Log(node.ID.ToString());
            }
        }
        
        for (int _ = 0; _ < MapNodes[level - 1][i].NextNode.Length; _++)
        {
            if (MapNodes[level - 1][i].NextNode[_].IsHide) AddHideNode(actions[_]); 
        }
        
        for (int nodeIndex = nodes.Length; nodeIndex < 3; nodeIndex++)
        {
            int chance = Random.Range(1, 100);
            if (chance > _roadChance && level >= 3)
            {
                ConnectOtherNodes(level, i, actions, nodeIndex);
                wayActionAmount++;
            }
            else
            {
                SetSpecialActions(actions, nodeIndex);
            }
        }

        SetObstacle(level, actions, wayActionAmount);

        Shuffle<NodeAction<Potion>>(actions);
        
        MapNodes[level - 1][i].SetNodeAction(actions);
    }

    private void SetObstacle(int level, NodeAction<Potion>[] actions, int wayActionAmount)
    {
        if (level >= 12)
        {
            foreach (var action in actions)
            {
                if (action.ActionType >= (NodeActionType)3) break;
                if (action.IsHide == false) AddObstacle(action);
            }
        }
        else if (level >= 8)
        {
            int potionAction = Random.Range(0, wayActionAmount - 1);
            foreach (var action in actions)
            {
                if (action.ActionType >= (NodeActionType)3) break;
                int addConditionChance = Random.Range(1, 100);
                if (addConditionChance >= 50 || action == actions[potionAction])
                {
                    if (action.IsHide == false) AddObstacle(action);
                }
            }
        }
        else if (level >= 3)
        {
            SetOneWayNeededCondition(wayActionAmount, actions);
        }
    }
    private void SetOneWayNeededCondition(int wayActionAmount, NodeAction<Potion>[] actions)
    {
        if (wayActionAmount < 2) return;
        int potionAction = Random.Range(0, wayActionAmount - 1);
        foreach (var action in actions)
        {
            if (action == actions[potionAction])
            {
                if (action.IsHide == false) AddObstacle(action);
            }
        }
    }
    private void AddObstacle(NodeAction<Potion> action)
    {
        int type = Random.Range(0, 0);
        Obstacle obstacle = new Obstacle(type, 0);
        action.LockType = obstacle;
        Debug.Log(obstacle.Type.ToString());
        action.LockAction();
    }
    private void AddHideNode(NodeAction<Potion> action)
    {
        int type = 9;
        Obstacle obstacle = new Obstacle(type, 0);
        action.LockType = obstacle;
        Debug.Log(obstacle.Type.ToString());
        action.HideNode();
    }
    private void RemoveObstacle(string potionID, int chosenIndex)
    {
        Potion potion = PotionBag.Get(potionID);
        NodeAction<Potion> action = _currentNode.NodeAction[chosenIndex];
        if (action.LockType == null) return;
        if (action.LockType.IsCorrectPotion(potion) && action.LockType.Type != (Obstacle.ObstacleType)9)
        {
            action.UnlockAction();
            RequestUpdateButtonView();
        }
        else if (action.LockType.IsCorrectPotion(potion) && action.LockType.Type == (Obstacle.ObstacleType)9)
        {
            action.CancelHideNode();
            action.UnlockAction();
            RequestUpdateButtonView();
        }
    }
    private void SetSpecialActions(NodeAction<Potion>[] actions, int nodeIndex)
    {
        int r = Random.Range(1, 100);
        NodeActionType actionType;
        if (r < _collectionChance) actionType = NodeActionType.COLLECTION;
        else actionType = NodeActionType.RECTIFICATION;

        Potion[] condition = new Potion[] { };
        NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1);
        actions[nodeIndex] = action;
    }
    private void ConnectOtherNodes(int level, int i, NodeAction<Potion>[] actions, int nodeIndex)
    {
        int randomLevel = Random.Range(level - 2, level+2);
        if (level >= 8) randomLevel = Random.Range(level - 2, level + 3);
        if (randomLevel >= _levelMax) randomLevel = Random.Range(level - 2, _levelMax);
        
        int[] randomIndexes = new int[MapNodes[randomLevel].Length];
        for (int _ = 0; _ < MapNodes[randomLevel].Length; _++)
        {
            randomIndexes[_] = _;
        }
        Shuffle<int>(randomIndexes);
        int randomIndex = randomIndexes[0];
        for (int _ = 1; _ < MapNodes[randomLevel].Length; _++)
        {
            if (randomIndex != nodeIndex) break;
            randomIndex = randomIndexes[_];
        }
        
        var nodes = MapNodes[level - 1][i].NextNode.Append(MapNodes[randomLevel][randomIndex]).ToArray();
        MapNodes[level - 1][i].SetNextNode(nodes);

        NodeActionType actionType = (NodeActionType)(MapNodes[level - 1][i].NextNode.Length - 1);
        Potion[] condition = new Potion[] { };
        NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1);
        actions[nodeIndex] = action;
    }
    private static void SetNormalActions(int nodeIndex, NodeAction<Potion>[] actions)
    {
        NodeActionType actionType = (NodeActionType)nodeIndex;
        Potion[] condition = new Potion[] { };
        NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1);
        actions[nodeIndex] = action;
    }
    private void SetGoal(MapNode[] nodes, int level, int i, NodeAction<Potion>[] actions)
    {
        for (int nodeIndex = 0; nodeIndex < nodes.Length; nodeIndex++)
        {
            NodeActionType actionType = NodeActionType.GOAL;
            Potion[] condition = new Potion[] { };
            NodeAction<Potion> action = new NodeAction<Potion>(actionType, condition, 1)
            {
                Locked = true
            };
            actions[nodeIndex] = action;
        }
        Shuffle<NodeAction<Potion>>(actions);
        MapNodes[level - 1][i].SetNodeAction(actions);
    }
    private static int HandleCurrentLevelNodesAmount(int[] nodesAmount, int currentLevelTotalNode)
    {
        int nodeLimit = NodeLimit[_nodeLimitIndex];
        for (int i = 0; i < nodesAmount.Length; i++)
        {
            nodesAmount[i] = Random.Range(1, 3);
            currentLevelTotalNode += nodesAmount[i];
        }
        if (currentLevelTotalNode > nodeLimit)
        {
            currentLevelTotalNode = ResetNodesAmount(nodesAmount);
            for (int i = 0; i < nodeLimit; i++)
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
        for (int t = 0; t < array.Length; t++)
        {
            T tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
    }
    private void DoNextStep(int chosenIndex)
    {
        if (CurrentStep <= 0)
        {
            Restart();
            RequestUpdateButtonView();
            return;
        }
        CurrentStep--;
        if (chosenIndex == -1)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = LastNode[^1];
            var lstLastNode = LastNode.ToList();
            lstLastNode.RemoveAt(LastNode.Length - 1);
            LastNode = lstLastNode.ToArray();
            _currentNode = node;
            RequestUpdateButtonView();
            return;
        }
        NodeAction<Potion> nodeAction = _currentNode.NodeAction[chosenIndex];
        if (nodeAction.Locked) return;
        nodeAction.UsedAction();
        if (nodeAction.ActionType == NodeActionType.NEXTNODE_0 && !nodeAction.IsHide)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = _currentNode.NextNode[0];
            Debug.Log(node.ID);
            LastNode = LastNode.Append(_currentNode).ToArray();
            _currentNode = node;
        }
        else if (nodeAction.ActionType == NodeActionType.NEXTNODE_1 && !nodeAction.IsHide)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = _currentNode.NextNode[1];
            Debug.Log(node.ID);
            LastNode = LastNode.Append(_currentNode).ToArray();
            _currentNode = node;
        }
        else if (nodeAction.ActionType == NodeActionType.NEXTNODE_2 && !nodeAction.IsHide)
        {
            ResetActionsTimes(_currentNode.NodeAction);
            MapNode node = _currentNode.NextNode[2];
            Debug.Log(node.ID);
            LastNode = LastNode.Append(_currentNode).ToArray();
            _currentNode = node;
        }
        else if (nodeAction.ActionType == NodeActionType.COLLECTION || nodeAction.IsHide)
        {
            Debug.Log("COLLECTION");
            string herbElement = CurrentNode.ElementIndex.ToString();
            string herbType = Random.Range(1, 6).ToString();
            if (HerbBag.Get(herbElement + "0" + herbType) == null) herbType = "1";
            int herbAmount = HerbBag.GetCount(herbElement + "0" + herbType);
            HerbBag.SetCount(herbElement + "0" + herbType, herbAmount+1);
        }
        else if (nodeAction.ActionType == NodeActionType.RECTIFICATION)
        {
            Debug.Log("RECTIFICATION");
            RectificationPanel panel = GameObject.FindFirstObjectByType<RectificationPanel>(FindObjectsInactive.Include);
            panel.gameObject.SetActive(true);
        }
        RequestUpdateButtonView();
    }

    private void Restart()
    {
        LastNode =  new MapNode[] { };
        _currentNode = MapNodes[0][0];
        CurrentStep = MaxStep;
    }
    private void ResetActionsTimes(NodeAction<Potion>[] nodeAction)
    {
        if (Object.FindFirstObjectByType<PlayStory>() != null) _playStoryRequestPublisher.Publish(new PlayStoryRequest(1));
        for (int i = 0; i < nodeAction.Length; i++)
        {
            if (nodeAction[i].IsHide) _playStoryRequestPublisher.Publish(new PlayStoryRequest(0));
            nodeAction[i].ResetUsedTimes();
        }
    }
}
