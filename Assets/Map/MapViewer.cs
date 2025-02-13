using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapViewer : MonoBehaviour
    {
        [SerializeField] private Button choice1;
        [SerializeField] private Button choice2;
        [SerializeField] private Button choice3;

        [SerializeField] private TMP_Text condition1;
        [SerializeField] private TMP_Text condition2;
        [SerializeField] private TMP_Text condition3;
        
        private NodeAction<Potion> _action1;
        private NodeAction<Potion> _action2;
        private NodeAction<Potion> _action3;
        private MapNode[] _nodes;
        private void Start()
        {
            UpdateText();
        }
        private void GetAction()
        {
            _action1 = MapController.Instance.CurrentNode.NodeAction[0];
            _action2 = MapController.Instance.CurrentNode.NodeAction[1];
            _action3 = MapController.Instance.CurrentNode.NodeAction[2];
            _nodes = MapController.Instance.CurrentNode.NextNode;
        }
        public void UpdateText()
        {
            GetAction();
            UpdateButtonText();
            UpdateConditionText();
        }

        private void UpdateButtonText()
        {
            HandleButtonText(choice1, _action1, _nodes);
            HandleButtonText(choice2, _action2, _nodes);
            HandleButtonText(choice3, _action3, _nodes);
        }

        private void HandleButtonText(Button button, NodeAction<Potion> action, MapNode[] nodes)
        {
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.text = action.ActionType.ToString();
            NodeActionType actionType = action.ActionType;
            if (actionType == NodeActionType.NEXTNODE_0) text.text = nodes[0].ID;
            if (actionType == NodeActionType.NEXTNODE_1) text.text = nodes[1].ID;
            if (actionType == NodeActionType.NEXTNODE_2) text.text = nodes[2].ID;
            if (action.Locked) button.GetComponent<Image>().color = new Color(155f/255f, 155f/255f, 155f/255f);
            else button.GetComponent<Image>().color = new Color(255f/255f, 255f/255f, 255f/255f);
        }

        private void UpdateConditionText()
        {
            HandleConditionText(condition1, _action1);
            HandleConditionText(condition2, _action2);
            HandleConditionText(condition3, _action3);
        }
        private void HandleConditionText(TMP_Text text, NodeAction<Potion> action)
        {
            text.text = "";
            foreach (var potion in action.Conditions)
            {
                text.text += potion.potionName + "\n";
            }
        }
    }
}
