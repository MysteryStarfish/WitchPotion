using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToPrepare : MonoBehaviour
{
    void Start()
    {
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Prepare");
        });
    }
}
