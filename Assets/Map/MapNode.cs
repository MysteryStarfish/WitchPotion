using Map;

public class MapNode
{
    public string ID { get; private set; }
    public int Level { get; private set; }
    public int Index { get; private set; }
    public NodeAction<Potion>[] NodeAction { get; private set; }
    public MapNode[] NextNode { get; private set; }
    public MapNode(string id, int level, int index)
    {
        this.ID = id;
        this.Level = level;
        this.Index = index;
        NextNode = new MapNode[3];
    }
    public void SetNextNode(MapNode[] nextNode)
    {
        NextNode = nextNode;
    }
    public void SetNodeAction(NodeAction<Potion>[] actions)
    {
        NodeAction = actions;
    }
}
