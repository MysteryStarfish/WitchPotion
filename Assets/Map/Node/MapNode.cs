using Map;
using UnityEngine;

public class MapNode
{
    private readonly string[] _elements = { "魔法", "物理", "風", "水", "火" };
    public string ID { get; private set; }
    public int Level { get; private set; }
    public int Index { get; private set; }
    public string Element { get; private set; }
    public int ElementIndex { get; private set; }
    public NodeAction<Potion>[] NodeAction { get; private set; }
    public MapNode[] NextNode { get; private set; }
    public bool IsHide { get; private set; }
    public MapNode(string id, int level, int index)
    {
        this.ID = id;
        this.Level = level;
        this.Index = index;
        NextNode = new MapNode[3];
        ElementIndex = Random.Range(0, _elements.Length);
        Element = _elements[ElementIndex];
    }
    public void SetNextNode(MapNode[] nextNode)
    {
        NextNode = nextNode;
    }
    public void SetNodeAction(NodeAction<Potion>[] actions)
    {
        NodeAction = actions;
    }
    public void SetElement(string element)
    {
        Element = element;
    }
    public void HideNode()
    {
        IsHide = true;
    }
    public void ShowNode()
    {
        IsHide = false;
    }
}
