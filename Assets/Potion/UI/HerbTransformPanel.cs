using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HerbTransformPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField targetCode;
    [SerializeField]
    private TMP_InputField count;
    [SerializeField]
    private Button upcastButton;
    [SerializeField]
    private Button downcastButton;
    [SerializeField]
    private TMP_Text transformLog;

    private HerbTransformer herbTransformer;

    void Start()
    {
        this.herbTransformer = GetComponent<HerbTransformer>();
        this.transformLog.text = "";
        this.upcastButton.onClick.AddListener(() =>
        {
            try
            {
                this.herbTransformer.upcast(this.targetCode.text, int.Parse(this.count.text));
                this.transformLog.text += $"Upcasted {this.targetCode.text}\n";
            }
            catch (System.Exception e)
            {
                this.transformLog.text += e.Message + "\n";
            }
        });
        this.downcastButton.onClick.AddListener(() =>
        {
            try
            {
                this.herbTransformer.downcast(this.targetCode.text, int.Parse(this.count.text));
                this.transformLog.text += $"Downcasted {this.targetCode.text}\n";
            }
            catch (System.Exception e)
            {
                this.transformLog.text += e.Message + "\n";
            }
        });
    }
}
