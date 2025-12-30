using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public InputActionReference pauseActionRef;

    public GameObject controlsMenuUI;
    public GameObject pauseMenu;

    public TextMeshProUGUI score;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI objectif;
    public float totalTime = 60f;
    public int objectiveScore = 20;
    private float currentTime;
    private int currentScore = 0;
    public static bool isPaused = false;

    private void OnEnable()
    {
        if (pauseActionRef != null) pauseActionRef.action.Enable();
    }

    private void OnDisable() {
        if (pauseActionRef != null) pauseActionRef.action.Disable();
    }

    void Start()
    {
        Resume();

        objectif.text = objectiveScore.ToString();
        currentTime = totalTime;
        UpdateScore(0);
    }

    void Update()
    {
        if (pauseActionRef.action.WasPressedThisFrame())
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!isPaused)
        {
            currentTime -= Time.deltaTime;
            timer.text = "Timer: " + Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0 || currentScore >= objectiveScore)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    public void UpdateScore(int newScore)
    {
        currentScore += newScore;
        score.text = currentScore.ToString();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        controlsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Touches(
        ) { }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenControls()
    {
        pauseMenu.SetActive(false);
        controlsMenuUI.SetActive(true);
    }

    public void BackToPause()
    {
        controlsMenuUI.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
