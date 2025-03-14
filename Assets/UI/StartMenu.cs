using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    void Start()
    {
        this.startButton.onClick.AddListener(StartGame);
        this.continueButton.onClick.AddListener(ContinueGame);
        this.tutorialButton.onClick.AddListener(StartTutorial);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("WitchHouse");
    }

    private void ContinueGame()
    {

    }

    private void StartTutorial()
    {
        this.tutorial.SetActive(true);
    }
}
