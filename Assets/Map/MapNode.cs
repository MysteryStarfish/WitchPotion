public enum NodeAction
{
    NEXTNODE_0,
    NEXTNODE_1,
    NEXTNODE_2,
    COLLECTION,
    RECTIFICATION
}
public class MapNode
{
    public string ID { get; private set; }
    public int Level { get; private set; }
    public int Index { get; private set; }
    public NodeAction[] NodeAction { get; private set; }
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
    public void SetNodeAction(NodeAction[] actions)
    {
        NodeAction = actions;
    }
}
