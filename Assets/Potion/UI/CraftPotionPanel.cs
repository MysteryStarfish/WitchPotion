using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftPotionPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField herbCode1;
    [SerializeField]
    private TMP_InputField herbCount1;
    [SerializeField]
    private TMP_InputField herbCode2;
    [SerializeField]
    private TMP_InputField herbCount2;
    [SerializeField]
    private TMP_InputField herbCode3;
    [SerializeField]
    private TMP_InputField herbCount3;
    [SerializeField]
    private Button craftButton;
    [SerializeField]
    private TMP_Text craftLog;

    private CraftPotion craftPotion;

    void Start()
    {
        this.craftPotion = GetComponent<CraftPotion>();
        this.craftLog.text = "";
        this.craftButton.onClick.AddListener(() =>
        {
            var herbs = new Dictionary<string, int>();
            if (!string.IsNullOrEmpty(this.herbCode1.text) && !string.IsNullOrEmpty(this.herbCount1.text))
            {
                herbs.Add(this.herbCode1.text, int.Parse(this.herbCount1.text));
            }
            if (!string.IsNullOrEmpty(this.herbCode2.text) && !string.IsNullOrEmpty(this.herbCount2.text))
            {
                herbs.Add(this.herbCode2.text, int.Parse(this.herbCount2.text));
            }
            if (!string.IsNullOrEmpty(this.herbCode3.text) && !string.IsNullOrEmpty(this.herbCount3.text))
            {
                herbs.Add(this.herbCode3.text, int.Parse(this.herbCount3.text));
            }

            var potion = this.craftPotion.Craft(herbs);
            if (potion != null)
            {
                this.craftLog.text += $"Crafted {potion.name}\n";
            }
            else
            {
                this.craftLog.text += "Failed to craft potion\n";
            }

            this.herbCode1.text = "";
            this.herbCount1.text = "";
            this.herbCode2.text = "";
            this.herbCount2.text = "";
            this.herbCode3.text = "";
            this.herbCount3.text = "";
        });
    }
}
