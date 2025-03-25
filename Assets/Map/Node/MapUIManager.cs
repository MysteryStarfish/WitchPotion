using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MapUIController : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public GameObject labelPrefab;
    public Transform mapPanel;
    
    private Dictionary<string, GameObject> nodeObjects = new Dictionary<string, GameObject>();
    [Inject] private MapController _mapController;
    private void Start()
    {
        MapNode[][] mapNodes = _mapController.MapNodes;
        GenerateMapUI(mapNodes);
    }

    public void GenerateMapUI(MapNode[][] mapNodes)
    {
        foreach (var levelNodes in mapNodes)
        {
            foreach (var node in levelNodes)
            {
                if (node == null) continue;

                // 創建節點 (Image)
                GameObject nodeObj = Instantiate(nodePrefab, mapPanel);
                nodeObj.transform.position = GetNodePosition(node);
                nodeObjects[node.ID] = nodeObj;

                // 創建標籤 (Text)
                GameObject labelObj = Instantiate(labelPrefab, mapPanel);
                labelObj.transform.position = nodeObj.transform.position + Vector3.up * 20;
                labelObj.GetComponent<TMP_Text>().text = node.ID;

                // 連接線 (LineRenderer)
                foreach (var nextNode in node.NextNode)
                {
                    if (nextNode == null) continue;

                    GameObject lineObj = Instantiate(linePrefab, mapPanel);
                    LineRenderer lr = lineObj.GetComponent<LineRenderer>();
                    lr.SetPositions(new Vector3[]
                    {
                        nodeObj.transform.position,
                        GetNodePosition(nextNode)
                    });
                }
            }
        }
    }

    private Vector3 GetNodePosition(MapNode node)
    {
        float x = node.Level * 200;
        float y = node.Index * -400;
        return new Vector3(x, y, 0);
    }
}