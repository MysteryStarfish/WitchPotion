using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private Canvas loadingCanvasPrefab;

    public async UniTaskVoid LoadScene(string sceneName)
    {
        var loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        Canvas loadingCanvas = Instantiate(loadingCanvasPrefab);
        DontDestroyOnLoad(loadingCanvas.gameObject);
        CanvasGroup loadingCanvasGroup = loadingCanvas.GetComponent<CanvasGroup>();
        await LMotion.Create(0f, 1f, 0.5f).BindToAlpha(loadingCanvasGroup);
        TMP_Text loadingText = loadingCanvas.GetComponentInChildren<TMP_Text>();
        var _ = loadingTextAnimation(loadingText, loadingCanvas.GetCancellationTokenOnDestroy());

        loadOp.allowSceneActivation = true;
        await UniTask.WhenAll(
            loadOp.ToUniTask(),
            UniTask.Delay(1000)
        );

        await LMotion.Create(1f, 0f, 0.5f)
            .BindToAlpha(loadingCanvasGroup)
            .AddTo(loadingCanvas);
        Destroy(loadingCanvas.gameObject);
    }

    private async UniTask loadingTextAnimation(TMP_Text text, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            text.text = "Loading";
            await UniTask.Delay(100);
            text.text = "Loading.";
            await UniTask.Delay(100);
            text.text = "Loading..";
            await UniTask.Delay(100);
            text.text = "Loading...";
            await UniTask.Delay(100);
        }
    }
}
