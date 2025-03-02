using MessagePipe;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Map
{
    public class MapViewer : MonoBehaviour
    {
        [Inject] private ISubscriber<UpdateButtonViewRequest.UpdateButtonViewRequest> _subscriber;
        [Inject] private MapController _mapController;

        [SerializeField] private TMP_Text history;
        [SerializeField] private TMP_Text leftStep;
        
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
            _subscriber.Subscribe(OnUpdateButtonViewRequested);
            Debug.Log($"Subscribe(OnUpdateButtonViewRequested");
            UpdateText();
        }
        private void OnUpdateButtonViewRequested(UpdateButtonViewRequest.UpdateButtonViewRequest request)
        {
            Debug.Log($"UpdateText");
            UpdateText();
        }
        private void GetAction()
        {
            _action1 = _mapController.CurrentNode.NodeAction[0];
            _action2 = _mapController.CurrentNode.NodeAction[1];
            _action3 = _mapController.CurrentNode.NodeAction[2];
            _nodes = _mapController.CurrentNode.NextNode;
        }

        private void UpdateText()
        {
            GetAction();
            UpdateButtonText();
            UpdateConditionText();
            UpdateHistory();
            UpdateLeftStep();
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
            if (action.IsHide)text.text = NodeActionType.COLLECTION.ToString();
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
            if (action.LockType != null)
            {
                Debug.Log(action.LockType.Type.ToString());
                text.text += action.LockType.Type.ToString();
            }   
        }
        private void UpdateHistory()
        {
            history.text = _mapController.CurrentNode.ID.ToString();
            if (_mapController.LastNode.Length != 0) history.text = _mapController.LastNode[^1].ID.ToString() + "->" + _mapController.CurrentNode.ID.ToString();
        }

        private void UpdateLeftStep()
        {
            leftStep.text = "剩下:" + _mapController.CurrentStep.ToString();
        }
    }
}
