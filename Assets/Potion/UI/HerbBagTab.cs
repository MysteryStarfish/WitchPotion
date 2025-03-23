using UnityEngine;
using UnityEngine.UI;

public class HerbBagTab : MonoBehaviour
{
    [SerializeField]
    private HerbBagPanel herbBagPanel;
    [SerializeField]
    private Button windButton;
    [SerializeField]
    private Button fireButton;
    [SerializeField]
    private Button waterButton;
    [SerializeField]
    private Button magicButton;
    [SerializeField]
    private Button physicButton;

    void Start()
    {
        this.herbBagPanel.filter = herb => herb.element == "風";
        this.windButton.onClick.AddListener(() =>
        {
            this.herbBagPanel.filter = herb => herb.element == "風";
        });
        this.fireButton.onClick.AddListener(() =>
        {
            this.herbBagPanel.filter = herb => herb.element == "火";
        });
        this.waterButton.onClick.AddListener(() =>
        {
            this.herbBagPanel.filter = herb => herb.element == "水";
        });
        this.magicButton.onClick.AddListener(() =>
        {
            this.herbBagPanel.filter = herb => herb.element == "魔法";
        });
        this.physicButton.onClick.AddListener(() =>
        {
            this.herbBagPanel.filter = herb => herb.element == "物理";
        });
    }
}
