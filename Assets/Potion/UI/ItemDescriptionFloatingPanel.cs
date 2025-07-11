using UnityEngine;

public class ItemDescriptionFloatingPanel : MonoBehaviour
{
    void OnEnable()
    {
        var canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene. ItemDescriptionFloatingPanel requires a Canvas to function.");
            return;
        }
        transform.SetParent(canvas.transform, true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
