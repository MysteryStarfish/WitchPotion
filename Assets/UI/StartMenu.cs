using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button tutorialButton;
    [SerializeField]
    private GameObject tutorial;

    [Inject]
    private readonly Loading loading;

    void Start()
    {
        this.startButton.onClick.AddListener(StartGame);
        this.continueButton.onClick.AddListener(ContinueGame);
        this.tutorialButton.onClick.AddListener(StartTutorial);
    }

    private void StartGame()
    {
        this.loading.LoadScene("WitchHouse").Forget();
    }

    private void ContinueGame()
    {

    }

    private void StartTutorial()
    {
        this.tutorial.SetActive(true);
    }
}
