using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrucibleContent : MonoBehaviour
{
    [SerializeField]
    private Button craftButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Transform herbListContainer;
    [SerializeField]
    private GameObject herbListItemPrefab;

    void Start()
    {
        this.closeButton.onClick.AddListener(() => Destroy(gameObject));
    }

    public GameObject AddHerb(Sprite herbIcon, int count)
    {
        var herbListItem = Instantiate(this.herbListItemPrefab, this.herbListContainer);
        herbListItem.GetComponentInChildren<Image>().sprite = herbIcon;
        herbListItem.GetComponentInChildren<TMPro.TMP_Text>().text = $"x{count}";
        return herbListItem;
    }

    public UnityEvent OnCraftButtonClicked => this.craftButton.onClick;
}
