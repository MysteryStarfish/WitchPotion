using Map.PlayStoryEvent;
using MessagePipe;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;
using WitchPotion.Story;

namespace Map
{
    public class MapViewer : MonoBehaviour
    {
        [Inject] private ISubscriber<UpdateButtonViewRequest.UpdateButtonViewRequest> _updateButtonViewRequestSubscriber;
        [Inject] private ISubscriber<PlayStoryRequest> _playStoryRequestSubscriber;
        [Inject] private MapController _mapController;

        [SerializeField] private TMP_Text history;
        [SerializeField] private TMP_Text leftStep;
        
        [SerializeField] private Button choice1;
        [SerializeField] private Button choice2;
        [SerializeField] private Button choice3;

        [SerializeField] private TMP_Text condition1;
        [SerializeField] private TMP_Text condition2;
        [SerializeField] private TMP_Text condition3;
        
        [SerializeField] private Sprite normal;
        [SerializeField] private Sprite obstacle石頭堆;
        [SerializeField] private Sprite obstacle刺藤團;
        [SerializeField] private Sprite obstacle牧草捆;
        [SerializeField] private Sprite obstacle黏液網;
        [SerializeField] private Sprite obstacle蛛絲地;
        [SerializeField] private Sprite obstacle瘴氣區;
        [SerializeField] private Sprite obstacle凍土台;
        [SerializeField] private Sprite obstacle蒸氣口;
        [SerializeField] private Sprite obstacle炎岩區;
        [SerializeField] private Sprite obstacle迷幻院;
        
        private NodeAction<Potion> _action1;
        private NodeAction<Potion> _action2;
        private NodeAction<Potion> _action3;
        private MapNode[] _nodes;
        
        [SerializeField] private PlayStory storyPrefab;
        [SerializeField] private PlayStory story;

        private void Start()
        {
            // story = Instantiate(storyPrefab);
            story.gameObject.SetActive(false);
            _updateButtonViewRequestSubscriber.Subscribe(OnUpdateButtonViewRequested);
            _playStoryRequestSubscriber.Subscribe(OnPlayStoryRequested);
            Debug.Log($"Subscribe(OnUpdateButtonViewRequested");
            UpdateText();
        }
        private void OnUpdateButtonViewRequested(UpdateButtonViewRequest.UpdateButtonViewRequest request)
        {
            Debug.Log($"UpdateText");
            UpdateText();
        }

        private void OnPlayStoryRequested(PlayStoryRequest request)
        {
            Debug.Log($"PlayStory");
            PlayStory(request._isFinished, request._showWord);
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
            button.GetComponent<Image>().color = Color.white;
            if (actionType == NodeActionType.NEXTNODE_0) text.text = nodes[0].ID;
            if (actionType == NodeActionType.NEXTNODE_1) text.text = nodes[1].ID;
            if (actionType == NodeActionType.NEXTNODE_2) text.text = nodes[2].ID;
            if (action.IsHide) button.GetComponent<Image>().sprite = obstacle迷幻院;
            if (action.Locked)
            {
                if (action.LockType.Type == Obstacle.ObstacleType.石頭堆)
                {
                    button.GetComponent<Image>().sprite = obstacle石頭堆;
                } else if (action.LockType.Type == Obstacle.ObstacleType.牧草捆)
                {
                    button.GetComponent<Image>().sprite = obstacle牧草捆;
                } else if (action.LockType.Type == Obstacle.ObstacleType.黏液網)
                {
                    button.GetComponent<Image>().sprite = obstacle黏液網;
                } else if (action.LockType.Type == Obstacle.ObstacleType.蛛絲地)
                {
                    button.GetComponent<Image>().sprite = obstacle蛛絲地;
                } else if (action.LockType.Type == Obstacle.ObstacleType.瘴氣區)
                {
                    button.GetComponent<Image>().sprite = obstacle瘴氣區;
                } else if (action.LockType.Type == Obstacle.ObstacleType.凍土台)
                {
                    button.GetComponent<Image>().sprite = obstacle凍土台;
                } else if (action.LockType.Type == Obstacle.ObstacleType.蒸氣口)
                {
                    button.GetComponent<Image>().sprite = obstacle蒸氣口;
                } else if (action.LockType.Type == Obstacle.ObstacleType.炎岩區)
                {
                    button.GetComponent<Image>().sprite = obstacle炎岩區;
                } 
                
            }
            else button.GetComponent<Image>().sprite = normal;
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
        private void PlayStory(int isFinished, string s)
        {
            if (isFinished == 0)
            {
                story.gameObject.SetActive(true);
                _ = story.showWord(s);
            }
            else
            {
                // Destroy(story.gameObject);
                // story = Instantiate(storyPrefab);
                story.gameObject.SetActive(false);
            }
            
        }
        
    }
}
