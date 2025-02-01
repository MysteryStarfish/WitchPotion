using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MapViewor : MonoBehaviour
{
    [SerializeField] private Button choice1;
    [SerializeField] private Button choice2;
    [SerializeField] private Button choice3;
    private void Start()
    {
        UpdateButtonText();
    }

    private void HandleText(Button button, NodeAction action, MapNode[] nodes)
    {
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        text.text = action.ToString();
        if (action == NodeAction.NEXTNODE_0) text.text = nodes[0].ID;
        if (action == NodeAction.NEXTNODE_1) text.text = nodes[1].ID;
        if (action == NodeAction.NEXTNODE_2) text.text = nodes[2].ID;
    }

    public void UpdateButtonText()
    {
        NodeAction action1 = MapController.Instance.CurrentNode.NodeAction[0];
        NodeAction action2 = MapController.Instance.CurrentNode.NodeAction[1];
        NodeAction action3 = MapController.Instance.CurrentNode.NodeAction[2];
        MapNode[] nodes = MapController.Instance.CurrentNode.NextNode;

        HandleText(choice2, action2, nodes);
        HandleText(choice1, action1, nodes);
        HandleText(choice3, action3, nodes);
    }
}
