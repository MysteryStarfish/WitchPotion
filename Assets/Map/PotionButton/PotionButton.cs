using MessagePipe;
using NodeChange;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace Map.PotionButton
{
    public class PotionButton : MonoBehaviour
    {
        [SerializeField] private Potion type;
        [SerializeField] private int _leftAmount;
        [SerializeField] private Button _button;
        [Inject] private IPublisher<UsePotionRemoveObstacleRequest> _publisher;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                _publisher.Publish(new UsePotionRemoveObstacleRequest(type.potionName));
            });
        }
    }
}